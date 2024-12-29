using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DigitalisKonyvtarkezelo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new LoginPage());
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.Content is LoginPage)
            {
                this.Width = 400;
                this.Height = 300;
            }
            else if (e.Content is RegisztraciosOldal)
            {
                this.Width = 400;
                this.Height = 300;
            }
            else if ( e.Content is BookManager)
            {
                this.Width = 800;
                this.Height = 600;
            }
        }
    }

}