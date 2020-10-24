using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace tweetz.core.Services
{
    /// <summary>
    /// Grayscale pixel shader effect using the HSP method.
    /// </summary>
    public class GrayscaleShaderEffect : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = RegisterPixelShaderSamplerProperty(
            "Input",
            typeof(GrayscaleShaderEffect),
            0);

        public GrayscaleShaderEffect()
        {
            PixelShader = new PixelShader
            {
                UriSource = new Uri("Infrastructure/Resources/GrayscaleShader.ps", UriKind.Relative),
            };
            UpdateShaderValue(InputProperty);
        }

        public Brush Input
        {
            get => (Brush)(GetValue(InputProperty));
            set => SetValue(InputProperty, value);
        }
    }
}