using Homework3.Forms;
using Homework3.Models;

namespace Homework3
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            using (var context = new AppDbContext())
            {
                DataSeeder.Initialize(context);
            }

            Application.Run(new MainForm());
        }
    }
}
