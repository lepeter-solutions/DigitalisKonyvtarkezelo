using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace DigitalisKonyvtarkezelo
{
    public class Konyv
    {
        public string Konyvcim { get; set; }
        public string Szerzo { get; set; }
        public int KiadasEve { get; set; }
        public List<string> Kategoria { get; set; }

        public List<Konyv> konyvek = new List<Konyv>();
        private const string BooksFilePath = "books.json";

        public string AddBook(string konyvcim, string szerzo, int kiadasEve, List<String> kategoria)
        {
            foreach (Konyv konyv in konyvek)
            {
                if (konyv.Konyvcim == konyvcim)
                {
                    return "A könyv már szerepel az adatbázisban.";
                }
            }

            if (kiadasEve < 1800 || kiadasEve > DateTime.Now.Year)
            {
                return "Érvénytelen kiadási év.";
            }

            if (kategoria.Count == 0)
            {
                return "Legalább egy kategóriát meg kell adni.";
            }

            Konyv ujKonyv = new Konyv
            {
                Konyvcim = konyvcim,
                Szerzo = szerzo,
                KiadasEve = kiadasEve,
                Kategoria = kategoria
            };

            konyvek.Add(ujKonyv);
            SaveBooksToFile();

            return "success";
        }

        public string UpdateBook(Konyv originalBook, string newTitle, string newAuthor, int newYear, List<string> newCategories)
        {
            Konyv konyvToUpdate = originalBook;
            if (konyvToUpdate == null)
            {
                return "A könyv nem található.";
            }

            if (newYear < 1800 || newYear > DateTime.Now.Year)
            {
                return "Érvénytelen kiadási év.";
            }

            if (newCategories.Count == 0)
            {
                return "Legalább egy kategóriát meg kell adni.";
            }

            konyvToUpdate.Konyvcim = newTitle;
            konyvToUpdate.Szerzo = newAuthor;
            konyvToUpdate.KiadasEve = newYear;
            konyvToUpdate.Kategoria = newCategories;

            SaveBooksToFile();

            return "success";
        }




        public void SaveBooksToFile()
        {
            try
            {
                string jsonString = JsonSerializer.Serialize(konyvek);
                File.WriteAllText(BooksFilePath, jsonString);
                Console.WriteLine("Books saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving books: {ex.Message}");
            }
        }

        public string DeleteBook(Konyv bookToDelete)
        {
            if (konyvek.Remove(bookToDelete))
            {
                SaveBooksToFile();
                return "success";
            }
            return "A könyv nem található.";
        }

        public void LoadBooksFromFile()
        {
            try
            {
                if (File.Exists(BooksFilePath))
                {
                    string jsonString = File.ReadAllText(BooksFilePath);
                    konyvek = JsonSerializer.Deserialize<List<Konyv>>(jsonString);
                    Console.WriteLine("Books loaded successfully.");
                }
                else
                {
                    Console.WriteLine("No books file found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading books: {ex.Message}");
            }
        }
        public override string ToString()
        {
            return $"Title: {Konyvcim}, Author: {Szerzo}, Year: {KiadasEve}, Genres: {string.Join(", ", Kategoria)}";
        }
    }
}