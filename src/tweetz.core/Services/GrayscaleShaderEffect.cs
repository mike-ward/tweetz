using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Effects;

namespace tweetz.core.Services
{
    public class GrayscaleShaderEffect : ShaderEffect
    {
        private static readonly PixelShader _pixelShader = new PixelShader
        {
            UriSource = new Uri("Infrastructure/Resources/GrayscaleShader.ps", UriKind.Relative)
        };

        public GrayscaleShaderEffect()
        {
            PixelShader = _pixelShader;

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(DesaturationFactorProperty);
        }

        public static readonly DependencyProperty InputProperty =
            ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(GrayscaleShaderEffect), 0);

        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }

        public static readonly DependencyProperty DesaturationFactorProperty =
            DependencyProperty.Register(
                "DesaturationFactor",
                typeof(double), typeof(GrayscaleShaderEffect),
                new UIPropertyMetadata(0.0, PixelShaderConstantCallback(0), CoerceDesaturationFactor));

        public double DesaturationFactor
        {
            get { return (double)GetValue(DesaturationFactorProperty); }
            set { SetValue(DesaturationFactorProperty, value); }
        }

        private static object CoerceDesaturationFactor(DependencyObject d, object value)
        {
            var effect = (GrayscaleShaderEffect)d;
            double newFactor = (double)value;

            if (newFactor < 0.0 || newFactor > 1.0)
            {
                return effect.DesaturationFactor;
            }

            return newFactor;
        }
    }
}