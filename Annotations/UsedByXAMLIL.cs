namespace Content.AL.UIKit.Annotations;

/// <summary>
/// Indicates that a method is used by XAMLIL.
/// This should only be applied to attached property methods.
/// </summary>
[MeansImplicitUse]
// ReSharper disable once InconsistentNaming
internal sealed class UsedByXAMLIL : Attribute
{
    
}