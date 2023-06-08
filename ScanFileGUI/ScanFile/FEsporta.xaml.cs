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
using System.Xml.Serialization;
using MessageBox = System.Windows.MessageBox;

namespace ScanFile
{
    /// <summary>
    /// Logica di interazione per FEsporta.xaml
    /// </summary>
    public partial class FEsporta : Window
    {
        CCartella Droot;
        CUtilities metodi = new CUtilities();
        public FEsporta(CCartella d)
        {
            Droot = d;
            InitializeComponent();
        }

        private void btnToXml_Click(object sender, RoutedEventArgs e)
        {
            if (lblPer.Content != "" && txtNmFile.Text != "")
            {
                metodi.toXml(lblPer.Content + "\\" + txtNmFile.Text, ref Droot);
                MessageBox.Show("File esportato", "avviso");
                this.Close();
            }
            else
            {
                MessageBox.Show("selezionare un percorso o il nome del file", "errore");
            }
        }

        private void btnToJson_Click(object sender, RoutedEventArgs e)
        {
            if (lblPer.Content != "" && txtNmFile.Text != "")
            {
                metodi.toJson(lblPer.Content + "\\" + txtNmFile.Text, ref Droot);
                MessageBox.Show("File esportato", "avviso");
                this.Close();
            }
            else
            {
                MessageBox.Show("selezionare un percorso o il nome del file", "errore");
            }
        }

        private void btnSelPer_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "seleziona una cartella";
            var result = fbd.ShowDialog();
            if (result.ToString().Equals("OK"))
            {
                lblPer.Content = fbd.SelectedPath;
            }
            else if (result.ToString().Equals("Cancel"))
            {
                return;
            }
        }
    }
}
