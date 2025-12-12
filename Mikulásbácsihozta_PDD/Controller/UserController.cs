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
            string sql = "SELECT * FROM kalaplengetőverseny_pdd.user";
            MySqlCommand cmd = new MySqlCommand(sql, connection);
            List<User> users = new List<User>();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                User user = new User(id: reader.GetInt32("ID"),
                    nev: reader.GetString("Nev"),
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
    }
}
