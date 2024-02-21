// // This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// // If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Numerics;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;

namespace Content.AL.UIKit.Widgets;

/// <summary>
///     Used for backbuffer effects, renders its children to a render target and then applies a shader when drawing the final texture.
/// </summary>
[PublicAPI]
[Virtual]
public class BackbufferControl : Control
{
    [Dependency] protected readonly IClyde Clyde = default!;

    /// <summary>
    ///     Stylesheet property ID for backbuffer shaders.
    /// </summary>
    public const string BackbufferShader = "backbuffer-shader";
 
    private IRenderTexture? _target = default;
    
    /// <summary>
    ///     Scale to apply to the target's pixel size.
    /// </summary>
    public Vector2i TargetScale { get; set; } = Vector2i.One;

    private ShaderInstance? _shader = null;
    
    /// <summary>
    ///     The shader currently being used, if any. Setting this will override the stylesheet provided shader if one exists.
    /// </summary>
    public ShaderInstance? Shader
    {
        get
        {
            if (_shader is not null)
                return _shader;

            if (!TryGetStyleProperty(BackbufferShader, out ShaderInstance? s))
                return null;
            
            return s;
        }
        set => _shader = value;
    }

    public BackbufferControl()
    {
        IoCManager.InjectDependencies(this);
    }

    protected override Vector2 MeasureOverride(Vector2 availableSize)
    {
        var measure = base.MeasureOverride(availableSize);
        if (measure.X < 4 || measure.Y < 4)
        {
            // Arbitrary "oops we give up" to avoid render target getting mad.
            _target = null;
            return measure;
        }

        return measure;
    }

    protected override Vector2 ArrangeOverride(Vector2 finalSize)
    {
        _target = Clyde.CreateRenderTarget(
            (Vector2i) (finalSize * UIScale) * TargetScale,
            new RenderTargetFormatParameters(RenderTargetColorFormat.Rgba8),
            name: "UI Backbuffer"
        );

        return base.ArrangeOverride(finalSize);
    }

    protected override void RenderChildOverride(ref ControlRenderArguments args, int childIndex, Vector2i position)
    {
        if (_target is null)
            return; // Nope.
        
        var screen = args.Handle.DrawingHandleScreen;
        args.ScissorBox = null; // Can't scissor here.
        var captured = new RenderArguments(ref args);
        screen.RenderInRenderTarget(_target, () =>
        {
            var a = new ControlRenderArguments()
            {
                CoordinateTransform = ref captured.CoordinateTransform,
                Handle = captured.Handle,
                Total = ref captured.Total,
                Position = captured.Position,
                Modulate = captured.Modulate,
                ScissorBox = captured.ScissorBox,
            };
            base.RenderChildOverride(ref a, childIndex, position - GlobalPixelPosition);
            captured = new(ref a);
        }, null);
        args.Total = captured.Total;
    }

    protected override void PostRenderChildren(ref ControlRenderArguments args)
    {
        if (_target is null)
            return;
        var screen = args.Handle.DrawingHandleScreen;
        
        screen.UseShader(Shader);
        screen.DrawTextureRect(_target.Texture, PixelSizeBox.Translated(GlobalPixelPosition));
        screen.UseShader(null);
        screen.RenderInRenderTarget(_target, () => { }, Color.Transparent);
    }

    private sealed class RenderArguments
    {
        public IRenderHandle Handle;
        public int Total;
        public Vector2i Position;
        public Color Modulate;
        public UIBox2i? ScissorBox;
        public Matrix3 CoordinateTransform;
        public RenderArguments(ref ControlRenderArguments args)
        {
            Handle = args.Handle;
            Total = args.Total;
            Position = args.Position;
            Modulate = args.Modulate;
            ScissorBox = args.ScissorBox;
            CoordinateTransform = args.CoordinateTransform;
        }
        
    }
}
