using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cookbook
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            // This Data Source string from Server Explorer Add Connection dialog box
            // "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\MyDox\C_sharp_projects\csharp-intermediate\WebScraperProjects\Cookbook\Cookbook\Cookbook.mdf;Integrated Security=True;Connect Timeout=30";
        }
    }
}
