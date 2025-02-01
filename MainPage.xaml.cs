using pap.Database;
namespace pap;
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }
    
    private void ShowMonitoring(object sender, EventArgs e)
    {
        MonitoringLayout.IsVisible = true;
        SettingsLayout.IsVisible = false;
        AboutLayout.IsVisible = false;
    }

    private void ShowSettings(object sender, EventArgs e)
    {
        MonitoringLayout.IsVisible = false;
        SettingsLayout.IsVisible = true;
        AboutLayout.IsVisible = false;
    }

    private void ShowAbout(object sender, EventArgs e)
    {
        MonitoringLayout.IsVisible = false;
        SettingsLayout.IsVisible = false;
        AboutLayout.IsVisible = true;
    }

    private async void OnButtonClicked(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            await button.ScaleTo(0.98, 100); // Animação ao pressionar
            await button.ScaleTo(1, 100);    // Retorna ao tamanho normal
        }

        try
        {
            var user = new Users
            {
                Name = "NameEntry.Text",
                Email = "EmailEntry.Text",
                Password = "PasswordEntry.Text"
            };
            await App.ConectionDatabase.SaveUser(user);

            await DisplayAlert("Success", "User saved successfully", "Ok");
        }
        catch (Exception)
        {
            await DisplayAlert("Error", "Error saving user", "Ok");
            throw;
        }
    }

    private async void ToggleChat(object sender, EventArgs e)
    {
        if (ChatFrame.IsVisible)
        {
            await ChatFrame.FadeTo(0, 250);
            ChatFrame.IsVisible = false;
        }
        else
        {
            ChatFrame.IsVisible = true;
            await ChatFrame.TranslateTo(0, -50, 250); // Desliza para cima
            await ChatFrame.FadeTo(1, 250);
        }
    }

    private void SendChatMessage(object sender, EventArgs e)
    {

    }
}