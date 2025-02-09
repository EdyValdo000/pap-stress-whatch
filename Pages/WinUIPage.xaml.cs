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

        heartRateGraphicsView.Drawable = heartRateGraph;

        // Iniciar anima��o do ECG
        StartECGAnimation();

        oxygenGraphicsView.Drawable = oxygenGauge;
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
   
    private void OnUpdateOxygen(object sender, EventArgs e)
    {
        Random random = new Random();
        double newOxygen = random.Next(0, 100); // Gera um valor aleat�rio entre 85% e 100%

        oxygenGauge.UpdateValue(0); // Atualiza o valor do gr�fico
        oxygenGraphicsView.Invalidate(); // Redesenha o gr�fico com o novo valor
    }
}