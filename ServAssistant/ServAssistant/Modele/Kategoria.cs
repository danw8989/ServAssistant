using System;
using System.Collections.Generic;
using System.Text;

namespace ServAssistant.Modele
{
    public class Kategoria
    {
        public int id { get; set; }
        public string nazwa { get; set; }
    }

    public class Kategorie
    {
        public List<Kategoria> kategorie = new List<Kategoria>();
    }
}
