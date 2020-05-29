﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp1
{
    class Specialization
    {
        public Dictionary<string, string> Info { get; private set; }

        public Specialization()
        {
            Info = new Dictionary<string, string>
            {
                { "City", "" },
                { "School", "" },
                { "Faculty", "" },
                { "Specialization", "" },
                { "Type", "" },
                { "Price", "" }
            };
        }

        public void SetData(string name, string data)
        {
            if (!Info.ContainsKey(name))
                throw new Exception("This key does not exist.");
            Info[name] = data;
            return;
        }

        public bool ValidatePrice()
        {
            if (Info["Price"].Length<1)
            {
                MessageBox.Show("There is no Price");
                return false;
            }
            Regex regex = new Regex("[0-9]");
            if (regex.Matches(Info["Price"]).Count== Info["Price"].Length)
            {
                if (Convert.ToInt32(Info["Price"]) > 0)
                    return true;
            }

            MessageBox.Show("Странная цена");
            return false;
        }

        public bool ValidateStrings()
        {
            foreach (KeyValuePair<string, string> kvp in Info)
            {
                if (Info[kvp.Key].Length < 1)
                {
                    MessageBox.Show($"{kvp.Key} is empty, set it!");
                    return false;
                }
            }
            return true;
        }

        public void AddToDb()
        {
            if (!(ValidatePrice() && ValidateStrings()))
            {
                MessageBox.Show("Странные данные.");
                return;
            }

            string insertionCommand = "INSERT INTO `specializations` (`id`, `City`, `School`, `Faculty`," +
                " `Specialization`, `StudyType`, `Price`) VALUES (NULL, @City, @School, @Faculty," +
                                      " @Specialization, @StudyType, @Price)";

            DB db = new DB();
            MySqlCommand command = new MySqlCommand(insertionCommand, db.getConnection());
            command.Parameters.Add("@City", MySqlDbType.VarChar).Value = Info["City"];
            command.Parameters.Add("@School", MySqlDbType.VarChar).Value = Info["School"];
            command.Parameters.Add("@Faculty", MySqlDbType.VarChar).Value = Info["Faculty"];
            command.Parameters.Add("@Specialization", MySqlDbType.VarChar).Value = Info["Specialization"];
            command.Parameters.Add("@StudyType", MySqlDbType.VarChar).Value = Info["Type"];
            command.Parameters.Add("@Price", MySqlDbType.VarChar).Value = Info["Price"];

            db.openConnection();

            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Ok");
                
            }
            else
            {
                MessageBox.Show("Проблемы с базой.");
            }

            db.closeConnection();
        }
    }
}
