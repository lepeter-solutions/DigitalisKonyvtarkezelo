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

namespace DigitalisKonyvtarkezelo
{
    /// <summary>
    /// Interaction logic for UserControl.xaml
    /// </summary>
    public partial class UserControl : Window
    {
        Felhasznalo felhasznaloKezelo;
        Felhasznalo currentlySelectedUser;
        Felhasznalo loggedInAdmin;
        public UserControl(Felhasznalo loggedInUser)
        {
            InitializeComponent();
            RegisztraciosOldal regisztraciosOldal = new RegisztraciosOldal();
            Felhasznalo felhasznalokezelo = regisztraciosOldal.FelhasznaloKezelo;
            felhasznaloKezelo = felhasznalokezelo;
            loggedInAdmin = loggedInUser;

            foreach (var user in felhasznalokezelo.felhasznalok)
            {
                felhasznaloLista.Items.Add(new ListBoxItem { Content = user.Felhasznalonev + " " + user.Email });
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentlySelectedUser.Felhasznalonev == loggedInAdmin.Felhasznalonev)
            {
                MessageBox.Show("Saját magad nem törölheted! Szívás!");
                return;
            }
            string response = currentlySelectedUser.DeleteUser(felhasznaloKezelo, currentlySelectedUser.Felhasznalonev);
            if (response != "success")
            {
                MessageBox.Show(response);
                return;
            }
            MessageBox.Show("Felhasználó törölve!");
            ClearFields();
            UnselectUser();
            RefreshList();
        }

        private void FillFieldsWithUser(Felhasznalo user)
        {
            felhasznalonevMezo.Text = user.Felhasznalonev;
            emailMezo.Text = user.Email;
            jelszoMezo.Password = user.Jelszo;
            admincheck.IsChecked = user.IsAdmin;
        }

        private void RefreshList()
        {
            felhasznaloLista.Items.Clear();
            foreach (var user in felhasznaloKezelo.felhasznalok)
            {
                felhasznaloLista.Items.Add(new ListBoxItem { Content = user.Felhasznalonev + " " + user.Email });
            }

        }

        private void felhasznaloLista_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (felhasznaloLista.SelectedItem != null)
            {

                Felhasznalo selectedUser = felhasznaloKezelo.felhasznalok[felhasznaloLista.SelectedIndex];
                currentlySelectedUser = selectedUser;
                FillFieldsWithUser(selectedUser);
            }
        }

        private void ClearFields()
        {
            felhasznalonevMezo.Text = "";
            emailMezo.Text = "";
            jelszoMezo.Password = "";
            admincheck.IsChecked = false;
        }

        private void UnselectUser()
        {
            felhasznaloLista.SelectedItem = null;
            currentlySelectedUser = null;
        }

        private void modifyButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentlySelectedUser == null)
            {
                MessageBox.Show("Nincs felhasználó kiválasztva!");
                return;
            }
            string response = currentlySelectedUser.ModifyUserByUsername(felhasznaloKezelo, currentlySelectedUser.Felhasznalonev, felhasznalonevMezo.Text, emailMezo.Text, jelszoMezo.Password, (bool)admincheck.IsChecked);
            if (response != "success")
            {
                MessageBox.Show(response);
                return;
            }
            MessageBox.Show("Felhasználó módosítva!");
            ClearFields();
            UnselectUser();
            RefreshList();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
            UnselectUser();
        }
    }
}
