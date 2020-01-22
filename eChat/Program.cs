using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using My_data;
namespace eChat
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            My_Database.Connect_Database();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //AllocConsole();
            Application.Run(new Login());
        }
    }
}
