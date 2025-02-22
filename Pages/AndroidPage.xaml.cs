using pap.Conection;
using pap.Graphics;
using pap.Model;
using StressWatchML;

namespace pap.Pages;
public partial class AndroidPage : ContentPage
{
    private HeartRateGraph heartRateGraph = new HeartRateGraph();
    private OxygenGauge oxygenGauge = new OxygenGauge();
    private ThermometerGauge thermometerGauge = new ThermometerGauge();
    private GsrWave gsrWave = new GsrWave();

    private float _BPM, _Oxy, _GSR, _Temp;

    private readonly WifiConection Esp8266 = new();

    private User User;

    public AndroidPage(User User)
    {
        InitializeComponent();

        heartRateGraphicsView.Drawable = heartRateGraph;
        oxygenGraphicsView.Drawable = oxygenGauge;
        temperatureGraphicsView.Drawable = thermometerGauge;
        gsrGraphicsView.Drawable = gsrWave;

        this.User = User;
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

    private void RandomUpdate()
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

        //Load sample data
        var sampleDataStressLevel = new MLStressInsides.ModelInput()
        {
            BPM = result.BPM,
            SpO2 = result.SpO2,
            GSR = result.GSR, 
            Temp = result.Temp,
            Nível_de_Estresse = result.Score
        };

        //Load model and predict output
        var resultRecomendations = MLStressInsides.Predict(sampleDataStressLevel);

        lbRecomendations.Text = resultRecomendations.PredictedLabel;
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

        if (ScrollRecommendations.IsVisible)
            fadeOutTasks.Add(ScrollRecommendations.FadeTo(0, 200, Easing.CubicOut));

        await Task.WhenAll(fadeOutTasks);

        ScrollMonitoring.IsVisible = false;
        ScrollConection.IsVisible = false;
        ScrollRecommendations.IsVisible = false;
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

    private async void btnRecommendations_Clicked(object sender, EventArgs e)
    {
        //Load sample data
        var sampleData = new MLStress.ModelInput()
        { BPM = _BPM, SpO2 = _Oxy, GSR = _GSR, Temp = _Temp };

        //Load model and predict output
        var result = MLStress.Predict(sampleData);

        //Load sample data
        var sampleDataStressLevel = new MLStressInsides.ModelInput()
        {
            BPM = result.BPM,
            SpO2 = result.SpO2,
            GSR = result.GSR,
            Temp = result.Temp,
            Nível_de_Estresse = result.Score
        };

        //Load model and predict output
        var resultRecomendations = MLStressInsides.Predict(sampleDataStressLevel);

        lbRecomendations.Text = resultRecomendations.PredictedLabel;

        var guardar = new SensorData
        {
            BPM = resultRecomendations.BPM,
            GSR = resultRecomendations.GSR,
            SpO2 = resultRecomendations.SpO2,
            Temperature = resultRecomendations.Temp,
            StressLevel = resultRecomendations.Nível_de_Estresse,
            UserId = User.Id,
            Advice = resultRecomendations.PredictedLabel,
            Timestamp = DateTime.UtcNow
        };

        await App.SensorDataService!.Save(guardar);

        await HideScrolls();

        ScrollRecommendations.Opacity = 0;
        ScrollRecommendations.TranslationY = 20;
        ScrollRecommendations.IsVisible = true;

        await Task.WhenAll(
            ScrollRecommendations.FadeTo(1, 300, Easing.CubicIn),
            ScrollRecommendations.TranslateTo(0, 0, 300, Easing.CubicOut)
        );
    }

    private async void btnBackRecommendations_Clicked(object sender, EventArgs e)
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

    #region Conexão
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

                _BPM = bpm;
                _Oxy = (float)oxygen;
                _GSR = (float)gsr;
                _Temp = (float)temperature;
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
    #endregion

    private void btnActualizar_Clicked(object sender, EventArgs e)
    {
        RandomUpdate();
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        // Redirecionar para a tela de login
        await Navigation.PopModalAsync();
    }

    private async void OnViewHistoryClicked(object sender, EventArgs e)
    {        
        // Redirecionar para a tela de histórico
        await Navigation.PushModalAsync(new HistoryPage(User));
    }
}