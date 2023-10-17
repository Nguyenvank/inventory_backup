using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_Data
{
    public partial class frmFinishGoodScanOut_v1o2_Warning : Form
    {
        System.Timers.Timer
            __timer_main = new System.Timers.Timer();

        DateTime
            __dt_now = DateTime.Now;

        int
            __dt_hrs = 0,
            __dt_min = 0,
            __dt_sec = 0,
            __close = 10;

        byte
            __err_code = 0;

        public frmFinishGoodScanOut_v1o2_Warning()
        {
            InitializeComponent();
        }

        public frmFinishGoodScanOut_v1o2_Warning(byte err_code)
        {
            InitializeComponent();

            __err_code = err_code;

            switch (__err_code)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    this.BackColor = Color.Firebrick;
                    lbl_err_msg_01.BackColor = Color.LightCoral;
                    lbl_err_msg_02.BackColor = lbl_err_msg_03.BackColor = Color.Firebrick;
                    break;
                case 100:
                    this.BackColor = Color.Blue;
                    lbl_err_msg_01.BackColor = Color.DeepSkyBlue;
                    lbl_err_msg_02.BackColor = lbl_err_msg_03.BackColor = Color.Blue;
                    break;
            }

            __timer_main.Interval = 1000;
            __timer_main.Enabled = true;
            __timer_main.Elapsed += __timer_main_Elapsed;
        }

        private void frmFinishGoodScanOut_v1o2_Warning_Load(object sender, EventArgs e)
        {
            Fnc_Load_Init();
        }

        public void Fnc_Load_Init()
        {
            Fnc_Load_Controls();
        }

        /***************************************/

        public void Fnc_Load_Controls()
        {
            string
                err_msg_01 = "",
                err_msg_02 = "";

            lbl_err_msg_01.Text = lbl_err_msg_02.Text = lbl_err_msg_03.Text = "";

            switch (__err_code)
            {
                case 1:
                    err_msg_01 = "SAI ĐỊNH DẠNG TEM";
                    err_msg_02 = "Hãy kiểm tra lại tem và quét lại";
                    break;
                case 2:
                    err_msg_01 = "LỖI NHẬP TRƯỚC XUẤT TRƯỚC (FIFO)";
                    err_msg_02 = "Hãy kiểm tra lại thứ tự nhập xuất";
                    break;
                case 3:
                    err_msg_01 = "XE CHƯA ĐỦ THỜI GIAN SẤY";
                    err_msg_02 = "Không thể xuất xe khi chưa đủ thời gian sấy";
                    break;
                case 4:
                    err_msg_01 = "KHÔNG TÌM THẤY MÃ XE TRÊN HỆ THỐNG";
                    err_msg_02 = "Không tìm thấy xe hàng tương ứng trên hệ thống";
                    break;
                case 100:
                    err_msg_01 = "THÀNH CÔNG !";
                    err_msg_02 = "Hệ thống đã ghi nhận xuất xe hàng thành công";
                    break;
            }

            lbl_err_msg_01.Text = err_msg_01;
            lbl_err_msg_02.Text = err_msg_02;
        }

        /***************************************/

        private void __timer_main_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            __dt_now = DateTime.Now;

            __dt_hrs = __dt_now.Hour;
            __dt_min = __dt_now.Minute;
            __dt_sec = __dt_now.Second;

            switch (__err_code)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    this.BackColor = lbl_err_msg_02.BackColor = lbl_err_msg_03.BackColor = (__dt_sec % 2 == 0) ? Color.Firebrick : Color.IndianRed;

                    if (__close >= 0)
                    {
                        __close -= 1;

                        lbl_err_msg_03.Text = "(tự động đóng sau " + String.Format("{0:00}", __close) + " giây)";

                        if (__close == 0)
                        {
                            this.Close();
                        }
                    }
                    break;
                case 100:
                    this.BackColor = lbl_err_msg_02.BackColor = lbl_err_msg_03.BackColor = (__dt_sec % 2 == 0) ? Color.Blue : Color.DodgerBlue;

                    if (__close >= 0)
                    {
                        __close -= 1;

                        lbl_err_msg_03.Text = "(tự động đóng sau " + String.Format("{0:00}", __close) + " giây)";

                        if (__close == 0)
                        {
                            this.Close();
                        }
                    }
                    break;
            }
        }
    }
}
