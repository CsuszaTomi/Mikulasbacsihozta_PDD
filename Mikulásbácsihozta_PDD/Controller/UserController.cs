using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mikulásbácsihozta_PDD.Models;
using MySql.Data.MySqlClient;

namespace Mikulásbácsihozta_PDD.Controller
{
    internal class UserController
    {
        static public List<User> GetUsers()
        {
            MySqlConnection connection = new MySqlConnection();
            string connectionString = "SERVER = localhost,DATABASE=kalaplengetőverseny_pdd;UID=root;PASSWORD=;";
            connection.ConnectionString = connectionString;
            connection.Open();
            string sql = "SELECT * FROM kalaplengetőverseny_pdd.versenyzok";
            MySqlCommand cmd = new MySqlCommand(sql, connection);
            List<User> users = new List<User>();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                User user = new User(id: reader.GetInt32("ID"),
                    nev: reader.GetString("Nev"),
                    pillhely: reader.GetInt32("Pillanatnyihelyezes"),
                    pont1: reader.GetInt32("Pont1"),
                    ido1: reader.GetDouble("Ido1"),
                    pont2: reader.GetInt32("Pont2"),
                    ido2: reader.GetDouble("Ido2"),
                    pont3: reader.GetInt32("Pont3"),
                    ido3: reader.GetDouble("Ido3"),
                    legjobbpont: reader.GetInt32("Legjobbpont"),
                    legjobbido: reader.GetDouble("Legjobbido")
                    );
                users.Add(user);
            }
            connection.Close();
            return users;
        }

        static public void UserModositas(MySqlConnection connection)
        {
            List<User> users = GetUsers();
            Console.Clear();
            Iras.WriteLineCentered("=== VERSENYZŐ MÓDOSÍTÁSA ===", "red");
            Iras.WriteCentered("Add meg a módosítandó versenyző nevét: ");
            string modositandonev = Console.ReadLine();
            int ellenorzes = 0;
            User versenyzo = null;
            foreach (User user in users)
            {
                if (user.Nev == modositandonev)
                {
                    versenyzo = user;
                    continue;
                }
                else
                {
                    ellenorzes++;
                    if (ellenorzes == users.Count)
                    {
                        Iras.WriteLineCentered("Nincs ilyen nevű versenyző!", "red");
                        Iras.WriteLineCentered("Nyomj meg egy gombot a visszatéréshez...");
                        Console.ReadKey();
                        return;
                    }
                }
            }
            Iras.WriteLineCentered("Ha nem egy bizonyos adatot módosítani akkor arra nyomjon szóközt/entert.", "yellow");
            Iras.WriteLineCentered($"Jelenlegi név: {modositandonev}");
            Iras.WriteCentered("Új név: ");
            string ujnev = Console.ReadLine();
            if (ujnev != "")
            {
                modositandonev = ujnev;
            }
            Iras.WriteLineCentered($"Jelenlegi első kör pontszám: {versenyzo.pont1}");
            Iras.WriteCentered("Új pontszám: ");
            string ujpont1 = Console.ReadLine();
            if (ujpont1 == "")
            {
                ujpont1 = versenyzo.pont1.ToString();
            }
            while (!Ellenorzo.SzamEllenorzo(ujpont1))
            {
                Iras.WriteLineCentered("Hibás pontszám érték!", "red");
                Iras.WriteCentered("Add meg újra: ");
                ujpont1 = Console.ReadLine();
                if (ujpont1 == "")
                {
                    break;
                }
            }
            if (ujpont1 != "")
            {
                versenyzo.pont1 = int.Parse(ujpont1);
            }
            Iras.WriteLineCentered($"Jelenlegi első kör idő: {versenyzo.ido1}");
            Iras.WriteCentered("Új idő: ");
            string ujido1 = Console.ReadLine();
            if (ujido1 == "")
            {
                ujido1 = versenyzo.ido1.ToString();
            }
            while (!Ellenorzo.IdoEllenorzo(ujido1))
            {
                Iras.WriteLineCentered("Hibás idő érték!", "red");
                Iras.WriteCentered("Add meg újra: ");
                ujido1 = Console.ReadLine();
                if (ujido1 == "")
                {
                    break;
                }
            }
            if (ujido1 != "")
            {
                versenyzo.ido1 = double.Parse(ujido1);
            }
            Iras.WriteLineCentered($"Jelenlegi második kör pontszám: {versenyzo.pont2}");
            Iras.WriteCentered("Új pontszám: ");
            string ujpont2 = Console.ReadLine();
            if (ujpont2 == "")
            {
                ujpont2 = versenyzo.pont2.ToString();
            }
            while (!Ellenorzo.SzamEllenorzo(ujpont2))
            {
                Iras.WriteLineCentered("Hibás pontszám érték!", "red");
                Iras.WriteCentered("Add meg újra: ");
                ujpont2 = Console.ReadLine();
                if (ujpont2 == "")
                {
                    break;
                }
            }
            if (ujpont2 != "")
            {
                versenyzo.pont2 = int.Parse(ujpont2);
            }
            Iras.WriteLineCentered($"Jelenlegi második kör idő: {versenyzo.ido2}");
            Iras.WriteCentered("Új idő: ");
            string ujido2 = Console.ReadLine();
            if (ujido2 == "")
            {
                ujido2 = versenyzo.ido2.ToString();
            }
            while (!Ellenorzo.IdoEllenorzo(ujido2))
            {
                Iras.WriteLineCentered("Hibás idő érték!", "red");
                Iras.WriteCentered("Add meg újra: ");
                ujido2 = Console.ReadLine();
                if (ujido2 == "")
                {
                    break;
                }
            }
            if (ujido2 != "")
            {
                versenyzo.ido2 = double.Parse(ujido2);
            }
            Iras.WriteLineCentered($"Jelenlegi harmadik kör pontszám: {versenyzo.pont3}");
            Iras.WriteCentered("Új pontszám: ");
            string ujpont3 = Console.ReadLine();
            if (ujpont3 == "")
            {
                ujpont3 = versenyzo.pont3.ToString();
            }
            while (!Ellenorzo.SzamEllenorzo(ujpont3))
            {
                Iras.WriteLineCentered("Hibás pontszám érték!", "red");
                Iras.WriteCentered("Add meg újra: ");
                ujpont3 = Console.ReadLine();
                if (ujpont3 == "")
                {
                    break;
                }
            }
            if (ujpont3 != "")
            {
                versenyzo.pont3 = int.Parse(ujpont3);
            }
            Iras.WriteLineCentered($"Jelenlegi harmadik kör idő: {versenyzo.ido3}");
            Iras.WriteCentered("Új idő: ");
            string ujido3 = Console.ReadLine();
            if (ujido3 == "")
            {
                ujido3 = versenyzo.ido3.ToString();
            }
            while (!Ellenorzo.IdoEllenorzo(ujido3))
            {
                Iras.WriteLineCentered("Hibás idő érték!", "red");
                Iras.WriteCentered("Add meg újra: ");
                ujido3 = Console.ReadLine();
                if (ujido3 == "")
                {
                    break;
                }
            }
            if (ujido3 != "")
            {
                versenyzo.ido3 = double.Parse(ujido3);
            }
            int ujlegjobbpont = Math.Max(versenyzo.pont1, Math.Max(versenyzo.pont2, versenyzo.pont3));
            double ujlegjobbido = Math.Min(versenyzo.ido1, Math.Min(versenyzo.ido2, versenyzo.ido3));
            connection.Open();
            string updatesql = $"UPDATE kalaplengetőverseny_pdd.versenyzok SET `ID`=@id,`Nev`=@nev,`pillanatnyihelyezes`=@pillanatnyihelyezes,`pont1`=@pont1,`ido1`=@ido1,`pont2`=@pont2,`ido2`=@ido2,`pont3`=@pont3,`ido3`=@ido3,`legjobbpont`=@legjobbpont,`legjobbido`=@legjobbido WHERE Nev = @eredetinev";
            MySqlCommand updatecmd = new MySqlCommand(updatesql, connection);
            updatecmd.Parameters.AddWithValue("@id", versenyzo.Id);
            updatecmd.Parameters.AddWithValue("@nev", modositandonev);
            updatecmd.Parameters.AddWithValue("@eredetinev", versenyzo.Nev);
            updatecmd.Parameters.AddWithValue("@pillanatnyihelyezes", versenyzo.pillhely);
            updatecmd.Parameters.AddWithValue("@pont1", versenyzo.pont1);
            updatecmd.Parameters.AddWithValue("@ido1", versenyzo.ido1);
            updatecmd.Parameters.AddWithValue("@pont2", versenyzo.pont2);
            updatecmd.Parameters.AddWithValue("@ido2", versenyzo.ido2);
            updatecmd.Parameters.AddWithValue("@pont3", versenyzo.pont3);
            updatecmd.Parameters.AddWithValue("@ido3", versenyzo.ido3);
            updatecmd.Parameters.AddWithValue("@legjobbpont", ujlegjobbpont);
            updatecmd.Parameters.AddWithValue("@legjobbido", ujlegjobbido);
            updatecmd.ExecuteNonQuery();
            connection.Close();
            Iras.WriteLineCentered("Sikeres módosítás!", "green");
        }

        static public void UserTorles(MySqlConnection connection)
        {
            List<User> users = GetUsers();
            Console.Clear();
            Iras.WriteLineCentered("=== VERSENYZŐ TÖRLÉSE ===", "red");
            Iras.WriteCentered("Add meg a törlendő versenyző nevét: ");
            string torlendonev = Console.ReadLine();
            int torloellen = 0;
            User versenyzo = null;
            foreach (User user in users)
            {
                if (user.Nev == torlendonev)
                {
                    versenyzo = user;
                    continue;
                }
                else
                {
                    torloellen++;
                    if (torloellen == users.Count)
                    {
                        Iras.WriteLineCentered("Nincs ilyen nevű versenyző!", "red");
                        Iras.WriteLineCentered("Nyomj meg egy gombot a visszatéréshez...");
                        Console.ReadKey();
                        return;
                    }
                }
            }
            connection.Open();
            string deletesql = $"DELETE FROM kalaplengetőverseny_pdd.versenyzok WHERE Nev = @nev";
            MySqlCommand deletecmd = new MySqlCommand(deletesql, connection);
            deletecmd.Parameters.AddWithValue("@nev", torlendonev);
            deletecmd.ExecuteNonQuery();
            connection.Close();
            Iras.WriteLineCentered("Sikeres törlés!", "green");
        }
    }
}
