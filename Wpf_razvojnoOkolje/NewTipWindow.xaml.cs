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
    /// Interaction logic for NewTipWindow.xaml
    /// </summary>
    public partial class TipWindow : Window
    {
        public ProgJezik PJ;
        public TipWindow()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            TipApp ta = new TipApp
            {
                Naziv = textBoxTipApp.Text
            };

            var asd = Properties.Settings.Default.ProgLangs.SingleOrDefault(a => a.Equals(PJ));
            if (asd.TipApps == null)
                asd.TipApps = new List<TipApp>();
            asd.TipApps.Add(ta);
            Properties.Settings.Default.Save();
        }
    }
}
