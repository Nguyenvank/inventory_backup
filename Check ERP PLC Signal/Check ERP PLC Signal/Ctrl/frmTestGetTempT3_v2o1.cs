using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Inventory_Data;

namespace Check_ERP_PLC_Signal.Ctrl
{
    public partial class frmTestGetTempT3_v2o1 : UserControl
    {

        string _tempData = "";
        int _maxSignal = 40;

        public frmTestGetTempT3_v2o1()
        {
            InitializeComponent();
        }

        private void frmTestGetTempT3_v2o1_Load(object sender, EventArgs e)
        {
            Fnc_Load_Init();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            cls.fnSetDateTime(lbl_Date);
            lbl_Date.Text = lbl_Date.Text.Replace(" - ", "\r\n").Replace("/", " / ").Replace(":", " : ");

            if (chk_Connect.Checked)
            {
                Fnc_Load_Connection();
            }
            else
            {
                Fnc_Load_Controls();
            }
        }

        public void Fnc_Load_Init()
        {
            cls.fnSetDateTime(lbl_Date);
            lbl_Date.Text = lbl_Date.Text.Replace(" - ", "\r\n").Replace("/", " / ").Replace(":", " : ");

            Fnc_Load_Controls();
        }

        /****************************************/

        public void Fnc_Load_Controls()
        {
            chk_Connect.Checked = false;
            lbl_Signal_Temp3.Text = "";
            //lbl_Signal_01.Text = lbl_Signal_02.Text = lbl_Signal_03.Text = lbl_Signal_04.Text = lbl_Signal_05.Text =
            //lbl_Signal_06.Text = lbl_Signal_07.Text = lbl_Signal_08.Text = lbl_Signal_09.Text = lbl_Signal_10.Text =
            //lbl_Signal_11.Text = lbl_Signal_12.Text = lbl_Signal_13.Text = lbl_Signal_14.Text = lbl_Signal_15.Text =
            //lbl_Signal_16.Text = lbl_Signal_17.Text = lbl_Signal_18.Text = lbl_Signal_19.Text = lbl_Signal_20.Text =
            //lbl_Signal_21.Text = lbl_Signal_22.Text = lbl_Signal_23.Text = lbl_Signal_24.Text = lbl_Signal_25.Text =
            //lbl_Signal_26.Text = lbl_Signal_27.Text = lbl_Signal_28.Text = lbl_Signal_29.Text = lbl_Signal_30.Text =
            //lbl_Signal_31.Text = lbl_Signal_32.Text = lbl_Signal_33.Text = lbl_Signal_34.Text = lbl_Signal_35.Text =
            //lbl_Signal_36.Text = lbl_Signal_37.Text = lbl_Signal_38.Text = lbl_Signal_39.Text = lbl_Signal_40.Text = "";

            for (int i = 1; i <= _maxSignal; i++)
            {
                Label lbl_Signal = (Label)cls.FindControlRecursive(pnl_0, "lbl_Signal_" + String.Format("{0:00}", i));
                lbl_Signal.Font = new Font("Times New Roman", 12, FontStyle.Bold);
                lbl_Signal.BackColor = Color.FromKnownColor(KnownColor.Control);

                lbl_Signal.Text = "";
            }
        }

        public void Fnc_Load_Connection()
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

                    _tempData = temp1;

                    //label4.Text = sysdate;
                    //textBox1.Text = temp1;
                    //lblTemp1.Text = temp1;

                    if (_tempData.Length > 0)
                    {
                        Fnc_Split_Signal();
                    }
                }
            }
            else
            {
                _tempData = "";

                //temp = "";
                //temp1 = "";
                //sysdate = "";
            }
        }

        public void Fnc_Split_Signal()
        {
            string temp = _tempData.Trim(), temp_Signal = "", temp_Split_String = "";
            int temp_Length = temp.Trim().Length, temp_Start = 0, temp_Step = 4, temp_Start_Split = 0, temp_Split = 10;

            //lbl_Signal_Temp3.Text = temp;


            //lbl_Signal_Temp3.Text = Fnc_String_Space_Characters(temp, 4);
            //lbl_Signal_Temp3.Text = Fnc_String_Break_Line(Fnc_String_Space_Characters(temp,4), 100);
            lbl_Signal_Temp3.Text = Fnc_String_Break_Line(Fnc_String_Space_Characters(temp, 4), 50);

            for (int i = 1; i <= _maxSignal; i++)
            {
                Label lbl_Signal = (Label)cls.FindControlRecursive(pnl_0, "lbl_Signal_" + String.Format("{0:00}", i));
                lbl_Signal.Font = new Font("Times New Roman", 12, FontStyle.Bold);

                lbl_Signal.Text = temp.Substring(temp_Start, temp_Step);
                lbl_Signal.BackColor = (lbl_Signal.Text == "0001") ? Color.LightGreen : Color.FromKnownColor(KnownColor.Control);

                temp_Start = temp_Start + temp_Step;

            }



            //while (temp_Start <= temp_Lenth)
            //{
            //    for (int i = 1; i <= _maxSignal; i++)
            //    {
            //        Label lbl_Signal = (Label)cls.FindControlRecursive(pnl_0, "lbl_Signal_" + String.Format("{0:00}", i));
            //        lbl_Signal.Font = new Font("Times New Roman", 15, FontStyle.Bold);

            //        lbl_Signal.Text = temp.Substring(temp_Start, temp_Step);
            //    }

            //    temp_Start = temp_Start + temp_Step;
            //}

            //for (int i = temp_Start; i < temp_Lenth; i++)
            //{
            //    temp_Signal = cls.Mid(temp, temp_Start, temp_Step);
            //}
        }

        public string Fnc_String_Break_Line(string str, int length)
        {
            string input = str;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                if (i % (length) == 0)
                    sb.Append("\r\n");
                sb.Append(input[i]);
            }
            string formatted = cls.Right(sb.ToString(),sb.Length-2);
            return formatted;
        }

        public string Fnc_String_Space_Characters(string str, int length)
        {
            string input = str;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                if (i % length == 0)
                    sb.Append(' ');
                sb.Append(input[i]);
            }
            string formatted = sb.ToString();
            return formatted;
        }

        public string Fnc_Splice_Text(string text, int lineLength)
        {
            var charCount = 0;
            var lines = text.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)
                            .GroupBy(w => (charCount += w.Length + 1) / lineLength)
                            .Select(g => string.Join(" ", g));

            return String.Join("\r\n", lines.ToArray());
        }

        /****************************************/

        private void chk_Connect_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_Connect.Checked)
            {
                Fnc_Load_Connection();
            }
            else
            {
                Fnc_Load_Controls();
            }
        }
    }
}
