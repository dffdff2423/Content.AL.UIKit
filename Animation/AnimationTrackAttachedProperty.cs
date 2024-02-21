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