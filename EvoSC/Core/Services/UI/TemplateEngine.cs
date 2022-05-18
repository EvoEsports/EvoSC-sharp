#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using EvoSC.Core.Configuration;
using Scriban;
using Scriban.Runtime;
using NLog;
using Scriban.Parsing;

namespace EvoSC.Core.Services.UI;

public class Colors
{
    public string accent { get; set; } = "";
    public string accent_dark { get; set; } = "";
    public string accent_bg { get; set; } = "";
    public string accent_light { get; set; } = "";
    public string background { get; set; } = "";
    public string text { get; set; } = "";
}

public class TemplateEngine
{
    private const bool PreserveWhiteSpace = false; // false for formatted xml string
    private const bool Validate = true; // enable validation of the output xmlDocument
    private static readonly Logger s_logger = LogManager.GetCurrentClassLogger();
    private readonly string _basePath;
    private string _errors;
    private readonly Dictionary<string, XmlDocument> _components = new();
    private readonly Dictionary<string, string> _scripts = new();
    private readonly XmlDocument _doc;
    private readonly ScriptObject _colors;

    public TemplateEngine(string path, string file)
    {
        _basePath = path;
        _errors = "";
        var theme = Config.GetTheme();
    
        var colors = new Colors()
        {
            accent = theme.Accent,
            accent_dark = theme.Darken(theme.Accent, 30),
            accent_light = theme.Lighten(theme.Accent, 30),
            accent_bg = theme.Dark(theme.Accent),
            background = theme.Background(theme.Accent),
            text = "f0f0f0"
        };
        _colors = new ScriptObject();
        _colors.Import(colors);
        
        _doc = PreProcess(path + "/" + file);
        ProcessScripts(_doc, "__main__");

        var scriptNode = _doc.CreateElement("script");
        var scripts = _doc.CreateCDataSection(File.ReadAllText(@"templates/scripts/initScript.Script.txt"));
        foreach (var tagName in _components)
        {
            scripts.Data += _scripts[tagName.Key];
        }

        scripts.Value += _scripts["__main__"];
        scripts.Value += File.ReadAllText(@"templates/scripts/mainScript.Script.txt");
        scripts.Value = scripts.Value.Replace("«««", "{{{").Replace("»»»", "}}}");
        scriptNode.AppendChild(scripts);
        _doc.DocumentElement?.AppendChild(scriptNode);
    }

    private void XmlValidatorErrorHandler(object? sender, ValidationEventArgs args)
    {
        switch (args.Severity)
        {
            case XmlSeverityType.Error:
                s_logger.Error(args.Message);
                _errors += args.Message + "\n";
                break;
            case XmlSeverityType.Warning:
                s_logger.Warn(args.Message);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ProcessScripts(XmlDocument doc, string component)
    {
        var text = "";
        var scripts = doc.SelectNodes("//script/comment()|//script/text()");
        if (scripts != null)
            foreach (dynamic node in scripts)
            {
                text += node.Value?.Replace("{{{", "«««").Replace("}}}", "»»»");
                node.ParentNode?.ParentNode?.RemoveChild(node.ParentNode);
            }

        _scripts.Add(component, text);
    }

    private string RemoveWhitespace(string? xml)
    {
        if (xml == null) return "";
        if (PreserveWhiteSpace) return xml;

        var regex = new Regex(@">\s*<");
        xml = regex.Replace(xml, "><");
        return xml.Trim();
    }

    private XmlDocument LoadDoc(string file, string component)
    {
        var doc = new XmlDocument {PreserveWhitespace = false};
        doc.Load(file);
        var instructions = doc.SelectNodes("//processing-instruction()");
        if (instructions != null)
            foreach (XmlProcessingInstruction instruction in instructions)
                doc.RemoveChild(instruction);

        ProcessScripts(doc, component);
        return doc;
    }

    private XmlDocument PreProcess(string file)
    {
        var doc = new XmlDocument {PreserveWhitespace = false};
        doc.Load(file);
        var instructions = doc.SelectNodes("//processing-instruction()");
        if (instructions == null) return doc;
        foreach (XmlProcessingInstruction instruction in instructions)
        {
            if (instruction.Target != "component") continue;
            var tagRegex = new Regex("tag=\"(.*?)\"");
            var tagMatch = tagRegex.Match(instruction.Value);
            var srcRegex = new Regex("src=\"(.*?)\"");
            var srcMatch = srcRegex.Match(instruction.Value);
            if (tagMatch.Groups.Count == 2 && srcMatch.Groups.Count == 2)
            {
                if (!_components.ContainsKey(tagMatch.Groups[1].Value))
                {
                    _components.Add(tagMatch.Groups[1].Value,
                        LoadDoc(_basePath + "/" + srcMatch.Groups[1].Value, tagMatch.Groups[1].Value));
                    PreProcess(_basePath + "/" + srcMatch.Groups[1].Value);
                }
            }

            doc.RemoveChild(instruction);
        }

        return doc;
    }

    public string Render(dynamic obj)
    {
        var context = new TemplateContext() {AutoIndent = true, NewLine = "\n",};
        var scriptObject = new ScriptObject();
        scriptObject.Import((object)obj);
        scriptObject.Add("colors", _colors);
        
        if (obj.GetType().GetProperty("size") != null)
        {
            scriptObject.Add("__size__", obj.size);
        }

        context.PushGlobal(scriptObject);

        var template = Template.Parse(_doc.OuterXml);
        if (template.HasErrors)
        {
            foreach (var message in template.Messages)
            {
                switch (message.Type)
                {
                    case ParserMessageType.Error:
                        s_logger.Error(message.Message);
                        break;
                    case ParserMessageType.Warning:
                        s_logger.Warn(message.Message);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        var doc = new XmlDocument();
        doc.LoadXml(template.Render(context));

        foreach (var tagName in _components)
        {
            var nodes = doc.SelectNodes("//" + tagName.Key);
            if (nodes == null) continue;
            foreach (XmlNode node in nodes)
            {
                if (node == null) continue;
                var ctx = new TemplateContext();
                var sObj = new ScriptObject();
                sObj.Add("colors", _colors);
                if (node.Attributes != null)
                    foreach (XmlAttribute attrib in node.Attributes)
                    {
                        sObj.Add(attrib.Name, attrib.Value);
                    }

                if (node.HasChildNodes)
                {
                    sObj.Add("__super__", node.InnerXml);
                }

                if (sObj.GetType().GetProperty("size") != null)
                {
                    sObj.Add("__size__", obj.size);
                }
                
                ctx.PushGlobal(sObj);
                var nod = doc.CreateDocumentFragment();
                var newText = Template.Parse(tagName.Value.DocumentElement?.InnerXml).Render(ctx);
                Console.WriteLine(newText);
                nod.InnerXml = newText.Trim();

                if (nod.FirstChild != null) node.ParentNode?.ReplaceChild(nod.FirstChild, node);
            }
        }

        if (Validate)
        {
            _errors = "";
            var validator = new ValidationEventHandler(XmlValidatorErrorHandler);
            doc.Schemas.Add("", Path.Join(@"templates/manialink_v3.xsd"));
            doc.Validate(validator);
            if (_errors != "")
            {
                //  throw new Exception("XML Validation errors:\n"+_errors);
            }
        }

        return RemoveWhitespace(doc.OuterXml);
    }
}
