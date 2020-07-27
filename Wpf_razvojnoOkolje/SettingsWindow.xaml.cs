using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Wpf_razvojnoOkolje
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        
        public SettingsWindow()
        {
            InitializeComponent();
            listBoxProgJeziki.ItemsSource = Properties.Settings.Default.ProgLangs;
            listBoxProgJeziki.DisplayMemberPath = "Naziv";

            listBoxTipiAplikacij.DisplayMemberPath = "Naziv";

            listBoxOgrodja.ItemsSource = Properties.Settings.Default.Ogrodja;
            listBoxOgrodja.DisplayMemberPath = "Naziv";

            List<string> themes = new List<string>();
            themes.Add("Default");
            themes.Add("Dark");
            themes.Add("LightBlue");
            comboBox_themes.ItemsSource = themes;
        }

        private void buttonDodajProg_Click(object sender, RoutedEventArgs e)
        {
            ProgLang newProgLang = new ProgLang();
            newProgLang.ShowDialog();
        }

        private void listBoxProgJeziki_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxProgJeziki.SelectedItem != null)
            {
                listBoxTipiAplikacij.ItemsSource = ((ProgJezik)listBoxProgJeziki.SelectedItem).TipApps;
            }
            else
                listBoxTipiAplikacij.ItemsSource = null;
        }

        private void buttonDodajTipiApp_Click(object sender, RoutedEventArgs e)
        {
            TipWindow ntw = new TipWindow();
            ntw.PJ = (ProgJezik)listBoxProgJeziki.SelectedItem;
            ntw.ShowDialog();
        }

        private void buttonBrisiProg_Click(object sender, RoutedEventArgs e)
        {
            if ((ProgJezik)listBoxProgJeziki.SelectedItem != null)
            {
                var asd = Properties.Settings.Default.ProgLangs.SingleOrDefault(a => a.GetHashCode().Equals(((ProgJezik)listBoxProgJeziki.SelectedItem).GetHashCode()));
                Properties.Settings.Default.ProgLangs.Remove(asd);
                Properties.Settings.Default.Save();
            }
        }

        private void buttonBrisiTipiApp_Click(object sender, RoutedEventArgs e)
        {
            if ((ProgJezik)listBoxProgJeziki.SelectedItem != null)
            {
                var asd = Properties.Settings.Default.ProgLangs.SingleOrDefault(a => a.GetHashCode().Equals(((ProgJezik)listBoxProgJeziki.SelectedItem).GetHashCode()));
                if (listBoxTipiAplikacij.SelectedItem != null)
                {
                    var asdd = asd.TipApps.SingleOrDefault(a => a.GetHashCode().Equals(((TipApp)listBoxTipiAplikacij.SelectedItem).GetHashCode()));
                    asd.TipApps.Remove(asdd);
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void buttonDodajOgrodja_Click(object sender, RoutedEventArgs e)
        {
            OgrodjeWindow ogrodjeWindow = new OgrodjeWindow();
            ogrodjeWindow.ShowDialog();
        }

        private void buttonBrisiOgrodja_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxOgrodja.SelectedItem != null)
            {
                var asd = Properties.Settings.Default.Ogrodja.SingleOrDefault(a => a.GetHashCode().Equals(((Ogrodje)listBoxOgrodja.SelectedItem).GetHashCode()));
                Properties.Settings.Default.Ogrodja.Remove(asd);
                Properties.Settings.Default.Save();
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string fileName = @"C:\Users\super\source\repos\Wpf_razvojnoOkolje\Wpf_razvojnoOkolje\Themes\DarkThemeDictionary.xaml";
            if (System.IO.File.Exists(fileName))
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    ResourceDictionary dic = (ResourceDictionary)XamlReader.Load(fs);
                    Application.Current.Resources.MergedDictionaries.Clear();
                    Application.Current.Resources.MergedDictionaries.Add(dic);
                }
            }
        }

        private void button_apply_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox_themes.SelectedItem != null)
            {
                //MessageBox.Show(comboBox_themes.SelectedItem.ToString());
                Properties.Settings.Default.Theme = comboBox_themes.SelectedItem.ToString();
                Properties.Settings.Default.Save();
                ChangeTheme();
            }
        }

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

        //TODO:ultralow priority - implement uredi button za tipi + prog jezik + orgrodja
    }
    
}
