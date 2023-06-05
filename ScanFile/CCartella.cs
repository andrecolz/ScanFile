using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanFile
{
    public class CCartella
    {
        public string nome;
        public string dimensione;
        public string path;
        public List<CFile> listF;
        public List<CCartella> listD;

        public CCartella(string nome, string dimensione, string path)
        {
            this.nome = nome;
            this.dimensione = dimensione;
            this.path = path;
        }

        public void pushFile(CFile file)
        {
            listF.Add(file);
        }

        public void pushDirector(CCartella directory)
        {
            listD.Add(directory);
        }
    }
}
