using Microsoft.Maui;
using pap.Conection;
using pap.Graphics;
using System.Security.Cryptography;

namespace pap.Pages;

public partial class WinUIPage : ContentPage
{
    private HeartRateGraph heartRateGraph = new HeartRateGraph();
    private OxygenGauge oxygenGauge = new OxygenGauge();
    private ThermometerGauge thermometerGauge = new ThermometerGauge();
    private GsrWave gsrWave = new GsrWave();

    private readonly WifiConection Esp8266 = new();

    public WinUIPage()
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
    private void HeartRateUpdate()
    {
        Random rnd = new Random();
        float newBPM = rnd.Next(60, 120); // Gera um BPM aleatório entre 60 e 120
        heartRateGraph.UpdateBPM(newBPM); // Atualiza apenas o BPM
        heartRateGraphicsView.Invalidate(); // Redesenha sem reiniciar a onda
    }

    private void OxygenUpdate()
    {
        Random random = new Random();
        double newValue = random.Next(0, 101); // Gera um valor aleatório de 0 a 100
        oxygenGauge.UpdateValue(newValue);
        oxygenGraphicsView.Invalidate(); // Atualiza o gráfico
    }

    private void TemperatureUpdate()
    {
        Random random = new Random();
        double newValue = random.Next(35, 42); // Gera um valor aleatório de 0 a 100
        thermometerGauge.UpdateValue(newValue);
        temperatureGraphicsView.Invalidate(); // Atualiza o gráfico
    }

    private void OnGSRValueChanged()
    {
        Random random = new Random();
        double newValue = random.Next(0, 101); // Gera um valor aleatório de 0 a 100
        gsrWave.UpdateValue(newValue);
        gsrGraphicsView.Invalidate(); // Atualiza o gráfico
    }
    #endregion


    private void UpdateAll(object sender, EventArgs e)
    {
        RandomUpdate();       
    }

    private void RandomUpdate()
    {
        OnGSRValueChanged();
        TemperatureUpdate();
        OxygenUpdate();
        HeartRateUpdate();
    }

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
                    
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        UpdateStatus(data);
                    });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
            finally
            {
                await Task.Delay(300); // Aguarda um pouco antes da próxima leitura
            }
        }
    }

    private void UpdateStatus(string[] data)
    {
        if (data.Length >= 4)
        {
            try
            {       
                float bpm = float.Parse(data[0]);
                double oxygen = double.Parse(data[1]);
                double temperature = double.Parse(data[2]);
                double gsr = double.Parse(data[3]);

                //heartRateGraph.UpdateBPM(bpm);
                
                oxygenGauge.UpdateValue(oxygen);
                thermometerGauge.UpdateValue(temperature);
                gsrWave.UpdateValue(gsr);                
            }
            catch (Exception ex)
            {
                DisplayAlert("Erro", ex.Message, "OK");
            }
            finally
            {
                gsrGraphicsView.Invalidate();
                temperatureGraphicsView.Invalidate();
                oxygenGraphicsView.Invalidate();
                heartRateGraphicsView.Invalidate();
            }
        }
    }

    #region Navigations Buttons
    private async void Settings_Clicked(object sender, EventArgs e)
    {
        await HideViews();

        ScrollConection.Opacity = 0;
        ScrollConection.TranslationY = 20;
        ScrollConection.IsVisible = true;

        await Task.WhenAll(
            ScrollConection.FadeTo(1, 300, Easing.CubicIn),
            ScrollConection.TranslateTo(0, 0, 300, Easing.CubicOut)
        );
    }

    private async void Multi_Monitor_Clicked(object sender, EventArgs e)
    {
        await HideViews();

        StackOxyAndTemp.Opacity = 0;
        StackMonitoring.Opacity = 0;
        StackOxyAndTemp.TranslationY = 20;
        StackMonitoring.TranslationY = 20;

        StackOxyAndTemp.IsVisible = true;
        StackMonitoring.IsVisible = true;

        await Task.WhenAll(
            StackOxyAndTemp.FadeTo(1, 300, Easing.CubicIn),
            StackOxyAndTemp.TranslateTo(0, 0, 300, Easing.CubicOut),
            StackMonitoring.FadeTo(1, 300, Easing.CubicIn),
            StackMonitoring.TranslateTo(0, 0, 300, Easing.CubicOut)
        );
    }

    private async void Welcome_Clicked(object sender, EventArgs e)
    {
        await HideViews();

        ScrollWelcome.Opacity = 0;
        ScrollWelcome.TranslationY = 20;
        ScrollWelcome.IsVisible = true;

        await Task.WhenAll(
            ScrollWelcome.FadeTo(1, 300, Easing.CubicIn),
            ScrollWelcome.TranslateTo(0, 0, 300, Easing.CubicOut)
        );
    }

    private async Task HideViews()
    {
        var fadeOutTasks = new List<Task>();

        if (ScrollConection.IsVisible)
            fadeOutTasks.Add(ScrollConection.FadeTo(0, 200, Easing.CubicOut));

        if (StackOxyAndTemp.IsVisible)
            fadeOutTasks.Add(StackOxyAndTemp.FadeTo(0, 200, Easing.CubicOut));

        if (StackMonitoring.IsVisible)
            fadeOutTasks.Add(StackMonitoring.FadeTo(0, 200, Easing.CubicOut));

        if (ScrollWelcome.IsVisible)
            fadeOutTasks.Add(ScrollWelcome.FadeTo(0, 200, Easing.CubicOut));

        await Task.WhenAll(fadeOutTasks);

        ScrollConection.IsVisible = false;
        StackOxyAndTemp.IsVisible = false;
        StackMonitoring.IsVisible = false;
        ScrollWelcome.IsVisible = false;
    }
    #endregion

    private void Menu_Clicked(object sender, EventArgs e)
    {

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
}