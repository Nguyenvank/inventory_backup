using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace Cham_cong
{
    static class cls0
    {
        public static string factcd = "F1";
        public static string factnm = "본사";
        public static string shiftsno = "1";
        public static string shiftsnm = "Night";
        public static string workdate = "";
        public static string sNow = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        public static DateTime sTime1 = new DateTime();
        public static DateTime sTime2 = new DateTime();
        public static BindingSource bindingSource0 = new BindingSource();
        public static SqlDataAdapter dataAdapter0 = new SqlDataAdapter();
        public static BindingSource bindingSource1 = new BindingSource();
        public static SqlDataAdapter dataAdapter1 = new SqlDataAdapter();
        public static BindingSource bindingSource2 = new BindingSource();
        public static SqlDataAdapter dataAdapter2 = new SqlDataAdapter();
        public static BindingSource bindingSource3 = new BindingSource();
        public static SqlDataAdapter dataAdapter3 = new SqlDataAdapter();
        public static BindingSource bindingSource4 = new BindingSource();
        public static SqlDataAdapter dataAdapter4 = new SqlDataAdapter();
        public static KeyPressEventHandler NumericCheckHandler = new KeyPressEventHandler(NumericCheck);
        public static KeyPressEventHandler NumericCheckHandlerDecimal = new KeyPressEventHandler(NumericCheckDecimal);

        public static void getProductInfo()
        {
            DateTime now = DateTime.Now;
            sNow = now.ToString("yyyy-MM-dd HH:mm:ss");
            sTime1 = new DateTime(now.Year, now.Month, now.Day, 20, 0, 0);
            sTime2 = new DateTime(now.Year, now.Month, now.Day, 8, 0, 0);
            new DateTime(now.Year, now.Month, 1).AddMonths(1).AddDays(-1.0);
            DateTime date = now.Date;
            if (DateTime.Now.TimeOfDay < TimeSpan.Parse("08:00:00"))
            {
                sTime1 = sTime1.AddDays(-1.0);
                sTime2 = sTime2.AddDays(0.0);
                shiftsnm = "Night";
                shiftsno = "2";
            }
            else if (now.TimeOfDay >= TimeSpan.Parse("20:00:00"))
            {
                sTime1 = sTime1.AddDays(0.0);
                sTime2 = sTime2.AddDays(1.0);
                shiftsnm = "Night";
                shiftsno = "2";
            }
            else
            {
                shiftsnm = "Day";
                shiftsno = "1";
            }
        }

        public static string getProductInfo(string datetimeformat)
        {
            DateTime now = DateTime.Now;
            sNow = now.ToString("yyyy-MM-dd HH:mm:ss");
            sTime1 = new DateTime(now.Year, now.Month, now.Day, 20, 0, 0);
            sTime2 = new DateTime(now.Year, now.Month, now.Day, 8, 0, 0);
            new DateTime(now.Year, now.Month, 1).AddMonths(1).AddDays(-1.0);
            DateTime date = now.Date;
            if (DateTime.Now.TimeOfDay < TimeSpan.Parse("08:00:00"))
            {
                sTime1 = sTime1.AddDays(-1.0);
                sTime2 = sTime2.AddDays(0.0);
                shiftsnm = "Night";
                shiftsno = "2";
            }
            else if (now.TimeOfDay >= TimeSpan.Parse("20:00:00"))
            {
                sTime1 = sTime1.AddDays(0.0);
                sTime2 = sTime2.AddDays(1.0);
                shiftsnm = "Night";
                shiftsno = "2";
            }
            else
            {
                shiftsnm = "Day";
                shiftsno = "1";
            }
            return now.ToString(datetimeformat);
        }

        public static string getProductInfo(string datetimeformat, string shift)
        {
            DateTime now = DateTime.Now;
            sNow = now.ToString("yyyy-MM-dd HH:mm:ss");
            sTime1 = new DateTime(now.Year, now.Month, now.Day, 20, 0, 0);
            sTime2 = new DateTime(now.Year, now.Month, now.Day, 8, 0, 0);
            new DateTime(now.Year, now.Month, 1).AddMonths(1).AddDays(-1.0);
            DateTime date = now.Date;
            if (DateTime.Now.TimeOfDay < TimeSpan.Parse("08:00:00"))
            {
                sTime1 = sTime1.AddDays(-1.0);
                sTime2 = sTime2.AddDays(0.0);
                shiftsnm = "Night";
                shiftsno = "2";
            }
            else if (now.TimeOfDay >= TimeSpan.Parse("20:00:00"))
            {
                sTime1 = sTime1.AddDays(0.0);
                sTime2 = sTime2.AddDays(1.0);
                shiftsnm = "Night";
                shiftsno = "2";
            }
            else
            {
                shiftsnm = "Day";
                shiftsno = "1";
            }
            return !(shift != "") ? sTime1.ToString(datetimeformat) : sTime1.ToString(datetimeformat + "-" + shiftsno);
        }

        public static string getShiftInfo()
        {
            DateTime now = DateTime.Now;
            sNow = now.ToString("yyyy-MM-dd HH:mm:ss");
            sTime1 = new DateTime(now.Year, now.Month, now.Day, 20, 0, 0);
            sTime2 = new DateTime(now.Year, now.Month, now.Day, 8, 0, 0);
            new DateTime(now.Year, now.Month, 1).AddMonths(1).AddDays(-1.0);
            DateTime date = now.Date;
            if (DateTime.Now.TimeOfDay < TimeSpan.Parse("08:00:00"))
            {
                sTime1 = sTime1.AddDays(-1.0);
                sTime2 = sTime2.AddDays(0.0);
                shiftsnm = "Night";
                shiftsno = "2";
            }
            else if (now.TimeOfDay >= TimeSpan.Parse("20:00:00"))
            {
                sTime1 = sTime1.AddDays(0.0);
                sTime2 = sTime2.AddDays(1.0);
                shiftsnm = "Night";
                shiftsno = "2";
            }
            else
            {
                shiftsnm = "Day";
                shiftsno = "1";
            }
            return shiftsnm;
        }

        public static string getShiftName()
        {
            DateTime now = DateTime.Now;
            sNow = now.ToString("yyyy-MM-dd HH:mm:ss");
            sTime1 = new DateTime(now.Year, now.Month, now.Day, 20, 0, 0);
            sTime2 = new DateTime(now.Year, now.Month, now.Day, 8, 0, 0);
            new DateTime(now.Year, now.Month, 1).AddMonths(1).AddDays(-1.0);
            DateTime date = now.Date;
            if (DateTime.Now.TimeOfDay < TimeSpan.Parse("08:00:00"))
            {
                sTime1 = sTime1.AddDays(-1.0);
                sTime2 = sTime2.AddDays(0.0);
                shiftsnm = "Night";
                shiftsno = "2";
            }
            else if (now.TimeOfDay >= TimeSpan.Parse("20:00:00"))
            {
                sTime1 = sTime1.AddDays(0.0);
                sTime2 = sTime2.AddDays(1.0);
                shiftsnm = "Night";
                shiftsno = "2";
            }
            else
            {
                shiftsnm = "Day";
                shiftsno = "1";
            }
            return shiftsnm;
        }

        public static string getShiftNo()
        {
            DateTime now = DateTime.Now;
            sNow = now.ToString("yyyy-MM-dd HH:mm:ss");
            sTime1 = new DateTime(now.Year, now.Month, now.Day, 20, 0, 0);
            sTime2 = new DateTime(now.Year, now.Month, now.Day, 8, 0, 0);
            new DateTime(now.Year, now.Month, 1).AddMonths(1).AddDays(-1.0);
            DateTime date = now.Date;
            if (DateTime.Now.TimeOfDay < TimeSpan.Parse("08:00:00"))
            {
                sTime1 = sTime1.AddDays(-1.0);
                sTime2 = sTime2.AddDays(0.0);
                shiftsnm = "Night";
                shiftsno = "2";
            }
            else if (now.TimeOfDay >= TimeSpan.Parse("20:00:00"))
            {
                sTime1 = sTime1.AddDays(0.0);
                sTime2 = sTime2.AddDays(1.0);
                shiftsnm = "Night";
                shiftsno = "2";
            }
            else
            {
                shiftsnm = "Day";
                shiftsno = "1";
            }
            return shiftsno;
        }

        public static int getCount(string sql)
        {
            int num = 0;
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
            sqlConnection.Open();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = sql;
            selectCommand.Connection = sqlConnection;
            try
            {
                selectCommand.ExecuteNonQuery();
                DataSet dataSet = new DataSet();
                new SqlDataAdapter(selectCommand).Fill(dataSet, "PackingID");
                num = dataSet.Tables["PackingID"].Rows.Count;
            }
            catch
            {
                throw;
            }
            finally
            {
                sqlConnection.Close();
            }
            return num;
        }

        public static int getCount(string sql, string connect)
        {
            int num = 0;
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[connect].ConnectionString);
            sqlConnection.Open();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = sql;
            selectCommand.Connection = sqlConnection;
            try
            {
                selectCommand.ExecuteNonQuery();
                DataSet dataSet = new DataSet();
                new SqlDataAdapter(selectCommand).Fill(dataSet, "PackingID");
                num = dataSet.Tables["PackingID"].Rows.Count;
            }
            catch
            {
                throw;
            }
            finally
            {
                sqlConnection.Close();
            }
            return num;
        }

        public static string getValue(string sql)
        {
            string str = "";
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
            sqlConnection.Open();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = sql;
            selectCommand.Connection = sqlConnection;
            try
            {
                selectCommand.ExecuteNonQuery();
                DataSet dataSet = new DataSet();
                new SqlDataAdapter(selectCommand).Fill(dataSet, "Sum");
                str = dataSet.Tables["Sum"].Rows[0][0].ToString();
            }
            catch
            {
            }
            finally
            {
                sqlConnection.Close();
            }
            return str;
        }

        public static DataTable getTable(string sql)
        {
            DataTable dataTable = new DataTable();
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
            sqlConnection.Open();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = sql;
            selectCommand.Connection = sqlConnection;
            try
            {
                selectCommand.ExecuteNonQuery();
                DataSet dataSet = new DataSet();
                new SqlDataAdapter(selectCommand).Fill(dataTable);
            }
            catch
            {
            }
            finally
            {
                sqlConnection.Close();
            }
            return dataTable;
        }

        public static string getValue(string sql, string connect)
        {
            string str = "";
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[connect].ConnectionString);
            sqlConnection.Open();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = sql;
            selectCommand.Connection = sqlConnection;
            try
            {
                selectCommand.ExecuteNonQuery();
                DataSet dataSet = new DataSet();
                new SqlDataAdapter(selectCommand).Fill(dataSet, "Sum");
                str = dataSet.Tables["Sum"].Rows[0][0].ToString();
            }
            catch
            {
            }
            finally
            {
                sqlConnection.Close();
            }
            return str;
        }

        public static int getWidth(DataGridView dgv)
        {
            return dgv.Width;
        }

        public static int getHeght(DataGridView dgv)
        {
            return dgv.Height;
        }

        public static void GetData(string selectCommand, DataGridView dgvName, BindingSource bindingsource, SqlDataAdapter sqldataadapter)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
                sqldataadapter = new SqlDataAdapter(selectCommand, connectionString);
                SqlCommandBuilder sqlCommandBuilder = new SqlCommandBuilder(sqldataadapter);
                DataTable dataTable = new DataTable();
                dataTable.Locale = CultureInfo.InvariantCulture;
                sqldataadapter.Fill(dataTable);
                bindingsource.DataSource = (object)dataTable;
                dgvName.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message.ToString(), "System Notice");
            }
        }

        public static void GetData(string strConnect, string selectCommand, DataGridView dgvName, BindingSource bindingsource, SqlDataAdapter sqldataadapter)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings[strConnect].ConnectionString;
                sqldataadapter = new SqlDataAdapter(selectCommand, connectionString);
                SqlCommandBuilder sqlCommandBuilder = new SqlCommandBuilder(sqldataadapter);
                DataTable dataTable = new DataTable();
                dataTable.Locale = CultureInfo.InvariantCulture;
                sqldataadapter.Fill(dataTable);
                bindingsource.DataSource = (object)dataTable;
                dgvName.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message.ToString(), "System Notice");
            }
        }

        public static void BindDataGrid(string CommandText, DataGridView GridView, BindingSource BindingSource)
        {
            DataTable dataTable = new DataTable();
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
            sqlConnection.Open();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandType = CommandType.StoredProcedure;
            selectCommand.CommandText = CommandText;
            selectCommand.Connection = sqlConnection;
            new SqlDataAdapter(selectCommand).Fill(dataTable);
            BindingSource.DataSource = (object)dataTable;
            GridView.DataSource = (object)BindingSource;
        }

        public static void BindDataGrid(string strConnect, string CommandText, DataGridView GridView, BindingSource BindingSource)
        {
            DataTable dataTable = new DataTable();
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[strConnect].ConnectionString);
            sqlConnection.Open();
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandType = CommandType.StoredProcedure;
            selectCommand.CommandText = CommandText;
            selectCommand.Connection = sqlConnection;
            new SqlDataAdapter(selectCommand).Fill(dataTable);
            BindingSource.DataSource = (object)dataTable;
            GridView.DataSource = (object)BindingSource;
        }

        public static void fnClearSelectColor(DataGridView dgv)
        {
            dgv.ClearSelection();
            dgv.CurrentCell = (DataGridViewCell)null;
        }

        public static void fnResetTimer(System.Windows.Forms.Timer timer)
        {
            timer.Stop();
            timer.Start();
        }

        public static void fnSetDatagridRowColor(DataGridView dgv)
        {
            foreach (DataGridViewRow row in (IEnumerable)dgv.Rows)
            {
                if ((uint)(row.Index % 2) > 0U)
                    row.DefaultCellStyle.BackColor = Color.LightCyan;
                else
                    row.DefaultCellStyle.BackColor = Color.White;
            }
        }

        public static void NumericCheck(object sender, KeyPressEventArgs e)
        {
            DataGridViewTextBoxEditingControl boxEditingControl = sender as DataGridViewTextBoxEditingControl;
            if (boxEditingControl != null && e.KeyChar == ',')
            {
                e.KeyChar = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
                e.Handled = boxEditingControl.Text.Contains<char>(e.KeyChar);
            }
            else
                e.Handled = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar);
        }

        public static void NumericCheckDecimal(object sender, KeyPressEventArgs e)
        {
            DataGridViewTextBoxEditingControl boxEditingControl = sender as DataGridViewTextBoxEditingControl;
            if (boxEditingControl != null && (e.KeyChar == '.' || e.KeyChar == ','))
            {
                e.KeyChar = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
                e.Handled = boxEditingControl.Text.Contains<char>(e.KeyChar);
            }
            else
                e.Handled = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar);
        }

        public static string EncryptString(string Message, string Passphrase)
        {
            UTF8Encoding utF8Encoding = new UTF8Encoding();
            MD5CryptoServiceProvider cryptoServiceProvider1 = new MD5CryptoServiceProvider();
            byte[] hash = cryptoServiceProvider1.ComputeHash(utF8Encoding.GetBytes(Passphrase));
            TripleDESCryptoServiceProvider cryptoServiceProvider2 = new TripleDESCryptoServiceProvider();
            cryptoServiceProvider2.Key = hash;
            cryptoServiceProvider2.Mode = CipherMode.ECB;
            cryptoServiceProvider2.Padding = PaddingMode.PKCS7;
            byte[] bytes = utF8Encoding.GetBytes(Message);
            byte[] inArray;
            try
            {
                inArray = cryptoServiceProvider2.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length);
            }
            finally
            {
                cryptoServiceProvider2.Clear();
                cryptoServiceProvider1.Clear();
            }
            return Convert.ToBase64String(inArray);
        }

        public static string DecryptString(string Message, string Passphrase)
        {
            UTF8Encoding utF8Encoding = new UTF8Encoding();
            MD5CryptoServiceProvider cryptoServiceProvider1 = new MD5CryptoServiceProvider();
            byte[] hash = cryptoServiceProvider1.ComputeHash(utF8Encoding.GetBytes(Passphrase));
            TripleDESCryptoServiceProvider cryptoServiceProvider2 = new TripleDESCryptoServiceProvider();
            cryptoServiceProvider2.Key = hash;
            cryptoServiceProvider2.Mode = CipherMode.ECB;
            cryptoServiceProvider2.Padding = PaddingMode.PKCS7;
            byte[] inputBuffer = Convert.FromBase64String(Message);
            byte[] bytes;
            try
            {
                bytes = cryptoServiceProvider2.CreateDecryptor().TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
            }
            finally
            {
                cryptoServiceProvider2.Clear();
                cryptoServiceProvider1.Clear();
            }
            return utF8Encoding.GetString(bytes);
        }

        public static string RightString(this string str, int length)
        {
            return str.Substring(str.Length - length, length);
        }

        public static void FreezeBand(DataGridViewBand band)
        {
            band.Frozen = true;
            band.DefaultCellStyle = new DataGridViewCellStyle()
            {
                BackColor = Color.WhiteSmoke
            };
        }

        public static int fnGetScreenWidth()
        {
            return SystemInformation.VirtualScreen.Width;
        }

        public static int fnGetScreenHeight()
        {
            return SystemInformation.VirtualScreen.Height;
        }

        public static Control GetControlByName(Control ParentCntl, string NameToSearch)
        {
            if (ParentCntl.Name == NameToSearch)
                return ParentCntl;
            foreach (Control control in (ArrangedElementCollection)ParentCntl.Controls)
            {
                Control controlByName = GetControlByName(control, NameToSearch);
                if (controlByName != null)
                    return controlByName;
            }
            return (Control)null;
        }

        public static void status(Label label, string message, int interval)
        {
            label.Text = message;
            if (message.ToUpper() == "OK")
                label.ForeColor = Color.Green;
            else
                label.ForeColor = Color.Red;
            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
            t.Interval = interval;
            t.Tick += (EventHandler)((s, e) =>
            {
                label.Hide();
                t.Stop();
            });
            t.Start();
        }

        public static string ShowDialog(string text, string caption)
        {
            Form form = new Form();
            form.Width = 500;
            form.Height = 200;
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.Text = caption;
            form.StartPosition = FormStartPosition.CenterScreen;
            Form prompt = form;
            Label label1 = new Label();
            label1.Left = 50;
            label1.Top = 10;
            label1.Text = text;
            label1.Width = 450;
            Label label2 = label1;
            TextBox textBox1 = new TextBox();
            textBox1.Left = 50;
            textBox1.Top = 40;
            textBox1.Width = 400;
            textBox1.Height = 80;
            textBox1.Multiline = true;
            TextBox textBox2 = textBox1;
            Button button1 = new Button();
            button1.Text = "XÁC NHẬN";
            button1.Left = 350;
            button1.Width = 100;
            button1.Top = 130;
            button1.DialogResult = DialogResult.OK;
            Button button2 = button1;
            button2.Click += (EventHandler)((sender, e) => prompt.Close());
            prompt.Controls.Add((Control)textBox2);
            prompt.Controls.Add((Control)button2);
            prompt.Controls.Add((Control)label2);
            prompt.AcceptButton = (IButtonControl)button2;
            return prompt.ShowDialog() == DialogResult.OK ? textBox2.Text : "";
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    using (webClient.OpenRead("http://clients3.google.com/generate_204"))
                        return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public class AutoClosingMessageBox
        {
            private System.Threading.Timer _timeoutTimer;
            private string _caption;
            private const int WM_CLOSE = 16;

            private AutoClosingMessageBox(string text, string caption, int timeout)
            {
                this._caption = caption;
                this._timeoutTimer = new System.Threading.Timer(new TimerCallback(this.OnTimerElapsed), (object)null, timeout, -1);
                using (this._timeoutTimer)
                {
                    int num = (int)MessageBox.Show(text, caption);
                }
            }

            public static void Show(string text, string caption, int timeout)
            {
                AutoClosingMessageBox closingMessageBox = new AutoClosingMessageBox(text, caption, timeout);
            }

            private void OnTimerElapsed(object state)
            {
                IntPtr window = AutoClosingMessageBox.FindWindow("#32770", this._caption);
                if (window != IntPtr.Zero)
                    AutoClosingMessageBox.SendMessage(window, 16U, IntPtr.Zero, IntPtr.Zero);
                this._timeoutTimer.Dispose();
            }

            [DllImport("user32.dll", SetLastError = true)]
            private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        }
    }
}
