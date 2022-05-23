using System.Collections.Generic;
using EvoSC.Core.Configuration;
using EvoSC.Core.Helpers;

namespace EvoSC.Core.Helpers
{
    public class ChatMessage
    {
        private string _message { get; set; }

        private string _color { get; set; }

        private string _icon { get; set; }

        private static Theme _theme;

        public ChatMessage()
        {
            _theme = Config.GetTheme();
        }

        public static string GetHighlightedString(string txt)
        {
            return $"$<$fff{_theme.Highlight}{txt}$>";
        }

        public void SetMessage(string message)
        {
            _message = message;
        }

        /// <summary>
        /// Sets the icon that should be displayed in front of the message.
        /// <para>Use this with an icon from <see cref="EvoSC.Core.Helpers.Icon"/></para>
        /// </summary>
        /// <example>SetIcon(Icon.Globe)</example>
        /// <param name="icon"></param>
        public void SetIcon(string icon)
        {
            _icon = icon;
        }

        public void SetDanger()
        {
            _color = _theme.Danger;
        }

        public void SetDefault()
        {
            _color = _theme.Default;
        }

        public void SetInfo()
        {
            _color = _theme.Info;
        }

        public void SetWarning()
        {
            _color = _theme.Warning;
        }

        public void SetSuccess()
        {
            _color = _theme.Success;
        }

        public string Render()
        {
            var message = (_icon != string.Empty ? $"$fff{_icon} " : string.Empty) + $"{_color}{_message}";
            return message;
        }
    }
}
