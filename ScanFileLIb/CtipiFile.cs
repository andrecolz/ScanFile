using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanFileLib
{
    public class CtipiFile
    {
        public string estensione { get; set; }
        public int peso { get; set; }
        public int quantita { get; set; }
        public double perc { get; set; }

        public CtipiFile()
        {
            this.estensione = "";
            this.peso = 0;
            this.quantita = 0;
            this.perc = 0;
        }

        public CtipiFile(string estensione, int peso, int quantita, double perc)
        {
            this.estensione = estensione.ToLower();
            this.peso = peso;
            this.quantita = quantita;
            this.perc = perc;
        }
    }
}
