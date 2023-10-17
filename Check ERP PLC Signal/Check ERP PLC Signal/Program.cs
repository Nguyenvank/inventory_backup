using Inventory_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Check_ERP_PLC_Signal
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
            //Application.Run(new frmTestGetTempT3());
            Application.Run(new frm_Main());
        }
    }
}
