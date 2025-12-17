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
                            Console.Clear();
                            Iras.WriteLineCentered("=== VERSENYZŐ MÓDOSÍTÁSA ===","red");
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
                            Iras.WriteLineCentered("Ha nem egy bizonyos adatot módosítani akkor arra nyomjon szóközt/entert.","yellow");
                            Iras.WriteLineCentered($"Jelenlegi név: {modositandonev}");
                            Iras.WriteCentered("Új név: ");
                            string ujnev = Console.ReadLine();
                            if (ujnev != "")
                            {
                                modositandonev = ujnev;
                            }
                            Iras.WriteCentered($"Jelenlegi első kör pontszám: {versenyzo.pont1} Új pontszám: ");
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
                            Iras.WriteCentered($"Jelenlegi első kör idő: {versenyzo.ido1} Új idő: ");
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
                            Iras.WriteCentered($"Jelenlegi második kör pontszám: {versenyzo.pont2} Új pontszám: ");
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
                            Iras.WriteCentered($"Jelenlegi második kör idő: {versenyzo.ido2} Új idő: ");
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
                            Iras.WriteCentered($"Jelenlegi harmadik kör pontszám: {versenyzo.pont3} Új pontszám: ");
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
                            Iras.WriteCentered($"Jelenlegi harmadik kör idő: {versenyzo.ido3} Új idő: ");
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
                            Thread.Sleep(2000);
                            break;
                        //Versenyző törlése
                        case 1:
                            Console.Clear();
                            Iras.WriteLineCentered("=== VERSENYZŐ TÖRLÉSE ===","red");
                            Iras.WriteCentered("Add meg a törlendő versenyző nevét: ");
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
                                        Iras.WriteLineCentered("Nincs ilyen nevű versenyző!","red");
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
                            Iras.WriteLineCentered("Sikeres törlés!","green");
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
