using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace DigitalisKonyvtarkezelo
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void ClearFields()
        {
            UsernameTextBox.Text = "";
            PasswordBox.Password = "";
        }   
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegisztraciosOldal());
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {

            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            RegisztraciosOldal regisztraciosOldal = new RegisztraciosOldal();
            Felhasznalo felhasznalokezelo = regisztraciosOldal.FelhasznaloKezelo;

            foreach (var user in felhasznalokezelo.felhasznalok)
            {
                if (user.Felhasznalonev == username && user.Jelszo == password)
                {
                    MessageBox.Show("Sikeres bejelentkezés!");
                    NavigationService.Navigate(new BookManager(user));
                    return;
                }
            }
            MessageBox.Show("Hibás felhasználónév vagy jelszó!");


        }
    }
}