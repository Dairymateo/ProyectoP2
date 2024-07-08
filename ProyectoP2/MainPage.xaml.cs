using ProyectoP2.ViewModels;
using CommunityToolkit.Mvvm.Messaging;

namespace ProyectoP2
{
    public partial class MainPage : ContentPage
    {
        private MainVM _viewModel;

        public MainPage(MainVM vm)
        {
            InitializeComponent();
            BindingContext = vm;
            _viewModel = vm;

        }

        //Para actualizar la View
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_viewModel != null)
            {
                await _viewModel.ObtenerResumen();
            }
        }
    }
}
