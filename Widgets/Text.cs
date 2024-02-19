// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Robust.Client.UserInterface.Controls;

namespace Content.AL.UIKit.Widgets;

[PublicAPI]
[Virtual]
public class Text : Label
{
    public Text()
    {
        Margin = new Thickness(2);
    }
}
