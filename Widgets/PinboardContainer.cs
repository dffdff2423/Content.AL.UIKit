﻿// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Numerics;
using Content.AL.UIKit.Annotations;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Animations;
using Robust.Shared.Input;

namespace Content.AL.UIKit.Widgets;

[PublicAPI]
[Virtual]
public class PinboardContainer : Container
{
    public static readonly AttachedProperty<Vector2> PinLocation = AttachedProperty<Vector2>.Create("PinLocation", typeof(PinboardContainer), defaultValue: Vector2.Zero);

    // XAMLIL magic, these are used to recognize attachedprops from XAML. It will ICE if you remove one without the other, as well. :)
    [UsedByXAMLIL]
    public static Vector2 GetPinLocation(Control c)
        => c.GetValue(PinLocation);
    [UsedByXAMLIL]
    public static void SetPinLocation(Control c, Vector2 loc)
    {
        c.SetValue(PinLocation, loc);
        if (c is IPinboardAware aware)
            aware.PositionUpdated(loc);

        c.Parent?.InvalidateArrange();
    }

    public Vector2 ScrollLocation = Vector2.Zero;
    [ViewVariables(VVAccess.ReadWrite)]
    [Animatable]
    public float Zoom { get; set; }= 1.0f;
    public Vector2 ChildAvailableSpace { get; set; } = new(1024, 1024);

    public bool DoCursorScroll = true;

    private bool _dragging = false;

    protected override void KeyBindDown(GUIBoundKeyEventArgs args)
    {
        base.KeyBindDown(args);

        if (args.Function != EngineKeyFunctions.UIClick)
        {
            return;
        }

        _dragging = true;
    }

    protected override void KeyBindUp(GUIBoundKeyEventArgs args)
    {
        base.KeyBindDown(args);

        if (args.Function != EngineKeyFunctions.UIClick)
        {
            return;
        }

        _dragging = false;
    }

    protected override void MouseMove(GUIMouseMoveEventArgs args)
    {
        base.MouseMove(args);

        if (!_dragging || !DoCursorScroll)
            return;

        ScrollLocation += args.Relative;
        InvalidateArrange();
    }

    public PinboardContainer()
    {
        RectClipContent = true;
        MouseFilter = MouseFilterMode.Stop;
        //PlayAnimation(Zoomies, "glonk");
    }



    protected override Vector2 MeasureOverride(Vector2 availableSize)
    {
        foreach (var c in Children)
        {
            c.Measure(ChildAvailableSpace);
        }

        return MinSize;
    }

    protected override Vector2 ArrangeOverride(Vector2 finalSize)
    {
        foreach (var c in Children)
        {
            var desired = c.DesiredSize;
            var pos = c.GetValue(PinLocation);
            c.Arrange(UIBox2.FromDimensions(pos + ScrollLocation, desired));
        }

        return finalSize;
    }

    protected override void RenderChildOverride(ref ControlRenderArguments args, int childIndex, Vector2i pos)
    {
        var screen = args.Handle.DrawingHandleScreen;
        var oldXform = screen.GetTransform();
        var xform = oldXform;
        var scale = Matrix3x2.CreateScale(Zoom, Zoom);
        xform *= scale;
        var posOffs = GlobalPixelPosition + (PixelSize / 2);
        var relPos = pos - posOffs;
        pos = (Vector2i)Vector2.Transform(relPos, scale) + posOffs;
        screen.SetTransform(xform);
        base.RenderChildOverride(ref args, childIndex, pos);
        screen.SetTransform(oldXform);
    }
}
