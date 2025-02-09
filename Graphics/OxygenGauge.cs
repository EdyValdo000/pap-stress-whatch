using Microsoft.Maui.Graphics;

namespace pap.Graphics
{
    public class OxygenGauge : IDrawable
    {
        public double Value { get; set; } = 98; // Valor inicial

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float centerX = (float)dirtyRect.Center.X;
            float centerY = (float)dirtyRect.Center.Y;
            float radius = (float)(dirtyRect.Width / 2.5);

            canvas.StrokeSize = 10;
            canvas.StrokeColor = Colors.LightGray;
            canvas.DrawArc(centerX - radius, centerY - radius, radius * 2, radius * 2, 180, 180, false, false);

            float percentage = (float)(Value / 100.0 * 180);
            canvas.StrokeColor = Colors.Blue;
            canvas.DrawArc(centerX - radius, centerY - radius, radius * 2, radius * 2, 180, percentage, false, false);

            canvas.FontColor = Colors.White;
            canvas.FontSize = 18;
            canvas.DrawString(Value.ToString("0") + "%", centerX, centerY, HorizontalAlignment.Center);
        }

        public void UpdateValue(double newValue)
        {
            Value = Math.Clamp(newValue, 0, 100); // Garante que o valor fique entre 0 e 100
        }
    }
}
