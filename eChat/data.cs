using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;

//数据库
namespace My_data
{
    public static class My_Database
    {
        //static string connect_str = "server=.;port=3306;user=root;password=root; database=mydatabase;";
        public static SQLiteConnection Connect_Database()
        {
            //string connect_str = @"D:\\Data\\Code\\C#\\eChat2.0\\eChat\\eChat\\data\\database.db";
            string connect_str = @"data\\database.db";
            SQLiteConnection connection = new SQLiteConnection("data source=" + connect_str);
            connection.Open();
            //using (SQLiteCommand command1 = new SQLiteCommand("CREATE TABLE IF NOT EXISTS user_table(username VARCHAR(10) PRIMARY KEY NOT NULL,password VARCHAR(20))", connection))
            //{
                //command1.ExecuteNonQuery();
            //}
            //using (SQLiteCommand command2 = new SQLiteCommand("CREATE TABLE IF NOT EXISTS friend_table(username VARCHAR(10) PRIMARY KEY NOT NULL,friend_list VARCHAR(500))", connection))
            //{
               // command2.ExecuteNonQuery();
           // }
            return connection;
            //connection.Close();
            //MySqlCommand cmd = new MySqlCommand(sql, connection);
        }
        public static void SQLite_Insert(string command, SQLiteConnection connection)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = command;
                cmd.ExecuteNonQuery();
            }
        }
        public static void SQLite_Update(string command, SQLiteConnection connection)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = command;
                cmd.ExecuteNonQuery();
            }
        }

        public static object SQLite_Select(string command, SQLiteConnection connection)
        {
            object obj;
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = command;
                obj = cmd.ExecuteScalar();
            }
            return obj;
        }
        //针对群聊的读取操作
        public static List<chat_group> SQLite_Select_all(string command, SQLiteConnection connection)
        {
            List<chat_group> all_chat_group = new List<chat_group>();
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = command;
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);
                foreach (DataRow r in table.Rows)
                {
                    chat_group new_chat_group = new chat_group();
                    new_chat_group.group_name = r["groupname"].ToString();
                    new_chat_group.user = r["username"].ToString();
                    all_chat_group.Add(new_chat_group);
                    //Console.WriteLine($"{r["group_name"]},{r["name"]},{r["type"]},{r["notnull"]},{r["dflt_value"]},{r["pk"]} ");
                }
                return all_chat_group;
            }

        }
    }
    public class chat_group
    {
        public string group_name = null;
        public string user = null;
    }
}
