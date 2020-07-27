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
    /// Interaction logic for NewOgrodjeWindow.xaml
    /// </summary>
    public partial class OgrodjeWindow : Window
    {
        public OgrodjeWindow()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            Ogrodje ogrodje = new Ogrodje
            {
                Naziv = textBoxOgrodje.Text
            };

            Properties.Settings.Default.Ogrodja.Add(ogrodje);
            Properties.Settings.Default.Save();
        }
    }
}
