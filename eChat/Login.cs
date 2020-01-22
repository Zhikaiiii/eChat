using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Get_Server;
using Get_Friend;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Data.SQLite;
using My_data;
namespace eChat
{
    public partial class Login : Form
    {
        private SQLiteConnection connection;
        public Login()
        {
            InitializeComponent();
            connection = My_Database.Connect_Database();
        }

        private void button_login_Click(object sender, EventArgs e)
        {
            string username = textBox_username.Text;
            //Server_Connection get_server = new Server_Connection();
            Socket client_socket = Server_Connection.Connect_Server();
            string search_text = "Select username from user_table where username=" + username;
            string search_text2 = "Select password from user_table where username=" + username;
            object result = My_Database.SQLite_Select(search_text, connection);
            object result2 = My_Database.SQLite_Select(search_text2, connection);
            if (result == null)
            {
                MessageBox.Show(this, "该用户还未注册", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if(result2.ToString() != textBox_password.Text)
            {
                MessageBox.Show(this, "密码错误", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string info;
                info = Server_Connection.Loginto_Server(username, client_socket);
                if (info == "lol")
                {
                    MessageBox.Show(this, "登录成功", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //登陆成功，打开主窗口界面
                    Main_Window main_window = new Main_Window(username, client_socket);
                    main_window.Show();
                    connection.Close();
                    this.Hide();
                }
                else if (info == "Incorrect login No.")
                {
                    MessageBox.Show(this, "该用户名不规范", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void button_registr_Click(object sender, EventArgs e)
        {
            Register register = new Register();
            register.Show();
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
