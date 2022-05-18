using System;
using System.Drawing;
using ColorHelper;
using Tomlet.Attributes;
using ColorConverter = ColorHelper.ColorConverter;

namespace EvoSC.Core.Configuration
{
    public class Theme
    {
        [TomlProperty("theme.default")] public string Default { get; set; }
        [TomlProperty("theme.info")] public string Info { get; set; }
        [TomlProperty("theme.highlight")] public string Highlight { get; set; }
        [TomlProperty("theme.success")] public string Success { get; set; }
        [TomlProperty("theme.warning")] public string Warning { get; set; }
        [TomlProperty("theme.danger")] public string Danger { get; set; }
        [TomlProperty("theme.accent")] public string Accent { get; set; }


        public string Darken(string hex6, ushort amount)
        {
            var hsv = ColorConverter.HexToHsl(new HEX("#" + hex6));
            int newVal = hsv.L - amount;
            if (newVal <= 0) newVal = 0;
            hsv.L = Convert.ToByte(newVal);
            return ColorConverter.HslToHex(hsv).ToString().Replace("#", "");
        }
        
        public string Lighten(string hex6, ushort amount)
        {
            var hsv = ColorConverter.HexToHsl(new HEX("#" + hex6));
            int newVal = hsv.L + amount;
            if (newVal >= 255) newVal = 255;
            int newSat = hsv.S + amount/2;
            if (newSat >= 255) newSat = 255;
            hsv.L = Convert.ToByte(newVal);
            hsv.S = Convert.ToByte(newSat);
            return ColorConverter.HslToHex(hsv).ToString().Replace("#", "");
        }
        public string Dark(string hex6)
        {
            var hsl = ColorConverter.HexToHsl(new HEX("#" + hex6));
            hsl.L = Convert.ToByte(20);
            return ColorConverter.HslToHex(hsl).ToString().Replace("#", "");
        }
        public string Background(string hex6)
        {
            var hsl = ColorConverter.HexToHsl(new HEX("#" + hex6));
            hsl.L = Convert.ToByte(5);
            return ColorConverter.HslToHex(hsl).ToString().Replace("#", "");
        }
        
        
        public string Dim(string hex6, ushort amount)
        {
            var hsv = ColorConverter.HexToHsl(new HEX("#" + hex6));
            int newVal = hsv.L - amount;
            if (newVal <= 0) newVal = 0;
            int newSat = hsv.S - amount;
            if (newSat <= 0) newSat = 0;
            hsv.L = Convert.ToByte(newVal);
            hsv.S = Convert.ToByte(newSat);
            return ColorConverter.HslToHex(hsv).ToString().Replace("#", "");
        }
        public string Light(string hex6)
        {
            var hsl = ColorConverter.HexToHsl(new HEX("#" + hex6));
            hsl.L += Convert.ToByte(20);
            hsl.S += Convert.ToByte(20);
            return ColorConverter.HslToHex(hsl).ToString().Replace("#", "");
        }
        
    }
}
