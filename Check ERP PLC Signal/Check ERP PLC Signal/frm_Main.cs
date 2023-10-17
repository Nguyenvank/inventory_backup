using Inventory_Data;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Check_ERP_PLC_Signal
{
    public partial class frm_Main : Form
    {
        int _loadView = 3;

        public frm_Main()
        {
            InitializeComponent();

            Fnc_Load_Menu_Context();
        }

        private void frm_Main_Load(object sender, EventArgs e)
        {
            Fnc_Load_View();
        }

        public void Fnc_Load_Menu_Context()
        {
            ContextMenu mnuPanel = new ContextMenu();
            mnuPanel.MenuItems.Add(new System.Windows.Forms.MenuItem("Load view 01", Fnc_Load_View_01_Click));
            mnuPanel.MenuItems.Add(new System.Windows.Forms.MenuItem("Load view 02", Fnc_Load_View_02_Click));
            mnuPanel.MenuItems.Add("-");
            mnuPanel.MenuItems.Add(new System.Windows.Forms.MenuItem("Exit application", Fnc_Form_Close_Click));
            panel1.ContextMenu = mnuPanel;
        }

        public void Fnc_Load_View()
        {
            int loadView = _loadView;
            panel1.Controls.Clear();

            switch (loadView)
            {
                case 1:
                    cls.showUC(new Ctrl.frmTestGetTempT3_v2o0(), panel1);
                    break;
                case 2:
                    cls.showUC(new Ctrl.frmTestGetTempT3_v2o1(), panel1);
                    break;
                case 3:
                    cls.showUC(new Ctrl.frmTestGetTempT3_v2o2(), panel1);
                    break;
                default:
                    cls.showUC(new Ctrl.frmTestGetTempT3_v2o0(), panel1);
                    break;
            }
        }

        private void Fnc_Load_View_01_Click(object sender, EventArgs e)
        {
            _loadView = 1;
            Fnc_Load_View();
        }

        private void Fnc_Load_View_02_Click(object sender, EventArgs e)
        {
            _loadView = 2;
            Fnc_Load_View();
        }

        private void Fnc_Form_Close_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Are you sure?", cls.appName(), MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            {
                System.Windows.Forms.Application.Exit();
            }
        }

    }
}
