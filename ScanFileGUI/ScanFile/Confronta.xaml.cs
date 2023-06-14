using Humanizer;
using Humanizer.Bytes;
using Newtonsoft.Json;
using ScanFileLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScanFile
{
    /// <summary>
    /// Logica di interazione per Confronta.xaml
    /// </summary>
    public partial class Confronta : Window
    {
        public Confronta()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fbd = new OpenFileDialog();
            var result = fbd.ShowDialog();
            if (result.ToString().Equals("OK"))
            {
                lblpath1.Content = fbd.FileName;
            }
            else if (result.ToString().Equals("Cancel"))
            {
                return;
            }
        }

        private void btnPath2_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fbd = new OpenFileDialog();
            var result = fbd.ShowDialog();
            if (result.ToString().Equals("OK"))
            {
                lblpath2.Content = fbd.FileName;
            }
            else if (result.ToString().Equals("Cancel"))
            {
                return;
            }
        }

        private void btnConfronto_Click(object sender, RoutedEventArgs e)
        {
            CUtilities metodi = new CUtilities();
            FileInfo fileInfo = new FileInfo(lblpath1.Content.ToString());
            FileInfo fileInfo2 = new FileInfo(lblpath2.Content.ToString());
            CListaTipi lst1;
            CListaTipi lst2;
            using (StreamReader file = File.OpenText(lblpath1.Content.ToString()))
            {
                JsonSerializer serializer = new JsonSerializer();
                lst1 = (CListaTipi)serializer.Deserialize(file, typeof(CListaTipi));
            }
            using (StreamReader file = File.OpenText(lblpath2.Content.ToString()))
            {
                JsonSerializer serializer = new JsonSerializer();
                lst2 = (CListaTipi)serializer.Deserialize(file, typeof(CListaTipi));
            }


            var differenze = new CListaTipi();
            for (int j = 0; j < lst2.nEstensioni; j++)
            {
                bool found = false;
                for (int i = 0; i < lst1.nEstensioni; i++)
                {
                    if (lst1.get(i).estensione.Equals(lst2.get(j).estensione))
                    {
                        var diff = new CtipiFile();
                        bool check1 = false, check2 = false;
                        diff.estensione = lst1.get(i).estensione;
                        if (lst1.get(i).quantita == lst2.get(j).quantita) { diff.quantita = lst1.get(i).quantita; }
                        if (lst1.get(i).quantita < lst2.get(j).quantita) { diff.quantita = (lst2.get(j).quantita - lst1.get(i).quantita); check1 = true; }
                        if (lst1.get(i).quantita > lst2.get(j).quantita) { diff.quantita = (lst1.get(i).quantita - lst2.get(j).quantita); check1 = true; }

                        if (lst1.get(i).peso == lst2.get(j).peso) { diff.peso = lst1.get(i).peso; }
                        if (lst1.get(i).peso < lst2.get(j).peso) { diff.peso = (lst2.get(j).peso - lst1.get(i).peso); check2 = true; }
                        if (lst1.get(i).peso > lst2.get(j).peso) { diff.peso = (lst1.get(i).peso - lst2.get(j).peso); check2 = true; }

                        if (check1 && check2) { differenze.add(diff); differenze.nFile += diff.quantita; }

                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    var diff = new CtipiFile();
                    diff.estensione = lst2.get(j).estensione;
                    diff.quantita = lst2.get(j).quantita;
                    diff.peso = lst2.get(j).peso;

                    differenze.add(diff);
                    differenze.nFile += diff.quantita;
                }
            }

            for (int i = 0; i < differenze.nEstensioni; i++)
            {
                lstbElenco.Items.Add(espandi("Estensione:", 17) + differenze.listaTipi[i].estensione + espandi("\nQuantita:", 18) + differenze.listaTipi[i].quantita + espandi("\nPeso:", 18) + ByteSize.FromBytes(differenze.listaTipi[i].peso).Humanize() + espandi("\nPercentuale:", 18) + differenze.listaTipi[i].perc + "%\n");
            }
        }

        public string espandi(string str, int l)
        {
            int n = str.Length;
            for (int i = n; i <= l; i++)
            {
                str += " ";
            }
            n = str.Length;
            return str;
        }
    }
}
