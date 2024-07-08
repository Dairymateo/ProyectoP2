using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoP2.DTOs

{
    public partial class CategoriaDTO : ObservableObject
    {
        [ObservableProperty]
        public int idCategoria;
        [ObservableProperty]
        public string nombre;
    }
}
