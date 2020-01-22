using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Get_Friend;
using Get_Server;
using System.Data.SQLite;
using My_data;
namespace eChat
{

    public partial class Main_Window : Form
    {
        //主界面
        //参数传递
        private Socket client_socket;//本机与服务器的套接字
        private SQLiteConnection connection;
        private int chat_select;//表示选择群聊列表or单聊列表
        public string user_name;
        public string friend_list = null;
        public int unread_num;
        public delegate void add_Unread_msg(string recv_str, Socket socket);//处理其他消息
        public delegate void query_Friend(bool flag);//查询好友状态
        public delegate void add_To_list(string recv_str);//添加未读消息
        List<unread_Object> unread_list = new List<unread_Object>();
        List<chat_group> all_chat_group = new List<chat_group>();
        //未读消息的集合
        ThreadStart threadstart_query;
        Thread thread_query;

        public Main_Window(string username, Socket s)
        {
            InitializeComponent();
            //数据库连接
            connection = My_Database.Connect_Database();
            label_username.Text = username;
            user_name = username;
            client_socket = s;
            int listen_port;
            listen_port = Convert.ToInt32(username.Substring(6)) + 50000;
            //开始监听
            P2P_Communication.Begin_Listening(listen_port,user_name,this);
            string search_text = "Select friend_list from friend_table where username=" + user_name;
            object friend_all = My_Database.SQLite_Select(search_text, connection).ToString();
            //string[] friend_array;
            //friend_list = friend_all.ToString().Split('.');
            if(friend_all.ToString().Length >10)
                friend_list = friend_all.ToString().Substring(11);
            get_friend_list(true);
            get_group_list();
            //创建进程刷新好友在线状态
            threadstart_query = new ThreadStart(query_friend_state);
            thread_query  = new Thread(threadstart_query);
            thread_query.IsBackground = true;
            thread_query.Start();
            chat_select = 0;
            unread_num = 0;
            listView_unread.Hide();
            listView_group.Hide();
            button_approve.Hide();
            button_refuse.Hide();
            label1.Hide();
            label2.Hide();
            button_refuse.Enabled = false;
            button_approve.Enabled = false;
            //unread_msg = null;
        }
        //得到群聊列表
        private void get_group_list()
        {
            string search_text = "Select * from group_table";
            all_chat_group = My_Database.SQLite_Select_all(search_text, connection);
            foreach(chat_group group in all_chat_group)
            {
                string[] group_username = group.user.Split('.');
                foreach(string user in group_username)
                {
                    if(user == user_name)
                    {
                        ListViewItem new_item = new ListViewItem(group.group_name);
                        listView_group.Items.Add(new_item);
                    }
                }
            }
        }
        //得到好友列表
        private void get_friend_list(bool flag)
        {
            if (listView1.InvokeRequired)
            {
                query_Friend d = new query_Friend(get_friend_list);
                this.Invoke(d, new object[] {false});
            }
            else
            {
                //listView1.Clear();
                if (friend_list != null)
                {
                    string[] friend_array;
                    friend_array = friend_list.Split('.');
                    for(int i=0;i<friend_array.Length;i++)
                    {
                        string friend = friend_array[i];
                        string info = Server_Connection.Search_Friend(friend, client_socket);
                        if(flag)//第一次刷新
                        {
                            ListViewItem new_item = new ListViewItem();
                            new_item.SubItems[0].Text = friend;
                            if (info == "n")
                            {
                                new_item.SubItems.Add("offline");
                            }
                            else
                            {
                                new_item.SubItems.Add("online");
                            }
                            listView1.Items.Add(new_item);
                        }
                        else
                        {
                            if (info == "n")
                            {
                                listView1.Items[i].SubItems[1].Text = "offline";
                            }
                            else
                            {
                                listView1.Items[i].SubItems[1].Text = "online";
                            }
                        }
                    }
                }

                //Console.WriteLine(DateTime.Now.ToString() + "\n");
            }
            //Thread.Sleep(500);
            //get_friend_list();
        }

        private void query_friend_state()
        {
            Thread.Sleep(1000);
            while(true)
            {
                get_friend_list(false);
                Thread.Sleep(1000);
            }

        }

        //添加朋友
        private void button_addfriend_Click(object sender, EventArgs e)
        {
            string friend_name = textBox_friendname.Text;
            bool state = true;
            //判断是否已经是好友
            foreach (ListViewItem item in listView1.Items)
            {
                if(friend_name == item.Text)
                {
                    MessageBox.Show(this, "已经在好友列表中", "信息提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    state = false;
                    break;
                }
            }
            //判断是否存在该账号
            if(state)
            {
                string recv_str = Server_Connection.Search_Friend(friend_name, client_socket);
                if(recv_str == "Incorrect No." || recv_str == "Please send the right message")
                {
                    MessageBox.Show(this, "用户不存在", "信息提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    string search_text = "Select username from user_table where username=" + friend_name;
                    object result = My_Database.SQLite_Select(search_text, connection);
                    if (result == null)
                    {
                        MessageBox.Show(this, "该用户还未注册", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if(recv_str == "n")
                    {
                        MessageBox.Show(this, "该用户不在线，请稍后再试", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {

                        Socket add_friend_socket = P2P_Communication.Commun_Friend(friend_name, client_socket);
                        string send_msg = "a." + user_name;
                        P2P_Communication.Connect_Send(add_friend_socket, send_msg);
                        Thread.Sleep(10);
                        add_friend_socket.Shutdown(SocketShutdown.Both);
                        add_friend_socket.Close();
                    }
                }
            }
        }

        public void add_to_list(string recv_str)
        {
            if (listView1.InvokeRequired)
            {
                add_To_list a = new add_To_list(add_to_list);
                this.Invoke(a, new object[] { recv_str });
            }
            else
            {
                string sender = find_group(recv_str) + " " + recv_str.Split('.')[1];
                listView_unread.Items.Add(sender);
                button_msglist.Text = "新消息!!!";
                button_msg.Enabled = true;
            }
        }
        //处理未读消息
        public void add_unread_record(string recv_str, Socket socket)
        {

            if (button_msg.InvokeRequired || label2.InvokeRequired || label2.InvokeRequired
                || label1.InvokeRequired || button_refuse.InvokeRequired || button_approve.InvokeRequired
                || listView1.InvokeRequired)
            {
                add_Unread_msg d = new add_Unread_msg(add_unread_record);
                this.Invoke(d, new object[] { recv_str, socket });
            }
            else
            {
                if(recv_str.Split('.')[0] == "a")//好友申请
                {
                    string friend_name = recv_str.Split('.')[1];
                    //Socket add_friend_socket = P2P_Communication.Commun_Friend(friend_name, client_socket);
                    label1.Show();
                    label2.Show();
                    label2.Text = friend_name + " 请求添加好友";
                    button_approve.Show();
                    button_refuse.Show();
                    button_approve.Enabled = true;
                    button_refuse.Enabled = true;
                }
                else if (recv_str.Split('.')[0] == "aa")//好友申请
                {
                    string friend_name = recv_str.Split('.')[1];
                    friend_list = friend_list + "." + friend_name;
                    string insert_text = "update friend_table set friend_list = '" + user_name + "." + friend_list + "' where username=" + user_name;
                    //string insert_text2 = "update friend_table set friend_list = '" + friend_list + "' where username=" + user_name;
                    My_Database.SQLite_Insert(insert_text, connection);
                    ListViewItem new_item = new ListViewItem();
                    new_item.SubItems[0].Text = friend_name;
                    new_item.SubItems.Add("online");
                    listView1.Items.Add(new_item);
                    MessageBox.Show(this, friend_name + " 通过了你的好友申请", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if(recv_str.Split('.')[0] == "ar")
                {
                    string friend_name = recv_str.Split('.')[1];
                    MessageBox.Show(this, friend_name + " 拒绝了你的好友申请", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    //收到一条新的未读消息
                    unread_Object new_obj = new unread_Object();
                    //string user = recv_str.Replace(user_name, "");
                    string user = recv_str.Substring(5);
                    string[] user_list = user.Split('.');
                    List<string> all_user = user_list.ToList();
                    if(recv_str.Split('.')[0] == "info")
                    {
                        new_obj.msg_count++;
                    }
                    all_user.Sort();//排序
                    new_obj.user_list = all_user;
                    all_user.RemoveAt(all_user.Count() - 1);
                    //new_obj.unread_msg.Add(recv_str);
                    new_obj.unread_socket = socket;
                    new_obj.main_Window = this;
                    //new_obj.socket_count++;
                    unread_list.Add(new_obj);
                    P2P_Communication.Unread_Chat_Receive(unread_list[unread_num], unread_num);
                    //Chat_Receive(unread_list[unread_num], unread_num);
                    unread_num++;
                    //string show_mess = info[1] + " " + info[info.Length - 1];
                    if(recv_str.Split('.')[0] == "info")
                    {
                        string sender = recv_str.Split('.')[1];
                        listView_unread.Items.Add(sender);
                        button_msglist.Text = unread_num.ToString();
                        button_msg.Enabled = true;
                    }
                }
            }
            //listView1.Items.Add(new_item);
        }

        //发起聊天
        private void button_chat_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0 && listView_group.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, " 请选中好友进行聊天", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (listView_group.SelectedItems.Count == 0 && listView1.SelectedItems[0].SubItems[1].Text == "offline")
            {
                MessageBox.Show(this, " 好友不在线，无法聊天", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                List<string> all_user = new List<string>();
                List<Socket> chat_socket = new List<Socket>();
                int chat_num = 0; //聊天人数
                if (chat_select == 0)
                {
                    string friend_name = user_name;
                    foreach (ListViewItem item in listView1.SelectedItems)
                    {
                        friend_name = friend_name + "." + item.Text;
                        chat_num++;
                    }
                    //开始发起聊天请求
                    string send_str = "chat." + friend_name;
                    all_user.Add(send_str);
                    if (chat_num > 1)
                    {
                        string select_text = "Select count(*) from group_table";
                        string group_name = "'chatgroup" + My_Database.SQLite_Select(select_text, connection) + "'";
                        //string group_name = "ccc";
                        string insert_text = "insert into group_table(groupname,username) values(" + group_name + "," + user_name + ")";
                        My_Database.SQLite_Insert(insert_text, connection);
                        string update_text = "update group_table set username = '" + friend_name + "' where groupname=" + group_name;
                        My_Database.SQLite_Update(update_text, connection);
                        ListViewItem new_item = new ListViewItem(group_name);
                    }
                }
                else
                {
                    int ind = listView_group.Items.IndexOf(listView_group.SelectedItems[0]);
                    string[] user_list = all_chat_group[ind].user.Split('.');
                    string send_str = "chat.";
                    string all = null;
                    for (int i = 0; i < user_list.Length; i++)
                    {
                        if (user_list[i] == user_name)
                        {
                            user_list[i] = user_list[0];
                            user_list[0] = user_name;
                            all = string.Join(".", user_list);
                            break;
                        }
                    }
                    send_str = send_str + all;

                    chat_num = all_chat_group[ind].user.Split('.').Length - 1;
                    all_user.Add(send_str);
                }
                //chat_socket = P2P_Communication.Commun_Friend(send_str, client_socket);
                //Socket chat_socket = P2P_Communication.Commun_Friend(friend_name, client_socket);
                //创建窗口
                Thread thread_chat = new Thread(() => Application.Run(new Chat_Window(all_user, chat_socket, chat_num, 0)));
                thread_chat.SetApartmentState(System.Threading.ApartmentState.STA);//单线程监听控制
                thread_chat.Start();
            }
        }

        //注销
        private void button_logout_Click(object sender, EventArgs e)
        {
            Server_Connection.Log_Out(user_name, client_socket);
            Login login = new Login();
            login.Show();
            connection.Close();
            thread_query.Abort();
            this.Hide();
        }

        //查看未读消息列表
        private void button_msglist_Click(object sender, EventArgs e)
        {
            listView1.Hide();
            listView_group.Hide();
            listView_unread.Show();
            chat_select = 0;
        }
        //查看好友列表
        private void button_friend_Click(object sender, EventArgs e)
        {
            listView1.Show();
            listView_unread.Hide();
            listView_group.Hide();
            chat_select = 0;
        }
        //查看群聊列表
        private void button_group_Click(object sender, EventArgs e)
        {
            listView1.Hide();
            listView_unread.Hide();
            listView_group.Show();
            chat_select = 1;
        }

        //查看消息
        private void button_msg_Click(object sender, EventArgs e)
        {
            //int index = comboBox_unread.SelectedIndex;
            int index = listView_unread.SelectedItems[0].Index;
            unread_Object selected_obj = unread_list[index];
            List<Socket> chat_socket = new List<Socket>();
            //chat_socket.Add(selected_obj.unread_socket);
            List<string> all_msg = new List<string>(); ;//消息类型
            int con_num = 0;
            int chat_num = selected_obj.user_list.Count() - 1;//聊天人数
            //unread_list.RemoveAt(index);
            //selected_obj.flag = false;
            //comboBox_unread.Items.Remove(index);
            //unread_num--;
            int i = 0;
            int m = 0;
            while(i<unread_list.Count())
            {
                if (unread_list[i].user_list.SequenceEqual(selected_obj.user_list))//如果用户列表相同
                {
                    chat_socket.Add(unread_list[i].unread_socket);
                    
                    con_num++;
                    foreach (string msg in unread_list[i].unread_msg)
                    {
                        string[] split_msg = msg.Split('.');
                        if (split_msg[0] == "info")
                        {
                            for (int k = 1; k < split_msg.Length - 1; k++)
                            {
                                if (split_msg[k] == user_name)
                                {
                                    string temp = split_msg[k];
                                    string temp2 = split_msg[1];
                                    split_msg[k] = split_msg[2];//自己
                                    split_msg[1] = temp;
                                    split_msg[2] = temp2;//消息发送方
                                    //split_msg[i] = temp2;
                                }
                            }
                            //unread_msg[count] = string.Join(".", split_msg);
                            //count++;
                        }
                        string new_msg = string.Join(".", split_msg);
                        all_msg.Add(new_msg);
                        // lengths = unread_list.unread_socket.EndReceive(ar);
                    }
                    unread_list[i].flag = false;
                    if (unread_list[i].msg_count != 0)
                    {
                        listView_unread.Items.RemoveAt(m);
                    }
                    unread_list.RemoveAt(i);
                    unread_num--;
                }
                else
                {
                    i++;
                    m++;
                }
            }
            button_msglist.Text = "新消息";
            if(unread_num == 0)
            {
                button_msg.Enabled = false;
            }
            Thread thread_chat = new Thread(() => Application.Run(new Chat_Window(all_msg, chat_socket, chat_num, con_num)));
            thread_chat.SetApartmentState(System.Threading.ApartmentState.STA);//单线程监听控制
            thread_chat.Start();
        }

        //通过好友申请
        private void button_approve_Click(object sender, EventArgs e)
        {
            string friend_name = label2.Text.Split(' ')[0];
            Socket add_friend_socket = P2P_Communication.Commun_Friend(friend_name, client_socket);
            string send_msg = "aa." + user_name;
            P2P_Communication.Connect_Send(add_friend_socket, send_msg);
            Thread.Sleep(10);
            add_friend_socket.Shutdown(SocketShutdown.Both);
            add_friend_socket.Close();
            label1.Hide();
            label2.Hide();
            button_approve.Hide();
            button_refuse.Hide();
            button_refuse.Enabled = false;
            button_approve.Enabled = false;
            if (friend_list == null)
                friend_list = friend_name;
            else
                friend_list = friend_list + "." + friend_name;
            string update_text = "update friend_table set friend_list = '" + user_name + "." + friend_list + "' where username=" + user_name;
            My_Database.SQLite_Update(update_text, connection);
            ListViewItem new_item = new ListViewItem();
            new_item.SubItems[0].Text = friend_name;
            new_item.SubItems.Add("online");
            listView1.Items.Add(new_item);
        }

        //拒绝好友申请
        private void button_refuse_Click(object sender, EventArgs e)
        {
            string friend_name = label2.Text.Split(' ')[0];
            Socket add_friend_socket = P2P_Communication.Commun_Friend(friend_name, client_socket);
            string send_msg = "ar." + user_name;
            P2P_Communication.Connect_Send(add_friend_socket, send_msg);
            Thread.Sleep(10);
            add_friend_socket.Shutdown(SocketShutdown.Both);
            add_friend_socket.Close();
            label1.Hide();
            label2.Hide();
            button_approve.Hide();
            button_refuse.Hide();
            button_refuse.Enabled = false;
            button_approve.Enabled = false;
        }

        //找到用户列表对应的群聊名称
        private string find_group(string recv_str)
        {
            string search_text = "Select * from group_table";
            List<chat_group> all_chat_group = My_Database.SQLite_Select_all(search_text, connection);
            List<string> all_name = recv_str.Split('.').ToList();
            all_name.RemoveAt(0);
            all_name.RemoveAt(all_name.Count() - 1);
            foreach (chat_group group in all_chat_group)
            {
                List<string> all_user = group.user.Split('.').ToList();
                all_user.Sort();
                List<string> temp_user = new List<string>(all_name);
                temp_user.Sort();
                if (all_user.All(temp_user.Contains) && all_user.Count == temp_user.Count)
                {
                    return group.group_name;
                }
            }
            return "";
        }

        private void Main_Window_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }

    //未读消息类
    public class unread_Object
    {
        public List<string> unread_msg = new List<string>();//用户
        public Socket unread_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//套接字
        public List<string> user_list = new List<string>();//用户
        public int msg_count = 0;//消息个数
                                 //public int socket_count = 0;//套接字个数
        public bool flag = true;
        public Main_Window main_Window = null;

    }
}
