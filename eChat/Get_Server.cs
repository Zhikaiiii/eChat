using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace Get_Server
{
    public class Server_Connection
    {
        public static string IP = "166.111.140.57";
        public static int port = 8000;
        //连接服务器
        public static Socket Connect_Server()
        {
            Socket serverSocket = null;
            Socket tempSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress server_ip = IPAddress.Parse(IP);
            IPEndPoint endPoint = new IPEndPoint(server_ip, port);
            try
            {
                tempSocket.Connect(endPoint);
                serverSocket = tempSocket;
            }
            catch
            {
                Console.WriteLine("Error");
                return serverSocket;
            }
            return serverSocket;
        }
        //上线
        public static string Loginto_Server(string username, Socket client_socket)
        {
            string send_message = username + "_net2019";
            try
            {
                client_socket.Send(Encoding.ASCII.GetBytes(send_message));
            }
            catch
            {
                string error_info = "error";
                return error_info;
            }
            string recv_str = "";
            byte[] recv_bytes = new byte[1024];
            int bytes;
            bytes = client_socket.Receive(recv_bytes, recv_bytes.Length, 0);//从客户端接受信息
            recv_str += Encoding.ASCII.GetString(recv_bytes, 0, bytes);
            return recv_str;
        }
        //查询朋友状态
        public static string Search_Friend(string username, Socket client_socket)
        {
            string send_message = "q" + username;
            client_socket.Send(Encoding.ASCII.GetBytes(send_message));
            string recv_str = "";
            byte[] recv_bytes = new byte[1024];
            int bytes;
            bytes = client_socket.Receive(recv_bytes, recv_bytes.Length, 0);//从客户端接受信息
            recv_str += Encoding.ASCII.GetString(recv_bytes, 0, bytes);
            return recv_str;
        }
        //下线
        public static void Log_Out(string username, Socket client_socket)
        {
            string send_message = "logout" + username;
            client_socket.Send(Encoding.ASCII.GetBytes(send_message));
            client_socket.Shutdown(SocketShutdown.Both);
            client_socket.Close();
        }
    }
}


























/*
private static void Listen(string ip, int port)
{
    IPAddress server_ip = IPAddress.Parse(ip);
    TcpListener tcp1 = new TcpListener(server_ip, port);
    tcp1.Start();
    while (true)
    {
        Socket s = tcp1.AcceptSocket();
        Byte[] stream = new Byte[80];
        int i = s.Receive(stream, stream.Length, 0);
        string message = Encoding.ASCII.GetString(stream, 0, i);
        Console.WriteLine(message);
    }
}
private static void Send(string ip, int port)
{
    //IPAddress server_ip = IPAddress.Parse(ip);
    TcpClient tcp_client = new TcpClient(ip, port);
    NetworkStream tcpStream = tcp_client.GetStream();
    StreamWriter reqStreamW = new StreamWriter(tcpStream);
    string send_message = Console.ReadLine();
    reqStreamW.Write(send_message);
    reqStreamW.Flush();
    tcpStream.Close();
    tcp_client.Close();

}

// This method requests the home page content for the specified server.
private static void ReceiveData(Socket client_socket)
{
    while (true)
    {
        string recv_str = "";
        byte[] recv_bytes = new byte[1024];
        int bytes;
        bytes = client_socket.Receive(recv_bytes, recv_bytes.Length, 0);//从客户端接受信息
        recv_str += Encoding.ASCII.GetString(recv_bytes, 0, bytes);
        Console.WriteLine(recv_str);
    }
}
/*
public static void Main(string[] args)
{
    string ip_self = "183.172.234.64";
    int port_self;
    port_self = Convert.ToInt32(Console.ReadLine());
    int port_friend;
    port_friend = Convert.ToInt32(Console.ReadLine());

    Thread threadListen = new Thread(() => Listen(ip_self, port_friend));
    threadListen.Start();
    Send(ip_self, port_self);
    int port = 8000;
    string ip = "166.111.140.57";

    Socket client_socket = null;
    client_socket = ConnectServer(ip, port);
    //string send_message = "2016011889_net2019";
    string send_message;
    Thread threadRead = new Thread(() => ReceiveData(client_socket));
    threadRead.Start();

    /*
    while (true)
    {
        send_message = Console.ReadLine();
        client_socket.Send(Encoding.ASCII.GetBytes(send_message));
    }
    /*
     *         
    if (ConnectSocket(ip, port))
    {
        Console.WriteLine("Yes");
    }
    else
    {
        Console.WriteLine("No");
    }
    */

