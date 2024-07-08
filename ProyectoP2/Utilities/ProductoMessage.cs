using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ProyectoP2.Utilities
{
    class ProductoMessage : ValueChangedMessage<ProductoResult>
    {
        public ProductoMessage(ProductoResult value) : base(value)
        {

        }
    }
}
