using System;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Modbus.Device;
using Modbus.Utility;
using Modbus_Poll_CS;


namespace Inventory_Data
{
    public partial class frm_Heating_Room_Temperature_v1o0 : Form
    {
        //short[] Value_FC03 = new short[16];

        modbus mb = new modbus();

        System.Timers.Timer
            __timer_main = new System.Timers.Timer();

        DateTime
            __dt_now = DateTime.Now;

        SerialPort
            __port = null;

        string
            __port_name = "";

        int
            __port_baud = 9600,
            __port_data = 8;

        Parity
            __port_parity = Parity.None;

        StopBits
            __port_stop = StopBits.None;

        string
            __sql = "",
            __app = "",
            __str_text01 = "",
            __str_text02 = "",
            __str_text03 = "",
            __str_text04 = "",
            __str_text05 = "";

        int
            __dt_sec = 0,
            __dt_min = 0,
            __dt_hrs = 0,
            pollCount = 0;

        bool
            isPolling = false;

        public frm_Heating_Room_Temperature_v1o0()
        {
            InitializeComponent();

            __timer_main.Interval = 1000;
            __timer_main.Enabled = true;
            __timer_main.Elapsed += __timer_main_Elapsed;
        }

        private void frm_Heating_Room_Temperature_v1o0_Load(object sender, EventArgs e)
        {
            Fnc_Load_Init();
        }

        public void Fnc_Load_Init()
        {
            Fnc_Load_Controls();

            Fnc_Load_Connect_RS485();
            /*
            
            this.cbb_ports.Items.AddRange(new object[] {
            "COM 1",
            "COM 2",
            "COM 3"});

             */
        }

        /********************************************************/

        public void Fnc_Load_Controls()
        {
            cbb_ports.Enabled = true;
            cbb_baud.Enabled = cbb_data.Enabled = cbb_parity.Enabled = cbb_stop.Enabled = false;
            btn_connect.Enabled = btn_disconnect.Enabled = false;
            lbl_status.Text = "";
            lbl_status.BackColor = Color.FromKnownColor(KnownColor.Control);
            lbl_temper_LL.Text = lbl_temper_PV.Text = lbl_temper_HH.Text = "0";
            chart_temper.Series.Clear();

            cbb_ports.Items.Clear();
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                cbb_ports.Items.Add(port);
            }
            cbb_ports.Items.Insert(0, "");
            cbb_ports.SelectedIndex = 0;

            cbb_baud.Items.Clear();
            cbb_baud.Items.Add("2400");
            cbb_baud.Items.Add("4800");
            cbb_baud.Items.Add("7200");
            cbb_baud.Items.Add("9600");
            cbb_baud.Items.Add("14400");
            cbb_baud.Items.Add("19200");
            cbb_baud.Items.Add("38400");
            cbb_baud.Items.Insert(0, "");
            cbb_baud.SelectedIndex = 0;

            cbb_data.Items.Clear();
            cbb_data.Items.Add("7");
            cbb_data.Items.Add("8");
            cbb_data.Items.Insert(0, "");
            cbb_data.SelectedIndex = 0;

            cbb_parity.Items.Clear();
            cbb_parity.Items.Add("Even");
            cbb_parity.Items.Add("Odd");
            cbb_parity.Items.Add("None");
            cbb_parity.Items.Add("Mark");
            cbb_parity.Items.Add("Space");
            cbb_parity.Items.Insert(0, "");
            cbb_parity.SelectedIndex = 0;

            cbb_stop.Items.Clear();
            cbb_stop.Items.Add("1");
            cbb_stop.Items.Add("1.5");
            cbb_stop.Items.Add("2");
            cbb_stop.Items.Insert(0, "");
            cbb_stop.SelectedIndex = 0;
        }

        public void Fnc_Load_Connect_RS485()
        {
            cbb_ports.Text = "COM20";
            cbb_baud.Text = "9600";
            cbb_parity.Text = "None";
            cbb_data.Text = "8";
            cbb_stop.Text = "1";

            __port = new SerialPort("COM20", 9600, Parity.None, 8, StopBits.One);
            __port.ReadTimeout = 2000;

            if (__port.IsOpen) { __port.Close(); }
            __port.Open();
            __port.DataReceived += new SerialDataReceivedEventHandler(__port_DataReceived);

            lbl_status.BackColor = Color.LightGreen;
            cbb_ports.Enabled =
                cbb_baud.Enabled =
                cbb_data.Enabled =
                cbb_parity.Enabled =
                cbb_stop.Enabled =
                btn_connect.Enabled = false;
            btn_disconnect.Enabled = true;
        }

        public void Fnc_Load_Disconnect_RS485()
        {
            if (__port.IsOpen) { __port.Close(); }

            Fnc_Load_Controls();
        }

        public void Fnc_Read_RS485_Data()
        {

        }

        /********************************************************/

        private void __timer_main_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            __dt_now = DateTime.Now;

            __dt_hrs = __dt_now.Hour;
            __dt_min = __dt_now.Minute;
            __dt_sec = __dt_now.Second;

            //modbusrtu.Instance().FC03(1, 120, 2, ref Value_FC03);

            //lbl_temper_PV.Text = Value_FC03[0].ToString();
        }

        private void cbb_ports_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int
                sel = cbb_ports.SelectedIndex;

            if (sel > 0)
            {
                cbb_baud.Enabled =
                    cbb_data.Enabled =
                    cbb_parity.Enabled =
                    cbb_stop.Enabled =
                    btn_connect.Enabled = true;
            }
            else
            {
                Fnc_Load_Controls();
            }
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            //__port_name = cbb_ports.Text.ToUpper();
            //__port_baud = Convert.ToInt32(cbb_baud.Text.Trim());
            //__port_data = Convert.ToInt32(cbb_data.Text.Trim());
            //switch (cbb_parity.SelectedIndex)
            //{
            //    case 1:
            //        __port_parity = Parity.Even;
            //        break;
            //    case 2:
            //        __port_parity = Parity.Odd;
            //        break;
            //    case 3:
            //        __port_parity = Parity.None;
            //        break;
            //    case 4:
            //        __port_parity = Parity.Mark;
            //        break;
            //    case 5:
            //        __port_parity = Parity.Space;
            //        break;
            //}
            //switch (cbb_stop.SelectedIndex)
            //{
            //    case 1:
            //        __port_stop = StopBits.One;
            //        break;
            //    case 2:
            //        __port_stop = StopBits.OnePointFive;
            //        break;
            //    case 3:
            //        __port_stop = StopBits.Two;
            //        break;
            //}

            //__port = new SerialPort(__port_name, __port_baud, __port_parity, __port_data, __port_stop);
            //__port.DataReceived += new SerialDataReceivedEventHandler(__port_DataReceived);

            //if (__port.IsOpen) { __port.Close(); }
            //__port.Open();

            //lbl_status.BackColor = Color.LightGreen;
            //cbb_ports.Enabled =
            //    cbb_baud.Enabled =
            //    cbb_data.Enabled =
            //    cbb_parity.Enabled =
            //    cbb_stop.Enabled =
            //    btn_connect.Enabled = false;
            //btn_disconnect.Enabled = true;

            Fnc_Load_Connect_RS485();
        }

        public byte[] Read_PV()
        {
            byte[] frame = new byte[6];
            frame = StringToByteArray("010303E80001");
            return frame;
        }

        public byte[] Read_LL()
        {
            byte[] frame = new byte[6];
            frame = StringToByteArray("010300A00001");
            return frame;
        }

        public byte[] Read_HH()
        {
            byte[] frame = new byte[6];
            frame = StringToByteArray("010300A10001");
            return frame;
        }

        private byte[] ReadHoldingRegistersMsg(byte[] subframe)
        {
            byte[] frame = new byte[8];

            frame[0] = subframe[0];   // slaveAddress;
            frame[1] = subframe[1];   // function;
            frame[2] = subframe[2];   // startAddress01;
            frame[3] = subframe[3];   // startAddress02;
            frame[4] = subframe[4];   // register01;
            frame[5] = subframe[5];   // register02;
            byte[] crc = CalculateCRC(frame);
            frame[frame.Length - 2] = crc[0];       // Error Check Low
            frame[frame.Length - 1] = crc[1];

            //byte[] data = { 1, 16, 0, 0, 0, 2, 4, value01, value02, value03, value04, crc[0], crc[1] };


            return frame;
        }

        private byte[] CalculateCRC(byte[] data)
        {
            ushort CRCFull = 0xFFFF; // Set the 16-bit register (CRC register) = FFFFH.
            byte CRCHigh = 0xFF, CRCLow = 0xFF;
            char CRCLSB;
            byte[] CRC = new byte[2];
            for (int i = 0; i < (data.Length) - 2; i++)
            {
                CRCFull = (ushort)(CRCFull ^ data[i]); // 

                for (int j = 0; j < 8; j++)
                {
                    CRCLSB = (char)(CRCFull & 0x0001);
                    CRCFull = (ushort)((CRCFull >> 1) & 0x7FFF);

                    if (CRCLSB == 1)
                        CRCFull = (ushort)(CRCFull ^ 0xA001);
                }
            }
            CRC[1] = CRCHigh = (byte)((CRCFull >> 8) & 0xFF);
            CRC[0] = CRCLow = (byte)(CRCFull & 0xFF);
            return CRC;
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:X2} ", b);
            return hex.ToString();
        }

        private void __port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;

            ////if (!__port.IsOpen) { __port.Open(); }

            //////Thread.Sleep(100);

            int
                length_PV = __port.BytesToRead;

            byte[]
                data_PV = ReadHoldingRegistersMsg(Read_PV());

            lbl_hex_PV.Text = ByteArrayToString(data_PV);
            lbl_temper_PV.Text = sp.Read(data_PV, 0, length_PV).ToString();

            this.Invoke(new EventHandler(DoUpdate));

            //Thread.Sleep(100);

            //int
            //    length_LL = __port.BytesToRead;

            //byte[]
            //    data_LL = ReadHoldingRegistersMsg(Read_LL());

            //lbl_hex_LL.Text = ByteArrayToString(data_LL);
            //lbl_temper_LL.Text = __port.Read(data_LL, 0, length_LL).ToString();

            //Thread.Sleep(100);

            //int
            //    length_HH = __port.BytesToRead;

            //byte[]
            //    data_HH = ReadHoldingRegistersMsg(Read_HH());

            //lbl_hex_HH.Text = ByteArrayToString(data_HH);
            //lbl_temper_HH.Text = __port.Read(data_HH, 0, length_HH).ToString();




            //string
            //    port_data = __port.ReadLine(),
            //    str_data = "",
            //    str_ascii = "";


            //if (__port.Read(data, 0, data_length) != 0)
            //{
            //    str_data = Encoding.UTF8.GetString(data);
            //    str_ascii = Regex.Replace(str_data, "[^\\u0020-\\u007E]", string.Empty);
            //    str_ascii = Regex.Replace(str_ascii, "\\r\\n", "");

            //    lbl_data.Text = str_ascii;
            //}

        }

        public void DoUpdate(object s, EventArgs e)
        {

        }

        private void btn_disconnect_Click(object sender, EventArgs e)
        {
            Fnc_Load_Disconnect_RS485();
        }

        private void frm_Heating_Room_Temperature_v1o0_FormClosing(object sender, FormClosingEventArgs e)
        {
            Fnc_Load_Disconnect_RS485();
        }

    }
}
