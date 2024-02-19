// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.AL.UIKit.ALStylesheetHelpers;

namespace Content.AL.UIKit.Sheets;

[Stylesheet]
public sealed class Buttons : BaseSubsheet
{
    public override StyleRule[] GetRules(BaseStyle origin)
    {
        return new StyleRule[]
        {
            Button().Normal().Prop(ALStyleConsts.StyleBox, origin.ButtonBackgrounds[2]),
            Button().Hover().Prop(ALStyleConsts.StyleBox, origin.ButtonBackgrounds[3]),
            Button().Pressed().Prop(ALStyleConsts.StyleBox, origin.ButtonBackgrounds[1]),
            Button().Disabled().Prop(ALStyleConsts.StyleBox, origin.ButtonBackgrounds[0]),

            Button().Positive().Normal().Prop(ALStyleConsts.StyleBox, origin.ButtonPositiveBackgrounds[2]),
            Button().Positive().Hover().Prop(ALStyleConsts.StyleBox, origin.ButtonPositiveBackgrounds[3]),
            Button().Positive().Pressed().Prop(ALStyleConsts.StyleBox, origin.ButtonPositiveBackgrounds[1]),
            Button().Positive().Disabled().Prop(ALStyleConsts.StyleBox, origin.ButtonPositiveBackgrounds[0]),

            Button().Negative().Normal().Prop(ALStyleConsts.StyleBox, origin.ButtonNegativeBackgrounds[2]),
            Button().Negative().Hover().Prop(ALStyleConsts.StyleBox, origin.ButtonNegativeBackgrounds[3]),
            Button().Negative().Pressed().Prop(ALStyleConsts.StyleBox, origin.ButtonNegativeBackgrounds[1]),
            Button().Negative().Disabled().Prop(ALStyleConsts.StyleBox, origin.ButtonNegativeBackgrounds[0]),
            E<Popup>().Class(OptionButton.StyleClassPopup).ParentOf(E<PanelContainer>())
                .Prop(PanelContainer.StylePropertyPanel, origin.PanelBackgrounds[0])
        };
    }
}
