using System.Security.Principal;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Globalization;
using Humanizer.Bytes;
using Humanizer;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;
using ScanFileLib;

class Program
{
    static void Main()
    {
        CUtilities metodi = new CUtilities();

        string[] args = new string[6];                                  // per debug
        args[0] = "C:\\Users\\Scansione1\\Desktop";                     // args[0] ---> path
        args[1] = "json";                                               // args[1] ---> tipo file export
        args[2] = "test";                                               // args[2] ---> nome file export
        args[3] = "C:\\Users\\Scansione1\\Desktop\\";                   // args[3] ---> percorso file export
        args[4] = "C:\\Users\\Scansione1\\Desktop\\test1.json";         // args[4] ---> percorso 1° file di confronto
        args[5] = "C:\\Users\\Scansione1\\Desktop\\test2.json";         // args[5] ---> percorso 2° file di confronto


        List<CFile> files = new List<CFile>();
        CListaTipi listatipi = new CListaTipi();
        DirectoryInfo dirInfo = new DirectoryInfo(args[0]);
        long pesoCartella = metodi.CalcolaPesoCartella(dirInfo.FullName);
        CCartella Droot = new CCartella(dirInfo.Name, pesoCartella.ToString(), 100, args[0]);
         
        scansiona(dirInfo, Droot, files, listatipi, Convert.ToDouble(pesoCartella));

        metodi.calcolaPesanti(files);
        Console.WriteLine("i 10 file più pesanti");
        for (int i = 0; i < 10; i++) { Console.WriteLine(files[i].nome + " | " + Int32.Parse(files[i].dimensione).Bytes().Humanize()); }

        //listatipi.visualizza();
        listatipi.calcolaDiff(pesoCartella);
        listatipi.visualizzaOrdinati(1);
        Console.WriteLine("\nTIPI DI ESTENSIONI (" + listatipi.nEstensioni + ")");
        for (int i = 0; i < listatipi.listaTipi.Count; i++)
        {
            Console.WriteLine("\nEstensione: " + listatipi.listaTipi[i].estensione + "\nQuantita: " + listatipi.listaTipi[i].quantita + "\nPeso: " + listatipi.listaTipi[i].peso.Bytes().Humanize() + "\nPercentuale: " + listatipi.listaTipi[i].perc);
        }

        if (args[1].ToLower().Equals("json"))
        {
            metodi.toJson(args[3] + args[2] + "reportFile", ref Droot);
            metodi.toJson(args[3] + args[2] + "reportTipi", ref listatipi);
        } else if (args[1].ToLower().Equals("xml"))
        {
            metodi.toXml(args[3] + args[2], ref Droot);
        }

        confronta(args[4], args[5], args[3] + "luca");
    }
    public static void scansiona(DirectoryInfo dirInfo, CCartella Droot, List<CFile> lfile, CListaTipi listatipi, double ptot) //possibile errore
    {
        CUtilities metodi = new CUtilities();
        foreach (FileInfo file in dirInfo.GetFiles())
        {
            double peso = Convert.ToDouble(file.Length);
            double perc = (peso / ptot) * 100;
            CFile ftmp = new CFile(file.Name, file.Length.ToString(), Math.Round(perc, 2), file.DirectoryName);          //string perc = per.ToString("P", CultureInfo.InvariantCulture);
            lfile.Add(ftmp);
            Droot.pushFile(ftmp);

            CtipiFile tmp = new CtipiFile(file.Extension, Convert.ToInt32(file.Length), 1, 0);
            listatipi.add(tmp);
        }

        foreach (DirectoryInfo subDir in dirInfo.GetDirectories())
        {
            double peso = Convert.ToDouble(metodi.CalcolaPesoCartella(subDir.FullName));
            double perc = (peso / ptot) * 100;
            CCartella dtmp = new CCartella(subDir.Name, peso.ToString(), Math.Round(perc, 2), subDir.FullName);                        //string perc = per.ToString("P", CultureInfo.InvariantCulture); 

            Droot.pushDirectory(dtmp);
            scansiona(subDir, dtmp, lfile, listatipi, ptot);
        }
    }
    public static void confronta(string path1, string path2, string p)
    {
        Console.WriteLine("confronto");
        CUtilities metodi = new CUtilities();
        FileInfo fileInfo = new FileInfo(path1);
        FileInfo fileInfo2 = new FileInfo(path2);
        CListaTipi lst1;
        CListaTipi lst2;
        using (StreamReader file = File.OpenText(path1))
        {
            JsonSerializer serializer = new JsonSerializer();
            lst1 = (CListaTipi)serializer.Deserialize(file, typeof(CListaTipi));
        }
        using (StreamReader file = File.OpenText(path2))
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

        metodi.toJson(p, ref differenze);
    }
}