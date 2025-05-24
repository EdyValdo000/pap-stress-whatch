using Font = Microsoft.Maui.Graphics.Font;
using Microsoft.Maui.Graphics;
using System.Collections.Generic;

namespace NewPap.Graphics;

public class BpmGraphDrawable : IDrawable
{
    // Configurações de estilo
    private const string FontFamily = "Segoe UI";
    private const float ValueFontSize = 28;
    private const float UnitFontSize = 12;
    private const float StatusFontSize = 10;

    // Cores do tema
    private readonly Color _normalColor = Color.FromArgb("#00B4FF");
    private readonly Color _warningColor = Color.FromArgb("#FFA500");
    private readonly Color _dangerColor = Color.FromArgb("#FF5555");
    private readonly Color _gridColor = Color.FromArgb("#252525");
    private readonly Color _backgroundColor = Color.FromArgb("#0D0D0D");
    private readonly Color _textColor = Colors.White;
    private readonly Color _borderColor = Color.FromArgb("#333333");

    // Dados
    private readonly List<int> _bpmHistory = new();
    private int _currentBpm = 72;
    private int _targetBpm = 120;
    private int _minNormalBpm = 60;
    private int _maxNormalBpm = 100;

    public void AddBpmReading(int bpm)
    {
        _currentBpm = bpm;
        _bpmHistory.Add(bpm);

        if (_bpmHistory.Count > 25) // Histórico reduzido para melhor visualização
        {
            _bpmHistory.RemoveAt(0);
        }
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        // Configuração inicial
        canvas.Antialias = true;
        canvas.BlendMode = BlendMode.Normal;

        // Fundo com bordas arredondadas
        DrawBackground(canvas, dirtyRect);

        // Margens internas
        var margin = 10f;
        var drawRect = new RectF(
            dirtyRect.X + margin,
            dirtyRect.Y + margin,
            dirtyRect.Width - 2 * margin,
            dirtyRect.Height - 2 * margin);

        // Áreas do gráfico
        var arcRect = new RectF(
            drawRect.X,
            drawRect.Y,
            drawRect.Width,
            drawRect.Height * 0.5f);

        var graphRect = new RectF(
            drawRect.X,
            drawRect.Y + drawRect.Height * 0.5f,
            drawRect.Width,
            drawRect.Height * 0.5f);

        // Componentes
        DrawArcGauge(canvas, arcRect); // Semi-arco na parte superior
        DrawLineGraph(canvas, graphRect);
        DrawCurrentValue(canvas, drawRect); // Valores centralizados
    }

    private void DrawBackground(ICanvas canvas, RectF rect)
    {
        // Fundo com bordas arredondadas
        canvas.FillColor = _backgroundColor;
        canvas.FillRoundedRectangle(rect, 16);

        // Borda sutil
        canvas.StrokeColor = _borderColor;
        canvas.StrokeSize = 1.5f;
        canvas.DrawRoundedRectangle(rect, 16);

        // Efeito de brilho sutil
        var glowPaint = new RadialGradientPaint
        {
            Center = new Point(rect.Width * 0.3f, rect.Height * 0.3f),
            Radius = rect.Width * 0.8f,
            GradientStops = new PaintGradientStop[]
            {
                new PaintGradientStop(0, Color.FromArgb("#20FFFFFF")),
                new PaintGradientStop(1, Colors.Transparent)
            }
        };

        canvas.SetFillPaint(glowPaint, rect);
        canvas.Alpha = 0.3f;
        canvas.FillRoundedRectangle(rect, 16);
        canvas.Alpha = 1f;
    }   

    private void DrawLineGraph(ICanvas canvas, RectF rect)
    {
        if (_bpmHistory.Count < 2) return;

        // Grade de fundo mais sutil
        canvas.StrokeColor = _gridColor;
        canvas.StrokeSize = 0.7f;
        canvas.StrokeDashPattern = new[] { 3f, 3f };

        // Linhas horizontais
        for (int i = 1; i <= 3; i++)
        {
            float y = rect.Y + rect.Height * (i / 3f);
            canvas.DrawLine(rect.X, y, rect.X + rect.Width, y);
        }

        canvas.StrokeDashPattern = null;

        // Escalonamento
        int minBpm = _minNormalBpm - 20;
        int maxBpm = _maxNormalBpm + 20;

        // Linha do gráfico com preenchimento gradiente
        var path = new PathF();
        float xStep = rect.Width / (_bpmHistory.Count - 1);

        for (int i = 0; i < _bpmHistory.Count; i++)
        {
            float x = rect.X + i * xStep;
            float y = rect.Y + rect.Height - ((_bpmHistory[i] - minBpm) / (float)(maxBpm - minBpm)) * rect.Height;

            if (i == 0) path.MoveTo(x, y);
            else path.LineTo(x, y);
        }

        // Preenchimento gradiente
        var fillPath = new PathF(path);
        fillPath.LineTo(rect.X + rect.Width, rect.Y + rect.Height);
        fillPath.LineTo(rect.X, rect.Y + rect.Height);
        fillPath.Close();

        var fillPaint = new LinearGradientPaint
        {
            StartColor = _normalColor.WithAlpha(0.25f),
            EndColor = _normalColor.WithAlpha(0.05f),
            StartPoint = new Point(0, 0),
            EndPoint = new Point(0, 1)
        };

        canvas.SetFillPaint(fillPaint, rect);
        canvas.FillPath(fillPath);

        // Linha do gráfico
        canvas.StrokeColor = _normalColor;
        canvas.StrokeSize = 2f;
        canvas.StrokeLineCap = LineCap.Round;
        canvas.DrawPath(path);

        // Pontos de destaque
        canvas.FillColor = _normalColor;
        for (int i = 0; i < _bpmHistory.Count; i += 4)
        {
            float x = rect.X + i * xStep;
            float y = rect.Y + rect.Height - ((_bpmHistory[i] - minBpm) / (float)(maxBpm - minBpm)) * rect.Height;

            canvas.FillColor = _normalColor.WithAlpha(0.3f);
            canvas.FillCircle(x, y, 5);
            canvas.FillColor = _normalColor;
            canvas.FillCircle(x, y, 2.5f);
        }
    }

    private void DrawArcGauge(ICanvas canvas, RectF rect)
    {
        const float startAngle = 0;
        const float sweepAngle = 180;
        float currentSweep = Math.Min(sweepAngle, sweepAngle * (_currentBpm / (float)_targetBpm));
        var arcColor = GetBpmColor(_currentBpm);

        // Configurações para um arco mais fechado (raio menor)
        float arcWidth = rect.Width * 0.8f;  // Reduz a largura do arco
        float arcHeight = rect.Height * 0.4f; // Reduz ainda mais a altura
        float arcXOffset = (rect.Width - arcWidth) / 2; // Centraliza horizontalmente
        float arcYOffset = rect.Height * 0.2f; // Posiciona mais para cima

        // Arco de fundo
        canvas.StrokeColor = arcColor.WithAlpha(0.2f);
        canvas.StrokeSize = 8;
        canvas.StrokeLineCap = LineCap.Round;
        canvas.DrawArc(
            rect.X + arcXOffset,
            rect.Y + arcYOffset,
            arcWidth,
            arcHeight,
            startAngle,
            startAngle + sweepAngle,
            false,
            false);

        // Arco de progresso
        canvas.StrokeColor = arcColor;
        canvas.DrawArc(
            rect.X + arcXOffset,
            rect.Y + arcYOffset,
            arcWidth,
            arcHeight,
            startAngle,
            startAngle + currentSweep,
            false,
            false);

        // Efeito de brilho no ponto final
        if (currentSweep > 0)
        {
            var endAngle = startAngle + currentSweep;
            var radiusX = arcWidth / 2;
            var radiusY = arcHeight / 2;
            var centerX = rect.X + arcXOffset + radiusX;
            var centerY = rect.Y + arcYOffset + radiusY;

            // Cálculo considerando a elipse (arcWidth pode ser diferente de arcHeight)
            var endX = centerX + radiusX * Math.Cos(endAngle * Math.PI / 180);
            var endY = centerY + radiusY * Math.Sin(endAngle * Math.PI / 180);

            canvas.FillColor = arcColor.WithAlpha(0.5f);
            canvas.FillCircle((float)endX, (float)endY, 5);
        }
    }

    private void DrawCurrentValue(ICanvas canvas, RectF rect)
    {
        var textColor = GetBpmColor(_currentBpm);
        var centerX = rect.X + rect.Width / 2;
        var centerY = rect.Y + rect.Height * 0.4f; // Ajustado para cima para o arco menor

        // Configurações comuns de fonte
        var boldFont = new Font(FontFamily, (int)FontWeight.Bold);
        var mediumFont = new Font(FontFamily, (int)FontWeight.Medium);

        // Valor BPM (grande)
        canvas.Font = boldFont;
        canvas.FontSize = ValueFontSize;
        canvas.FontColor = textColor;

        var bpmText = _currentBpm.ToString();
        var bpmSize = canvas.GetStringSize(bpmText, boldFont, ValueFontSize);

        // Unidade "BPM" (pequena) - na mesma linha
        canvas.FontSize = UnitFontSize;
        var unitText = "BPM";
        var unitSize = canvas.GetStringSize(unitText, boldFont, UnitFontSize);

        // Status (mais embaixo)
        string status = _currentBpm < _minNormalBpm ? "BAIXO" :
                       _currentBpm > _maxNormalBpm ? "ALTO" : "NORMAL";

        canvas.Font = mediumFont;
        canvas.FontSize = StatusFontSize;
        var statusSize = canvas.GetStringSize(status, mediumFont, StatusFontSize);

        // Desenhar valor BPM e unidade na mesma linha
        float combinedWidth = bpmSize.Width + unitSize.Width;
        float startX = centerX - combinedWidth / 2;

        // Valor BPM
        canvas.DrawString(
            bpmText,
            startX,
            centerY,
            bpmSize.Width,
            bpmSize.Height,
            HorizontalAlignment.Left,
            VerticalAlignment.Top);

        // Unidade "BPM"
        canvas.FontColor = _textColor.WithAlpha(0.8f);
        canvas.DrawString(
            unitText,
            startX + bpmSize.Width,
            centerY + (bpmSize.Height - unitSize.Height) * 0.5f, // Alinhamento vertical no meio
            unitSize.Width,
            unitSize.Height,
            HorizontalAlignment.Left,
            VerticalAlignment.Top);

        // Desenhar status mais abaixo (ajustado para ficar mais baixo)
        float statusY = centerY + bpmSize.Height + 20; // 20px abaixo do valor BPM (aumentado)
        canvas.FontColor = GetBpmColor(_currentBpm);
        canvas.DrawString(
            status,
            centerX - statusSize.Width / 2,
            statusY,
            statusSize.Width,
            statusSize.Height,
            HorizontalAlignment.Left,
            VerticalAlignment.Top);

        // Efeito de brilho sutil no texto (opcional)
        canvas.FontColor = textColor.WithAlpha(0.3f);
        canvas.SetShadow(new SizeF(0, 0), 5, textColor);
        canvas.DrawString(
            bpmText,
            startX,
            centerY,
            bpmSize.Width,
            bpmSize.Height,
            HorizontalAlignment.Left,
            VerticalAlignment.Top);
        canvas.SetShadow(new SizeF(0, 0), 0, Colors.Transparent);
    }

    private Color GetBpmColor(int bpm)
    {
        if (bpm < _minNormalBpm - 10 || bpm > _maxNormalBpm + 10)
            return _dangerColor;

        if (bpm < _minNormalBpm || bpm > _maxNormalBpm)
            return _warningColor;

        return _normalColor;
    }

    public void UpdateBpmValue(int newBpm)
    {
        AddBpmReading(newBpm);
    }
}