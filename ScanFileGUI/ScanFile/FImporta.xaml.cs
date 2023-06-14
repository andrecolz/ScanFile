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
using Newtonsoft.Json;
using static Humanizer.In;
using Humanizer;
using ScanFileLib;
using Microsoft.Win32;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace ScanFile
{
    /// <summary>
    /// Logica di interazione per FImporta.xaml
    /// </summary>
    public partial class FImporta : Window
    {
        public FImporta()
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

        private void btnImporta_Click(object sender, RoutedEventArgs e)
        {
            CCartella Droot;
            if (lblpath1.Content != null)
            {
                using (StreamReader file = File.OpenText(lblpath1.Content.ToString()))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    Droot = (CCartella)serializer.Deserialize(file, typeof(CCartella));
                }
                MainWindow mainWindow = new MainWindow(Droot, 1);
                mainWindow.Show();
                this.Close();
            }
        }

        public string espandi(string str, int l)
        {
            int n = str.Length;
            for (int i = n; i <= l; i++)
            {
                str += " ";
            }
            return str;
        }
    }
}
