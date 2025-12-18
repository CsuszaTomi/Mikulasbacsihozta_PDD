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
                    Console.Clear();
                    Iras.WriteLineCentered("=== ÚJ FELHASZNÁLÓ RÖGZÍTÉSE ===", "red");
                    Iras.WriteCentered("Add meg a versenyző nevét: ");
                    string nev = Console.ReadLine();
                    if (nev == "")
                    {
                        return;
                    }
                    Iras.WriteCentered("Add meg versenyző első körben elért pontszámát: ");
                    string pont1 = Console.ReadLine();
                    while (!Ellenorzo.SzamEllenorzo(pont1) || pont1 == "")
                    {
                        Iras.WriteLineCentered("Hibás pontszám érték!", "red");
                        Iras.WriteCentered("Add meg újra: ");
                        pont1 = Console.ReadLine();
                    }
                    int pont1int = int.Parse(pont1);
                    Iras.WriteCentered("Add meg versenyző első körben eltelt idejét: ");
                    string ido1 = Console.ReadLine();
                    while (!Ellenorzo.IdoEllenorzo(ido1) || ido1 == "")
                    {
                        Iras.WriteLineCentered("Hibás idő érték!", "red");
                        Iras.WriteCentered("Add meg újra: ");
                        ido1 = Console.ReadLine();
                    }
                    double ido1double = double.Parse(ido1);
                    Iras.WriteCentered("Add meg versenyző második körben elért pontszámát: ");
                    string pont2 = Console.ReadLine();
                    while (!Ellenorzo.SzamEllenorzo(pont2) || pont2 == "")
                    {
                        Iras.WriteLineCentered("Hibás pontszám érték!", "red");
                        Iras.WriteCentered("Add meg újra: ");
                        pont2 = Console.ReadLine();
                    }
                    int pont2int = int.Parse(pont2);
                    Iras.WriteCentered("Add meg versenyző második körben eltelt idejét: ");
                    string ido2 = Console.ReadLine();
                    while (!Ellenorzo.IdoEllenorzo(ido2) || ido2 == "")
                    {
                        Iras.WriteLineCentered("Hibás idő érték!", "red");
                        Iras.WriteCentered("Add meg újra: ");
                        ido2 = Console.ReadLine();
                    }
                    double ido2double = double.Parse(ido2);
                    Iras.WriteCentered("Add meg versenyző harmadik körben elért pontszámát: ");
                    string pont3 = Console.ReadLine();
                    while (!Ellenorzo.SzamEllenorzo(pont3) || pont3 == "")
                    {
                        Iras.WriteLineCentered("Hibás pontszám érték!", "red");
                        Iras.WriteCentered("Add meg újra: ");
                        pont3 = Console.ReadLine();
                    }
                    int pont3int = int.Parse(pont3);
                    Iras.WriteCentered("Add meg versenyző harmadik körben eltelt idejét: ");
                    string ido3 = Console.ReadLine();
                    while (!Ellenorzo.IdoEllenorzo(ido3) || ido3 == "")
                    {
                        Iras.WriteLineCentered("Hibás idő érték!", "red");
                        Iras.WriteCentered("Add meg újra: ");
                        ido3 = Console.ReadLine();
                    }
                    double ido3double = double.Parse(ido3);
                    int legjobbpont = Math.Max(pont1int, Math.Max(pont2int, pont3int));
                    double legjobbido = Math.Min(ido1double, Math.Min(ido2double, ido3double));
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
                    int id = 0;
                    foreach (User user in users)
                    {
                        if (user.Id >= id)
                        {
                            id = user.Id+1;
                        }
                    }
                    connection.Open();
                    string insertsql = $"INSERT INTO kalaplengetőverseny_pdd.versenyzok (`ID`, `Nev`, `pillanatnyihelyezes`, `pont1`, `ido1`, `pont2`, `ido2`,`pont3`, `ido3`,`legjobbpont`, `legjobbido`) VALUES (@id,@nev,@pillanatnyihelyezes,@pont1,@ido1,@pont2,@ido2,@pont3,@ido3,@legjobbpont,@legjobbido)";
                    MySqlCommand insertCmd = new MySqlCommand(insertsql, connection);
                    insertCmd.Parameters.AddWithValue("@id", id);
                    insertCmd.Parameters.AddWithValue("@nev", nev);
                    insertCmd.Parameters.AddWithValue("@pillanatnyihelyezes", pillanatnyihelyezes);
                    insertCmd.Parameters.AddWithValue("@pont1", pont1int);
                    insertCmd.Parameters.AddWithValue("@ido1", ido1double);
                    insertCmd.Parameters.AddWithValue("@pont2", pont2int);
                    insertCmd.Parameters.AddWithValue("@ido2", ido2double);
                    insertCmd.Parameters.AddWithValue("@pont3", pont3int);
                    insertCmd.Parameters.AddWithValue("@ido3", ido3double);
                    insertCmd.Parameters.AddWithValue("@legjobbpont", legjobbpont);
                    insertCmd.Parameters.AddWithValue("@legjobbido", legjobbido);
                    int sorokszama = insertCmd.ExecuteNonQuery();
                    string valasz = sorokszama > 0 ? "Sikeres hozzáadás" : "Sikertelen hozzáadás!";
                    Iras.WriteLineCentered(valasz,"green");
                    connection.Close();
                    Thread.Sleep(2000);
                    break;
                //Versenyzők kezelése
                case 2:
                    int menu2 = Iras.ArrowMenu(new string[] { "Versenyző módosítása","Versenyző törlése","Helyezések kiszámítása","Vissza" }, "=== VERSENYZŐ KEZELÉS ===");
                    switch (menu2)
                    {
                        //Versenyző módosítása
                        case 0:
                            UserController.UserModositas(connection);
                            Thread.Sleep(2000);
                            break;
                        //Versenyző törlése
                        case 1:
                            UserController.UserTorles(connection);
                            Thread.Sleep(2000);
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