using pap.Model;
namespace pap;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();        
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CarregarUsers();
    }

    private void CarregarUsers()
    {
        var users = App.UserService!.GetAll().Result;
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
        
        var user = new User
        {
            Name = await DisplayPromptAsync("Nome","Digite o seu nome", "Ok", "Cancelar"),
            Gender = await DisplayActionSheet("Gênero", "Cancelar", null, "Masculino", "Feminino", "Outro"),
            Age = Convert.ToInt32(await DisplayPromptAsync("Idade", "Digite a sua idade", "Ok", "Cancelar"))
        };

        await App.UserService!.Save(user);
        await DisplayAlert("Base de dados", "Novo usuário!", "OK");

        var users = App.UserService!.GetAll().Result;
        Lista.ItemsSource = users;
    }
}