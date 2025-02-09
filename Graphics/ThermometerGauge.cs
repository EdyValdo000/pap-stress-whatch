using Microsoft.Maui.Graphics;
using System;
using System.Threading.Tasks;

namespace pap.Graphics
{
    public class ThermometerGauge : IDrawable
    {
        public double Value { get; set; } = 36.5; // Temperatura inicial
        private bool _isAnimating = false;

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float centerX = dirtyRect.Center.X;
            float centerY = dirtyRect.Center.Y;
            float width = dirtyRect.Width / 5; // Largura do tubo
            float height = dirtyRect.Height * 0.7f; // Altura do tubo

            // Normaliza o valor da temperatura entre 35°C e 42°C
            float minTemp = 35.0f, maxTemp = 42.0f;
            float normalizedValue = Math.Clamp((float)((Value - minTemp) / (maxTemp - minTemp)), 0, 1);
            float mercuryHeight = height * normalizedValue; // Altura do líquido
            float mercuryTop = centerY + height / 2 - mercuryHeight; // Posição do topo do líquido

            // Desenha o tubo do termômetro com efeito de vidro fosco
            DrawTube(canvas, centerX, centerY, width, height);

            // Desenha o líquido com cor sólida (azul, verde ou vermelho)
            DrawMercury(canvas, centerX, mercuryTop, width, mercuryHeight);

            // Desenha a escala de temperatura
            DrawScale(canvas, centerX, centerY, width, height, minTemp, maxTemp);

            // Exibe a temperatura atual no topo
            DrawCurrentTemperature(canvas, centerX, centerY - height / 2 - 30);
        }

        private void DrawTube(ICanvas canvas, float centerX, float centerY, float width, float height)
        {
            // Efeito de sombra para dar profundidade
            canvas.StrokeColor = Colors.Gray;
            canvas.StrokeSize = 2;
            canvas.DrawRoundedRectangle(centerX - width / 2 + 2, centerY - height / 2 + 2, width, height, width / 2);

            // Tubo principal com efeito de vidro fosco
            canvas.StrokeSize = 4;
            canvas.StrokeColor = Colors.White;
            canvas.FillColor = Color.FromRgba(255, 255, 255, 100); // Transparência para efeito de vidro
            canvas.DrawRoundedRectangle(centerX - width / 2, centerY - height / 2, width, height, width / 2);
            canvas.FillRoundedRectangle(centerX - width / 2, centerY - height / 2, width, height, width / 2);
        }

        private void DrawMercury(ICanvas canvas, float centerX, float mercuryTop, float width, float mercuryHeight)
        {
            // Define a cor do líquido com base no valor da temperatura
            Color liquidColor = GetTemperatureColor();

            // Desenha o líquido com a cor calculada
            canvas.FillColor = liquidColor;
            canvas.FillRoundedRectangle(centerX - width / 2 + 2, mercuryTop, width - 4, mercuryHeight, width / 2);
        }

        private Color GetTemperatureColor()
        {
            // Define a cor com base em intervalos fixos
            if (Value < 37.0)
            {
                return Colors.Blue; // Azul para temperaturas abaixo de 37°C
            }
            else if (Value >= 37.0 && Value < 39.0)
            {
                return Colors.Green; // Verde para temperaturas entre 37°C e 39°C
            }
            else
            {
                return Colors.Red; // Vermelho para temperaturas acima de 39°C
            }
        }

        private void DrawScale(ICanvas canvas, float centerX, float centerY, float width, float height, float minTemp, float maxTemp)
        {
            for (int i = 0; i <= 7; i++)
            {
                float tempValue = minTemp + i;
                float y = centerY + height / 2 - (i / 7.0f) * height;

                // Marcadores da escala
                canvas.StrokeSize = 1;
                canvas.StrokeColor = Colors.White;
                canvas.DrawLine(centerX + width / 2 + 5, y, centerX + width / 2 + 15, y);

                // Valores da escala
                canvas.FontColor = Colors.White;
                canvas.FontSize = 12;
                canvas.DrawString(tempValue.ToString("0") + "°C", centerX + width / 2 + 20, y - 6, 40, 20, HorizontalAlignment.Left, VerticalAlignment.Center);
            }
        }

        private void DrawCurrentTemperature(ICanvas canvas, float centerX, float y)
        {
            // Exibe a temperatura atual no topo
            canvas.FontColor = Colors.White;
            canvas.FontSize = 24;
            canvas.DrawString(Value.ToString("0.0") + "°C", centerX - 20, y, 100, 30, HorizontalAlignment.Center, VerticalAlignment.Center);
        }

        public void UpdateValue(double newValue)
        {
            Value = Math.Clamp(newValue, 35.0, 42.0);

            if (!_isAnimating)
            {
                _isAnimating = true;
                StartAnimation();
            }
        }

        private async void StartAnimation()
        {
            while (_isAnimating)
            {
                await Task.Delay(50); // Simula uma animação suave
            }
        }
    }
}