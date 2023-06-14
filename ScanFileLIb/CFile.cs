using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanFileLib
{
    public class CFile
    {
        public string nome { get; set; }
        public long dimensione { get; set; }
        public double percentuale { get; set; }
        public string path { get; set; }

        public CFile()
        {
            this.nome = "";
            this.dimensione = 0;
            this.path = "";
            this.percentuale = 0.00;
        }

        public CFile(string nome, long dimensione, double percentuale, string path)
        {
            this.nome = nome;
            this.dimensione = dimensione;
            this.percentuale = percentuale;
            this.path = path;
        }
    }
}
