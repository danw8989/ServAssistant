using System;
using System.Collections.Generic;
using System.Text;

namespace ServAssistant.Modele
{
    public class TypAkcji
    {
        public int id { get; set; }
        public string nazwa { get; set; }
    }

    public class TypyAkcji
    {
        public List<TypAkcji> typ_akcji = new List<TypAkcji>();
    }
}
