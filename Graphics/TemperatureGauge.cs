namespace pap.Graphics;

public class TemperatureGauge : IDrawable
{
    public float Value { get; set; }
    public float MaxValue { get; set; } = 120;

    // Variáveis configuráveis
    public float GaugeScale { get; set; } = 1.0f;  // Escala do gráfico (1.0 = padrão)
    public float GaugeOffsetX { get; set; } = 0;   // Deslocamento horizontal
    public float GaugeOffsetY { get; set; } = -30; // Deslocamento vertical
    public float StartAngle { get; set; } = 100;   // Ângulo inicial do arco
    public float SweepAngleMax { get; set; } = 100; // Ângulo máximo do arco

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        float centerX = dirtyRect.Center.X + GaugeOffsetX;
        float centerY = dirtyRect.Center.Y + GaugeOffsetY;
        float radius = (Math.Min(dirtyRect.Width, dirtyRect.Height) / 2 - 10) * GaugeScale;

        float sweepAngle = SweepAngleMax * ((Value - 240) / MaxValue); // Arco proporcional à temperatura

        // Fundo do arco
        canvas.StrokeSize = 10 * GaugeScale;
        canvas.StrokeColor = Colors.Gray.WithAlpha(0.3f);
        canvas.DrawArc(centerX - radius, centerY, radius * 2, radius * 2, StartAngle, SweepAngleMax, false, false);

        // Arco da temperatura (Azul)
        canvas.StrokeColor = Colors.Blue;
        canvas.DrawArc(centerX - radius, centerY, radius * 2, radius * 2, StartAngle, sweepAngle, false, false);

        // Texto da temperatura
        canvas.FontColor = Colors.White;
        canvas.FontSize = 20 * GaugeScale;
        canvas.DrawString($"{Value}°C", centerX, centerY + (10 * GaugeScale) + 30, HorizontalAlignment.Center);
    }

    // Método para atualizar o valor da temperatura
    public void UpdateTemperature(float newValue)
    {
        Value = newValue;
    }

    // Método para ajustar a escala do gráfico
    public void SetScale(float scale)
    {
        GaugeScale = scale;
    }

    // Método para ajustar a posição do gráfico
    public void SetPosition(float offsetX, float offsetY)
    {
        GaugeOffsetX = offsetX;
        GaugeOffsetY = offsetY;
    }
}