// // This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// // If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Numerics;
using Vector4 = Robust.Shared.Maths.Vector4;

namespace Content.AL.UIKit.Colorspace;

/// <summary>
///     Oklab is an alternate color space that more accurately imitates how color actually behaves.
///     Useful if you want to adjust the lightness/saturation of a color without potentially altering hue.
///     https://bottosson.github.io/posts/oklab/
///     Oklch hue and chroma are also provided.
/// </summary>
[PublicAPI]
public struct OklabColor
{
    private Vector4 _color;
    
    /// <summary>
    ///     Lightness/saturation of the value.
    /// </summary>
    public float L
    {
        get => _color.X;
        set => _color.X = value;
    }
    
    /// <summary>
    ///     The A value in Oklab.
    /// </summary>
    public float A
    {
        get => _color.Y;
        set => _color.Y = value;
    }
    
    /// <summary>
    ///     The B value in Oklab.
    /// </summary>
    public float B
    {
        get => _color.Z;
        set => _color.Z = value;
    }

    /// <summary>
    ///     Transparency.
    /// </summary>
    public float Alpha
    {
        get => _color.W;
        set => _color.W = value;
    }


    /// <summary>
    ///     Oklch hue.
    /// </summary>
    /// <remarks>This may be NaN or have unpredictable values when length((a,b)) is near zero! Use HNorm to normalize it to zero.</remarks>
    public Angle H
    {
        get => float.Atan2(A, B);
        set => SetHueChroma(value, C);
    }

    /// <summary>
    ///     Oklch hue, normalizing to zero when hue approaches unusable values.
    /// </summary>
    public Angle HNorm
    {
        get
        {
            if (C < HueDistEpsilon)
                return 0;

            return H;
        }
    }
    
    /// <summary>
    ///     Oklch chroma. You probably shouldn't set this above 0.4.
    /// </summary>
    public float C
    {
        get => new Vector2(A, B).Length();
        set => SetHueChroma(H, value);
    }
    
    /// <summary>
    ///     Value below which hue is deemed to be "missing" and as such A = B = 0 (black/white).
    /// </summary>
    public const float HueEpsilon = 0.0001f;
    /// <summary>
    ///     Value below which hue is deemed to be "missing" and as such HNorm returns 0.
    /// </summary>
    public const float HueDistEpsilon = 0.0001f;

    /// <summary>
    ///     Sets the hue and chroma together. They're mutually dependent variables so this results in less imprecision.
    /// </summary>
    public void SetHueChroma(Angle h, float c)
    {
        A = c * float.Cos((float)h.Theta);
        B = c * float.Sin((float)h.Theta);
    }
    
    public OklabColor(Color c)
    {
        // I won't pretend to know how this works.
        // https://bottosson.github.io/posts/oklab/
        var l = 0.4122214708f * c.R + 0.5363325363f * c.G + 0.0514459929f * c.B;
        var m = 0.2119034982f * c.R + 0.6806995451f * c.G + 0.1073969566f * c.B;
        var s = 0.0883024619f * c.R + 0.2817188376f * c.G + 0.6299787005f * c.B;

        var l_ = float.Cbrt(l);
        var m_ = float.Cbrt(m);
        var s_ = float.Cbrt(s);

        L = 0.2104542553f * l_ + 0.7936177850f * m_ - 0.0040720468f * s_;
        A = 1.9779984951f * l_ - 2.4285922050f * m_ + 0.4505937099f * s_;
        B = 0.0259040371f * l_ + 0.7827717662f * m_ - 0.8086757660f * s_;
        Alpha = c.A;
    }

    public static explicit operator Color(OklabColor c)
    {
        var l_ = c.L + 0.3963377774f * c.A + 0.2158037573f * c.B;
        var m_ = c.L - 0.1055613458f * c.A - 0.0638541728f * c.B;
        var s_ = c.L - 0.0894841775f * c.A - 1.2914855480f * c.B;

        var l = l_*l_*l_;
        var m = m_*m_*m_;
        var s = s_*s_*s_;

        return new(
            +4.0767416621f * l - 3.3077115913f * m + 0.2309699292f * s,
            -1.2684380046f * l + 2.6097574011f * m - 0.3413193965f * s,
            -0.0041960863f * l - 0.7034186147f * m + 1.7076147010f * s,
            c.Alpha
        );
    }
}
