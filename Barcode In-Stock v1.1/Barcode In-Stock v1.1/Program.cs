using Inventory_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Inventory_Data
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
            ////Application.Run(new frmFinishGoodScanIn_v1o1());        // Bellows + Gasket
            //Application.Run(new frmFinishGoodScanIn_v1o2());
            //Application.Run(new frmFinishGoodScanIn_v1o3());
            //Application.Run(new frmFinishGoodScanIn_v1o4());        // packStd status
            //Application.Run(new frmFinishGoodScanIn_v1o5());        // Change packing quantity

            //Application.Run(new frmFinishGoodScanOut_v1o0());   // version light
            //Application.Run(new frmFinishGoodScanOut_v1o1());   // version dark
            //Application.Run(new frmFinishGoodScanOut_v1o2());   // disabled FIFO
            Application.Run(new frmFinishGoodScanOut_v1o3());   // disabled temper chart
            //Application.Run(new frm_Heating_Room_Temperature_v1o0());
        }
    }
}
