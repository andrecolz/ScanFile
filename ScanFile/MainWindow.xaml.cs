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

namespace ScanFile
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            List<string> files = new List<string>();
            InitializeComponent();
        }

        private void selezioneDc_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "seleziona una cartella";
            fbd.ShowDialog();

            DirectoryInfo dirInfo = new DirectoryInfo(fbd.SelectedPath);
            long pesoCartella = CalcolaPesoCartella(dirInfo.FullName);
            CCartella Droot = new CCartella(dirInfo.Name, pesoCartella.ToString(), fbd.SelectedPath);


            scansiona(dirInfo, Droot);


            string[] directories = System.IO.Directory.GetDirectories(fbd.SelectedPath, "*", System.IO.SearchOption.AllDirectories);
            for(int i = 0; i < directories.Length; i++)
            {
                tre.Items.Add(directories[i]);
            }
        }

        public static void scansiona(DirectoryInfo dirInfo, CCartella Droot)
        {
            foreach (FileInfo file in dirInfo.GetFiles())
            {
                CFile ftmp = new CFile(file.Name, file.Length.ToString(), file.DirectoryName);
                Droot.pushFile(ftmp);
            }

            foreach (DirectoryInfo subDir in dirInfo.GetDirectories())
            {
                scansiona(subDir, Droot);
            }
        }


        public static long CalcolaPesoCartella(string path)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            long pesoTotale = 0;

            foreach (FileInfo file in dirInfo.GetFiles())
            {
                pesoTotale += file.Length;
            }

            foreach (DirectoryInfo subDir in dirInfo.GetDirectories())
            {
                pesoTotale += CalcolaPesoCartella(subDir.FullName);
            }

            return pesoTotale;
        }
    }
}
