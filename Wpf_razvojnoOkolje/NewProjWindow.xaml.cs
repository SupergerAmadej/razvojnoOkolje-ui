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
//using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace Wpf_razvojnoOkolje
{
    /// <summary>
    /// Interaction logic for NewProjWindow.xaml
    /// </summary>
    public partial class NewProjWindow : Window
    {
        public Projekt Projekt;
        public NewProjWindow()
        {
            InitializeComponent();
            comboBoxProjVrsta.IsEnabled = false;
            comboBoxProjProgJezik.ItemsSource = Properties.Settings.Default.ProgLangs;
            comboBoxProjProgJezik.DisplayMemberPath = "Naziv";

            comboBoxProjOgrodje.ItemsSource = Properties.Settings.Default.Ogrodja;
            comboBoxProjOgrodje.DisplayMemberPath = "Naziv";
        }

        private void buttonProjBrowseLocation_Click(object sender, RoutedEventArgs e)
        {
            //SaveFileDialog dialog = new SaveFileDialog();
            //dialog.Filter = "XML-File | *.xml";
            //if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    textBlockProjLokacija.Text = dialog.FileName;
            //}

            using(var dialog = new FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                textBlockProjLokacija.Text = dialog.SelectedPath;
            }
        }

        private void comboBoxProjProgJezik_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(comboBoxProjProgJezik.SelectedItem != null)
            {
                comboBoxProjVrsta.IsEnabled = true;
                comboBoxProjVrsta.ItemsSource = ((ProgJezik)comboBoxProjProgJezik.SelectedItem).TipApps;
                comboBoxProjVrsta.DisplayMemberPath = "Naziv";
            }
            else
                comboBoxProjVrsta.IsEnabled = false;

        }

        private void buttonProjCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonProjConfirm_Click(object sender, RoutedEventArgs e)
        {
            if(textBoxProjNaziv.Text != "" && textBoxProjAvtor.Text != "" && comboBoxProjProgJezik.SelectedItem != null
                && comboBoxProjVrsta.SelectedItem != null && comboBoxProjOgrodje.SelectedItem != null)
            {
                Projekt projekt = new Projekt
                {
                    Naziv = textBoxProjNaziv.Text,
                    Avtor = textBoxProjAvtor.Text,
                    ProgramskiJezik = (ProgJezik)comboBoxProjProgJezik.SelectedItem,
                    VrstaProjekta = (TipApp)comboBoxProjVrsta.SelectedItem,
                    Ogrodje = (Ogrodje)comboBoxProjOgrodje.SelectedItem,
                    Lokacija = textBlockProjLokacija.Text
                };

                //string folder = Directory.GetParent(projekt.Lokacija).ToString() + "/" + projekt.Naziv; //ce bi meu file
                string folder = projekt.Lokacija + "/" + projekt.Naziv;
                Directory.CreateDirectory(folder);
                Directory.CreateDirectory(folder + "/Data");

                XmlSerializer serializer = new XmlSerializer(typeof(Projekt));
                projekt.Lokacija = folder + "/" + projekt.Naziv + "-info.xml";
                using (FileStream fileStream = new FileStream(projekt.Lokacija, FileMode.Create))
                {
                    serializer.Serialize(fileStream, projekt);
                }
                Projekt = projekt;
                DialogResult = true;
            }
        }
    }
}
