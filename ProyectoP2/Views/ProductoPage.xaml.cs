using ProyectoP2.ViewModels;

namespace ProyectoP2.Views;

public partial class ProductoPage : ContentPage
{
    private readonly ProductoVM viewModel;
    private readonly int _idProducto;
    public ProductoPage(ProductoVM vm, int idProducto)
    {
        InitializeComponent();
        BindingContext = vm;
        viewModel = vm;
        _idProducto = idProducto;

    }

    protected override void OnAppearing()
    {

        viewModel.Inicio(_idProducto);
    }

    //protected override void OnDisappearing()
    //{
    //    viewModel.Desuscribir();
    //}
}