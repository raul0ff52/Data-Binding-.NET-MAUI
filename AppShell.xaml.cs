namespace Data_Binding
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();


            //Registra Las rutas de navegacion
            Routing.RegisterRoute(nameof(FormPage), typeof(FormPage));
            
        }
    }
}
