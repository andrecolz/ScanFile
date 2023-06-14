using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanFileLib
{
    public class CListaTipi
    {
        public List<CtipiFile> listaTipi { get; set; }
        public int nEstensioni { get; set; }
        public int nFile { get; set; }
        public CListaTipi()
        {
            listaTipi = new List<CtipiFile>();
            nEstensioni = 0;
            nFile = 0;
        }

        public void add(CtipiFile file)
        {
            if (listaTipi.Count != null)
            {
                for (int i = 0; i < listaTipi.Count; i++)
                {
                    if (listaTipi[i].estensione.Equals(file.estensione))
                    {
                        listaTipi[i].quantita++;
                        nFile++;
                        listaTipi[i].peso += file.peso;
                        return;
                    }
                }
            }
            listaTipi.Add(file);
            nEstensioni++;
        }

        public CtipiFile get(int i)
        {
            return listaTipi[i];
        }

        public void calcolaDiff(double ptot)
        {
            for(int i = 0; i < listaTipi.Count; i++)
            {
                long peso = listaTipi[i].peso;
                double perc = (peso / ptot) * 100;
                listaTipi[i].perc = Math.Round(perc, 2);
            }
        }

        public void visualizza()
        {
            Console.WriteLine("TIPI DI ESTENSIONI (" + nEstensioni + ")");
            for(int i = 0;i < listaTipi.Count; i++)
            {
                Console.WriteLine("\nEstensione: " + listaTipi[i].estensione + "\nQuantita: " + listaTipi[i].quantita + "\nPeso: " + listaTipi[i].peso.Bytes().Humanize());
            }
        }

        public List<CtipiFile> visualizzaOrdinati(int n)
        {
            List<CtipiFile> tmp = listaTipi.OrderByDescending(f => f.peso).ToList();
            return tmp;
        }
    }
}
