using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Data_Binding
{
    public partial class FormPage : ContentPage
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public event EventHandler<PersonaModel> PersonAdded;

        public FormPage()
        {
            InitializeComponent();
        }

        // guardar una nueva persona
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nombreEntry.Text) ||
                string.IsNullOrWhiteSpace(apellidoEntry.Text) ||
                sexoPicker.SelectedItem == null ||
                string.IsNullOrWhiteSpace(rolEntry.Text) ||
                !int.TryParse(rolEntry.Text, out int idRol))
            {
                await DisplayAlert("Error", "Todos los campos son obligatorios.", "OK");
                return;
            }

            var nuevaPersona = new PersonaModel
            {
                nombre = nombreEntry.Text,
                apellido = apellidoEntry.Text,
                sexo = sexoPicker.SelectedItem.ToString(),
                fh_nac = fechaNacimientoPicker.Date,
                id_rol = idRol
            };

            var json = JsonSerializer.Serialize(nuevaPersona);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("https://fi.jcaguilar.dev/v1/escuela/persona", content);

                if (response.IsSuccessStatusCode)
                {
                    //evento para actualizar la lista
                    PersonAdded?.Invoke(this, nuevaPersona);
                    await DisplayAlert("Éxito", "Persona agregada correctamente.", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo agregar la persona.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
            }
        }
    }
}
