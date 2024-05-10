using Content.AL.UIKit.Interfaces;
using Robust.Client.UserInterface.Controls;

namespace Content.AL.UIKit.Widgets;

public abstract class Panel : PanelContainer, IBrightnessAware
{
    public float Luminance()
    {
        return GetStyleBox().Luminance();
    }
}

[Virtual]
public class VPanel : Panel
{
    public readonly VStack Inner = new();

    public VPanel()
    {
        AddChild(Inner);
        XamlChildren = Inner.Children;
    }
}

[Virtual]
public class HPanel : Panel
{
    public readonly HStack Inner = new();

    public HPanel()
    {
        AddChild(Inner);
        XamlChildren = Inner.Children;
    }
}
