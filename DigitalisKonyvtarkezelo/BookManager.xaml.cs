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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.Specialized;

namespace DigitalisKonyvtarkezelo
{
    /// <summary>
    /// Interaction logic for BookManager.xaml
    /// </summary>
    public partial class BookManager : Page
    {
        Felhasznalo loggedInUser;
        Konyv konyvKezelo = new Konyv();
        public List<string> genres = new List<string> { "All", "Fantasy", "Drama", "Science Fiction", "Mystery", "Romance", "Horror",
    "Non-Fiction", "Biography", "Adventure", "Historical", "Thriller", "Poetry",
    "Self-Help", "Health", "Travel", "Children's", "Religion", "Science",
    "Cookbooks", "Art", "Comics", "Graphic Novels", "History", "Philosophy",
    "Psychology", "True Crime", "Humor", "Sports", "Music", "Education",
    "Business", "Technology", "Politics", "Spirituality", "Memoir", "Short Stori" };


        public Konyv GetKonyvKezelo
        {
            get { return konyvKezelo; }
        }
        


        public BookManager(Felhasznalo bejelentkezettFelhasznalo)
        {
            InitializeComponent();
            konyvKezelo.LoadBooksFromFile();
            fillGenres();
            userPlaceholder.Text = $"{bejelentkezettFelhasznalo.Felhasznalonev} (Admin: {bejelentkezettFelhasznalo.IsAdmin})";
            loggedInUser = bejelentkezettFelhasznalo;
            showPermissionButtons();

            
            ShowBooksByGenre();
        }
        private void fillGenres()
        {
            bookGenres.Text = "Kategóriák: ";
            foreach (string genre in genres)
            {
                bookGenres.Items.Add(genre);
            }
            bookGenres.SelectedIndex = 0; 
        }

        public void ShowBooksByGenre(string genre = "All")
        {
            if (genre == "All")
            {
                bookGenres.SelectedIndex = 0;
            }
            bookList.Items.Clear();
            string allGenres = bookGenres.Items[0].ToString();
            foreach (Konyv konyv in konyvKezelo.konyvek)
            {
                if (genre == allGenres || konyv.Kategoria.Contains(genre))
                {
                    bookList.Items.Add(konyv);
                }
            }
        }

        private void bookGenres_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowBooksByGenre(bookGenres.SelectedItem.ToString());
        }

        private void showPermissionButtons()
        {
            if (!loggedInUser.IsAdmin)
            {
                ujkonyvfelvete.Visibility = Visibility.Hidden;
                felhasznalokezeles.Visibility = Visibility.Hidden;
            }
        }

        private void kijelentkezesGomb_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }

        private void ujkonyvfelvete_Click(object sender, RoutedEventArgs e)
        {
            if (loggedInUser.IsAdmin)
            {
                UjKonyvHozzaadas ujKonyvHozzaadasWindow = new UjKonyvHozzaadas(this);
                ujKonyvHozzaadasWindow.Show();
            }
        }
        private void bookList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!loggedInUser.IsAdmin) 
            {
                Konyv selectedBook = (Konyv)bookList.SelectedItem;
                MessageBox.Show($"Kiválasztottad a következőt: {selectedBook.Konyvcim}");
                return;
            }
            if (bookList.SelectedItem != null)
            {
                Konyv selectedBook = (Konyv)bookList.SelectedItem;
                //MessageBox.Show($"You double-clicked on: {selectedBook.Konyvcim}");
                KonyvSzerkesztes konyvSzerkesztes = new KonyvSzerkesztes(this, selectedBook);
                konyvSzerkesztes.Show();
            }
        }

        private void felhasznalokezeles_Click(object sender, RoutedEventArgs e)
        {
            if (!loggedInUser.IsAdmin)
            { 
                MessageBox.Show("Nincs jogosultságod ehhez a művelethez!");
                return;
            }
            UserControl userControl = new UserControl(loggedInUser);
            userControl.Show();
        }
    }
}
