using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using ProyectoP2.DataAccess;
using ProyectoP2.DTOs;
using ProyectoP2.Models;
using ProyectoP2.Services;
using ProyectoP2.Utilities;
using ProyectoP2.Views;

namespace ProyectoP2.ViewModels
{
    public partial class InventarioVM : ObservableObject
    {
        private readonly VentaDbContext _context;
        private readonly BookServices _bookServices;

        public InventarioVM(VentaDbContext context, BookServices bookServices)
        {
            _context = context;
            _bookServices = bookServices;
            WeakReferenceMessenger.Default.Register<ProductoMessage>(this, (r, m) =>
            {
                ProductoMensajeRecibido(m.Value);
            });

            // Inicializar tareas asincrónicas en lugar de hacerlo en el constructor
            Inicializar();
        }

        private async void Inicializar()
        {
            await ObtenerCategorias();
            await ObtenerProductos();
        }

        private void InventarioVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BuscarProducto))
            {
                BtnLimpiarEsVisible = !string.IsNullOrEmpty(BuscarProducto);
            }
        }

        [ObservableProperty]
        private CategoriaDTO categoriaSeleccionada;

        [ObservableProperty]
        private ObservableCollection<CategoriaDTO> listaCategoria = new ObservableCollection<CategoriaDTO>();

        [ObservableProperty]
        private string buscarProducto = string.Empty;

        [ObservableProperty]
        private bool loadingEsVisible = false;

        [ObservableProperty]
        private bool loadingCategoriaEsVisible = false;

        [ObservableProperty]
        private bool dataEsVisible = false;

        [ObservableProperty]
        private bool btnLimpiarEsVisible = false;

        [ObservableProperty]
        private ObservableCollection<ProductoDTO> listaProductos = new ObservableCollection<ProductoDTO>();

        private async Task ObtenerCategorias()
        {
            LoadingCategoriaEsVisible = true;
            try
            {
                var lstCategoria = await _context.Categorias.ToListAsync();
                var lstTemp = new ObservableCollection<CategoriaDTO>();
                var categoriaDefault = new CategoriaDTO { IdCategoria = 0, Nombre = "Todas las categorias" };
                lstTemp.Add(categoriaDefault);
                foreach (var item in lstCategoria)
                {
                    lstTemp.Add(new CategoriaDTO
                    {
                        IdCategoria = item.IdCategoria,
                        Nombre = item.Nombre
                    });
                }

                ListaCategoria = lstTemp;
                CategoriaSeleccionada = categoriaDefault;
            }
            finally
            {
                LoadingCategoriaEsVisible = false;
            }
        }

        public async Task ObtenerProductos()
        {
            DataEsVisible = false;
            LoadingEsVisible = true;
            try
            {
                var lstProductos = await _context.Productos.Include(c => c.RefCategoria).ToListAsync();
                var lstTemp = new ObservableCollection<ProductoDTO>();

                foreach (var item in lstProductos)
                {
                    lstTemp.Add(new ProductoDTO
                    {
                        IdProducto = item.IdProducto,
                        Codigo = item.Codigo,
                        Nombre = item.Nombre,
                        Categoria = new CategoriaDTO() { IdCategoria = item.RefCategoria.IdCategoria, Nombre = item.RefCategoria.Nombre },
                        Cantidad = item.Cantidad,
                        Precio = Convert.ToDouble(item.Precio)
                    });
                }

                ListaProductos = lstTemp;
                DataEsVisible = true;
            }
            finally
            {
                LoadingEsVisible = false;
            }
        }

        private RelayCommand _actualizarCommand;
        public RelayCommand ActualizarCommand => _actualizarCommand ??= new RelayCommand(async () => await Actualizar());

        public async Task Actualizar()
        {
             Inicializar();
        }


        [RelayCommand]
        private async Task Buscar()
        {
            DataEsVisible = false;
            LoadingEsVisible = true;

            try
            {
                ObservableCollection<ProductoDTO> encontrados = new ObservableCollection<ProductoDTO>();
                List<Producto> bdListCategorias = new List<Producto>();

                if (CategoriaSeleccionada.IdCategoria == 0)
                    bdListCategorias = await _context.Productos.Where(p => p.Nombre.ToLower().Contains(BuscarProducto.ToLower())).ToListAsync();
                else
                    bdListCategorias = await _context.Productos.Where(p => p.Nombre.ToLower().Contains(BuscarProducto.ToLower()) && p.IdCategoria == CategoriaSeleccionada.IdCategoria).ToListAsync();

                foreach (var item in bdListCategorias)
                {
                    encontrados.Add(new ProductoDTO
                    {
                        IdProducto = item.IdProducto,
                        Codigo = item.Codigo,
                        Nombre = item.Nombre,
                        Categoria = new CategoriaDTO { IdCategoria = item.RefCategoria.IdCategoria, Nombre = item.RefCategoria.Nombre },
                        Cantidad = item.Cantidad,
                        Precio = (double)item.Precio
                    });
                }

                ListaProductos = encontrados;
                DataEsVisible = true;
            }
            finally
            {
                LoadingEsVisible = false;
            }
        }

        [RelayCommand]
        private async Task Limpiar()
        {
            DataEsVisible = false;
            LoadingEsVisible = true;

            try
            {
                BuscarProducto = "";
                await ObtenerProductos();
                DataEsVisible = true;
            }
            finally
            {
                LoadingEsVisible = false;
            }
        }

        [RelayCommand]
        private async Task IrProducto()
        {
            await Shell.Current.Navigation.PushModalAsync(new ProductoPage(new ProductoVM(_context, _bookServices), 0));
        }

        private void ProductoMensajeRecibido(ProductoResult result)
        {
            if (result.esCrear)
            {
                ListaProductos.Add(result.producto);
            }
            else
            {
                var prod = ListaProductos.First(p => p.IdProducto == result.producto.IdProducto);

                prod.Codigo = result.producto.Codigo;
                prod.Nombre = result.producto.Nombre;
                prod.Categoria = result.producto.Categoria;
                prod.Cantidad = result.producto.Cantidad;
                prod.Precio = result.producto.Precio;
            }
        }

        [RelayCommand]
        private async Task Editar(ProductoDTO producto)
        {
            await Shell.Current.Navigation.PushModalAsync(new ProductoPage(new ProductoVM(_context, _bookServices), producto.IdProducto));
        }

        [RelayCommand]
        private async Task Eliminar(ProductoDTO producto)
        {
            bool answer = await Shell.Current.DisplayAlert("Mensaje", "Desea eliminar el producto?", "Si, continuar", "No, volver");
            if (answer)
            {
                LoadingEsVisible = true;

                try
                {
                    var prod = await _context.Productos.FirstAsync(p => p.IdProducto == producto.IdProducto);
                    _context.Productos.Remove(prod);
                    await _context.SaveChangesAsync();
                    ListaProductos.Remove(producto);
                }
                finally
                {
                    LoadingEsVisible = false;
                }
            }
        }
    }
}
