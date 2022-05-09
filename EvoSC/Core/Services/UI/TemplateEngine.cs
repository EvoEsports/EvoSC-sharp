using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using Scriban;
using Scriban.Runtime;
using NLog;

namespace EvoSC.Core.Services.UI;

public class TemplateEngine
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private const bool PreserveWhiteSpace = false; // false for formatted xml string
    private readonly string _basePath;
    private static int s_errors;
    private readonly string _baseFile;
    private readonly Dictionary<string, XmlDocument> _components = new();
    private readonly Dictionary<string, string> s_scripts = new();
    private readonly XmlDocument _doc;
    public TemplateEngine(string path, string file)
    {
        _basePath = path;
        _baseFile = file;
        s_errors = 0;
        _doc = PreProcess(path + "/" + file);
        ProcessScripts(_doc, "__main__");
    }

    private static void XmlValidatorErrorHandler(object? sender, ValidationEventArgs args)
    {
        switch (args.Severity)
        {
            case XmlSeverityType.Error:
                s_errors += 1;
                _logger.Error(args.Message);
                break;
            case XmlSeverityType.Warning:
                _logger.Warn(args.Message);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ProcessScripts(XmlDocument doc, string component)
    {
        var text = "";
        var scripts = doc.SelectNodes("//script/comment()");
        if (scripts != null)
            foreach (XmlComment node in scripts)
            {
                text += node.Value?.Replace("{{{", "«««").Replace("}}}", "»»»");
                node.ParentNode?.ParentNode?.RemoveChild(node.ParentNode);
            }

        s_scripts.Add(component, text);
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
        var doc = new XmlDocument
        {
            PreserveWhitespace = false
        };
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
        var doc = new XmlDocument
        {
            PreserveWhitespace = false
        };
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

    private string Process(TemplateContext ctx)
    {
        var regex = new Regex("{{\\s*size\\s*}}");
        var text = regex.Replace(_doc.OuterXml, "{{__size__}}");
        _doc.InnerXml = Template.Parse(text).Render(ctx);

        var scriptNode = _doc.CreateElement("script");
        var scripts = _doc.CreateCDataSection(File.ReadAllText(@"templates/scripts/initScript.Script.txt"));

        foreach (var tagName in _components)
        {
            var nodes = _doc.SelectNodes("//" + tagName.Key);
            scripts.Data += s_scripts[tagName.Key];
            if (nodes == null) continue;
            foreach (XmlNode node in nodes)
            {
                if (node == null) continue;
                var context = new TemplateContext();
                var obj = new ScriptObject();
                if (node.Attributes != null)
                    foreach (XmlAttribute attrib in node.Attributes)
                    {
                        obj.Add(attrib.Name, attrib.Value);
                    }

                if (node.HasChildNodes)
                {
                    obj.Add("__super__", node.InnerXml);
                }

                context.PushGlobal(obj);
                var nod = _doc.CreateDocumentFragment();
                var newText = Template.Parse(tagName.Value.DocumentElement?.InnerXml).Render(context);
                nod.InnerXml = newText.Trim();

                if (nod.FirstChild != null) node.ParentNode?.ReplaceChild(nod.FirstChild, node);
            }
        }

        scripts.Value += s_scripts["__main__"];
        scripts.Value += File.ReadAllText(@"templates/scripts/mainScript.Script.txt");
        scripts.Value = Template.Parse(scripts.Value).Render(ctx).Replace("«««", "{{{").Replace("»»»", "}}}");
        scriptNode.AppendChild(scripts);
        _doc.DocumentElement?.AppendChild(scriptNode);
        s_errors = 0;
        var validator = new ValidationEventHandler(XmlValidatorErrorHandler);
        _doc.Schemas.Add("", Path.Join(@"templates/manialink_v3.xsd"));
        _doc.Validate(validator);
        if (s_errors > 0)
        {
            //  throw new Exception();
        }

        return _doc.OuterXml;
    }

    
    public string Render(dynamic obj)
    {
        var context = new TemplateContext()
        {
            AutoIndent = true,
            NewLine = "\n",
        };
        var scriptObject = new ScriptObject();
        scriptObject.Import((object)obj);
        if (obj.GetType().GetProperty("size") != null) {
            scriptObject.Add("__size__", obj.size);
        }
        context.PushGlobal(scriptObject);

        return RemoveWhitespace(Process(context));
    }
}
