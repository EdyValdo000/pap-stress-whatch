using pap.Pages;
using pap.Repositore;
using pap.Service;

namespace pap;
public partial class App : Application
{
    public static UserService? UserService { get; private set; }
    public static SensorDataService? SensorDataService { get; private set; }
    public static MonitoringSessionService? MonitoringSessionService { get; private set; }
    public static AIRecommendationService? AIRecommendationService { get; private set; }

    public App()
    {
        InitializeComponent();

        UserService = new UserService(new UserRepository());
        SensorDataService = new SensorDataService(new HeartOximeterDataRepository(), new TemperatureDataRepository(), new GSRDataRepository());
        MonitoringSessionService = new MonitoringSessionService(new MonitoringSessionRepository());
        AIRecommendationService = new AIRecommendationService(new AIRecommendationRepository());
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        Page startPage;

        if (DeviceInfo.Current.Platform == DevicePlatform.Android)
        {
            startPage = new AndroidPage();
        }
        else if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
        {
            startPage = new WinUIPage();
        }
        else
        {
            startPage = new AndroidPage(); // Página padrão (pode ser alterada)
        }

        return new Window(startPage);
    }
}