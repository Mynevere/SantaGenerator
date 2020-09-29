using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SantaGenerator
{
    class Program
    {

        static void Main(string[] args)
        {

            //DB connection
            string datasource = @"DESKTOP-RNC5KLO\SQLEXPRESS";

            string database = "santa_db ";
            string username = "sa";
            string password = "sa";

            string connString = @"Data Source=" + datasource + ";Initial Catalog="
                       + database + ";Persist Security Info=True;User ID=" + username + ";Password=" + password;

            SqlConnection sqlConnection = new SqlConnection( connString);
            sqlConnection.Open();

            //query to insert data in DB
            string query = "insert into participants(name) values(@name)";
            string queryP = "INSERT INTO participants(name) VALUES(@playerName)";

            SqlCommand cmd = new SqlCommand(query,sqlConnection);
            SqlCommand cmdP = new SqlCommand(queryP, sqlConnection);

            // welcome message
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("***Welcome to Santa Generator 2019***");
            Console.ResetColor();
            Console.WriteLine();

            //ask for player name
            Console.WriteLine("Write your name to play: ");
            string playerName = Console.ReadLine();
            Console.WriteLine();
            //write data in DB
            cmdP.Parameters.AddWithValue("@playerName", playerName);
            cmdP.ExecuteNonQuery();


            //
            Console.WriteLine("How many people you want to add in list: ");
            int nrP = Convert.ToInt32( Console.ReadLine());
            while(nrP % 2 != 0)
            {
                Console.WriteLine("You should write an even number.Please write again: ");
                nrP = Convert.ToInt32(Console.ReadLine());
            }
           
            int index = 1;
            
            //add participants names
            Console.WriteLine();
            Console.WriteLine("**Create your secret santa list**");
            Console.WriteLine();
            Console.WriteLine("Name: ");
            while (index < nrP)
            {
                cmd.Parameters.Clear();
                string name = Console.ReadLine();

                cmd.Parameters.AddWithValue("@name", name);
                cmd.ExecuteNonQuery();
                index++;
            }

            int maxP = index + 1; 


            // array to save our participants
            Participants[] participants = new Participants[maxP];

            // get participants from database
            string queryG = "Select id,name from participants";


            var list = new List<Participants>();

            //save participants into array (participants)
            using (SqlCommand cmdI = new SqlCommand(queryG, sqlConnection))
            {

                using (var reader = cmdI.ExecuteReader())
                {
                    while (reader.Read()) {
                        list.Add(new Participants { IdP = reader.GetInt32(0), nameP = reader.GetString(1) });
                        participants = list.ToArray();
                    }

                }
            }


            //recipient generated list
            Generate(list);

            Random random = new Random();
            //print list of secret santa 
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("***Secret Santa Generated List***");
            Console.ResetColor();
            Console.WriteLine();
            for(int i = 0; i < participants.Count(); i++)
            {
                Console.Write(participants[i].nameP + " will sent gift to -> ");
                Console.WriteLine(list[i].nameP);
                cmd.ExecuteNonQuery();
               
            }

           /* string queryD = "Delete from participants";
            SqlCommand cmdD = new SqlCommand(queryD, sqlConnection);
            cmdD.ExecuteNonQuery();
            Console.ReadKey();*/

            
        }

        public static void Generate(List<Participants> p)
        {

            Random rand = new Random();
            for (var i = p.Count(); i > 1;)
            {
                i--;
                var j = (int)(rand.NextDouble() * (i));
                var tmp = p[j];
                p[j] = p[i];
                p[i] = tmp;

            }
        }

    }
}
    
