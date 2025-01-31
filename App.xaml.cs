using pap.Database;
namespace pap;
public partial class App : Application
{
    public static ConectionDatabase? ConectionDatabase { get; private set; }
    public App()
    {
        InitializeComponent();
        ConectionDatabase = new ConectionDatabase();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}