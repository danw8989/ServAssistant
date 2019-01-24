using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ServAssistant.Modele
{
    [Serializable]
    public class Obiekt
    {
        public int id { get; set; }
        public string imageUri { get; set; }
        public string miasto { get; set; }
        public string nazwa { get; set; }
        public int numer { get; set; }
        public string ulica { get; set; }
        public string info { get; set; }

        public bool ShouldSerializeid()
        {
            return false;
        }

    }

    [Serializable]
    public class Obiekty
    {
        public ObservableCollection<Obiekt> obiekty { get; set; }

        public Obiekty()
        {
            obiekty = new ObservableCollection<Obiekt>();
        }
    }
}
