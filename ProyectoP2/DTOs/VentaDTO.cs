
namespace ProyectoP2.DTOs
{
    public class VentaDTO
    {
        public string Cliente { get; set; }
        public string NumeroVenta { get; set; }
        public double Total { get; set; }
        public double PagoCon { get; set; }
        public double Cambio { get; set; }
        public string FechaRegistro { get; set; }
        public List<DetalleVentaDTO> DetalleVenta { get; set; } = new List<DetalleVentaDTO>();
    }
}