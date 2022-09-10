using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsgoBot
{
    class MainWindow
    {
        public static Form1 Form1 { get; set; }
        public MainWindow(Form1 form1)
        {
            Form1 = form1;
        }

        Thread thread = new Thread(new ThreadStart(Form1.MainMethod));
        //thread.IsBackground = true;
        //thread.Name = "Data Polling Thread";
        //thread.S Start();
        //Application.Run();

        //Thread th = new Thread(() =>
        //{
        //   while (true)
        //   {
        //       //Form1.MainMethod();
        //   }
        //}).Start();

        
    }
}






