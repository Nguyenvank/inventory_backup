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
    public partial class frmFinishGoodScanIn_v1o4_CheckProof : Form
    {
        bool _packStd = false;
        public string _line = "", _boxCode = "", _partIDx = "", _partName = "", _partCode = "", _partLOT = "", _partDAY = "", _partLOC = "", _partQTY = "";
        public string _subCode01 = "", _subCode02 = "", _subCode03 = "";
        string _sample_code_01 = "", _sample_code_02 = "", _sample_code_03 = "";
        public int _seq = 1, _subCodeNo = 0;
        public Boolean _status = false, _same = false;

        public System.Windows.Forms.Timer tmrDelay, timerMsg;

        public frmFinishGoodScanIn_v1o4_CheckProof()
        {
            InitializeComponent();
        }

        public frmFinishGoodScanIn_v1o4_CheckProof(bool packStd)
        {
            InitializeComponent();

            _packStd = packStd;
        }

        private void frmFinishGoodScanIn_v1o4_CheckProof_Load(object sender, EventArgs e)
        {
            rdb_Line01.Checked = rdb_Line02.Checked = false;
            rdb_Line01.BackColor = rdb_Line02.BackColor = Color.FromKnownColor(KnownColor.Control);

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

            Fnc_Check_Pack_Std_Status();

            //rdb_Line01.Checked = rdb_Line02.Checked = false;
            //rdb_Line01.BackColor = rdb_Line02.BackColor = Color.FromKnownColor(KnownColor.Control);

            //tlpSampleCode.BackColor = txtSampleCode.BackColor = Color.Gainsboro;
            //tlpSampleCode.Enabled = false;

            tmrDelay = new System.Windows.Forms.Timer();
            tmrDelay.Interval = 500;
            tmrDelay.Enabled = false;

            timerMsg = new System.Windows.Forms.Timer();
            timerMsg.Interval = 1500;
            timerMsg.Enabled = false;

            txtSampleCode.Focus();
        }

        public void Fnc_Check_Pack_Std_Status()
        {
            switch (_packStd)
            {
                case true:
                    rdb_Line01.Enabled = rdb_Line02.Enabled = true;
                    rdb_Line01.Checked = rdb_Line02.Checked = false;
                    rdb_Line01.BackColor = rdb_Line02.BackColor = Color.FromKnownColor(KnownColor.Control);

                    tlpSampleCode.BackColor = txtSampleCode.BackColor = Color.Gainsboro;
                    tlpSampleCode.Enabled = false;
                    break;
                case false:
                    rdb_Line01.Enabled = rdb_Line02.Enabled = false;
                    rdb_Line01.Checked = rdb_Line02.Checked = false;
                    rdb_Line01.BackColor = rdb_Line02.BackColor = Color.FromKnownColor(KnownColor.Control);

                    tlpSampleCode.BackColor = txtSampleCode.BackColor = Color.White;
                    tlpSampleCode.Enabled = true;
                    break;
            }

            //rdb_Line01.Checked = rdb_Line02.Checked = false;
            //rdb_Line01.BackColor = rdb_Line02.BackColor = Color.FromKnownColor(KnownColor.Control);

            //tlpSampleCode.BackColor = txtSampleCode.BackColor = Color.Gainsboro;
            //tlpSampleCode.Enabled = false;
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
            //_subCodeNo = subCodeNo.ToString();
            _subCodeNo = subCodeNo;
        }

        public void fnGetPart(string boxCode, string partIDx, string partName, string partCode, string partLOT, string partDAY, string partLOC, string partQTY, bool partStd)
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
            //_subCodeNo = subCodeNo.ToString();
            _subCodeNo = subCodeNo;
        }

        private void txtSampleCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string sample_code = "";
                try
                {
                    sample_code = txtSampleCode.Text.Trim();

                    //if (txtSampleCode.Text.Trim().Length == 1)
                    if (sample_code.Length > 0)
                    {
                        switch (_seq)
                        {
                            case 1:
                                _sample_code_01 = sample_code;
                                break;
                            case 2:
                                _sample_code_02 = sample_code;
                                break;
                            case 3:
                                _sample_code_03 = sample_code;
                                break;
                        }
                        //if (_sample_code_01.Length == 0) { _sample_code_01 = sample_code.ToUpper(); }
                        //else if (_sample_code_02.Length == 0) { _sample_code_02 = sample_code.ToUpper(); }
                        //else if (_sample_code_03.Length == 0) { _sample_code_03 = sample_code.ToUpper(); }

                        if (_sample_code_01.Length > 0 && _sample_code_02.Length > 0 && _sample_code_03.Length > 0)
                        {
                            //MessageBox.Show("Sample 01: " + _sample_code_01 + "\r\nSample 02: " + _sample_code_02 + "\r\nSample 03: " + _sample_code_03);
                        }

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
        }

        private void txtSampleCode_TextChanged(object sender, EventArgs e)
        {
            //string sample_code = "";
            //try
            //{
            //    sample_code = txtSampleCode.Text.Trim();

            //    if (txtSampleCode.Text.Trim().Length == 1)
            //    if (sample_code.Length > 0)
            //    {
            //        if (_sample_code_01.Length == 0) { _sample_code_01 = sample_code.ToUpper(); }
            //        else if (_sample_code_02.Length == 0) { _sample_code_02 = sample_code.ToUpper(); }
            //        else if (_sample_code_03.Length == 0) { _sample_code_03 = sample_code.ToUpper(); }

            //        if (_sample_code_01.Length > 0 && _sample_code_02.Length > 0 && _sample_code_03.Length > 0)
            //        {
            //            MessageBox.Show("Sample 01: " + _sample_code_01 + "\r\nSample 02: " + _sample_code_02 + "\r\nSample 03: " + _sample_code_03);

            //            tmrDelay.Enabled = true;
            //            tmrDelay.Start();
            //            tmrDelay.Tick += new EventHandler(tmrDelay_Tick);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
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
                switch (_subCodeNo)
                {
                    case 1:
                        Fnc_Check_SubCode_01(code);
                        break;
                    case 2:
                        Fnc_Check_SubCode_02(code);
                        break;
                    case 3:
                        Fnc_Check_SubCode_03(code);
                        break;
                }
            }
        }

        public void Fnc_Check_SubCode_01(string code)
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
                    if (code.Contains(_subCode01) && cls.Fnc_Compare_String_OrdinalIgnoreCase(code, _sample_code_01) == false)
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
                    if (code.Contains(_subCode01) && cls.Fnc_Compare_String_OrdinalIgnoreCase(code, _sample_code_01) == false && cls.Fnc_Compare_String_OrdinalIgnoreCase(code, _sample_code_02) == false)
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

        public void Fnc_Check_SubCode_02(string code)
        {
            switch (_seq)
            {
                case 1:
                    if (code.Contains(_subCode01) || code.Contains(_subCode02))
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
                    if (code.Contains(_subCode01) || code.Contains(_subCode02))
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
                    if (code.Contains(_subCode01) || code.Contains(_subCode02))
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

        public void Fnc_Check_SubCode_03(string code)
        {

        }

        private void lblStatus03_TextChanged(object sender, EventArgs e)
        {
            string line = (rdb_Line01.Checked) ? "4" : "5";
            string status01 = lblStatus01.Text;
            string status02 = lblStatus02.Text;
            string status03 = lblStatus03.Text;

            txtSampleCode.Enabled = false;
            txtSampleCode.BackColor = Color.LightGray;
            tlpSampleCode.BackColor = Color.LightGray;

            _status = (status01 == "OK" && status02 == "OK" && status03 == "OK") ? true : false;
            _line = line;

            //_differ = (string.Compare(_sample_code_01, _sample_code_02) == 0 && string.Compare(_sample_code_02, _sample_code_03) == 0 && string.Compare(_sample_code_03, _sample_code_01) == 0) ? true : false;
            //_differ = (_sample_code_01 != _sample_code_02 && _sample_code_02 != _sample_code_03 && _sample_code_03 != _sample_code_01) ? true : false;
            //_differ = (string.Compare(_sample_code_01, _sample_code_02) == 0 || string.Compare(_sample_code_02, _sample_code_03) == 0 || string.Compare(_sample_code_03, _sample_code_01) == 0) ? true : false;
            //_differ = (String.Compare(_sample_code_01, _sample_code_02, true) == 0 || String.Compare(_sample_code_02, _sample_code_03, true) == 0 || String.Compare(_sample_code_03, _sample_code_01, true) == 0) ? true : false;
            _same = cls.Fnc_Compare_Three_String(_sample_code_01, _sample_code_02, _sample_code_03) ? true : false;

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
                if (_status == true && _same == false)
                {
                    msg = "HỆ THỐNG CHẤP NHẬN NHẬP KHO KIỆN HÀNG";
                    //MessageBox.Show(msg, cls.appName(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    cls.AutoClosingMessageBox.Show(msg, cls.appName(), 3000);

                    frmFinishGoodScanIn_v1o4 frmScanIn = new frmFinishGoodScanIn_v1o4();
                    frmScanIn.fnGetProofStatus(_status, _line, _boxCode, _partIDx, _partName, _partCode, _partLOT, _partDAY, _partLOC, _partQTY);
                    //this.Close();

                    _sample_code_01 = _sample_code_02 = _sample_code_03 = "";
                    _status = false;
                    _same = true;
                }
                else
                {
                    if (_status == false)
                    {
                        msg = "PHÁT HIỆN CÓ HÀNG LẪN.\r\nHệ thống chưa thể nhập kho kiện " + _boxCode + "\r\n\r\nHÃY KIỂM TRA LẠI TOÀN BỘ HÀNG TRÊN KIỆN.";
                    }

                    if (_same == true)
                    {
                        msg = "PHÁT HIỆN MẪU QUÉT BỊ LẶP.\r\nHệ thống phát hiện mẫu quét trùng nhau " + _boxCode + "\r\n\r\nHÃY CHỌN 3 MẪU KHÁC NHAU ĐỂ QUÉT.\r\n\r\n";
                        msg += "Mẫu 01: " + _sample_code_01 + "\r\n";
                        msg += "Mẫu 02: " + _sample_code_02 + "\r\n";
                        msg += "Mẫu 03: " + _sample_code_03 + "\r\n";
                    }
                    MessageBox.Show(msg, cls.appName(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show("HÃY CHỌN MẪU KHÁC NHAU VÀ QUÉT LẠI !!!\r\n\r\nHệ thống KHÔNG ghi nhận mã " + _boxCode + " do lỗi trùng mẫu.", cls.appName(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Close();
                //frmFinishGoodScanIn_v1o4 frmScanIn = new frmFinishGoodScanIn_v1o4();
                //frmScanIn.fnGetProofStatus(_status, _line, _boxCode, _partIDx, _partName, _partCode, _partLOT, _partDAY, _partLOC, _partQTY);
                //this.Close();
            }
        }

        public void Fnc_Choose_Line_BackColor()
        {
            if (rdb_Line01.Checked || rdb_Line02.Checked)
            {
                if (rdb_Line01.Checked) { rdb_Line01.BackColor = Color.LightGreen; rdb_Line02.BackColor = Color.FromKnownColor(KnownColor.Control); }
                if (rdb_Line02.Checked) { rdb_Line02.BackColor = Color.LightGreen; rdb_Line01.BackColor = Color.FromKnownColor(KnownColor.Control); }

                tlpSampleCode.Enabled = txtSampleCode.Enabled = true;
                tlpSampleCode.BackColor = txtSampleCode.BackColor = Color.White;
                txtSampleCode.Focus();
            }
        }

        private void rdb_Line01_CheckedChanged(object sender, EventArgs e)
        {
            Fnc_Choose_Line_BackColor();
        }

        private void rdb_Line02_CheckedChanged(object sender, EventArgs e)
        {
            Fnc_Choose_Line_BackColor();
        }

        private void frmFinishGoodScanIn_v1o4_CheckProof_Shown(object sender, EventArgs e)
        {
            Fnc_Check_Pack_Std_Status();

            //rdb_Line01.Checked = rdb_Line02.Checked = false;
            //rdb_Line01.BackColor = rdb_Line02.BackColor = Color.FromKnownColor(KnownColor.Control);

            //tlpSampleCode.BackColor = txtSampleCode.BackColor = Color.Gainsboro;
            //tlpSampleCode.Enabled = false;

        }

        private void frmFinishGoodScanIn_v1o4_CheckProof_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

    }
}
