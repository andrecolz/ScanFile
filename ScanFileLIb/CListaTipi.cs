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
                double peso = Convert.ToDouble(listaTipi[i].peso);
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

        public void visualizzaOrdinati(int n)
        {
            for (int i = 0; i < listaTipi.Count - 1; i++)
            {
                for (int j = 0; j < listaTipi.Count - i - 1; j++)
                {
                    if (listaTipi[j].peso < listaTipi[j + 1].peso)
                    {
                        CtipiFile tempVar = listaTipi[j];
                        listaTipi[j] = listaTipi[j + 1];
                        listaTipi[j + 1] = tempVar;
                    }
                }
            }
        }
    }
}
