using System.Collections.Generic;
using Microsoft.Maui.Controls;
using pap.Model;
using pap.Service;

namespace pap.Pages
{
    public partial class HistoryPage : ContentPage
    {
        private User User;

        public HistoryPage(User user)
        {
            InitializeComponent();
            this.User = user;

            LoadHistoryData();
        }

        // Carrega os dados de histórico do usuário
        private async void LoadHistoryData()
        {
            // Obtém todos os SensorData do usuário
            var sensorDataList = await App.SensorDataService!.GetSensorDataByUserId(User.Id);

            // Converte os dados para o formato exibido na tela
            var historyData = new List<HistoryItem>();
            foreach (var data in sensorDataList)
            {
                historyData.Add(new HistoryItem
                {
                    Date = data.Timestamp.ToString("dd/MM/yyyy HH:mm"), // Formata a data e hora
                    Description = $"Nível de estresse: {data.StressLevel} - {data.Advice}"
                });
            }

            // Atualiza a CollectionView com os dados
            historyCollectionView.ItemsSource = historyData;
        }

        // Evento do botão Voltar
        private async void btnBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync(); // Volta para a tela anterior
        }
    }

    public class HistoryItem
    {
        public string Date { get; set; }
        public string Description { get; set; }
    }
}