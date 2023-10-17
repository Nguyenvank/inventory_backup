using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Data.SqlClient;

namespace Inventory_Data.Ctrl
{
    public partial class uc_CapacityScanner_v3o1 : UserControl
    {
        string lineno = "", lineId = "", linenm = "", interval = "", tagname = "", opc = "", lineOK = "", lineNG = "", lingWN = "", prevCode = "";

        static int VALIDATION_DELAY = 200;
        cls.Ini ini = new cls.Ini(Application.StartupPath + "\\" + Application.ProductName + ".ini");
        System.Threading.Timer timer = (System.Threading.Timer)null;
        System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer tNG = new System.Windows.Forms.Timer();

        //public string _date;
        string _shiftname="",_shiftno = "",_machine = "",_idx = "",_partID = "",_partname = "",_partcode = "",_partuph = "";
        string _partordertime = "",_partordertotal = "",_partsubcode = "",_partsubcode01 = "",_partsubcode02 = "",_partsubcode03 = "";
        string _nextIdx = "", _nextPartname = "", _nextTime = "", _prevBarcode = "", _code = "", _lastCode = "";
        int _statusOK = 0, _statusNG = 0, _total = 0, _autoNG = 0, _counter = 0, _counterDis = 0, _codeN = 0, _scanStage = 0;

        decimal _rate = 0;
        bool _scanStatus;

        DateTime _dt;
        DateTime _dtLunchStart;
        DateTime _dtLunchEnd;
        DateTime _dtDinnerStart;
        DateTime _dtDinnerEnd;
        DateTime _dtNightStart;
        DateTime _dtNightEnd;
        DateTime _dtBreakfastStart;
        DateTime _dtBreakfastEnd;

        public uc_CapacityScanner_v3o1()
        {
            InitializeComponent();

            linenm = ini.GetIniValue("MACHINE", "NM", "DISPENSER").Trim();
            lineno = ini.GetIniValue("MACHINE", "NO", "1").Trim();
            lineId = ini.GetIniValue("MACHINE", "ID", "4").Trim();
            interval = ini.GetIniValue("MACHINE", "TM", "25").Trim();

            tNG.Interval = 1000;
            tNG.Enabled = true;
            tNG.Tick += new EventHandler(Fnc_CheckAutoNG);

            _counter = (interval != "" && interval != null) ? Convert.ToInt32(interval) : 25;
            _counterDis = _counter;

            _autoNG = (chkAutoNG.Checked) ? 1 : 0;
            chkAutoNG.BackColor = (_autoNG == 1) ? Color.Red : Color.OrangeRed;

            cls.SetDoubleBuffer(tableLayoutPanel1, true);
            cls.SetDoubleBuffer(tableLayoutPanel2, true);
            cls.SetDoubleBuffer(tableLayoutPanel3, true);
        }

        private void uc_CapacityScanner_v3o1_Load(object sender, EventArgs e)
        {
            init();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Fnc_Load_Timer();
        }

        public void init()
        {
            init_Load_Controls();
        }

        public void init_Load_Controls()
        {
            //Fnc_Load_Thread();
            Fnc_Load_Controls();
        }


        /***********************************************/

        public void Fnc_Load_Controls()
        {
            lblStatus.Text =
                lblItemCode.Text =
                lblMessage.Text =
                lblPIC.Text = "";
            lblStatus.BackColor =
                lblItemCode.BackColor =
                lblMessage.BackColor =
                lblPIC.BackColor = Color.Gray;
            lblOrder.Text =
                lblGoal.Text =
                lblOK.Text =
                lblNG.Text =
                lblRate.Text = "0";
            lbl_Code_01.Text =
                lbl_Code_02.Text =
                lblNextOrder.Text = "";
        }

        public void Fnc_Load_Timer()
        {
            txtItemCode.Focus();
            cls.fnDateTime(lblDateTime, 3);
            lblDateTime.ForeColor = Color.White;

            Fnc_BindInit();
        }

        public void Fnc_Load_Thread()
        {
            Thread loadDateTime = new Thread(() =>
            {
                while (true)
                {
                    //Fnc_Select_Scan_No(lineId);
                    txtItemCode.Focus();
                    cls.fnDateTime(lblDateTime, 3);
                    lblDateTime.ForeColor = Color.White;

                    Thread.Sleep(1000);
                }
            });
            loadDateTime.IsBackground = true;
            loadDateTime.Start();

            //Thread loadData = new Thread(() =>
            //{
            //    while (true)
            //    {
            //        //Fnc_Select_Scan_No(lineId);

            //        Thread.Sleep(500);
            //    }
            //});
            //loadData.IsBackground = true;
            //loadData.Start();

        }

        public void Fnc_BindInit()
        {
            try
            {
                string idx = "", statusOK = "", statusNG = "";
                string line = "", partname = "", partcode = "", order = "";
                string ok = "0", ng = "0", rate = "0", nextorder = "", pic = "";
                string code01 = "", code02 = "", code03 = "", goal = "";
                int codeN = 0;
                string sqlInit = "V2o1_BASE_Capacity_Dispenser_Scan_SelItem_V2o2_Addnew";
                DataTable dtInit = new DataTable();

                SqlParameter[] sParamsInit = new SqlParameter[1]; // Parameter count
                sParamsInit[0] = new SqlParameter();
                sParamsInit[0].SqlDbType = SqlDbType.Int;
                sParamsInit[0].ParameterName = "@lineId";
                sParamsInit[0].Value = lineId;

                dtInit = cls.ExecuteDataTable(sqlInit, sParamsInit);

                if (dtInit.Rows.Count > 0)
                {
                    idx = dtInit.Rows[0][0].ToString();
                    line = dtInit.Rows[0][7].ToString().ToUpper();
                    partname = dtInit.Rows[0][4].ToString();
                    partcode = dtInit.Rows[0][5].ToString();
                    pic = "PIC: " + dtInit.Rows[0][9].ToString().ToUpper();
                    order = dtInit.Rows[0][14].ToString();
                    ok = dtInit.Rows[0][16].ToString();
                    ng = dtInit.Rows[0][17].ToString();
                    rate = dtInit.Rows[0][19].ToString();
                    code01 = dtInit.Rows[0][20].ToString();
                    code02 = dtInit.Rows[0][21].ToString();
                    code03 = dtInit.Rows[0][22].ToString();
                    goal = dtInit.Rows[0][23].ToString();
                    nextorder = "";
                    //txtItemCode.Enabled = true;
                    txtItemCode.Focus();

                    if (code01 != "" && code01 != null)
                    {
                        codeN = 1;
                    }
                    if (code02 != "" && code02 != null)
                    {
                        codeN = 2;
                    }
                    if (code03 != "" && code03 != null)
                    {
                        codeN = 3;
                    }
                }
                else
                {
                    idx = "0";
                    line = "N/A";
                    partname = "N/A";
                    partcode = "N/A";
                    pic = "";
                    order = "0";
                    ok = "0";
                    ng = "0";
                    rate = "0.0";
                    code01 = "";
                    code02 = "";
                    code03 = "";
                    nextorder = "";
                    //txtItemCode.Enabled = false;

                    codeN = 0;
                }

                //string msg = "";
                //msg += "idx: " + idx + "\r\n";
                //msg += "line: " + line + "\r\n";
                //msg += "partname: " + partname + "\r\n";
                //msg += "partcode: " + partcode + "\r\n";
                //msg += "pic: " + pic + "\r\n";
                //msg += "order: " + order + "\r\n";
                //msg += "ok: " + ok + "\r\n";
                //msg += "ng: " + ng + "\r\n";
                //msg += "rate: " + rate + "\r\n";
                //msg += "code01: " + code01 + "\r\n";
                //msg += "code02: " + code02 + "\r\n";
                //msg += "code03: " + code03 + "\r\n";
                //msg += "nextorder: " + nextorder + "\r\n";

                //MessageBox.Show(msg);

                //_code = _partsubcode.ToString();

                _codeN = codeN;
                _idx = idx;
                _statusOK = (ok != "" && ok != null) ? Convert.ToInt32(ok) : 0;
                _statusNG = (ng != "" && ng != null) ? Convert.ToInt32(ng) : 0;
                //_total = _statusOK + _statusNG;
                _rate = (rate != "" && rate != null) ? Convert.ToDecimal(rate) : 0;
                _partsubcode01 = code01;
                _partsubcode02 = code02;
                _partsubcode03 = code03;

                lbl_Code_01.Text = code01;
                lbl_Code_02.Text = code02;

                lblLine.Text = line;
                lblPartName.Text = partname + "\r\n" + partcode;
                //lblPartCode.Text = partcode;
                lblPIC.Text = pic;
                lblOrder.Text = order;
                lblOK.Text = ok;
                lblNG.Text = ng;
                lblGoal.Text = goal;
                //lblRate.Text = rate + "%";
                lblRate.Text = String.Format("{0:0.0}", _rate);// + "%";
                //lblRate.Text = String.Format("{0:0}", _rate);// + "%";
                lblNextOrder.Text = nextorder;

                //fnGetNextOrder();
                //fnc_RateColor();
            }
            catch
            {

            }
            finally
            {

            }
        }

        public void Fnc_CheckAutoNG(object sender, EventArgs e)
        {
            try
            {
                _counterDis--;
                if (_counterDis < 0)
                {
                    _counterDis = _counter;
                    fnInsertDB("Not found barcode " + String.Format("{0:yyyyMMdd_HHmmss}", DateTime.Now));
                    fnDisplayMsg();
                }
                else
                {
                    label9.Text = "ITEM (" + String.Format("{0:00}", _counterDis) + ")";

                    //string packCode = txtItemCode.Text.Trim();

                    //if (packCode != "" && packCode != null)
                    //{

                    //}
                    //else
                    //{
                    //    //fnInsertDB("Not found barcode " + String.Format("{0:yyyyMMdd_HHmmss}", DateTime.Now));
                    //}
                }
            }
            catch
            {

            }
            finally
            {

            }
        }

        public void NGStatus()
        {

            try
            {
                lblStatus.BackColor = Color.Red;
                lblStatus.ForeColor = Color.White;
                lblStatus.Text = "NG";
                lblMessage.BackColor = Color.Red;
                lblMessage.ForeColor = Color.White;

                lblItemCode.BackColor = Color.Red;
                lblItemCode.ForeColor = Color.White;
                lblPIC.BackColor = Color.Red;

                //txtItemCode.Enabled = false;

                //tagname = "Channel1.Device1." + lblLine.Text.ToLower().Replace(" ", "") + "_NG";

                //if (tagname != "0" && opc != "0")
                //if (tagname != "" && tagname != null && opc != "0")
                //{
                //    //fnConnectOPC(tagname);
                //    fnWarningStart(tagname);
                //    fnWarningStop(tagname);
                //    //fnDisConnectOPC();
                //}
                //tagname = "";
            }
            catch
            {

            }
            finally
            {

            }
        }

        public void OKStatus()
        {
            try
            {
                lblStatus.BackColor = Color.DodgerBlue;
                lblStatus.ForeColor = Color.White;
                lblStatus.Text = "OK";
                lblMessage.BackColor = Color.DodgerBlue;
                lblMessage.ForeColor = Color.White;

                lblItemCode.BackColor = Color.DodgerBlue;
                lblItemCode.ForeColor = Color.White;
                lblPIC.BackColor = Color.DodgerBlue;

                //txtItemCode.Enabled = false;

                //tagname = "Channel1.Device1." + lblLine.Text.ToLower().Replace(" ", "") + "_OK";
            }
            catch
            {

            }
            finally
            {

            }
        }

        public void fnInsertDB(string s)
        {
            int seq = 1;
            try
            {
                if (s != prevCode)
                {
                    int nCode = (!(_partsubcode01 != "") ? 0 : (!(_partsubcode02 != "") ? 1 : (!(_partsubcode03 != "") ? 2 : 3)));
                    if (nCode > 1)
                    {
                        if (s.Contains(_partsubcode01) && _partsubcode01 != "" || s.Contains(_partsubcode02) && _partsubcode02 != "" || s.Contains(_partsubcode03) && _partsubcode03 != "")
                        {
                            string str = "";
                            if (s.Contains(_partsubcode01) && _partsubcode01 != "")
                            {
                                str = _partsubcode01;
                                lbl_Code_01.BackColor = Color.LightGreen;
                            }
                            else if (s.Contains(_partsubcode02) && _partsubcode02 != "")
                            {
                                str = _partsubcode02;
                                lbl_Code_02.BackColor = Color.LightGreen;
                            }
                            else if (s.Contains(_partsubcode03) && _partsubcode03 != "")
                            {
                                str = _partsubcode03;
                            }

                            //if (str != _prevBarcode)
                            if (str != _prevBarcode)
                            {
                                OKStatus();
                                //if (s.Contains(str))
                                if (s.Contains(_partsubcode01))
                                {
                                    _statusOK = _statusOK + 1;
                                    //lbl_Code_01.BackColor = Color.LightGreen;
                                    _prevBarcode = s;
                                }
                                lblOK.Text = _statusOK.ToString();
                                lblMessage.Text = "";
                                _scanStatus = true;
                                _scanStage = 2;
                            }
                            else
                            {
                                NGStatus();
                                _statusNG = _statusNG + 1;
                                lblNG.Text = _statusNG.ToString();
                                lblMessage.Text = "CANNOT SCAN THE QR BARCODE ON PREVIOUS VALVE   /   KHÔNG QUÉT ĐƯỢC MÃ VẠCH TRÊN THÂN VAN NGAY TRƯỚC ĐÓ";
                                _scanStatus = false;
                                _scanStage = 1;
                            }
                            _prevBarcode = str;
                        }
                        else
                        {
                            NGStatus();
                            _statusNG = _statusNG + 1;
                            lblNG.Text = _statusNG.ToString();
                            lblMessage.Text = "ITEM NG BECAUSE WRONG TYPE   /   HÀNG LỖI DO SAI LOẠI VAN";
                            _scanStatus = false;
                            _scanStage = 1;
                        }
                        _total = _statusOK + _statusNG;
                        fnSaveTotal(s);
                    }
                    else
                    {
                        if (s.Contains(_partsubcode01) == true)
                        {
                            OKStatus();
                            _statusOK = _statusOK + 1;
                            lblOK.Text = _statusOK.ToString();
                            lblMessage.Text = "";
                            _scanStatus = true;
                            _scanStage = 2;

                            lbl_Code_01.BackColor = Color.LightGreen;
                        }
                        else
                        {
                            NGStatus();
                            _statusNG = _statusNG + 1;
                            lblNG.Text = _statusNG.ToString();
                            lblMessage.Text = "ITEM NG BECAUSE WRONG TYPE   /   HÀNG LỖI DO SAI LOẠI VAN";
                            _scanStatus = false;
                            _scanStage = 1;

                            lbl_Code_01.BackColor = Color.Red;
                        }
                        _total = _statusOK + _statusNG;
                        fnSaveTotal(s);
                    }


                    Fnc_Update_Scan_Readable(lineId);

                    prevCode = (nCode > 1) ? s : s + String.Format("{0:ddMMyyyyHHmmss}", DateTime.Now);
                    //Fnc_BindInit();
                    txtItemCode.Text = "";
                    txtItemCode.Focus();

                    _counterDis = 0;
                    _counterDis = _counter;
                    tNG.Stop();
                    tNG.Start();

                }
                else
                {
                    NGStatus();
                    _statusNG = _statusNG + 1;
                    lblNG.Text = _statusNG.ToString();
                    lblMessage.Text = "ITEM NG BECAUSE WRONG TYPE   /   HÀNG LỖI DO SAI LOẠI VAN";
                    _scanStage = 1;
                }

            }
            catch
            {

            }
            finally
            {

            }
        }

        public void fnSaveTotal(string barcode)
        {
            try
            {
                string assyOK = "", assyNG = "", assyTotal = "", idx = "";
                assyOK = _statusOK.ToString();
                assyNG = _statusNG.ToString();
                assyTotal = _total.ToString();
                idx = _idx;

                //string str = "";
                //str += "assyOK: " + assyOK + "\r\n";
                //str += "assyNG: " + assyNG + "\r\n";
                //str += "assyTotal: " + assyTotal + "\r\n";
                //str += "idx: " + idx + "\r\n";
                //MessageBox.Show(str);

                string sql = "V2o1_BASE_Capacity_Dispenser_Scan_AddItem_V2o2_Addnew";

                SqlParameter[] sParams = new SqlParameter[5]; // Parameter count
                sParams[0] = new SqlParameter();
                sParams[0].SqlDbType = SqlDbType.Int;
                sParams[0].ParameterName = "@totalOK";
                sParams[0].Value = assyOK;

                sParams[1] = new SqlParameter();
                sParams[1].SqlDbType = SqlDbType.Int;
                sParams[1].ParameterName = "@totalNG";
                sParams[1].Value = assyNG;

                sParams[2] = new SqlParameter();
                sParams[2].SqlDbType = SqlDbType.Int;
                sParams[2].ParameterName = "@total";
                sParams[2].Value = assyTotal;

                sParams[3] = new SqlParameter();
                sParams[3].SqlDbType = SqlDbType.Int;
                sParams[3].ParameterName = "@idx";
                sParams[3].Value = idx;

                sParams[4] = new SqlParameter();
                sParams[4].SqlDbType = SqlDbType.VarChar;
                sParams[4].ParameterName = "@barcode";
                sParams[4].Value = barcode;

                cls.fnUpdDel(sql, sParams);

                //fnBindInit();
            }
            catch
            {

            }
            finally
            {

            }
        }

        public void fnSaveTotal(string barcode01, string barcode02 = "", string barcode03 = "")
        {
            try
            {
                string assyOK = "", assyNG = "", assyTotal = "", idx = "";
                assyOK = _statusOK.ToString();
                assyNG = _statusNG.ToString();
                assyTotal = _total.ToString();
                idx = _idx;

                //string str = "";
                //str += "assyOK: " + assyOK + "\r\n";
                //str += "assyNG: " + assyNG + "\r\n";
                //str += "assyTotal: " + assyTotal + "\r\n";
                //str += "idx: " + idx + "\r\n";
                //MessageBox.Show(str);

                string sql = "V2o1_BASE_Capacity_Dispenser_Scan_AddItem_V2o3_Addnew";

                SqlParameter[] sParams = new SqlParameter[7]; // Parameter count
                sParams[0] = new SqlParameter();
                sParams[0].SqlDbType = SqlDbType.Int;
                sParams[0].ParameterName = "@totalOK";
                sParams[0].Value = assyOK;

                sParams[1] = new SqlParameter();
                sParams[1].SqlDbType = SqlDbType.Int;
                sParams[1].ParameterName = "@totalNG";
                sParams[1].Value = assyNG;

                sParams[2] = new SqlParameter();
                sParams[2].SqlDbType = SqlDbType.Int;
                sParams[2].ParameterName = "@total";
                sParams[2].Value = assyTotal;

                sParams[3] = new SqlParameter();
                sParams[3].SqlDbType = SqlDbType.Int;
                sParams[3].ParameterName = "@idx";
                sParams[3].Value = idx;

                sParams[4] = new SqlParameter();
                sParams[4].SqlDbType = SqlDbType.VarChar;
                sParams[4].ParameterName = "@barcode01";
                sParams[4].Value = barcode01;

                sParams[5] = new SqlParameter();
                sParams[5].SqlDbType = SqlDbType.VarChar;
                sParams[5].ParameterName = "@barcode02";
                sParams[5].Value = barcode02;

                sParams[6] = new SqlParameter();
                sParams[6].SqlDbType = SqlDbType.VarChar;
                sParams[6].ParameterName = "@barcode03";
                sParams[6].Value = barcode03;

                cls.fnUpdDel(sql, sParams);

                //fnBindInit();
            }
            catch
            {

            }
            finally
            {

            }
        }

        public void fnDisplayMsg()
        {
            try
            {
                t.Interval = 2000;
                t.Tick += new EventHandler(fnChangeStatusBackColor);
                t.Enabled = true;
                t.Start();
                //txtItemCode.Enabled = false;
            }
            catch
            {

            }
            finally
            {

            }
        }

        public void fnChangeStatusBackColor(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                lblStatus.BackColor = Color.Gray;
                lblStatus.ForeColor = Color.White;

                lblMessage.Text = "Waiting for checking...   /   Đang chờ để kiểm tra...";
                lblMessage.BackColor = Color.Gray;
                lblMessage.ForeColor = Color.White;

                lblItemCode.BackColor = Color.Gray;
                lblItemCode.ForeColor = Color.White;

                lblPIC.BackColor = Color.Gray;
                lblPIC.ForeColor = Color.White;

                lbl_Code_01.BackColor = lbl_Code_02.BackColor = Color.Tan;

                prevCode = "";

                //txtItemCode.Enabled = true;
                _scanStage = 0;

                txtItemCode.Focus();
                t.Stop();
            }
            catch
            {

            }
            finally
            {

            }
        }

        public void Fnc_Update_Scan_Readable(string line)
        {
            try
            {
                int listCount = 0;
                bool _readable = true;
                string sql = "V2_BASE_CAPACITY_GET_DISPENSER_SCAN_READABLE_UPDITEM_ADDNEW";

                SqlParameter[] sParams = new SqlParameter[2]; // Parameter count

                sParams[0] = new SqlParameter();
                sParams[0].SqlDbType = SqlDbType.Int;
                sParams[0].ParameterName = "@line";
                sParams[0].Value = line;

                sParams[1] = new SqlParameter();
                sParams[1].SqlDbType = SqlDbType.Bit;
                sParams[1].ParameterName = "@readable";
                sParams[1].Value = _readable;

                cls.fnUpdDel(sql, sParams);
            }
            catch
            {

            }
            finally
            {

            }
        }



        /***********************************************/

        private void txtItemCode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!(sender as TextBox).ContainsFocus)
                    return;
                DisposeTimer();
                timer = new System.Threading.Timer(new TimerCallback(TimerElapsed), (object)null, VALIDATION_DELAY, VALIDATION_DELAY);
            }
            catch
            {

            }
            finally
            {

            }
        }

        public void DisposeTimer()
        {
            try
            {
                if (timer == null)
                    return;
                timer.Dispose();
                timer = (System.Threading.Timer)null;
            }
            catch
            {

            }
            finally
            {

            }
        }

        public void TimerElapsed(object obj)
        {
            try
            {
                CheckSyntaxAndReport();
                DisposeTimer();
            }
            catch
            {

            }
            finally
            {

            }
        }

        public void CheckSyntaxAndReport()
        {
            try
            {
                Invoke(new Action(() =>
                {
                    //Do everything on the UI thread itself

                    string s = txtItemCode.Text.ToUpper(); 
                    lblItemCode.Text = s;
                    ////txtItemCode.Enabled = false;

                    //fnDisplayMsg();
                    string upper = txtItemCode.Text.ToUpper();
                    string str1 = upper.Substring(0, 4);

                    try
                    {
                        if (upper != _lastCode)
                        {
                            fnInsertDB(upper);
                            _lastCode = (_codeN > 1) ? upper : "";
                            fnDisplayMsg();

                            //fnBindInit();
                        }
                    }
                    catch //(Exception ex)
                    {
                        //int num = (int)MessageBox.Show(ex.ToString());
                    }
                    finally
                    {

                    }

                    ////txtItemCode.Enabled = true;
                    txtItemCode.Text = "";
                    txtItemCode.Focus();
                }));
            }
            catch
            {

            }
            finally
            {

            }
        }

    }
}
