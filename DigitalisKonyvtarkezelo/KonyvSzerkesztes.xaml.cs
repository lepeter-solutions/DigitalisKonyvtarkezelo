using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DigitalisKonyvtarkezelo
{
    public partial class KonyvSzerkesztes : Window
    {

        Konyv konyvKezelo;
        BookManager BookManagerHandler;
        Konyv szerkesztoKonyv;
        public KonyvSzerkesztes(BookManager bookManager, Konyv kivalasztottKonyv)
        {
            InitializeComponent();
            BookManagerHandler = bookManager;
            szerkesztoKonyv = kivalasztottKonyv;
            Konyv getKonyvKezelo = bookManager.GetKonyvKezelo;
            List<string> genreList = bookManager.genres;
            konyvKezelo = getKonyvKezelo;
            foreach (string genre in genreList)
            {
                KategoriaListBox.Items.Add(new ListBoxItem { Content = genre });
            }
            KonyvcimTextBox.Text = kivalasztottKonyv.Konyvcim;
            SzerzoNeveTextBox.Text = kivalasztottKonyv.Szerzo;
            KiadasEveTextBox.Text = kivalasztottKonyv.KiadasEve.ToString();
            foreach (string kategoria in kivalasztottKonyv.Kategoria)
            {
                foreach (ListBoxItem item in KategoriaListBox.Items)
                {
                    if (item.Content.ToString() == kategoria)
                    {
                        item.IsSelected = true;
                        break;
                    }
                }
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

        private void SzerkesztesButton_Click(object sender, RoutedEventArgs e)
        {
            string konyvcim = KonyvcimTextBox.Text;
            string szerzoNeve = SzerzoNeveTextBox.Text;
            int kiadasEve = int.Parse(KiadasEveTextBox.Text);
            var selectedKategoria = KategoriaListBox.SelectedItems.Cast<ListBoxItem>().Select(item => item.Content.ToString()).ToList();

            string response = konyvKezelo.UpdateBook(szerkesztoKonyv, konyvcim, szerzoNeve, kiadasEve, selectedKategoria);
            if (response != "success")
            {
                MessageBox.Show(response);
                return;
            }
            MessageBox.Show("Könyv sikeresen szerkesztve!");
            BookManagerHandler.ShowBooksByGenre();
        }

        private void TorlesButton_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result = MessageBox.Show("Biztosan törölni szeretné a könyvet?", "Megerősítés", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                string response = konyvKezelo.DeleteBook(szerkesztoKonyv);
                if (response != "success")
                {
                    MessageBox.Show(response);
                    return;
                }
                MessageBox.Show("Könyv sikeresen törölve!");
                BookManagerHandler.ShowBooksByGenre();
                this.Close();
            }

        }
    }
}