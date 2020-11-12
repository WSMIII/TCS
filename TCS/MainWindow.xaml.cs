using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
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

namespace TCS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly public struct PageStates
        {
            public Home home { get; }
            public Settings settings { get; }
            public Create create { get; }
            public Open open { get; }
            public Redeem redeem { get; }

            public PageStates(Home in_home, Settings in_settings, Create in_create, Open in_open, Redeem in_redeem)
            {
                this.home = in_home;
                this.settings = in_settings;
                this.create = in_create;
                this.open = in_open;
                this.redeem = in_redeem;
            }
        }
        public PageStates states;
        public int lastPage = 1;

        public MainWindow()
        {
            InitializeComponent();
            Core.initCore();

            states = new PageStates(new Home(this), new Settings(this), new Create(this), new Open(this), new Redeem(this));
            this.Content = states.home;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Core.deCore(states.settings);
        }
    }
}
