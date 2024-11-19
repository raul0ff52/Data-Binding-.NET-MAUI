using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Data_Binding
{
    public partial class MainPage : ContentPage
    {
        private readonly HttpClient _httpClient = new HttpClient();

        private ObservableCollection<PersonaModel> _personas;
        public ObservableCollection<PersonaModel> Personas
        {
            get => _personas;
            set
            {
                _personas = value;
                OnPropertyChanged();
            }
        }

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            GetData();
        }

        // Obtiene la lista inicial de personas 
        public async void GetData()
        {
            try
            {
                var response = await _httpClient.GetStringAsync("https://fi.jcaguilar.dev/v1/escuela/persona");
                var personas = JsonSerializer.Deserialize<ObservableCollection<PersonaModel>>(response);
                Personas = personas;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo obtener la lista de personas: {ex.Message}", "OK");
            }
        }

        // Abre la página de formulario para agregar una nueva persona
        private async void Button_Clicked(object sender, EventArgs e)
        {
            var formPage = new FormPage();
            formPage.PersonAdded += OnPersonAdded;
            await Navigation.PushAsync(formPage);
        }


        private void OnPersonAdded(object sender, PersonaModel nuevaPersona)
        {
            Personas.Add(nuevaPersona);
        }
    }
}
