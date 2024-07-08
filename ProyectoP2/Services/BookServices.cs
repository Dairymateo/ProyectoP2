using Newtonsoft.Json;
using ProyectoP2.Models;
using ProyectoP2.DataAccess;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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
            var producto = await DevuelveProductoAsync();
            if (producto != null)
            {
                _context.Productos.Add(producto);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Producto guardado: {producto.Nombre}");
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
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("http://127.0.0.1:5000/random_producto");
            var responseJson = await response.Content.ReadAsStringAsync();
            var producto = JsonConvert.DeserializeObject<Producto>(responseJson);
            _logger.LogInformation($"Producto obtenido de la API: {producto.Nombre}");
            return producto;
        }
    }
}
