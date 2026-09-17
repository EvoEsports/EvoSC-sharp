namespace EvoSC.Manialinks.Vdom.Vdom;

/// <summary>
/// The physical control kind a <see cref="VNode"/> ultimately maps to. <see cref="Frame"/> is
/// special: with <see cref="VContainerNeed.None"/> it is a pure grouping/transform node that
/// never consumes a pool slot (see <c>Mount/LayoutFlattener.cs</c>); it only becomes a real
/// pooled control when it needs clipping or scrolling, which M1 does not use (see M2 in the
/// plan).
///
/// Numeric values are part of the wire contract with <c>Runtime/VdomRuntime.ms</c> -- see the
/// pinning note on <see cref="VPropId"/>, which applies here too.
/// </summary>
public enum VKind
{
    Quad = 0,
    Label = 1,
    Entry = 2,
    Frame = 3
}

/// <summary>
/// Whether a <see cref="VKind.Frame"/> node needs a real pooled frame control. <c>None</c> means
/// the frame is purely a virtual grouping/transform node erased during layout flattening -- see
/// <c>Mount/LayoutFlattener.cs</c>. <c>Clip</c> and <c>Scroll</c> are M2 (deferred: LiveRanking
/// needs neither).
/// </summary>
public enum VContainerNeed
{
    None,
    Clip,
    Scroll
}

public enum VAlignHorizontal
{
    Left,
    Center,
    Right
}

public enum VAlignVertical
{
    Top,
    Center,
    Bottom
}

/// <summary>
/// A 2D offset/position in manialink grid units. Matches <c>RelativePosition_V3</c>, which
/// (despite the name) is a <c>Vec2</c> on the game side.
/// </summary>
public readonly record struct VVec2(double X, double Y)
{
    public static readonly VVec2 Zero = new(0, 0);

    public static VVec2 operator +(VVec2 a, VVec2 b) => new(a.X + b.X, a.Y + b.Y);

    public static VVec2 operator *(VVec2 v, double scalar) => new(v.X * scalar, v.Y * scalar);

    /// <summary>
    /// Rotates this vector by <paramref name="degrees"/> around the origin, matching
    /// ManiaScript's rotation convention (positive = counter-clockwise).
    /// </summary>
    public VVec2 Rotate(double degrees)
    {
        if (degrees == 0)
        {
            return this;
        }

        var radians = degrees * Math.PI / 180.0;
        var cos = Math.Cos(radians);
        var sin = Math.Sin(radians);

        return new VVec2(X * cos - Y * sin, X * sin + Y * cos);
    }

    public override string ToString() => $"{X:0.###} {Y:0.###}";
}

/// <summary>
/// An RGB color, each channel in [0, 1], matching the <c>Vec3</c> ManiaScript expects for
/// <c>BgColor</c> / <c>TextColor</c> / <c>Colorize</c> / <c>ModulateColor</c>.
/// </summary>
public readonly record struct VColor(double R, double G, double B)
{
    /// <summary>
    /// Parses a 3- or 6-digit hex color, with or without a leading '#', matching the shorthand
    /// used throughout the existing manialink XML (e.g. "0AF", "FFCC00").
    /// </summary>
    public static VColor FromHex(string hex)
    {
        ArgumentNullException.ThrowIfNull(hex);

        var span = hex.AsSpan().TrimStart('#');

        return span.Length switch
        {
            3 => new VColor(
                HexNibble(span[0]) / 15.0,
                HexNibble(span[1]) / 15.0,
                HexNibble(span[2]) / 15.0),
            6 => new VColor(
                HexByte(span[0], span[1]) / 255.0,
                HexByte(span[2], span[3]) / 255.0,
                HexByte(span[4], span[5]) / 255.0),
            _ => throw new ArgumentException(
                $"Expected a 3- or 6-digit hex color, got '{hex}'.", nameof(hex))
        };
    }

    private static int HexNibble(char c) => Convert.ToInt32(c.ToString(), 16);

    private static int HexByte(char hi, char lo) => Convert.ToInt32($"{hi}{lo}", 16);

    /// <summary>
    /// Renders as a 6-digit hex string with no leading '#' -- the format mount-time XML
    /// attributes use throughout the existing manialink templates (e.g. <c>bgcolor="0AF"</c>).
    /// Not the same format as <see cref="ToString"/>, which the runtime patch wire uses instead.
    /// </summary>
    public string ToHex() =>
        $"{ToHexByte(R)}{ToHexByte(G)}{ToHexByte(B)}";

    private static string ToHexByte(double channel) =>
        Math.Clamp((int)Math.Round(channel * 255.0), 0, 255).ToString("X2");

    /// <summary>Renders as "r g b" reals in [0, 1] -- the wire format the client parses.</summary>
    public override string ToString() => $"{R:0.###} {G:0.###} {B:0.###}";
}
