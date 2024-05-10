// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Numerics;
using Content.AL.UIKit.Misc;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Robust.Client.UserInterface.StylesheetHelpers;

namespace Content.AL.UIKit;

[PublicAPI]
public static class ALStylesheetHelpers
{
    public static MutableSelectorElement E<T>()
        where T: Control
    {
        return Element<T>();
    }

    public static MutableSelectorChild ParentOf(this MutableSelector selector, MutableSelector other)
    {
        return Child().Parent(selector).Child(other);
    }

    public static MutableSelectorChild ParentOf<TOther>(this MutableSelector selector)
        where TOther: Control
    {
        return selector.ParentOf(Element<TOther>());
    }

    public static MutableSelectorElement Button()
    {
        var e = new MutableSelectorElement() {Type = typeof(ContainerButton)}.Class(ContainerButton.StyleClassButton);
        return e;
    }

    public static MutableSelectorElement Button<T>()
    {
        var e = new MutableSelectorElement() {Type = typeof(T)}.Class(ContainerButton.StyleClassButton);
        return e;
    }

    public static MutableSelectorElement Positive(this MutableSelectorElement i)
    {
        return i.Class(ALStyleConsts.Positive);
    }

    public static MutableSelectorElement Negative(this MutableSelectorElement i)
    {
        return i.Class(ALStyleConsts.Negative);
    }

    public static MutableSelectorElement Normal(this MutableSelectorElement i)
    {
        return i.Pseudo(ContainerButton.StylePseudoClassNormal);
    }

    public static MutableSelectorElement Hover(this MutableSelectorElement i)
    {
        return i.Pseudo(ContainerButton.StylePseudoClassHover);
    }

    public static MutableSelectorElement Pressed(this MutableSelectorElement i)
    {
        return i.Pseudo(ContainerButton.StylePseudoClassPressed);
    }

    public static MutableSelectorElement Disabled(this MutableSelectorElement i)
    {
        return i.Pseudo(ContainerButton.StylePseudoClassDisabled);
    }

    public static MutableSelectorLuminance BgBrighterThan(this MutableSelector selector, float lum)
    {
        return new(selector, lum, SelectorLuminance.SelectorMode.GreaterThan);
    }

    public static MutableSelectorLuminance BgDarkerThan(this MutableSelector selector, float lum)
    {
        return new(selector, lum, SelectorLuminance.SelectorMode.LessThan);
    }

    public static MutableSelectorNeighbour Above(this MutableSelector selector, MutableSelector other, int by = 1)
    {
        return new MutableSelectorNeighbour(selector, other, by, SelectorNeighbour.NeighbourDirection.Below);
    }

    public static MutableSelectorNeighbour Below(this MutableSelector selector, MutableSelector other, int by = 1)
    {
        return new MutableSelectorNeighbour(selector, other, by, SelectorNeighbour.NeighbourDirection.Above);
    }

    public static MutableSelectorNeighbour Nearby(this MutableSelector selector, MutableSelector other, int by = 1)
    {
        return new MutableSelectorNeighbour(selector, other, by, SelectorNeighbour.NeighbourDirection.Either);
    }

    public static MutableSelector Margin(this MutableSelector selector, Thickness margin)
    {
        return selector.Prop(nameof(Control.Margin), margin);
    }

    public static MutableSelector Margin(this MutableSelector selector, float margin)
    {
        return selector.Margin(new Thickness(margin));
    }

    public static MutableSelector MinWidth(this MutableSelector selector, float width)
    {
        return selector.Prop(nameof(Control.MinWidth), width);
    }

    public static MutableSelector MinHeight(this MutableSelector selector, float height)
    {
        return selector.Prop(nameof(Control.MinHeight), height);
    }

    public static MutableSelector MinSize(this MutableSelector selector, Vector2 size)
    {
        return selector.MinWidth(size.X).MinHeight(size.Y);
    }

    public static MutableSelector MaxWidth(this MutableSelector selector, float width)
    {
        return selector.Prop(nameof(Control.MaxWidth), width);
    }

    public static MutableSelector MaxHeight(this MutableSelector selector, float height)
    {
        return selector.Prop(nameof(Control.MaxHeight), height);
    }

    public static MutableSelector MaxSize(this MutableSelector selector, Vector2 size)
    {
        return selector.MaxWidth(size.X).MaxHeight(size.Y);
    }

    public static MutableSelector SetWidth(this MutableSelector selector, float width)
    {
        return selector.Prop(nameof(Control.SetWidth), width);
    }

    public static MutableSelector SetHeight(this MutableSelector selector, float height)
    {
        return selector.Prop(nameof(Control.SetHeight), height);
    }

    public static MutableSelector SetSize(this MutableSelector selector, Vector2 size)
    {
        return selector.SetWidth(size.X).SetHeight(size.Y);
    }

    public static MutableSelector HorizontalExpand(this MutableSelector selector, bool val)
    {
        return selector.Prop(nameof(Control.HorizontalExpand), val);
    }

    public static MutableSelector VerticalExpand(this MutableSelector selector, bool val)
    {
        return selector.Prop(nameof(Control.VerticalExpand), val);
    }

    public static MutableSelector HorizontalAlignment(this MutableSelector selector, Control.HAlignment val)
    {
        return selector.Prop(nameof(Control.HorizontalExpand), val);
    }

    public static MutableSelector VerticalAlignment(this MutableSelector selector, Control.VAlignment  val)
    {
        return selector.Prop(nameof(Control.VerticalExpand), val);
    }


    public static T AndIf<T>(this T inp, bool v, Func<T, T> fn)
    {
        if (v)
            return fn(inp);

        return inp;
    }

    public static T OrIf<T>(this T inp, bool v, T other)
    {
        if (v)
            return other;

        return inp;
    }
}
