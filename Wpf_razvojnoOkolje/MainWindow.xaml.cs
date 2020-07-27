using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace Wpf_razvojnoOkolje
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int fileNameLength = 12;

        private bool isDataSaved;
        private Projekt Projekt;
        private Datoteka currDat;
        private List<Datoteka> DatotekeKoda;
        private List<Metoda> Metode;

        public ObservableCollection<Item> Datoteke { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            if (Properties.Settings.Default.ProgLangs == null)
                Properties.Settings.Default.ProgLangs = new System.Collections.ObjectModel.ObservableCollection<ProgJezik>();

            if (Properties.Settings.Default.Ogrodja == null)
                Properties.Settings.Default.Ogrodja = new System.Collections.ObjectModel.ObservableCollection<Ogrodje>();

            ChangeTheme();
            
            

            //this.Closing += new CancelEventHandler(this.Window_Closing); 
            this.isDataSaved = true;
            //this.isDataSaved = false;

            Datoteke = new ObservableCollection<Item>();
            currDat = new Datoteka();
            DatotekeKoda = new List<Datoteka>();
            Projekt = new Projekt();
            Metode = new List<Metoda>();

        }

        /// <summary>
        /// Za spreminjanje teme/stila 
        /// </summary>
        private void ChangeTheme()
        {
            string rsrc = "";
            if (Properties.Settings.Default.Theme == "Dark")
                rsrc = "Themes/DarkThemeDictionary.xaml";
            else if (Properties.Settings.Default.Theme == "LightBlue")
                rsrc = "Themes/LightBlueThemeDictionary.xaml";
            else
                rsrc = "Themes/DefaultThemeDictionary.xaml";

            var currentRsrc = new Uri(rsrc, UriKind.RelativeOrAbsolute);
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = currentRsrc });
        }

        /// <summary>
        /// metoda se proži ob window closing eventu, uporabimo jo za prestreg zaprtja porgrama in potencialno shranimo projekt, v primeru da ni
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure about that?", "studio se bo zaprl", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (!isDataSaved)
                {
                    MessageBoxResult messageBoxResultShrani = MessageBox.Show("Shrani Projekt?", "projekt se bo shranil", System.Windows.MessageBoxButton.YesNo);
                    if (messageBoxResultShrani == MessageBoxResult.Yes)
                    {
                        ShraniProj();
                    }
                }
            }
            else
                e.Cancel = true;

            if (!e.Cancel)
                Environment.Exit(0);

        }

        private void TreeViewItem_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        /// <summary>
        /// odpre window newProjWindow, ki služi za ustvarjanje novega projekta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click_Ustvari_Projekt(object sender, RoutedEventArgs e)
        {
            
            NewProjWindow newProjWindow = new NewProjWindow();
            newProjWindow.ShowDialog();
            if(newProjWindow.DialogResult == true)
            {
                Projekt = newProjWindow.Projekt;
                SetupProject(Projekt.Lokacija);
                //ShraniProj();
                
                
            }

            //var itemProvider = new ItemProvider();
            //string startupPath = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            //var items = itemProvider.GetItems(startupPath);

            //treeView.Items.Clear();
            //DataContext = items;
        }

        /// <summary>
        /// za pridobivanje random alfanumericnega stringa
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private static string RandomString(int length)
        {
            Random random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// doda file v "projectPath/Data", ta file je uporabljen kot .cs datoteka v visual studiu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_dodajDatoteko_Click(object sender, RoutedEventArgs e)
        {
            //string path = Directory.GetCurrentDirectory() + "/dodaneDatoteke";
            //if (!Directory.Exists(path))
            //    Directory.CreateDirectory(path);

            string path = Directory.GetParent(Projekt.Lokacija) + "/Data";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            Datoteka dat = new Datoteka();
            dat.Naziv = RandomString(fileNameLength); // TODO:medium priority - spremen da bo custom pa to
            dat.Data = "static void Main(string[] args)\n{\n\n}";

            XmlSerializer serializerDat = new XmlSerializer(typeof(Datoteka));
            string datFolder = Directory.GetParent(Projekt.Lokacija) + "/Data";

            using (FileStream fileStream = new FileStream(datFolder + "/" + dat.Naziv + ".xml", FileMode.Create))
            {
                serializerDat.Serialize(fileStream, dat);
            }

            DatotekeKoda.Add(dat);

            //string filename = RandomString(12) + ".txt";
            //if (!System.IO.File.Exists(path + "/" + filename))
            //{
            //    using (System.IO.FileStream fs = System.IO.File.Create(path + "/" + filename))
            //    {
            //        for (byte i = 0; i < 100; i++)
            //        {
            //            fs.WriteByte(i);
            //        }
            //    }
            //}

            UpdateTree();
        }

        /// <summary>
        /// Posodobi treeView strukturo z datotekami in direktoriji iz projekta
        /// </summary>
        private void UpdateTree()
        {
            var itemProvider = new ItemProvider();
            string startupPath = Directory.GetParent(Projekt.Lokacija).FullName; //Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            List<Item> items = itemProvider.GetItems(startupPath);

            if (treeView.ItemsSource == null)
            {
                treeView.Items.Clear();
            }

            Datoteke = new ObservableCollection<Item>(items);
            treeView.DataContext = null;
            treeView.DataContext = this;
        }

        //private string SelectedItemTreePath(TreeView tree)
        //{
        //    FileItem fi = new FileItem();
        //    DirectoryItem di = new DirectoryItem();
        //    if (treeView.SelectedItem.GetType().Equals(fi.GetType()))
        //    {
        //        //return fi.Path
        //    }
        //    else if (treeView.SelectedItem.GetType().Equals(di.GetType()))
        //    {

        //    }
        //    else
        //        MessageBox.Show("izbrati je potrebno directory ali file v direktoriju dodatneDatoteke");

        //    return "";
        //}

        

        private void button_brisiDatoteko_Click(object sender, RoutedEventArgs e)
        {
            //DirectoryItem di = new DirectoryItem();
            //if (treeView.SelectedItem.GetType().Equals(fi.GetType()))
            //{
            //    fi = (FileItem)treeView.SelectedItem;
            //    if (fi.Path.EndsWith(".txt") && fi.Path.Contains("dodaneDatoteke"))
            //    {
            //        MessageBoxResult messageBoxResult = MessageBox.Show("zbrisi: " + fi.Path, "confirm", MessageBoxButton.OKCancel);
            //        if (messageBoxResult == MessageBoxResult.OK)
            //        {
            //            FileInfo finfo = new FileInfo(fi.Path);
            //            finfo.Delete();
            //        }
            //    }
            //}
            //else if (treeView.SelectedItem.GetType().Equals(di.GetType()))
            //{
            //    di = (DirectoryItem)treeView.SelectedItem;
            //    if (di.Path.EndsWith("dodaneDatoteke"))
            //    {
            //        MessageBoxResult messageBoxResult = MessageBox.Show("zbrisi: " + di.Path, "confirm", MessageBoxButton.OKCancel);
            //        if (messageBoxResult == MessageBoxResult.OK)
            //        {
            //            DirectoryInfo dinfo = new DirectoryInfo(di.Path);
            //            dinfo.Delete(true);
            //        }
            //    }
            //}
            //else
            //    MessageBox.Show("izbrati je potrebno directory ali file v direktoriju dodatneDatoteke");

            FileItem fi = new FileItem();
            if (treeView.SelectedItem != null)
            {
                var item = treeView.SelectedItem;
                if (item.GetType().Equals(fi.GetType()))
                {
                    fi = (FileItem)item;
                    if (fi.Path.EndsWith(".xml") && fi.Path.Contains(@"\Data\"))
                    {
                        MessageBoxResult messageBoxResult = MessageBox.Show("zbrisi: " + fi.Path, "confirm", MessageBoxButton.OKCancel);
                        if (messageBoxResult == MessageBoxResult.OK)
                        {
                            FileInfo finfo = new FileInfo(fi.Path);
                            finfo.Delete();
                        }
                    }
                    else
                        MessageBox.Show("Izbran mora biti '.xml' file v direktoriju 'Data'");
                }
                else
                    MessageBox.Show("Izbran mora biti '.xml' file v direktoriju 'Data'");
            }
            else
                MessageBox.Show("Izbran mora biti '.xml' file v direktoriju 'Data'");

            UpdateTree();
            UpdateLevo();
            richTextBox_koda.Document.Blocks.Clear();
        }

        /// <summary>
        /// odpre window nastavitve
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click_Nastavitve(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog();
        }

        private void MenuItem_Click_OdpriProjekt(object sender, RoutedEventArgs e)
        {
            using(System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog()) // nek ambiguious stuff je biu pa se mi ni dal changat
            {
                ofd.Filter = "XML-File | *.xml";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    SetupProject(ofd.FileName);
                }
            }

        }

        /// <summary>
        /// odpre .xml file ki vsebuje podatke o projektu(lokacijo, ime, avtorja, jezik, ogrode) in pripravi UI, tree view populatea z datotekami projekta in pripravi projekt za delo z njim
        /// </summary>
        /// <param name="projektPath"></param>
        private void SetupProject(string projektPath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Projekt));
            using(FileStream fileStream = new FileStream(projektPath, FileMode.Open))
            {
                Projekt projekt = (Projekt)serializer.Deserialize(fileStream);
                this.Projekt = projekt;
                this.Projekt.Lokacija = projektPath; //updatamo path ce je slucjan bla dat premaknjena pa pol odprta TODO: test this
                                                     //MessageBox.Show(Projekt.Naziv);

                UpdateTree();
            }

            var dirInfo = new DirectoryInfo(Directory.GetParent(projektPath) + "/Data");

            DatotekeKoda.Clear();
            XmlSerializer serializerDat = new XmlSerializer(typeof(Datoteka));
            foreach (var file in dirInfo.GetFiles())
            {
                using (FileStream fileStream = new FileStream(file.FullName, FileMode.Open))
                {
                    DatotekeKoda.Add((Datoteka)serializerDat.Deserialize(fileStream));
                }
            }

        }

        /// <summary>
        /// Uporablja se za organizacija listView
        /// </summary>
        class Metoda
        {
            public string Ime { get; set; }
            public List<string> Spremenljivke { get; set; }

            public Metoda()
            {
                Spremenljivke = new List<string>();
            }
        }


        /// <summary>
        /// posodobi listview na levi strani vmesnika z metodami in spremenljivkami v metodi
        /// </summary>
        private void UpdateLevo()
        {
            string t = currDat.Data;
            Metode = new List<Metoda>();
            if (t == "")
            {
                UpdateListView();
            }
            
            var text = t.Split(' ','\n');

            string[] visibility = { "public", "private", "protected", "static" };
            string[] returnType = { "void", "int", "string", "double", "bool", "byte", "sbyte", "char", "decimal", "float", "long", "object", "short" };
            string[] types = { "int", "string", "double", "bool", "byte", "sbyte", "char", "decimal", "float", "long", "object", "short" };

            //List<Metoda> metode = new List<Metoda>();
            

            for (int i = 0; i < text.Length; i++)
            {
                if (i < text.Length)
                {
                    if (visibility.Contains(text[i]) && returnType.Contains(text[i + 1]))
                    {

                        int k = i + 2;
                        string ime = "";

                        while (k < text.Length)
                        {
                            ime += text[k];
                            if (text[k].Contains(')'))
                                break;
                            else
                                ime += " ";
                            k++;
                        }

                        int j = k + 1;

                        int count = 1; // counta { } da ti lahka vrne vse spremenljivke v "scope-u"
                        List<string> spremenljivke = new List<string>();
                        while (j < text.Length && count > 0)
                        {
                            if (text[j] == "{")
                                count++;
                            else if (text[j] == "}")
                                count--;

                            if (types.Contains(text[j]))
                            {
                                spremenljivke.Add(text[j] +" " +text[j+1]);
                                j++;
                            }
                            j++;
                        }
                        Metode.Add(new Metoda()
                        {
                            Ime = ime,
                            Spremenljivke = spremenljivke
                        });
                    }
                }
            }

            //var listViewItem = new ListViewItem(new[] { "id123", "asd" });
            //listView_RazredneMetodeSpremenljivke
            //metode.ForEach(a => )

            //listView_RazredneMetodeSpremenljivke.Items.Add(metode[0].Ime).SubItems.AddRange(metode[0].Spremenljivke);

            //foreach (var a in Metode)
            //{
            //    MessageBox.Show(a.Ime);
            //    foreach (var b in a.Spremenljivke)
            //    {
            //        MessageBox.Show(b);
            //    }
            //}
            //listView_RazredneMetodeSpremenljivke.ItemsSource = Metode;
            UpdateListView();
        }

        /// <summary>
        /// metoda služi da populateamo listview z metodami in spremenljivkami iz ene datoteke
        /// </summary>
        private void UpdateListView()
        {
                List<ListItem> li = new List<ListItem>();
                Queue<Metoda> metodas = new Queue<Metoda>(Metode);
                Queue<string> spremenljivke;

                if (listView_RazredneMetodeSpremenljivke.SelectedItem != null)
                {
                    if (((ListItem)listView_RazredneMetodeSpremenljivke.SelectedItem).Metoda != "")
                        spremenljivke = new Queue<string>(Metode.SingleOrDefault(a => a.Ime == ((ListItem)listView_RazredneMetodeSpremenljivke.SelectedItem).Metoda.ToString()).Spremenljivke);
                    else
                        spremenljivke = new Queue<string>();
                }
                else
                    spremenljivke = new Queue<string>();
                while (metodas.Count != 0 || spremenljivke.Count != 0)
                {
                    ListItem listItem = new ListItem();
                    if (metodas.Count != 0)
                        listItem.Metoda = metodas.Dequeue().Ime;
                    if (spremenljivke.Count != 0)
                        listItem.Spremenljivka = spremenljivke.Dequeue();
                    li.Add(listItem);
                }

                listView_RazredneMetodeSpremenljivke.ItemsSource = li;
            
        }

        /// <summary>
        /// metoda se proži ob spremembi izbire v listView z metodami in spremenljivaki, in sicer posodboi listView z novimi spremenljivkami iz izbrane metode (scope-a)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView_RazredneMetodeSpremenljivke_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //TODO:medium priority - ko das shrani projekt more shranit trenutn file pa ga updatat z novimi metodami/spremenjlivkami

            if (listView_RazredneMetodeSpremenljivke.SelectedItem != null)
            {
                //MessageBox.Show("triggrd");
                List<ListItem> li = new List<ListItem>();
                Queue<Metoda> metodas = new Queue<Metoda>(Metode);
                Queue<string> spremenljivke;

                if (listView_RazredneMetodeSpremenljivke.SelectedItem != null && ((ListItem)listView_RazredneMetodeSpremenljivke.SelectedItem).Metoda != null)
                {
                    if (((ListItem)listView_RazredneMetodeSpremenljivke.SelectedItem).Metoda != "")
                        spremenljivke = new Queue<string>(Metode.SingleOrDefault(a => a.Ime == ((ListItem)listView_RazredneMetodeSpremenljivke.SelectedItem).Metoda.ToString()).Spremenljivke);
                    else
                        spremenljivke = new Queue<string>();
                }
                else
                    spremenljivke = new Queue<string>();
                while (metodas.Count != 0 || spremenljivke.Count != 0)
                {
                    ListItem listItem = new ListItem();
                    if (metodas.Count != 0)
                        listItem.Metoda = metodas.Dequeue().Ime;
                    if (spremenljivke.Count != 0)
                        listItem.Spremenljivka = spremenljivke.Dequeue();
                    li.Add(listItem);
                }

                listView_RazredneMetodeSpremenljivke.ItemsSource = li;
            }
        }

        /// <summary>
        /// Uporablja se za organizacija listView
        /// </summary>
        class ListItem
        {
            public string Metoda { get; set; }
            public string Spremenljivka { get; set; }
        }

        private void MenuItem_Click_ShraniProjekt(object sender, RoutedEventArgs e)
        {
            ShraniProj();
        }

        /// <summary>
        /// metoda shrani projekt in asociirane .xml filee
        /// </summary>
        private void ShraniProj()
        {
            //string folder = Directory.GetParent(projekt.Lokacija).ToString() + "/" + projekt.Naziv;
            //Directory.CreateDirectory(folder);
            //Directory.CreateDirectory(folder + "/Data");

            string projFolder = Directory.GetParent(Projekt.Lokacija).ToString();

            currDat.Data = new TextRange(richTextBox_koda.Document.ContentStart, richTextBox_koda.Document.ContentEnd).Text;

            XmlSerializer serializerProj = new XmlSerializer(typeof(Projekt));
            using (FileStream fileStream = new FileStream(Projekt.Lokacija, FileMode.Create)) //shrani projekt
            {
                serializerProj.Serialize(fileStream, Projekt);
            }

            XmlSerializer serializerDat = new XmlSerializer(typeof(Datoteka));
            string datFolder = Directory.GetParent(Projekt.Lokacija) + "/Data";

            foreach (var d in DatotekeKoda)
            {
                using (FileStream fileStream = new FileStream(datFolder + "/" + d.Naziv + ".xml", FileMode.Create)) //shrani datoteke za kodo
                {
                    serializerDat.Serialize(fileStream, d);
                }
            }
        }

        /// <summary>
        /// uporabljamo za izbiro durgega .xml filea
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            FileItem fi = new FileItem();
            DirectoryItem di = new DirectoryItem();
            if (treeView.SelectedItem.GetType().Equals(fi.GetType()))
            {
                var naziv = ((FileItem)treeView.SelectedItem).Name;
                naziv = naziv.Substring(0, fileNameLength);
                foreach(var f in DatotekeKoda)
                {
                    if (naziv == f.Naziv)
                    {
                        currDat.Data = new TextRange(richTextBox_koda.Document.ContentStart, richTextBox_koda.Document.ContentEnd).Text;
                        richTextBox_koda.Document.Blocks.Clear();
                        currDat = f;
                        richTextBox_koda.Document.Blocks.Add(new Paragraph(new Run(f.Data)));
                        break;
                    }
                }
            }

            UpdateLevo();
            //else if (treeView.SelectedItem.GetType().Equals(di.GetType()))
            //{

            //}
        }

        private void richTextBox_koda_TextChanged(object sender, TextChangedEventArgs e)
        {
            isDataSaved = false;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Window_Closing(this, new CancelEventArgs());
        }




        //[vaja2 tocka1]
        //    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        //    {

        //    }
        //}

        //public class ExitDogodek
        //{
        //    public delegate void ExitHandler(object sender, ExitEventArgs e);
        //    public event ExitHandler OnExit;

        //    public static void Exit()
        //    {

        //    }
        //    private void exit()
        //    {
        //        if (OnExit == null) return;
        //    }
    }

    /// <summary>
    /// Uporablja se za organizacija treeView
    /// </summary>
    public class Item
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }

    /// <summary>
    /// Uporablja se za organizacija treeView
    /// </summary>
    public class FileItem : Item
    {

    }

    /// <summary>
    /// Uporablja se za organizacija treeView
    /// </summary>
    public class DirectoryItem : Item
    {
        public List<Item> Items { get; set; }

        public DirectoryItem()
        {
            Items = new List<Item>();
        }
    }

    /// <summary>
    /// Uporablja se za organizacija treeView
    /// </summary>
    public class ItemProvider
    {
        public List<Item> GetItems(string path)
        {
            var items = new List<Item>();

            var dirInfo = new DirectoryInfo(path);

            foreach (var directory in dirInfo.GetDirectories())
            {
                var item = new DirectoryItem
                {
                    Name = directory.Name,
                    Path = directory.FullName,
                    Items = GetItems(directory.FullName)
                };

                items.Add(item);
            }

            foreach (var file in dirInfo.GetFiles())
            {
                var item = new FileItem
                {
                    Name = file.Name,
                    Path = file.FullName
                };

                items.Add(item);
            }

            return items;
        }
    }
}
