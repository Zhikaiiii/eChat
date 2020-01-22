using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using Get_Server;
using System.Windows.Forms;
using System.IO;
using eChat;
using NAudio.Wave;
namespace Get_Friend
{
    //p2p通信
    public class P2P_Communication
    {
        //聊天窗口接受消息对应的Object
        public class ChatObject
        {
            public Socket client_socket = null;
            public string recv_str = null;
            public Chat_Window chat_window = null;
            public bool chat_state = true;
        }
        //主窗口接受消息对应的Object
        public class MainObject
        {
            public Socket work_socket = null;
            public string user_name = null;
            public Main_Window main_window = null;
        }
        //音频通话对应的Object
        public class VoiceObject
        {
            public WaveOut recieved_stream = null;
            public BufferedWaveProvider wave_provider =null;
            public Socket work_socket = null;
            public Chat_Window chat_Window = null;
        }

        static byte[] recv_bytes = new byte[8192];
        static byte[] voice_buffer = new byte[5242880];

        //开始监听
        public static void Begin_Listening(int listen_port, string user_name, Main_Window main_window)
        {
            IPAddress local_ip = Get_Local_IP();
            //int listen_port = 12000;
            IPEndPoint endpoint = new IPEndPoint(local_ip, listen_port);
            //创建套接字
            Socket server_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //关联套接字
            server_socket.Bind(endpoint);
            server_socket.Listen(32);
            MainObject s_obj = new MainObject();
            s_obj.work_socket = server_socket;
            s_obj.user_name = user_name;
            s_obj.main_window = main_window;
            //开始接受异步连接
            server_socket.BeginAccept(new AsyncCallback(Connect_Callback), s_obj);
        }
        
        public static void Connect_Callback(IAsyncResult ar)
        {
            MainObject listener = (MainObject)ar.AsyncState;
            //返回与客户端通信的套接字
            Socket client_socket = listener.work_socket.EndAccept(ar);
            try
            {
                listener.work_socket.BeginAccept(new AsyncCallback(Connect_Callback), listener);//继续监听其余连接
            }
            catch(Exception ex)
            {
                string messgae = ex.Message;
            }
            Connect_Receive(client_socket, listener.user_name,listener.main_window);//监听客户端的信息
        }
        #region 接受消息
        //异步接收消息
        public static void Connect_Receive(Socket client_socket, string user_name, Main_Window main_window)
        {
            MainObject s_obj = new MainObject();
            s_obj.work_socket = client_socket;
            s_obj.user_name = user_name;
            s_obj.main_window = main_window;
            try
            {
                client_socket.BeginReceive(recv_bytes, 0, recv_bytes.Length, SocketFlags.None, new AsyncCallback(Receive_Callback),s_obj);
            }
            catch (Exception ex)
            {
                Console.WriteLine("异常信息：", ex.Message);
            }
        }
        //回调函数
        public static void Receive_Callback(IAsyncResult ar)
        {
            MainObject s_obj = (MainObject)ar.AsyncState;
            int lengths = s_obj.work_socket.EndReceive(ar);
            string recv_str = null;
            recv_str = Encoding.UTF8.GetString(recv_bytes, 0, lengths);
            s_obj.main_window.add_unread_record(recv_str, s_obj.work_socket);
        }
        //聊天消息接受
        public static void Chat_Receive(Socket client_socket, Chat_Window chat_window, bool chat_state)
        {
            ChatObject ss = new ChatObject();
            ss.client_socket = client_socket;
            ss.chat_window = chat_window;
            ss.chat_state = chat_state;
            try
            {
                //开始接收消息
                if(chat_state)
                    client_socket.BeginReceive(recv_bytes, 0, recv_bytes.Length, SocketFlags.None, new AsyncCallback(Chat_Receive_Callback), ss);
            }
            catch (Exception ex)
            {
                Console.WriteLine("异常信息：", ex.Message);
            }
        }
        public static void Chat_Receive_Callback(IAsyncResult ar)
        {
            ChatObject temp_ss = (ChatObject) ar.AsyncState;
            //Socket temp_ss = (Socket)ar.AsyncState;
            if (temp_ss.client_socket.Connected)
            {
                int lengths = temp_ss.client_socket.EndReceive(ar);
                string recv_str = null;
                recv_str = Encoding.UTF8.GetString(recv_bytes, 0, lengths);
                if (recv_str != "")
                {
                    if (recv_str.Split('.')[0] == "onchat")
                    {
                        string send_message = "onchat." + temp_ss.chat_window.user_name[0];//回应打招呼
                        int index = temp_ss.chat_window.user_name.IndexOf(recv_str.Split('.')[1]);
                        Connect_Send(temp_ss.chat_window.socket_to_friend[index - 1], send_message);
                        Chat_Receive(temp_ss.client_socket, temp_ss.chat_window, true);
                    }
                    if (recv_str.Split('.')[0] == "bye")
                    {
                        int index = temp_ss.chat_window.user_name.IndexOf(recv_str.Split('.')[1]);
                        temp_ss.chat_window.socket_to_friend[index - 1].Shutdown(SocketShutdown.Both);
                        temp_ss.chat_window.socket_to_friend[index - 1].Close();
                    }
                    else if (recv_str.Split('.')[0] == "file")
                    {
                        temp_ss.chat_window.add_info(recv_str);
                        Chat_Receive(temp_ss.client_socket, temp_ss.chat_window, false);
                    }
                    else if (recv_str.Split('.')[0] == "fileaccept" || recv_str.Split('.')[0] == "voiceend" 
                        || recv_str.Split('.')[0] == "voice" || recv_str.Split('.')[0] == "voiceaccept"|| recv_str.Split('.')[0] == "info")
                    {
                        temp_ss.chat_window.add_info(recv_str);
                        Chat_Receive(temp_ss.client_socket, temp_ss.chat_window,true);
                    }
                }
                Console.WriteLine("hhh" + recv_str);

            }
        }

        //未读消息接受
        public static void Unread_Chat_Receive(unread_Object unread_obj, int idx)
        {
            try
            {
                //开始接收消息
                if (unread_obj.flag)
                {
                    unread_obj.unread_socket.BeginReceive(recv_bytes, 0, recv_bytes.Length, SocketFlags.None, new AsyncCallback(Unread_Chat_Receive_Callback), unread_obj);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("异常信息：", ex.Message);
            }
        }
        //回调函数
        public static void Unread_Chat_Receive_Callback(IAsyncResult ar)
        {
            unread_Object unread_obj = (unread_Object)ar.AsyncState;
            if (unread_obj.flag)
            {
                int lengths = unread_obj.unread_socket.EndReceive(ar);
                string recv_str = null;
                recv_str = Encoding.UTF8.GetString(recv_bytes, 0, lengths);
                if (recv_str != null && recv_str.Split('.')[0] == "info")
                {
                    unread_obj.unread_msg.Add(recv_str);
                    unread_obj.msg_count++;
                    if(unread_obj.msg_count == 1)
                    {
                        unread_obj.main_Window.add_to_list(recv_str);
                    }
                }
                unread_obj.unread_socket.BeginReceive(recv_bytes, 0, recv_bytes.Length, SocketFlags.None, new AsyncCallback(Unread_Chat_Receive_Callback), unread_obj);
            }

        }
        #endregion

        #region 发送消息
        //异步发送消息
        public static void Connect_Send(Socket client_socket, string send_mess)
        {
            byte[] send_bytes = Encoding.UTF8.GetBytes(send_mess);
            try
            {
                //if(client_socket.Connected == true)
                client_socket.BeginSend(send_bytes, 0, send_bytes.Length, SocketFlags.None, new AsyncCallback(Send_Callback), client_socket);
                Console.WriteLine(send_mess);
            }
            catch (Exception ex)
            {
                Console.WriteLine("异常信息：{0}", ex.Message);
            }
        }
        //发送消息回调函数
        public static void Send_Callback(IAsyncResult ar)
        {
            Socket temp_socket = (Socket)ar.AsyncState;
            int lengths = temp_socket.EndSend(ar);
        }
        #endregion

        #region 文件操作
        public static bool File_Send(Socket client_socket, string filepath)
        {
            try
            {
                client_socket.SendFile(filepath);
            }
            catch
            {
                return false;
            }
            return true;
        }

        //接受文件
        public static void File_Receive(Socket client_socket, Chat_Window chat_window, string recv_filename)
        {
            SaveFileDialog file_dlg = new SaveFileDialog();
            file_dlg.InitialDirectory = chat_window.datapath;
            file_dlg.FileName = recv_filename;
            file_dlg.Filter = "(*." + recv_filename.Split('.')[recv_filename.Split('.').Length - 1] +  ")|*." + recv_filename.Split('.')[recv_filename.Split('.').Length - 1];
            if (file_dlg.ShowDialog() == DialogResult.OK)
            {
                string recv_filepath = file_dlg.FileName;
                FileStream source_stream = new FileStream(recv_filepath, FileMode.Create, FileAccess.Write);
                byte[] file_bytes = new byte[1024];
                int packege_length = file_bytes.Length;
                int total_length = 0;
                string send_msg = "fileaccept." + chat_window.user_name[0];
                Connect_Send(chat_window.socket_to_friend[chat_window.file_requestuser], send_msg);
                while (packege_length == 1024)
                {
                    packege_length = client_socket.Receive(file_bytes, 0, 1024, SocketFlags.None);
                    source_stream.Write(file_bytes, 0, packege_length);//写入文件
                    source_stream.Flush();
                    total_length = packege_length + total_length;
                    Console.WriteLine("{0:G}", total_length);
                }
                source_stream.Close();

            }
            MessageBox.Show(chat_window, "接受文件成功", "信息提示",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            string file_confim = "info." + chat_window.user_name[0] + " ." + DateTime.Now.ToString() + "\n" + "File receive successfully\n";
            Connect_Send(client_socket, file_confim);
            Chat_Receive(client_socket, chat_window,true);
        }
        #endregion

        #region 语音操作
        public static void Voice_Receive(Socket work_socket,WaveOut recv_stream, BufferedWaveProvider provider, Chat_Window chat_Window)
        {
            try
            {
                VoiceObject v_obj = new VoiceObject();
                v_obj.work_socket = work_socket;
                v_obj.recieved_stream = recv_stream;
                v_obj.wave_provider = provider;
                v_obj.chat_Window = chat_Window;
                work_socket.BeginReceive(voice_buffer, 0, 5242880, SocketFlags.None,
                    Voice_Receive_Callback, v_obj);
            }
            catch (Exception ex)
            {
                Console.WriteLine("异常信息：", ex.Message);
            }
        }

        public static void Voice_Receive_Callback(IAsyncResult ar)
        {
            VoiceObject v_obj = (VoiceObject)ar.AsyncState;
            try
            {
                var bytesRead = v_obj.work_socket.EndReceive(ar);
                string recv_str = null;
                recv_str = Encoding.UTF8.GetString(recv_bytes, 0, bytesRead);
                if (recv_str.Split('.')[0] != "voiceend")
                {
                    v_obj.wave_provider.AddSamples(voice_buffer, 0, bytesRead);
                    v_obj.recieved_stream.Play();
                    v_obj.work_socket.BeginReceive(voice_buffer, 0, 5242880, SocketFlags.None,
                        Voice_Receive_Callback, v_obj);
                }
                else
                    v_obj.chat_Window.end_voice_chat();
            }
            catch (Exception ex)
            {
                Console.WriteLine("异常信息：", ex.Message);
            }
        }

        public static void Voice_Send(byte[] buf, int bytesRecorded, WaveInEvent source_stream, bool state, Socket voice_socket)
        {
            try
            {
                if (!state)
                {
                    source_stream.StopRecording();
                    return;
                }
                else
                    voice_socket.BeginSend(buf, 0, bytesRecorded, SocketFlags.None, new AsyncCallback(Send_Callback), voice_socket);
                //Console.WriteLine(send_mess);
            }
            catch (Exception)
            {
                source_stream.StopRecording();
            }
        }

        #endregion

        //主动与朋友发起聊天 创建套接字
        public static Socket Commun_Friend(string friend_name, Socket socket_to_server)
        {
            Socket socket_to_friend = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string friend_ip = Server_Connection.Search_Friend(friend_name, socket_to_server);
            if(friend_ip != "Incorrect login No." && friend_ip != "Please send the correct message.")
            {
                IPEndPoint endPoint;
                int target_port;
                target_port = Convert.ToInt32(friend_name.Substring(6)) + 50000;
                endPoint = new IPEndPoint(IPAddress.Parse(friend_ip), target_port);
                //socket_to_friend = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //socket_to_friends[i] = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket_to_friend.Connect(endPoint);
            }
            return socket_to_friend;

        }
        //得到本机IP地址
        public static IPAddress Get_Local_IP()
        {
            IPAddress local_ip = null;
            IPAddress[] ip_list = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress ipAddress in ip_list)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    local_ip = ipAddress;
                }
            }
            return local_ip;
        }
    }
}
