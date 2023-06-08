using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows;
using System.IO;
using static System.Net.WebRequestMethods;
using ScanFileLib;
using static Humanizer.In;
using Humanizer;

namespace ScanFile
{
    public partial class MainWindow : Window
    {
        CCartella Droot;
        CUtilities metodi = new CUtilities();
        public MainWindow()
        {
            InitializeComponent();

        }

        private void selezioneDc_Click(object sender, RoutedEventArgs e)
        {
            lstbPesanti.Items.Clear();
            lstpTipi.Items.Clear();
            TreeFile.Items.Clear();
            lblTempo.Content = "";
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "seleziona una cartella";
            var result = fbd.ShowDialog();
            if (result.ToString().Equals("OK"))
            {
                List<CFile> files = new List<CFile>();
                CListaTipi listatipi = new CListaTipi();
                DirectoryInfo dirInfo = new DirectoryInfo(fbd.SelectedPath);
                long pesoCartella = metodi.CalcolaPesoCartella(dirInfo.FullName);
                Droot = new CCartella(dirInfo.Name, pesoCartella.ToString(), 100, fbd.SelectedPath);

                TreeViewItem rootNode = new TreeViewItem();
                rootNode.Header = "100% - " + pesoCartella.Bytes().Humanize() + " | " + dirInfo.Name;
                TreeFile.Items.Add(rootNode);

                DateTime start = DateTime.Now;
                scansiona(dirInfo, Droot, rootNode, files, listatipi, Convert.ToDouble(pesoCartella));
                DateTime end = DateTime.Now;
                TimeSpan ts = (end - start);
                lblTempo.Content = Math.Round(ts.TotalSeconds, 2) + " s";
                
                metodi.calcolaPesanti(files);
                for (int i = 0; i < 10; i++) { lstbPesanti.Items.Add(espandi(files[i].nome, 27) + " | " + Int32.Parse(files[i].dimensione).Bytes().Humanize()); }

                listatipi.calcolaDiff(pesoCartella);
                listatipi.visualizzaOrdinati(1);
                string d = "";
     
                for (int i = 0; i < listatipi.nEstensioni; i++) { lstpTipi.Items.Add(espandi("Estensione:", 17) + listatipi.listaTipi[i].estensione + espandi("\nQuantita:", 18) + listatipi.listaTipi[i].quantita + espandi("\nPeso:", 18) + listatipi.listaTipi[i].peso.Bytes().Humanize() + espandi("\nPercentuale:", 18) + listatipi.listaTipi[i].perc + "%\n"); }

            } else if (result.ToString().Equals("Cancel"))
            { 
                return;
            }
        }

        public void scansiona(DirectoryInfo dirInfo, CCartella Droot, TreeViewItem rootNode, List<CFile> lfile, CListaTipi listatipi, double ptot)
        {
            TreeViewItem child1Node = rootNode;


            foreach (FileInfo file in dirInfo.GetFiles())
            {
                double peso = Convert.ToDouble(file.Length);
                double perc = Math.Round((peso / ptot) * 100, 2);
                CFile ftmp = new CFile(file.Name, file.Length.ToString(), perc, file.DirectoryName);
                Droot.pushFile(ftmp);
                lfile.Add(ftmp);
                CtipiFile tmp = new CtipiFile(file.Extension, Convert.ToInt32(file.Length), 1, 0);
                listatipi.add(tmp);

                TreeViewItem grandchild1Node = new TreeViewItem();
                grandchild1Node.Header = espandi(perc + "% - " + file.Length.Bytes().Humanize(), 23) + "| " + file.Name;
                child1Node.Items.Add(grandchild1Node);
            }

            foreach (DirectoryInfo subDir in dirInfo.GetDirectories())
            {
                double peso = Convert.ToDouble(metodi.CalcolaPesoCartella(subDir.FullName));
                double perc = Math.Round((peso / ptot) * 100, 2);
                CCartella dtmp = new CCartella(subDir.Name, metodi.CalcolaPesoCartella(subDir.FullName).ToString(), perc, subDir.FullName);

                child1Node = new TreeViewItem();
                child1Node.Header = espandi(perc + "% - " + Convert.ToInt32(peso).Bytes().Humanize(), 23) + "| " + subDir.Name;
                rootNode.Items.Add(child1Node);
                

                Droot.pushDirectory(dtmp);
                scansiona(subDir, dtmp, child1Node, lfile, listatipi, ptot);
            }
        }

        public string espandi(string str, int l)
        {
            int n = str.Length;
            for(int i = n; i <= l; i++)
            {
                str += " ";
            }
            n = str.Length;
            return str;
        }



        private void Esporta_Click(object sender, RoutedEventArgs e)
        {
            FEsporta fEsporta = new FEsporta(Droot);
            fEsporta.Show();
        }

        private void Importa_Click(object sender, RoutedEventArgs e)
        {
            FImporta fImporta = new FImporta();
            fImporta.Show();
        }
    }
}
