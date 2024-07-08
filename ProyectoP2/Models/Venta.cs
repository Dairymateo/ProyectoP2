using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoP2.Models
{
    public class Venta
    {
        [Key]
        public int IdVenta { get; set; }
        public string Cliente { get; set; }
        public string NumeroVenta { get; set; }
        public double Total { get; set; }
        public double PagoCon { get; set; }
        public double Cambio { get; set; }
        public DateTime FechaRegistro { get; set; }
        public virtual ICollection<DetalleVenta> RefDetalleVenta { get; set; } = new List<DetalleVenta>();
    }
}