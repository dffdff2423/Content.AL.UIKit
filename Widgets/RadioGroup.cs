// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Content.AL.UIKit.Interfaces;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;

namespace Content.AL.UIKit.Widgets;

[PublicAPI]
public sealed class RadioGroup : Control, IGroupOrganizer
{
    public Dictionary<string, ButtonGroup> Groups { get; } = new();
}
