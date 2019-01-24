using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Newtonsoft.Json;

namespace ServAssistant.Modele
{
    [Serializable]
    public class Akcja
    {
        public int id { get; set; }
        public string nazwa { get; set; }
        public DateTime dataStart { get; set; }
        public DateTime dataKoniec { get; set; }
        public int obiekt { get; set; }
        public bool zakonczone { get; set; }
        public bool konserwacja { get; set; }
        public int okres { get; set; }

        public bool ShouldSerializeid()
        {
            return false;
        }
    }

    [Serializable]
    public class Akcje
    {
        public ObservableCollection<Akcja> akcja { get; set; }

        public Akcje()
        {
            akcja = new ObservableCollection<Akcja>();
        }
    }
}
