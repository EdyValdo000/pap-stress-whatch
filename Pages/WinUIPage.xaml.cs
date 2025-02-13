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

        StartAnimations();
    }

    #region Animations graphics
    private void StartECGAnimation()
    {
        Device.StartTimer(TimeSpan.FromMilliseconds(50), () =>
        {
            heartRateGraphicsView.Invalidate(); // Redesenha a linha animada
            return true; // Continua executando
        });
    }

    private async void StartOxygenGaugeAnimation()
    {
        while (true)
        {
            oxygenGraphicsView.Invalidate(); // Atualiza o desenho
            await Task.Delay(50); // Atualiza a animação suavemente
        }
    }

    private void StartGSRAnimation()
    {
        Device.StartTimer(TimeSpan.FromMilliseconds(50), () =>
        {
            gsrGraphicsView.Invalidate(); // Redesenha a linha animada
            return true; // Continua executando
        });
    }
    
    private void StartAnimations()
    {        
        StartGSRAnimation();
        StartECGAnimation();
        StartOxygenGaugeAnimation();
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

            await Task.Delay(500); // Aguarda um pouco antes da próxima leitura
        }
    }

    // Atualiza o status das lâmpadas na interface do usuário
    private void UpdateStatus(string[] data)
    {
        if (data.Length >= 4)
        {
            heartRateGraph.UpdateBPM(float.Parse(data[0]));
            oxygenGauge.UpdateValue(double.Parse(data[1]));
            thermometerGauge.UpdateValue(double.Parse(data[2]));
            gsrWave.UpdateValue(double.Parse(data[3]));

            gsrGraphicsView.Invalidate();
            temperatureGraphicsView.Invalidate();
            oxygenGraphicsView.Invalidate();
            heartRateGraphicsView.Invalidate();
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
}