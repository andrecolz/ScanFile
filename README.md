# ScanFile CMD

```
static void Main(string[] args)
```
- args[0] ---> path di ricerca
- args[1] ---> tipo file export
- args[2] ---> nome file export
- args[3] ---> percorso file export
- args[4] ---> percorso 1° file di confronto
- args[5] ---> percorso 2° file di confronto


# ScanFile GUI
- <img src="ScanFileGUI/ScanFile/file.png" width="75" height="75"> seleziona il path di ricerca
- <img src="ScanFileGUI/ScanFile/export.png" width="75" height="75"> pulsante per l'export
- <img src="ScanFileGUI/ScanFile/import.png" width="75" height="75"> pulsante per l'import
- <img src="ScanFileGUI/ScanFile/compare.png" width="75" height="75"> pulsante per confrontare due file json


# ScanFile LIB
- `CCartella` è la classe principale e contiene oltre che a degli attributi e due liste una di CCartella e una di CFile
- `CFile` è la classe grazie al quale si possono salvare le informazioni base di un file
- `ClistaTipi` è la classe che contiene una lista di tutte le estensioni trovate dei file (.pdf ecc)
- `CtipiFile` è la classe grazie al quale si possono salvare le informazioni da salvare in ClistaTipi
- `CCartella` è una classe che contiene una serie di metodi come `toJson()` e `CalcolaPesoCartella()`
