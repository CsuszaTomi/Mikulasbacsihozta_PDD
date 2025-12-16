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
            int menu = TextDecoration.ArrowMenu(new string[] { "Versenyzők listázása", "Új versenyző rögzítése", "Versenyzők kezelése","HTML Generálása", "Kilépés" }, "=== FŐMENÜ ===");
            switch (menu)
            {
                case 0:
                    Console.Clear();
                    users = HelyezesSzamito(connection);
                    TextDecoration.WriteLineCentered("=== VERSENYZŐK LISTÁJA ===", "red");
                    TextDecoration.WriteLineCentered(TextDecoration.CenterText("Helyezés | Név                | Legjobb pontszám | Legjobb idő", 70));
                    TextDecoration.WriteLineCentered(new string('-', 70));
                    foreach (var user in users)
                    {
                        TextDecoration.WriteLineCentered(TextDecoration.CenterText($"{user.pillhely,8} | {user.Nev,-18} | {user.Legjobbpont,16} | {user.Legjobbido,11}", 70));
                    }
                    TextDecoration.WriteLineCentered(new string('-', 70));
                    TextDecoration.WriteLineCentered("Nyomj meg egy gombot a visszatéréshez...");
                    Console.ReadKey();

                    break;
                case 1:
                    #region Felhasználó felvétele
                    Console.Clear();
                    TextDecoration.WriteLineCentered("=== ÚJ FELHASZNÁLÓ RÖGZÍTÉSE ===", "red");
                    TextDecoration.WriteCentered("Add meg a versenyző nevét: ");
                    string nev = Console.ReadLine();
                    TextDecoration.WriteCentered("Add meg versenyző első körben elért pontszámát: ");
                    int pont1 = int.Parse(Console.ReadLine());
                    TextDecoration.WriteCentered("Add meg versenyző első körben eltelt idejét: ");
                    double ido1 = double.Parse(Console.ReadLine());
                    TextDecoration.WriteCentered("Add meg versenyző második körben elért pontszámát: ");
                    int pont2 = int.Parse(Console.ReadLine());
                    TextDecoration.WriteCentered("Add meg versenyző második körben eltelt idejét: ");
                    double ido2 = double.Parse(Console.ReadLine());
                    TextDecoration.WriteCentered("Add meg versenyző harmadik körben elért pontszámát: ");
                    int pont3 = int.Parse(Console.ReadLine());
                    TextDecoration.WriteCentered("Add meg versenyző harmadik körben eltelt idejét: ");
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
                    insertCmd.Parameters.AddWithValue("@pont1", pont1);
                    insertCmd.Parameters.AddWithValue("@ido1", ido1);
                    insertCmd.Parameters.AddWithValue("@pont2", pont2);
                    insertCmd.Parameters.AddWithValue("@ido2", ido2);
                    insertCmd.Parameters.AddWithValue("@pont3", pont3);
                    insertCmd.Parameters.AddWithValue("@ido3", ido3);
                    insertCmd.Parameters.AddWithValue("@legjobbpont", legjobbpont);
                    insertCmd.Parameters.AddWithValue("@legjobbido", legjobbido);
                    int sorokszama = insertCmd.ExecuteNonQuery();
                    string valasz = sorokszama > 0 ? "Sikeres hozzáadás" : "Sikertelen hozzáadás!";
                    TextDecoration.WriteLineCentered(valasz,"green");
                    connection.Close();
                    Thread.Sleep(2000);
                    #endregion
                    break;
                case 2:
                    int menu2 = TextDecoration.ArrowMenu(new string[] { "Versenyző módosítása","Versenyző törlése","Helyezések kiszámítása","Vissza" }, "=== VERSENYZŐ KEZELÉS ===");
                    switch (menu2)
                    {
                        case 0:
                            Console.Clear();
                            TextDecoration.WriteLineCentered("=== VERSENYZŐ MÓDOSÍTÁSA ===");
                            TextDecoration.WriteCentered("Add meg a módosítandó versenyző nevét: ");
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
                                        TextDecoration.WriteLineCentered("Nincs ilyen nevű versenyző!", "red");
                                        TextDecoration.WriteLineCentered("Nyomj meg egy gombot a visszatéréshez...");
                                        Console.ReadKey();
                                        return;
                                    }
                                }
                            }
                            TextDecoration.WriteLineCentered("Ha nem egy bizonyos adatot módosítani akkor arra nyomjon szóközt/entert.");
                            TextDecoration.WriteCentered($"Jelenlegi név: {modositandonev} Új név: ");
                            string ujnev = Console.ReadLine();
                            if (ujnev != "")
                            {
                                modositandonev = ujnev;
                            }
                            TextDecoration.WriteCentered($"Jelenlegi pillanatnyi helyezés: {versenyzo.pillhely} Új helyezés: ");
                            string ujhelyezes = Console.ReadLine();
                            if (ujhelyezes != "")
                            {
                                versenyzo.pillhely = int.Parse(ujhelyezes);
                            }
                            TextDecoration.WriteCentered($"Jelenlegi első kör pontszám: {versenyzo.pont1} Új pontszám: ");
                            string ujpont1 = Console.ReadLine();
                            if (ujpont1 != "")
                            {
                                versenyzo.pont1 = int.Parse(ujpont1);
                            }
                            TextDecoration.WriteCentered($"Jelenlegi első kör idő: {versenyzo.ido1} Új idő: ");
                            string ujido1 = Console.ReadLine();
                            if (ujido1 != "")
                            {
                                versenyzo.ido1 = double.Parse(ujido1);
                            }
                            TextDecoration.WriteCentered($"Jelenlegi második kör pontszám: {versenyzo.pont2} Új pontszám: ");
                            string ujpont2 = Console.ReadLine();
                            if (ujpont2 != "")
                            {
                                versenyzo.pont2 = int.Parse(ujpont2);
                            }
                            TextDecoration.WriteCentered($"Jelenlegi második kör idő: {versenyzo.ido2} Új idő: ");
                            string ujido2 = Console.ReadLine();
                            if (ujido2 != "")
                            {
                                versenyzo.ido2 = double.Parse(ujido2);
                            }
                            TextDecoration.WriteCentered($"Jelenlegi harmadik kör pontszám: {versenyzo.pont3} Új pontszám: ");
                            string ujpont3 = Console.ReadLine();
                            if (ujpont3 != "")
                            {
                                versenyzo.pont3 = int.Parse(ujpont3);
                            }
                            TextDecoration.WriteCentered($"Jelenlegi harmadik kör idő: {versenyzo.ido3} Új idő: ");
                            string ujido3 = Console.ReadLine();
                            if (ujido3 != "")
                            {
                                versenyzo.ido3 = double.Parse(ujido3);
                            }
                            int ujlegjobbpont = Math.Max(versenyzo.pont1, Math.Max(versenyzo.pont2, versenyzo.pont3));
                            double ujlegjobbido = Math.Min(versenyzo.ido1, Math.Min(versenyzo.ido2, versenyzo.ido3));
                            connection.Open();
                            string updatesql = $"UPDATE kalaplengetőverseny_pdd.versenyzok SET `ID`=@id,`Nev`=@nev,`pillanatnyihelyezes`=@pillanatnyihelyezes,`pont1`=@pont1,`ido1`=@ido1,`pont2`=@pont2,`ido2`=@ido2,`pont3`=@pont3,`ido3`=@ido3,`legjobbpont`=@legjobbpont,`legjobbido`=@legjobbido WHERE Nev = \"{versenyzo.Nev}\"";
                            MySqlCommand updatecmd = new MySqlCommand(updatesql, connection);
                            updatecmd.Parameters.AddWithValue("@id", versenyzo.Id);
                            updatecmd.Parameters.AddWithValue("@nev", modositandonev);
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
                            TextDecoration.WriteLineCentered("Sikeres módosítás!", "green");
                            Thread.Sleep(2000);
                            break;
                        case 1:
                            Console.Clear();
                            TextDecoration.WriteLineCentered("=== VERSENYZŐ TÖRLÉSE ===","red");
                            TextDecoration.WriteCentered("Add meg a törlendő versenyző nevét: ");
                            string torlendonev = Console.ReadLine();
                            int torloellen = 0;
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
                                        TextDecoration.WriteLineCentered("Nincs ilyen nevű versenyző!","red");
                                        TextDecoration.WriteLineCentered("Nyomj meg egy gombot a visszatéréshez...");
                                        Console.ReadKey();
                                        return;
                                    }
                                }
                            }
                            connection.Open();
                            string deletesql = $"DELETE FROM kalaplengetőverseny_pdd.versenyzok WHERE Nev = \"{torlendonev}\"";
                            MySqlCommand deletecmd = new MySqlCommand(deletesql, connection);
                            deletecmd.ExecuteNonQuery();
                            connection.Close();
                            TextDecoration.WriteLineCentered("Sikeres törlés!","green");
                            Thread.Sleep(2000);
                            break;
                        case 2:
                            Console.Clear();
                            TextDecoration.WriteLineCentered("=== HELYEZÉSEK KISZÁMÍTÁSA ===", "red");
                            users = HelyezesSzamito(connection);
                            connection.Close();
                            TextDecoration.WriteLineCentered("Helyezések sikeresen frissítve!", "green");
                            Thread.Sleep(2000);
                            break;
                    }
                    break;
                case 3:
                    Console.Clear();
                    TextDecoration.WriteLineCentered("=== HTML GENERÁLÁSA ===","red");
                    HelyezesSzamito(connection);
                    HTMLController.HTMLGeneralo(users);
                    TextDecoration.WriteLineCentered("HTML fájl sikeresen generálva!","green");
                    TextDecoration.WriteLineCentered("Nyomj meg egy gombot a visszatéréshez...");
                    Console.ReadKey();
                    break;
                case 4:
                    Environment.Exit(0);
                    break;
            }   
        }

        private static List<User> HelyezesSzamito(MySqlConnection connection)
        {
            List<User> users = UserController.GetUsers();
            users = users.OrderByDescending(u => u.Legjobbpont).ThenBy(u => u.Legjobbido).ToList();
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
