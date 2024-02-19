// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Content.AL.UIKit.Sheets;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Utility;
using static Robust.Client.UserInterface.StylesheetHelpers;


namespace Content.AL.UIKit.Widgets;

// TODO: Replace OptionButton with our own version, so we can stylize it like a drawer.
[PublicAPI]
[Virtual]
public class EnumSelector : OptionButton
{
    public const string EnumSelectorTextureProperty = "selectorTexture";

    public Type? CurrentType { get; private set; }

    public EnumSelector()
    {
        // TODO: Figure out how to calculate the minimum width of the control based on the selectable items.
        //HorizontalExpand = false;
        Margin = new Thickness(2);
        OptionStyleClasses.Add(ALStyleConsts.EnumSelectorOptionClass);
    }

    public Enum Selected => (Enum) SelectedMetadata!;

    public T GetSelected<T>()
        where T : Enum
    {
        return (T) Selected;
    }

    public void Setup(Type selectable)
    {
        CurrentType = selectable;
        var targets = Enum.GetValues(selectable);
        DebugTools.AssertEqual(Enum.GetUnderlyingType(selectable), typeof(int));
        var tValues = Enum.GetValuesAsUnderlyingType(selectable);

        var idx = 0;
        foreach (var v in targets)
        {
            var name = Localize(selectable, Enum.GetName(selectable, v)!);
            var enumVal = (int) tValues.GetValue(idx)!;
            if (TryGetStyleProperty(EnumPropertyName(selectable, name, EnumSelectorTextureProperty), out Texture? tex))
                AddItem(tex!, name, enumVal);
            else
                AddItem(name, enumVal);

            SetItemMetadata(enumVal, v);
            idx++;
        }
    }

    public static string EnumPropertyName(Type t, string enumName, string subProp)
    {
        return $"uiprop-{t.Name}-{enumName}-{subProp}";
    }

    private string Localize(Type t, string enumName)
    {
        return Loc.GetString($"ui-enumselector-{t.Name}-{enumName}");
    }

    public override void ButtonOverride(Robust.Client.UserInterface.Controls.Button button)
    {
        //TODO: This really should be a stylesheet thing but that's a refactor I don't want to take upon myself right now.
        button.Margin = new Thickness(4);
    }
}

[Stylesheet]
public sealed class EnumSelectorSheet : BaseSubsheet
{
    public override StyleRule[] GetRules(BaseStyle origin)
    {
        return new StyleRule[]
        {
            Element().Class(ALStyleConsts.EnumSelectorOptionClass).Normal()
                .Prop(ALStyleConsts.StyleBox, origin.ButtonBackgrounds[2]),
            Element().Class(ALStyleConsts.EnumSelectorOptionClass).Hover()
                .Prop(ALStyleConsts.StyleBox, origin.ButtonBackgrounds[2]),
            Element().Class(ALStyleConsts.EnumSelectorOptionClass).Pressed()
                .Prop(ALStyleConsts.StyleBox, origin.ButtonBackgrounds[2]),
            Element().Class(ALStyleConsts.EnumSelectorOptionClass).Disabled()
                .Prop(ALStyleConsts.StyleBox, origin.ButtonBackgrounds[2]),
        };
    }
}
