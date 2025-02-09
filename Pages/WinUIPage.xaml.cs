using pap.Graphics;
using System.Security.Cryptography;

namespace pap.Pages;

public partial class WinUIPage : ContentPage
{
    private HeartRateGraph heartRateGraph = new HeartRateGraph();
    private OxygenGauge oxygenGauge = new OxygenGauge();
    private ThermometerGauge thermometerGauge = new ThermometerGauge();
    private GsrWave gsrWave = new GsrWave();

    public WinUIPage()
	{
		InitializeComponent();
        
        #region Gr�fico do BPM
        heartRateGraphicsView.Drawable = heartRateGraph;
        // Iniciar anima��o do ECG
        StartECGAnimation();
        #endregion

        #region Gr�fico de Oxig�nio
        oxygenGraphicsView.Drawable = oxygenGauge;
        StartOxygenGaugeAnimation();
        #endregion

        #region Gr�fico de Temperatura
        temperatureGraphicsView.Drawable = thermometerGauge;
        #endregion

        gsrGraphicsView.Drawable = gsrWave;
        StartGSRAnimation();
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

    #region Gr�fico de Oxig�nio
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
    #endregion
    
    #region Gr�fico de Temperatura
    private void Button_Clicked(object sender, EventArgs e)
    {
        Random random = new Random();
        double newValue = random.Next(35, 42); // Gera um valor aleat�rio de 0 a 100
        thermometerGauge.UpdateValue(newValue);
        temperatureGraphicsView.Invalidate(); // Atualiza o gr�fico
    }
    #endregion

    #region Gr�fico de GSR
    private void StartGSRAnimation()
    {
        Device.StartTimer(TimeSpan.FromMilliseconds(50), () =>
        {
            gsrGraphicsView.Invalidate(); // Redesenha a linha animada
            return true; // Continua executando
        });
    }

    private void OnGSRValueChanged(object sender, EventArgs e)
    {
        Random random = new Random();
        double newValue = random.Next(0, 101); // Gera um valor aleat�rio de 0 a 100
        gsrWave.UpdateValue(newValue);
        gsrGraphicsView.Invalidate(); // Atualiza o gr�fico
    }
    #endregion
}