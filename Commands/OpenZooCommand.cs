// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Content.AL.UIKit.Windows;
using Robust.Shared.Console;

namespace Content.AL.UIKit.Commands;

internal sealed class OpenZooCommand : LocalizedCommands
{
    public override string Command => "openzoo";

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        var zoo = new ALZoo();
        zoo.OpenCentered();
    }
}