using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanFile
{
    public class CFile
    {
        public string nome;
        public string dimensione;
        public string path;

        public CFile(string nome, string dimensione, string path)
        {
            this.nome = nome;
            this.dimensione = dimensione;
            this.path = path;
        }
    }
}
