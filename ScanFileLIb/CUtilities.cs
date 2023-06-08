using ScanFileLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;
using Humanizer;

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

        public void calcolaPesanti(List<CFile> lfile)
        {
            for (int i = 0; i < lfile.Count - 1; i++)
            {
                for (int j = 0; j < lfile.Count - i - 1; j++)
                {
                    if (Int32.Parse(lfile[j].dimensione) < Int32.Parse(lfile[j + 1].dimensione))
                    {
                        CFile tempVar = lfile[j];
                        lfile[j] = lfile[j + 1];
                        lfile[j + 1] = tempVar;
                    }
                }
            }
        }
    }
}
