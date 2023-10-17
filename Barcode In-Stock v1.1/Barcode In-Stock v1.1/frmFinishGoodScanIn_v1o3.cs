using Inventory_Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Inventory_Data
{
    public partial class frmFinishGoodScanIn_v1o3 : Form
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();

        public int _dgvIN_List_Width;
        public int _dgvBind_List_Width;

        public string _partIDx = "", _partName = "", _partCode = "", _partLOT = "", _partDAY = "";
        public string _partLOC = "", _partPCS = "", _partBOX = "", _partCAR = "", _partPAL = "";
        public Boolean _status;

        public string _msgText = "";
        public int _msgType = 0;

        public string _range = "1";

        private cls.Ini ini = new cls.Ini(Application.StartupPath + "\\" + Application.ProductName + ".ini");
        public string _focus = "", _line = "";

        Timer timer = new Timer();
        System.Windows.Forms.Timer chk_Timer = new System.Windows.Forms.Timer();

        string _modelIDx = "", _pro_Code01 = "", _pro_Code02 = "", _pro_Code03 = "", _chk_Code01 = "", _chk_Code02 = "", _chk_Code03 = "", _chk_Result = "";
        string _last_Code01 = "", _last_Code02 = "", _last_Code03 = "";
        int _nCheck = 1;
        int _nCheck_Total = 0;


        public frmFinishGoodScanIn_v1o3()
        {
            InitializeComponent();

            _focus= ini.GetIniValue("PROGRAM", "FC", "1").Trim();
            _line= ini.GetIniValue("DISPENSER", "LINE", "1").Trim();

            cls.SetDoubleBuffer(dgvIN_List, true);
            cls.SetDoubleBuffer(dgvBind_List, true);
        }

        private void frmFinishGoodScanIn_v1o3_Load(object sender, EventArgs e)
        {
            _dgvIN_List_Width = cls.fnGetDataGridWidth(dgvIN_List);
            _dgvBind_List_Width = cls.fnGetDataGridWidth(dgvBind_List);

            init();

            lblIN_Name.AutoSize = true;
            //lblIN_Name.Text = "Đến với thế giới của chúng tôi là những điều tốt đẹp nhất";

            //string str01 = "5220FR2075WCBI0O58189";
            //string str02 = "5220FR2075WCBI0O50952";
            //string str03 = "5220FR2075WCBI0O72373";
            //bool samelike = cls.Fnc_Compare_Three_String(str01, str02, str03);
            //MessageBox.Show(samelike.ToString());
            //bool compare = (String.Compare(str01, str02) == 0 && String.Compare(str02, str03) == 0) ? true : false;
            //bool allEqual = !new string[] { str01, str02, str03 }.Distinct().Skip(1).Any();
            //MessageBox.Show("Compare: " + compare.ToString() + "\r\nAllEqual: " + allEqual.ToString());
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            cls.fnSetDateTime(tssDateTime);
            if (_focus == "1")
            {
                txtIN_Packing.Focus();
            }
        }

        public void init()
        {
            cls.fnSetDateTime(tssDateTime);

            initIN();
            tlpIN_Qty.Visible = (_partIDx != "" && _partIDx != null) ? true : false;
            //initBind();
        }

        private void tssMsg_TextChanged(object sender, EventArgs e)
        {
            timer.Interval = 5000;
            timer.Enabled = true;
            timer.Tick += new System.EventHandler(this.timer_Tick);
            if (tssMsg.Text.Length > 0)
            {
                timer.Start();
            }
            else
            {
                timer.Stop();
            }
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            tssMsg.Text = "";
            timer.Stop();
        }

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int sel = tabControl1.SelectedIndex;
            switch (sel)
            {
                case 0:
                    initIN();
                    break;
                case 1:
                    init_IV_Check_Load_Controls();
                    break;
                case 2:
                    Fnc_TabPage03_Load_Controls();
                    break;
            }
        }



        #region SCAN IN


        public void initIN()
        {
            txtIN_Packing.Text = "";
            txtIN_Packing.Focus();

            lblIN_Name.Text = lblIN_Code.Text = lblIN_LOT.Text = lblIN_Date.Text = lblIN_Locate.Text = "N/A";

            lblIN_PCS.BackColor = lblIN_BOX.BackColor = lblIN_CAR.BackColor = lblIN_PAL.BackColor = 
                lblIN_Name.BackColor = lblIN_Code.BackColor = lblIN_LOT.BackColor = lblIN_Date.BackColor = Color.Gainsboro;

            tlpIN_PCS.BackColor = tlpIN_BOX.BackColor = tlpIN_CAR.BackColor = tlpIN_PAL.BackColor = Color.Silver;

            txtIN_PCS.Text = txtIN_BOX.Text = txtIN_CAR.Text = txtIN_PAL.Text = "0";

            txtIN_PCS.BackColor = txtIN_BOX.BackColor = txtIN_CAR.BackColor = txtIN_PAL.BackColor = Color.Silver;

            //lblIN_Total.Text = "0";
            //lblIN_FCount.Text = "0";
            //lblIN_FSum.Text = "0";
            //lblIN_NCount.Text = "0";
            //lblIN_NSum.Text = "0";

            initIN_Qty();
        }

        public void initIN_List()
        {
            string partIDx = _partIDx;
            string partLot = _partLOT;

            string sql = "V2_BASE_Inventory_ScanIn_List_SelItem_V1o1_Addnew";
            SqlParameter[] sParams = new SqlParameter[2]; // Parameter count

            sParams[0] = new SqlParameter();
            sParams[0].SqlDbType = SqlDbType.Int;
            sParams[0].ParameterName = "@partIDx";
            sParams[0].Value = partIDx;

            sParams[1] = new SqlParameter();
            sParams[1].SqlDbType = SqlDbType.VarChar;
            sParams[1].ParameterName = "@partLOT";
            sParams[1].Value = partLot;

            DataTable dt = new DataTable();
            dt = cls.ExecuteDataTable(sql, sParams);

            _dgvIN_List_Width = cls.fnGetDataGridWidth(dgvIN_List);
            dgvIN_List.DataSource = dt;

            //dgvIN_List.Columns[0].Width = 25 * _dgvIN_List_Width / 100;    // ProdId
            dgvIN_List.Columns[1].Width = 80 * _dgvIN_List_Width / 100;    // boxcode
            dgvIN_List.Columns[2].Width = 20 * _dgvIN_List_Width / 100;    // boxquantity
            //dgvIN_List.Columns[3].Width = 25 * _dgvIN_List_Width / 100;    // IN_Stock

            dgvIN_List.Columns[0].Visible = false;
            dgvIN_List.Columns[1].Visible = true;
            dgvIN_List.Columns[2].Visible = true;
            dgvIN_List.Columns[3].Visible = false;

            cls.fnFormatDatagridview(dgvIN_List, 12, 30);
            //dgvIN_List.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        public void initIN_Qty()
        {
            string total = "", Fcount = "", Fsum = "", Ncount = "", Nsum = "";
            if (_partIDx != "" && _partIDx != null)
            {
                string prodIDx = _partIDx;
                string sql = "V2_BASE_Inventory_ScanIn_Qty_SelItem_V1o1_Addnew";

                SqlParameter[] sParams = new SqlParameter[1]; // Parameter count
                sParams[0] = new SqlParameter();
                sParams[0].SqlDbType = SqlDbType.Int;
                sParams[0].ParameterName = "@prodIDx";
                sParams[0].Value = prodIDx;

                DataSet ds = new DataSet();
                ds = cls.ExecuteDataSet(sql, sParams);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    total = ds.Tables[0].Rows[0][3].ToString();
                    Fcount = ds.Tables[0].Rows[0][4].ToString();
                    Fsum = ds.Tables[0].Rows[0][5].ToString();
                    Ncount = ds.Tables[0].Rows[0][6].ToString();
                    Nsum = ds.Tables[0].Rows[0][7].ToString();
                }
                else
                {
                    total = "0";
                    Fcount = "0";
                    Fsum = "0";
                    Ncount = "0";
                    Nsum = "0";
                }
            }
            else
            {
                total = "0";
                Fcount = "0";
                Fsum = "0";
                Ncount = "0";
                Nsum = "0";
            }

            lblIN_Total.Text = total;
            lblIN_FCount.Text = Fcount;
            lblIN_FSum.Text = Fsum;
            lblIN_NCount.Text = Ncount;
            lblIN_NSum.Text = Nsum;
        }

        private void txtIN_Packing_KeyDown(object sender, KeyEventArgs e)
        {
            string pack = txtIN_Packing.Text.Trim();
            if (e.KeyCode == Keys.Enter)
            {
                if (pack != "" && pack != null)
                {
                    if (pack.Length > 12)
                    {
                        string packType = pack.Substring(0, 3);
                        switch (packType.ToUpper())
                        {
                            case "TYP":
                                fnPackTYP(pack);
                                tlpIN_Qty.Visible = true;
                                initIN_Qty();
                                break;
                            case "PRO":
                                fnPackPRO(pack);
                                tlpIN_Qty.Visible = true;
                                initIN_Qty();
                                break;
                            case "COD":
                                fnPackCOD(pack);
                                tlpIN_Qty.Visible = false;
                                break;
                        }
                    }
                    else
                    {
                        _msgText = "Vui lòng nhập đúng loại mã kiện";
                        _msgType = 2;
                    }
                }
                else
                {
                    _msgText = "Vui lòng nhập mã kiện.";
                    _msgType = 2;
                }
                txtIN_Packing.Text = "";
                txtIN_Packing.Focus();
            }

            cls.fnMessage(tssMsg, _msgText, _msgType);
        }

        public void fnPackTYP(string pack)
        {
            string packType = pack.Substring(0, 3);
            string packCode = pack.Substring(4);
            int _part01 = packCode.IndexOf("|");
            int _part02 = packCode.IndexOf("/");
            string partName = packCode.Substring(0, _part01).ToString();
            string partCode = packCode.Substring(_part01 + 1, _part02 - (_part01 + 1)).ToString();
            string partQty = packCode.Substring(_part02 + 1).ToString();

            //MessageBox.Show(packType + "\r\n" + packCode + "\r\n" + partName + "\r\n" + partCode + "\r\n" + partQty);

            string sql = "V2_BASE_Inventory_ScanIn_PackingStandard_SelItem_V1o1_Addnew";
            SqlParameter[] sParams = new SqlParameter[1]; // Parameter count

            sParams[0] = new SqlParameter();
            sParams[0].SqlDbType = SqlDbType.VarChar;
            sParams[0].ParameterName = "@partCode";
            sParams[0].Value = partCode;

            DataSet ds = new DataSet();
            ds = cls.ExecuteDataSet(sql, sParams);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string idx = ds.Tables[0].Rows[0][0].ToString();
                    string loc = ds.Tables[0].Rows[0][1].ToString();
                    string pcs = ds.Tables[0].Rows[0][2].ToString();
                    string box = ds.Tables[0].Rows[0][3].ToString();
                    string car = ds.Tables[0].Rows[0][4].ToString();
                    string pal = ds.Tables[0].Rows[0][5].ToString();
                    string lot = cls.fnGetDate("ls");
                    string day = cls.fnGetDate("lot");

                    _partIDx = idx;
                    _partName = partName;
                    _partCode = partCode;
                    _partLOT = lot;
                    _partDAY = day;
                    _partLOC = loc;
                    _partPCS = pcs;
                    _partBOX = box;
                    _partCAR = car;
                    _partPAL = pal;

                    int _pcs = (pcs != "" && pcs != null) ? Convert.ToInt32(pcs) : 0;
                    int _box = (box != "" && box != null) ? Convert.ToInt32(box) : 0;
                    int _car = (car != "" && car != null) ? Convert.ToInt32(car) : 0;
                    int _pal = (pal != "" && pal != null) ? Convert.ToInt32(pal) : 0;

                    if (_pcs != 0 || _box != 0 || _car != 0 || _pal != 0)
                    {
                        lblIN_Name.Text = _partName;
                        lblIN_Code.Text = _partCode;
                        lblIN_LOT.Text = _partLOT;
                        lblIN_Date.Text = _partDAY;
                        lblIN_Locate.Text = _partLOC;
                        txtIN_PCS.Text = _partPCS;
                        txtIN_BOX.Text = _partBOX;
                        txtIN_CAR.Text = _partCAR;
                        txtIN_PAL.Text = _partPAL;

                        lblIN_PCS.BackColor = (_pcs != 0) ? Color.LightGreen : Color.Gainsboro;
                        lblIN_BOX.BackColor = (_box != 0) ? Color.LightGreen : Color.Gainsboro;
                        lblIN_CAR.BackColor = (_car != 0) ? Color.LightGreen : Color.Gainsboro;
                        lblIN_PAL.BackColor = (_pal != 0) ? Color.LightGreen : Color.Gainsboro;

                        tlpIN_PCS.BackColor = (_pcs != 0) ? Color.White : Color.Silver;
                        tlpIN_BOX.BackColor = (_box != 0) ? Color.White : Color.Silver;
                        tlpIN_CAR.BackColor = (_car != 0) ? Color.White : Color.Silver;
                        tlpIN_PAL.BackColor = (_pal != 0) ? Color.White : Color.Silver;

                        txtIN_PCS.BackColor = (_pcs != 0) ? Color.White : Color.Silver;
                        txtIN_BOX.BackColor = (_box != 0) ? Color.White : Color.Silver;
                        txtIN_CAR.BackColor = (_car != 0) ? Color.White : Color.Silver;
                        txtIN_PAL.BackColor = (_pal != 0) ? Color.White : Color.Silver;

                        txtIN_PCS.Enabled = (_pcs != 0) ? true : false;
                        txtIN_BOX.Enabled = (_box != 0) ? true : false;
                        txtIN_CAR.Enabled = (_car != 0) ? true : false;
                        txtIN_PAL.Enabled = (_pal != 0) ? true : false;

                        txtIN_Packing.Text = "";
                        txtIN_Packing.Focus();

                        //initIN_List();
                        initBind();
                    }
                    else
                    {
                        txtIN_Packing.Text = "";
                        //txtIN_Packing.Focus();

                        lblIN_Name.Text = "N/A";
                        lblIN_Code.Text = "N/A";
                        lblIN_LOT.Text = "N/A";
                        lblIN_Date.Text = "N/A";
                        lblIN_Locate.Text = "N/A";

                        lblIN_PCS.BackColor = Color.Gainsboro;
                        lblIN_BOX.BackColor = Color.Gainsboro;
                        lblIN_CAR.BackColor = Color.Gainsboro;
                        lblIN_PAL.BackColor = Color.Gainsboro;

                        tlpIN_PCS.BackColor = Color.Silver;
                        tlpIN_BOX.BackColor = Color.Silver;
                        tlpIN_CAR.BackColor = Color.Silver;
                        tlpIN_PAL.BackColor = Color.Silver;

                        txtIN_PCS.Text = "0";
                        txtIN_BOX.Text = "0";
                        txtIN_CAR.Text = "0";
                        txtIN_PAL.Text = "0";

                        txtIN_PCS.BackColor = Color.Silver;
                        txtIN_BOX.BackColor = Color.Silver;
                        txtIN_CAR.BackColor = Color.Silver;
                        txtIN_PAL.BackColor = Color.Silver;

                        _msgText = "Vui lòng thông báo Phòng Sản xuất cài đặt giá trị Packing Standard cho " + partCode;
                        _msgType = 2;
                    }
                }
                else
                {
                    _msgText = "Không tìm thấy mã hàng (" + partCode + ") tương ứng.";
                    _msgType = 2;
                }
            }
            else
            {
                txtIN_Packing.Text = "";
                //txtIN_Packing.Focus();

                lblIN_Name.Text = "N/A";
                lblIN_Code.Text = "N/A";
                lblIN_LOT.Text = "N/A";
                lblIN_Date.Text = "N/A";
                lblIN_Locate.Text = "N/A";

                lblIN_PCS.BackColor = Color.Gainsboro;
                lblIN_BOX.BackColor = Color.Gainsboro;
                lblIN_CAR.BackColor = Color.Gainsboro;
                lblIN_PAL.BackColor = Color.Gainsboro;

                tlpIN_PCS.BackColor = Color.Silver;
                tlpIN_BOX.BackColor = Color.Silver;
                tlpIN_CAR.BackColor = Color.Silver;
                tlpIN_PAL.BackColor = Color.Silver;

                txtIN_PCS.Text = "0";
                txtIN_BOX.Text = "0";
                txtIN_CAR.Text = "0";
                txtIN_PAL.Text = "0";

                txtIN_PCS.BackColor = Color.Silver;
                txtIN_BOX.BackColor = Color.Silver;
                txtIN_CAR.BackColor = Color.Silver;
                txtIN_PAL.BackColor = Color.Silver;

                _msgText = "Không tìm thấy mã hàng (" + partCode + ") tương ứng.";
                _msgType = 2;
            }
            txtIN_Packing.Text = "";
            txtIN_Packing.Focus();

            cls.fnMessage(tssMsg, _msgText, _msgType);
        }

        public void fnPackPRO(string pack)
        {
            string packType = pack.Substring(0, 3).ToUpper();
            string packKind = pack.Substring(4, 3).ToUpper();
            string packCode = pack.Substring(8);

            //MessageBox.Show(packType + "\r\n" + packKind + "\r\n" + packCode);

            if (packType == "PRO")
            {
                if (packKind == "PCS" || packKind == "BOX" || packKind == "CAR" || packKind == "PAL")
                {
                    bool sync = false;
                    string qty = "";
                    switch (packKind)
                    {
                        case "PCS":
                            sync = (_partPCS != "0") ? true : false;
                            qty = txtIN_PCS.Text.Trim();
                            break;
                        case "BOX":
                            sync = (_partBOX != "0") ? true : false;
                            qty = txtIN_BOX.Text.Trim();
                            break;
                        case "CAR":
                            sync = (_partCAR != "0") ? true : false;
                            qty = txtIN_CAR.Text.Trim();
                            break;
                        case "PAL":
                            sync = (_partPAL != "0") ? true : false;
                            qty = txtIN_PAL.Text.Trim();
                            break;
                    }

                    //MessageBox.Show(sync.ToString());

                    if (sync)
                    {
                        if (packCode.Length >= 5)
                        {
                            string sqlChk = "V2_BASE_Inventory_ScanIn_ExistCheck_SelItem_V1o1_Addnew";
                            SqlParameter[] sParamsChk = new SqlParameter[1]; // Parameter count

                            sParamsChk[0] = new SqlParameter();
                            sParamsChk[0].SqlDbType = SqlDbType.VarChar;
                            sParamsChk[0].ParameterName = "@packing";
                            sParamsChk[0].Value = pack;

                            DataSet ds = new DataSet();
                            ds = cls.ExecuteDataSet(sqlChk, sParamsChk);

                            if (ds.Tables[0].Rows.Count == 0)
                            {
                                if (qty != "0")
                                {
                                    string boxCode = pack;
                                    string partIDx = _partIDx;
                                    string partName = _partName;
                                    string partCode = _partCode;
                                    string partLOT = _partLOT;
                                    string partDAY = _partDAY;
                                    string partLOC = _partLOC;
                                    string partQTY = qty;

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
                                    if (partName.ToLower().Contains("dispenser"))
                                    {
                                        //frmCheckProof frm = new frmCheckProof();
                                        frmFinishGoodScanIn_v1o3_CheckProof frm = new frmFinishGoodScanIn_v1o3_CheckProof();
                                        frm.fnGetPart(boxCode, partIDx, partName, partCode, partLOT, partDAY, partLOC, partQTY);
                                        frm.ShowDialog();

                                        //if (_status)
                                        //{
                                        //    _msgText = "Nhập kho thành công.";
                                        //    _msgType = 1;
                                        //}
                                        //else
                                        //{
                                        //    _msgText = "Hệ thông phát hiện có hàng lẫn. Vui lòng kiểm tra lại toàn bộ kiện hàng.";
                                        //    _msgType = 2;
                                        //}
                                    }
                                    else
                                    {
                                        string sql = "V2_BASE_Inventory_ScanIn_List_AddItem_V1o1_Addnew";
                                        SqlParameter[] sParams = new SqlParameter[7]; // Parameter count

                                        sParams[0] = new SqlParameter();
                                        sParams[0].SqlDbType = SqlDbType.VarChar;
                                        sParams[0].ParameterName = "@packing";
                                        sParams[0].Value = pack;

                                        sParams[1] = new SqlParameter();
                                        sParams[1].SqlDbType = SqlDbType.Int;
                                        sParams[1].ParameterName = "@partIDx";
                                        sParams[1].Value = partIDx;

                                        sParams[2] = new SqlParameter();
                                        sParams[2].SqlDbType = SqlDbType.NVarChar;
                                        sParams[2].ParameterName = "@partName";
                                        sParams[2].Value = partName;

                                        sParams[3] = new SqlParameter();
                                        sParams[3].SqlDbType = SqlDbType.VarChar;
                                        sParams[3].ParameterName = "@partCode";
                                        sParams[3].Value = partCode;

                                        sParams[4] = new SqlParameter();
                                        sParams[4].SqlDbType = SqlDbType.VarChar;
                                        sParams[4].ParameterName = "@partLOT";
                                        sParams[4].Value = partLOT;

                                        sParams[5] = new SqlParameter();
                                        sParams[5].SqlDbType = SqlDbType.VarChar;
                                        sParams[5].ParameterName = "@partLOC";
                                        sParams[5].Value = partLOC;

                                        sParams[6] = new SqlParameter();
                                        sParams[6].SqlDbType = SqlDbType.Int;
                                        sParams[6].ParameterName = "@partQTY";
                                        sParams[6].Value = partQTY;

                                        cls.fnUpdDel(sql, sParams);


                                        _msgText = "Nhập kho thành công.";
                                        _msgType = 1;
                                    }

                                    ////////initIN_List();
                                    initBind_List();

                                }
                                else
                                {
                                    _msgText = "Không thể nhập kho với số lượng bằng 0.";
                                    _msgType = 2;
                                }
                                switch (packKind)
                                {
                                    case "PCS":
                                        txtIN_PCS.Text = _partPCS;
                                        break;
                                    case "BOX":
                                        txtIN_BOX.Text = _partBOX;
                                        break;
                                    case "CAR":
                                        txtIN_CAR.Text = _partCAR;
                                        break;
                                    case "PAL":
                                        txtIN_PAL.Text = _partPAL;
                                        break;
                                }
                            }
                            else
                            {
                                _msgText = "Mã kiện '" + pack + "' đang được dùng trên hệ thống và chưa xuất ra. Vui lòng kiểm tra lại.";
                                _msgType = 2;
                            }
                        }
                        else
                        {
                            _msgText = "Mã kiện không đúng định dạng. Vui lòng kiểm tra lại.";
                            _msgType = 2;
                        }
                    }
                    else
                    {
                        _msgText = "Số lượng chuẩn cho packing " + packKind + " chưa được thiết lập.";
                        _msgType = 2;
                    }
                }
                else
                {
                    _msgText = "Loại kiện bắt buộc chỉ 1 trong 4 loại: [PCS] / [BOX] / [CAR] / [PAL]";
                    _msgType = 2;
                }
            }
            else
            {
                _msgText = "Mã kiện bắt buộc phải bắt đầu bằng 'PRO'. Vui lòng kiểm tra lại.";
                _msgType = 2;
            }
        }

        public void fnGetProofStatus(Boolean status, string line, string boxCode, string partIDx, string partName, string partCode, string partLOT, string partDAY, string partLOC, string partQTY)
        {
            _status = status;
            if (status)
            {
                //string sql = "V2_BASE_Inventory_ScanIn_List_AddItem_V1o1_Addnew";
                string sql = "V2_BASE_Inventory_ScanIn_List_AddItem_V1o2_Addnew";
                SqlParameter[] sParams = new SqlParameter[8]; // Parameter count

                sParams[0] = new SqlParameter();
                sParams[0].SqlDbType = SqlDbType.VarChar;
                sParams[0].ParameterName = "@packing";
                sParams[0].Value = boxCode;

                sParams[1] = new SqlParameter();
                sParams[1].SqlDbType = SqlDbType.Int;
                sParams[1].ParameterName = "@partIDx";
                sParams[1].Value = partIDx;

                sParams[2] = new SqlParameter();
                sParams[2].SqlDbType = SqlDbType.NVarChar;
                sParams[2].ParameterName = "@partName";
                sParams[2].Value = partName;

                sParams[3] = new SqlParameter();
                sParams[3].SqlDbType = SqlDbType.VarChar;
                sParams[3].ParameterName = "@partCode";
                sParams[3].Value = partCode;

                sParams[4] = new SqlParameter();
                sParams[4].SqlDbType = SqlDbType.VarChar;
                sParams[4].ParameterName = "@partLOT";
                sParams[4].Value = partLOT;

                sParams[5] = new SqlParameter();
                sParams[5].SqlDbType = SqlDbType.VarChar;
                sParams[5].ParameterName = "@partLOC";
                sParams[5].Value = partLOC;

                sParams[6] = new SqlParameter();
                sParams[6].SqlDbType = SqlDbType.Int;
                sParams[6].ParameterName = "@partQTY";
                sParams[6].Value = partQTY;

                sParams[7] = new SqlParameter();
                sParams[7].SqlDbType = SqlDbType.Int;
                sParams[7].ParameterName = "@line";
                sParams[7].Value = line;

                cls.fnUpdDel(sql, sParams);

                _msgText = "Nhập kho thành công.";
                _msgType = 1;
            }
            else
            {
                _msgText = "Hệ thông phát hiện có hàng lẫn. Vui lòng kiểm tra lại toàn bộ kiện hàng.";
                _msgType = 2;
            }
            cls.fnMessage(tssMsg, _msgText, _msgType);
        }

        public void fnPackCOD(string pack)
        {
            _partIDx = "";
            _partName = "";
            _partCode = "";
            _partLOT = "";
            _partDAY = "";
            _partLOC = "";
            _partPCS = "";
            _partBOX = "";
            _partCAR = "";
            _partPAL = "";

            initIN();
        }

        private void txtIN_PCS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtIN_Packing.Text = "";
                txtIN_Packing.Focus();
            }
        }

        private void txtIN_BOX_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtIN_Packing.Text = "";
                txtIN_Packing.Focus();
            }
        }

        private void txtIN_CAR_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtIN_Packing.Text = "";
                txtIN_Packing.Focus();
            }
        }

        private void txtIN_PAL_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtIN_Packing.Text = "";
                txtIN_Packing.Focus();
            }
        }

        private void txtIN_PCS_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtIN_BOX_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtIN_CAR_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtIN_PAL_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtIN_PCS_TextChanged(object sender, EventArgs e)
        {
            string input = txtIN_PCS.Text.Trim();
            int _input = (input != "" && input != null) ? Convert.ToInt32(input) : 0;
            string value = _partPCS;
            int _value = Convert.ToInt32(value);
            txtIN_PCS.Text = (_input > _value) ? value : input;
        }

        private void txtIN_BOX_TextChanged(object sender, EventArgs e)
        {
            string input = txtIN_BOX.Text.Trim();
            int _input = (input != "" && input != null) ? Convert.ToInt32(input) : 0;
            string value = _partBOX;
            int _value = Convert.ToInt32(value);
            txtIN_BOX.Text = (_input > _value) ? value : input;
        }

        private void txtIN_CAR_TextChanged(object sender, EventArgs e)
        {
            string input = txtIN_CAR.Text.Trim();
            int _input = (input != "" && input != null) ? Convert.ToInt32(input) : 0;
            string value = _partCAR;
            int _value = Convert.ToInt32(value);
            txtIN_CAR.Text = (_input > _value) ? value : input;
        }

        private void txtIN_PAL_TextChanged(object sender, EventArgs e)
        {
            string input = txtIN_PAL.Text.Trim();
            int _input = (input != "" && input != null) ? Convert.ToInt32(input) : 0;
            string value = _partPAL;
            int _value = Convert.ToInt32(value);
            txtIN_PAL.Text = (_input > _value) ? value : input;
        }

        private void dgvIN_List_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

        }

        private void dgvIN_List_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                cls.fnDatagridClickCell(dgvIN_List, e);
            }
        }

        private void dgvIN_List_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {

        }


        #endregion


        #region BINDING DATA

        public void initBind()
        {
            initBind_List();
            initBind_Filter();
            fnLinkColor();
        }

        public void initBind_List()
        {
            string partIDx = _partIDx;
            string range = _range;
            string sql = "V2_BASE_Inventory_ScanIn_Bind_SelItem_V1o2_Addnew";
            SqlParameter[] sParams = new SqlParameter[2]; // Parameter count

            sParams[0] = new SqlParameter();
            sParams[0].SqlDbType = SqlDbType.Int;
            sParams[0].ParameterName = "@partIDx";
            sParams[0].Value = partIDx;

            sParams[1] = new SqlParameter();
            sParams[1].SqlDbType = SqlDbType.TinyInt;
            sParams[1].ParameterName = "@range";
            sParams[1].Value = range;

            DataTable dt = new DataTable();
            dt = cls.ExecuteDataTable(sql, sParams);

            _dgvBind_List_Width = cls.fnGetDataGridWidth(dgvBind_List);
            dgvBind_List.DataSource = dt;

            //dgvBind_List.Columns[0].Width = 25 * _dgvBind_List_Width / 100;    // ProdId
            dgvBind_List.Columns[1].Width = 22 * _dgvBind_List_Width / 100;    // boxcode
            dgvBind_List.Columns[2].Width = 16 * _dgvBind_List_Width / 100;    // boxpartname
            dgvBind_List.Columns[3].Width = 13 * _dgvBind_List_Width / 100;    // boxpartno
            dgvBind_List.Columns[4].Width = 10 * _dgvBind_List_Width / 100;    // boxsublocate
            dgvBind_List.Columns[5].Width = 13 * _dgvBind_List_Width / 100;    // packingdate
            dgvBind_List.Columns[6].Width = 5 * _dgvBind_List_Width / 100;    // uom
            dgvBind_List.Columns[7].Width = 8 * _dgvBind_List_Width / 100;    // boxquantity
            dgvBind_List.Columns[8].Width = 13 * _dgvBind_List_Width / 100;    // OUT_Date

            dgvBind_List.Columns[0].Visible = false;
            dgvBind_List.Columns[1].Visible = true;
            dgvBind_List.Columns[2].Visible = true;
            dgvBind_List.Columns[3].Visible = true;
            dgvBind_List.Columns[4].Visible = true;
            dgvBind_List.Columns[5].Visible = true;
            dgvBind_List.Columns[6].Visible = true;
            dgvBind_List.Columns[7].Visible = true;
            dgvBind_List.Columns[8].Visible = true;

            dgvBind_List.Columns[5].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
            dgvBind_List.Columns[8].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";

            cls.fnFormatDatagridview(dgvBind_List, 12, 30);
            //dgvBind_List.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            if(dgvBind_List.Rows.Count > 0)
            {
                lnkBind_Today.Enabled = true;
                lnkBind_3Days.Enabled = true;
                lnkBind_10Days.Enabled = true;
                lnkBind_2Weeks.Enabled = true;
                lnkBind_3Months.Enabled = true;
                lnkBind_6Months.Enabled = true;
                lnkBind_9Months.Enabled = true;
                lnkBind_1Year.Enabled = true;

                cbbBind_Filter.Enabled = true;
            }
            else
            {
                lnkBind_Today.Enabled = false;
                lnkBind_3Days.Enabled = false;
                lnkBind_10Days.Enabled = false;
                lnkBind_2Weeks.Enabled = false;
                lnkBind_3Months.Enabled = false;
                lnkBind_6Months.Enabled = false;
                lnkBind_9Months.Enabled = false;
                lnkBind_1Year.Enabled = false;

                cbbBind_Filter.Enabled = false;
            }
        }

        private void dgvBind_List_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

        }

        private void dgvBind_List_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                cls.fnDatagridClickCell(dgvBind_List, e);
            }
        }

        private void dgvBind_List_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        public void initBind_Filter()
        {
            cbbBind_Filter.Items.Clear();
            cbbBind_Filter.Items.Add("Mã kiện");
            cbbBind_Filter.Items.Add("Tên hàng");
            cbbBind_Filter.Items.Add("Mã hàng");
            cbbBind_Filter.Items.Insert(0, "");
            cbbBind_Filter.SelectedIndex = 0;

        }

        private void cbbBind_Filter_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbbBind_Filter.SelectedIndex > 0)
            {
                txtBind_Filter.Enabled = true;
                txtBind_Filter.Focus();
            }
            else
            {
                txtBind_Filter.Text = "";
                txtBind_Filter.Enabled = false;
            }
        }

        private void txtBind_Filter_TextChanged(object sender, EventArgs e)
        {

        }

        private void lnkBind_Today_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            dgvBind_List.DataSource = "";
            dgvBind_List.Refresh();
            _range = "1";
            initBind_List();
            fnLinkColor();
        }

        private void lnkBind_3Days_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            dgvBind_List.DataSource = "";
            dgvBind_List.Refresh();
            _range = "2";
            initBind_List();
            fnLinkColor();
        }

        private void lnkBind_10Days_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            dgvBind_List.DataSource = "";
            dgvBind_List.Refresh();
            _range = "3";
            initBind_List();
            fnLinkColor();
        }

        private void lnkBind_2Weeks_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            dgvBind_List.DataSource = "";
            dgvBind_List.Refresh();
            _range = "4";
            initBind_List();
            fnLinkColor();
        }

        private void lnkBind_3Months_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            dgvBind_List.DataSource = "";
            dgvBind_List.Refresh();
            _range = "5";
            initBind_List();
            fnLinkColor();
        }

        private void lnkBind_6Months_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            dgvBind_List.DataSource = "";
            dgvBind_List.Refresh();
            _range = "6";
            initBind_List();
            fnLinkColor();
        }

        private void lnkBind_9Months_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            dgvBind_List.DataSource = "";
            dgvBind_List.Refresh();
            _range = "7";
            initBind_List();
            fnLinkColor();
        }

        private void lnkBind_1Year_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            dgvBind_List.DataSource = "";
            dgvBind_List.Refresh();
            _range = "8";
            initBind_List();
            fnLinkColor();
        }

        public void fnLinkColor()
        {
            switch (_range)
            {
                case "1":
                    lnkBind_Today.LinkColor = Color.Red;
                    lnkBind_3Days.LinkColor = Color.Blue;
                    lnkBind_10Days.LinkColor = Color.Blue;
                    lnkBind_2Weeks.LinkColor = Color.Blue;
                    lnkBind_3Months.LinkColor = Color.Blue;
                    lnkBind_6Months.LinkColor = Color.Blue;
                    lnkBind_9Months.LinkColor = Color.Blue;
                    lnkBind_1Year.LinkColor = Color.Blue;
                    break;
                case "2":
                    lnkBind_Today.LinkColor = Color.Blue;
                    lnkBind_3Days.LinkColor = Color.Red;
                    lnkBind_10Days.LinkColor = Color.Blue;
                    lnkBind_2Weeks.LinkColor = Color.Blue;
                    lnkBind_3Months.LinkColor = Color.Blue;
                    lnkBind_6Months.LinkColor = Color.Blue;
                    lnkBind_9Months.LinkColor = Color.Blue;
                    lnkBind_1Year.LinkColor = Color.Blue;
                    break;
                case "3":
                    lnkBind_Today.LinkColor = Color.Blue;
                    lnkBind_3Days.LinkColor = Color.Blue;
                    lnkBind_10Days.LinkColor = Color.Red;
                    lnkBind_2Weeks.LinkColor = Color.Blue;
                    lnkBind_3Months.LinkColor = Color.Blue;
                    lnkBind_6Months.LinkColor = Color.Blue;
                    lnkBind_9Months.LinkColor = Color.Blue;
                    lnkBind_1Year.LinkColor = Color.Blue;
                    break;
                case "4":
                    lnkBind_Today.LinkColor = Color.Blue;
                    lnkBind_3Days.LinkColor = Color.Blue;
                    lnkBind_10Days.LinkColor = Color.Blue;
                    lnkBind_2Weeks.LinkColor = Color.Red;
                    lnkBind_3Months.LinkColor = Color.Blue;
                    lnkBind_6Months.LinkColor = Color.Blue;
                    lnkBind_9Months.LinkColor = Color.Blue;
                    lnkBind_1Year.LinkColor = Color.Blue;
                    break;
                case "5":
                    lnkBind_Today.LinkColor = Color.Blue;
                    lnkBind_3Days.LinkColor = Color.Blue;
                    lnkBind_10Days.LinkColor = Color.Blue;
                    lnkBind_2Weeks.LinkColor = Color.Blue;
                    lnkBind_3Months.LinkColor = Color.Red;
                    lnkBind_6Months.LinkColor = Color.Blue;
                    lnkBind_9Months.LinkColor = Color.Blue;
                    lnkBind_1Year.LinkColor = Color.Blue;
                    break;
                case "6":
                    lnkBind_Today.LinkColor = Color.Blue;
                    lnkBind_3Days.LinkColor = Color.Blue;
                    lnkBind_10Days.LinkColor = Color.Blue;
                    lnkBind_2Weeks.LinkColor = Color.Blue;
                    lnkBind_3Months.LinkColor = Color.Blue;
                    lnkBind_6Months.LinkColor = Color.Red;
                    lnkBind_9Months.LinkColor = Color.Blue;
                    lnkBind_1Year.LinkColor = Color.Blue;
                    break;
                case "7":
                    lnkBind_Today.LinkColor = Color.Blue;
                    lnkBind_3Days.LinkColor = Color.Blue;
                    lnkBind_10Days.LinkColor = Color.Blue;
                    lnkBind_2Weeks.LinkColor = Color.Blue;
                    lnkBind_3Months.LinkColor = Color.Blue;
                    lnkBind_6Months.LinkColor = Color.Blue;
                    lnkBind_9Months.LinkColor = Color.Red;
                    lnkBind_1Year.LinkColor = Color.Blue;
                    break;
                case "8":
                    lnkBind_Today.LinkColor = Color.Blue;
                    lnkBind_3Days.LinkColor = Color.Blue;
                    lnkBind_10Days.LinkColor = Color.Blue;
                    lnkBind_2Weeks.LinkColor = Color.Blue;
                    lnkBind_3Months.LinkColor = Color.Blue;
                    lnkBind_6Months.LinkColor = Color.Blue;
                    lnkBind_9Months.LinkColor = Color.Blue;
                    lnkBind_1Year.LinkColor = Color.Red;
                    break;
                default:
                    lnkBind_Today.LinkColor = Color.Red;
                    lnkBind_3Days.LinkColor = Color.Blue;
                    lnkBind_10Days.LinkColor = Color.Blue;
                    lnkBind_2Weeks.LinkColor = Color.Blue;
                    lnkBind_3Months.LinkColor = Color.Blue;
                    lnkBind_6Months.LinkColor = Color.Blue;
                    lnkBind_9Months.LinkColor = Color.Blue;
                    lnkBind_1Year.LinkColor = Color.Blue;
                    break;
            }
        }


        #endregion

        #region I/V CHECK CODE


        public void init_IV_Check_Load_Controls()
        {
            _modelIDx = _pro_Code01 = _pro_Code02 = _pro_Code03 = _chk_Code01 = _chk_Code02 = _chk_Code03 = _chk_Result = "";
            _last_Code01 = _last_Code02 = _last_Code03 = "";

            lbl_Product.Text =
                lbl_Code_01_Status.Text =
                lbl_Code_01.Text =
                lbl_Code_02_Status.Text =
                lbl_Code_02.Text = "-";
            lbl_Product.BackColor =
                lbl_Code_01_Status.BackColor =
                lbl_Code_01.BackColor =
                lbl_Code_02_Status.BackColor =
                lbl_Code_02.BackColor = Color.Gainsboro;
            rdb_Line_01.Checked = rdb_Line_01.Enabled = 
                rdb_Line_02.Checked = rdb_Line_02.Enabled = false;
            txt_Input.Text = "";
            txt_Input.Focus();
            dgv_Checked.DataSource = null;

            //string sLong = "123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            //string sShort = "89ABC";
            //string sStatus = (sLong.Contains(sShort)) ? "Contain" : "Not contain";
            //MessageBox.Show(sStatus);
        }

        private void Txt_Input_KeyDown(object sender, KeyEventArgs e)
        {
            string pack = "", packType = "", line = "";
            if (e.KeyCode == Keys.Enter)
            {
                pack = txt_Input.Text.Trim();
                if (pack != "" && pack != null)
                {
                    packType = pack.Substring(0, 3);

                    switch (packType)
                    {
                        case "TYP":
                            Fnc_IV_Check_Model_Load(pack);
                            break;
                        default:
                            if (rdb_Line_01.Checked || rdb_Line_02.Checked)
                            {
                                line = (rdb_Line_01.Checked) ? "1" : "2";
                                Fnc_IV_Check_Model_Code(pack, line);
                            }
                            else
                            {
                                MessageBox.Show("Chọn chuyền sản xuất trước khi kiểm tra mã van", cls.appName(), MessageBoxButtons.OK, MessageBoxIcon.Question);
                            }
                            break;
                    }

                }
                else
                {
                    _msgText = "Quét mã QR-code để bắt đầu kiểm tra";
                    _msgType = 2;
                }

                txt_Input.Text = "";
                txt_Input.Focus();

                cls.fnMessage(tssMsg, _msgText, _msgType);
            }
        }

        public void Fnc_IV_Check_Model_Load(string pack)
        {
            string packCode = "", partName = "", partCode = "", partQty = "";
            string modelIDx = "", valveCode01 = "", valveCode02 = "", valveCode03 = "";
            int _part01 = 0, _part02 = 0, listCount = 0, rowCount = 0, valveCode = 0;

            _nCheck = 1;
            packCode = pack.Substring(4);
            _part01 = packCode.IndexOf("|");
            _part02 = packCode.IndexOf("/");
            partName = packCode.Substring(0, _part01).ToString();
            partCode = packCode.Substring(_part01 + 1, _part02 - (_part01 + 1)).ToString();
            partQty = packCode.Substring(_part02 + 1).ToString();

            string sql = "V2_BASE_Inventory_ScanIn_IV_Check_Model_SelItem_V1o1_Addnew";

            SqlParameter[] sParams = new SqlParameter[1]; // Parameter count

            sParams[0] = new SqlParameter();
            sParams[0].SqlDbType = SqlDbType.VarChar;
            sParams[0].ParameterName = "@partCode";
            sParams[0].Value = partCode;

            DataSet ds = new DataSet();
            ds = cls.ExecuteDataSet(sql, sParams);
            listCount = ds.Tables.Count;
            rowCount = ds.Tables[0].Rows.Count;

            if (listCount > 0 && rowCount > 0)
            {
                _modelIDx = modelIDx = ds.Tables[0].Rows[0][0].ToString();
                _pro_Code01 = valveCode01 = ds.Tables[0].Rows[0][3].ToString();
                _pro_Code02 = valveCode02 = ds.Tables[0].Rows[0][4].ToString();
                _pro_Code03 = valveCode03 = ds.Tables[0].Rows[0][5].ToString();

                lbl_Product.BackColor =
                    lbl_Code_01_Status.BackColor =
                    lbl_Code_01.BackColor =
                    lbl_Code_02_Status.BackColor =
                    lbl_Code_02.BackColor = Color.White;

                rdb_Line_01.Checked = rdb_Line_02.Checked = false;
                rdb_Line_01.Enabled = rdb_Line_02.Enabled = true;

                _msgText = "Bắt đầu quét mã QR-code trên thân valve";
                _msgType = 1;
            }
            else
            {
                valveCode01 = valveCode02 = valveCode03 = "-";

                lbl_Product.BackColor = 
                    lbl_Code_01_Status.BackColor = 
                    lbl_Code_01.BackColor = 
                    lbl_Code_02_Status.BackColor = 
                    lbl_Code_02.BackColor = Color.Gainsboro;

                rdb_Line_01.Checked = rdb_Line_02.Checked = false;
                rdb_Line_01.Enabled = rdb_Line_02.Enabled = false;

                _msgText = "Không tìm thấy mã QR-code tương ứng của sản phẩm. Thử lại với mã sản phẩm khác hoặc thông báo cho nhân viên sản xuất.";
                _msgType = 2;

                MessageBox.Show(_msgText.Replace(". ", "\r\n"));
            }

            lbl_Product.Text = (listCount > 0 && rowCount > 0) ? partName : "-";

            lbl_Code_01.Text = valveCode01;
            lbl_Code_02.Text = valveCode02;

            lbl_Code_01.BackColor = (valveCode01 != "") ? Color.White : Color.Gainsboro;
            lbl_Code_02.BackColor = (valveCode02 != "") ? Color.White : Color.Gainsboro;

            lbl_Code_01_Status.Text = lbl_Code_02_Status.Text = "-";
            lbl_Code_01_Status.BackColor = lbl_Code_02_Status.BackColor = Color.Gainsboro;

            _nCheck_Total = (lbl_Code_02.Text != "") ? 2 : 1;
            Fnc_IV_Check_Model_List_Code();

            txt_Input.Text = "";
            txt_Input.Focus();

            cls.fnMessage(tssMsg, _msgText, _msgType);
        }

        public void Fnc_IV_Check_Model_Code(string code)
        {
            string code01 = "", code02 = "", code03 = "";
            int nCode = 0, nCheck = 0;

            code01 = lbl_Code_01.Text;
            code02 = lbl_Code_02.Text;

            if (code01.Length > 0) { nCode = 1; }
            if (code01.Length > 0 && code02.Length > 0) { nCode = 2; }

            if (nCode > 1)
            {

                //if (_nCheck == 1)
                //{
                //    _chk_Code01 = code;
                //    lbl_Code_01_Status.Text = (code.Contains(code01)) ? "OK" : "NG";
                //    lbl_Code_01.BackColor = lbl_Code_01_Status.BackColor = (code.Contains(code01)) ? Color.LightGreen : Color.LightPink;

                //    //_nCheck = _nCheck + 1;
                //}
                //else if (_nCheck == 2)
                //{
                //    _chk_Code02 = code;
                //    lbl_Code_02_Status.Text = (code.Contains(code02)) ? "OK" : "NG";
                //    lbl_Code_02.BackColor = lbl_Code_02_Status.BackColor = (code.Contains(code02)) ? Color.LightGreen : Color.LightPink;
                //}

                //_nCheck = _nCheck + 1;


                switch (_nCheck)
                {
                    case 1:
                        if (code != _last_Code01)
                        {
                            _chk_Code01 = _last_Code01 = code;
                            lbl_Code_01_Status.Text = (code.Contains(code01)) ? "OK" : "NG";
                            lbl_Code_01.BackColor = lbl_Code_01_Status.BackColor = (code.Contains(code01)) ? Color.LightGreen : Color.LightPink;
                        }
                        else
                        {
                            MessageBox.Show("Mã valve này vừa kiểm tra rồi");
                        }
                        break;
                    case 2:
                        if (code != _last_Code02)
                        {
                            _chk_Code02 = _last_Code02 = code;
                            lbl_Code_02_Status.Text = (code.Contains(code02)) ? "OK" : "NG";
                            lbl_Code_02.BackColor = lbl_Code_02_Status.BackColor = (code.Contains(code02)) ? Color.LightGreen : Color.LightPink;
                        }
                        else
                        {
                            MessageBox.Show("Mã valve này vừa kiểm tra rồi");
                        }
                        break;
                    case 3:
                        if (code != _last_Code03)
                        {
                            _chk_Code03 = _last_Code03 = code;
                        }
                        else
                        {
                            MessageBox.Show("Mã valve này vừa kiểm tra rồi");
                        }
                        break;
                }


                if (_nCheck == nCode)
                {
                    lbl_Product.BackColor = (lbl_Code_01_Status.Text == "OK" && lbl_Code_02_Status.Text == "OK") ? Color.LightGreen : Color.LightPink;
                    _chk_Result = (lbl_Product.BackColor == Color.LightGreen) ? "True" : "False";
                    _nCheck = 1;

                    Fnc_IV_Check_Color_To_Status();
                    Fnc_IV_Check_Model_Save_Code();
                    Fnc_IV_Check_Model_List_Code();
                }
                else
                {
                    lbl_Product.BackColor = Color.White;
                    _nCheck = _nCheck + 1;
                }
            }
            else
            {
                if (code != _last_Code01)
                {
                    lbl_Code_01_Status.Text = (code.Contains(code01)) ? "OK" : "NG";
                    lbl_Code_01.BackColor = lbl_Code_01_Status.BackColor = (code.Contains(code01)) ? Color.LightGreen : Color.LightPink;

                    lbl_Product.BackColor = (lbl_Code_01_Status.Text == "OK") ? Color.LightGreen : Color.LightPink;
                    _chk_Result = (lbl_Product.BackColor == Color.LightGreen) ? "True" : "False";
                    _nCheck = 1;

                    _chk_Code01 = _last_Code01 = code;

                    Fnc_IV_Check_Color_To_Status();
                    Fnc_IV_Check_Model_Save_Code();
                    Fnc_IV_Check_Model_List_Code();
                }
            }

        }

        public void Fnc_IV_Check_Model_Code(string code,string line)
        {
            string code01 = "", code02 = "", code03 = "";
            int nCode = 0, nCheck = 0;

            code01 = lbl_Code_01.Text;
            code02 = lbl_Code_02.Text;

            if (code01.Length > 0) { nCode = 1; }
            if (code01.Length > 0 && code02.Length > 0) { nCode = 2; }

            if (nCode > 1)
            {

                //if (_nCheck == 1)
                //{
                //    _chk_Code01 = code;
                //    lbl_Code_01_Status.Text = (code.Contains(code01)) ? "OK" : "NG";
                //    lbl_Code_01.BackColor = lbl_Code_01_Status.BackColor = (code.Contains(code01)) ? Color.LightGreen : Color.LightPink;

                //    //_nCheck = _nCheck + 1;
                //}
                //else if (_nCheck == 2)
                //{
                //    _chk_Code02 = code;
                //    lbl_Code_02_Status.Text = (code.Contains(code02)) ? "OK" : "NG";
                //    lbl_Code_02.BackColor = lbl_Code_02_Status.BackColor = (code.Contains(code02)) ? Color.LightGreen : Color.LightPink;
                //}

                //_nCheck = _nCheck + 1;


                switch (_nCheck)
                {
                    case 1:
                        if (code != _last_Code01)
                        {
                            _chk_Code01 = _last_Code01 = code;
                            lbl_Code_01_Status.Text = (code.Contains(code01)) ? "OK" : "NG";
                            lbl_Code_01.BackColor = lbl_Code_01_Status.BackColor = (code.Contains(code01)) ? Color.LightGreen : Color.LightPink;
                        }
                        else
                        {
                            MessageBox.Show("Mã valve này vừa kiểm tra rồi");
                        }
                        break;
                    case 2:
                        if (code != _last_Code02)
                        {
                            _chk_Code02 = _last_Code02 = code;
                            lbl_Code_02_Status.Text = (code.Contains(code02)) ? "OK" : "NG";
                            lbl_Code_02.BackColor = lbl_Code_02_Status.BackColor = (code.Contains(code02)) ? Color.LightGreen : Color.LightPink;
                        }
                        else
                        {
                            MessageBox.Show("Mã valve này vừa kiểm tra rồi");
                        }
                        break;
                    case 3:
                        if (code != _last_Code03)
                        {
                            _chk_Code03 = _last_Code03 = code;
                        }
                        else
                        {
                            MessageBox.Show("Mã valve này vừa kiểm tra rồi");
                        }
                        break;
                }


                if (_nCheck == nCode)
                {
                    lbl_Product.BackColor = (lbl_Code_01_Status.Text == "OK" && lbl_Code_02_Status.Text == "OK") ? Color.LightGreen : Color.LightPink;
                    _chk_Result = (lbl_Product.BackColor == Color.LightGreen) ? "True" : "False";
                    _nCheck = 1;

                    Fnc_IV_Check_Color_To_Status();
                    Fnc_IV_Check_Model_Save_Code(line);
                    Fnc_IV_Check_Model_List_Code();
                }
                else
                {
                    lbl_Product.BackColor = Color.White;
                    _nCheck = _nCheck + 1;
                }
            }
            else
            {
                if (code != _last_Code01)
                {
                    lbl_Code_01_Status.Text = (code.Contains(code01)) ? "OK" : "NG";
                    lbl_Code_01.BackColor = lbl_Code_01_Status.BackColor = (code.Contains(code01)) ? Color.LightGreen : Color.LightPink;

                    lbl_Product.BackColor = (lbl_Code_01_Status.Text == "OK") ? Color.LightGreen : Color.LightPink;
                    _chk_Result = (lbl_Product.BackColor == Color.LightGreen) ? "True" : "False";
                    _nCheck = 1;

                    _chk_Code01 = _last_Code01 = code;

                    Fnc_IV_Check_Color_To_Status();
                    Fnc_IV_Check_Model_Save_Code(line);
                    Fnc_IV_Check_Model_List_Code();
                }
            }

        }

        public void Fnc_IV_Check_Model_Save_Code()
        {
            try
            {
                string line = "", prodIDx = "", chk_result = "";
                string pro_code01 = "", pro_code02 = "", pro_code03 = "";
                string chk_code01 = "", chk_code02 = "", chk_code03 = "";

                //line = _line;
                prodIDx = _modelIDx;
                pro_code01 = _pro_Code01;
                pro_code02 = _pro_Code02;
                pro_code03 = _pro_Code03;
                chk_code01 = _chk_Code01;
                chk_code02 = _chk_Code02;
                chk_code03 = _chk_Code03;
                chk_result = _chk_Result;

                string msg = "";
                msg += "line: " + line + "\r\n";
                msg += "prodIDx: " + prodIDx + "\r\n";
                msg += "pro_code01: " + pro_code01 + "\r\n";
                msg += "pro_code02: " + pro_code02 + "\r\n";
                msg += "pro_code03: " + pro_code03 + "\r\n";
                msg += "chk_code01: " + chk_code01 + "\r\n";
                msg += "chk_code02: " + chk_code02 + "\r\n";
                msg += "chk_code03: " + chk_code03 + "\r\n";
                msg += "chk_result: " + chk_result + "\r\n";
                //MessageBox.Show(msg);
                //return;

                string sql = "V2_BASE_Inventory_ScanIn_IV_Check_Model_Code_AddItem_V1o1_Addnew";

                SqlParameter[] sParams = new SqlParameter[9]; // Parameter count

                sParams[0] = new SqlParameter();
                sParams[0].SqlDbType = SqlDbType.TinyInt;
                sParams[0].ParameterName = "@line";
                sParams[0].Value = line;

                sParams[1] = new SqlParameter();
                sParams[1].SqlDbType = SqlDbType.Int;
                sParams[1].ParameterName = "@prodIDx";
                sParams[1].Value = prodIDx;

                sParams[2] = new SqlParameter();
                sParams[2].SqlDbType = SqlDbType.VarChar;
                sParams[2].ParameterName = "@pro_Code01";
                sParams[2].Value = pro_code01;

                sParams[3] = new SqlParameter();
                sParams[3].SqlDbType = SqlDbType.VarChar;
                sParams[3].ParameterName = "@pro_Code02";
                sParams[3].Value = pro_code02;

                sParams[4] = new SqlParameter();
                sParams[4].SqlDbType = SqlDbType.VarChar;
                sParams[4].ParameterName = "@pro_Code03";
                sParams[4].Value = pro_code03;

                sParams[5] = new SqlParameter();
                sParams[5].SqlDbType = SqlDbType.VarChar;
                sParams[5].ParameterName = "@chk_Code01";
                sParams[5].Value = chk_code01;

                sParams[6] = new SqlParameter();
                sParams[6].SqlDbType = SqlDbType.VarChar;
                sParams[6].ParameterName = "@chk_Code02";
                sParams[6].Value = chk_code02;

                sParams[7] = new SqlParameter();
                sParams[7].SqlDbType = SqlDbType.VarChar;
                sParams[7].ParameterName = "@chk_Code03";
                sParams[7].Value = chk_code03;

                sParams[8] = new SqlParameter();
                sParams[8].SqlDbType = SqlDbType.Bit;
                sParams[8].ParameterName = "@chk_Result";
                sParams[8].Value = chk_result;

                cls.fnUpdDel(sql, sParams);

                _msgText = "Thêm mã kiểm tra thành công";
                _msgType = 1;
            }
            catch (SqlException sqlEx)
            {
                _msgText = "Có lỗi dữ liệu phát sinh, vui lòng báo cho người quản trị hệ thống biết về nội dung hộp thoại đang hiện";
                _msgType = 3;

                MessageBox.Show(sqlEx.Message);
            }
            catch(Exception ex)
            {
                _msgText = "Có lỗi phát sinh, vui lòng báo cho người quản trị hệ thống biết về nội dung hộp thoại đang hiện";
                _msgType = 2;

                MessageBox.Show(ex.Message);
            }
            finally
            {
                cls.fnMessage(tssMsg, _msgText, _msgType);
            }
        }

        public void Fnc_IV_Check_Model_Save_Code(string line)
        {
            try
            {
                string prodIDx = "", chk_result = "";
                string pro_code01 = "", pro_code02 = "", pro_code03 = "";
                string chk_code01 = "", chk_code02 = "", chk_code03 = "";

                //line = _line;
                prodIDx = _modelIDx;
                pro_code01 = _pro_Code01;
                pro_code02 = _pro_Code02;
                pro_code03 = _pro_Code03;
                chk_code01 = _chk_Code01;
                chk_code02 = _chk_Code02;
                chk_code03 = _chk_Code03;
                chk_result = _chk_Result;

                string msg = "";
                msg += "line: " + line + "\r\n";
                msg += "prodIDx: " + prodIDx + "\r\n";
                msg += "pro_code01: " + pro_code01 + "\r\n";
                msg += "pro_code02: " + pro_code02 + "\r\n";
                msg += "pro_code03: " + pro_code03 + "\r\n";
                msg += "chk_code01: " + chk_code01 + "\r\n";
                msg += "chk_code02: " + chk_code02 + "\r\n";
                msg += "chk_code03: " + chk_code03 + "\r\n";
                msg += "chk_result: " + chk_result + "\r\n";
                //MessageBox.Show(msg);
                //return;

                string sql = "V2_BASE_Inventory_ScanIn_IV_Check_Model_Code_AddItem_V1o1_Addnew";

                SqlParameter[] sParams = new SqlParameter[9]; // Parameter count

                sParams[0] = new SqlParameter();
                sParams[0].SqlDbType = SqlDbType.TinyInt;
                sParams[0].ParameterName = "@line";
                sParams[0].Value = line;

                sParams[1] = new SqlParameter();
                sParams[1].SqlDbType = SqlDbType.Int;
                sParams[1].ParameterName = "@prodIDx";
                sParams[1].Value = prodIDx;

                sParams[2] = new SqlParameter();
                sParams[2].SqlDbType = SqlDbType.VarChar;
                sParams[2].ParameterName = "@pro_Code01";
                sParams[2].Value = pro_code01;

                sParams[3] = new SqlParameter();
                sParams[3].SqlDbType = SqlDbType.VarChar;
                sParams[3].ParameterName = "@pro_Code02";
                sParams[3].Value = pro_code02;

                sParams[4] = new SqlParameter();
                sParams[4].SqlDbType = SqlDbType.VarChar;
                sParams[4].ParameterName = "@pro_Code03";
                sParams[4].Value = pro_code03;

                sParams[5] = new SqlParameter();
                sParams[5].SqlDbType = SqlDbType.VarChar;
                sParams[5].ParameterName = "@chk_Code01";
                sParams[5].Value = chk_code01;

                sParams[6] = new SqlParameter();
                sParams[6].SqlDbType = SqlDbType.VarChar;
                sParams[6].ParameterName = "@chk_Code02";
                sParams[6].Value = chk_code02;

                sParams[7] = new SqlParameter();
                sParams[7].SqlDbType = SqlDbType.VarChar;
                sParams[7].ParameterName = "@chk_Code03";
                sParams[7].Value = chk_code03;

                sParams[8] = new SqlParameter();
                sParams[8].SqlDbType = SqlDbType.Bit;
                sParams[8].ParameterName = "@chk_Result";
                sParams[8].Value = chk_result;

                cls.fnUpdDel(sql, sParams);

                _msgText = "Thêm mã kiểm tra thành công";
                _msgType = 1;
            }
            catch (SqlException sqlEx)
            {
                _msgText = "Có lỗi dữ liệu phát sinh, vui lòng báo cho người quản trị hệ thống biết về nội dung hộp thoại đang hiện";
                _msgType = 3;

                MessageBox.Show(sqlEx.Message);
            }
            catch (Exception ex)
            {
                _msgText = "Có lỗi phát sinh, vui lòng báo cho người quản trị hệ thống biết về nội dung hộp thoại đang hiện";
                _msgType = 2;

                MessageBox.Show(ex.Message);
            }
            finally
            {
                cls.fnMessage(tssMsg, _msgText, _msgType);
            }
        }

        public void Fnc_IV_Check_Model_List_Code()
        {
            try
            {
                string line = _line;
                string prodIDx = _modelIDx, status = "";
                bool _status = true;
                int listCount = 0;

                string sql = "V2_BASE_Inventory_ScanIn_IV_Check_Model_Code_SelItem_V1o1_Addnew";

                SqlParameter[] sParams = new SqlParameter[2]; // Parameter count

                sParams[0] = new SqlParameter();
                sParams[0].SqlDbType = SqlDbType.TinyInt;
                sParams[0].ParameterName = "@line";
                sParams[0].Value = line;

                sParams[1] = new SqlParameter();
                sParams[1].SqlDbType = SqlDbType.Int;
                sParams[1].ParameterName = "@prodIDx";
                sParams[1].Value = prodIDx;

                DataTable dt = new DataTable();
                dt = cls.ExecuteDataTable(sql, sParams);
                listCount = dt.Rows.Count;

                int dgv_Checked_Width = cls.fnGetDataGridWidth(dgv_Checked);
                dgv_Checked.DataSource = dt;

                dgv_Checked.Columns[0].Width = 10 * dgv_Checked_Width / 100;    // No.
                //dgv_Checked.Columns[1].Width = 5 * dgv_Checked_Width / 100;    // [line].
                //dgv_Checked.Columns[2].Width = 5 * dgv_Checked_Width / 100;    // [prodIDx].
                //dgv_Checked.Columns[3].Width = 20 * dgv_Checked_Width / 100;    // Name.
                //dgv_Checked.Columns[4].Width = 30 * dgv_Checked_Width / 100;    // BarCode.
                //dgv_Checked.Columns[5].Width = 5 * dgv_Checked_Width / 100;    // [pro_Code01].
                dgv_Checked.Columns[6].Width = 250;    // [chk_Code01].
                //dgv_Checked.Columns[7].Width = 15 * dgv_Checked_Width / 100;    // [pro_Code02].
                dgv_Checked.Columns[8].Width = 250;    // [chk_Code02].
                //dgv_Checked.Columns[9].Width = 5 * dgv_Checked_Width / 100;    // [pro_Code03].
                //dgv_Checked.Columns[10].Width = 5 * dgv_Checked_Width / 100;    // [chk_Code03].
                //dgv_Checked.Columns[11].Width = 5 * dgv_Checked_Width / 100;    // [chk_Result].
                dgv_Checked.Columns[12].Width = 150;    // added.

                dgv_Checked.Columns[0].Visible = true;
                dgv_Checked.Columns[1].Visible = false;
                dgv_Checked.Columns[2].Visible = false;
                dgv_Checked.Columns[3].Visible = false;
                dgv_Checked.Columns[4].Visible = false;
                dgv_Checked.Columns[5].Visible = false;
                dgv_Checked.Columns[6].Visible = true;
                dgv_Checked.Columns[7].Visible = false;
                dgv_Checked.Columns[8].Visible = (_nCheck_Total > 1) ? true : false;
                dgv_Checked.Columns[9].Visible = false;
                dgv_Checked.Columns[10].Visible = false;
                dgv_Checked.Columns[11].Visible = false;
                dgv_Checked.Columns[12].Visible = true;

                dgv_Checked.Columns[12].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";

                cls.fnFormatDatagridview(dgv_Checked, 11, 30);

                dgv_Checked.ScrollBars = ScrollBars.Both;

                foreach(DataGridViewRow row in dgv_Checked.Rows)
                {
                    status = row.Cells[11].Value.ToString();
                    _status = (status.ToLower() == "true") ? true : false;

                    row.DefaultCellStyle.BackColor = (_status == true) ? Color.LightGreen : Color.LightPink;
                }
            }
            catch
            {

            }
            finally
            {

            }
        }

        public void Fnc_IV_Check_Color_To_Default(object sender, EventArgs e)
        {
            lbl_Product.BackColor = (lbl_Product.Text != "-") ? Color.White : Color.Gainsboro;
            lbl_Code_01.BackColor = (lbl_Code_01.Text != "-") ? Color.White : Color.Gainsboro;
            lbl_Code_02.BackColor = (lbl_Code_02.Text != "-") ? Color.White : Color.Gainsboro;
            lbl_Code_01_Status.BackColor = lbl_Code_02_Status.BackColor = Color.Gainsboro;
            lbl_Code_01_Status.Text = lbl_Code_02_Status.Text = "-";

            chk_Timer.Stop();
        }

        public void Fnc_IV_Check_Color_To_Status()
        {
            chk_Timer.Interval = 4000;
            chk_Timer.Tick += new EventHandler(Fnc_IV_Check_Color_To_Default);
            chk_Timer.Enabled = true;
            chk_Timer.Start();
        }


        public void fnBindInit()
        {
            //try
            //{
            //    string idx = "", statusOK = "", statusNG = "";
            //    string line = "", partname = "", partcode = "", order = "";
            //    string ok = "0", ng = "0", rate = "0", nextorder = "", pic = "";
            //    string code01 = "", code02 = "", code03 = "", goal = "";
            //    int codeN = 0;
            //    string sqlInit = "V2o1_BASE_Capacity_Dispenser_Scan_SelItem_V2o2_Addnew";
            //    DataTable dtInit = new DataTable();

            //    SqlParameter[] sParamsInit = new SqlParameter[1]; // Parameter count
            //    sParamsInit[0] = new SqlParameter();
            //    sParamsInit[0].SqlDbType = SqlDbType.Int;
            //    sParamsInit[0].ParameterName = "@lineId";
            //    sParamsInit[0].Value = lineId;

            //    dtInit = cls.ExecuteDataTable(sqlInit, sParamsInit);

            //    if (dtInit.Rows.Count > 0)
            //    {
            //        idx = dtInit.Rows[0][0].ToString();
            //        line = dtInit.Rows[0][7].ToString().ToUpper();
            //        partname = dtInit.Rows[0][4].ToString();
            //        partcode = dtInit.Rows[0][5].ToString();
            //        pic = "PIC: " + dtInit.Rows[0][9].ToString().ToUpper();
            //        order = dtInit.Rows[0][14].ToString();
            //        ok = dtInit.Rows[0][16].ToString();
            //        ng = dtInit.Rows[0][17].ToString();
            //        rate = dtInit.Rows[0][19].ToString();
            //        code01 = dtInit.Rows[0][20].ToString();
            //        code02 = dtInit.Rows[0][21].ToString();
            //        code03 = dtInit.Rows[0][22].ToString();
            //        goal = dtInit.Rows[0][23].ToString();
            //        nextorder = "";
            //        txtItemCode.Enabled = true;
            //        txtItemCode.Focus();

            //        if (code01 != "" && code01 != null)
            //        {
            //            codeN = 1;
            //        }
            //        if (code02 != "" && code02 != null)
            //        {
            //            codeN = 2;
            //        }
            //        if (code03 != "" && code03 != null)
            //        {
            //            codeN = 3;
            //        }
            //    }
            //    else
            //    {
            //        idx = "0";
            //        line = "N/A";
            //        partname = "N/A";
            //        partcode = "N/A";
            //        pic = "";
            //        order = "0";
            //        ok = "0";
            //        ng = "0";
            //        rate = "0.0";
            //        code01 = "";
            //        code02 = "";
            //        code03 = "";
            //        nextorder = "";
            //        txtItemCode.Enabled = false;

            //        codeN = 0;
            //    }

            //    //string msg = "";
            //    //msg += "idx: " + idx + "\r\n";
            //    //msg += "line: " + line + "\r\n";
            //    //msg += "partname: " + partname + "\r\n";
            //    //msg += "partcode: " + partcode + "\r\n";
            //    //msg += "pic: " + pic + "\r\n";
            //    //msg += "order: " + order + "\r\n";
            //    //msg += "ok: " + ok + "\r\n";
            //    //msg += "ng: " + ng + "\r\n";
            //    //msg += "rate: " + rate + "\r\n";
            //    //msg += "code01: " + code01 + "\r\n";
            //    //msg += "code02: " + code02 + "\r\n";
            //    //msg += "code03: " + code03 + "\r\n";
            //    //msg += "nextorder: " + nextorder + "\r\n";

            //    //MessageBox.Show(msg);

            //    //_code = _partsubcode.ToString();

            //    _codeN = codeN;
            //    _idx = idx;
            //    _statusOK = (ok != "" && ok != null) ? Convert.ToInt32(ok) : 0;
            //    _statusNG = (ng != "" && ng != null) ? Convert.ToInt32(ng) : 0;
            //    //_total = _statusOK + _statusNG;
            //    _rate = (rate != "" && rate != null) ? Convert.ToDecimal(rate) : 0;
            //    _partsubcode01 = code01;
            //    _partsubcode02 = code02;
            //    _partsubcode03 = code03;

            //    lbl_Code_01.Text = code01;
            //    lbl_Code_02.Text = code02;

            //    lblLine.Text = line;
            //    lblPartName.Text = partname + "\r\n" + partcode;
            //    //lblPartCode.Text = partcode;
            //    lblPIC.Text = pic;
            //    lblOrder.Text = order;
            //    lblOK.Text = ok;
            //    lblNG.Text = ng;
            //    lblGoal.Text = goal;
            //    //lblRate.Text = rate + "%";
            //    lblRate.Text = String.Format("{0:0.0}", _rate);// + "%";
            //    //lblRate.Text = String.Format("{0:0}", _rate);// + "%";
            //    lblNextOrder.Text = nextorder;

            //    fnGetNextOrder();
            //    fnc_RateColor();
            //}
            //catch
            //{

            //}
            //finally
            //{

            //}
        }

        #endregion


        #region DISPENSER SCAN CHECK ITEM

        public void Fnc_TabPage03_Load_Controls()
        {
            txt_code.Text =
                lbl_code.Text =
                lbl_qty.Text =
                lbl_pattern01.Text =
                lbl_pattern02.Text =
                lbl_prodNm.Text =
                lbl_prodCd.Text = "";

            lbl_code.BackColor =
                lbl_qty.BackColor =
                lbl_pattern01.BackColor =
                lbl_pattern02.BackColor =
                lbl_prodNm.BackColor =
                lbl_prodCd.BackColor =
                dgv_code.BackgroundColor = Color.Gainsboro;

            dgv_code.DataSource = null;

            btn_view.Enabled = 
                btn_scan.Enabled = 
                btn_clear.Enabled = false;

            txt_code.Focus();
        }

        /************************************************************/

        public void Fnc_TabPage03_Load_Data()
        {
            string cart_code = txt_code.Text.Trim();

            Fnc_TabPage03_Load_Data_Cart(cart_code);
            Fnc_TabPage03_Load_Data_Item();
        }

        public void Fnc_TabPage03_Load_Data_Cart(string cart)
        {
            if (cart.Length > 0)
            {

            }
        }

        public void Fnc_TabPage03_Load_Data_Item()
        {

        }

        /************************************************************/

        private void txt_code_KeyDown(object sender, KeyEventArgs e)
        {
            Fnc_TabPage03_Load_Data();
        }


        #endregion


        #region STATUS MENU

        private void thoátChươngTrìnhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Bạn có chắc chắn?", cls.appName(), MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            {
                //this.Close();
                System.Windows.Forms.Application.Exit();
            }
        }

        private void khởiĐộngLạiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Bạn có chắc chắn?", cls.appName(), MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            {
                System.Windows.Forms.Application.Restart();
            }
        }


        #endregion

    }
}
