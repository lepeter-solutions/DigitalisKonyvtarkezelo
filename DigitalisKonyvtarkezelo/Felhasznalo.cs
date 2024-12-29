using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Xml.Linq;

public class Felhasznalo
{
    public string Felhasznalonev { get; set; }
    public string Jelszo { get; set; }
    public string Email { get; set; }
    public bool IsAdmin { get; set; }

    public List<Felhasznalo> felhasznalok = new List<Felhasznalo>();

  

    public string Register(string felhasznalonev, string jelszo, string email)
    {
        foreach (Felhasznalo felhasznalo in felhasznalok)
        {
            if (felhasznalo.Felhasznalonev == felhasznalonev)
            {
                return "A felhasználónév már foglalt.";
            }
            if (felhasznalo.Email == email)
            {
                return "Az email cím már regisztrálva van.";
            }
        }

        if (!IsValidPassword(jelszo))
        {
            return "A jelszónak legalább 8 karakter hosszúnak kell lennie, és tartalmaznia kell nagybetűt, kisbetűt és számot.";
        }

        if (!IsValidEmail(email))
        {
            return "Érvénytelen email formátum.";
        }

        Felhasznalo ujFelhasznalo = new Felhasznalo
        {
            Felhasznalonev = felhasznalonev,
            Jelszo = jelszo,
            Email = email,
            IsAdmin = felhasznalok.Count == 0 
        };

        felhasznalok.Add(ujFelhasznalo);
        SaveUsersToFile();

        return "success";
    }

    private static bool IsValidPassword(string jelszo)
    {
        if (jelszo.Length < 8)
        {
            return false;
        }

        bool hasUpperCase = false;
        bool hasLowerCase = false;
        bool hasDigit = false;

        foreach (char c in jelszo)
        {
            if (char.IsUpper(c)) hasUpperCase = true;
            if (char.IsLower(c)) hasLowerCase = true;
            if (char.IsDigit(c)) hasDigit = true;
        }

        return hasUpperCase && hasLowerCase && hasDigit;
    }

    public string ModifyUserByUsername(Felhasznalo Felhasznalokezelo, string regifelhasznalonev, string ujfelhasznalonev, string ujemail, string ujjelszo, bool isAdmin)
    {
        foreach (var felhasznalo in Felhasznalokezelo.felhasznalok)
        {
            if (felhasznalo.Felhasznalonev != regifelhasznalonev)
            {
                if (felhasznalo.Felhasznalonev == ujfelhasznalonev)
                {
                    return "A felhasználónév már foglalt.";
                }
                if (felhasznalo.Email == ujemail)
                {
                    return "Az email cím már regisztrálva van.";
                }
            }
        }

        // Validate the new password
        if (!IsValidPassword(ujjelszo))
        {
            return "A jelszónak legalább 8 karakter hosszúnak kell lennie, és tartalmaznia kell nagybetűt, kisbetűt és számot.";
        }

        // Validate the new email
        if (!IsValidEmail(ujemail))
        {
            return "Érvénytelen email formátum.";
        }

        // Update the user information
        foreach (var felhasznalo in Felhasznalokezelo.felhasznalok)
        {
            if (felhasznalo.Felhasznalonev == regifelhasznalonev)
            {
                felhasznalo.Felhasznalonev = ujfelhasznalonev;
                felhasznalo.Email = ujemail;
                felhasznalo.Jelszo = ujjelszo;
                felhasznalo.IsAdmin = isAdmin;
                Felhasznalokezelo.SaveUsersToFile();
                return "success";
            }
        }

        return "A felhasználó nem található.";
    }

    

    public string DeleteUser(Felhasznalo felhasznalokezelo, string felhasznalonev)
    {
        foreach (var felhasznalo in felhasznalokezelo.felhasznalok)
        {
            if (felhasznalo.Felhasznalonev == felhasznalonev)
            {
                felhasznalokezelo.felhasznalok.Remove(felhasznalo);
                felhasznalokezelo.SaveUsersToFile();
                return "success";
            }
        }
        
        return "A felhasználó nem található.";
    }


    public static Felhasznalo GetUserByUsername(Felhasznalo Felhasznalokezelo, string felhasznalonev)
    {
        foreach (var felhasznalo in Felhasznalokezelo.felhasznalok)
        {
            if (felhasznalo.Felhasznalonev == felhasznalonev)
            {
                return felhasznalo;
            }
        }

        return null;
    }

    private static bool IsValidEmail(string email)
    {
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailPattern);
    }

    public static void MakeAdmin(Felhasznalo felhasznalo)
    {
        felhasznalo.IsAdmin = true;
    }

    public static void RemoveAdmin(Felhasznalo felhasznalo)
    {
        felhasznalo.IsAdmin = false;
    }

    private void SaveUsersToFile()
    {
        string json = JsonSerializer.Serialize(felhasznalok, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText("users.json", json);
    }

    public void LoadUsersFromFile()
    {
        if (File.Exists("users.json"))
        {
            string json = File.ReadAllText("users.json");
            felhasznalok = JsonSerializer.Deserialize<List<Felhasznalo>>(json);
        }
    }

    public override string ToString()
    {
        return $"Name: {Felhasznalonev}, Age: {Email}, Admin: {IsAdmin}";
    }

}