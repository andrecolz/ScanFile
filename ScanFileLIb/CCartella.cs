using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanFileLib
{
    public class CCartella
    {
        public string nome { get; set; }
        public long dimensione { get; set; }
        public double percentuale { get; set; }
        public string path { get; set; }
        public List<CFile> listF { get; set; }
        public List<CCartella> listD { get; set; }

        public CCartella()
        {
            this.nome = "";
            this.dimensione = 0;
            this.percentuale = 0.00;
            this.path = "";
            listF = new List<CFile>();
            listD = new List<CCartella>();
        }

        public CCartella(string nome, long dimensione, double percentuale, string path)
        {
            this.nome = nome;
            this.dimensione = dimensione;
            this.percentuale = percentuale;
            this.path = path;
            listF = new List<CFile>();
            listD = new List<CCartella>();
        }

        public void pushFile(CFile file)
        {
            listF.Add(file);
        }

        public void pushDirectory(CCartella directory)
        {
            listD.Add(directory);
        }
    }
}
