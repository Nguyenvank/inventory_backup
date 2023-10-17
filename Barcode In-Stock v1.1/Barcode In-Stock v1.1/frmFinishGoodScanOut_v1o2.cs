using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Inventory_Data
{
    public partial class frmFinishGoodScanOut_v1o2 : Form
    {
        System.Timers.Timer
            __timer_main = new System.Timers.Timer();

        DateTime
            __dt_now = DateTime.Now,
            __shift_fr = DateTime.Now,
            __shift_to = DateTime.Now;

        cls.Ini ini = new cls.Ini(AppDomain.CurrentDomain.BaseDirectory + "\\" + Application.ProductName + ".ini");

        DataTable
            __dt_data = null;

        DataSet
            __ds_data = null;

        string
            __sql = "",
            __app = cls.appName(),
            __temper_type = "2",
            __fle_local = Path.Combine(Application.StartupPath.Replace("\\" + Application.ProductName, ""), "Temper Data"),
            __fle_server = Path.Combine(Application.StartupPath.Replace("\\" + Application.ProductName, ""), "Temper Data", "temper_server.txt"),
            __trg_refresh_data_time = "", __trg_refresh_data_unit = "",
            __trg_refresh_temp_time = "", __trg_refresh_temp_unit = "",
            __max_temp_point_disp = "",
            __view_scan_out_detail = "";

        int
            __tblCnt = 0,
            __rowCnt = 0,
            __colCnt = 0,
            __dt_sec = 0,
            __dt_min = 0,
            __dt_hrs = 0,
            __dt_day = 0,
            __dt_month = 0,
            __dt_year = 0,
            __shift_no = 0,
            __range = 1,
            __temper_now = 0,
            __temper_min = 0,
            __temper_max = 0,
            __max_temp_point_chart = 0,
            __focus = 10;

        bool
            __bool_refresh_data = false,
            __bool_refresh_temp = false,
            __bool_view_details = false;

        Point
            last_point;

        public frmFinishGoodScanOut_v1o2()
        {
            InitializeComponent();

            Fnc_Set_Config();

            __timer_main.Interval = 1000;
            __timer_main.Enabled = true;
            __timer_main.Elapsed += __timer_main_Elapsed;

            cls.SetDoubleBuffer(pnl_main, true);
            cls.SetDoubleBuffer(tlp_main, true);
            cls.SetDoubleBuffer(tlp_left, true);
            cls.SetDoubleBuffer(tlp_code, true);
            cls.SetDoubleBuffer(tlp_temper, true);
            cls.SetDoubleBuffer(dgv_list_in, true);
            cls.SetDoubleBuffer(dgv_list_out, true);
            cls.SetDoubleBuffer(chart_temper, true);
        }

        private void frmFinishGoodScanOut_v1o2_Load(object sender, EventArgs e)
        {
            Fnc_Load_Init();
        }

        public void Fnc_Load_Init()
        {
            Fnc_Load_Controls();
        }

        /*************************************************/

        public void Fnc_Load_Controls()
        {
            //Fnc_Set_Time_Shift();
            Fnc_Get_Temper_Filename();
            //Fnc_Get_Temper();
            Fnc_Get_Temper_Spec();

            txt_code.Text = "";
            txt_code.Focus();

            lbl_datetime.Text = String.Format("{0:dd/MM/yyyy HH:mm:sss}", __dt_now);

            cbb_time.Items.Clear();
            cbb_time.Items.AddRange(new object[] {
            "Hôm nay",
            "3 ngày",
            "1 tuần",
            "3 tuần",
            "1 tháng",
            "3 tháng",
            "6 tháng",
            "1 năm"});
            cbb_time.SelectedIndex = 0;

            tlp_heating_filter.Enabled = txt_heating_filter.Enabled = false;
            tlp_heating_filter.BackColor = txt_heating_filter.BackColor = Color.Black;
            txt_heating_filter.ForeColor = Color.FromKnownColor(KnownColor.Control);
            txt_heating_filter.Text = "";


            dgv_list_in.DataSource = dgv_list_out.DataSource = null;
            //dgv_list_ready.ForeColor = dgv_list_in.ForeColor = dgv_list_out.ForeColor = Color.Black;
            //dgv_list_ready.BackgroundColor = dgv_list_in.BackgroundColor = dgv_list_out.BackgroundColor = Color.Black;
            tlp_temper.BackColor = Color.FromKnownColor(KnownColor.Control);
            tlp_temper.ForeColor = Color.Black;


            Fnc_Load_Data();
            Fnc_Load_Data_Ready();
            Fnc_Load_Data_In();
            Fnc_Load_Data_Out();

            Fnc_Load_Fill_Chart();
            Fnc_Read_Temper_From_File(__fle_local);
        }

        public void Fnc_Set_Config()
        {
            __trg_refresh_data_time = ini.GetIniValue("TRIGGER", "TRIGGER_DATA_TIME", "1");
            __trg_refresh_data_unit = ini.GetIniValue("TRIGGER", "TRIGGER_DATA_UNIT", "M");
            __trg_refresh_temp_time = ini.GetIniValue("TRIGGER", "TRIGGER_TEMPER_TIME", "5");
            __trg_refresh_temp_unit = ini.GetIniValue("TRIGGER", "TRIGGER_TEMPER_UNIT", "M");
            __max_temp_point_disp = ini.GetIniValue("CHART", "MAX_POINT", "72");
            __view_scan_out_detail = ini.GetIniValue("VIEW", "DETAIL", "1");

            __max_temp_point_chart = (__max_temp_point_disp.Length > 0) ? Convert.ToInt32(__max_temp_point_disp) : 72;
            __bool_view_details = (__view_scan_out_detail == "1") ? true : false;
        }

        public void Fnc_Set_Refresh_Data()
        {
            string
                refresh_time = __trg_refresh_data_time,
                refresh_unit = __trg_refresh_data_unit;

            int
                _refresh_time = (refresh_time.Length > 0) ? Convert.ToInt32(refresh_time) : 1;

            switch (refresh_unit.ToLower())
            {
                case "h":
                    if ((__dt_hrs == 0 || __dt_hrs % _refresh_time == 0) && __dt_min == 0 && __dt_sec == 0) { __bool_refresh_data = true; }
                    break;
                case "m":
                    if ((__dt_min == 0 || __dt_min % _refresh_time == 0) && __dt_sec == 0) { __bool_refresh_data = true; }
                    break;
                case "s":
                    if (__dt_sec % _refresh_time == 0) { __bool_refresh_data = true; }
                    break;
            }
        }

        public void Fnc_Set_Refresh_Temper()
        {
            string
                refresh_time = __trg_refresh_temp_time,
                refresh_unit = __trg_refresh_temp_unit;

            int
                _refresh_time = (refresh_time.Length > 0) ? Convert.ToInt32(refresh_time) : 5;

            switch (refresh_unit.ToLower())
            {
                case "h":
                    if ((__dt_hrs == 0 || __dt_hrs % _refresh_time == 0) && __dt_min == 0 && __dt_sec == 3) { __bool_refresh_temp = true; }
                    break;
                case "m":
                    if ((__dt_min == 0 || __dt_min % _refresh_time == 0) && __dt_sec == 3) { __bool_refresh_temp = true; }
                    break;
                case "s":
                    if (__dt_sec % _refresh_time == 0) { __bool_refresh_temp = true; }
                    break;
            }
        }

        public void Fnc_Set_Time_Shift()
        {
            string
                folder_year = String.Format("{0:yyyy}", __dt_now),
                folder_month = String.Format("{0:yyyyMM}", __dt_now),
                temper_folder = "",
                temper_file = "",
                file_name = "";

            if (__dt_hrs >= 8 && __dt_hrs <= 19)
            {
                __shift_fr = new DateTime(__dt_now.Year, __dt_now.Month, __dt_now.Day, 8, 0, 0);
                __shift_to = new DateTime(__dt_now.Year, __dt_now.Month, __dt_now.Day, 19, 59, 59);
                __shift_no = 1;
            }
            else if (__dt_hrs >= 20 && __dt_hrs <= 23)
            {
                __shift_fr = new DateTime(__dt_now.Year, __dt_now.Month, __dt_now.Day, 20, 0, 0);
                __shift_to = new DateTime(__dt_now.Year, __dt_now.Month, __dt_now.Day, 07, 59, 59).AddDays(1);
                __shift_no = 2;
            }
            else if (__dt_hrs >= 0 && __dt_hrs <= 7)
            {
                __shift_fr = new DateTime(__dt_now.Year, __dt_now.Month, __dt_now.Day, 20, 0, 0).AddDays(-1);
                __shift_to = new DateTime(__dt_now.Year, __dt_now.Month, __dt_now.Day, 07, 59, 59);
                __shift_no = 2;
            }

            //file_name = String.Format("Temper {0:yyyyMMdd}-{1:0}.txt", __dt_now, __shift_no);
            file_name = String.Format("Temper {0:yyyyMMdd}.txt", __dt_now);

            temper_folder = Path.Combine(Application.StartupPath, "Temper Data", folder_year, folder_month);

            if (!Directory.Exists(temper_folder)) { Directory.CreateDirectory(temper_folder); }

            temper_file = Path.Combine(Application.StartupPath, "Temper Data", folder_year, folder_month, file_name);
            __fle_local = temper_file;
        }

        public void Fnc_Get_Temper_Filename()
        {
            string
                folder_main = Path.Combine(@"D:\", "Services", "Temper Data"),
                folder_year = String.Format("{0:yyyy}", __dt_now),
                folder_month = String.Format("{0:yyyyMM}", __dt_now),
                temper_local = String.Format("Temper {0:yyyyMMdd}.txt", __dt_now),
                temper_server = "temper_server.txt";

            string
                file_local = Path.Combine(folder_main, folder_year, folder_month, temper_local),
                file_server = Path.Combine(folder_main, temper_server);

            __fle_local = file_local;
            __fle_server = file_server;

            //label7.Text = file_local;
        }

        public void Fnc_Get_Temper()
        {
            int
                temper = 0;

            Random
                rand = new Random();

            temper = rand.Next(50, 90);

            lbl_temper_now.Text = String.Format("{0}" + (char)186 + "C", temper);

            Fnc_Get_Temper_Spec();
            Fnc_Write_Temper_To_File(temper);
            //Fnc_Write_Temper_To_DB(temper);
        }

        public void Fnc_Get_Temper_Spec()
        {
            try
            {
                string
                    heating_idx = "",
                    temper_min = "",
                    temper_max = "",
                    heating_hrs = "",
                    heating_sec = "";

                int
                    _temper_min = 0,
                    _temper_max = 0,
                    _heating_sec = 0;

                decimal
                    _heating_hrs = 0;

                __sql = "V2_BASE_WB_Heating_Spec_Item_SelItem_V1o0";

                SqlParameter[] sParams = new SqlParameter[1]; // Parameter count

                sParams[0] = new SqlParameter();
                sParams[0].SqlDbType = SqlDbType.TinyInt;
                sParams[0].ParameterName = "@temper_type";
                sParams[0].Value = __temper_type;

                __ds_data = new DataSet();
                __ds_data = cls.ExecuteDataSet(__sql, sParams);
                __tblCnt = __ds_data.Tables.Count;
                __rowCnt = __ds_data.Tables[0].Rows.Count;

                if (__tblCnt > 0 && __rowCnt > 0)
                {
                    heating_idx = __ds_data.Tables[0].Rows[0][0].ToString();
                    temper_min = __ds_data.Tables[0].Rows[0][1].ToString();
                    temper_max = __ds_data.Tables[0].Rows[0][2].ToString();
                    heating_hrs = __ds_data.Tables[0].Rows[0][3].ToString();
                    heating_sec = __ds_data.Tables[0].Rows[0][4].ToString();

                    _temper_min = (temper_min.Length > 0) ? Convert.ToInt32(temper_min) : 0;
                    _temper_max = (temper_max.Length > 0) ? Convert.ToInt32(temper_max) : 0;
                    _heating_hrs = (heating_hrs.Length > 0) ? Convert.ToDecimal(heating_hrs) : 0;
                    _heating_sec = (heating_sec.Length > 0) ? Convert.ToInt32(heating_sec) : 0;
                }
                else
                {
                    _temper_min = _temper_max = _heating_sec = 0;
                    _heating_hrs = 0;
                }

                __temper_min = _temper_min;
                __temper_max = _temper_max;

                //lbl_title_temper_spec.Text = "Thời gian sấy: từ " + _heating_hrs + "h | Nhiệt độ sấy: " + _temper_min + (char)186 + "C ~ " + _temper_max + (char)186 + "C";
                lbl_heating_time.Text = String.Format(">= {0}h", _heating_hrs);
                lbl_heating_temper.Text = String.Format("{0}" + (char)186 + "C ~ {1}" + (char)186 + "C", _temper_min, _temper_max);
            }
            catch { }
            finally { }
        }

        public void Fnc_Write_Temper_To_File(int temper)
        {
            try
            {
                FileInfo
                    file_local = new FileInfo(__fle_local),
                    file_server = new FileInfo(__fle_server);

                using (StreamWriter sw_local = file_local.AppendText())
                {
                    sw_local.WriteLine(String.Format("{0:dd/MM/yyyy HH:mm:ss},{1:0}", __dt_now, temper));
                }

                //using (StreamWriter sw_server = file_server.AppendText())
                //{
                //    sw_server.WriteLine(String.Format("{0:dd/MM/yyyy HH:mm:ss},{1:0}", __dt_now, temper));
                //}
            }
            catch { }
            finally { }
        }

        public void Fnc_Write_Temper_To_DB(string file)
        {
            string
                temper_line = "",
                temper_value = "",
                temper_date = "";

            int
                _max_row = 50,
                _split_pos = 0,
                _temper_value = 0;

            DateTime
                _temper_date = DateTime.Now;

            bool
                bool_exist = (File.Exists(file)) ? true : false,
                bool_conn = check.IsConnectedToLAN("192.168.1.1");

            DataTable
                dt_temper_server = null;

            DataColumn
                col_temper = null;

            DataRow
                row_temper = null;

            try
            {
                if (bool_conn == true && bool_exist == true)
                {
                    dt_temper_server = new DataTable("temper");

                    dt_temper_server.Columns.Add("Temper", typeof(Int32));
                    dt_temper_server.Columns.Add("Date", typeof(DateTime));

                    var lines = File.ReadLines(file);
                    foreach (var line in lines)
                    {
                        _split_pos = line.IndexOf(",");

                        temper_date = line.Substring(0, _split_pos);
                        temper_value = line.Substring(_split_pos + 1);

                        _temper_value = Convert.ToInt32(temper_value);
                        _temper_date = Convert.ToDateTime(temper_date);

                        row_temper = dt_temper_server.NewRow();
                        row_temper[0] = _temper_value;
                        row_temper[1] = _temper_date;
                        dt_temper_server.Rows.Add(row_temper);
                    }


                    __sql = "V2_BASE_Heating_Temperature_AddItem_V1o0";

                    SqlParameter[] sParams = new SqlParameter[3]; // Parameter count

                    foreach (DataRow dr in dt_temper_server.Rows)
                    {
                        sParams[0] = new SqlParameter();
                        sParams[0].SqlDbType = SqlDbType.TinyInt;
                        sParams[0].ParameterName = "@temper_value";
                        sParams[0].Value = dr[0].ToString();

                        sParams[1] = new SqlParameter();
                        sParams[1].SqlDbType = SqlDbType.DateTime;
                        sParams[1].ParameterName = "@temper_date";
                        sParams[1].Value = dr[1].ToString();

                        sParams[2] = new SqlParameter();
                        sParams[2].SqlDbType = SqlDbType.TinyInt;
                        sParams[2].ParameterName = "@temper_type";
                        sParams[2].Value = __temper_type;

                        cls.fnUpdDel(__sql, sParams);
                    }

                    dt_temper_server = null;

                    File.WriteAllText(__fle_server, String.Empty);
                }
            }
            catch { }
            finally { }
        }

        public void Fnc_Read_Temper_From_File(string file)
        {
            try
            {
                string
                    temper_line = "",
                    temper_value = "",
                    temper_date = "";

                int
                    _max_row = __max_temp_point_chart,
                    _split_pos = 0,
                    _temper_value = 0,
                    _temper_current = 0;

                DateTime
                    _temper_date = DateTime.Now;

                bool
                    bool_exist = (File.Exists(file)) ? true : false;

                DataTable
                    dt_temper = null;

                DataColumn
                    col_temper = null;

                DataRow
                    row_temper = null;

                if (bool_exist)
                {
                    dt_temper = new DataTable("temper");

                    dt_temper.Columns.Add("Temper", typeof(Int32));
                    dt_temper.Columns.Add("Date", typeof(DateTime));

                    //col_temper = new DataColumn();
                    //col_temper.DataType = typeof(Int32);
                    //col_temper.ColumnName = "Temper";
                    //dt_temper.Columns.Add(col_temper);

                    //col_temper = new DataColumn();
                    //col_temper.DataType = typeof(DateTime);
                    //col_temper.ColumnName = "Date";
                    //dt_temper.Columns.Add(col_temper);

                    var lines = File.ReadLines(file).Reverse();
                    foreach (var line in lines)
                    {
                        if (_max_row >= 0)
                        {
                            _split_pos = line.IndexOf(",");

                            temper_date = line.Substring(0, _split_pos);
                            temper_value = line.Substring(_split_pos + 1);

                            _temper_value = Convert.ToInt32(temper_value);
                            _temper_date = Convert.ToDateTime(temper_date);

                            row_temper = dt_temper.NewRow();
                            row_temper[0] = _temper_value;
                            row_temper[1] = _temper_date;
                            dt_temper.Rows.Add(row_temper);

                            if (_max_row == __max_temp_point_chart)
                            {
                                _temper_current = _temper_value;
                            }

                            _max_row -= 1;
                        }
                    }

                    __temper_now = _temper_current;

                    lbl_temper_now.Text = String.Format("{0}" + (char)186 + "C", _temper_current);
                    Fnc_Add_Temper_To_Chart(dt_temper);
                }
            }
            catch { }
            finally { }
        }

        public void Fnc_Load_Temper_Now_Blink()
        {
            Color
                upper01 = Color.LightPink, upper02 = Color.Tomato,
                equal01 = Color.LightGreen, equal02 = Color.Lime,
                lower01 = Color.Yellow, lower02 = Color.Gold,
                status01 = Color.Transparent, status02 = Color.Transparent;

            try
            {
                if (__temper_now >= __temper_min && __temper_now <= __temper_max)
                {
                    status01 = equal01;
                    status02 = equal02;
                }
                else if (__temper_now < __temper_min)
                {
                    status01 = lower01;
                    status02 = lower02;
                }
                else if (__temper_now > __temper_max)
                {
                    status01 = upper01;
                    status02 = upper02;
                }

                tlp_temper.BackColor = (__dt_sec % 2 == 0) ? status01 : status02;
                tlp_temper.ForeColor = Color.Black;
            }
            catch { }
            finally { }
        }

        public void Fnc_Add_Temper_To_Chart(DataTable dt)
        {
            try
            {
                if (chart_temper.Series[0].Points.Count > 50)
                {
                    chart_temper.Series[0].Points.RemoveAt(0);
                }

                chart_temper.Series[0].XValueMember = "Date";
                chart_temper.Series[0].YValueMembers = "Temper";
                chart_temper.DataSource = dt;

                if (__temper_now >= __temper_min && __temper_now <= __temper_max)
                {
                    chart_temper.ChartAreas[0].AxisY.Maximum = 90;
                    chart_temper.ChartAreas[0].AxisY.Minimum = 40;
                }

                chart_temper.ChartAreas[0].AxisX.MajorGrid.LineColor =
                    chart_temper.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromKnownColor(KnownColor.ControlDarkDark);

                dt = null;
                dt.Dispose();
            }
            catch { }
            finally { }
        }

        public void Fnc_Load_Data()
        {
            try
            {
                int
                    tblCnt = 0,
                    rowCnt = 0,
                    colCnt = 0;

                __sql = "V2_BASE_WB_Heating_ScanOut_List_SelItem_V1o1";

                __dt_data = new DataTable();
                __dt_data = cls.ExecuteDataTable(__sql);

                //Fnc_Load_Data_Ready();
                //Fnc_Load_Data_In();
                //Fnc_Load_Data_Out();
            }
            catch { }
            finally { }
        }

        public void Fnc_Load_Data_Ready()
        {
            try
            {
                //int
                //    rowCnt = 0;

                //DataView
                //    dt_view = new DataView(__dt_data);
                //dt_view.RowFilter = "box_out=0 and ready=1 and rank=1";

                //dgv_list_ready.DataSource = dt_view;
                //rowCnt = dgv_list_ready.Rows.Count;

                ////dgv_list_ready.Columns[0].FillWeight = 5;      //STT
                ////dgv_list_ready.Columns[1].FillWeight = 5;      //boxId
                //dgv_list_ready.Columns[2].FillWeight = 40;      //[code]
                ////dgv_list_ready.Columns[3].FillWeight = 5;      //boxcode
                ////dgv_list_ready.Columns[4].FillWeight = 5;      //prodId
                //dgv_list_ready.Columns[5].FillWeight = 60;      //boxpartname
                ////dgv_list_ready.Columns[6].FillWeight = 60;      //boxpartno
                ////dgv_list_ready.Columns[7].FillWeight = 15;      //boxquantity
                ////dgv_list_ready.Columns[8].FillWeight = 5;      //IN_Date
                ////dgv_list_ready.Columns[9].FillWeight = 5;      //box_out
                ////dgv_list_ready.Columns[10].FillWeight = 5;      //box_out_dt
                ////dgv_list_ready.Columns[11].FillWeight = 5;      //box_out_seq
                ////dgv_list_ready.Columns[12].FillWeight = 5;      //box_confirm
                ////dgv_list_ready.Columns[13].FillWeight = 5;      //confirm_dt
                ////dgv_list_ready.Columns[14].FillWeight = 5;      //[Heating second]
                ////dgv_list_ready.Columns[15].FillWeight = 5;      //[Ready]
                ////dgv_list_ready.Columns[16].FillWeight = 40;      //[Heating time]
                ////dgv_list_ready.Columns[17].FillWeight = 5;      //[Rank]
                ////dgv_list_ready.Columns[18].FillWeight = 5;      //temper

                //dgv_list_ready.Columns[0].Visible = false;
                //dgv_list_ready.Columns[1].Visible = false;
                //dgv_list_ready.Columns[2].Visible = true;
                //dgv_list_ready.Columns[3].Visible = false;
                //dgv_list_ready.Columns[4].Visible = false;
                //dgv_list_ready.Columns[5].Visible = true;
                //dgv_list_ready.Columns[6].Visible = false;
                //dgv_list_ready.Columns[7].Visible = false;
                //dgv_list_ready.Columns[8].Visible = false;
                //dgv_list_ready.Columns[9].Visible = false;
                //dgv_list_ready.Columns[10].Visible = false;
                //dgv_list_ready.Columns[11].Visible = false;
                //dgv_list_ready.Columns[12].Visible = false;
                //dgv_list_ready.Columns[13].Visible = false;
                //dgv_list_ready.Columns[14].Visible = false;
                //dgv_list_ready.Columns[15].Visible = false;
                //dgv_list_ready.Columns[16].Visible = false;
                //dgv_list_ready.Columns[17].Visible = false;
                //dgv_list_ready.Columns[18].Visible = false;

                //cls.fnFormatDatagridview_FullWidth(dgv_list_ready, 35, 50);

                //dgv_list_ready.BackgroundColor = Color.Black;

                //lbl_cart_ready.Text = "Xe sẵn sàng để xuất (" + rowCnt + ")";

                //Fnc_Load_Data_Ready_Color();
                //Fnc_Load_Data_Ready_Blink();
            }
            catch { }
            finally { }
        }

        public void Fnc_Load_Data_Ready_Color()
        {
            try
            {
                //if (dgv_list_ready.Rows.Count > 0)
                //{
                //    foreach (DataGridViewRow row in dgv_list_ready.Rows)
                //    {
                //        row.DefaultCellStyle.BackColor = Color.LightGreen;
                //        row.DefaultCellStyle.ForeColor = Color.Black;
                //    }
                //}

                //dgv_list_in.ClearSelection();
            }
            catch { }
            finally { }
        }

        public void Fnc_Load_Data_Ready_Blink()
        {
            try
            {
                //if (dgv_list_ready.Rows.Count > 0)
                //{
                //    foreach (DataGridViewRow row in dgv_list_ready.Rows)
                //    {
                //        row.DefaultCellStyle.BackColor = (__dt_sec % 2 == 0) ? Color.LightGreen : Color.LimeGreen;
                //    }
                //}
            }
            catch { }
            finally { }
        }

        public void Fnc_Load_Data_In()
        {
            try
            {
                int
                    rowCnt = 0;

                DataView
                    dt_view = new DataView(__dt_data);
                dt_view.RowFilter = "box_out=0";

                dgv_list_in.DataSource = dt_view;
                rowCnt = dgv_list_in.Rows.Count;

                //dgv_list_in.Columns[0].FillWeight = 10;      //STT
                //dgv_list_in.Columns[1].FillWeight = 5;      //boxId
                //dgv_list_in.Columns[2].FillWeight = 40;      //[code]
                dgv_list_in.Columns[3].FillWeight = 45;      //boxcode
                //dgv_list_in.Columns[4].FillWeight = 5;      //prodId
                dgv_list_in.Columns[5].FillWeight = 28;      //boxpartname
                //dgv_list_in.Columns[6].FillWeight = 25;      //boxpartno
                //dgv_list_in.Columns[7].FillWeight = 10;      //boxquantity
                //dgv_list_in.Columns[8].FillWeight = 23;      //IN_Date
                //dgv_list_in.Columns[9].FillWeight = 5;      //box_out
                //dgv_list_in.Columns[10].FillWeight = 5;      //box_out_dt
                //dgv_list_in.Columns[11].FillWeight = 5;      //box_out_seq
                //dgv_list_in.Columns[12].FillWeight = 5;      //box_confirm
                //dgv_list_in.Columns[13].FillWeight = 5;      //confirm_dt
                //dgv_list_in.Columns[14].FillWeight = 5;      //[Heating second]
                //dgv_list_in.Columns[15].FillWeight = 5;      //[Ready]
                dgv_list_in.Columns[16].FillWeight = 27;      //[Heating time]
                //dgv_list_in.Columns[17].FillWeight = 5;      //[Rank]
                //dgv_list_in.Columns[18].FillWeight = 5;      //temper

                dgv_list_in.Columns[0].Visible = false;
                dgv_list_in.Columns[1].Visible = false;
                dgv_list_in.Columns[2].Visible = false;
                dgv_list_in.Columns[3].Visible = true;
                dgv_list_in.Columns[4].Visible = false;
                dgv_list_in.Columns[5].Visible = true;
                dgv_list_in.Columns[6].Visible = false;
                dgv_list_in.Columns[7].Visible = false;
                dgv_list_in.Columns[8].Visible = false;
                dgv_list_in.Columns[9].Visible = false;
                dgv_list_in.Columns[10].Visible = false;
                dgv_list_in.Columns[11].Visible = false;
                dgv_list_in.Columns[12].Visible = false;
                dgv_list_in.Columns[13].Visible = false;
                dgv_list_in.Columns[14].Visible = false;
                dgv_list_in.Columns[15].Visible = false;
                dgv_list_in.Columns[16].Visible = true;
                dgv_list_in.Columns[17].Visible = false;
                dgv_list_in.Columns[18].Visible = false;

                dgv_list_in.Columns[8].DefaultCellStyle.Format = "dd/MM HH:mm";

                cls.fnFormatDatagridview_FullWidth(dgv_list_in, 13, 30);

                dgv_list_in.BackgroundColor = Color.Black;

                lbl_cart_wait.Text = "Xe đang trong phòng sấy (" + rowCnt + ")";

                tlp_heating_filter.Enabled = txt_heating_filter.Enabled = (rowCnt > 0) ? true : false;
                txt_heating_filter.Text = "";

                Fnc_Load_Data_In_Color();
                //Fnc_Load_Data_In_Blink();
            }
            catch { }
            finally { }
        }

        public void Fnc_Load_Data_In_Color()
        {
            try
            {
                string
                    ready = "",
                    rank = "";

                bool
                    _ready = false;

                int
                    _rank = 0;

                foreach (DataGridViewRow row in dgv_list_in.Rows)
                {
                    ready = row.Cells[15].Value.ToString();
                    rank = row.Cells[17].Value.ToString();

                    _ready = (ready.ToLower() == "true") ? true : false;
                    _rank = (rank.Length > 0) ? Convert.ToInt32(rank) : 0;

                    if (_rank == 1)
                    {
                        row.DefaultCellStyle.BackColor = (_ready == true) ? Color.LightGreen : Color.Yellow;
                        row.DefaultCellStyle.ForeColor = Color.Black;
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.Black;
                        row.DefaultCellStyle.ForeColor = Color.FromKnownColor(KnownColor.Control);
                    }

                }

                dgv_list_out.ClearSelection();
            }
            catch { }
            finally { }
        }

        public void Fnc_Load_Data_In_Blink()
        {
            try
            {
                string
                    ready = "",
                    rank = "";

                bool
                    _ready = false;

                int
                    _rank = 0;

                foreach (DataGridViewRow row in dgv_list_in.Rows)
                {
                    ready = row.Cells[15].Value.ToString();
                    rank = row.Cells[17].Value.ToString();

                    _ready = (ready.ToLower() == "true") ? true : false;
                    _rank = (rank.Length > 0) ? Convert.ToInt32(rank) : 0;

                    //if (_rank == 1)
                    if (_rank > 0)
                    {
                        switch (_ready)
                        {
                            case true:
                                row.DefaultCellStyle.BackColor = (__dt_sec % 2 == 0) ? Color.LightGreen : Color.LimeGreen;
                                break;
                            case false:
                                row.DefaultCellStyle.BackColor = (__dt_sec % 2 == 0) ? Color.Yellow : Color.Gold;
                                break;
                        }
                        row.DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
            }
            catch { }
            finally { }
        }

        public void Fnc_Load_Data_Out()
        {
            try
            {
                int
                    rowCnt = 0,
                    vw_range = (cbb_time.SelectedIndex > 0) ? cbb_time.SelectedIndex : 0;

                string
                    vw_time = "";

                //DataView
                //    dt_view = new DataView(__dt_data);

                //switch (vw_range)
                //{
                //    case 0:
                //        vw_time = " and datediff(day,box_out_dt,getdate())=0";
                //        break;
                //    case 1:
                //        vw_time = " and datediff(day,box_out_dt,getdate())<=3";
                //        break;
                //    case 2:
                //        vw_time = " and datediff(week,box_out_dt,getdate())=0";
                //        break;
                //    case 3:
                //        vw_time = " and datediff(week,box_out_dt,getdate())<=3";
                //        break;
                //    case 4:
                //        vw_time = " and datediff(month,box_out_dt,getdate())=0";
                //        break;
                //    case 5:
                //        vw_time = " and datediff(month,box_out_dt,getdate())<=3";
                //        break;
                //    case 6:
                //        vw_time = " and datediff(month,box_out_dt,getdate())<=6";
                //        break;
                //    case 7:
                //        vw_time = " and datediff(year,box_out_dt,getdate())=0";
                //        break;
                //    default:
                //        vw_time = " and 1=1";
                //        break;
                //}

                //dt_view.RowFilter = "box_out=1";
                //dt_view.Sort = "[Ngày xuất] desc";

                //dgv_list_out.DataSource = dt_view;

                DataTable
                    dt = new DataTable();

                __sql = "V2_BASE_WB_Heating_ScanOut_Data_SelItem_V1o0";

                SqlParameter[] sParams = new SqlParameter[2]; // Parameter count

                sParams[0] = new SqlParameter();
                sParams[0].SqlDbType = SqlDbType.TinyInt;
                sParams[0].ParameterName = "@box_type";
                sParams[0].Value = __temper_type;

                sParams[1] = new SqlParameter();
                sParams[1].SqlDbType = SqlDbType.TinyInt;
                sParams[1].ParameterName = "@range";
                sParams[1].Value = __range;

                dt = cls.ExecuteDataTable(__sql, sParams);
                dgv_list_out.DataSource = dt;
                rowCnt = dgv_list_out.Rows.Count;

                dgv_list_out.Columns[0].FillWeight = 5;      //STT
                //dgv_list_out.Columns[1].FillWeight = 5;      //boxId
                //dgv_list_out.Columns[2].FillWeight = 40;      //[code]
                dgv_list_out.Columns[3].FillWeight = 20;      //boxcode
                //dgv_list_out.Columns[4].FillWeight = 5;      //prodId
                dgv_list_out.Columns[5].FillWeight = 10;      //boxpartname
                dgv_list_out.Columns[6].FillWeight = 11;      //boxpartno
                dgv_list_out.Columns[7].FillWeight = 6;      //boxquantity
                dgv_list_out.Columns[8].FillWeight = 9;      //IN_Date
                //dgv_list_out.Columns[9].FillWeight = 5;      //box_out
                dgv_list_out.Columns[10].FillWeight = 9;      //box_out_dt
                //dgv_list_out.Columns[11].FillWeight = 5;      //box_out_seq
                //dgv_list_out.Columns[12].FillWeight = 5;      //box_confirm
                //dgv_list_out.Columns[13].FillWeight = 5;      //confirm_dt
                //dgv_list_out.Columns[14].FillWeight = 5;      //[Heating second]
                //dgv_list_out.Columns[15].FillWeight = 5;      //[Ready]
                dgv_list_out.Columns[16].FillWeight = 12;      //[Heating time]
                //dgv_list_out.Columns[17].FillWeight = 5;      //[Rank]
                dgv_list_out.Columns[18].FillWeight = 11;      //temper

                dgv_list_out.Columns[0].Visible = true;
                dgv_list_out.Columns[1].Visible = false;
                dgv_list_out.Columns[2].Visible = false;
                dgv_list_out.Columns[3].Visible = true;
                dgv_list_out.Columns[4].Visible = false;
                dgv_list_out.Columns[5].Visible = true;
                dgv_list_out.Columns[6].Visible = true;
                dgv_list_out.Columns[7].Visible = true;
                dgv_list_out.Columns[8].Visible = true;
                dgv_list_out.Columns[9].Visible = false;
                dgv_list_out.Columns[10].Visible = true;
                dgv_list_out.Columns[11].Visible = false;
                dgv_list_out.Columns[12].Visible = false;
                dgv_list_out.Columns[13].Visible = false;
                dgv_list_out.Columns[14].Visible = false;
                dgv_list_out.Columns[15].Visible = false;
                dgv_list_out.Columns[16].Visible = true;
                dgv_list_out.Columns[17].Visible = false;
                dgv_list_out.Columns[18].Visible = true;

                dgv_list_out.Columns[8].DefaultCellStyle.Format =
                    dgv_list_out.Columns[10].DefaultCellStyle.Format = "dd/MM HH:mm";

                cls.fnFormatDatagridview_FullWidth(dgv_list_out, 13, 30);

                dgv_list_out.BackgroundColor = Color.Black;

                //cbb_time.Enabled = (rowCnt > 0) ? true : false;
                txt_filter.Text = "";
                txt_filter.Enabled = (rowCnt > 0) ? true : false;

                lbl_list_out.Text = "Danh sách xe đã xuất (" + rowCnt + ")";

                Fnc_Load_Data_Out_Color();
            }
            catch { }
            finally { }
        }

        public void Fnc_Load_Data_Out_Color()
        {
            try
            {
                foreach (DataGridViewRow row in dgv_list_out.Rows)
                {
                    row.DefaultCellStyle.BackColor = Color.Black;
                    row.DefaultCellStyle.ForeColor = Color.FromKnownColor(KnownColor.Control);
                }
                dgv_list_out.ClearSelection();
            }
            catch { }
            finally { }
        }

        public void Fnc_Load_Data_Out_Click(DataGridViewCellEventArgs e)
        {
            string
                box_idx = "",
                temper_type = __temper_type;

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                cls.fnDatagridClickCell(dgv_list_out, e);

                DataGridViewRow row = new DataGridViewRow();
                row = dgv_list_out.Rows[e.RowIndex];

                box_idx = row.Cells[1].Value.ToString();

                frmFinishGoodScanOut_v1o2_Details details = new frmFinishGoodScanOut_v1o2_Details(box_idx, temper_type);
                details.ShowDialog();
            }
        }

        public void Fnc_Load_Fill_Chart()
        {
            try
            {
                Title title = new Title();
                title.Font = new Font("Times New Roman", 15, FontStyle.Bold);
                title.Text = "NHIỆT ĐỘ PHÒNG SẤY";
                title.ForeColor = Color.FromKnownColor(KnownColor.Control);
                chart_temper.Titles.Add(title);

                chart_temper.Series.Clear();
                Series series_temper = chart_temper.Series.Add("Temperature of Heating Room");
                series_temper.ChartType = SeriesChartType.Spline;
                series_temper.BorderWidth = 5;
                series_temper.Color = Color.Red;

                chart_temper.Legends[0].Docking = Docking.Bottom;
                chart_temper.Legends[0].Font = new Font("Times New Roman", 12, FontStyle.Regular);

                //chart_temper.ChartAreas[0].AxisX.Interval = 10;
                //chart_temper.ChartAreas[0].AxisX.Maximum = 100;
                //chart_temper.ChartAreas[0].AxisX.Minimum = 40;



                //chart_temper.Series.Add("Spec. MIN");
                //chart_temper.Series.Add("Spec. MAX");

                chart_temper.Series[0].XValueType = ChartValueType.DateTime;
                chart_temper.ChartAreas[0].AxisX.LabelStyle.Format = "dd/MM\r\nHH:mm";
                chart_temper.Series[0].IsValueShownAsLabel = true;
                chart_temper.Series[0].LabelForeColor = Color.FromKnownColor(KnownColor.Control);

                StripLine stripline_min = new StripLine();
                stripline_min.Interval = 0;
                stripline_min.IntervalOffset = 60;
                stripline_min.StripWidth = 1;
                stripline_min.BackColor = Color.DodgerBlue;
                chart_temper.ChartAreas[0].AxisY.StripLines.Add(stripline_min);

                StripLine stripline_max = new StripLine();
                stripline_max.Interval = 0;
                stripline_max.IntervalOffset = 80;
                stripline_max.StripWidth = 1;
                stripline_max.BackColor = Color.DodgerBlue;
                chart_temper.ChartAreas[0].AxisY.StripLines.Add(stripline_max);

                //chart_temper.Series[0].Points.AddXY(0, 95);
                //chart_temper.Series["Spec. MAX"].Points.AddXY(0, 80);

            }
            catch { }
            finally { }
        }

        public void Fnc_Load_Scan_Code()
        {
            try
            {
                byte
                    msg_code = 0;

                string
                    code = txt_code.Text.Trim(),
                    code_lower = txt_code.Text.Trim().ToLower().Replace("pro-", ""),
                    box_idx = "";

                frmFinishGoodScanOut_v1o2_Warning warning;

                if (code.Length > 0)
                {
                    if (code.Length >= 20)
                    {
                        string
                            code_type = code.Substring(0, 3),
                            code_kind = code.Substring(4, 3),
                            code_name = code.Substring(8);

                        bool
                            bool_valid_form = (code_type.ToLower() == "pro" && code_kind.ToLower() == "car" && code_name.Length == 12) ? true : false,
                            bool_valid_exist = Fnc_Load_Scan_Code_Check_Exist(code_lower),
                            //bool_valid_rank = Fnc_Load_Scan_Code_Check_Rank(code_lower),
                            bool_valid_rank = true,
                            bool_valid_ready = Fnc_Load_Scan_Code_Check_Ready(code_lower);

                        //MessageBox.Show(code_type + " | " + code_kind + " | " + code_name);
                        //MessageBox.Show(bool_valid_form.ToString() + "\r\n" + bool_valid_exist.ToString() + "\r\n" + bool_valid_rank.ToString() + "\r\n" + bool_valid_ready.ToString());
                        //return;

                        if (bool_valid_form == true && bool_valid_exist == true && bool_valid_rank == true && bool_valid_ready == true)
                        {
                            box_idx = Fnc_Get_BoxIDx_From_Code(code_lower);

                            //MessageBox.Show(box_idx);

                            __sql = "V2o1_ERP_Heating_Balance_ScanOut_Item_UpdItem_V1o0";

                            SqlParameter[] sParams = new SqlParameter[2]; // Parameter count

                            sParams[0] = new SqlParameter();
                            sParams[0].SqlDbType = SqlDbType.Int;
                            sParams[0].ParameterName = "@box_idx";
                            sParams[0].Value = box_idx;

                            sParams[1] = new SqlParameter();
                            sParams[1].SqlDbType = SqlDbType.TinyInt;
                            sParams[1].ParameterName = "@temper_type";
                            sParams[1].Value = __temper_type;

                            cls.fnUpdDel(__sql, sParams);

                            Fnc_Load_Data();
                            Fnc_Load_Data_In();
                            Fnc_Load_Data_Ready();
                            Fnc_Load_Data_Out();

                            txt_code.Text = "";
                            txt_code.Focus();

                            msg_code = 100;

                            warning = new frmFinishGoodScanOut_v1o2_Warning(msg_code);
                            warning.ShowDialog();
                            return;
                        }
                        else
                        {
                            txt_code.Text = "";
                            txt_code.Focus();

                            if (bool_valid_form == false)
                            {
                                msg_code = 1;

                                warning = new frmFinishGoodScanOut_v1o2_Warning(msg_code);
                                warning.ShowDialog();
                                return;
                            }

                            if (bool_valid_exist == false)
                            {
                                msg_code = 4;

                                warning = new frmFinishGoodScanOut_v1o2_Warning(msg_code);
                                warning.ShowDialog();
                                return;
                            }

                            if (bool_valid_rank == false)
                            {
                                msg_code = 2;

                                warning = new frmFinishGoodScanOut_v1o2_Warning(msg_code);
                                warning.ShowDialog();
                                return;
                            }

                            if (bool_valid_ready == false)
                            {
                                msg_code = 3;

                                warning = new frmFinishGoodScanOut_v1o2_Warning(msg_code);
                                warning.ShowDialog();
                                return;
                            }
                        }

                    }
                    else
                    {
                        txt_code.Text = "";
                        txt_code.Focus();

                        msg_code = 1;

                        warning = new frmFinishGoodScanOut_v1o2_Warning(msg_code);
                        warning.ShowDialog();
                        return;
                    }
                }
            }
            catch { }
            finally { }
        }

        public string Fnc_Get_BoxIDx_From_Code(string code)
        {
            string
                idx = "";

            string
                exist_code = "";

            int
                _exist = 0;
            try
            {

                foreach (DataGridViewRow row in dgv_list_in.Rows)
                {
                    exist_code = row.Cells[3].Value.ToString().ToLower().Replace("pro-", "");

                    if (code == exist_code)
                    {
                        idx = row.Cells[1].Value.ToString();
                        _exist += 1;
                    }
                }

                if (_exist > 1) { idx = ""; }
            }
            catch { }
            finally { }

            return idx;
        }

        public bool Fnc_Load_Scan_Code_Check_Exist(string code)
        {
            bool
                bool_valid_exist = false;

            string
                exist_code = "";

            int
                _exist = 0;
            try
            {

                foreach (DataGridViewRow row in dgv_list_in.Rows)
                {
                    exist_code = row.Cells[3].Value.ToString().ToLower().Replace("pro-", "");

                    //if (code.ToLower().Contains(exist_code))
                    if (code == exist_code)
                    {
                        _exist += 1;
                    }
                }

                bool_valid_exist = (_exist == 1) ? true : false;
            }
            catch { }
            finally { }

            return bool_valid_exist;
        }

        public bool Fnc_Load_Scan_Code_Check_Rank(string code)
        {
            bool
                bool_valid_rank = false;

            string
                exist_code = "",
                rank = "";

            int
                _rank = 0,
                _exist = 0;

            try
            {
                foreach (DataGridViewRow row in dgv_list_in.Rows)
                {
                    exist_code = row.Cells[3].Value.ToString().ToLower().Replace("pro-", "");

                    //if (code.ToLower().Contains(exist_code))
                    if (code == exist_code)
                    {
                        rank = row.Cells[17].Value.ToString();
                        _rank = (rank.Length > 0) ? Convert.ToInt32(rank) : 0;

                        if (_rank == 1) { _exist += 1; }
                    }
                    else
                    {

                    }
                }

                bool_valid_rank = (_exist == 1) ? true : false;
            }
            catch { }
            finally { }

            return bool_valid_rank;
        }

        public bool Fnc_Load_Scan_Code_Check_Ready(string code)
        {
            bool
                bool_valid_ready = false;

            string
                exist_code = "",
                ready = "";

            bool
                _ready = false;

            int
                _exist = 0;

            try
            {
                foreach (DataGridViewRow row in dgv_list_in.Rows)
                {
                    exist_code = row.Cells[3].Value.ToString().ToLower().Replace("pro-", "");

                    //if (code.ToLower().Contains(exist_code))
                    if (code == exist_code)
                    {
                        ready = row.Cells[15].Value.ToString();
                        _ready = (ready.ToLower() == "true") ? true : false;

                        if (_ready == true) { _exist += 1; }
                    }
                    else
                    {
                        bool_valid_ready = false;
                    }
                }

                bool_valid_ready = (_exist == 1) ? true : false;
            }
            catch { }
            finally { }

            return bool_valid_ready;
        }

        /*************************************************/

        private void __timer_main_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                __dt_now = DateTime.Now;

                __dt_sec = __dt_now.Second;
                __dt_min = __dt_now.Minute;
                __dt_hrs = __dt_now.Hour;
                __dt_day = __dt_now.Day;
                __dt_month = __dt_now.Month;
                __dt_year = __dt_now.Year;

                //Fnc_Set_Time_Shift();
                Fnc_Get_Temper_Filename();

                Fnc_Set_Refresh_Data();
                if (__bool_refresh_data)
                {
                    Fnc_Load_Data();
                    Fnc_Load_Data_Ready();
                    Fnc_Load_Data_In();

                    __bool_refresh_data = false;
                }

                Fnc_Set_Refresh_Temper();
                if (__bool_refresh_temp)
                {
                    Fnc_Get_Temper_Spec();
                    Fnc_Read_Temper_From_File(__fle_local);

                    __bool_refresh_temp = false;
                }

                lbl_datetime.Text = String.Format("{0:dd/MM/yyyy HH:mm:ss}", __dt_now);

                //Fnc_Load_Data_Ready_Blink();
                //Fnc_Load_Data_In_Blink();
                Fnc_Load_Temper_Now_Blink();
            }
            catch { }
            finally { }
        }

        private void cbb_time_SelectionChangeCommitted(object sender, EventArgs e)
        {
            __range = cbb_time.SelectedIndex + 1;
            Fnc_Load_Data_Out();
        }

        private void lbl_temper_now_MouseClick(object sender, MouseEventArgs e)
        {
            Fnc_Read_Temper_From_File(__fle_local);
        }

        private void frmFinishGoodScanOut_v1o2_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Có chắc là bạn muốn tắt chương trình này ?", cls.appName(), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void txt_code_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                Fnc_Load_Scan_Code();
            }
        }

        private void txt_filter_TextChanged(object sender, EventArgs e)
        {
            cls.fnFilterDatagridRow(dgv_list_out, txt_filter, 3);
            Fnc_Load_Data_Out_Color();
        }

        private void lbl_list_out_Click(object sender, EventArgs e)
        {
            Fnc_Load_Data_Out();
        }

        private void dgv_list_out_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (__bool_view_details) { Fnc_Load_Data_Out_Click(e); }
        }

        private void dgv_list_out_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //Fnc_Load_Data_Out_Click(e);
        }

        private void tlp_code_Click(object sender, EventArgs e)
        {
            txt_code.Focus();
        }

        private void tlp_code_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.IBeam;
        }

        private void tlp_code_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void frmFinishGoodScanOut_v1o2_MouseMove(object sender, MouseEventArgs e)
        {
            //__timer_focus.Enabled = true;
        }

        private void txt_heating_filter_TextChanged(object sender, EventArgs e)
        {
            cls.fnFilterDatagridRow(dgv_list_in, txt_heating_filter, 3);
            Fnc_Load_Data_In_Color();
            dgv_list_in.ClearSelection();
        }

        private void lbl_cart_wait_Click(object sender, EventArgs e)
        {
            Fnc_Load_Data_In();
        }
    }
}
