using pap.Database;
namespace pap;
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        var users = App.ConectionDatabase!.GetUsersAsync().Result;
        Lista.ItemsSource = users;
    }

    private void CommonButtonClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is string parameter)
        {
            SendChatMessage();
            // Oculta todos os layouts de ambas as plataformas
            MonitoringLayout.IsVisible = false;
            SettingsLayout.IsVisible = false;
            AboutLayout.IsVisible = false;

            MonitoringLayoutWin.IsVisible = false;
            SettingsLayoutWin.IsVisible = false;
            AboutLayoutWin.IsVisible = false;

            // Verifica a plataforma atual
            bool isWindows = DeviceInfo.Platform == DevicePlatform.WinUI;

            switch (parameter)
            {
                case "monitoring":
                    if (isWindows)
                        MonitoringLayoutWin.IsVisible = true;
                    else
                        MonitoringLayout.IsVisible = true;
                    break;

                case "settings":
                    if (isWindows)
                        SettingsLayoutWin.IsVisible = true;
                    else
                        SettingsLayout.IsVisible = true;
                    break;

                case "about":
                    if (isWindows)
                        AboutLayoutWin.IsVisible = true;
                    else
                        AboutLayout.IsVisible = true;
                    break;

                case "toggleChat":
                    ChatFrame.IsVisible = !ChatFrame.IsVisible;
                    //ChatFrame.Opacity = ChatFrame.IsVisible ? 1 : 0;
                    break;

                case "sendChat":
                    // Aqui você pode tratar o envio da mensagem do chat
                    SendChatMessage();
                    break;

                default:
                    break;
            }
        }
    }

    private async void SendChatMessage()
    {
        // Implementação do envio da mensagem do chat
        await DisplayAlert("Chat", "Mensagem enviada!", "OK");
        
        var user = new Users
        {
            Name = await DisplayPromptAsync("Nome","Digite o seu nome", "Ok", "Cancelar"),
            Email = await DisplayPromptAsync("Email", "Digite o seu email", "Ok", "Cancelar"),
            Password = await DisplayPromptAsync("Senha", "Digite a sua senha", "Ok", "Cancelar")
        };

        await App.ConectionDatabase!.SaveUser(user);
        await DisplayAlert("Base de dados", "Novo usuário!", "OK");

        var users = App.ConectionDatabase!.GetUsersAsync().Result;
        Lista.ItemsSource = users;
    }
}