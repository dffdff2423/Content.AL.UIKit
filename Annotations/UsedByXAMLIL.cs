// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

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