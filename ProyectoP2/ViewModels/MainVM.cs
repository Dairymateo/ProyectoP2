using ProyectoP2.DataAccess;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoP2.ViewModels
{
    public partial class MainVM : ObservableObject
    {
        private readonly VentaDbContext _context;
        public MainVM(VentaDbContext context)
        {
            _context = context;

            Task.Run(async () => await ObtenerResumen());
        }

        [ObservableProperty]
        private double totalIngresos;

        [ObservableProperty]
        private int totalVentas;

        [ObservableProperty]
        private int totalProductos;

        [ObservableProperty]
        private int totalCategorias;

        public async Task ObtenerResumen()
        {
            try
            {
                double totalIngresos = 0;

                // Obtener todas las ventas de manera asincrónica
                var lstVentas = await _context.Ventas.ToListAsync();

                // Calcular total de ingresos
                foreach (var item in lstVentas)
                {
                    totalIngresos += item.Total;
                }

                // Actualizar propiedades observables
                TotalIngresos = totalIngresos;
                TotalVentas = await _context.Ventas.CountAsync(); // CountAsync() es preferible a Count() para operaciones asincrónicas
                TotalProductos = await _context.Productos.CountAsync();
                TotalCategorias = await _context.Categorias.CountAsync();

                // Notificar cambios en las propiedades observables
                OnPropertyChanged(nameof(TotalIngresos));
                OnPropertyChanged(nameof(TotalVentas));
                OnPropertyChanged(nameof(TotalProductos));
                OnPropertyChanged(nameof(TotalCategorias));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el resumen: {ex.Message}");
                // Puedes manejar la excepción según sea necesario
            }
        }


    }
}
