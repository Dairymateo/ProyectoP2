using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging.Messages;
using ProyectoP2.DTOs;

namespace ProyectoP2.Utilities
{
    class ProductoVentaMessage : ValueChangedMessage<ProductoDTO>
    {
        public ProductoVentaMessage(ProductoDTO value) : base(value)
        {
        }
    }
}
