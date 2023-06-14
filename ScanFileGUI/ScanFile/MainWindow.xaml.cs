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
using System.IO;
using static System.Net.WebRequestMethods;
using ScanFileLib;
using static Humanizer.In;
using Humanizer;
using Humanizer.Bytes;

namespace ScanFile
{
    public partial class MainWindow : Window
    {
        CCartella Droot;
        CListaTipi listatipi = new CListaTipi();
        CUtilities metodi = new CUtilities();
        List<CFile> files = new List<CFile>();
        List<CCartella> cartellas = new List<CCartella>();
        bool caricato = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(CCartella d, int n)
        {
            InitializeComponent();
            if (n == 1)
            {
                Droot = d;
                impo();
            }
        }

        public void inizScan()
        {
            if (caricato == true)
            {
                lstbPesanti.Items.Clear();
                lstpTipi.Items.Clear();
                TreeFile.Items.Clear();
                lblTempo.Content = "";
            }
            caricato = true;
            files = new List<CFile>();
            cartellas = new List<CCartella>();
            listatipi = new CListaTipi();
        }

        private void selezioneDc_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "seleziona una cartella";
            var result = fbd.ShowDialog();

            if (result.ToString().Equals("OK"))
            {
                DateTime start = DateTime.Now;
                inizScan();

                DirectoryInfo dirInfo = new DirectoryInfo(fbd.SelectedPath);
                long pesoCartella = metodi.CalcolaPesoCartella(dirInfo.FullName);
                Droot = new CCartella(dirInfo.Name, pesoCartella, 100, fbd.SelectedPath);

                TreeViewItem rootNode = new TreeViewItem();
                rootNode.Header = "100% - " + pesoCartella.Bytes().Humanize() + " | " + dirInfo.Name;
                TreeFile.Items.Add(rootNode);

                scansiona(dirInfo, Droot, rootNode, files, listatipi, Convert.ToDouble(pesoCartella));
                
                files = metodi.calcolaPesanti(files);
                cartellas = metodi.calcolaPesantiCartella(cartellas);
                btnCaricaFile.Background = new SolidColorBrush(Colors.Gray);
                for (int i = 0; i < files.Count; i++) { lstbPesanti.Items.Add(espandi(files[i].nome, 45) + " | " + ByteSize.FromBytes(files[i].dimensione).Humanize()); }

                listatipi.calcolaDiff(pesoCartella);
                listatipi.listaTipi = listatipi.visualizzaOrdinati(1);
                for (int i = 0; i < listatipi.nEstensioni; i++) { lstpTipi.Items.Add(espandi("Estensione:", 17) + listatipi.listaTipi[i].estensione + espandi("\nQuantita:", 18) + listatipi.listaTipi[i].quantita + espandi("\nPeso:", 18) + listatipi.listaTipi[i].peso.Bytes().Humanize() + espandi("\nPercentuale:", 18) + listatipi.listaTipi[i].perc + "%\n"); }

                DateTime end = DateTime.Now;
                TimeSpan ts = (end - start);
                lblTempo.Content = Math.Round(ts.TotalSeconds, 2) + " s";

            } else if (result.ToString().Equals("Cancel")) { return; }
        }

        public void scansiona(DirectoryInfo dirInfo, CCartella Droot, TreeViewItem rootNode, List<CFile> lfile, CListaTipi listatipi, double ptot)
        {
            TreeViewItem child1Node = rootNode;

            try
            {
                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    double peso = Convert.ToDouble(file.Length);
                    double perc = Math.Round((peso / ptot) * 100, 2);
                    CFile ftmp = new CFile(file.Name, file.Length, perc, file.DirectoryName);
                    Droot.pushFile(ftmp);
                    lfile.Add(ftmp);
                    CtipiFile tmp = new CtipiFile(file.Extension, file.Length, 1, 0);
                    listatipi.add(tmp);

                    TreeViewItem grandchild1Node = new TreeViewItem();
                    grandchild1Node.Header = espandi(espandi(perc.ToString(), 5) + "% - " + modifica(file.Length.Bytes().Humanize(), 9), 19) + "| " + file.Name;
                    child1Node.Items.Add(grandchild1Node);
                }

                foreach (DirectoryInfo subDir in dirInfo.GetDirectories())
                {
                    double peso = Convert.ToDouble(metodi.CalcolaPesoCartella(subDir.FullName));
                    double perc = Math.Round((peso / ptot) * 100, 2);
                    long pes = metodi.CalcolaPesoCartella(subDir.FullName);
                    CCartella dtmp = new CCartella(subDir.Name, pes, perc, subDir.FullName);
                    CCartella tmp = new CCartella(subDir.Name, pes, perc, subDir.FullName);

                    child1Node = new TreeViewItem();

                    child1Node.Header = espandi(espandi(perc.ToString(), 5) + "% - " + modifica(Convert.ToInt64(peso).Bytes().Humanize(), 9), 19) + "| " + subDir.Name;
                    rootNode.Items.Add(child1Node);

                    cartellas.Add(tmp);
                    Droot.pushDirectory(dtmp);
                    scansiona(subDir, dtmp, child1Node, lfile, listatipi, ptot);
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public string getExt(string ext)
        {
            string[] v = ext.Split('.');
            return v[v.Length - 1];
        }

        public void scanImp(CCartella Droot, TreeViewItem rootNode, List<CFile> files, CListaTipi listatipi)
        {
            TreeViewItem child1Node = rootNode;

            try
            {
                foreach (CFile file in Droot.listF)
                {
                    files.Add(file);
                    CtipiFile tmp = new CtipiFile(getExt(file.nome), file.dimensione, 1, 0);
                    listatipi.add(tmp);

                    TreeViewItem grandchild1Node = new TreeViewItem();
                    grandchild1Node.Header = espandi(espandi(file.percentuale.ToString(), 5) + "% - " + modifica(file.dimensione.Bytes().Humanize(), 9), 19) + "| " + file.nome;
                    child1Node.Items.Add(grandchild1Node);
                }

                foreach (CCartella subDir in Droot.listD)
                {
                    CCartella dtmp = new CCartella(subDir.nome, subDir.dimensione, subDir.percentuale, subDir.path);
                    CCartella tmp = new CCartella(subDir.nome, subDir.dimensione, subDir.percentuale, subDir.path);

                    child1Node = new TreeViewItem();

                    child1Node.Header = espandi(espandi(subDir.percentuale.ToString(), 5) + "% - " + modifica(subDir.dimensione.Bytes().Humanize(), 9), 19) + "| " + subDir.nome;
                    rootNode.Items.Add(child1Node);

                    cartellas.Add(tmp);
                    scanImp(subDir, child1Node, files, listatipi);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void impo()
        {
            DateTime start = DateTime.Now;
            inizScan();
            
            TreeViewItem rootNode = new TreeViewItem();
            rootNode.Header = "100% - " + Droot.dimensione.Bytes().Humanize() + " | " + Droot.nome;
            TreeFile.Items.Add(rootNode);

            scanImp(Droot, rootNode, files, listatipi);

            files = metodi.calcolaPesanti(files);
            cartellas = metodi.calcolaPesantiCartella(cartellas);
            btnCaricaFile.Background = new SolidColorBrush(Colors.Gray);
            for (int i = 0; i < files.Count; i++) { lstbPesanti.Items.Add(espandi(files[i].nome, 45) + " | " + ByteSize.FromBytes(files[i].dimensione).Humanize()); }

            listatipi.calcolaDiff(Droot.dimensione);
            listatipi.listaTipi = listatipi.visualizzaOrdinati(1);
            for (int i = 0; i < listatipi.nEstensioni; i++) { lstpTipi.Items.Add(espandi("Estensione:", 17) + listatipi.listaTipi[i].estensione + espandi("\nQuantita:", 18) + listatipi.listaTipi[i].quantita + espandi("\nPeso:", 18) + listatipi.listaTipi[i].peso.Bytes().Humanize() + espandi("\nPercentuale:", 18) + listatipi.listaTipi[i].perc + "%\n"); }

            DateTime end = DateTime.Now;
            TimeSpan ts = (end - start);
            lblTempo.Content = Math.Round(ts.TotalSeconds, 2) + " s";
        }

        public string modifica(string str, int l)
        {
            string[] v = str.Split(' ');
            int n = str.Length;
            for (int i = n; i <= l; i++)
                v[0] += " ";

            return v[0] + v[1];
        }

        public string espandi(string str, int l)
        {
            int n = str.Length;
            for(int i = n; i <= l; i++)
                str += " ";
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
            this.Close();
        }

        private void btnCompara_Click(object sender, RoutedEventArgs e)
        {
            Confronta fconfronta = new Confronta();
            fconfronta.Show();
        }


        private void btnCaricaFile_Click(object sender, RoutedEventArgs e)
        {
            if (caricato == true)
            {
                lstbPesanti.Items.Clear();
                btnCaricaFile.Background = new SolidColorBrush(Colors.Gray);
                btnCaricaCartelle.Background = new SolidColorBrush(Colors.LightGray);
                for (int i = 0; i < files.Count; i++) { lstbPesanti.Items.Add(espandi(files[i].nome, 45) + " | " + ByteSize.FromBytes(files[i].dimensione).Humanize()); }
            }
        }

        private void btnCaricaCartelle_Click(object sender, RoutedEventArgs e)
        {
            if (caricato == true)
            {
                lstbPesanti.Items.Clear();
                btnCaricaFile.Background = new SolidColorBrush(Colors.LightGray);
                btnCaricaCartelle.Background = new SolidColorBrush(Colors.Gray);
                for (int i = 0; i < cartellas.Count; i++) { lstbPesanti.Items.Add(espandi(cartellas[i].nome, 45) + " | " + ByteSize.FromBytes(cartellas[i].dimensione).Humanize()); }
            }
        }
    }
}