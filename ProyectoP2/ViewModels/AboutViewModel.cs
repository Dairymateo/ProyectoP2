using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace ProyectoP2.ViewModels
{
    public class AboutViewModel : ObservableObject
    {
        public ICommand NavigateToStoreCommand { get; }

        public AboutViewModel()
        {
            NavigateToStoreCommand = new Command(OnNavigateToStore);
        }

        public ObservableCollection<Testimonial> Testimonials { get; } = new ObservableCollection<Testimonial>
    {
        new Testimonial { Quote = "BookSwap ha sido un salvavidas para mí. Como estudiante, no siempre tengo el presupuesto para comprar libros nuevos, y gracias a esta " +
            "plataforma he podido conseguir los textos que necesito para mis clases a precios mucho más accesibles. ¡Además, he descubierto " +
            "nuevos autores y géneros que no conocía!", 
            Image = "joaquin.webp", Name = "Joaquin Chacon", Title = "Estudiante de Ingenieria" },

        new Testimonial { Quote = "Soy un apasionado de los libros y me encanta compartir mi pasión con mis alumnos. BookSwap me ha permitido encontrar ediciones " +
            "antiguas y descatalogadas de obras clásicas que tutilizo para enriquecer mis clases. ¡Mis estudiantes están fascinados con la posibilidad de leer libros " +
            "que no encontrarían en ninguna librería!", 
            Image = "mateo.jpg", Name = "Mateo Herrera", Title = "Profesor" },

        new Testimonial { Quote = "Mis hijos son ávidos lectores y siempre están buscando nuevos libros. BookSwap nos ha permitido intercambiar " +
            "libros que ya han leído por otros que les interesan, lo que nos ha ahorrado mucho dinero y ha " +
            "fomentado el hábito de la lectura en casa. ¡Además, es una forma divertida de conocer a otros amantes de los libros en nuestra comunidad!", 
            Image = "diego.jpg", Name = "Diego Correa", Title = "Padre de familia" }
    };
        private void OnNavigateToStore()
        {
            // Implement the navigation to the store
        }
    }

    public class Testimonial
    {
        public string Quote { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
    }
}
