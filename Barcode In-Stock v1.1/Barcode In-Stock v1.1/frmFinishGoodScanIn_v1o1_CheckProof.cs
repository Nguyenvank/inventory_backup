using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Inventory_Data;

namespace Inventory_Data
{
    public partial class frmFinishGoodScanIn_v1o1_CheckProof : Form
    {
        public string _boxCode = "", _partIDx = "", _partName = "", _partCode = "", _partLOT = "", _partDAY = "", _partLOC = "", _partQTY = "";
        public string _subCode01 = "", _subCode02 = "", _subCode03 = "", _subCodeNo = "";
        public int _seq = 1;
        public Boolean _status = false;

        public System.Windows.Forms.Timer tmrDelay, timerMsg;


        public frmFinishGoodScanIn_v1o1_CheckProof()
        {
            InitializeComponent();
        }

        private void frmFinishGoodScanIn_v1o1_CheckProof_Load(object sender, EventArgs e)
        {
            init();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            fnGetdate();
            txtSampleCode.Focus();
        }

        public void init()
        {
            fnGetdate();

            tmrDelay = new System.Windows.Forms.Timer();
            tmrDelay.Interval = 500;
            tmrDelay.Enabled = false;

            timerMsg = new System.Windows.Forms.Timer();
            timerMsg.Interval = 1500;
            timerMsg.Enabled = false;

            txtSampleCode.Focus();
        }

        public void fnGetdate()
        {

        }

        public void fnGetPart(string boxCode, string partIDx, string partName, string partCode, string partLOT, string partDAY, string partLOC, string partQTY)
        {
            //string msg = "";
            //msg += "boxCode: " + boxCode + "\r\n";
            //msg += "partIDx: " + partIDx + "\r\n";
            //msg += "partName: " + partName + "\r\n";
            //msg += "partCode: " + partCode + "\r\n";
            //msg += "partLOT: " + partLOT + "\r\n";
            //msg += "partDAY: " + partDAY + "\r\n";
            //msg += "partLOC: " + partLOC + "\r\n";
            //msg += "partQTY: " + partQTY + "\r\n";
            //MessageBox.Show(msg);

            int subCodeNo = 0;

            _boxCode = boxCode;
            _partIDx = partIDx;
            _partName = partName;
            _partCode = partCode;
            _partLOT = partLOT;
            _partDAY = partDAY;
            _partLOC = partLOC;
            _partQTY = partQTY;


            string sql = "BASE_Product_InStock_Dispenser_Definition_SelItem_Addnew";

            SqlParameter[] sParams = new SqlParameter[1]; // Parameter count

            sParams[0] = new SqlParameter();
            sParams[0].SqlDbType = SqlDbType.Int;
            sParams[0].ParameterName = "@prodID";
            sParams[0].Value = _partIDx;

            DataSet ds = new DataSet();
            ds = cls.ExecuteDataSet(sql, sParams);
            if (ds.Tables[0].Rows.Count > 0)
            {
                _subCode01 = ds.Tables[0].Rows[0][4].ToString();
                _subCode02 = ds.Tables[0].Rows[0][5].ToString();
                _subCode03 = ds.Tables[0].Rows[0][6].ToString();
            }
            else
            {
                _subCode01 = "";
                _subCode02 = "";
                _subCode03 = "";
            }

            subCodeNo = (_subCode01 != "" && _subCode01 != null) ? subCodeNo + 1 : subCodeNo;
            subCodeNo = (_subCode02 != "" && _subCode02 != null) ? subCodeNo + 1 : subCodeNo;
            subCodeNo = (_subCode03 != "" && _subCode03 != null) ? subCodeNo + 1 : subCodeNo;
            _subCodeNo = subCodeNo.ToString();
        }

        private void txtSampleCode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtSampleCode.Text.Trim().Length == 1)
                {
                    tmrDelay.Enabled = true;
                    tmrDelay.Start();
                    tmrDelay.Tick += new EventHandler(tmrDelay_Tick);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void tmrDelay_Tick(object sender, EventArgs e)
        {
            try
            {
                tmrDelay.Stop();
                string strCurrentString = txtSampleCode.Text.Trim().ToString();
                if (strCurrentString != "")
                {
                    //Do something with the barcode entered 
                    fnCatchCode(strCurrentString);

                    txtSampleCode.Text = "";
                }
                txtSampleCode.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void fnCatchCode(string code)
        {
            //string msg = "";
            //msg += "boxCode: " + _boxCode + "\r\n";
            //msg += "partIDx: " + _partIDx + "\r\n";
            //msg += "partName: " + _partName + "\r\n";
            //msg += "partCode: " + _partCode + "\r\n";
            //msg += "partLOT: " + _partLOT + "\r\n";
            //msg += "partDAY: " + _partDAY + "\r\n";
            //msg += "partLOC: " + _partLOC + "\r\n";
            //msg += "partQTY: " + _partQTY + "\r\n";
            //msg += "-------------------------\r\n";
            //msg += "subCode01: " + _subCode01 + "\r\n";
            //msg += "subCode02: " + _subCode02 + "\r\n";
            //msg += "subCode03: " + _subCode03 + "\r\n";
            //msg += "subCodeNo: " + _subCodeNo + "\r\n";
            //msg += "-------------------------\r\n";
            //msg += "code: " + code + "\r\n";
            //MessageBox.Show(msg);

            if (_seq < 4)
            {
                switch (_seq)
                {
                    case 1:
                        if (code.Contains(_subCode01))
                        {
                            lblStatus01.Text = "OK";
                            lblStatus01.BackColor = Color.Green;
                        }
                        else
                        {
                            lblStatus01.Text = "NG";
                            lblStatus01.BackColor = Color.Red;

                        }
                        lblStatus01.ForeColor = Color.White;
                        break;
                    case 2:
                        if (code.Contains(_subCode01))
                        {
                            lblStatus02.Text = "OK";
                            lblStatus02.BackColor = Color.Green;
                        }
                        else
                        {
                            lblStatus02.Text = "NG";
                            lblStatus02.BackColor = Color.Red;

                        }
                        lblStatus02.ForeColor = Color.White;
                        break;
                    case 3:
                        if (code.Contains(_subCode01))
                        {
                            lblStatus03.Text = "OK";
                            lblStatus03.BackColor = Color.Green;
                        }
                        else
                        {
                            lblStatus03.Text = "NG";
                            lblStatus03.BackColor = Color.Red;

                        }
                        lblStatus03.ForeColor = Color.White;
                        break;
                }

                _seq = _seq + 1;
            }
        }

        private void lblStatus03_TextChanged(object sender, EventArgs e)
        {
            string status01 = lblStatus01.Text;
            string status02 = lblStatus02.Text;
            string status03 = lblStatus03.Text;

            txtSampleCode.Enabled = false;
            txtSampleCode.BackColor = Color.LightGray;
            tableLayoutPanel2.BackColor = Color.LightGray;

            _status = (status01 == "OK" && status02 == "OK" && status03 == "OK") ? true : false;

            timerMsg.Enabled = true;
            timerMsg.Start();
            timerMsg.Tick += new EventHandler(timerMsg_Tick);
        }

        public void timerMsg_Tick(object sender, EventArgs e)
        {
            try
            {
                timerMsg.Stop();
                string msg = "";
                if (_status == true)
                {
                    msg = "HỆ THỐNG CHẤP NHẬN NHẬP KHO KIỆN HÀNG";
                    //MessageBox.Show(msg, cls.appName(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    cls.AutoClosingMessageBox.Show(msg, cls.appName(), 3000);
                }
                else
                {
                    msg = "PHÁT HIỆN CÓ HÀNG LẪN.\r\nHệ thống chưa thể nhập kho kiện " + _boxCode + "\r\n\r\nHÃY KIỂM TRA LẠI TOÀN BỘ HÀNG TRÊN KIỆN.";
                    MessageBox.Show(msg, cls.appName(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                frmFinishGoodScanIn_v1o1 frmScanIn = new frmFinishGoodScanIn_v1o1();
                frmScanIn.fnGetProofStatus(_status, _boxCode, _partIDx, _partName, _partCode, _partLOT, _partDAY, _partLOC, _partQTY);
                this.Close();
            }
        }

    }
}
