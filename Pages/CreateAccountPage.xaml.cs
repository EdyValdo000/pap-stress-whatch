using Microsoft.Maui.Controls;
using pap.Model;

namespace pap.Pages
{
    public partial class CreateAccountPage : ContentPage
    {
        public CreateAccountPage()
        {
            InitializeComponent();
        }

        private async void OnCreateAccountClicked(object sender, EventArgs e)
        {
            string name = nameEntry.Text;
            string password = passwordEntry.Text;
            string gender = genderPicker.SelectedItem.ToString()!; // Acessa o gênero selecionado

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(gender))
            {
                errorLabel.Text = "Por favor, preencha todos os campos.";
                errorLabel.IsVisible = true;
                return;
            }

            // Cria o usuário com os dados fornecidos
            var user = new User
            {
                Name = name,
                Password = password,
                Gender = gender
            };

            bool isValid = await App.UserService!.CheckCredentials(name, password);

            if (!isValid)
            {
                // Salva o usuário no banco de dados
                await App.UserService!.Save(user);

                // Redirecionar para a tela de login após a criação da conta
                await DisplayAlert("Sucesso", "Conta criada com sucesso!", "OK");
                await Navigation.PopModalAsync();
            }
            else
            {
                errorLabel.Text = "Essa conta já existe";
                errorLabel.IsVisible = true;
                return;
            }            
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            // Redirecionar para a tela de login
            await Navigation.PopModalAsync();
        }
    }
}