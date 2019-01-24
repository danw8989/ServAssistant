using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ServAssistant.Modele
{
    [Serializable]
    public class Urzadzenie
    {
        public int id { get; set; }
        public string nazwa { get; set; }
        public string imgUri { get; set; }
        public int kategoria { get; set; }
        public int akcja { get; set; }
        public int typ_akcji { get; set; }

        public bool ShouldSerializeid()
        {
            return false;
        }
    }

    [Serializable]
    public class Urzadzenia
    {
        public ObservableCollection<Urzadzenie> urzadzenie { get; set; } = new ObservableCollection<Urzadzenie>();
    }
}
