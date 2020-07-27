using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Wpf_razvojnoOkolje
{
    [XmlRoot("Projekt", Namespace ="", IsNullable =false)]
    public class Projekt
    {
        public string Naziv { get; set; }

        public string Avtor { get; set; }

        public string Lokacija { get; set; }

        public ProgJezik ProgramskiJezik { get; set; }

        public TipApp VrstaProjekta { get; set; }

        public Ogrodje Ogrodje { get; set; }

        //public List<Datoteka> Datoteke { get; set; }

        //public Projekt()
        //{
        //    Datoteke = new List<Datoteka>();
        //}
    }

    [XmlRoot("Datoteka", Namespace = "", IsNullable = false)]
    public class Datoteka
    {
        public string Naziv { get; set; }
        public string Data { get; set; }
    }


    public class ProgJezik
    {
        public string Naziv { get; set; }

        //[XmlIgnore]
        public List<TipApp> TipApps { get; set; }
        public ProgJezik()
        {
            TipApps = new List<TipApp>();
        }
    }
    public class TipApp
    {
        public string Naziv { get; set; }

    }
    public class Ogrodje
    {
        public string Naziv { get; set; }
        public string Lokacija { get; set; }

    }
}
