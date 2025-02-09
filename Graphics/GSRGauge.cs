using Microsoft.Maui.Graphics;

namespace pap.Graphics
{
    public class GSRGauge : IDrawable
    {
        public double Value { get; set; } = 1000;

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float centerX = (float)dirtyRect.Center.X;
            float centerY = (float)dirtyRect.Center.Y;
            float radius = (float)(dirtyRect.Width / 2.5);

            canvas.StrokeSize = 10;
            canvas.StrokeColor = Colors.LightGray;
            canvas.DrawArc(centerX - radius, centerY - radius, radius * 2, radius * 2, 180, 180, false, false);

            float percentage = (float)(Value / 2000.0 * 180);
            canvas.StrokeColor = Colors.Green;
            canvas.DrawArc(centerX - radius, centerY - radius, radius * 2, radius * 2, 180, percentage, false, false);

            canvas.FontColor = Colors.White;
            canvas.FontSize = 18;
            canvas.DrawString(Value.ToString("0") + " µS", centerX, centerY, HorizontalAlignment.Center);
        }
    }
}
