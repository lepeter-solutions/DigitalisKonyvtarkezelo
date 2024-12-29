using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DigitalisKonyvtarkezelo
{
    public partial class UjKonyvHozzaadas : Window
    {

        Konyv konyvKezelo;
        BookManager BookManagerHandler;
        public UjKonyvHozzaadas(BookManager bookManager)
        {
            InitializeComponent();
            BookManagerHandler = bookManager;
            Konyv getKonyvKezelo = bookManager.GetKonyvKezelo;
            List<string> genreList = bookManager.genres;
            konyvKezelo = getKonyvKezelo;
            foreach (string genre in genreList)
            {
                KategoriaListBox.Items.Add(new ListBoxItem { Content = genre });
            }
        }

        private void ClearFields()
        {
            KonyvcimTextBox.Text = "";
            SzerzoNeveTextBox.Text = "";
            KiadasEveTextBox.Text = "";
            KategoriaListBox.SelectedItems.Clear();
        }   

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void HozzaadasButton_Click(object sender, RoutedEventArgs e)
        {
            // Handle the add book logic here
            string konyvcim = KonyvcimTextBox.Text;
            string szerzoNeve = SzerzoNeveTextBox.Text;
            int kiadasEve = int.Parse(KiadasEveTextBox.Text);
            var selectedKategoria = KategoriaListBox.SelectedItems.Cast<ListBoxItem>().Select(item => item.Content.ToString()).ToList();

            string response = konyvKezelo.AddBook(konyvcim, szerzoNeve, kiadasEve, selectedKategoria);
            if (response != "success")
            {
                MessageBox.Show(response);
                return;
            }
            ClearFields();
            MessageBox.Show("Könyv hozzáadva!");
            BookManagerHandler.ShowBooksByGenre();
            // Add your logic to process the form data
        }
    }
}