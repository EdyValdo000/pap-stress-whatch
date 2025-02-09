using Microsoft.Maui.Graphics;
using System;
using System.Threading.Tasks;

namespace pap.Graphics
{
    public class OxygenGauge : IDrawable
    {
        public double Value { get; set; } = 0; // Começa vazio
        private float _waveOffset = 0; // Controle da animação das ondas
        private bool _isAnimating = false;

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float centerX = dirtyRect.Center.X;
            float centerY = dirtyRect.Center.Y;
            float radius = dirtyRect.Width / 2.5f;

            // 🟢 Desenha o contorno do medidor
            canvas.StrokeSize = 5;
            canvas.StrokeColor = Colors.LightGray;
            canvas.DrawCircle(centerX, centerY, radius);

            if (Value == 0) return; // Se for 0%, não desenha nada

            // 🔵 Define a altura do nível da "água"
            float fillHeight = (float)(radius * 2 * (Value / 100.0));
            float waterLevel = centerY + radius - fillHeight;

            // 🔵 Cria um gradiente para a "água"
            var gradient = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 1),
                GradientStops = new GradientStopCollection
                {
                    new GradientStop(Colors.Blue, 0.0f),
                    new GradientStop(Colors.Cyan, 1.0f)
                }
            };

            // 🔵 Garante que o preenchimento não ultrapasse os limites do círculo
            canvas.ClipPath(CreateCirclePath(centerX, centerY, radius));
            canvas.SetFillPaint(gradient, dirtyRect);

            // 🔵 Desenha a água com ondulação dentro do círculo
            for (float x = centerX - radius; x < centerX + radius; x += 2)
            {
                float waveAmplitude = 6; // Intensidade da onda
                float waveFrequency = 0.12f; // Frequência da onda
                float wave = waveAmplitude * (float)Math.Sin(waveFrequency * (x + _waveOffset));

                float y = waterLevel + wave;
                if (y < centerY + radius) // Mantém dentro do círculo
                {
                    canvas.FillRectangle(x, y, 2, centerY + radius - y);
                }
            }

            // 🔵 Restaura o estado do canvas após aplicar o ClipPath
            canvas.ResetState();

            // 🔵 Exibe o valor no centro do círculo
            canvas.FontColor = Colors.White;
            canvas.FontSize = 24;

            float textWidth = 60; // Define um tamanho aproximado para o texto
            float textHeight = 30;

            float textX = centerX - textWidth / 2;
            float textY = centerY - textHeight / 2;

            canvas.DrawString(Value.ToString("0") + "%", textX, textY, textWidth, textHeight, HorizontalAlignment.Center, VerticalAlignment.Center);
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

        // 🔄 Animação contínua das ondas
        private async void StartWaveAnimation()
        {
            while (_isAnimating)
            {
                _waveOffset += 5; // Movimento suave da onda
                await Task.Delay(50); // Atualização da animação
            }
        }

        private PathF CreateCirclePath(float cx, float cy, float r)
        {
            PathF path = new PathF();
            path.AppendCircle(cx, cy, r);
            return path;
        }
    }
}
