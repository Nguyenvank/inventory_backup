using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Inventory_Data
{
    public partial class frmTestGetTempT3 : Form
    {
        public int _start = 0;
        public int _count = 0;
        //Label lbl;
        public string _prevValue = "", _currValue = "";

        public static DateTime _dt;
        public DateTime _dtDay;
        public DateTime _dtNight;
        public int cmpDay, cmpNight;


        public frmTestGetTempT3()
        {
            InitializeComponent();

    }

        private void frmTestGetTempT3_Load(object sender, EventArgs e)
        {
            _prevValue = "0000";
            _currValue = "0000";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _dt = DateTime.Now;

            fnReset();

            if (_start == 1)
            {
                fnLoad_Data();
            }
            else
            {

            }
        }

        public void init()
        {

        }

        public void fnReset()
        {
            DateTime dt = new DateTime(_dt.Year, _dt.Month, _dt.Day, _dt.Hour, _dt.Minute, 0);
            DateTime dtDay = new DateTime(_dt.Year, _dt.Month, _dt.Day, 8, 0, 0);
            DateTime dtNight = new DateTime(_dt.Year, _dt.Month, _dt.Day, 20, 0, 0);

            if (dt.TimeOfDay == dtDay.TimeOfDay || dt.TimeOfDay == dtNight.TimeOfDay)
            {
                string count = _count.ToString();
                fnSaveList("Injection 06-" + count + ".txt");
                _count = 0;
                lblCount.Text = "0";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _start = 1;
            int _mcNum = 0;
            string lblMachine = "lbl";
            if (comboBox1.SelectedIndex > 0 && comboBox2.SelectedIndex > 0)
            {
                lblMachine += (comboBox1.SelectedIndex == 1) ? "Rub" : "Inj";
                _mcNum = Convert.ToInt32(comboBox2.Text);
                lblMachine += (_mcNum < 10) ? "0" + comboBox2.Text : comboBox2.Text;

                //lbl = (Label)this.GetControlByName(lblMachine);
                //lbl.TextChanged += new System.EventHandler(this.lbl_TextChanged);

            }
            else
            {
                lblCount.Text = "0";
            }
        }

        private void lbl_TextChanged(object sender, EventArgs e)
        {
            //string value = lbl.Text;
            //int _value = Convert.ToInt32(value);

            //if (value == "0001")
            //{
            //    _count = _count + 1;
            //}
            //lblCount.Text = _count.ToString();
        }


        public void fnLoad_Data()
        {
            string sql = "VNUSER_CHECK_TEMPT3";

            DataSet ds = new DataSet();
            ds = cls.ExecuteDataSet(sql, CommandType.StoredProcedure, "connINJREC_VM");

            string temp = "";
            string temp1 = "";
            string sysdate = "";

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    temp = ds.Tables[0].Rows[0][0].ToString();
                    temp1 = ds.Tables[0].Rows[0][1].ToString();
                    sysdate = ds.Tables[0].Rows[0][2].ToString();

                    label4.Text = sysdate;
                    textBox1.Text = temp1;
                    lblTemp1.Text = temp1;
                }
            }
            else
            {
                temp = "";
                temp1 = "";
                sysdate = "";
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string signal01 = "", signal02 = "", signal03 = "", signal04 = "", signal05 = "", signal06 = "", signal07 = "", signal08 = "", signal09 = "", signal10 = "";
            string signal11 = "", signal12 = "", signal13 = "", signal14 = "", signal15 = "", signal16 = "", signal17 = "", signal18 = "", signal19 = "", signal20 = "";
            string signal21 = "", signal22 = "", signal23 = "", signal24 = "", signal25 = "", signal26 = "", signal27 = "", signal28 = "", signal29 = "", signal30 = "";
            string signal31 = "", signal32 = "", signal33 = "", signal34 = "", signal35 = "", signal36 = "", signal37 = "", signal38 = "", signal39 = "", signal40 = "";
            string temp1 = textBox1.Text.Trim();
            string sysdate = label4.Text;

            signal01 = temp1.Substring(0, 4); signal02 = temp1.Substring(4, 4); signal03 = temp1.Substring(8, 4); signal04 = temp1.Substring(12, 4); signal05 = temp1.Substring(16, 4);
            signal06 = temp1.Substring(20, 4); signal07 = temp1.Substring(24, 4); signal08 = temp1.Substring(28, 4); signal09 = temp1.Substring(32, 4); signal10 = temp1.Substring(36, 4);
            signal11 = temp1.Substring(40, 4); signal12 = temp1.Substring(44, 4); signal13 = temp1.Substring(48, 4); signal14 = temp1.Substring(52, 4); signal15 = temp1.Substring(56, 4);
            signal16 = temp1.Substring(60, 4); signal17 = temp1.Substring(64, 4); signal18 = temp1.Substring(68, 4); signal19 = temp1.Substring(72, 4); signal20 = temp1.Substring(76, 4);
            signal21 = temp1.Substring(80, 4); signal22 = temp1.Substring(84, 4); signal23 = temp1.Substring(88, 4); signal24 = temp1.Substring(92, 4); signal25 = temp1.Substring(96, 4);
            signal26 = temp1.Substring(100, 4); signal27 = temp1.Substring(104, 4); signal28 = temp1.Substring(108, 4); signal29 = temp1.Substring(112, 4); signal30 = temp1.Substring(116, 4);
            signal31 = temp1.Substring(120, 4); signal32 = temp1.Substring(124, 4); signal33 = temp1.Substring(128, 4); signal34 = temp1.Substring(132, 4); signal35 = temp1.Substring(136, 4);
            signal36 = temp1.Substring(140, 4); signal37 = temp1.Substring(144, 4); signal38 = temp1.Substring(148, 4); signal39 = temp1.Substring(152, 4); signal40 = temp1.Substring(156, 4);

            lblRub01.Text = signal01; lblRub02.Text = signal02; lblRub03.Text = signal03; lblRub04.Text = signal04; lblRub05.Text = signal05;
            lblRub06.Text = signal06; lblRub07.Text = signal07; lblRub08.Text = signal08; lblRub09.Text = signal09; lblRub10.Text = signal10;
            lblRub11.Text = signal11; lblRub12.Text = signal12; lblRub13.Text = signal13; lblRub14.Text = signal14; lblRub15.Text = signal15;
            lblRub16.Text = signal16; lblRub16.Text = signal17; lblRub16.Text = signal18; lblRub16.Text = signal19; lblRub16.Text = signal20;

            lblInj01.Text = signal21; lblInj02.Text = signal22; lblInj03.Text = signal23; lblInj04.Text = signal24; lblInj05.Text = signal25;
            lblInj06.Text = signal26; lblInj07.Text = signal27; lblInj08.Text = signal28; lblInj09.Text = signal29; lblInj10.Text = signal30;
            lblInj11.Text = signal31; lblInj12.Text = signal32;

            listBox1.Items.Add("[" + sysdate + "]     " + temp1);
            listBox1.SelectedIndex = listBox1.Items.Count - 1;

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > 0)
            {
                comboBox2.Enabled = true;
                comboBox2.Items.Clear();
                switch (comboBox1.SelectedIndex)
                {
                    case 1:
                        for (int i = 1; i <= 20; i++)
                        {
                            comboBox2.Items.Add(i.ToString());
                        }
                        break;
                    case 2:
                        for (int j = 1; j <= 12; j++)
                        {
                            comboBox2.Items.Add(j.ToString());
                        }
                        break;
                }
                comboBox2.Items.Insert(0, "");
                comboBox2.SelectedIndex = 0;

            }
            else
            {
                comboBox2.Items.Clear();
                comboBox2.Enabled = false;
            }
        }

        private void lblInj06_TextChanged(object sender, EventArgs e)
        {
            
            string value = lblInj06.Text;
            lblInj06.BackColor = (value == "0001") ? Color.LightSalmon : Color.FromKnownColor(KnownColor.Control);
            _currValue = value;

            if (_prevValue == "0000" && _currValue == "0001")
            {
                _count = _count + 1;
                _prevValue = "0000";
                _currValue = "0000";
                //_prevValue = _currValue;
                //_currValue = "";
            }
            lblCount.Text = _count.ToString();
        }

        private void lblRub01_TextChanged(object sender, EventArgs e)
        {
            string value = lblRub01.Text;
            lblRub01.BackColor = (value == "0001") ? Color.LightGreen : Color.FromKnownColor(KnownColor.Control);
        }

        private void lblRub02_TextChanged(object sender, EventArgs e)
        {
            string value = lblRub02.Text;
            lblRub02.BackColor = (value == "0001") ? Color.LightGreen : Color.FromKnownColor(KnownColor.Control);
        }

        private void lblRub03_TextChanged(object sender, EventArgs e)
        {
            string value = lblRub03.Text;
            lblRub03.BackColor = (value == "0001") ? Color.LightGreen : Color.FromKnownColor(KnownColor.Control);
        }

        private void lblRub04_TextChanged(object sender, EventArgs e)
        {
            string value = lblRub04.Text;
            lblRub04.BackColor = (value == "0001") ? Color.LightGreen : Color.FromKnownColor(KnownColor.Control);
        }

        private void lblRub05_TextChanged(object sender, EventArgs e)
        {
            string value = lblRub05.Text;
            lblRub05.BackColor = (value == "0001") ? Color.LightGreen : Color.FromKnownColor(KnownColor.Control);
        }

        private void lblRub06_TextChanged(object sender, EventArgs e)
        {
            string value = lblRub06.Text;
            lblRub06.BackColor = (value == "0001") ? Color.LightGreen : Color.FromKnownColor(KnownColor.Control);
        }

        private void lblRub07_TextChanged(object sender, EventArgs e)
        {
            string value = lblRub07.Text;
            lblRub07.BackColor = (value == "0001") ? Color.LightGreen : Color.FromKnownColor(KnownColor.Control);
        }

        private void lblRub08_TextChanged(object sender, EventArgs e)
        {
            string value = lblRub08.Text;
            lblRub08.BackColor = (value == "0001") ? Color.LightGreen : Color.FromKnownColor(KnownColor.Control);
        }

        private void lblRub09_TextChanged(object sender, EventArgs e)
        {
            string value = lblRub09.Text;
            lblRub09.BackColor = (value == "0001") ? Color.LightGreen : Color.FromKnownColor(KnownColor.Control);
        }

        private void lblRub10_TextChanged(object sender, EventArgs e)
        {
            string value = lblRub10.Text;
            lblRub10.BackColor = (value == "0001") ? Color.LightGreen : Color.FromKnownColor(KnownColor.Control);
        }

        private void lblRub11_TextChanged(object sender, EventArgs e)
        {
            string value = lblRub11.Text;
            lblRub11.BackColor = (value == "0001") ? Color.LightGreen : Color.FromKnownColor(KnownColor.Control);
        }

        private void lblRub12_TextChanged(object sender, EventArgs e)
        {
            string value = lblRub12.Text;
            lblRub12.BackColor = (value == "0001") ? Color.LightGreen : Color.FromKnownColor(KnownColor.Control);
        }

        private void lblRub13_TextChanged(object sender, EventArgs e)
        {
            string value = lblRub13.Text;
            lblRub13.BackColor = (value == "0001") ? Color.LightGreen : Color.FromKnownColor(KnownColor.Control);
        }

        private void lblRub14_TextChanged(object sender, EventArgs e)
        {
            string value = lblRub14.Text;
            lblRub14.BackColor = (value == "0001") ? Color.LightGreen : Color.FromKnownColor(KnownColor.Control);
        }

        private void lblRub15_TextChanged(object sender, EventArgs e)
        {
            string value = lblRub15.Text;
            lblRub15.BackColor = (value == "0001") ? Color.LightGreen : Color.FromKnownColor(KnownColor.Control);
        }

        private void lblRub16_TextChanged(object sender, EventArgs e)
        {
            string value = lblRub16.Text;
            lblRub16.BackColor = (value == "0001") ? Color.LightGreen : Color.FromKnownColor(KnownColor.Control);
        }

        private void lblRub17_TextChanged(object sender, EventArgs e)
        {
            string value = lblRub17.Text;
            lblRub17.BackColor = (value == "0001") ? Color.LightGreen : Color.FromKnownColor(KnownColor.Control);
        }

        private void lblRub18_TextChanged(object sender, EventArgs e)
        {
            string value = lblRub18.Text;
            lblRub18.BackColor = (value == "0001") ? Color.LightGreen : Color.FromKnownColor(KnownColor.Control);
        }

        private void lblRub19_TextChanged(object sender, EventArgs e)
        {
            string value = lblRub19.Text;
            lblRub19.BackColor = (value == "0001") ? Color.LightGreen : Color.FromKnownColor(KnownColor.Control);
        }

        private void lblRub20_TextChanged(object sender, EventArgs e)
        {
            string value = lblRub20.Text;
            lblRub20.BackColor = (value == "0001") ? Color.LightGreen : Color.FromKnownColor(KnownColor.Control);
        }


        private void lblInj01_TextChanged(object sender, EventArgs e)
        {
            string value = lblInj01.Text;
            lblInj01.BackColor = (value == "0001") ? Color.LightGreen : Color.FromKnownColor(KnownColor.Control);
        }

        private void lblInj02_TextChanged(object sender, EventArgs e)
        {
            string value = lblInj02.Text;
            lblInj02.BackColor = (value == "0001") ? Color.LightGreen : Color.FromKnownColor(KnownColor.Control);
        }

        private void lblInj03_TextChanged(object sender, EventArgs e)
        {
            string value = lblInj03.Text;
            lblInj03.BackColor = (value == "0001") ? Color.LightGreen : Color.FromKnownColor(KnownColor.Control);
        }

        private void lblInj04_TextChanged(object sender, EventArgs e)
        {
            string value = lblInj04.Text;
            lblInj04.BackColor = (value == "0001") ? Color.LightGreen : Color.FromKnownColor(KnownColor.Control);
        }

        private void lblInj05_TextChanged(object sender, EventArgs e)
        {
            string value = lblInj05.Text;
            lblInj05.BackColor = (value == "0001") ? Color.LightGreen : Color.FromKnownColor(KnownColor.Control);
        }

        private void lblInj07_TextChanged(object sender, EventArgs e)
        {
            string value = lblInj07.Text;
            lblInj07.BackColor = (value == "0001") ? Color.LightGreen : Color.FromKnownColor(KnownColor.Control);
        }

        private void lblInj08_TextChanged(object sender, EventArgs e)
        {
            string value = lblInj08.Text;
            lblInj08.BackColor = (value == "0001") ? Color.LightGreen : Color.FromKnownColor(KnownColor.Control);
        }

        private void lblInj09_TextChanged(object sender, EventArgs e)
        {
            string value = lblInj09.Text;
            lblInj09.BackColor = (value == "0001") ? Color.LightGreen : Color.FromKnownColor(KnownColor.Control);
        }

        private void lblInj10_TextChanged(object sender, EventArgs e)
        {
            string value = lblInj10.Text;
            lblInj10.BackColor = (value == "0001") ? Color.LightGreen : Color.FromKnownColor(KnownColor.Control);
        }

        private void lblInj11_TextChanged(object sender, EventArgs e)
        {
            string value = lblInj11.Text;
            lblInj11.BackColor = (value == "0001") ? Color.LightGreen : Color.FromKnownColor(KnownColor.Control);
        }

        private void lblInj12_TextChanged(object sender, EventArgs e)
        {
            string value = lblInj12.Text;
            lblInj12.BackColor = (value == "0001") ? Color.LightGreen : Color.FromKnownColor(KnownColor.Control);
        }

        private void saveToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string count = _count.ToString();
            fnSaveList("Injection 06-" + count + ".txt");
        }

        public void fnSaveList(string filename)
        {
            using (StreamWriter sr = File.CreateText(filename))
            {
                foreach (string s in listBox1.Items)
                {
                    sr.WriteLine(s);
                }
            }
        }

        public Control GetControlByName(string Name)
        {
            foreach (Control c in this.Controls)
                if (c.Name == Name)
                    return c;

            return null;
        }
    }
}
