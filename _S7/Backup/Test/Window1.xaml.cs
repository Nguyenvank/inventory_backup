using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace S7
{
    /// <summary>
    /// Interaktionslogik für Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        System.ComponentModel.BackgroundWorker worker = null;
        System.Threading.Timer timer = null;
        S7.PLC plc = null;

        public Window1()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(Window1_Loaded);
            Closing += new System.ComponentModel.CancelEventHandler(Window1_Closing);
        }

        void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            plc = new PLC(CPU_Type.S7300, "192.168.1.130", 0, 2);
            ErrorCode errCode = plc.Open();
            if (errCode == ErrorCode.NoError)
            {
                // reads a value defined by a string and converts it to a double value
                double v = Types.Double.FromDWord((uint)plc.Read("DB2.DBD6"));

                // reads a struct from DataBlock 1
                testStruct test = (testStruct)plc.ReadStruct(typeof(testStruct), 1);
                // and writes it to DB2
                S7.ErrorCode code = plc.WriteStruct(test, 2);

                //timer = new System.Threading.Timer(Worker, null, 500, 500);
            }
        }

        void Window1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (plc.IsConnected)
            {
                if (timer != null)
                    timer.Dispose();
                plc.Close();
            }
        }

        #region unused at the moment
        //private void Worker(object data)
        //{
        //    ctrlPlc0.Dispatcher.Invoke(new UpdateValuesDelegate(UpdateValues), null);
        //    System.Threading.Thread.Sleep(10);
        //}

        //public delegate void UpdateValuesDelegate();
        //private void UpdateValues()
        //{
        //    if (plc.IsConnected)
        //    {
        //        byte[] byte01 = (byte[])plc.Read(DataType.Input, 0, 4, VarType.Byte, 2);
        //        byte[] byte23 = (byte[])plc.Read(DataType.Output, 0, 4, VarType.Byte, 2);
        //        ctrlPlc0.ValueByte0 = byte01[0];
        //        ctrlPlc0.ValueByte1 = byte01[1];
        //        ctrlPlc0.ValueByte2 = byte23[0];
        //        ctrlPlc0.ValueByte3 = byte23[1];
        //        ctrlPlc0.InvalidateVisual();
        //    }
        //}
        #endregion
    }
    
    public struct testStruct
    {
        public bool varBool0;
        public bool varBool1;
        public bool varBool2;
        public bool varBool3;
        public bool varBool4;
        public bool varBool5;
        public bool varBool6;

        public byte varByte0;
        public byte varByte1;

        public ushort varWord0;

        public double varReal0;
        public bool varBool7;
        public double varReal1;

        public byte varByte2;
        public UInt32 varDWord;
    }

}
