using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_Data.Ctrl
{
    public partial class uc_CapacityScanner_v2o6 : UserControl

    {
        public static int item = 0;


        public string lineno = "";
        public string lineId = "";
        public string linenm = "";
        public string interval = "";
        public string tagname = "";
        public string opc = "SERVER02";
        public string lineOK = "";
        public string lineNG = "";
        public string lingWN = "";

        public string prevCode = "";

        //private Timer _timer;
        //private DateTime _lastBarCodeCharReadTime;

        private static int VALIDATION_DELAY = 500;
        private cls.Ini ini = new cls.Ini(Application.StartupPath + "\\" + Application.ProductName + ".ini");
        private System.Threading.Timer timer = (System.Threading.Timer)null;
        private System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer tNG = new System.Windows.Forms.Timer();

        //public string _date;
        public string _shiftname;
        public string _shiftno;
        public string _machine;
        public string _idx;
        public string _partID;
        public string _partname;
        public string _partcode;
        public string _partuph;
        public string _partordertime;
        public string _partordertotal;
        public string _partsubcode;
        public string _partsubcode01;
        public string _partsubcode02;
        public string _partsubcode03;
        public int _statusOK;
        public int _statusNG;
        public int _total;
        public Decimal _rate;
        public string _nextIdx;
        public string _nextPartname;
        public string _nextTime;
        public string _prevBarcode;
        public string _code;
        public int _autoNG;
        public int _counter;
        public int _counterDis;

        public DateTime _dt;
        public DateTime _dtLunchStart;
        public DateTime _dtLunchEnd;
        public DateTime _dtDinnerStart;
        public DateTime _dtDinnerEnd;
        public DateTime _dtNightStart;
        public DateTime _dtNightEnd;
        public DateTime _dtBreakfastStart;
        public DateTime _dtBreakfastEnd;

        string _lastCode = "";
        int _codeN = 0;
        bool _scanStatus;
        int _scanStage = 0;

        public uc_CapacityScanner_v2o6()
        {
            InitializeComponent();

            linenm = ini.GetIniValue("MACHINE", "NM", "DISPENSER").Trim();
            lineno = ini.GetIniValue("MACHINE", "NO", "1").Trim();
            lineId = ini.GetIniValue("MACHINE", "ID", "4").Trim();
            interval = ini.GetIniValue("MACHINE", "TM", "25").Trim();
            //tagname = ini.GetIniValue("MACHINE", "CH", "Channel1.Device1.dispenser1").Trim();
            //opc = ini.GetIniValue("MACHINE", "IP", "192.168.0.48").Trim();

            tNG.Interval = 1000;
            tNG.Enabled = true;
            tNG.Tick += new EventHandler(fnCheckAutoNG);

            _counter = (interval != "" && interval != null) ? Convert.ToInt32(interval) : 25;
            _counterDis = _counter;

            _autoNG = (chkAutoNG.Checked) ? 1 : 0;
            chkAutoNG.BackColor = (_autoNG == 1) ? Color.Red : Color.OrangeRed;

            cls.SetDoubleBuffer(tableLayoutPanel1, true);
            cls.SetDoubleBuffer(tableLayoutPanel2, true);
            cls.SetDoubleBuffer(tableLayoutPanel3, true);
        }

        private void frmCapacityScanner_v2o4_Load(object sender, EventArgs e)
        {
            try
            {
                init();

                //OKStatus();

                //fnConnectOPC();

                //if (tagname != "0" && opc != "0")
                //if (tagname != "" && tagname != null && opc != "0")
                //{
                //    fnConnectOPC(tagname);
                //}
            }
            catch
            {

            }
            finally
            {

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                fnGetDate();

                fnBindInit();

                _dt = DateTime.Now;
                _dtLunchStart = new DateTime(_dt.Year, _dt.Month, _dt.Day, 11, 50, 0);
                _dtLunchEnd = new DateTime(_dt.Year, _dt.Month, _dt.Day, 12, 59, 59);
                _dtDinnerStart = new DateTime(_dt.Year, _dt.Month, _dt.Day, 17, 0, 0);
                _dtDinnerEnd = new DateTime(_dt.Year, _dt.Month, _dt.Day, 17, 40, 59);
                _dtNightStart = new DateTime(_dt.Year, _dt.Month, _dt.Day, 23, 50, 0);
                _dtNightEnd = new DateTime(_dt.Year, _dt.Month, _dt.Day, 0, 59, 59).AddDays(1);
                _dtBreakfastStart = new DateTime(_dt.Year, _dt.Month, _dt.Day, 5, 0, 0);
                _dtBreakfastEnd = new DateTime(_dt.Year, _dt.Month, _dt.Day, 5, 40, 59);

                if (_autoNG == 1)
                {
                    if (cls.isTimeBetween(_dt, _dtLunchStart, _dtLunchEnd) == true
                        || cls.isTimeBetween(_dt, _dtDinnerStart, _dtDinnerEnd) == true
                        || cls.isTimeBetween(_dt, _dtNightStart, _dtNightEnd) == true
                        || cls.isTimeBetween(_dt, _dtBreakfastStart, _dtBreakfastEnd) == true)
                    {
                        _counterDis = _counter;
                        tNG.Stop();
                        label9.Text = "ITEM";
                    }
                    else
                    {
                        tNG.Start();
                    }
                }
                else
                {
                    _counterDis = _counter;
                    tNG.Stop();
                    label9.Text = "ITEM";
                }
            }
            catch
            {

            }
            finally
            {

            }
        }

        public void init()
        {
            try
            {
                fnGetDate();
                //fnBindInit();
                Fnc_Refresh_Dispenser_Scan_No_Data();

                string idx = "", statusOK = "", statusNG = "";
                string line = "", partname = "", partcode = "", order = "";
                string ok = "", ng = "", rate = "", nextorder = "", pic = "";
                string code01 = "", code02 = "", code03 = "";

                lblStatus.Text = "";
                lblStatus.BackColor = Color.Gray;
                lblMessage.BackColor = Color.Gray;
                lblItemCode.BackColor = Color.Gray;
                lblPIC.BackColor = Color.Gray;

                lblMessage.Text = "Waiting for checking...   /   Đang chờ để kiểm tra...";
                lblMessage.ForeColor = Color.White;

                line = linenm + " " + lineno;
                partname = "N/A";
                partcode = "N/A";
                pic = "";
                order = "0";
                ok = "0";
                ng = "0";
                rate = "0%";
                code01 = "";
                code02 = "";
                code03 = "";
                nextorder = "N/A";
                _autoNG = (chkAutoNG.Checked) ? 1 : 0;
                chkAutoNG.BackColor = (_autoNG == 1) ? Color.Red : Color.OrangeRed;
                chkAutoNG.Text = (_autoNG == 1) ? "AUTO NG" : "NG";
                if (_autoNG == 1)
                {
                    fnAutoNG();
                }

                lblLine.Text = line;
                lblPartName.Text = partname + "\r\n" + partcode;
                //lblPartCode.Text = partcode;
                lblPIC.Text = pic;
                lblOrder.Text = order;
                lblOK.Text = ok;
                lblNG.Text = ng;
                lblRate.Text = rate;
                lblNextOrder.Text = nextorder;

                fnBindInit();
                fnGetNextOrder();
                fnc_RateColor();
            }
            catch
            {

            }
            finally
            {

            }
        }

        public void fnGetDate()
        {
            try
            {
                if (check.IsConnectedToInternet())
                {
                    lblDateTime.Text = cls.fnGetDate("SD") + " - " + cls.fnGetDate("CT");
                    lblDateTime.ForeColor = Color.White;
                }
                else
                {
                    lblDateTime.Text = String.Format("{0:dd/MM/yyyy}") + " - " + String.Format("{0:HH:mm:ss}");
                    lblDateTime.ForeColor = Color.Black;
                }
            }
            catch
            {

            }
            finally
            {

            }
        }

        public void fnBindInit()
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

                fnGetNextOrder();
                fnc_RateColor();
            }
            catch
            {
                
            }
            finally
            {

            }
        }

        private void txtItemCode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!(sender as TextBox).ContainsFocus)
                    return;
                DisposeTimer();
                timer = new System.Threading.Timer(new TimerCallback(TimerElapsed), (object)null, uc_CapacityScanner_v2o6.VALIDATION_DELAY, uc_CapacityScanner_v2o6.VALIDATION_DELAY);
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

        public void CheckSyntaxAndReport()
        {
            try
            {
                Invoke(new Action(() =>
                {
                    string s = txtItemCode.Text.ToUpper(); //Do everything on the UI thread itself
                    lblItemCode.Text = s;
                    ////txtItemCode.Enabled = false;

                    //fnDisplayMsg();
                    string upper = txtItemCode.Text.ToUpper();
                    string str1 = upper.Substring(0, 4);
                    //if (str1 == "NG-1" || str1 == "NG+1" || str1 == "OK-1" || str1 == "OK+1")
                    //{
                    //    //if (_statusOK >= 1 && _statusNG >= 1)
                    //    //{
                    //    //    string str2 = str1;
                    //    //    if (!(str2 == "NG-1"))
                    //    //    {
                    //    //        if (!(str2 == "NG+1"))
                    //    //        {
                    //    //            if (!(str2 == "OK-1"))
                    //    //            {
                    //    //                if (str2 == "OK+1")
                    //    //                    fnResetCapacityLine((byte)4);
                    //    //            }
                    //    //            else
                    //    //                fnResetCapacityLine((byte)3);
                    //    //        }
                    //    //        else
                    //    //            fnResetCapacityLine((byte)2);
                    //    //    }
                    //    //    else
                    //    //        fnResetCapacityLine((byte)1);
                    //    //}
                    //}
                    //else
                    //{
                    //    try
                    //    {
                    //        fnInsertDB(upper);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        int num = (int)MessageBox.Show(ex.ToString());
                    //    }
                    //    finally
                    //    {

                    //    }
                    //}
                    try
                    {
                        if (upper != _lastCode)
                        {
                            fnInsertDB(upper);
                            _lastCode = (_codeN > 1) ? upper : "";
                            fnDisplayMsg();

                            //_counterDis = 0;
                            //_counterDis = _counter;
                            //tNG.Stop();
                            //tNG.Start();

                            fnBindInit();
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

        /*************************************************/

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

        public void fnAutoNG()
        {
            //DateTime dt = DateTime.Now;
            //DateTime dtLunchStart = new DateTime(dt.Year, dt.Month, dt.Day, 11, 50, 0);
            //DateTime dtLunchEnd = new DateTime(dt.Year, dt.Month, dt.Day, 12, 59, 59);
            //DateTime dtDinnerStart = new DateTime(dt.Year, dt.Month, dt.Day, 17, 0, 0);
            //DateTime dtDinnerEnd = new DateTime(dt.Year, dt.Month, dt.Day, 17, 40, 59);
            //DateTime dtNightStart = new DateTime(dt.Year, dt.Month, dt.Day, 23, 50, 0);
            //DateTime dtNightEnd = new DateTime(dt.Year, dt.Month, dt.Day, 0, 59, 59).AddDays(1);
            //DateTime dtBreakfastStart = new DateTime(dt.Year, dt.Month, dt.Day, 5, 0, 0);
            //DateTime dtBreakfastEnd = new DateTime(dt.Year, dt.Month, dt.Day, 5, 40, 59);

            try
            {
                //tNG.Stop();
                //_counterDis = _counter;

                if (_autoNG == 1)
                {
                    //tNG.Interval = 1000;
                    //tNG.Enabled = true;
                    //tNG.Tick += new EventHandler(fnCheckAutoNG);
                    tNG.Start();
                }
                else
                {
                    label9.Text = "ITEM";
                }
            }
            catch
            {

            }
            finally
            {

            }
        }

        public void fnCheckAutoNG(object sender, EventArgs e)
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

        public void fnGetNextOrder()
        {
            try
            {
                string nextId = "", nextTime = "", nextProd = "";
                string sql = "V2o1_BASE_Capacity_Dispenser_NextOrder_SelItem_Addnew";

                SqlParameter[] sParams = new SqlParameter[1]; // Parameter count
                sParams[0] = new SqlParameter();
                sParams[0].SqlDbType = SqlDbType.Int;
                sParams[0].ParameterName = "@lineId";
                sParams[0].Value = lineId;

                DataTable dt = new DataTable();
                dt = cls.ExecuteDataTable(sql, sParams);
                if (dt.Rows.Count > 0)
                {
                    nextId = dt.Rows[0][0].ToString();
                    nextTime = "(" + dt.Rows[0][1].ToString() + ")";
                    nextProd = dt.Rows[0][2].ToString();
                }
                else
                {
                    nextId = "N/A";
                    nextTime = "";
                    nextProd = "";
                }

                lblNextOrder.Text = nextTime + " " + nextProd;
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
                    fnBindInit();
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

        public void FNC_InsertDB_02(string s)
        {
            if (s != prevCode)
            {
                switch (_codeN)
                {
                    case 1:
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
                        break;
                    case 2:
                        if (s != prevCode)
                        {

                        }
                        break;
                    case 3:
                        break;
                }

                prevCode = s;
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

        public void fnRate()
        {
            //string total = lblOrder.Text.Trim();
            string total = lblGoal.Text.Trim();
            string valOK = lblOK.Text.Trim();
            string valNG = lblNG.Text.Trim();
            decimal _total = 0, _valOK = 0, _valNG = 0;
            decimal _rate = 0;

            _total = (total != "" && total != null) ? Convert.ToInt32(total) : 0;
            _valOK = (valOK != "" && valOK != null) ? Convert.ToInt32(valOK) : 0;
            _valNG = (valNG != "" && valNG != null) ? Convert.ToInt32(valNG) : 0;

            _rate = Convert.ToDecimal(((_valOK + _valNG) * 100) / _total);

            lblRate.Text = String.Format("{0:0.0}", _rate);// + "%";
            //lblRate.Text = String.Format("{0:0}", _rate);// + "%";
            fnc_RateColor();
        }

        public void fnc_RateColor()
        {
            if (_rate >= 0 && _rate < 95)
            {
                lblRate.BackColor = label17.BackColor = Color.LightCoral;
            }
            else if (_rate >= 95 && _rate < 105)
            {
                lblRate.BackColor = label17.BackColor = Color.LightGreen;
            }
            else
            {
                lblRate.BackColor = label17.BackColor = Color.SteelBlue;
            }
        }

        public void Fnc_Select_Scan_No(string line)
        {
            try
            {
                int listCount = 0, rowCount = 0;
                string scanNo = "";
                bool _scanNo = false;
                string sql = "V2_BASE_CAPACITY_GET_DISPENSER_SCAN_NO_SELITEM_ADDNEW";

                SqlParameter[] sParams = new SqlParameter[1]; // Parameter count

                sParams[0] = new SqlParameter();
                sParams[0].SqlDbType = SqlDbType.Int;
                sParams[0].ParameterName = "@line";
                sParams[0].Value = line;

                DataSet ds = new DataSet();
                ds = cls.ExecuteDataSet(sql, sParams);
                listCount = ds.Tables.Count;
                rowCount = ds.Tables[0].Rows.Count;

                if (listCount > 0 && rowCount > 0)
                {
                    scanNo = ds.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    scanNo = "True";
                }

                _scanNo = (scanNo.ToLower() == "true") ? true : false;

                if (_scanNo == true)
                {
                    NGStatus();
                    //lblStatus.BackColor = lblMessage.BackColor = lblItemCode.BackColor = lblPIC.BackColor = Color.Red;
                    //lblStatus.ForeColor = lblMessage.ForeColor = lblItemCode.ForeColor = Color.White;

                    lblStatus.Text = "NG";
                    lblMessage.Text = "CANNOT SCAN THE QR BARCODE ON CURRENT VALVE   /   KHÔNG QUÉT ĐƯỢC MÃ VẠCH TRÊN THÂN VAN HIỆN TẠI";
                    //txtItemCode.Enabled = false;

                    _prevBarcode = "";
                }
                else
                {
                    switch (_scanStage)
                    {
                        case 2:
                            OKStatus();
                            fnDisplayMsg();
                            break;
                        case 1:
                            NGStatus();
                            fnDisplayMsg();
                            break;
                        case 0:
                            lblStatus.BackColor = lblMessage.BackColor = lblItemCode.BackColor = lblPIC.BackColor = Color.Gray;
                            lblStatus.ForeColor = lblMessage.ForeColor = lblItemCode.ForeColor = lblPIC.ForeColor = Color.White;

                            lblStatus.Text = "";
                            lblMessage.Text = "Waiting for checking...   /   Đang chờ để kiểm tra...";
                            break;
                        //default:
                        //    lblStatus.BackColor = lblMessage.BackColor = lblItemCode.BackColor = lblPIC.BackColor = Color.Gray;
                        //    lblStatus.ForeColor = lblMessage.ForeColor = lblItemCode.ForeColor = lblPIC.ForeColor = Color.White;

                        //    lblStatus.Text = "";
                        //    lblMessage.Text = "Waiting for checking...   /   Đang chờ để kiểm tra...";
                        //    break;
                    }
                }
            }
            catch
            {

            }
            finally
            {

            }
        }

        public void Fnc_Refresh_Dispenser_Scan_No_Data()
        {
            Thread loadData = new Thread(() =>
            {
                while (true)
                {
                    Fnc_Select_Scan_No(lineId);

                    Thread.Sleep(500);
                }
            });
            loadData.IsBackground = true;
            loadData.Start();

            //Thread loadDate = new Thread(() =>
            //{
            //    while (true)
            //    {
            //        //Fnc_Load_DateTime();

            //        Thread.Sleep(1000);
            //    }
            //});
            //loadDate.IsBackground = true;
            //loadDate.Start();
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

        /*************************************************/

        private void lblOK_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //fnUpdateData(lineId, linenm + "-" + lineno.Replace("0", ""), "OK", lblOK.Text.Trim(), lblNG.Text.Trim());
                fnRate();
            }
            catch
            {

            }
            finally
            {

            }

        }

        private void lblNG_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //fnUpdateData(lineId, linenm + "-" + lineno.Replace("0", ""), "NG", lblOK.Text.Trim(), lblNG.Text.Trim());
                fnRate();
            }
            catch
            {

            }
            finally
            {

            }
        }

        public void fnUpdateData(string lineID, string line, string type, string valueOK, string valueNG)
        {
            string shift = cls.fnGetDate("s").ToUpper();
            int chk = line.IndexOf("-");
            string name = line.Substring(0, chk);
            string pos = line.Substring(chk + 1);
            //string lineId = "0";


            string sql = "";
            sql = "V2_BASE_CAPACITY_GET_INS_PLC_DATA_ADDNEW";
            SqlParameter[] sParams = new SqlParameter[5]; // Parameter count

            sParams[0] = new SqlParameter();
            sParams[0].SqlDbType = SqlDbType.VarChar;
            sParams[0].ParameterName = "assyShift";
            sParams[0].Value = shift;

            sParams[1] = new SqlParameter();
            sParams[1].SqlDbType = SqlDbType.Int;
            sParams[1].ParameterName = "lineId";
            sParams[1].Value = lineID;

            sParams[2] = new SqlParameter();
            sParams[2].SqlDbType = SqlDbType.VarChar;
            sParams[2].ParameterName = "assyLine";
            sParams[2].Value = name + " 0" + pos;

            sParams[3] = new SqlParameter();
            sParams[3].SqlDbType = SqlDbType.Int;
            sParams[3].ParameterName = "valueOK";
            sParams[3].Value = valueOK;

            sParams[4] = new SqlParameter();
            sParams[4].SqlDbType = SqlDbType.Int;
            sParams[4].ParameterName = "valueNG";
            sParams[4].Value = valueNG;

            cls.fnUpdDel(sql, sParams);

            //lblMessage.ForeColor = (type == "OK") ? Color.Blue : Color.Red;
            //lblMessage.Text = "Update data for line " + name + " 0" + pos + " successfull at " + DateTime.Now + ".";
        }

        private void chkAutoNG_CheckedChanged(object sender, EventArgs e)
        {
            _autoNG = (chkAutoNG.Checked) ? 1 : 0;
            chkAutoNG.BackColor = (_autoNG == 1) ? Color.Red : Color.OrangeRed;
            chkAutoNG.Text = (_autoNG == 1) ? "AUTO NG" : "NG";

            if (_autoNG == 0)
            {
                tNG.Stop();
                label9.Text = "ITEM";
            }
            else
            {
                _counterDis = 0;
                _counterDis = _counter;
                tNG.Stop();
                tNG.Start();
                fnAutoNG();
            }
            //fnAutoNG();
            //if (_autoNG == 1)
            //{
            //    fnAutoNG();
            //}
            //else
            //{
            //    tNG.Stop();
            //    label9.Text = "ITEM";
            //}
        }
    }
}
