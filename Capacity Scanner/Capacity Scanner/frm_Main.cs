using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_Data
{
    public partial class frm_Main : Form
    {
        public cls.Ini ini = new cls.Ini(Application.StartupPath + "\\" + Application.ProductName + ".ini");

        public frm_Main()
        {
            InitializeComponent();

            //Application.OpenForms["frm_Main"].Focus();
        }

        private void Frm_Main_Load(object sender, EventArgs e)
        {
            this.BringToFront();
            this.Activate();

            init();
        }

        public void init()
        {
            Fnc_Load_Activate();
            Fnc_Load_Control();
        }

        public void Fnc_Load_Control()
        {
            //cls.showUC(new Ctrl.uc_CapacityScanner_v2o1(), panel1);
            //cls.showUC(new Ctrl.uc_CapacityScanner_v2o2(), panel1);
            //cls.showUC(new Ctrl.uc_CapacityScanner_v2o3(), panel1);
            //cls.showUC(new Ctrl.uc_CapacityScanner_v2o4(), panel1);     // Using
            //cls.showUC(new Ctrl.uc_CapacityScanner_v2o4o1(), panel1);
            cls.showUC(new Ctrl.uc_CapacityScanner_v2o4o2(), panel1);       // Active barcode scanning to PLC signal directly function
            //cls.showUC(new Ctrl.uc_CapacityScanner_v2o5(), panel1);
            //cls.showUC(new Ctrl.uc_CapacityScanner_v2o6(), panel1);
            //cls.showUC(new Ctrl.uc_CapacityScanner_v3o0(), panel1);
            //cls.showUC(new Ctrl.uc_CapacityScanner_v3o1(), panel1);
        }

        public void Fnc_Load_Activate()
        {
            Thread loadActive = new Thread(() =>
            {
                while (true)
                {
                    this.BringToFront();
                    this.Activate();
                    //this.SetTopLevel(true);

                    Thread.Sleep(1000);
                }
            });
            loadActive.IsBackground = true;
            loadActive.Start();
        }

        private void frm_Main_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    if (this.FormBorderStyle == FormBorderStyle.None)
                    {
                        this.FormBorderStyle = FormBorderStyle.FixedSingle;
                    }
                    else
                    {
                        //Application.ExitThread();
                        //Application.Exit();
                    }
                    break;
                case Keys.F:
                    if (e.Modifiers == Keys.Control)
                    {
                        this.FormBorderStyle = FormBorderStyle.None;
                    }
                    break;
                //case Keys.F1:
                //    ini.SetIniValue("MACHINE", "NO", "1");
                //    ini.SetIniValue("MACHINE", "ID", "4");

                //    Application.Restart();
                //    break;
                //case Keys.F2:
                //    ini.SetIniValue("MACHINE", "NO", "2");
                //    ini.SetIniValue("MACHINE", "ID", "5");

                //    Application.Restart();
                //    break;
            }
        }
    }
}
