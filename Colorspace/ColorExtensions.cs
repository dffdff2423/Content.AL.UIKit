// // This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// // If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Robust.Shared.Utility;

namespace Content.AL.UIKit.Colorspace;

[PublicAPI]
public static class ColorExtensions
{
    /// <summary>
    ///     Adjusts the lightness of a color using the Oklab color space.
    /// </summary>
    public static Color WithLightness(this Color c, float lightness)
    {
        DebugTools.Assert(lightness >= 0.0f && lightness <= 1.0f);
        
        var o = new OklabColor(c)
        {
            L = lightness
        };
        
        return (Color) o;
    }
    
    /// <summary>
    ///     Adjusts the hue of a color using the Oklab color space.
    /// </summary>
    public static Color WithHue(this Color c, Angle hue)
    {
        var o = new OklabColor(c)
        {
            H = hue,
        };
        
        return (Color) o;
    }
    
    /// <summary>
    ///     Adjusts the hue of a color using the Oklab color space.
    /// </summary>
    public static Color WithChroma(this Color c, float chroma)
    {
        var o = new OklabColor(c)
        {
            C = chroma,
        };
        
        return (Color) o;
    }
}