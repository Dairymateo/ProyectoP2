using Newtonsoft.Json;
using ProyectoP2.Models;
using ProyectoP2.DataAccess;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;

namespace ProyectoP2.Services
{
    public class BookServices
    {
        private readonly VentaDbContext _context;
        private readonly ILogger<BookServices> _logger;

        public BookServices(VentaDbContext context, ILogger<BookServices> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task GuardarLibroAsync()
        {
            try
            {
                var producto = await DevuelveProductoAsync();
                if (producto != null)
                {
                    var productoExistente = await _context.Productos
                        .FirstOrDefaultAsync(p => p.Codigo == producto.Codigo);

                    if (productoExistente != null)
                    {
                        // Incrementar la cantidad del producto existente
                        productoExistente.Cantidad += producto.Cantidad;
                        _context.Productos.Update(productoExistente);
                        _logger.LogInformation($"Cantidad del producto existente incrementada: {productoExistente.Nombre}");
                    }
                    else
                    {
                        // Agregar un nuevo producto
                        _context.Productos.Add(producto);
                        _logger.LogInformation($"Producto guardado: {producto.Nombre}");
                    }

                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar el producto en la base de datos.");
                throw; // Opcionalmente, puedes relanzar la excepción para propagarla
            }
        }



        public async Task<List<Producto>> DevulveListaDeProductosAsync()
        {
            var productos = await _context.Productos.ToListAsync();
            _logger.LogInformation($"Número de productos recuperados: {productos.Count}");
            return productos;
        }

        public async Task<Producto> DevuelveProductoAsync()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var client = new HttpClient(handler); 

            

           // HttpClient client = new HttpClient();
            var response = await client.GetAsync("http://127.0.0.1:5000/random_producto");
            var responseJson = await response.Content.ReadAsStringAsync();
            Debug.WriteLine(responseJson);
            Console.WriteLine(responseJson);
            var producto = JsonConvert.DeserializeObject<Producto>(responseJson);
           // _logger.LogInformation($"Producto obtenido de la API: {producto.Nombre}");
            return producto;

        }
    }
}
