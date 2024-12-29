using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace DigitalisKonyvtarkezelo
{
    public partial class RegisztraciosOldal : Page
    {

        public Felhasznalo felhasznaloKezelo = new Felhasznalo();
        public Felhasznalo FelhasznaloKezelo
        {
            get { return felhasznaloKezelo; }
        }
        public RegisztraciosOldal()
        {
            InitializeComponent();
            felhasznaloKezelo.LoadUsersFromFile();
        }

        public void DebugShowUsers()
        {
            string userList = "";
            foreach (var user in felhasznaloKezelo.felhasznalok)
            {
                userList += user.ToString() + "\n";
            }
            MessageBox.Show(userList);
        }
        private void ClearFields()
        {
            UsernameTextBox.Text = "";
            EmailTextBox.Text = "";
            PasswordBox.Password = "";

        }
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;
            string feedback = felhasznaloKezelo.Register(username, password, email);

            if (feedback != "success")
            {
                MessageBox.Show(feedback);
                return;
            }
            MessageBox.Show("Sikeres regisztráció!");

            DebugShowUsers();

            ClearFields();

            
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }
    }
}