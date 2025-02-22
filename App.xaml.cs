using pap.Pages;
using pap.Repositore;
using pap.Service;

namespace pap;
public partial class App : Application
{
    public static UserService? UserService { get; set; }
    public static SensorDataService? SensorDataService { get; set; }

    public App()
    {
        InitializeComponent();

        UserService = new UserService(new UserRepository());
        SensorDataService = new SensorDataService(new SensorDataRepository());       
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        Page startPage;

        if (DeviceInfo.Current.Platform == DevicePlatform.Android)
        {
            startPage = new LoginPage();
        }
        else if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
        {
            startPage = new WinUIPage();
        }
        else
        {
            startPage = new LoginPage(); // Página padrão (pode ser alterada)
        }

        return new Window(startPage);
    }
}