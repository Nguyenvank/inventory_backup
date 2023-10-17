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
using System.Windows.Forms.DataVisualization.Charting;

namespace Inventory_Data
{
    public partial class frmFinishGoodScanOut_v1o3_Details : Form
    {
        System.Timers.Timer
            __timer_main = new System.Timers.Timer();

        DateTime
            __dt_now = DateTime.Now;

        string
            __sql = "",
            __app = cls.appName(),
            __box_idx = "",
            __temper_type = "";

        DataSet
            __ds_data = null;

        DataTable
            __dt_data = null;

        int
            __tblCnt = 0,
            __rowCnt = 0,
            __colCnt = 0,
            __dt_sec = 0,
            __dt_min = 0,
            __dt_hrs = 0;

        public frmFinishGoodScanOut_v1o3_Details()
        {
            InitializeComponent();
        }

        public frmFinishGoodScanOut_v1o3_Details(string box_idx,string temper_type)
        {
            InitializeComponent();

            __box_idx = box_idx;
            __temper_type = temper_type;

            __timer_main.Interval = 1000;
            __timer_main.Enabled = true;
            __timer_main.Elapsed += __timer_main_Elapsed;

            this.Text = "THÔNG TIN CHI TIẾT XE ĐÃ XUẤT [" + String.Format("{0:dd/MM/yyyy HH:mm:ss}", __dt_now) + "]";

            cls.SetDoubleBuffer(tlp_main, true);
        }

        private void frmFinishGoodScanOut_v1o3_Details_Load(object sender, EventArgs e)
        {
            Fnc_Load_Init();
        }

        private void frmFinishGoodScanOut_v1o3_Details_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        public void Fnc_Load_Init()
        {
            Fnc_Load_Controls();
        }

        /****************************************************/

        public void Fnc_Load_Controls()
        {
            lbl_cart_code.Text =
                lbl_model_name.Text = lbl_model_code.Text = lbl_cart_qty.Text =
                lbl_heating_time.Text = lbl_temper_min.Text = lbl_temper_max.Text =
                lbl_scan_in.Text = lbl_scan_out.Text = "";

            chart_temper.Series.Clear();

            Fnc_Load_Cart_Info();
            Fnc_Load_Fill_Chart();
            Fnc_Load_Cart_Temper();
        }

        public void Fnc_Load_Cart_Info()
        {
            try
            {
                string
                    box_idx = __box_idx,
                    temper_type = __temper_type,
                    box_code = "",
                    model_name = "", model_code = "", box_qty = "",
                    heating_time = "", temper_min = "", temper_max = "",
                    box_in = "", box_out = "",
                    spec_temper_min = "", spec_temper_max = "", spec_heating_time = "", spec_apply_date = "", spec_created_by = "";

                int
                    _box_qty = 0,
                    _temper_min = 0, _temper_max = 0,
                    _spec_temper_min = 0, _spec_temper_max = 0;

                decimal
                    _heating_time = 0,
                    _spec_heating_time = 0;

                DateTime
                    _box_in = DateTime.Now, _box_out = DateTime.Now,
                    _spec_apply_date = DateTime.Now;

                __sql = "V2o1_ERP_Heating_Balance_ScanOut_Item_Info_SelItem_V1o0";

                SqlParameter[] sParams = new SqlParameter[2]; // Parameter count

                sParams[0] = new SqlParameter();
                sParams[0].SqlDbType = SqlDbType.Int;
                sParams[0].ParameterName = "@box_idx";
                sParams[0].Value = box_idx;

                sParams[1] = new SqlParameter();
                sParams[1].SqlDbType = SqlDbType.TinyInt;
                sParams[1].ParameterName = "@temper_type";
                sParams[1].Value = temper_type;

                __ds_data = cls.ExecuteDataSet(__sql, sParams);
                __tblCnt = __ds_data.Tables.Count;
                __rowCnt = __ds_data.Tables[0].Rows.Count;

                if (__tblCnt > 0 && __rowCnt > 0)
                {
                    box_code = __ds_data.Tables[0].Rows[0][0].ToString();
                    model_name = __ds_data.Tables[0].Rows[0][1].ToString();
                    model_code = __ds_data.Tables[0].Rows[0][2].ToString();
                    box_qty = __ds_data.Tables[0].Rows[0][3].ToString();
                    heating_time = __ds_data.Tables[0].Rows[0][4].ToString();
                    temper_min = __ds_data.Tables[0].Rows[0][5].ToString();
                    temper_max = __ds_data.Tables[0].Rows[0][6].ToString();
                    box_in = __ds_data.Tables[0].Rows[0][7].ToString();
                    box_out = __ds_data.Tables[0].Rows[0][8].ToString();
                    spec_temper_min = __ds_data.Tables[0].Rows[0][9].ToString();
                    spec_temper_max = __ds_data.Tables[0].Rows[0][10].ToString();
                    spec_heating_time = __ds_data.Tables[0].Rows[0][11].ToString();
                    spec_apply_date = __ds_data.Tables[0].Rows[0][12].ToString();
                    spec_created_by = __ds_data.Tables[0].Rows[0][13].ToString();

                    _box_qty = (box_qty.Length > 0) ? Convert.ToInt32(box_qty) : 0;
                    //_heating_time = (heating_time.Length > 0) ? Convert.ToDecimal(heating_time) : 0;
                    _temper_min = (temper_min.Length > 0) ? Convert.ToInt32(temper_min) : 0;
                    _temper_max = (temper_max.Length > 0) ? Convert.ToInt32(temper_max) : 0;
                    _box_in = Convert.ToDateTime(box_in);
                    _box_out = Convert.ToDateTime(box_out);
                    _spec_temper_min = (spec_temper_min.Length > 0) ? Convert.ToInt32(spec_temper_min) : 0;
                    _spec_temper_max = (spec_temper_max.Length > 0) ? Convert.ToInt32(spec_temper_max) : 0;
                    _spec_heating_time = (spec_heating_time.Length > 0) ? Convert.ToDecimal(spec_heating_time) : 0;
                    _spec_apply_date = Convert.ToDateTime(spec_apply_date);

                    lbl_cart_code.Text = box_code.ToUpper();
                    lbl_model_name.Text = model_name;
                    lbl_model_code.Text = model_code;
                    lbl_cart_qty.Text = box_qty + " ea";
                    lbl_heating_time.Text = heating_time;
                    lbl_temper_min.Text = String.Format("{0}" + (char)186 + "C", _temper_min);
                    lbl_temper_max.Text = String.Format("{0}" + (char)186 + "C", _temper_max);
                    lbl_scan_in.Text = String.Format("{0:dd/MM/yyyy HH:mm:ss}", _box_in);
                    lbl_scan_out.Text = String.Format("{0:dd/MM/yyyy HH:mm:ss}", _box_out);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy dữ liệu !!!");
                    this.Close();
                }
            }
            catch { }
            finally { }
        }

        public void Fnc_Load_Cart_Temper()
        {
            try
            {
                string
                    box_idx = __box_idx,
                    temper_type = __temper_type;

                __sql = "V2o1_ERP_Heating_Balance_ScanOut_Item_Temper_SelItem_V1o0";

                SqlParameter[] sParams = new SqlParameter[2]; // Parameter count

                sParams[0] = new SqlParameter();
                sParams[0].SqlDbType = SqlDbType.Int;
                sParams[0].ParameterName = "@box_idx";
                sParams[0].Value = box_idx;

                sParams[1] = new SqlParameter();
                sParams[1].SqlDbType = SqlDbType.TinyInt;
                sParams[1].ParameterName = "@temper_type";
                sParams[1].Value = temper_type;

                __dt_data = cls.ExecuteDataTable(__sql, sParams);

                chart_temper.Series[0].XValueMember = "Date";
                chart_temper.Series[0].YValueMembers = "Temper";
                chart_temper.DataSource = __dt_data;

                chart_temper.ChartAreas[0].AxisX.MajorGrid.LineColor =
                    chart_temper.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gainsboro;
            }
            catch { }
            finally { }
        }

        public void Fnc_Load_Fill_Chart()
        {
            try
            {
                Title title = new Title();
                title.Font = new Font("Times New Roman", 15, FontStyle.Bold);
                title.Text = "NHIỆT ĐỘ SẤY";
                chart_temper.Titles.Add(title);

                chart_temper.Series.Clear();
                Series series_temper = chart_temper.Series.Add("Temperature of Heating");
                series_temper.ChartType = SeriesChartType.Spline;
                series_temper.BorderWidth = 1;
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


        /****************************************************/

        private void __timer_main_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            __dt_now = DateTime.Now;

            __dt_sec = __dt_now.Second;
            __dt_min = __dt_now.Minute;
            __dt_hrs = __dt_now.Hour;

            this.Text = "THÔNG TIN CHI TIẾT XE ĐÃ XUẤT [" + String.Format("{0:dd/MM/yyyy HH:mm:ss}", __dt_now) + "]";
        }
    }
}
