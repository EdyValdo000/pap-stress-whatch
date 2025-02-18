using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;

namespace pap.Graphics
{
    public class HeartRateGraph : IDrawable
    {
        private List<float> _points = new List<float>();
        private float _currentBPM = 75;
        private int _maxPoints = 500; // Aumentado para dar mais espaço entre os batimentos
        private float _time = 0;
        private float _waveSpeed = 1f;
        public float CurrentBPM => _currentBPM;

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            // Define a cor do traço com base no BPM
            Color strokeColor;
            if (_currentBPM < 60 || _currentBPM > 100)
            {
                strokeColor = new Color(1f, 0.2f, 0.2f); // Vermelho suave
            }
            else
            {
                strokeColor = new Color(0.2f, 0.8f, 0.2f); // Verde suave
            }

            canvas.StrokeColor = strokeColor;
            canvas.StrokeSize = 3;

            if (_points.Count == 0)
                GenerateECGWave();

            _time += _waveSpeed;
            var offset = (int)(_time * 10) % _maxPoints;

            // Desenha a onda com interpolação suave
            for (int i = 1; i < _maxPoints; i++)
            {
                float x1 = (i - 1 - offset + _maxPoints) % _maxPoints * (dirtyRect.Width / _maxPoints);
                float y1 = dirtyRect.Height / 2 - _points[i - 1] * (dirtyRect.Height / 4);
                float x2 = (i - offset + _maxPoints) % _maxPoints * (dirtyRect.Width / _maxPoints);
                float y2 = dirtyRect.Height / 2 - _points[i] * (dirtyRect.Height / 4);

                if (Math.Abs(x2 - x1) < dirtyRect.Width / 4)
                {                    
                    canvas.DrawLine(x1, y1, x2, y2);
                }
            }

            // Exibe o valor de BPM
            DrawBPMText(canvas, dirtyRect);
        }

        private void DrawBPMText(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FontColor = Colors.White;
            canvas.FontSize = 16;
            string bpmText = $"BPM: {_currentBPM}";
            canvas.DrawString(bpmText, dirtyRect.Width - 100, 20, 100, 20, HorizontalAlignment.Right, VerticalAlignment.Top);
        }

        public void UpdateBPM(float bpm)
        {
            _currentBPM = bpm;
            _waveSpeed = bpm / 60f * 2f; // Ajuste para tornar a movimentação mais fluida
            GenerateECGWave();
        }

        private void GenerateECGWave()
        {
            _points.Clear();
            float frequency = _currentBPM / 60f;
            frequency = frequency < 1 ? 1 : frequency;
            float step = 2 * MathF.PI / _maxPoints;
            Random rand = new Random();

            for (int i = 0; i < _maxPoints; i++)
            {
                float baseLine = 0;
                float variation = 0;

                // Aumenta o espaçamento entre os picos ajustando o divisor (_maxPoints / (int)(frequency * 3))
                int spacing = _maxPoints / (int)(frequency * 3);

                // Simula a onda P (pequena subida inicial)
                if (i % spacing == 0)
                    variation = 0.3f + (float)rand.NextDouble() * 0.1f;

                // Simula o complexo QRS (pico alto)
                if (i % spacing == 1)
                    variation = -0.4f + (float)rand.NextDouble() * 0.1f; // Pequena queda antes do pico

                if (i % spacing == 2)
                    variation = 1.5f + (float)rand.NextDouble() * 0.2f; // Pico alto agora mais visível

                if (i % spacing == 3)
                    variation = -0.8f + (float)rand.NextDouble() * 0.1f; // Queda após o pico

                // Simula a onda T (recuperação)
                if (i % spacing == 6)
                    variation = 0.5f + (float)rand.NextDouble() * 0.1f;

                // Adiciona pequenas variações aleatórias para tornar a onda mais realista
                variation += (float)(rand.NextDouble() - 0.5) * 0.1f;

                _points.Add(baseLine + variation);
            }
        }
    }
}