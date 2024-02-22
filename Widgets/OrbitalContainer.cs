// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Linq;
using System.Numerics;
using Content.AL.UIKit.Annotations;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Animations;

namespace Content.AL.UIKit.Widgets;

/// <summary>
///     A container that places its children into an orbit around its center at either manually provided angles or automatically calculated ones.
///     This can be used to implement radial menus, among other things.
/// </summary>
[Virtual]
[PublicAPI]
public class OrbitalContainer : Container
{
    /// <summary>
    /// The angle of a given satellite in an OrbitalContainer.
    /// </summary>
    public static readonly AttachedProperty<Angle> SatelliteAngle = AttachedProperty<Angle>.Create("SatelliteAngle", typeof(OrbitalContainer), changed: Changed);

    /// <summary>
    /// For use in generated code by XAMLIL. Not for direct end user use, please use GetValue.
    /// </summary>
    [UsedByXAMLIL]
    public static Angle GetSatelliteAngle(Control c)
        => c.GetValue(SatelliteAngle);
    
    /// <summary>
    /// For use in generated code by XAMLIL. Not for direct end user use, please use SetValue.
    /// </summary>
    [UsedByXAMLIL]
    public static void SetSatelliteAngle(Control c, Angle loc)
        => c.SetValue(SatelliteAngle, loc);
    
    private static void Changed(Control c, AttachedPropertyChangedEventArgs<Angle> _)
    {
        if (c.Parent is not OrbitalContainer orbit)
        {
            return; // We can't really do anything with this yet.
        }
        
        orbit.OnSatelliteUpdated(c);
    }

    /// <summary>
    ///     How far children orbit from the center of the control.
    /// </summary>
    [Animatable]
    public float OrbitDistance { get; set; }
    /// <summary>
    ///     The placement used when arranging children, if any.
    /// </summary>
    public SatellitePlacementMode PlacementMode { get; set; }

    /// <summary>
    /// Whether or not the orbit should be the control's center or its corner.
    /// </summary>
    /// <remarks> Useful for placing radials directly on the screen, as it means you don't need to figure out their measured center.</remarks>
    public bool CenterToControl { get; set; } = true;

    /// <summary>
    ///     The angle of the top of the control, as used by StackedClockwise and StackedCounterClockwise
    /// </summary>
    public Angle Top { get; set; } = Angle.FromWorldVec(-Vector2.UnitX);

    public Color? DebugCircleColor { get; set; } = null;
    
    public enum SatellitePlacementMode
    {
        /// <summary>
        ///     Invalid placement mode.
        /// </summary>
        Invalid = 0,
        /// <summary>
        ///     Manual placement, end user is required to set the angle.
        /// </summary>
        Manual,
        /// <summary>
        ///     Places all children with even spacing.
        /// </summary>
        Even,
        /// <summary>
        ///     Stacks all children from the top, clockwise.
        /// </summary>
        StackedClockwise,
        /// <summary>
        ///     Stacks all children from the top, counterclockwise.
        /// </summary>
        StackedCounterClockwise,
    }

    protected override void ChildAdded(Control newChild)
    {
        RecalculateSatelliteAngles();
    }

    protected override void ChildMoved(Control child, int oldIndex, int newIndex)
    {
        RecalculateSatelliteAngles();
    }

    protected override void ChildRemoved(Control child)
    {
        RecalculateSatelliteAngles();
    }

    /// <summary>
    ///     Recalculates the angle of the child controls, without arranging them.
    /// </summary>
    protected void RecalculateSatelliteAngles()
    {
        switch (PlacementMode)
        {
            case SatellitePlacementMode.Manual:
                return;
            case SatellitePlacementMode.Even:
                PlaceChildrenEvenly();
                return;
            case SatellitePlacementMode.StackedCounterClockwise:
            case SatellitePlacementMode.StackedClockwise:
            {
                if (ChildCount == 0)
                    return;
                
                var lastAngle = Top; // up.
                {
                    var first = Children.First();
                    var diameter = GetControlDiameter(first);
                    var travelAngle = new Angle(diameter / OrbitDistance);
                    lastAngle -= travelAngle; // Compensate to make sure the first child is exactly at the top.
                }
                foreach (var c in Children)
                {
                    var diameter = GetControlDiameter(c);
                    var travelAngle = new Angle(diameter / OrbitDistance);
                    c.SetValue(SatelliteAngle, lastAngle + travelAngle);
                    if (PlacementMode == SatellitePlacementMode.StackedClockwise)
                        lastAngle += travelAngle;
                    else
                        lastAngle += travelAngle;
                }

                return;
            }
            default:
                throw new NotImplementedException($"Missing support for {PlacementMode}");
        }

    }

    private void PlaceChildrenEvenly()
    {
        var theta = (float.Pi * 2) / ChildCount;

        var lastAngle = Angle.FromWorldVec(-Vector2.UnitX) - theta;
        

        foreach (var c in Children)
        {
            c.SetValue(SatelliteAngle, lastAngle + theta);

            lastAngle += theta;
        }
    }

    /// <summary>
    ///     Computes the expected arrangement, centered around zero.
    /// </summary>
    protected List<(Control, UIBox2)> GetExpectedArrangement()
    {
        var list = new List<(Control, UIBox2)>();
        
        foreach (var c in Children)
        {
            var center = GetControlCenter(c);
            var diameter = GetControlDiameter(c);
            list.Add((c, UIBox2.FromDimensions(center - Vector2.One*(diameter/2), Vector2.One*diameter)));
        }

        return list;
    }

    protected override Vector2 MeasureOverride(Vector2 available)
    {
        RecalculateSatelliteAngles();

        return Vector2.Zero; // measure code sucks 
        
        //TODO: this shit doesnt work
        
        // Create a box enclosing all the arranged boxes.
        /*
        var topleft = Vector2.Zero;
        var bottomright = Vector2.Zero;
        foreach (var (c, box) in GetExpectedArrangement())
        {
            if (Vector2.Distance(topleft, Vector2.Zero) < Vector2.Distance(box.TopLeft, Vector2.Zero))
                topleft = box.TopLeft;
            
            if (Vector2.Distance(bottomright, Vector2.Zero) < Vector2.Distance(box.BottomRight, Vector2.Zero))
                bottomright = box.BottomRight;
        }

        var nbox = new UIBox2(topleft, bottomright);
        return nbox.Size;
        */
    }

    protected override Vector2 ArrangeOverride(Vector2 finalSize)
    {
        RecalculateSatelliteAngles();
        var offs = finalSize / 2;
        if (CenterToControl == false)
            offs = Vector2.Zero;
        
        foreach (var (c, box) in GetExpectedArrangement())
        {
            c.Arrange(box.Translated(offs));
        }
        
        return finalSize;
    }

    protected override void Draw(DrawingHandleScreen handle)
    {
        if (DebugCircleColor is { } c)
        {
            handle.DrawCircle((PixelSize / 2), OrbitDistance, c);
        }
        
        base.Draw(handle);
        
    }

    protected float GetControlDiameter(Control r)
    {
        return float.Min(r.Size.X, r.Size.Y); 
    }

    protected Vector2 GetControlCenter(Control c)
    {
        var theta = c.GetValue(SatelliteAngle);
        var r = OrbitDistance;
        var (y, x) = float.SinCos((float) theta.Theta);
        return new Vector2(r * x, r * y);
    }

    protected virtual void OnSatelliteUpdated(Control satellite)
    {
        InvalidateMeasure();
    }

    public OrbitalContainer()
    {
        HorizontalAlignment = HAlignment.Center;
        VerticalAlignment = VAlignment.Center;
    }
}


