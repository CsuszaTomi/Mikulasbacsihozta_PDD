using Mikulásbácsihozta_PDD.Controller;
using Mikulásbácsihozta_PDD.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            users = UserController.GetUsers();
            while (true)
            {
                Console.Clear();
                users = UserController.GetUsers();
                MainMenu(connection,users);
            }
        }

        private static void MainMenu(MySqlConnection connection, List<User> users)
        {
            int menu = Iras.ArrowMenu(new string[] { "Versenyzők listázása", "Új versenyző rögzítése", "Versenyzők kezelése","HTML Generálása", "Kilépés" }, "=== FŐMENÜ ===");
            switch (menu)
            {
                //Versenyzők listázása
                case 0:
                    Console.Clear();
                    users = HelyezesSzamito(connection);
                    Iras.WriteLineCentered("=== VERSENYZŐK LISTÁJA ===", "red");
                    Iras.WriteLineCentered(Iras.CenterText("Helyezés | Név                | Legjobb pontszám | Legjobb idő", 70));
                    Iras.WriteLineCentered(new string('-', 70));
                    foreach (var user in users)
                    {
                        Iras.WriteLineCentered(Iras.CenterText($"{user.pillhely,8} | {user.Nev,-18} | {user.Legjobbpont,16} | {user.Legjobbido,11}", 70));
                    }
                    Iras.WriteLineCentered(new string('-', 70));
                    Iras.WriteLineCentered("Nyomj meg egy gombot a visszatéréshez...");
                    Console.ReadKey();

                    break;
                //Új versenyző rögzítése
                case 1:
                    UserController.UserHozzaAdas(connection,users);
                    Thread.Sleep(2000);
                    break;
                //Versenyzők kezelése
                case 2:
                    int menu2 = Iras.ArrowMenu(new string[] { "Versenyző módosítása","Versenyző törlése","Helyezések kiszámítása","Vissza" }, "=== VERSENYZŐ KEZELÉS ===");
                    switch (menu2)
                    {
                        //Versenyző módosítása
                        case 0:
                            UserController.UserModositas(connection,users);
                            Thread.Sleep(2000);
                            break;
                        //Versenyző törlése
                        case 1:
                            UserController.UserTorles(connection,users);
                            break;
                        //Helyezések kiszámítása
                        case 2:
                            Console.Clear();
                            Iras.WriteLineCentered("=== HELYEZÉSEK KISZÁMÍTÁSA ===", "red");
                            users = HelyezesSzamito(connection);
                            connection.Close();
                            Iras.WriteLineCentered("Helyezések sikeresen frissítve!", "green");
                            Thread.Sleep(2000);
                            break;
                    }
                    break;
                //HTML generálása
                case 3:
                    Console.Clear();
                    Iras.WriteLineCentered("=== HTML GENERÁLÁSA ===","red");
                    users = HelyezesSzamito(connection);
                    HTMLController.HTMLGeneralo(users);
                    Iras.WriteLineCentered("HTML fájl sikeresen generálva!","green");
                    Iras.WriteLineCentered("Nyomj meg egy gombot a visszatéréshez...");
                    Console.ReadKey();
                    break;
                //Kilépés
                case 4:
                    Environment.Exit(0);
                    break;
            }   
        }

        private static List<User> HelyezesSzamito(MySqlConnection connection)
        {
            List<User> users = UserController.GetUsers();
            users = users.OrderByDescending(user => user.Legjobbpont).ThenBy(user => user.Legjobbido).ThenBy(user => user.Nev).ToList();
            for (int i = 0; i < users.Count; i++)
            {
                users[i].pillhely = i + 1;

            }
            connection.Open();
            foreach (var user in users)
            {
                string helyezesupdatesql = $"UPDATE kalaplengetőverseny_pdd.versenyzok SET `pillanatnyihelyezes`=@pillanatnyihelyezes WHERE ID = @id";
                MySqlCommand updateplacecmd = new MySqlCommand(helyezesupdatesql, connection);
                updateplacecmd.Parameters.AddWithValue("@pillanatnyihelyezes", user.pillhely);
                updateplacecmd.Parameters.AddWithValue("@id", user.Id);
                updateplacecmd.ExecuteNonQuery();
            }
            connection.Close();
            return users;
        }
    }
}