using pap.Pages;
using Microsoft.Maui.Controls;
using pap.Model;

namespace pap.Pages
{
    public partial class LoginPage : ContentPage
    {
        private User User = new User();

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string name = nameEntry.Text;
            string password = passwordEntry.Text;

            // Simula��o de autentica��o
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password))
            {
                errorLabel.Text = "Por favor, preencha todos os campos.";
                errorLabel.IsVisible = true;
                return;
            }

            User.Name = name;
            User.Password = password;

            bool isValid = await App.UserService!.CheckCredentials(name, password);

            if (isValid)
            {
                this.User = await App.UserService!.GetUserByNameAndPassword(User.Name, User.Password);
                // Redirecionar para a tela principal ap�s o login
                await Navigation.PushModalAsync(new AndroidPage(User));
            }
            else 
            {
                errorLabel.Text = "Credenciais invalidas";
                errorLabel.IsVisible = true;
            }
            
        }

        private async void OnCreateAccountClicked(object sender, EventArgs e)
        {
            // Redirecionar para a tela de cria��o de conta
            await Navigation.PushModalAsync(new CreateAccountPage());
        }
    }
}