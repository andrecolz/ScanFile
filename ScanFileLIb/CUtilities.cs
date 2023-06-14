using ScanFileLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;
using Humanizer;
using System.Linq;

namespace ScanFileLib
{
    public class CUtilities
    {
        public CUtilities()
        {
        }

        public void toJson<T>(string nomeFile, ref T c)
        {
            string json = JsonConvert.SerializeObject(c, Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText(nomeFile + ".json", json);
        }

        public void toXml<T>(string nomeFile, ref T c)
        {
            XmlSerializer myXml = new XmlSerializer(typeof(CCartella));
            StreamWriter fOut = new StreamWriter(nomeFile + ".xml");
            myXml.Serialize(fOut, c);
            fOut.Close();
        }

        public long CalcolaPesoCartella(string path)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            long pesoTotale = 0;

            try
            {
                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    pesoTotale += file.Length;
                }

                foreach (DirectoryInfo subDir in dirInfo.GetDirectories())
                {
                    pesoTotale += CalcolaPesoCartella(subDir.FullName);
                }
            } catch (Exception e) { 
                Console.WriteLine(e.Message);
            }

            return pesoTotale;
        }

        public List<CFile> calcolaPesanti(List<CFile> lfile)
        {
            List<CFile> li = lfile.OrderByDescending(f => f.dimensione).ToList();
            return li;
        }

        public List<CCartella> calcolaPesantiCartella(List<CCartella> cart)
        {
            List<CCartella> li = cart.OrderByDescending(f => f.dimensione).ToList();
            return li;

        }
    }
}
