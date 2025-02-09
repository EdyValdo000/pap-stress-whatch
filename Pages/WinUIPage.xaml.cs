using pap.Graphics;
using System.Security.Cryptography;

namespace pap.Pages;

public partial class WinUIPage : ContentPage
{
    private HeartRateGraph heartRateGraph = new HeartRateGraph();
    private OxygenGauge oxygenGauge = new OxygenGauge();

    public WinUIPage()
	{
		InitializeComponent();
        
        #region Gr�fico do BPM
        heartRateGraphicsView.Drawable = heartRateGraph;
        // Iniciar anima��o do ECG
        StartECGAnimation();
        #endregion
        
        oxygenGraphicsView.Drawable = oxygenGauge;
        StartOxygenGaugeAnimation();
    }

    #region Gr�fico do BPM
    private void StartECGAnimation()
    {
        Device.StartTimer(TimeSpan.FromMilliseconds(50), () =>
        {
            heartRateGraphicsView.Invalidate(); // Redesenha a linha animada
            return true; // Continua executando
        });
    }    

    private void StartHeartRate(object sender, EventArgs e)
    {
        Random rnd = new Random();
        float newBPM = rnd.Next(60, 120); // Gera um BPM aleat�rio entre 60 e 120
        heartRateGraph.UpdateBPM(newBPM); // Atualiza apenas o BPM
        heartRateGraphicsView.Invalidate(); // Redesenha sem reiniciar a onda
    }
    #endregion

    private async void StartOxygenGaugeAnimation()
    {
        while (true)
        {
            oxygenGraphicsView.Invalidate(); // Atualiza o desenho
            await Task.Delay(50); // Atualiza a anima��o suavemente
        }
    }

    private void OnValueChanged(object sender, EventArgs e)
    {
        Random random = new Random();
        double newValue = random.Next(0, 101); // Gera um valor aleat�rio de 0 a 100
        oxygenGauge.UpdateValue(newValue);
        oxygenGraphicsView.Invalidate(); // Atualiza o gr�fico
    }

}