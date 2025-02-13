using Microsoft.Maui.Graphics;
using System;
using System.Threading.Tasks;

namespace pap.Graphics
{
    public class GsrWave : IDrawable
    {
        public double Value { get; set; } = 50; // Valor inicial médio
        private float _waveOffset = 0;
        private bool _isAnimating = false;

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float centerX = dirtyRect.Center.X;
            float centerY = dirtyRect.Center.Y;
            float width = dirtyRect.Width * 0.8f;
            float height = dirtyRect.Height * 0.5f;

            // 🟢 Gradiente de fundo sutil
            var gradient = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 1),
                GradientStops = new GradientStopCollection
                {
                    new GradientStop(Colors.DarkBlue.WithAlpha(0.2f), 0.0f),
                    new GradientStop(Colors.Teal.WithAlpha(0.1f), 1.0f)
                }
            };
            canvas.SetFillPaint(gradient, dirtyRect);
            canvas.FillRectangle(dirtyRect);

            // 🔵 Cor da onda muda conforme a intensidade do GSR
            Color waveColor = GetGsrColor(Value);
            canvas.StrokeSize = 4;
            canvas.StrokeColor = waveColor;

            // 🔵 Desenha a onda baseada no GSR
            PathF path = new PathF();
            float amplitude = 20 + (float)(Value / 5); // Ajusta a altura conforme o GSR
            float frequency = 0.05f + (float)(Value / 1000); // Aumenta a frequência conforme o GSR

            for (float x = centerX - width / 2; x <= centerX + width / 2; x += 5)
            {
                float y = centerY + (float)(amplitude * Math.Sin(frequency * (x + _waveOffset)));
                if (x == centerX - width / 2)
                    path.MoveTo(x, y);
                else
                    path.LineTo(x, y);
            }

            canvas.DrawPath(path);

            // 🔹 Exibe o valor do GSR no centro
            canvas.FontColor = Colors.White;
            canvas.FontSize = 24;
            canvas.DrawString(Value.ToString("0"), centerX, centerY - 40, HorizontalAlignment.Center);
        }

        public void UpdateValue(double newValue)
        {
            Value = Math.Clamp(newValue, 0, 100);

            if (!_isAnimating)
            {
                _isAnimating = true;
                StartWaveAnimation();
            }
        }

        // 🌊 Animação contínua da onda
        private async void StartWaveAnimation()
        {
            while (_isAnimating)
            {
                _waveOffset += (float)(Value / 20); // Aumenta a velocidade conforme o GSR
                await Task.Delay(50);
            }
        }

        // 🔥 Retorna a cor da onda baseada no GSR
        private Color GetGsrColor(double value)
        {
            if (value < 30) return Colors.Blue;  // Relaxado
            if (value < 60) return Colors.Teal;  // Neutro
            return Colors.LimeGreen;             // Estresse elevado
        }
    }
}