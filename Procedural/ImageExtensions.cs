// // This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// // If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Robust.Client.Utility;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Color = Robust.Shared.Maths.Color;

namespace Content.AL.UIKit.Procedural;

public static class ImageExtensions
{
    public static void ApplyGenerator(this Image<Rgba32> img, Func<int, int, Image<Rgba32>, Color> gen)
    {
        for (var i = 0; i < img.Width; i++)
        {
            for (var j = 0; j < img.Height; j++)
            {
                img[i, j] = gen(i, j, img).ConvertImgSharp();
            }            
        }
    }
}