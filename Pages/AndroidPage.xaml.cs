using pap.Conection;
using pap.Graphics;
using StressWatchML;

namespace pap.Pages;
public partial class AndroidPage : ContentPage
{
    private HeartRateGraph heartRateGraph = new HeartRateGraph();
    private OxygenGauge oxygenGauge = new OxygenGauge();
    private ThermometerGauge thermometerGauge = new ThermometerGauge();
    private GsrWave gsrWave = new GsrWave();

    private readonly WifiConection Esp8266 = new();

    public AndroidPage()
    {
        InitializeComponent();

        heartRateGraphicsView.Drawable = heartRateGraph;
        oxygenGraphicsView.Drawable = oxygenGauge;
        temperatureGraphicsView.Drawable = thermometerGauge;
        gsrGraphicsView.Drawable = gsrWave;
    }

    #region Animations graphics
    private CancellationTokenSource oxygenAnimationToken;

    private void StartECGAnimation()
    {
        Device.StartTimer(TimeSpan.FromMilliseconds(50), () =>
        {
            heartRateGraphicsView.Invalidate();
            return animationsRunning; // Continua executando se estiver ativo
        });
    }

    private async void StartOxygenGaugeAnimation()
    {
        oxygenAnimationToken = new CancellationTokenSource();
        var token = oxygenAnimationToken.Token;

        while (!token.IsCancellationRequested)
        {
            oxygenGraphicsView.Invalidate();
            await Task.Delay(50);
        }
    }

    private void StartGSRAnimation()
    {
        Device.StartTimer(TimeSpan.FromMilliseconds(50), () =>
        {
            gsrGraphicsView.Invalidate();
            return animationsRunning; // Continua executando se estiver ativo
        });
    }

    private bool animationsRunning = false;

    private void StartAnimations()
    {
        animationsRunning = true;
        StartGSRAnimation();
        StartECGAnimation();
        StartOxygenGaugeAnimation();
    }

    private void StopAnimations()
    {
        animationsRunning = false;
        oxygenAnimationToken?.Cancel(); // Interrompe a animação do oxigênio
    }
    #endregion

    #region Random Update
    private double HeartRateUpdate()
    {
        Random rnd = new Random();
        float newBPM = rnd.Next(60, 120); // Gera um BPM aleatório entre 60 e 120
        heartRateGraph.UpdateBPM(newBPM); // Atualiza apenas o BPM
        heartRateGraphicsView.Invalidate(); // Redesenha sem reiniciar a onda
        return newBPM;
    }

    private double OxygenUpdate()
    {
        Random random = new Random();
        double newValue = random.Next(0, 101); // Gera um valor aleatório de 0 a 100
        oxygenGauge.UpdateValue(newValue);
        oxygenGraphicsView.Invalidate(); // Atualiza o gráfico
        return newValue;
    }

    private double TemperatureUpdate()
    {
        Random random = new Random();
        double newValue = random.Next(35, 42); // Gera um valor aleatório de 0 a 100
        thermometerGauge.UpdateValue(newValue);
        temperatureGraphicsView.Invalidate(); // Atualiza o gráfico
        return newValue;
    }

    private double OnGSRValueChanged()
    {
        Random random = new Random();
        double newValue = random.Next(0, 101); // Gera um valor aleatório de 0 a 100
        gsrWave.UpdateValue(newValue);
        gsrGraphicsView.Invalidate(); // Atualiza o gráfico
        return newValue;
    }
    #endregion

    private async void RandomUpdate()
    {
        double NewGSR = OnGSRValueChanged();
        double NewTemp = TemperatureUpdate();
        double NewOxy = OxygenUpdate();
        double NewBPM = HeartRateUpdate();

        //Load sample data
        var sampleData = new MLStress.ModelInput()
        { BPM = (float)NewBPM, SpO2 = (float)NewOxy, GSR = (float)NewGSR, Temp = (float)NewTemp };

        //Load model and predict output
        var result = MLStress.Predict(sampleData);

        lbStressWatch.Text = "Stress Watch: " + result.Score.ToString();
    }

    #region Navigations Buttons  

    private async void btnBPM_Clicked(object sender, EventArgs e)
    {
        await HideGraphics();

        heartRateGraphicsView.Opacity = 0;
        heartRateGraphicsView.TranslationY = 20;
        heartRateGraphicsView.IsVisible = true;

        await Task.WhenAll(
            heartRateGraphicsView.FadeTo(1, 300, Easing.CubicIn),
            heartRateGraphicsView.TranslateTo(0, 0, 300, Easing.CubicOut)
        );
    }

    private async void btnGSR_Clicked(object sender, EventArgs e)
    {
        await HideGraphics();

        gsrGraphicsView.Opacity = 0;
        gsrGraphicsView.TranslationY = 20;
        gsrGraphicsView.IsVisible = true;

        await Task.WhenAll(
            gsrGraphicsView.FadeTo(1, 300, Easing.CubicIn),
            gsrGraphicsView.TranslateTo(0, 0, 300, Easing.CubicOut)
        );
    }

    private async void btnOxygen_Clicked(object sender, EventArgs e)
    {
        await HideGraphics();

        oxygenGraphicsView.Opacity = 0;
        oxygenGraphicsView.TranslationY = 20;
        oxygenGraphicsView.IsVisible = true;

        await Task.WhenAll(
            oxygenGraphicsView.FadeTo(1, 300, Easing.CubicIn),
            oxygenGraphicsView.TranslateTo(0, 0, 300, Easing.CubicOut)
        );
    }

    private async void btnTemperature_Clicked(object sender, EventArgs e)
    {
        await HideGraphics();

        temperatureGraphicsView.Opacity = 0;
        temperatureGraphicsView.TranslationY = 20;
        temperatureGraphicsView.IsVisible = true;

        await Task.WhenAll(
            temperatureGraphicsView.FadeTo(1, 300, Easing.CubicIn),
            temperatureGraphicsView.TranslateTo(0, 0, 300, Easing.CubicOut)
        );
    }

    private async Task HideGraphics()
    {
        var fadeOutTasks = new List<Task>();

        if (heartRateGraphicsView.IsVisible)
            fadeOutTasks.Add(heartRateGraphicsView.FadeTo(0, 200, Easing.CubicOut));

        if (oxygenGraphicsView.IsVisible)
            fadeOutTasks.Add(oxygenGraphicsView.FadeTo(0, 200, Easing.CubicOut));

        if (temperatureGraphicsView.IsVisible)
            fadeOutTasks.Add(temperatureGraphicsView.FadeTo(0, 200, Easing.CubicOut));

        if (gsrGraphicsView.IsVisible)
            fadeOutTasks.Add(gsrGraphicsView.FadeTo(0, 200, Easing.CubicOut));

        await Task.WhenAll(fadeOutTasks);

        heartRateGraphicsView.IsVisible = false;
        oxygenGraphicsView.IsVisible = false;
        temperatureGraphicsView.IsVisible = false;
        gsrGraphicsView.IsVisible = false;
    }

    private async Task HideScrolls()
    {
        var fadeOutTasks = new List<Task>();

        if (ScrollMonitoring.IsVisible)
            fadeOutTasks.Add(ScrollMonitoring.FadeTo(0, 200, Easing.CubicOut));

        if (ScrollConection.IsVisible)
            fadeOutTasks.Add(ScrollConection.FadeTo(0, 200, Easing.CubicOut));

        await Task.WhenAll(fadeOutTasks);

        ScrollMonitoring.IsVisible = false;
        ScrollConection.IsVisible = false;
    }

    private async void btnConecao_Clicked(object sender, EventArgs e)
    {
        await HideScrolls();

        ScrollConection.Opacity = 0;
        ScrollConection.TranslationY = 20;
        ScrollConection.IsVisible = true;

        await Task.WhenAll(
            ScrollConection.FadeTo(1, 300, Easing.CubicIn),
            ScrollConection.TranslateTo(0, 0, 300, Easing.CubicOut)
        );
    }

    private async void btnBack_Clicked(object sender, EventArgs e)
    {
        await HideScrolls();

        ScrollMonitoring.Opacity = 0;
        ScrollMonitoring.TranslationY = 20;
        ScrollMonitoring.IsVisible = true;

        await Task.WhenAll(
            ScrollMonitoring.FadeTo(1, 300, Easing.CubicIn),
            ScrollMonitoring.TranslateTo(0, 0, 300, Easing.CubicOut)
        );
    }

    #endregion

    private void OnConnectClicked(object sender, EventArgs e)
    {
        string ip = ipEntry.Text;
        if (string.IsNullOrEmpty(ip))
        {
            ipEntry.Focus(); // Foca no campo email
            statusLabel.Text = "O endereço ip não pode ser inválido";
            return;
        }

        if (!int.TryParse(portEntry.Text, out int ports))
        {
            statusLabel.Text = "Porta inválida!";
            return;
        }

        Int16 port = Convert.ToInt16(portEntry.Text);

        try
        {
            Esp8266.Conected(ip, port);
            if (Esp8266.IsConected())
            {
                StartAnimations();
                statusLabel.Text = "Conectado!";
                btnDesconectar.IsVisible = true;
                btnConectar.IsVisible = false;
                ipEntry.IsEnabled = false;
                portEntry.IsEnabled = false;
                _ = Task.Run(ReadEsp8266DataAsync);
            }
            else
            {
                statusLabel.Text = "Ainda não estás conectado";
            }

        }
        catch (Exception ex)
        {
            statusLabel.Text = ex.Message;
        }
    }

    private async Task ReadEsp8266DataAsync()
    {
        while (Esp8266.IsConected())
        {
            try
            {
                string receber = Esp8266.Read()!;
                if (!string.IsNullOrEmpty(receber))
                {
                    string[] data = receber.Split('#');

                    // Atualiza o estado das lâmpadas na UI thread
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        UpdateStatus(data);
                    });
                }
            }
            catch (Exception ex)
            {
                break; // Termina o loop em caso de erro crítico
            }

            await Task.Delay(1000); // Aguarda um pouco antes da próxima leitura
        }
    }

    private async void UpdateStatus(string[] data)
    {
        if (data.Length >= 4)
        {
            try
            {
                float bpm = float.Parse(data[0]);
                double oxygen = double.Parse(data[1]);
                double temperature = double.Parse(data[2]);
                double gsr = double.Parse(data[3]);
                
                heartRateGraph.UpdateBPM(bpm);
                oxygenGauge.UpdateValue(oxygen);
                thermometerGauge.UpdateValue(temperature);
                gsrWave.UpdateValue(gsr);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }

            gsrGraphicsView.Invalidate();
            temperatureGraphicsView.Invalidate();
            oxygenGraphicsView.Invalidate();
            heartRateGraphicsView.Invalidate();
        }
    }

    private void OffConnectClicked(object sender, EventArgs e)
    {
        if (Esp8266.IsConected())
        {
            Esp8266.Desconected();
            btnDesconectar.IsVisible = false;
            btnConectar.IsVisible = true;
            statusLabel.Text = "Desconectado!";
            ipEntry.IsEnabled = true;
            portEntry.IsEnabled = true;
            StopAnimations();
        }
    }

    private void btnActualizar_Clicked(object sender, EventArgs e)
    {
        RandomUpdate();
    }
}