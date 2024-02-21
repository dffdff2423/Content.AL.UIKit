// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Robust.Client.Animations;
using Robust.Client.UserInterface;

namespace Content.AL.UIKit.Animation;

[PublicAPI]
public sealed class AnimationTrackAttachedProperty : AnimationTrackProperty
{
    public AttachedProperty? Property { get; set; }
    
    protected override void ApplyProperty(object context, object value)
    {
        if (context is not Control c)
            throw new ArgumentException("Context must be a control.");

        if (Property is null)
            throw new ArgumentException("Must have a target property!");
        
        c.SetValue(Property, value);
    }
}