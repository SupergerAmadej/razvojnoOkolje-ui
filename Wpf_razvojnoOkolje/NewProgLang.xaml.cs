using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Wpf_razvojnoOkolje
{
    /// <summary>
    /// Interaction logic for NewProgLang.xaml
    /// </summary>
    public partial class ProgLang : Window
    {
        public ProgLang()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            ProgJezik pg = new ProgJezik
            {
                Naziv = textBoxNazivProgLang.Text
            };

            Properties.Settings.Default.ProgLangs.Add(pg);
            Properties.Settings.Default.Save();
        }
    }
}
