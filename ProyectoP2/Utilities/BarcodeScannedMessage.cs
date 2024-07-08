using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ProyectoP2.Utilities
{
    public class BarcodeScannedMessage : ValueChangedMessage<BarcodeResult>
    {
        public BarcodeScannedMessage(BarcodeResult value) : base(value)
        {
        }
    }
}
