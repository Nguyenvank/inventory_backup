using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_Data
{
    public partial class uc_Dispenser_Item_Remain_Scan_V1o1_ProBox : Form
    {
        System.Windows.Forms.Timer _timer = new System.Windows.Forms.Timer();

        string
            __part_idx = "",
            _pack_idx = "",
            _pack_code = "",
            _prod_line = "",
            _prod_idx = "";

        int
            _pack_qty = 0,
            _pack_std = 0;

        Color
            _color_enable = Color.White,
            _color_disable = Color.Gainsboro,
            _color_selected = Color.Gold,
            _color_normal = Color.White;

        public uc_Dispenser_Item_Remain_Scan_V1o1_ProBox()
        {
            InitializeComponent();

            FlexBox.FlexibleMessageBox.FONT = new Font("Times New Roman", 15, FontStyle.Regular);

            _timer.Interval = 1000;
            _timer.Enabled = false;
            _timer.Tick += _timer_Tick;
        }

        public uc_Dispenser_Item_Remain_Scan_V1o1_ProBox(string pack_code)
        {
            InitializeComponent();

            FlexBox.FlexibleMessageBox.FONT = new Font("Times New Roman", 15, FontStyle.Regular);

            _pack_code = pack_code;
        }

        private void uc_Dispenser_Item_Remain_Scan_V1o1_ProBox_Load(object sender, EventArgs e)
        {
            Fnc_Load_Init();
        }

        private void uc_Dispenser_Item_Remain_Scan_V1o1_ProBox_KeyDown(object sender, KeyEventArgs e)
        {
            string
                msg = "";

            if (e.KeyCode == Keys.Escape)
            {
                if (rdb_Pack_None.Checked || rdb_Pack_More.Checked || rdb_Pack_Less.Checked)
                {
                    msg = "CÓ CHẮC LÀ MUỐN ĐÓNG CỬA SỔ NÀY ?";
                    DialogResult dialog = FlexBox.FlexibleMessageBox.Show(msg, cls.appName(), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialog == DialogResult.Yes)
                    {
                        this.Close();
                    }
                }
                else
                {
                    this.Close();
                }
            }
        }

        private void uc_Dispenser_Item_Remain_Scan_V1o1_ProBox_Shown(object sender, EventArgs e)
        {
            rdb_Pack_None.Checked = rdb_Pack_More.Checked = rdb_Pack_Less.Checked =
                rdb_Pack_None.Enabled = rdb_Pack_More.Enabled = rdb_Pack_Less.Enabled = false;
            //tlp_Code.Enabled = txt_Code.Enabled = false;
            //tlp_Code.BackColor = txt_Code.BackColor = _color_disable;
        }

        public void Fnc_Load_Init()
        {
            Fnc_Load_Controls();
            Fnc_Load_Pack_IDx();
            Fnc_Load_Pack_List();
        }

        /***************************************************************/

        public void Fnc_Load_Controls()
        {
            lbl_Pack_Code.Text =
                lbl_Prod_Name.Text =
                lbl_Prod_Code.Text =
                lbl_Pack_Date.Text =
                lbl_Pack_Type.Text =
                lbl_Pack_Qty.Text =
                lbl_Pack_LOT.Text =
                lbl_Prod_Line.Text =
                lbl_Pack_Std.Text =
                lbl_Same_Code01.Text =
                lbl_Same_Code02.Text =
                lbl_Pattern_Code01.Text =
                lbl_Pattern_Code02.Text =
                txt_Code.Text =
                lbl_Last_Code.Text =
                lbl_Scan_Item.Text = "";

            lbl_Pack_Code.BackColor =
                lbl_Prod_Name.BackColor =
                lbl_Prod_Code.BackColor =
                lbl_Pack_Date.BackColor =
                lbl_Pack_Type.BackColor =
                lbl_Pack_Qty.BackColor =
                lbl_Pack_LOT.BackColor =
                lbl_Prod_Line.BackColor =
                lbl_Pack_Std.BackColor =
                lbl_Pattern_Code01.BackColor =
                lbl_Pattern_Code02.BackColor =
                //tlp_Code.BackColor =
                //txt_Code.BackColor =
                lbl_Last_Code.BackColor =
                lbl_Scan_Item.BackColor = _color_disable;

            rdb_Pack_None.Checked = rdb_Pack_More.Checked = rdb_Pack_Less.Checked = false;
            rdb_Pack_None.Enabled = rdb_Pack_More.Enabled = rdb_Pack_Less.Enabled = false;
            lbl_Same_Code01.Visible = lbl_Same_Code02.Visible =
                tlp_Code.Enabled = txt_Code.Enabled = true;
            tlp_Code.BackColor = txt_Code.BackColor = _color_enable;
            pgr_Item_Scan.Minimum = 
                pgr_Item_Scan.Value = 
                pgr_Item_Scan.Maximum = 0;

            dgv_Item_List.DataSource =
                dgv_Short_List.DataSource =
                dgv_Short_Item.DataSource = null;

            lbl_Short_Code.Text = "Hàng chuyển chi tiết:";
            txt_Code.Focus();
        }

        public void Fnc_Load_Pack_IDx()
        {
            string
                msg = "",
                sql = "",
                pack_idx = "",
                pack_code = _pack_code,
                prod_idx = "",
                prod_name = "",
                prod_code = "",
                pack_date = "",
                pack_type = "",
                pack_qty = "",
                pack_lot = "",
                prod_line = "",
                line_name = "",
                pack_std = "",
                pattern01 = "",
                pattern02 = "";
            DateTime
                pack_dt = DateTime.Now;
            int
                qty_box = 0,
                qty_std = 0,
                tblCnt = 0,
                rowCnt = 0;

            if (pack_code.Length > 0)
            {
                sql = "V2_BASE_Inventory_ScanIn_PackBox_Item_SelItem_V1o0_Addnew";

                SqlParameter[] sParams = new SqlParameter[1];

                sParams[0] = new SqlParameter();
                sParams[0].SqlDbType = SqlDbType.VarChar;
                sParams[0].ParameterName = "@pack_code";
                sParams[0].Value = pack_code;

                DataSet ds = new DataSet();
                ds = cls.ExecuteDataSet(sql, sParams);
                tblCnt = ds.Tables.Count;
                rowCnt = ds.Tables[0].Rows.Count;

                //MessageBox.Show(tblCnt + " | " + rowCnt);

                if (tblCnt > 0 && rowCnt > 0)
                {
                    _pack_idx = pack_idx = ds.Tables[0].Rows[0][0].ToString();
                    //pack_code = ds.Tables[0].Rows[0][1].ToString();
                    prod_name = ds.Tables[0].Rows[0][2].ToString();
                    prod_code = ds.Tables[0].Rows[0][3].ToString();
                    pack_date = ds.Tables[0].Rows[0][4].ToString();
                    pack_type = ds.Tables[0].Rows[0][5].ToString();
                    pack_qty = ds.Tables[0].Rows[0][6].ToString();
                    pack_lot = ds.Tables[0].Rows[0][7].ToString();
                    _prod_line = prod_line = ds.Tables[0].Rows[0][8].ToString();
                    pack_std = ds.Tables[0].Rows[0][9].ToString();
                    pattern01 = ds.Tables[0].Rows[0][10].ToString();
                    pattern02 = ds.Tables[0].Rows[0][11].ToString();
                    _prod_idx = ds.Tables[0].Rows[0][12].ToString();

                    pack_dt = (pack_date.Length > 0) ? Convert.ToDateTime(pack_date) : DateTime.Now;
                    line_name = (prod_line.Length > 0) ? (prod_line == "4") ? "Line 01" : "Line 02" : "";
                    _pack_qty = qty_box = (pack_qty.Length > 0) ? Convert.ToInt32(pack_qty) : 0;
                    _pack_std = qty_std = (pack_std.Length > 0) ? Convert.ToInt32(pack_std) : 0;

                    lbl_Pack_Code.Text = pack_code;
                    lbl_Prod_Name.Text = prod_name;
                    lbl_Prod_Code.Text = prod_code;
                    lbl_Pack_Date.Text = String.Format("{0:dd/MM/yyyy HH:mm:ss}", pack_dt);
                    lbl_Pack_Type.Text = pack_type;
                    lbl_Pack_Qty.Text = String.Format("{0:0}", qty_box);
                    lbl_Pack_LOT.Text = pack_lot;
                    lbl_Prod_Line.Text = line_name;
                    lbl_Pack_Std.Text = String.Format("{0:0}", qty_std);
                    lbl_Pattern_Code01.Text = pattern01;
                    lbl_Pattern_Code02.Text = pattern02;
                    lbl_Scan_Item.Text = "[0] / " + qty_box;

                    lbl_Pack_Code.BackColor = _color_enable;
                    rdb_Pack_None.Checked = rdb_Pack_More.Checked = rdb_Pack_Less.Checked = false;
                    rdb_Pack_None.Enabled = rdb_Pack_More.Enabled = rdb_Pack_Less.Enabled = true;
                    tlp_Code.Enabled = txt_Code.Enabled = false;
                    tlp_Code.BackColor = txt_Code.BackColor = _color_disable;
                }
                else
                {
                    Fnc_Load_Controls();

                    _pack_idx = "";

                    msg = "KHÔNG TÌM THẤY DỮ LIỆU !!!\r\n\r\n";
                    msg += "Kiểm tra lại mã tem xe và quét lại";

                    FlexBox.FlexibleMessageBox.Show(msg, cls.appName(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void Fnc_Load_Pack_List()
        {
            try
            {
                string
                    msg = "",
                    sql = "",
                    pack_idx = _pack_idx;
                int
                    tblCnt = 0,
                    rowCnt = 0;

                if (pack_idx.Length > 0)
                {
                    sql = "V2_BASE_Inventory_ScanIn_PackBox_Item_List_SelItem_V1o0_Addnew";

                    SqlParameter[] sParams = new SqlParameter[1];

                    sParams[0] = new SqlParameter();
                    sParams[0].SqlDbType = SqlDbType.Int;
                    sParams[0].ParameterName = "@pack_idx";
                    sParams[0].Value = pack_idx;

                    DataTable dt = new DataTable();
                    dt = cls.ExecuteDataTable(sql, sParams);
                    dgv_Item_List.DataSource = dt;

                    tblCnt = dt.Rows.Count;

                    dgv_Item_List.Columns[0].FillWeight = 7;    // STT
                    //dgv_Item_List.Columns[1].FillWeight = 5;    // cart_idx
                    //dgv_Item_List.Columns[2].FillWeight = 5;    // line_no
                    dgv_Item_List.Columns[3].FillWeight = 34;    // code_01
                    dgv_Item_List.Columns[4].FillWeight = 34;    // code_02
                    //dgv_Item_List.Columns[5].FillWeight = 5;    // compare_ok
                    dgv_Item_List.Columns[6].FillWeight = 25;    // scan_date

                    dgv_Item_List.Columns[0].Visible = true;
                    dgv_Item_List.Columns[1].Visible = false;
                    dgv_Item_List.Columns[2].Visible = false;
                    dgv_Item_List.Columns[3].Visible = true;
                    dgv_Item_List.Columns[4].Visible = true;
                    dgv_Item_List.Columns[5].Visible = false;
                    dgv_Item_List.Columns[6].Visible = true;

                    dgv_Item_List.Columns[6].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
                    cls.fnFormatDatagridview_FullWidth(dgv_Item_List, 11, 30);

                    //rdb_Pack_More.Enabled = (_pack_qty == _pack_std) ? false : true;
                    //rdb_Pack_Less.Enabled = (tblCnt == 0) ? false : true;

                    pgr_Item_Scan.Minimum = 0;
                    pgr_Item_Scan.Maximum = _pack_qty;
                    pgr_Item_Scan.Value = tblCnt;

                    Fnc_Load_Short_List();

                    lbl_Scan_Item.Text = lbl_Scan_Item.Text.Replace("[0]", tblCnt.ToString());
                }
            }
            catch { }
            finally { }
        }

        public void Fnc_Load_Short_List()
        {
            string
                msg = "",
                sql = "",
                part_idx = "",
                part_name = "",
                part_code = "";

            int
                tblCnt = 0,
                rowCnt = 0,
                _part_idx = 0,
                _part_short = (_prod_idx.Length > 0) ? Convert.ToInt32(_prod_idx) : 0;

            sql = "V2_BASE_Inventory_ScanIn_PackBox_Short_List_SelItem_V1o0_Addnew";

            DataTable dt = new DataTable();
            dt = cls.ExecuteDataTable(sql);
            dgv_Short_List.DataSource = dt;

            tblCnt = dt.Rows.Count;

            dgv_Short_List.Columns[0].FillWeight = 15;    // STT
            //dgv_Short_List.Columns[1].FillWeight = 10;    // ProdId
            dgv_Short_List.Columns[2].FillWeight = 70;    // Name
            //dgv_Short_List.Columns[3].FillWeight = 70;    // BarCode
            dgv_Short_List.Columns[4].FillWeight = 15;    // Qty

            dgv_Short_List.Columns[0].Visible = true;
            dgv_Short_List.Columns[1].Visible = false;
            dgv_Short_List.Columns[2].Visible = true;
            dgv_Short_List.Columns[3].Visible = false;
            dgv_Short_List.Columns[4].Visible = true;

            cls.fnFormatDatagridview_FullWidth(dgv_Short_List, 11, 30);

            foreach(DataGridViewRow row in dgv_Short_List.Rows)
            {
                __part_idx = part_idx = row.Cells[1].Value.ToString();
                part_name = row.Cells[2].Value.ToString();
                part_code = row.Cells[3].Value.ToString();

                _part_idx = (part_idx.Length > 0) ? Convert.ToInt32(part_idx) : 0;

                if (_part_idx > 0 && _part_idx == _part_short) 
                {
                    lbl_Short_Code.Text = "Hàng chuyển chi tiết: " + part_name;
                    row.DefaultCellStyle.BackColor = _color_selected;
                    Fnc_Load_Short_Item(part_idx); 
                }
            }
        }

        public void Fnc_Load_Short_Item(string part_idx)
        {
            string
                msg = "",
                sql = "";

            int
                tblCnt = 0,
                rowCnt = 0;

            sql = "V2_BASE_Inventory_ScanIn_PackBox_Short_Item_SelItem_V1o0_Addnew";

            SqlParameter[] sParams = new SqlParameter[1];

            sParams[0] = new SqlParameter();
            sParams[0].SqlDbType = SqlDbType.Int;
            sParams[0].ParameterName = "@part_idx";
            sParams[0].Value = part_idx;

            DataTable dt = new DataTable();
            dt = cls.ExecuteDataTable(sql, sParams);
            dgv_Short_Item.DataSource = dt;

            tblCnt = dt.Rows.Count;

            dgv_Short_Item.Columns[0].FillWeight = 15;    // STT
            //dgv_Short_Item.Columns[1].FillWeight = 7;    // prev_cart
            //dgv_Short_Item.Columns[2].FillWeight = 7;    // prev_prod
            //dgv_Short_Item.Columns[3].FillWeight = 7;    // prev_line
            //dgv_Short_Item.Columns[4].FillWeight = 7;    // prev_scan
            dgv_Short_Item.Columns[5].FillWeight = 60;    // code_01
            //dgv_Short_Item.Columns[6].FillWeight = 60;    // code_02
            //dgv_Short_Item.Columns[7].FillWeight = 7;    // less_item
            dgv_Short_Item.Columns[8].FillWeight = 25;    // less_date

            dgv_Short_Item.Columns[0].Visible = true;
            dgv_Short_Item.Columns[1].Visible = false;
            dgv_Short_Item.Columns[2].Visible = false;
            dgv_Short_Item.Columns[3].Visible = false;
            dgv_Short_Item.Columns[4].Visible = false;
            dgv_Short_Item.Columns[5].Visible = true;
            dgv_Short_Item.Columns[6].Visible = false;
            dgv_Short_Item.Columns[7].Visible = false;
            dgv_Short_Item.Columns[8].Visible = true;

            dgv_Short_Item.Columns[8].DefaultCellStyle.Format = "dd/MM HH:mm:ss";
            cls.fnFormatDatagridview_FullWidth(dgv_Short_Item, 11, 30);
        }

        public void Fnc_Load_Pack_Change_Status_Enable()
        {
            tlp_Code.Enabled = txt_Code.Enabled = true;
            tlp_Code.BackColor = txt_Code.BackColor = _color_enable;
            txt_Code.Focus();
        }

        public void Fnc_Load_Pack_Change_Status_Disable()
        {
            tlp_Code.Enabled = txt_Code.Enabled = false;
            tlp_Code.BackColor = txt_Code.BackColor = _color_disable;
        }

        public void Fnc_Load_Pack_Change_Status_Disable(RadioButton rdb)
        {
            rdb.Checked = false;
            tlp_Code.Enabled = txt_Code.Enabled = false;
            tlp_Code.BackColor = txt_Code.BackColor = _color_disable;
        }

        /***************************************************************/

        public void Fnc_Load_Pack_None(string item_code)
        {

        }

        public void Fnc_Load_Pack_More(string item_code)
        {
            string
                msg = "",
                sql = "",
                item_code01 = "",
                item_code02 = "",
                pack_idx = _pack_idx,
                prod_line = _prod_line;

            int
                tblCnt = 0,
                rowCnt = 0,
                item_seq = 0,
                pack_qty = _pack_qty,
                pack_std = _pack_std;

            bool
                item_same = false;

            foreach (DataGridViewRow row in dgv_Short_Item.Rows)
            {
                item_code01 = row.Cells[5].Value.ToString();
                item_code02 = row.Cells[6].Value.ToString();

                if (cls.Fnc_Compare_String_OrdinalIgnoreCase(item_code, item_code01) == true)
                {
                    item_same = true;
                    item_seq = 1;
                }
                else if (cls.Fnc_Compare_String_OrdinalIgnoreCase(item_code, item_code02) == true)
                {
                    item_same = true;
                    item_seq = 2;
                }
                else
                {
                    item_same = false;
                    item_seq = 0;
                }

                //MessageBox.Show("item_same: " + item_same.ToString() + "\r\nitem_seq: " + item_seq);

                if (item_same == true && _pack_qty <= _pack_std)
                {
                    if (_pack_qty <= _pack_std)
                    {
                        sql = "V2_BASE_Inventory_ScanIn_PackBox_Items_More_AddItem_V1o0_Addnew";

                        SqlParameter[] sParams = new SqlParameter[4];

                        sParams[0] = new SqlParameter();
                        sParams[0].SqlDbType = SqlDbType.Int;
                        sParams[0].ParameterName = "@pack_idx";
                        sParams[0].Value = pack_idx;

                        sParams[1] = new SqlParameter();
                        sParams[1].SqlDbType = SqlDbType.TinyInt;
                        sParams[1].ParameterName = "@prod_line";
                        sParams[1].Value = prod_line;

                        sParams[2] = new SqlParameter();
                        sParams[2].SqlDbType = SqlDbType.VarChar;
                        sParams[2].ParameterName = "@item_code";
                        sParams[2].Value = item_code;

                        sParams[3] = new SqlParameter();
                        sParams[3].SqlDbType = SqlDbType.TinyInt;
                        sParams[3].ParameterName = "@item_seq";
                        sParams[3].Value = item_seq;

                        cls.fnUpdDel(sql, sParams);

                        Fnc_Load_Pack_IDx();
                        Fnc_Load_Pack_List();

                        rdb_Pack_None.Checked = rdb_Pack_Less.Checked = false; rdb_Pack_More.Checked = true;
                        rdb_Pack_None.Enabled = rdb_Pack_Less.Enabled = false; rdb_Pack_More.Enabled = true;
                        tlp_Code.Enabled = txt_Code.Enabled = true;
                        tlp_Code.BackColor = txt_Code.BackColor = _color_enable;
                    }
                    else
                    {
                        tlp_Code.Enabled = txt_Code.Enabled = false;
                        tlp_Code.BackColor = txt_Code.BackColor = _color_disable;

                        msg = "CÓ LỖI SỐ LƯỢNG !!!\r\n\r\n";
                        msg += "Không thể thêm hàng khi số lượng lớn hơn " + _pack_std;

                        FlexBox.FlexibleMessageBox.Show(msg, cls.appName(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public void Fnc_Load_Pack_Less(string item_code)
        {
            string
                msg = "",
                sql = "",
                item_code01 = "",
                item_code02 = "",
                pack_idx = _pack_idx;

            int
                tblCnt = 0,
                rowCnt = 0,
                item_seq = 0,
                pack_qty = _pack_qty,
                pack_std = _pack_std;

            bool
                item_same = false;

            foreach (DataGridViewRow row in dgv_Item_List.Rows)
            {
                item_code01 = row.Cells[3].Value.ToString();
                item_code02 = row.Cells[4].Value.ToString();

                if(cls.Fnc_Compare_String_OrdinalIgnoreCase(item_code, item_code01) == true)
                {
                    item_same = true;
                    item_seq = 1;
                }
                else if(cls.Fnc_Compare_String_OrdinalIgnoreCase(item_code, item_code02) == true)
                {
                    item_same = true;
                    item_seq = 2;
                }
                else
                {
                    item_same = false;
                    item_seq = 0;
                }

                //MessageBox.Show("Action: Less\r\nItem code: " + item_code + "\r\nItem same: " + item_same.ToString() + "\r\nItem Seq#: " + item_seq);

                if (item_same == true && _pack_qty > 0)
                {
                    if (_pack_qty > 0)
                    {
                        sql = "V2_BASE_Inventory_ScanIn_PackBox_Items_Less_DelItem_V1o0_Addnew";

                        SqlParameter[] sParams = new SqlParameter[3];

                        sParams[0] = new SqlParameter();
                        sParams[0].SqlDbType = SqlDbType.Int;
                        sParams[0].ParameterName = "@pack_idx";
                        sParams[0].Value = pack_idx;

                        sParams[1] = new SqlParameter();
                        sParams[1].SqlDbType = SqlDbType.VarChar;
                        sParams[1].ParameterName = "@item_code";
                        sParams[1].Value = item_code;

                        sParams[2] = new SqlParameter();
                        sParams[2].SqlDbType = SqlDbType.TinyInt;
                        sParams[2].ParameterName = "@item_seq";
                        sParams[2].Value = item_seq;

                        cls.fnUpdDel(sql, sParams);

                        Fnc_Load_Pack_IDx();
                        Fnc_Load_Pack_List();

                        rdb_Pack_None.Checked = rdb_Pack_More.Checked = false; rdb_Pack_Less.Checked = true;
                        rdb_Pack_None.Enabled = rdb_Pack_More.Enabled = false; rdb_Pack_Less.Enabled = true;
                        tlp_Code.Enabled = txt_Code.Enabled = true;
                        tlp_Code.BackColor = txt_Code.BackColor = _color_enable;
                    }
                    else
                    {
                        tlp_Code.Enabled = txt_Code.Enabled = false;
                        tlp_Code.BackColor = txt_Code.BackColor = _color_disable;

                        msg = "CÓ LỖI SỐ LƯỢNG !!!\r\n\r\n";
                        msg += "Không thể bớt hàng khi số lượng bằng 0";

                        FlexBox.FlexibleMessageBox.Show(msg, cls.appName(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
        }

        /***************************************************************/

        public void Fnc_Load_522(string code)
        {
            string
                msg = "",
                item = code,
                item_kind = code.Substring(0, 3);

            if (_pack_code.Length > 0 && _pack_idx.Length > 0)
            {
                if (item_kind == "522")
                {
                    if (rdb_Pack_None.Checked)
                    {
                        Fnc_Load_Pack_None(item);
                    }
                    else if (rdb_Pack_More.Checked)
                    {
                        Fnc_Load_Pack_More(item);
                    }
                    else if (rdb_Pack_Less.Checked)
                    {
                        Fnc_Load_Pack_Less(item);
                    }
                    else
                    {
                        msg = "CÓ LỖI CHỨC NĂNG !!!\r\n\r\n";
                        msg += "Chưa chọn chức năng nào khi quét van\r\n";
                        msg += "Hãy chọn chức năng\r\n";
                        msg += "(Quét lại / Thêm hàng / Bớt hàng)\r\n";
                        msg += "và quét lại mã van đúng";

                        FlexBox.FlexibleMessageBox.Show(msg, cls.appName(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    msg = "CÓ LỖI MÃ VAN !!!\r\n\r\n";
                    msg += "Loại mã van không đúng quy định\r\n";
                    msg += "Hãy kiểm tra và quét lại mã van đúng";

                    FlexBox.FlexibleMessageBox.Show(msg, cls.appName(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                msg = "CÓ LỖI QUY TRÌNH !!!\r\n\r\n";
                msg += "Chưa quét mã xe có chứa van\r\n";
                msg += "Hãy kiểm tra và quét lại mã xe trước khi quét mã van";

                FlexBox.FlexibleMessageBox.Show(msg, cls.appName(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Fnc_Load_PRO(string code)
        {
            string
                msg = "",
                pack_code = code.Substring(0, 3);

            int
                pack_qty = _pack_qty,
                pack_std = _pack_std,
                item_list = dgv_Item_List.Rows.Count;

            if (pack_code == "PRO")
            {
                _pack_code = code;

                //Fnc_Load_Controls();
                Fnc_Load_Pack_IDx();
                Fnc_Load_Pack_List();

                _timer.Enabled = (_pack_idx.Length > 0) ? true : false;

                if (_pack_idx.Length > 0)
                {
                    rdb_Pack_None.Checked = rdb_Pack_More.Checked = rdb_Pack_Less.Checked = false;
                    rdb_Pack_None.Enabled = rdb_Pack_More.Enabled = rdb_Pack_Less.Enabled = true;
                }
            }
            else
            {
                msg = "CÓ LỖI MÃ XE !!!\r\n\r\n";
                msg += "Mã xe này không đúng loại dành cho sản xuất\r\n";
                msg += "(" + code + ")\r\n\r\n";
                msg += "Kiểm tra và quét lại đúng mã bắt đầu bằng PRO";

                FlexBox.FlexibleMessageBox.Show(msg, cls.appName(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /***************************************************************/

        private void _timer_Tick(object sender, EventArgs e)
        {
            //Fnc_Load_Short_Item(__part_idx);
        }

        private void rdb_Pack_None_MouseClick(object sender, MouseEventArgs e)
        {
            string
                msg = "";
            if (rdb_Pack_None.Checked)
            {
                msg = "XÁC NHẬN LẠI THÔNG TIN !!!\r\n\r\n";
                msg += "Chức năng này cho phép quét lại hàng\r\n";
                msg += "chưa quét từng van trước đó (quy trình cũ)\r\n";
                msg += "Tất cả van đã quét trước (nếu có) sẽ bị xoá hết\r\n\r\n";
                msg += "Vẫn muốn tiếp tục?";

                DialogResult dialog = FlexBox.FlexibleMessageBox.Show(msg, cls.appName(), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialog == DialogResult.Yes)
                {
                    Fnc_Load_Pack_Change_Status_Enable();
                }
                else
                {
                    //rdb_Pack_More.Checked = false;
                    Fnc_Load_Pack_Change_Status_Disable(rdb_Pack_None);
                }
                rdb_Pack_More.Enabled = rdb_Pack_Less.Enabled = (dialog == DialogResult.Yes) ? false : true;
                //rdb_Pack_More.Enabled = rdb_Pack_Less.Enabled = false;
            }
            else
            {
                Fnc_Load_Pack_Change_Status_Disable(rdb_Pack_None);
            }
        }

        private void rdb_Pack_More_MouseClick(object sender, MouseEventArgs e)
        {
            string
                msg = "";
            if (rdb_Pack_More.Checked)
            {
                msg = "XÁC NHẬN LẠI THÔNG TIN !!!\r\n\r\n";
                msg += "Chức năng này cho phép THÊM hàng vào xe\r\n";
                msg += "(chưa đủ số lượng theo số lượng tiêu chuẩn: " + _pack_std + ")\r\n\r\n";
                msg += "Vẫn muốn tiếp tục?";

                DialogResult dialog = FlexBox.FlexibleMessageBox.Show(msg, cls.appName(), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialog == DialogResult.Yes)
                {
                    Fnc_Load_Pack_Change_Status_Enable();
                }
                else
                {
                    //rdb_Pack_More.Checked = false;
                    Fnc_Load_Pack_Change_Status_Disable(rdb_Pack_More);
                }
                rdb_Pack_None.Enabled = rdb_Pack_Less.Enabled = (dialog == DialogResult.Yes) ? false : true;
                //rdb_Pack_None.Enabled = rdb_Pack_Less.Enabled = false;
            }
            else
            {
                Fnc_Load_Pack_Change_Status_Disable(rdb_Pack_More);
            }
        }

        private void rdb_Pack_Less_MouseClick(object sender, MouseEventArgs e)
        {
            string
                msg = "";
            if (rdb_Pack_Less.Checked)
            {
                msg = "XÁC NHẬN LẠI THÔNG TIN !!!\r\n\r\n";
                msg += "Chức năng này cho phép BỚT hàng khỏi xe\r\n";
                msg += "(đủ số lượng theo yêu cầu giao hàng: hàng lẻ)\r\n\r\n";
                msg += "Vẫn muốn tiếp tục?";

                DialogResult dialog = FlexBox.FlexibleMessageBox.Show(msg, cls.appName(), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialog == DialogResult.Yes)
                {
                    Fnc_Load_Pack_Change_Status_Enable();
                }
                else
                {
                    //rdb_Pack_More.Checked = false;
                    Fnc_Load_Pack_Change_Status_Disable(rdb_Pack_Less);
                }
                rdb_Pack_None.Enabled = rdb_Pack_More.Enabled = (dialog == DialogResult.Yes) ? false : true;
                //rdb_Pack_None.Enabled = rdb_Pack_More.Enabled = false;
            }
            else
            {
                Fnc_Load_Pack_Change_Status_Disable(rdb_Pack_Less);
            }
        }

        private void txt_Code_KeyDown(object sender, KeyEventArgs e)
        {
            string
                msg = "",
                item = "",
                item_kind = "",
                item_type = "",
                item_code = "";

            int
                item_len = 0;

            if (e.KeyCode == Keys.Enter)
            {
                item = txt_Code.Text.Trim();
                item_len = item.Length;

                if (item_len >= 20)
                {
                    item_kind = item.Substring(0, 3);

                    switch (item_kind)
                    {
                        case "522":
                            Fnc_Load_522(item);
                            break;
                        case "PRO":
                            Fnc_Load_PRO(item);
                            break;
                        case "MMT":
                            break;
                    }
                }
                //else
                //{
                //    msg = "CÓ LỖI MÃ VAN !!!\r\n\r\n";
                //    msg += "Độ dài mã van không đúng quy định\r\n";
                //    msg += "Hãy kiểm tra và quét lại mã van đúng";

                //    FlexBox.FlexibleMessageBox.Show(msg, cls.appName(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}

                txt_Code.Text = "";
                txt_Code.Focus();
            }
        }
    }
}
