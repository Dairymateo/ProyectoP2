using System.Drawing;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using ProyectoP2.DataAccess;
using ProyectoP2.DTOs;
using ProyectoP2.Models;
using ProyectoP2.Utilities;
using ProyectoP2.Views;
using Microsoft.EntityFrameworkCore;
using ProyectoP2.Services;

namespace ProyectoP2.ViewModels
{
    public partial class VentaVM : ObservableObject
    {
        private readonly VentaDbContext _context;
        private readonly BookServices _bookServices;
        public VentaVM(VentaDbContext context, BookServices bookServices)
        {
            WeakReferenceMessenger.Default.Register<ProductoVentaMessage>(this, (r, m) =>
            {
                ProductoMensajeRecibido(m.Value);
            });
            _context = context;
            _bookServices = bookServices; 

            PropertyChanged += VentaVM_PropertyChanged;
        }

        private void VentaVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PagoCon))
            {
                if (PagoCon - Total >= 0)
                {
                    Cambio = PagoCon - Total;
                    CambioColor = System.Drawing.Color.Black;
                }
                else
                {
                    Cambio = 0;
                    CambioColor = System.Drawing.Color.Red;
                }
            }
        }

        [ObservableProperty]
        private ObservableCollection<DetalleVentaDTO> detalleVenta = new ObservableCollection<DetalleVentaDTO>();

        [ObservableProperty]
        private double total;

        [ObservableProperty]
        private double pagoCon;

        [ObservableProperty]
        private double cambio;

        [ObservableProperty]
        private System.Drawing.Color cambioColor;

        [RelayCommand]
        private async Task TapBuscar()
        {
            await Shell.Current.Navigation.PushModalAsync(new BuscarProductoPage(new BuscarProductoVM(_context, _bookServices)));
        }

        [RelayCommand]
        private async Task TapEscanear()
        {
            await Shell.Current.Navigation.PushModalAsync(new EscanearProductoPage(new VentaDbContext()));
        }

        private void ProductoMensajeRecibido(ProductoDTO result)
        {
            var encontrado = DetalleVenta.FirstOrDefault(dv => dv.Producto.IdProducto == result.IdProducto);
            if (encontrado == null)
            {
                DetalleVenta.Add(new DetalleVentaDTO
                {
                    Producto = result,
                    Cantidad = 1,
                    Total = 1 * result.Precio
                });
                MostarTotal();
            }

        }


        [RelayCommand]
        private void DisminuirEvent(DetalleVentaDTO detalle)
        {
            if (detalle.Cantidad - 1 >= 1)
            {
                detalle.Cantidad -= 1;
                detalle.Total = detalle.Producto.Precio * detalle.Cantidad;
                MostarTotal();
            }

        }

        [RelayCommand]
        private void AumentarEvent(DetalleVentaDTO detalle)
        {
            detalle.Cantidad += 1;
            detalle.Total = detalle.Producto.Precio * detalle.Cantidad;
            MostarTotal();
        }

        [RelayCommand]
        private void EliminarEvent(DetalleVentaDTO detalle)
        {
            DetalleVenta.Remove(detalle);
            MostarTotal();
        }

        public void MostarTotal()
        {
            Total = DetalleVenta.Sum(c => c.Total);
        }


        [RelayCommand]
        private async Task FinalizarVenta()
        {
            if (PagoCon == 0)
            {
                await Shell.Current.DisplayAlert("Mensaje", "Debe ingresar el pago", "Aceptar");
                return;
            }

            if (PagoCon - Total < 0)
            {
                await Shell.Current.DisplayAlert("Mensaje", "Pago insuficiente", "Aceptar");
                return;
            }

            if (DetalleVenta.Count < 1)
            {
                await Shell.Current.DisplayAlert("Mensaje", "No existen productos para la venta", "Aceptar");
                return;
            }

            // Verificar el inventario antes de registrar la venta
            var productosId = DetalleVenta.Select(dv => dv.Producto.IdProducto).Distinct().ToList();
            var productosDisponibles = await _context.Productos.Where(p => productosId.Contains(p.IdProducto)).ToListAsync();

            foreach (var item in DetalleVenta)
            {
                var productoDisponible = productosDisponibles.FirstOrDefault(p => p.IdProducto == item.Producto.IdProducto);
                if (productoDisponible == null || productoDisponible.Cantidad < item.Cantidad)
                {
                    await Shell.Current.DisplayAlert("Error", $"No hay suficiente stock de {item.Producto.Nombre}. Cantidad disponible: {productoDisponible?.Cantidad ?? 0}", "Aceptar");
                    return; // Detener la venta si no hay suficiente stock
                }
            }

            string nombreCliente = await Shell.Current.DisplayPromptAsync("Información del cliente", "Nombres:", accept: "Continuar", cancel: "Volver", placeholder: "(opcional)");

            Venta venta = null; // Declarar venta fuera del bloque using

            try
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    //Registrar la venta
                    List<DetalleVenta> detalleVentas = new List<DetalleVenta>();
                    foreach (var item in DetalleVenta)
                    {
                        detalleVentas.Add(new DetalleVenta
                        {
                            IdProducto = item.Producto.IdProducto,
                            Cantidad = item.Cantidad,
                            Total = item.Total
                        });

                        // Actualizar inventario
                        var productoDisponible = productosDisponibles.First(p => p.IdProducto == item.Producto.IdProducto);
                        productoDisponible.Cantidad -= item.Cantidad;
                        _context.Productos.Update(productoDisponible);
                    }

                    venta = new Venta()
                    {
                        Cliente = nombreCliente,
                        NumeroVenta = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper(),
                        Total = Total,
                        PagoCon = PagoCon,
                        Cambio = Cambio,
                        FechaRegistro = DateTime.Now,
                        RefDetalleVenta = detalleVentas
                    };

                    _context.Ventas.Add(venta);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }

                // Mostrar el número de venta después de registrar la venta y confirmar la transacción
                await Shell.Current.DisplayAlert("Listo!", $"El número de venta '{venta.NumeroVenta}' fue generada.", "Aceptar");

                DetalleVenta.Clear();
                PagoCon = 0;
                Cambio = 0;
                MostarTotal();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error!", $"No se pudo registrar la venta: {ex.Message}", "Aceptar");
            }
        }





    }
}
