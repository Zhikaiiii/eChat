using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Get_Friend;
using Get_Server;
using System.Data.SQLite;
using My_data;
using NAudio.Wave;
namespace eChat
{
    public partial class Chat_Window : Form
    {
        private SQLiteConnection connection;
        private Socket socket_to_server;//与服务器通信的套接字
        public List<Socket> socket_to_friend = new List<Socket>();
        public List<string> new_msg = new List<string>();
        public List<string> user_name = new List<string>();
        public delegate void addRecord(string recv_str);
        public delegate void fileReceive_Delegate(Socket ss, Chat_Window chat_Window, string filename);
        private int chat_num=0;//聊天人数
        private bool send_state = true;//表示发送消息还是文件
        private bool voice_state = false;
        private string filepath = null;//存储文件打开路径
        private string filename = null;//存储文件名
        private string recv_filename = null;//存储接受到的文件名
        public string recordpath = null;//聊天记录路径
        public string datapath = null;
        public int file_requestuser = -1;//文件发送方
        private WaveInEvent sourceStream;
        private WaveOut recievedStream;
        private BufferedWaveProvider waveProvider;
        private Thread thread_voice_chat;

        public Chat_Window(List<string> recv_msg, List<Socket> s, int num, int con_num)
        {
            InitializeComponent();
            connection = My_Database.Connect_Database();
            socket_to_server = Server_Connection.Connect_Server();
            user_name = recv_msg[0].Substring(5).Split('.').ToList();
            List<string> unconnect_name = new List<string>(user_name);
            unconnect_name.RemoveAt(0);
            chat_num = num;
            datapath = ".\\data\\" + user_name[0] + "\\";
            if (!Directory.Exists(datapath))
            {
                Directory.CreateDirectory(datapath);
            }
            //读入聊天记录
            recordpath = ".\\data\\" + user_name[0] + "\\";
            for (int i = 0; i < chat_num; i++)
            {
                recordpath = recordpath + user_name[i + 1];
            }
            recordpath = recordpath + ".txt";

            StreamReader sr = new StreamReader(new FileStream(recordpath, FileMode.OpenOrCreate));
            string exist_record;
            while ((exist_record = sr.ReadLine()) != null)
            {
                string[] record_split = exist_record.Split('.');
                if(record_split.Length >= 3)
                {
                    if (record_split[0] == user_name[0])
                    {
                        richTextBox1.SelectionAlignment = HorizontalAlignment.Left;
                    }
                    else
                    {
                        richTextBox1.SelectionAlignment = HorizontalAlignment.Right;
                    }
                    string show_msg = record_split[0] + " " + record_split[1] + "\n" + record_split[2] + "\n";
                    richTextBox1.AppendText(show_msg);
                }
                else
                    richTextBox1.AppendText(exist_record);
            }
            sr.Close();
            //如果为接收消息方 显示消息
            for (int i = 0; i <recv_msg.Count();i++)
            {
                string[] info = recv_msg[i].Split('.');
                if (info[0] == "info")
                {
                    //string recv_msg = fri_name + recv_str.Split('.')[2];
                    string show_msg = info[2] + " " +  info[info.Length - 1];
                    richTextBox1.SelectionAlignment = HorizontalAlignment.Right;
                    richTextBox1.AppendText(show_msg);
                    new_msg.Add(show_msg);
                    if(i == 0)
                    {
                        unconnect_name.Remove(info[2]);
                        user_name.RemoveAt(user_name.Count() - 1);
                    }
                }
            }

            if (s.Count() != 0)
            {
                socket_to_friend = s;
                //已连接用户
                for(int j =0; j<con_num;j++)
                {
                    string send_message = "onchat." + user_name[0];
                    P2P_Communication.Connect_Send(socket_to_friend[j], send_message);
                }
                for(int i =con_num;i<chat_num;i++)
                {
                    socket_to_friend.Add(P2P_Communication.Commun_Friend(unconnect_name[i-con_num], socket_to_server));
                    
                    string send_message = "chat.";
                    for(int j =0;j<=chat_num;j++)
                    {
                        send_message = send_message + user_name[j] + ".";
                    }
                    P2P_Communication.Connect_Send(socket_to_friend[i], send_message);
                }           
            }
            else
            {
                for (int i = con_num; i < chat_num; i++)
                {
                    socket_to_friend.Add(P2P_Communication.Commun_Friend(unconnect_name[i - con_num], socket_to_server));
                    string send_message = "chat.";
                    for (int j = 0; j <= chat_num; j++)
                    {
                        send_message = send_message + user_name[j] + ".";
                    }
                    P2P_Communication.Connect_Send(socket_to_friend[i], send_message);
                }
            }
            //开始接受消息
            for (int i= 0; i< chat_num;i++)
            {
                P2P_Communication.Chat_Receive(socket_to_friend[i], this, true);
            }
            //控件初始化
            //如果为群聊
            if (chat_num > 1)
            {
                textBox_name.Text = findGroup(user_name);
            }
            string others = null;
            for(int i = 0;i<chat_num;i++)
            {
                others = others + user_name[i + 1] + "\n";
            }
            label3.Text = user_name[0];
            label4.Text = others;
            label5.Hide();
            button_fileaccept.Hide();
            button_filerefuse.Hide();
            button_voicerefuse.Hide();
            if(chat_num > 1)
            {
                button_voicechat.Enabled = false;
            }
            waveProvider = new BufferedWaveProvider(new WaveFormat(8000, 16, WaveIn.GetCapabilities(0).Channels));
            recievedStream = new WaveOut();
            recievedStream.Init(waveProvider);

        }

        //找到用户列表对应的群聊名称
        private string findGroup(List<string> all_name)
        {
            string search_text = "Select * from group_table";
            List<chat_group> all_chat_group = My_Database.SQLite_Select_all(search_text, connection);
            foreach(chat_group group in all_chat_group)
            {
                List<string> all_user = group.user.Split('.').ToList();
                all_user.Sort();
                List<string> temp_user = new List<string>(all_name);
                temp_user.Sort();
                if(all_user.All(temp_user.Contains) && all_user.Count == temp_user.Count)
                {
                    return group.group_name;
                }
            }
            return "";
        }

        //收到信息并显示
        public void add_info(string recv_str)
        {
            string[] info = recv_str.Split('.');

            if (richTextBox1.InvokeRequired)
            {
                addRecord d = new addRecord(add_info);
                this.Invoke(d, new object[] { recv_str });
            }
            else
            {
                if(info[0] == "info")
                {
                    string show_mess = info[1] + " " + info[info.Length - 1];
                    string save_mess = info[1] + "." + info[info.Length - 1];
                    richTextBox1.SelectionAlignment = HorizontalAlignment.Right;
                    richTextBox1.AppendText(show_mess);
                    new_msg.Add(save_mess);
                }
                else if (info[0] == "file")
                {
                    string show_mess = info[1] + " " + info[info.Length-2] + "." + info[info.Length - 1];
                    recv_filename = recv_str.Split(' ')[recv_str.Split(' ').Length-1];
                    recv_filename = recv_filename.Replace("\n", "");
                    file_requestuser = user_name.IndexOf(info[1]) - 1;
                    richTextBox1.SelectionAlignment = HorizontalAlignment.Right;
                    richTextBox1.AppendText(show_mess);
                    button_filerefuse.Show();
                    button_fileaccept.Show();
                }
                else if(info[0] == "fileaccept")
                {
                    int file_sender = user_name.IndexOf(info[1]) - 1;
                    if (!P2P_Communication.File_Send(socket_to_friend[file_sender], filepath))
                    {
                        MessageBox.Show(this, "文件发送失败", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    MessageBox.Show(this, "文件发送成功", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    send_state = true;
                }
                //语音聊天
                else if(info[0] == "voice")
                {
                    string show_mess = info[1] + " " + info[info.Length - 1] + "向您发起了语音通话\n";
                    richTextBox1.SelectionAlignment = HorizontalAlignment.Right;
                    richTextBox1.AppendText(show_mess);
                    button_voicechat.Text = "接听";
                    button_voicerefuse.Show();
                }
                else if(info[0] == "voiceaccept")//接受聊天
                {
                    //voice_socket = P2P_Communication.Commun_Friend(user_name[1], socket_to_server);
                    //thread_voice_chat = new Thread(() => start_voice_chat(voice_socket));
                    //voice_socket = P2P_Communication.Commun_Friend(user_name[1], socket_to_server);
                    thread_voice_chat = new Thread(() => start_voice_chat(socket_to_friend[0]));
                    thread_voice_chat.SetApartmentState(System.Threading.ApartmentState.STA);//单线程监听控制
                    thread_voice_chat.IsBackground = true;
                    thread_voice_chat.Start();
                    button_voicechat.Text = "接听";
                    button_voicerefuse.Show();
                    button_voicerefuse.Text = "挂断";
                    label5.Show();
                }
                else if(info[0] == "voiceend")
                {
                    //end_voice_chat(socket_to_friend[0]);
                    voice_state = false;
                    thread_voice_chat.Abort();

                    string show_mess = info[1] + " " + info[info.Length - 1] + "通话结束";
                    richTextBox1.SelectionAlignment = HorizontalAlignment.Right;
                    richTextBox1.AppendText(show_mess);
                    button_voicerefuse.Text = "拒绝";
                    button_voicechat.Text = "音频";
                    button_voicerefuse.Hide();
                    label5.Hide();
                }
            }
        }

        #region 控件槽函数
        //发消息
        private void button_send_Click(object sender, EventArgs e)
        {
            string send_message = null;
            if (send_state)//若为发消息
            {
                send_message = "info." + user_name[0] + ".";
            }
            else//发文件之前先通知其他用户
            {
                send_message = "file." + user_name[0] + ".";
                send_state = false;
            }
            string show_message = user_name[0] + " " + DateTime.Now.ToString() + "\n" + textBox1.Text + "\n";
            string save_message = user_name[0] + "." + DateTime.Now.ToString() + "." + textBox1.Text;
            richTextBox1.SelectionAlignment = HorizontalAlignment.Left;
            richTextBox1.AppendText(show_message);
            new_msg.Add(save_message);

            for (int i = 0; i < chat_num; i++)
            {
                send_message = send_message + user_name[i + 1] + ".";
            }
            send_message = send_message + DateTime.Now.ToString() + "\n" + textBox1.Text + "\n";
            for (int i = 0; i < chat_num; i++)
            {
                if (!socket_to_friend[i].Connected)
                {
                    socket_to_friend[i] = P2P_Communication.Commun_Friend(user_name[i + 1], socket_to_server);
                    P2P_Communication.Chat_Receive(socket_to_friend[i], this, true);
                }
                P2P_Communication.Connect_Send(socket_to_friend[i], send_message);
            }

            textBox1.Clear();
            //socket_to_fri.Send(Encoding.ASCII.GetBytes(send_message));
        }
        //关闭窗口
        private void button_close_Click(object sender, EventArgs e)
        {
            string send_message = "bye." + user_name[0];//发送离开聊天信息
            foreach (Socket socket in socket_to_friend)
            {
                if (!socket.Connected)
                {
                  continue;
                }
                P2P_Communication.Connect_Send(socket, send_message);
                Thread.Sleep(10);
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            //FileStream source_stream = new FileStream(recordpath, FileMode.OpenOrCreate, FileAccess.Write);
            socket_to_server.Shutdown(SocketShutdown.Both);
            socket_to_server.Close();
            //写入新的聊天记录
            StreamWriter sw = new StreamWriter(recordpath,true);
            foreach (string msg in new_msg)
            {
                string new_msg = msg.Replace("\n", ".");
                sw.WriteLine(new_msg);
            }
            sw.Close();
            this.Close();
        }

        //选择文件
        private void button_filesend_Click(object sender, EventArgs e)
        {
            //Thread thread_file = new Thread(() => Application.Run(new Chat_Window(all_user, chat_socket, chat_num, 0)));
            //thread_file.Start();
            OpenFileDialog file_dlg = new OpenFileDialog();
            file_dlg.ShowDialog();
            file_dlg.InitialDirectory = "./";
            filepath = file_dlg.FileName;
            filename = filepath.Split('\\').Last();
            if(filename.Length>0)
            {
                textBox1.Text = "Send File: " +  filename;
                send_state = false;
            }
            //string filename = filepath[filepath.Length - 2] + filepath[filepath.Length - 1];
        }

        //确定接受文件
        private void button_fileaccept_Click(object sender, EventArgs e)
        {
            fileReceive_Delegate f = new fileReceive_Delegate(P2P_Communication.File_Receive);
            this.Invoke(f, new object[] { socket_to_friend[file_requestuser], this, recv_filename });
 
            button_filerefuse.Hide();
            button_fileaccept.Hide();
        }
        private void button_filerefuse_Click(object sender, EventArgs e)
        {
            button_filerefuse.Hide();
            button_fileaccept.Hide();
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.TextLength;
            richTextBox1.ScrollToCaret();
        }
        //更改群名
        private void textBox_name_TextChanged(object sender, EventArgs e)
        {
            string current_group_name = findGroup(user_name);
            string new_group_name = textBox_name.Text;
            string update_text = "Update group_table set groupname='" + new_group_name + "' where groupname = '" + current_group_name + "'";
            My_Database.SQLite_Update(update_text, connection);
        }
        //发起语音聊天
        private void button_voicechat_Click(object sender, EventArgs e)
        {
            //发起语音
            if(button_voicechat.Text == "音频")
            {
                string send_msg = "voice." + user_name[0] + "." + DateTime.Now.ToString() + "\n";
                P2P_Communication.Connect_Send(socket_to_friend[0], send_msg);
            }
            //接听
            else
            {
                string send_msg = "voiceaccept." + user_name[0] + "." + DateTime.Now.ToString() + "\n";
                P2P_Communication.Connect_Send(socket_to_friend[0], send_msg);
                label5.Show();
                button_voicerefuse.Text = "挂断";
                //voice_socket = P2P_Communication.Commun_Friend(user_name[1], socket_to_server);
                //thread_voice_chat = new Thread(() => start_voice_chat(voice_socket));
                //voice_socket = P2P_Communication.Commun_Friend(user_name[1], socket_to_server);
                thread_voice_chat = new Thread(() => start_voice_chat(socket_to_friend[0]));
                thread_voice_chat.SetApartmentState(System.Threading.ApartmentState.STA);//单线程监听控制
                thread_voice_chat.IsBackground = true;
                thread_voice_chat.Start();
            }

        }
        //拒绝语音聊天
        private void button_voicerefuse_Click(object sender, EventArgs e)
        {
            if(button_voicerefuse.Text == "拒绝")
            {
                string send_msg = "info." + user_name[0] + "." + DateTime.Now.ToString() + "\n" + "拒绝通话请求\n";
                P2P_Communication.Connect_Send(socket_to_friend[0], send_msg);
                button_voicechat.Text = "音频";
                button_voicerefuse.Hide();
            }
            else
            {
                voice_state = false;
                //end_voice_chat(socket_to_friend[0]);
                thread_voice_chat.Abort();
                string send_msg = "voiceend." + user_name[0] + "." + DateTime.Now.ToString() + "\n";
                P2P_Communication.Connect_Send(socket_to_friend[0], send_msg);
                P2P_Communication.Chat_Receive(socket_to_friend[0], this, true);
                P2P_Communication.Chat_Receive(socket_to_friend[0], this, true);
                button_voicechat.Text = "音频";
                button_voicerefuse.Hide();
                label5.Hide();
            }
        }
        #endregion
        public void start_voice_chat(Socket voice_socket)
        {
            voice_state = true;
            sourceStream = new WaveInEvent
            {
                DeviceNumber = 0,
                WaveFormat = new WaveFormat(8000, 16, WaveIn.GetCapabilities(0).Channels)
            };

            sourceStream.DataAvailable += sourceStream_DataAvailable;
            P2P_Communication.Voice_Receive(voice_socket,recievedStream,waveProvider,this);
            //sourceStream.DataAvailable += sourceStream_DataAvailable;
            sourceStream.StartRecording();

        }
        private void sourceStream_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (sourceStream == null)
                return;
            P2P_Communication.Voice_Send(e.Buffer, e.BytesRecorded, sourceStream, voice_state,socket_to_friend[0]);
        }
        public void end_voice_chat()
        {
            voice_state = false;
            thread_voice_chat.Abort();
            button_voicechat.Text = "音频";
            button_voicerefuse.Hide();
            P2P_Communication.Chat_Receive(socket_to_friend[0], this, true);
            P2P_Communication.Chat_Receive(socket_to_friend[0], this, true);

        }


    }
}
