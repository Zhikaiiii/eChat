using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Windows.Forms;
using Get_Server;
using System.Data.SQLite;
using My_data;
namespace eChat
{
    public partial class Register : Form
    {
        private Socket client_socket;
        private SQLiteConnection connection;
        public Register()
        {
            InitializeComponent();
            client_socket = Server_Connection.Connect_Server();
            connection = My_Database.Connect_Database();
            //connection.Open();
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            textBox_username.Clear();
            textBox_password.Clear();
            textBox_password2.Clear();
        }

        private void button_register_Click(object sender, EventArgs e)
        {
            string username = textBox_username.Text;
            string password = textBox_password.Text;
            string password2 = textBox_password2.Text;
            //密码输入不一致
            if(password != password2)
            {
                MessageBox.Show(this, "密码输入不一致", "信息提示",MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string info = Server_Connection.Loginto_Server(username, client_socket);
                if(info == "Incorrect login No.")
                {
                    MessageBox.Show(this, "用户名不符合规范", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    string search_text = "Select username from user_table where username=" + username;
                    /*
                    Object obj;
                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = search_text;
                        obj = cmd.ExecuteScalar();
                    }
                    */
                    object result = My_Database.SQLite_Select(search_text, connection);
                    if(result != null)
                    {
                        MessageBox.Show(this, "该用户已被注册", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    //注册成功
                    else
                    {
                        string insert_text = "insert into user_table(username,password) values(" + username + "," + password + ")";
                        string insert_text2 = "insert into friend_table(username,friend_list) values(" + username  + "," + username+ ")";
                        /*
                        using (SqlCommand cmd = connection.CreateCommand())
                        {
                            cmd.CommandText = insert_text;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = insert_text2;
                            cmd.ExecuteNonQuery();
                        }
                        */
                        My_Database.SQLite_Insert(insert_text, connection);
                        My_Database.SQLite_Insert(insert_text2, connection);
                        MessageBox.Show(this, "注册成功", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        connection.Close();
                        this.Close();
                    }
                }
            }
            textBox_username.Clear();
            textBox_password.Clear();
            textBox_password2.Clear();
        }
    }
}
