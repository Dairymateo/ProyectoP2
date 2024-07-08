using ProyectoP2.DataAccess;
using ProyectoP2.DTOs;
using ProyectoP2.Models;
using ProyectoP2.Utilities;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
 

namespace ProyectoP2.Views;

public partial class EscanearProductoPage : ContentPage
{
    private readonly VentaDbContext _context;
    public EscanearProductoPage(VentaDbContext context)
    {
        InitializeComponent();
        cameraView.BarCodeOptions = new Camera.MAUI.BarcodeDecodeOptions()
        {
            TryHarder = true,
            PossibleFormats = MapBarcodeFormats(new List<ZXing.BarcodeFormat> { ZXing.BarcodeFormat.All_1D }) 
        };
        _context = context;
    }

    private void cameraView_CamerasLoaded(object sender, EventArgs e)
    {
        if (cameraView.Cameras.Count > 0)
        {
            cameraView.Camera = cameraView.Cameras.First();
            MainThread.BeginInvokeOnMainThread(new Action(async () =>
            {

                await cameraView.StopCameraAsync();
                await cameraView.StartCameraAsync();
            }));
        }
    }

    private async void cameraView_BarcodeDetected(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            string codigo = args.Result[0].Text;
            Producto dbProducto = await _context.Productos.Include(c => c.RefCategoria).FirstOrDefaultAsync(p => p.Codigo == codigo);
            ProductoDTO producto = new ProductoDTO()
            {
                IdProducto = dbProducto.IdProducto,
                Codigo = dbProducto.Codigo,
                Nombre = dbProducto.Nombre,
                Categoria = new CategoriaDTO()
                {
                    IdCategoria = dbProducto.IdCategoria,
                    Nombre = dbProducto.RefCategoria.Nombre
                },
                Cantidad = dbProducto.Cantidad,
                Precio = (double)dbProducto.Precio
            };
            WeakReferenceMessenger.Default.Send(new ProductoVentaMessage(producto));
        });

        await Shell.Current.Navigation.PopModalAsync();
    }

    // M�todo para mapear formatos de ZXing a Camera.MAUI
    private List<Camera.MAUI.BarcodeFormat> MapBarcodeFormats(List<ZXing.BarcodeFormat> zxingFormats)
    {
        var cameraMauiFormats = new List<Camera.MAUI.BarcodeFormat>();
        foreach (var format in zxingFormats)
        {
            cameraMauiFormats.Add((Camera.MAUI.BarcodeFormat)Enum.Parse(typeof(Camera.MAUI.BarcodeFormat), format.ToString()));
        }
        return cameraMauiFormats;
    }
}