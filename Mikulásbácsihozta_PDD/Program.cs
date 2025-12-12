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
            MySqlConnection connection = new MySqlConnection();
            string connectionString = "SERVER = localhost,DATABASE=kalaplengetőverseny_pdd;UID=root;PASSWORD=;";
            connection.ConnectionString = connectionString;
            List<User> users = new List<User>();
            while (true)
            {
                Console.Clear();
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
                    users = UserController.GetUsers();
                    foreach (var user in users)
                    {
                        TextDecoration.WriteLineCentered($"ID: {user.Id} Név: {user.Nev} Pont1: {user.pont1} Idő1: {user.ido1} Pont2: {user.pont2} Idő2: {user.ido2} Pont3: {user.pont3} Idő3: {user.ido3} LegjobbPont: {user.Legjobbpont} LegjobbIdő: {user.Legjobbido}");
                    }
                    TextDecoration.WriteLineCentered("Nyomj meg egy gombot a visszatéréshez...");
                    Console.ReadKey();
                    break;
                case 1:
                    #region Felhasználó felvétele
                    TextDecoration.WriteLineCentered("=== ÚJ FELHASZNÁLÓ RÖGZÍTÉSE ===");
                    TextDecoration.WriteCentered("Add meg a versenyző nevét: ");
                    string nev = Console.ReadLine();
                    TextDecoration.WriteCentered("Add meg versenyző első körben elért pontszámát: ");
                    int pont1 = int.Parse(Console.ReadLine());
                    TextDecoration.WriteCentered("Add meg versenyző első körben elért idejét: ");
                    double ido1 = double.Parse(Console.ReadLine());
                    TextDecoration.WriteCentered("Add meg versenyző második körben elért pontszámát: ");
                    int pont2 = int.Parse(Console.ReadLine());
                    TextDecoration.WriteCentered("Add meg versenyző második körben elért idejét: ");
                    double ido2 = double.Parse(Console.ReadLine());
                    TextDecoration.WriteCentered("Add meg versenyző harmadik körben elért pontszámát: ");
                    int pont3 = int.Parse(Console.ReadLine());
                    TextDecoration.WriteCentered("Add meg versenyző harmadik körben elért idejét: ");
                    double ido3 = double.Parse(Console.ReadLine());
                    int legjobbpont = Math.Max(pont1, Math.Max(pont2, pont3));
                    double legjobbido = Math.Min(ido1, Math.Min(ido2, ido3));
                    int pillanatnyihelyezes = 1;
                    foreach (var user in users)
                    {
                        if (legjobbpont < user.Legjobbpont)
                        {
                            pillanatnyihelyezes++;
                        }
                        else if (legjobbpont == user.Legjobbpont && legjobbido > user.Legjobbido)
                        {
                            pillanatnyihelyezes++;
                        }
                    }
                    connection.Open();
                    string insertsql = $"INSERT INTO kalaplengetőverseny_pdd.versenyzok (`ID`, `Nev`, `pillanatnyihelyezes`, `pont1`, `ido1`, `pont2`, `ido2`,`pont3`, `ido3`,`legjobbpont`, `legjobbido`) VALUES (null,@nev,@pillanatnyihelyezes,@pont1,@ido1,@pont2,@ido2,@pont3,@ido3,@legjobbpont,@legjobbido)";
                    MySqlCommand insertCmd = new MySqlCommand(insertsql, connection);
                    insertCmd.Parameters.AddWithValue("@nev", nev);
                    insertCmd.Parameters.AddWithValue("@pillanatnyihelyezes", pillanatnyihelyezes);
                    insertCmd.Parameters.AddWithValue("@pont1", pont1);
                    insertCmd.Parameters.AddWithValue("@ido1", ido1);
                    insertCmd.Parameters.AddWithValue("@pont2", pont2);
                    insertCmd.Parameters.AddWithValue("@ido2", ido2);
                    insertCmd.Parameters.AddWithValue("@pont3", pont3);
                    insertCmd.Parameters.AddWithValue("@ido3", ido3);
                    insertCmd.Parameters.AddWithValue("@legjobbpont", legjobbpont);
                    insertCmd.Parameters.AddWithValue("@legjobbido", legjobbido);
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
