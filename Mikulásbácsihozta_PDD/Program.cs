using Mikulásbácsihozta_PDD.Controller;
using Mikulásbácsihozta_PDD.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mikulásbácsihozta_PDD
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                MySqlConnection connection = new MySqlConnection();
                string connectionString = "SERVER = localhost,DATABASE=kalaplengetőverseny_pdd;UID=root;PASSWORD=;";
                connection.ConnectionString = connectionString;
                List<User> users = new List<User>();
                users = UserController.GetUsers();
                MainMenu(connection,users);
            }
        }

        private static void MainMenu(MySqlConnection connection, List<User> users)
        {
            int menu = TextDecoration.ArrowMenu(new string[] { "Versenyzők listázása", "Új versenyző rögzítése", "Versenyzők kezelése", "Kilépés" }, "Főmenü");
            switch (menu)
            {
                case 0:
                    foreach (var user in users)
                    {
                        TextDecoration.WriteLineCentered($"ID: {user.Id} Név: {user.Nev} Pont1: {user.pont1} Idő1: {user.ido1} Pont2: {user.pont2} Idő2: {user.ido2} Pont3: {user.pont3} Idő3: {user.ido3} LegjobbPont: {user.Legjobbpont} LegjobbIdő: {user.Legjobbido}");
                    }
                    TextDecoration.WriteLineCentered("Nyomj meg egy gombot a visszatéréshez...");
                    Console.ReadKey();
                    break;
                case 1:
                    #region Felhasználó felvétele
                    Console.WriteLine("=== ÚJ FELHASZNÁLÓ RÖGZÍTÉSE ===");
                    Console.WriteLine("Add meg a felhasználó nevét: ");
                    string nev = Console.ReadLine();
                    Console.WriteLine("Add meg a felhasználó login nevét: ");
                    string loginName = Console.ReadLine();
                    Console.WriteLine("A felhasználó aktív-e? (i/n): ");
                    string aktivInput = Console.ReadLine();
                    bool aktiv = aktivInput.ToLower() == "i" ? true : false;
                    Console.WriteLine("Add meg a saltot: ");
                    string salt = Console.ReadLine();
                    Console.WriteLine("Add meg a hasht: ");
                    string hash = Console.ReadLine();
                    Console.WriteLine("Add meg a felhasználó születési dátumát (éééé-hh-nn): ");
                    DateTime szuldat = DateTime.Parse(Console.ReadLine());
                    connection.Open();
                    string insertsql = $"INSERT INTO kalaplengetőverseny_pdd.user (`ID`, `Nev`, `pillanatnyihelyezes`, `pont1`, `ido1`, `pont2`, `ido2`,`pont3`, `ido3`,`legjobbpont`, `legjobbido`) VALUES (null,@nev,@loginName,@aktiv,@salt,@hash,@szuldat)";
                    MySqlCommand insertCmd = new MySqlCommand(insertsql, connection);
                    insertCmd.Parameters.AddWithValue("@nev", nev);
                    insertCmd.Parameters.AddWithValue("@loginName", loginName);
                    insertCmd.Parameters.AddWithValue("@aktiv", aktiv);
                    insertCmd.Parameters.AddWithValue("@salt", salt);
                    insertCmd.Parameters.AddWithValue("@hash", hash);
                    insertCmd.Parameters.AddWithValue("@szuldat", szuldat);
                    int sorokszama = insertCmd.ExecuteNonQuery();
                    string valasz = sorokszama > 0 ? "sikeres" : "sikertelen";
                    Console.WriteLine(valasz);
                    connection.Close();
                    #endregion
                    break;
                case 2:
                    // Versenyzők kezelése
                    break;
                case 3:
                    Environment.Exit(0);
                    break;
            }
        }
    }
}
