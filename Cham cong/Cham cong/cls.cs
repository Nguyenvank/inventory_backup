using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;
using System.Collections;
using System.Drawing.Printing;
using System.Data.OleDb;
//using DataMatrix.net;               // Add ref to DataMatrix.net.dll

namespace Inventory_Data
{
    public static class DBConnect
    {
        public static SqlConnection myCon = null;

        public static void CreateConnection()
        {
            myCon = new SqlConnection(cls.getConnectionString());
            myCon.Open();

        }
    }

    public static class cls
    {
        public static BindingSource bindingSource0 = new BindingSource();
        public static BindingSource bindingSource1 = new BindingSource();
        public static BindingSource bindingSource2 = new BindingSource();
        public static BindingSource bindingSource3 = new BindingSource();
        public static BindingSource bindingSource4 = new BindingSource();
        public static SqlDataAdapter dataAdapter0 = new SqlDataAdapter();
        public static SqlDataAdapter dataAdapter1 = new SqlDataAdapter();
        public static SqlDataAdapter dataAdapter2 = new SqlDataAdapter();
        public static SqlDataAdapter dataAdapter3 = new SqlDataAdapter();
        public static SqlDataAdapter dataAdapter4 = new SqlDataAdapter();

        public static string factcd = "F1";
        public static string factnm = "본사";
        public static string shiftsno = "1";
        public static string shiftsnm = "Night";
        public static string workdate = "";
        public static string sNow = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        public static DateTime sTime1 = new DateTime();
        public static DateTime sTime2 = new DateTime();

        public static string fnGetDate(string format)
        {
            string s = "";

            DateTime nNow = DateTime.Now;
            //sNow = nNow.ToString("yyyy-MM-dd HH:mm:ss");

            //button2.Text = sNow;//DateTime.Now.TimeOfDay.ToString();

            if (DateTime.Now.TimeOfDay < TimeSpan.Parse("08:00:00"))
            {
                sTime1 = new DateTime(nNow.Year, nNow.Month, nNow.Day, 20, 0, 0).AddDays(-1);
                sTime2 = new DateTime(nNow.Year, nNow.Month, nNow.Day, 8, 0, 0);
                shiftsnm = "Night";
                shiftsno = "2";
            }
            else if (nNow.TimeOfDay >= TimeSpan.Parse("20:00:00"))
            {

                sTime1 = new DateTime(nNow.Year, nNow.Month, nNow.Day, 20, 0, 0);
                sTime2 = new DateTime(nNow.Year, nNow.Month, nNow.Day, 8, 0, 0).AddDays(1);
                shiftsnm = "Night";
                shiftsno = "2";
            }
            else
            {
                sTime1 = new DateTime(nNow.Year, nNow.Month, nNow.Day, 8, 0, 0);
                sTime2 = new DateTime(nNow.Year, nNow.Month, nNow.Day, 20, 0, 0);
                shiftsnm = "Day";
                shiftsno = "1";
            }
            // sTime1 = sTime1.AddDays(-2);
            //workdate = sTime1.ToString("yyyyMMdd");
            //button1.Text = sTime1.ToString("yyyy/MM/dd") + " " + shiftsnm;
            switch (format)
            {
                case "d":   //Date: 09/10/2017
                    s = nNow.ToString("dd/MM/yyyy");
                    break;
                case "dt":  //Date time: 09/10/2017 19:36:22
                    s = nNow.ToString("dd/MM/yyyy HH:mm:ss");
                    break;
                case "t":   //Time: 19:36:22
                    s = nNow.ToString("HH:mm:ss");
                    break;
                case "sd":  //Shift date: Day(Night) 09/10/2017
                    s = (shiftsno == "1") ? (shiftsnm + " " + nNow.ToString("dd/MM/yyyy")) : (shiftsnm + " " + nNow.AddDays(-1).ToString("dd/MM/yyyy"));
                    break;
                case "SD":  // Shift date: DAY(NIGHT) 09/10/2017
                    s = (shiftsno == "1") ? (shiftsnm.ToUpper() + " " + nNow.ToString("dd/MM/yyyy")) : (shiftsnm.ToUpper() + " " + nNow.AddDays(-1).ToString("dd/MM/yyyy"));
                    break;
                case "ct":  //Country time: Vina 19:36:22
                    s = "Vina " + nNow.ToString("HH:mm:ss");
                    break;
                case "CT":  // Country time: VINA 19:36:22
                    s = "VINA " + nNow.ToString("HH:mm:ss");
                    break;
                case "s":   // Shift: Day/Night
                    s = shiftsnm;
                    break;
                case "S":   // Shift (capital): DAY/NIGHT
                    s = shiftsnm.ToUpper();
                    break;
                case "sn":  // Shift number: 1-Day; 2-Night
                    s = shiftsno;
                    break;
                case "lot": // LOT date: 20171009
                    s = (shiftsno == "1") ? nNow.ToString("yyyyMMdd") : nNow.AddDays(-1).ToString("yyyyMMdd");
                    break;
                case "ls":  // LOT number: 20171009-1 (Day); 20171009-2 (Night)
                    s = (shiftsno == "1") ? nNow.ToString("yyyyMMdd") + "-1" : nNow.AddDays(-1).ToString("yyyyMMdd") + "-2";
                    break;
            }

            return s;
        }

        public static DataSet ExecuteDataSet(string sql)
        {
            using (DataSet ds = new DataSet())
            using (SqlConnection connStr = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, connStr))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    cmd.Connection.Open();
                    new SqlDataAdapter(cmd).Fill(ds);
                }
                //catch (SqlException ex)
                catch
                {
                    //log to a file or Throw a message ex.Message;
                }
                return ds;
            }
        }

        public static DataSet ExecuteDataSet(string sql, CommandType cmdType)
        {
            using (DataSet ds = new DataSet())
            using (SqlConnection connStr = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, connStr))
            {
                cmd.CommandType = cmdType;

                try
                {
                    cmd.Connection.Open();
                    new SqlDataAdapter(cmd).Fill(ds);
                }
                //catch (SqlException ex)
                catch
                {
                    //log to a file or Throw a message ex.Message;
                }
                return ds;
            }
        }

        public static DataSet ExecuteDataSet(string sql, CommandType cmdType, string conName)
        {
            using (DataSet ds = new DataSet())
            using (SqlConnection connStr = new SqlConnection(ConfigurationManager.ConnectionStrings[conName].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, connStr))
            {
                cmd.CommandType = cmdType;

                try
                {
                    cmd.Connection.Open();
                    new SqlDataAdapter(cmd).Fill(ds);
                }
                //catch (SqlException ex)
                catch
                {
                    //log to a file or Throw a message ex.Message;
                }
                return ds;
            }
        }

        public static DataSet ExecuteDataSet(string sql, CommandType cmdType, string conName, params SqlParameter[] parameters)
        {
            using (DataSet ds = new DataSet())
            using (SqlConnection connStr = new SqlConnection(ConfigurationManager.ConnectionStrings[conName].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, connStr))
            {
                cmd.CommandType = cmdType;
                foreach (var item in parameters)
                {
                    cmd.Parameters.Add(item);
                }

                try
                {
                    cmd.Connection.Open();
                    new SqlDataAdapter(cmd).Fill(ds);
                }
                //catch (SqlException ex)
                catch
                {
                    //log to a file or Throw a message ex.Message;
                }
                return ds;
            }
            //SqlParameter[] sParams = new SqlParameter[2]; // Parameter count

            //sParams[0] = new SqlParameter();
            //sParams[0].SqlDbType = SqlDbType.Int;
            //sParams[0].ParameterName = "@IMPORTID";
            //sParams[0].Value = SelectedListID;

            //sParams[1] = new SqlParameter();
            //sParams[1].SqlDbType = SqlDbType.VarChar;
            //sParams[1].ParameterName = "@PREFIX";
            //sParams[1].Value = selectedPrefix;
        }

        public static DataSet ExecuteDataSet(string sql, CommandType cmdType, params SqlParameter[] parameters)
        {
            using (DataSet ds = new DataSet())
            using (SqlConnection connStr = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, connStr))
            {
                cmd.CommandType = cmdType;
                foreach (var item in parameters)
                {
                    cmd.Parameters.Add(item);
                }

                try
                {
                    cmd.Connection.Open();
                    new SqlDataAdapter(cmd).Fill(ds);
                }
                //catch (SqlException ex)
                catch
                {
                    //log to a file or Throw a message ex.Message;
                }
                return ds;
            }
            //SqlParameter[] sParams = new SqlParameter[2]; // Parameter count

            //sParams[0] = new SqlParameter();
            //sParams[0].SqlDbType = SqlDbType.Int;
            //sParams[0].ParameterName = "@IMPORTID";
            //sParams[0].Value = SelectedListID;

            //sParams[1] = new SqlParameter();
            //sParams[1].SqlDbType = SqlDbType.VarChar;
            //sParams[1].ParameterName = "@PREFIX";
            //sParams[1].Value = selectedPrefix;
        }

        public static DataSet ExecuteDataSet(string sql, params SqlParameter[] parameters)
        {
            using (DataSet ds = new DataSet())
            using (SqlConnection connStr = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, connStr))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (var item in parameters)
                {
                    cmd.Parameters.Add(item);
                }

                try
                {
                    cmd.Connection.Open();
                    new SqlDataAdapter(cmd).Fill(ds);
                }
                //catch (SqlException ex)
                catch
                {
                    //log to a file or Throw a message ex.Message;
                }
                return ds;
            }
            //SqlParameter[] sParams = new SqlParameter[2]; // Parameter count

            //sParams[0] = new SqlParameter();
            //sParams[0].SqlDbType = SqlDbType.Int;
            //sParams[0].ParameterName = "@IMPORTID";
            //sParams[0].Value = SelectedListID;

            //sParams[1] = new SqlParameter();
            //sParams[1].SqlDbType = SqlDbType.VarChar;
            //sParams[1].ParameterName = "@PREFIX";
            //sParams[1].Value = selectedPrefix;
        }

        public static DataSet ExecuteDataSet(string sql, string table, params SqlParameter[] parameters)
        {
            using (DataSet ds = new DataSet())
            using (SqlConnection connStr = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, connStr))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (var item in parameters)
                {
                    cmd.Parameters.Add(item);
                }

                try
                {
                    cmd.Connection.Open();
                    new SqlDataAdapter(cmd).Fill(ds,table);
                }
                //catch (SqlException ex)
                catch
                {
                    //log to a file or Throw a message ex.Message;
                }
                return ds;
            }
            //SqlParameter[] sParams = new SqlParameter[2]; // Parameter count

            //sParams[0] = new SqlParameter();
            //sParams[0].SqlDbType = SqlDbType.Int;
            //sParams[0].ParameterName = "@IMPORTID";
            //sParams[0].Value = SelectedListID;

            //sParams[1] = new SqlParameter();
            //sParams[1].SqlDbType = SqlDbType.VarChar;
            //sParams[1].ParameterName = "@PREFIX";
            //sParams[1].Value = selectedPrefix;
        }

        public static DataTable ExecuteDataTable(string sql)
        {
            using (DataSet ds = new DataSet())
            using (SqlConnection connStr = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, connStr))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    cmd.Connection.Open();
                    new SqlDataAdapter(cmd).Fill(ds);
                }
                //catch (SqlException ex)
                catch
                {
                    //Show a message or log a message on ex.Message
                }
                return ds.Tables[0];
            }
        }

        public static DataTable ExecuteDataTable(string sql, CommandType cmdType)
        {
            using (DataSet ds = new DataSet())
            using (SqlConnection connStr = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, connStr))
            {
                cmd.CommandType = cmdType;

                try
                {
                    cmd.Connection.Open();
                    new SqlDataAdapter(cmd).Fill(ds);
                }
                //catch (SqlException ex)
                catch
                {
                    //Show a message or log a message on ex.Message
                }
                return ds.Tables[0];
            }
        }

        public static DataTable ExecuteDataTable(string sql, CommandType cmdType, string conName)
        {
            using (DataSet ds = new DataSet())
            using (SqlConnection connStr = new SqlConnection(ConfigurationManager.ConnectionStrings[conName].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, connStr))
            {
                cmd.CommandType = cmdType;

                try
                {
                    cmd.Connection.Open();
                    new SqlDataAdapter(cmd).Fill(ds);
                }
                //catch (SqlException ex)
                catch
                {
                    //Show a message or log a message on ex.Message
                }
                return ds.Tables[0];
            }
        }

        public static DataTable ExecuteDataTable(string sql, CommandType cmdType, string conName, params SqlParameter[] parameters)
        {
            using (DataSet ds = new DataSet())
            using (SqlConnection connStr = new SqlConnection(ConfigurationManager.ConnectionStrings[conName].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, connStr))
            {
                cmd.CommandType = cmdType;
                foreach (var item in parameters)
                {
                    cmd.Parameters.Add(item);
                }

                try
                {
                    cmd.Connection.Open();
                    new SqlDataAdapter(cmd).Fill(ds);
                }
                //catch (SqlException ex)
                catch
                {
                    //Show a message or log a message on ex.Message
                }
                return ds.Tables[0];
            }
            //SqlParameter[] sParams = new SqlParameter[2]; // Parameter count

            //sParams[0] = new SqlParameter();
            //sParams[0].SqlDbType = SqlDbType.Int;
            //sParams[0].ParameterName = "@IMPORTID";
            //sParams[0].Value = SelectedListID;

            //sParams[1] = new SqlParameter();
            //sParams[1].SqlDbType = SqlDbType.VarChar;
            //sParams[1].ParameterName = "@PREFIX";
            //sParams[1].Value = selectedPrefix;
        }

        public static DataTable ExecuteDataTable(string sql, CommandType cmdType, params SqlParameter[] parameters)
        {
            using (DataSet ds = new DataSet())
            using (SqlConnection connStr = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, connStr))
            {
                cmd.CommandType = cmdType;
                foreach (var item in parameters)
                {
                    cmd.Parameters.Add(item);
                }

                try
                {
                    cmd.Connection.Open();
                    new SqlDataAdapter(cmd).Fill(ds);
                }
                //catch (SqlException ex)
                catch
                {
                    //Show a message or log a message on ex.Message
                }
                return ds.Tables[0];
            }
            //SqlParameter[] sParams = new SqlParameter[2]; // Parameter count

            //sParams[0] = new SqlParameter();
            //sParams[0].SqlDbType = SqlDbType.Int;
            //sParams[0].ParameterName = "@IMPORTID";
            //sParams[0].Value = SelectedListID;

            //sParams[1] = new SqlParameter();
            //sParams[1].SqlDbType = SqlDbType.VarChar;
            //sParams[1].ParameterName = "@PREFIX";
            //sParams[1].Value = selectedPrefix;
        }

        public static DataTable ExecuteDataTable(string sql, params SqlParameter[] parameters)
        {
            using (DataSet ds = new DataSet())
            using (SqlConnection connStr = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, connStr))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (var item in parameters)
                {
                    cmd.Parameters.Add(item);
                }

                try
                {
                    cmd.Connection.Open();
                    new SqlDataAdapter(cmd).Fill(ds);
                }
                //catch (SqlException ex)
                catch
                {
                    //Show a message or log a message on ex.Message
                }
                return ds.Tables[0];
            }
            //SqlParameter[] sParams = new SqlParameter[2]; // Parameter count

            //sParams[0] = new SqlParameter();
            //sParams[0].SqlDbType = SqlDbType.Int;
            //sParams[0].ParameterName = "@IMPORTID";
            //sParams[0].Value = SelectedListID;

            //sParams[1] = new SqlParameter();
            //sParams[1].SqlDbType = SqlDbType.VarChar;
            //sParams[1].ParameterName = "@PREFIX";
            //sParams[1].Value = selectedPrefix;
        }

        public static string fnGetRow(string sql, params SqlParameter[] parameters)
        {
            string rowValue = "";
            DataSet ds = new DataSet();
            ds = ExecuteDataSet(sql, parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                rowValue = ds.Tables[0].Rows[0][0].ToString();
            }
            return rowValue;
        }

        public static void RemoveSelection(Object obj)
        {
            TextBox textbox = obj as TextBox;
            if (textbox != null)
            {
                textbox.SelectionLength = 0;
            }
        }

        public static int fnGetCount(string sql)
        {
            int count = 0;
            DataSet ds = new DataSet();
            ds = ExecuteDataSet(sql, CommandType.StoredProcedure);
            count = ds.Tables[0].Rows.Count;
            return count;
        }
            
        public static string getConnectionString()
        {
            string strConn = "";
            strConn = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
            //strConn = ConfigurationSettings.AppSettings["conn"];
            return strConn;
        }

        public static string GetConnectionStringByName(string name)
        {
            // Assume failure.
            string returnValue = null;

            // Look for the name in the connectionStrings section.
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[name];

            // If found, return the connection string.
            if (settings != null)
                returnValue = settings.ConnectionString;

            return returnValue;
        }

        public static DataTable fnSelect(string procedure)
        {
            using (DataTable dt = new DataTable())
            {
                string connString = getConnectionString();
                string sql = procedure;

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    try
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter())
                        {
                            da.SelectCommand = new SqlCommand(sql, conn);
                            da.SelectCommand.CommandType = CommandType.StoredProcedure;

                            //DataSet ds = new DataSet();
                            //da.Fill(ds, "result_name");

                            //dt = ds.Tables["result_name"];
                            da.Fill(dt);
                            //foreach (DataRow row in dt.Rows)
                            //{
                            //    //manipulate your data
                            //}
                            //dtData = dt;
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("SQL Error: " + ex.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error: " + e.Message);
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
                return dt;
            }
        }

        public static DataTable fnSelect(string procedure,string connName)
        {
            using (DataTable dt = new DataTable())
            {
                string connString = GetConnectionStringByName(connName);
                string sql = procedure;

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    try
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter())
                        {
                            da.SelectCommand = new SqlCommand(sql, conn);
                            da.SelectCommand.CommandType = CommandType.StoredProcedure;

                            //DataSet ds = new DataSet();
                            //da.Fill(ds, "result_name");

                            //dt = ds.Tables["result_name"];
                            da.Fill(dt);
                            //foreach (DataRow row in dt.Rows)
                            //{
                            //    //manipulate your data
                            //}
                            //dtData = dt;
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("SQL Error: " + ex.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error: " + e.Message);
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
                return dt;
            }
        }

        public static void fnUpdDel(string procedure,string connName, params SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection _con = new SqlConnection(GetConnectionStringByName(connName)))
                using (SqlCommand _cmdDelete = new SqlCommand())
                {
                    _cmdDelete.CommandType = CommandType.StoredProcedure;
                    _cmdDelete.CommandText = procedure;
                    _cmdDelete.Connection = _con;

                    // add parameter
                    foreach (var item in parameters)
                    {
                        _cmdDelete.Parameters.Add(item);
                    }

                    // open connection, execute command, close connection
                    _con.Open();
                    _cmdDelete.ExecuteNonQuery();
                    _con.Close();
                }
            }
            catch
            {

            }
            finally
            {

            }
        }

        public static void fnUpdDel(string procedure, params SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection _con = new SqlConnection(GetConnectionStringByName("conn")))
                using (SqlCommand _cmdDelete = new SqlCommand())
                {
                    _cmdDelete.CommandType = CommandType.StoredProcedure;
                    _cmdDelete.CommandText = procedure;
                    _cmdDelete.Connection = _con;

                    // add parameter
                    foreach (var item in parameters)
                    {
                        _cmdDelete.Parameters.Add(item);
                    }

                    // open connection, execute command, close connection
                    _con.Open();
                    _cmdDelete.ExecuteNonQuery();
                    _con.Close();
                }
            }
            catch
            {

            }
            finally
            {

            }
        }

        public static bool IsFormOpen(Type t)
        {
            if (!t.IsSubclassOf(typeof(Form)) && !(t == typeof(Form)))
                throw new ArgumentException("Type is not a form", "t");
            try
            {
                for (int i1 = 0; i1 < Application.OpenForms.Count; i1++)
                {
                    Form f = Application.OpenForms[i1];
                    if (t.IsInstanceOfType(f))
                        return true;
                }
            }
            catch (IndexOutOfRangeException)
            {
                //This can change if they close/open a form while code is running. Just throw it away
            }
            return false;
        }

        public static void GetDataProcedure(string selectProcedure,DataGridView dgv,string strCon)
        {

        }

        public static void fnDatagridClickCell(DataGridView dgv, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = new DataGridViewRow();
            row = dgv.Rows[e.RowIndex];
            dgv.DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue;
            row.Selected = true;
        }

        public static void fnFormatDatagridview(DataGridView dgv, byte fontsize)
        {
            // Initialize basic DataGridView properties.
            dgv.Dock = DockStyle.Fill;
            dgv.BackgroundColor = Color.LightGray;
            dgv.BorderStyle = BorderStyle.Fixed3D;

            //// Set property values appropriate for read-only display and 
            //// limited interactivity. 
            //dgv.AllowUserToAddRows = false;
            //dgv.AllowUserToDeleteRows = false;
            //dgv.AllowUserToOrderColumns = true;
            //dgv.ReadOnly = true;
            //dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //dgv.MultiSelect = false;
            //dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            //dgv.AllowUserToResizeColumns = false;
            //dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            //dgv.AllowUserToResizeRows = false;
            //dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            // Hide row header
            dgv.RowHeadersVisible = false;

            // Hide horizontal scrollbar
            dgv.ScrollBars = ScrollBars.Vertical;

            // Align content to center of cell/column
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Set format to column headers
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", fontsize, FontStyle.Bold);

            // Set the selection background color for all the cells.
            dgv.DefaultCellStyle.SelectionBackColor = Color.White;
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Set the background color for all rows and for alternating rows. 
            // The value for alternating rows overrides the value for all rows. 
            ////dgv.RowsDefaultCellStyle.BackColor = Color.LightGray;
            dgv.RowsDefaultCellStyle.BackColor = Color.White;
            ////dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.DarkGray;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan;

            // Set the row and column header styles.
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            dgv.RowHeadersDefaultCellStyle.BackColor = Color.Black;


            // Clear selection
            dgv.ClearSelection();

            // Set font and fontsize
            dgv.DefaultCellStyle.Font = new Font("Times New Roman", fontsize);

            //using (Font font = new Font(dgv.DefaultCellStyle.Font.FontFamily, fontsize, FontStyle.Regular))
            //{
            //    //dgvCodeDefine.Columns["Rating"].DefaultCellStyle.Font = font;
            //    dgv.DefaultCellStyle.Font = font;
            //}

        }

        public static void fnFormatDatagridview(DataGridView dgv, byte fontsize, int headerHeight)
        {
            // Initialize basic DataGridView properties.
            dgv.Dock = DockStyle.Fill;
            dgv.BackgroundColor = Color.LightGray;
            dgv.BorderStyle = BorderStyle.Fixed3D;

            //// Set property values appropriate for read-only display and 
            //// limited interactivity. 
            //dgv.AllowUserToAddRows = false;
            //dgv.AllowUserToDeleteRows = false;
            //dgv.AllowUserToOrderColumns = true;
            //dgv.ReadOnly = true;
            //dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //dgv.MultiSelect = false;
            //dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            //dgv.AllowUserToResizeColumns = false;
            //dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            //dgv.AllowUserToResizeRows = false;
            //dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            // Hide row header
            dgv.RowHeadersVisible = false;

            // Hide horizontal scrollbar
            dgv.ScrollBars = ScrollBars.Vertical;

            // Align content to center of cell/column
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Set format to column headers
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", fontsize, FontStyle.Bold);

            // Set the selection background color for all the cells.
            dgv.DefaultCellStyle.SelectionBackColor = Color.White;
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Set the background color for all rows and for alternating rows. 
            // The value for alternating rows overrides the value for all rows. 
            ////dgv.RowsDefaultCellStyle.BackColor = Color.LightGray;
            dgv.RowsDefaultCellStyle.BackColor = Color.White;
            ////dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.DarkGray;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan;

            // Set the row and column header styles.
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            dgv.RowHeadersDefaultCellStyle.BackColor = Color.Black;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.ColumnHeadersHeight = headerHeight;


            // Clear selection
            dgv.ClearSelection();

            // Set font and fontsize
            dgv.DefaultCellStyle.Font = new Font("Times New Roman", fontsize);

            //using (Font font = new Font(dgv.DefaultCellStyle.Font.FontFamily, fontsize, FontStyle.Regular))
            //{
            //    //dgvCodeDefine.Columns["Rating"].DefaultCellStyle.Font = font;
            //    dgv.DefaultCellStyle.Font = font;
            //}

        }

        public static void fnFormatDatagridviewWhite(DataGridView dgv, byte fontsize, int headerHeight)
        {
            // Initialize basic DataGridView properties.
            dgv.Dock = DockStyle.Fill;
            dgv.BackgroundColor = Color.LightGray;
            dgv.BorderStyle = BorderStyle.Fixed3D;

            //// Set property values appropriate for read-only display and 
            //// limited interactivity. 
            //dgv.AllowUserToAddRows = false;
            //dgv.AllowUserToDeleteRows = false;
            //dgv.AllowUserToOrderColumns = true;
            //dgv.ReadOnly = true;
            //dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //dgv.MultiSelect = false;
            //dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            //dgv.AllowUserToResizeColumns = false;
            //dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            //dgv.AllowUserToResizeRows = false;
            //dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            // Hide row header
            dgv.RowHeadersVisible = false;

            // Hide horizontal scrollbar
            dgv.ScrollBars = ScrollBars.Vertical;

            // Align content to center of cell/column
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Set format to column headers
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", fontsize, FontStyle.Bold);

            // Set the selection background color for all the cells.
            dgv.DefaultCellStyle.SelectionBackColor = Color.White;
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Set the background color for all rows and for alternating rows. 
            // The value for alternating rows overrides the value for all rows. 
            ////dgv.RowsDefaultCellStyle.BackColor = Color.LightGray;
            dgv.RowsDefaultCellStyle.BackColor = Color.White;
            ////dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.DarkGray;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.White;

            // Set the row and column header styles.
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            dgv.RowHeadersDefaultCellStyle.BackColor = Color.Black;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.ColumnHeadersHeight = headerHeight;


            // Clear selection
            dgv.ClearSelection();

            // Set font and fontsize
            dgv.DefaultCellStyle.Font = new Font("Times New Roman", fontsize);

            //using (Font font = new Font(dgv.DefaultCellStyle.Font.FontFamily, fontsize, FontStyle.Regular))
            //{
            //    //dgvCodeDefine.Columns["Rating"].DefaultCellStyle.Font = font;
            //    dgv.DefaultCellStyle.Font = font;
            //}

        }

        public static void fnFormatDatagridview(DataGridView dgv, byte fontsize,string scroll)
        {
            // Initialize basic DataGridView properties.
            dgv.Dock = DockStyle.Fill;
            dgv.BackgroundColor = Color.LightGray;
            dgv.BorderStyle = BorderStyle.Fixed3D;

            //// Set property values appropriate for read-only display and 
            //// limited interactivity. 
            //dgv.AllowUserToAddRows = false;
            //dgv.AllowUserToDeleteRows = false;
            //dgv.AllowUserToOrderColumns = true;
            //dgv.ReadOnly = true;
            //dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //dgv.MultiSelect = false;
            //dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            //dgv.AllowUserToResizeColumns = false;
            //dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            //dgv.AllowUserToResizeRows = false;
            //dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            // Hide row header
            dgv.RowHeadersVisible = false;

            // Hide horizontal scrollbar
            switch(scroll)
            {
                case "vertical":
                    dgv.ScrollBars = ScrollBars.Vertical;
                    break;
                case "horizontal":
                    dgv.ScrollBars = ScrollBars.Horizontal;
                    break;
                case "both":
                    dgv.ScrollBars = ScrollBars.Both;
                    break;
            }
            

            // Align content to center of cell/column
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Set format to column headers
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", fontsize, FontStyle.Bold);

            // Set the selection background color for all the cells.
            dgv.DefaultCellStyle.SelectionBackColor = Color.White;
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Set the background color for all rows and for alternating rows. 
            // The value for alternating rows overrides the value for all rows. 
            ////dgv.RowsDefaultCellStyle.BackColor = Color.LightGray;
            dgv.RowsDefaultCellStyle.BackColor = Color.White;
            ////dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.DarkGray;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan;

            // Set the row and column header styles.
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            dgv.RowHeadersDefaultCellStyle.BackColor = Color.Black;


            // Clear selection
            dgv.ClearSelection();

            // Set font and fontsize
            dgv.DefaultCellStyle.Font = new Font("Times New Roman", fontsize);

            //using (Font font = new Font(dgv.DefaultCellStyle.Font.FontFamily, fontsize, FontStyle.Regular))
            //{
            //    //dgvCodeDefine.Columns["Rating"].DefaultCellStyle.Font = font;
            //    dgv.DefaultCellStyle.Font = font;
            //}

        }

        public class MyDataGrid : DataGrid
        {

            public MyDataGrid()
            {
                //make scrollbar visible & hook up handler
                this.VertScrollBar.Visible = true;
                this.VertScrollBar.VisibleChanged += new EventHandler(ShowScrollBars);
            }

            private int CAPTIONHEIGHT = 21;
            private int BORDERWIDTH = 2;

            private void ShowScrollBars(object sender, EventArgs e)
            {
                if (!this.VertScrollBar.Visible)
                {
                    int width = this.VertScrollBar.Width;
                    this.VertScrollBar.Location = new Point(this.ClientRectangle.Width - width - BORDERWIDTH, CAPTIONHEIGHT);
                    this.VertScrollBar.Size = new Size(width, this.ClientRectangle.Height - CAPTIONHEIGHT - BORDERWIDTH);
                    this.VertScrollBar.Show();
                }
            }
        }

        public static int fnGetDataGridWidth(DataGridView dgv)
        {
            int dgvWidth = 0;
            int verticalWidth = (System.Windows.Forms.SystemInformation.VerticalScrollBarWidth + 5);
            if (dgv.Height > dgv.Rows.GetRowsHeight(DataGridViewElementStates.Visible))
            {
                // Scrollbar not visible
                dgvWidth = dgv.Width;
            }
            else
            {
                // Scrollbar visible
                dgvWidth = dgv.Width - verticalWidth;
            }
            //dgvWidth = (scroll.Visible) ? dgv.Width = 20 : dgv.Width;
            //dgvWidth = ((dgv.ScrollBars & ScrollBars.Vertical) != ScrollBars.None) ? dgv.Width : dgv.Width - 20;
            return dgvWidth;
        }

        public static void GetData(string selectCommand, DataGridView dgv, BindingSource bindingSource, SqlDataAdapter dataAdapter)
        {
            try
            {
                // Specify a connection string. Replace the given value with a 
                // valid connection string for a Northwind SQL Server sample
                // database accessible to your system.
                ////String connectionString = GetConnectionStringByName("conn");
                String connectionString = getConnectionString();

                // Create a new data adapter based on the specified query.
                dataAdapter = new SqlDataAdapter(selectCommand, connectionString);

                // Create a command builder to generate SQL update, insert, and
                // delete commands based on selectCommand. These are used to
                // update the database.
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

                // Populate a new data table and bind it to the BindingSource.
                DataTable table = new DataTable();
                table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                dataAdapter.Fill(table);
                bindingSource.DataSource = table;

                // Resize the DataGridView columns to fit the newly loaded content.
                dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
            }
            catch (SqlException)
            {
                MessageBox.Show("Please check connection string again.");
            }
        }

        /// <summary>
        /// creates and open a sqlconnection
        /// </summary>
        /// <param name="connectionString">
        /// A <see cref="System.String"/> that contains the sql connectin parameters
        /// </param>
        /// <returns>
        /// A <see cref="SqlConnection"/> 
        /// </returns>
        public static SqlConnection GetConnection(string connectionString)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
            }
            //catch (SqlException ex)
            catch
            {
                //ex should be written into a error log

                // dispose of the connection to avoid connections leak
                if (connection != null)
                {
                    connection.Dispose();
                }
            }
            return connection;
        }

        /// <summary>
        /// Creates a sqlcommand
        /// </summary>
        /// <param name="connection">
        /// A <see cref="SqlConnection"/>
        /// </param>
        /// <param name="commandText">
        /// A <see cref="System.String"/> of the sql query.
        /// </param>
        /// <param name="commandType">
        /// A <see cref="CommandType"/> of the query type.
        /// </param>
        /// <returns>
        /// A <see cref="SqlCommand"/>
        /// </returns>
        public static SqlCommand GetCommand(this SqlConnection connection, string commandText, CommandType commandType)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandTimeout = connection.ConnectionTimeout;
            command.CommandType = commandType;
            command.CommandText = commandText;
            return command;
        }

        /// <summary>
        /// Adds a parameter to the command parameter array.
        /// </summary>
        /// <param name="command">
        /// A <see cref="SqlCommand"/> 
        /// </param>
        /// <param name="parameterName">
        /// A <see cref="System.String"/> of the named parameter in the sql query.
        /// </param>
        /// <param name="parameterValue">
        /// A <see cref="System.Object"/> of the parameter value.
        /// </param>
        /// <param name="parameterSqlType">
        /// A <see cref="SqlDbType"/>
        /// </param>
        public static void AddParameter(this SqlCommand command, string parameterName, object parameterValue, SqlDbType parameterSqlType)
        {
            if (!parameterName.StartsWith("@"))
            {
                parameterName = "@" + parameterName;
            }
            command.Parameters.Add(parameterName, parameterSqlType);
            command.Parameters[parameterName].Value = parameterValue;
        }

        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "abc!123123";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "abc!123123";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public static string Left(string Text, int TextLenth)
        {
            string ConvertText;
            if (Text.Length < TextLenth)
            {
                TextLenth = Text.Length;
            }
            ConvertText = Text.Substring(0, TextLenth);
            return ConvertText;
        }

        public static string Right(string Text, int TextLenth)
        {
            string ConvertText;
            if (Text.Length < TextLenth)
            {
                TextLenth = Text.Length;
            }
            ConvertText = Text.Substring(Text.Length - TextLenth, TextLenth);
            return ConvertText;
        }

        public static string Mid(string Text, int Startint, int Endint)
        {
            string ConvertText;
            if (Startint < Text.Length || Endint < Text.Length)
            {
                ConvertText = Text.Substring(Startint, Endint);
                return ConvertText;
            }
            else
                return Text;
        }

        public static string IndexOf(string str,byte strPosReturn)
        {
            // strPosReturn: 0: before; 1: after
            string pos = "";
            int dot = str.IndexOf(". ");
            string before = str.Substring(0, dot);
            string after = str.Substring(dot + 1);
            pos = (strPosReturn == 0) ? before : after;
            return pos;
        }

        public static string IndexOf(string str,string compare, byte strPosReturn)
        {
            // strPosReturn: 0: before; 1: after
            string pos = "";
            int dot = str.IndexOf(compare);
            string before = str.Substring(0, dot);
            string after = str.Substring(dot + 1);
            pos = (strPosReturn == 0) ? before : after;
            return pos;
        }

        public static void SerialPortOpen(SerialPort srp, string portname)
        {
            srp.Close();
            srp.PortName = portname;
            srp.BaudRate = 38400;
            srp.DataBits = 8;
            srp.Parity = Parity.None;
            srp.StopBits = StopBits.One;
            srp.Open();
        }

        public static bool IsNumeric(this string s)
        {
            foreach (char c in s)
            {
                if (!char.IsDigit(c) && c != '.')
                {
                    return false;
                }
            }

            return true;
        }

        public class AutoClosingMessageBox
        {
            System.Threading.Timer _timeoutTimer;
            string _caption;
            AutoClosingMessageBox(string text, string caption, int timeout)
            {
                _caption = caption;
                _timeoutTimer = new System.Threading.Timer(OnTimerElapsed,
                    null, timeout, System.Threading.Timeout.Infinite);
                MessageBox.Show(text, caption);
            }

            public static void Show(string text, string caption, int timeout)
            {
                new AutoClosingMessageBox(text, caption, timeout);
            }

            void OnTimerElapsed(object state)
            {
                IntPtr mbWnd = FindWindow(null, _caption);
                if (mbWnd != IntPtr.Zero)
                    SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                _timeoutTimer.Dispose();
            }
            const int WM_CLOSE = 0x0010;
            [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
            static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
            [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        }

        public static string appName()
        {
            string _appname = "";
            return _appname = Application.ProductName;
        }

        public static bool isTimeBetween(this DateTime time, DateTime startTime, DateTime endTime)
        {
            if (time.TimeOfDay == startTime.TimeOfDay) return true;
            if (time.TimeOfDay == endTime.TimeOfDay) return true;

            if (startTime.TimeOfDay <= endTime.TimeOfDay)
                return (time.TimeOfDay >= startTime.TimeOfDay && time.TimeOfDay <= endTime.TimeOfDay);
            else
                return !(time.TimeOfDay >= endTime.TimeOfDay && time.TimeOfDay <= startTime.TimeOfDay);
        }

        public static double fnTimeSpan(DateTime _timeFr, DateTime _timeTo, string type)
        {
            double _span = 0;
            TimeSpan span;
            double second;
            if (_timeTo >= _timeFr)
            {
                span = _timeTo - _timeFr;
            }
            else
            {
                span = _timeTo.AddDays(1) - _timeFr;
            }
            switch(type.ToLower())
            {
                case "h":
                case "hour":
                    second = (span.TotalSeconds) / 3600;
                    _span = second;
                    break;
                case "m":
                case "minute":
                    second = (span.TotalSeconds) / 60;
                    _span = second;
                    break;
                case "s":
                case "second":
                    second = (span.TotalSeconds);
                    _span = second;
                    break;
            }
            return _span;
        }

        public static byte ExportToExcel(DataGridView dgv, string wsheet)
        {
            byte status = 0;
            // Creating a Excel object. 
            Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

            try
            {
                //excel.Columns.ColumnWidth = 25;
                //worksheet = workbook.ActiveSheet;
                excel.Visible = true;
                worksheet = workbook.Sheets["Sheet1"];
                worksheet = workbook.ActiveSheet;
                worksheet.Name = "Records";


                //worksheet.Name = "ExportedFromDatGrid";
                worksheet.Name = wsheet;

                int cellRowIndex = 1;
                int cellColumnIndex = 1;

                for (int i = 0; i < dgv.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1] = dgv.Columns[i].HeaderText;
                }

                //for (int i = 1; i < dgv.Columns.Count + 1; i++)
                //{
                //    excel.Cells[1, i] = dgv.Columns[i - 1].HeaderText;
                //}

                //Loop through each row and read value from each column. 
                for (int i = 0; i < dgv.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < dgv.Columns.Count; j++)
                    {
                        //// Excel index starts from 1,1. As first Row would have the Column headers, adding a condition check. 
                        //if (cellRowIndex == 1)
                        //{
                        //    worksheet.Cells[cellRowIndex, cellColumnIndex] = dgv.Columns[j].HeaderText;
                        //}
                        //else
                        //{
                        //    worksheet.Cells[cellRowIndex, cellColumnIndex] = dgv.Rows[i-1].Cells[j-1].Value.ToString();
                        //}

                        worksheet.Cells[cellRowIndex, cellColumnIndex] = dgv.Rows[i].Cells[j].Value.ToString();
                        cellColumnIndex++;
                    }
                    cellColumnIndex = 1;
                    cellRowIndex++;
                }

                //Getting the location and file name of the excel to save from user. 
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
                saveDialog.FilterIndex = 1;

                if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    workbook.SaveAs(saveDialog.FileName);
                    //MessageBox.Show("Export Successful");
                    status = 1;
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                status = 0;
            }
            finally
            {
                excel.Quit();
                workbook = null;
                excel = null;
            }
            return status;
        }

        public static void ExportTOExcel(DataGridView gridviewID)
        {
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlApp = new Microsoft.Office.Interop.Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            //add data 
            int StartCol = 1;//`enter code here`
                int StartRow = 1;
            int j = 0, i = 0;

            //Write Headers
            for (j = 0; j < gridviewID.Columns.Count; j++)
            {
                Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[StartRow, StartCol + j];
                myRange.Value2 = gridviewID.Columns[j].HeaderText;
            }

            StartRow++;

            //Write datagridview content
            for (i = 0; i < gridviewID.Rows.Count; i++)
            {
                for (j = 0; j < gridviewID.Columns.Count; j++)
                {
                    try
                    {
                        Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[StartRow + i, StartCol + j];
                        myRange.Value2 = gridviewID[j, i].Value == null ? "" : gridviewID[j, i].Value;
                    }
                    catch
                    {
                        ;
                    }
                }
            }

            Microsoft.Office.Interop.Excel.Range chartRange;

            Microsoft.Office.Interop.Excel.ChartObjects xlCharts = (Microsoft.Office.Interop.Excel.ChartObjects)xlWorkSheet.ChartObjects(Type.Missing);
            Microsoft.Office.Interop.Excel.ChartObject myChart = (Microsoft.Office.Interop.Excel.ChartObject)xlCharts.Add(10, 80, 300, 250);
            Microsoft.Office.Interop.Excel.Chart chartPage = myChart.Chart;

            chartRange = xlWorkSheet.get_Range("A1", "B" + gridviewID.Rows.Count);
            chartPage.SetSourceData(chartRange, misValue);
            chartPage.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlColumnClustered;

            xlApp.Visible = true;

        }

        public static void showTooltip(Label lbl, string title, string msg)
        {
            ToolTip yourToolTip = new ToolTip();
            //The below are optional, of course,

            yourToolTip.ToolTipTitle = title;
            yourToolTip.ToolTipIcon = ToolTipIcon.Info;
            yourToolTip.IsBalloon = true;
            yourToolTip.ShowAlways = true;

            yourToolTip.SetToolTip(lbl, msg);
        }

        public static void ManageCheckGroupBox(CheckBox chk, GroupBox grp)
        {
            // Make sure the CheckBox isn't in the GroupBox.
            // This will only happen the first time.
            if (chk.Parent == grp)
            {
                // Reparent the CheckBox so it's not in the GroupBox.
                grp.Parent.Controls.Add(chk);

                // Adjust the CheckBox's location.
                chk.Location = new Point(
                    chk.Left + grp.Left,
                    chk.Top + grp.Top);

                // Move the CheckBox to the top of the stacking order.
                chk.BringToFront();
            }

            // Enable or disable the GroupBox.
            grp.Enabled = chk.Checked;
        }

        public static bool fnCheckPackCode(string packCode)
        {
            string codeCheck = cls.Left(packCode, 3);
            string codeType = cls.Mid(packCode, 4, 3);
            string codeID = cls.Right(packCode, 5);


            if (codeCheck.ToUpper() == "MMT" && (codeType.ToUpper() == "PCS" || codeType.ToUpper() == "BOX" || codeType.ToUpper() == "PAK" || codeType.ToUpper() == "PAL"))
                return true;

            if (codeCheck.ToUpper() == "PRO" && (codeType.ToUpper() == "PCS" || codeType.ToUpper() == "BOX" || codeType.ToUpper() == "CAR" || codeType.ToUpper() == "PAL"))
                return true;

            return false;
        }

        public static void fnDateTime(Label lbl,byte kind)
        {
            DateTime _dt = DateTime.Now;
            if(check.IsConnectedToInternet())
            {
                switch(kind)
                {
                    case 1:
                        lbl.Text = cls.fnGetDate("SD");
                        break;
                    case 2:
                        lbl.Text = cls.fnGetDate("CT");
                        break;
                    case 3:
                        lbl.Text = cls.fnGetDate("SD") + " - " + cls.fnGetDate("CT");
                        break;
                }
                lbl.ForeColor = Color.Black;
            }
            else
            {
                switch (kind)
                {
                    case 1:
                        lbl.Text = String.Format("{0:dd/MM/yyyy}", _dt);
                        break;
                    case 2:
                        lbl.Text = String.Format("{0:HH:mm:ss}", _dt);
                        break;
                    case 3:
                        lbl.Text = String.Format("{0:dd/MM/yyyy HH:mm:ss}", _dt);
                        break;
                }
                lbl.ForeColor = Color.Red;
            }
        }

        public static void fnDateTime(ToolStripStatusLabel lbl, byte kind)
        {
            DateTime _dt = DateTime.Now;
            if (check.IsConnectedToInternet())
            {
                switch (kind)
                {
                    case 1:
                        lbl.Text = cls.fnGetDate("SD");
                        break;
                    case 2:
                        lbl.Text = cls.fnGetDate("CT");
                        break;
                    case 3:
                        lbl.Text = cls.fnGetDate("SD") + " - " + cls.fnGetDate("CT");
                        break;
                }
                lbl.ForeColor = Color.Black;
            }
            else
            {
                switch (kind)
                {
                    case 1:
                        lbl.Text = String.Format("{0:dd/MM/yyyy}", _dt);
                        break;
                    case 2:
                        lbl.Text = String.Format("{0:HH:mm:ss}", _dt);
                        break;
                    case 3:
                        lbl.Text = String.Format("{0:dd/MM/yyyy HH:mm:ss}", _dt);
                        break;
                }
                lbl.ForeColor = Color.Red;
            }
        }

        public static void fnDateTime(Button btn, byte kind)
        {
            DateTime _dt = DateTime.Now;
            if (check.IsConnectedToInternet())
            {
                switch (kind)
                {
                    case 1:
                        btn.Text = cls.fnGetDate("SD");
                        break;
                    case 2:
                        btn.Text = cls.fnGetDate("CT");
                        break;
                    case 3:
                        btn.Text = cls.fnGetDate("SD") + " - " + cls.fnGetDate("CT");
                        break;
                }
                btn.ForeColor = Color.Black;
            }
            else
            {
                switch (kind)
                {
                    case 1:
                        btn.Text = String.Format("{0:dd/MM/yyyy}", _dt);
                        break;
                    case 2:
                        btn.Text = String.Format("{0:HH:mm:ss}", _dt);
                        break;
                    case 3:
                        btn.Text = String.Format("{0:dd/MM/yyyy HH:mm:ss}", _dt);
                        break;
                }
                btn.ForeColor = Color.Red;
            }
        }


        //public static string DecodeText(string sFileName)
        //{
        //    //DmtxImageDecoder decoder = new DmtxImageDecoder();
        //    //System.Drawing.Bitmap oBitmap = new System.Drawing.Bitmap(sFileName);
        //    //List<string> oList = decoder.DecodeImage(oBitmap);

        //    //StringBuilder sb = new StringBuilder();
        //    //sb.Length = 0;
        //    //foreach (string s in oList)
        //    //{
        //    //    sb.Append(s);
        //    //}
        //    //return sb.ToString();
        //}
    }

    public static class Helper
    {
        //public virtual void Button1_Click(object sender, EventArgs args)
        //{
        //    // get the connection
        //    using (SqlConnection connection = Helper.GetConnection("Pooling=true;Min Pool Size=5;Max Pool Size=40;Connect Timeout=10;server=server\instance;database=mydatabase;Integrated Security=false;User Id=username;Password=password;"))
        //    {
        //        //create the command	
        //        using (SqlCommand command = connection.GetCommand("SELECT textBox1 = @textBox1 FROM dbo.table1 WHERE textBox2 = @textBox2", CommandType.Text))
        //        {
        //            // add the parameter
        //            command.AddParameter("@textBox1", TextBox1.Text, SqlDbType.VarChar);
        //            command.AddParameter("@textBox2", TextBox2.Text, SqlDbType.VarChar);

        //            // initialize the reader and execute the command 
        //            using (SqlDataReader reader = command.ExecuteReader())
        //            {

        //                if (!reader.HasRows)
        //                {
        //                    reader.Close();
        //                    command.CommandText = "INSERT INTO dbo.table1 (textBox1, textBox2) VALUES (@textBox1, @textBox2)";
        //                    command.ExecuteNonQuery();
        //                }
        //            }
        //        }

        //        //create the command
        //        using (SqlCommand command = connection.GetCommand("SELECT * FROM dbo.table1 WHERE textBox1 = @textBox1", CommandType.Text))
        //        {
        //            //add the parameters
        //            command.AddParameter("@textBox1", TextBox1.Text, SqlDbType.VarChar);

        //            // initialize the reader and execute the command 
        //            using (SqlDataReader reader = command.ExecuteReader())
        //            {
        //                Label1.Text = Convert.ToString(reader["textBox2"]);
        //            }
        //        }
        //    }
        //}
    }

    class Ini
    {
        private string iniPath;

        static bool factory_index;

        /// <summary>
        /// Vị trí file .ini
        /// </summary>
        /// <param name="path">Vị trí file .ini</param>
        public Ini(string path)
        {
            // TODO: Complete member initialization
            iniPath = path;
        }

        [DllImport("kernel32.dll")]
        //ini 파일 읽기
        private static extern int GetPrivateProfileString(String section, String key, String def, StringBuilder retVal, int size, String filepath);
        [DllImport("kernel32.dll")]
        //ini 파일 쓰기
        private static extern int WritePrivateProfileString(String section, String key, String val, String filepath);

        //ini 파일 유무
        /// <summary>
        /// Kiểm tra file .ini
        /// </summary>
        /// <returns>True: có tồn tại | False: không tìm thấy</returns>
        public bool IniExists()
        {
            factory_index = File.Exists(iniPath);

            return factory_index;
        }

        /// <summary>
        /// ini 파일 생성
        /// </summary>
        public void CreateIni()
        {
            File.Create(iniPath).Close();
        }

        /// <summary>
        /// Đọc giá trị từ file .ini
        /// </summary>
        /// <param name="section">Section của nội dung file .ini</param>
        /// <param name="key">Key của section trong file .ini</param>
        /// <returns>Trả về giá trị của [section, key] tương ứng</returns>
        public string GetIniValue(string section, string key)
        {
            StringBuilder result = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, "", result, 255, iniPath);
            return result.ToString();
        }


        public string GetIniValue(string section, string key, string value)
        {
            StringBuilder result = new StringBuilder(255);

            try
            {
                int i = GetPrivateProfileString(section, key, "", result, 255, iniPath);

                if (result.ToString() == "")
                {
                    SetIniValue(section, key, value);
                    return value.ToString();
                }
                else
                {
                    return result.ToString();
                }

            }
            catch (Exception)
            {
                SetIniValue(section, key, value);
                return value.ToString();

            }

        }
        /// <summary>
        /// Ghi giá trị vào file .ini
        /// </summary>
        /// <param name="section">Section</param>
        /// <param name="key">Key</param>
        /// <param name="val">Giá trị</param>
        public void SetIniValue(string section, string key, string val)
        {
            WritePrivateProfileString(section, key, val, iniPath);
        }
    }

    class msServer
    {
        //DB 접속 정보
        string _serverinfo = "";
        string _mdbinfo = "";

        //실행할 쿼리
        public string _query = "";
        public string _mdbquery = "";

        private SqlConnection _conn;
        private OleDbConnection _mdbconn;

        public DataSet _ds;
        public DataSet _mdbds;

        /// <summary>
        /// 생성자
        /// </summary>
        public msServer()
        {

        }

        public void SetDBInfo(string ip, string name)
        {
            // ini 파일의 DB ip로 세팅, DB 계정 정보는 보안상 프로그램 내에 하드코딩하자!
            //_serverinfo = "server=" + ip + "; database=" + name + "; user id=sait; password=it4118vcs";
            _serverinfo = "server=" + ip + "; database=" + name + "; user id=vnuser; password=dung@2016";
        }

        public void SetMdbInfo()
        {
            _mdbinfo = "Provider = Microsoft.JET.OLEDB.4.0;" + "Data Source = c:\\disconnected.mdb";
        }

        public string State()
        {
            string state;

            //this._conn = new SqlConnection(_serverinfo);

            if (this._conn.State == ConnectionState.Open)
            {
                state = "YES";
            }
            else
            {
                state = "NO";
            }

            return state;
        }

        public void Open()
        {
            try
            {
                this._conn = new SqlConnection(_serverinfo);
                this._conn.Open();
            }
            catch (IndexOutOfRangeException e)
            {
                int a = 0;
            }
            catch (Exception e)
            {
                int a = 0;
            }
        }

        public void MdbOpen()
        {
            try
            {
                this._mdbconn = new OleDbConnection(_mdbinfo);
                this._mdbconn.Open();
            }
            catch (IndexOutOfRangeException e)
            {
                int a = 0;
            }
            catch (Exception e)
            {
                int a = 0;
            }

        }

        public void Close()
        {
            try
            {
                this._conn.Close();
            }
            catch (IndexOutOfRangeException e)
            {
                int a = 0;
            }
            catch (Exception e)
            {
                int a = 0;
            }
        }

        public void MdbClose()
        {
            try
            {
                this._mdbconn.Close();
            }
            catch (IndexOutOfRangeException e)
            {
                int a = 0;
            }
            catch (Exception e)
            {
                int a = 0;
            }
        }

        /// <summary>
        /// select 쿼리 실행
        /// </summary>
        public bool execQuery()
        {
            try
            {
                //SqlDataAdapter 객체 생성
                SqlDataAdapter _sda = new SqlDataAdapter();

                _sda.SelectCommand = new SqlCommand(_query, _conn);

                _ds = new DataSet();    //DataSet 객체 생성

                _sda.Fill(_ds); //생성한 DataSet 객체에 Sda 객체의 데이터를 채우기

                if (_ds.Tables[0].Rows.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (IndexOutOfRangeException e)
            {
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool MdbexecQuery()
        {
            try
            {
                OleDbDataAdapter _oda = new OleDbDataAdapter();

                _oda.SelectCommand = new OleDbCommand(_mdbquery, _mdbconn);

                _mdbds = new DataSet();

                _oda.Fill(_mdbds);

                if (_mdbds.Tables[0].Rows.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (IndexOutOfRangeException e)
            {
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// insert, update 등의 쿼리 실행
        /// </summary>
        public bool execNonQuery()
        {
            try
            {
                SqlCommand _comm = new SqlCommand(_query, _conn);

                if (_comm.ExecuteNonQuery() == -1)  //성공
                {
                    return true;
                }
                else  //실패
                {
                    return false;
                }
            }
            catch (IndexOutOfRangeException e)
            {
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool MdbexecNonQuery()
        {
            try
            {
                OleDbCommand _mdbcomm = new OleDbCommand(_mdbquery, _mdbconn);

                if (_mdbcomm.ExecuteNonQuery() == -1)
                {
                    return true;
                }
                else  //실패
                {
                    return false;
                }
            }
            catch (IndexOutOfRangeException e)
            {
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }

    public static class check
    {
        //Creating the extern function...
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        //Creating a function that uses the API function...
        public static bool IsConnectedToInternet()
        {
            int Desc;
            return InternetGetConnectedState(out Desc, 0);

        }

        public static bool IsConnectedToLAN(string ip)
        {
            Ping x = new Ping();
            PingReply reply = x.Send(IPAddress.Parse(ip));

            if (reply.Status == IPStatus.Success)
                return true;
            else
                return false;
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }
    }

    public static class Prompt
    {
        public static string ShowDialog(string text, string caption, string type)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 40, Width = 400 };
            Button confirmation = new Button() { Text = "CONFIRM", Left = 350, Width = 100, Top = 80, DialogResult = DialogResult.OK };
            switch (type)
            {
                case "1":
                case "pass":
                    textBox.UseSystemPasswordChar = true;
                    break;
                case "0":
                case "text":
                    break;
            }
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }

    public class ClsPrint
    {
        #region Variables

        int iCellHeight = 0; //Used to get/set the datagridview cell height
        int iTotalWidth = 0; //
        int iRow = 0;//Used as counter
        bool bFirstPage = false; //Used to check whether we are printing first page
        bool bNewPage = false;// Used to check whether we are printing a new page
        int iHeaderHeight = 0; //Used for the header height
        StringFormat strFormat; //Used to format the grid rows.
        ArrayList arrColumnLefts = new ArrayList();//Used to save left coordinates of columns
        ArrayList arrColumnWidths = new ArrayList();//Used to save column widths
        private PrintDocument _printDocument = new PrintDocument();
        private DataGridView gw = new DataGridView();
        private string _ReportHeader;

        #endregion

        public ClsPrint(DataGridView gridview, string ReportHeader)
        {
            _printDocument.PrintPage += new PrintPageEventHandler(_printDocument_PrintPage);
            _printDocument.BeginPrint += new PrintEventHandler(_printDocument_BeginPrint);
            gw = gridview;
            _ReportHeader = ReportHeader;
        }

        public void PrintForm()
        {
            ////Open the print dialog
            //PrintDialog printDialog = new PrintDialog();
            //printDialog.Document = _printDocument;
            //printDialog.UseEXDialog = true;

            ////Get the document
            //if (DialogResult.OK == printDialog.ShowDialog())
            //{
            //    _printDocument.DocumentName = "Test Page Print";
            //    _printDocument.Print();
            //}

            //Open the print preview dialog
            PrintPreviewDialog objPPdialog = new PrintPreviewDialog();
            objPPdialog.Document = _printDocument;
            objPPdialog.ShowDialog();
        }

        private void _printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //try
            //{
            //Set the left margin
            int iLeftMargin = e.MarginBounds.Left;
            //Set the top margin
            int iTopMargin = e.MarginBounds.Top;
            //Whether more pages have to print or not
            bool bMorePagesToPrint = false;
            int iTmpWidth = 0;

            //For the first page to print set the cell width and header height
            if (bFirstPage)
            {
                foreach (DataGridViewColumn GridCol in gw.Columns)
                {
                    iTmpWidth = (int)(Math.Floor((double)((double)GridCol.Width /
                        (double)iTotalWidth * (double)iTotalWidth *
                        ((double)e.MarginBounds.Width / (double)iTotalWidth))));

                    iHeaderHeight = (int)(e.Graphics.MeasureString(GridCol.HeaderText,
                        GridCol.InheritedStyle.Font, iTmpWidth).Height) + 11;

                    // Save width and height of headers
                    arrColumnLefts.Add(iLeftMargin);
                    arrColumnWidths.Add(iTmpWidth);
                    iLeftMargin += iTmpWidth;
                }
            }
            //Loop till all the grid rows not get printed
            while (iRow <= gw.Rows.Count - 1)
            {
                DataGridViewRow GridRow = gw.Rows[iRow];
                //Set the cell height
                iCellHeight = GridRow.Height + 5;
                int iCount = 0;
                //Check whether the current page settings allows more rows to print
                if (iTopMargin + iCellHeight >= e.MarginBounds.Height + e.MarginBounds.Top)
                {
                    bNewPage = true;
                    bFirstPage = false;
                    bMorePagesToPrint = true;
                    break;
                }
                else
                {

                    if (bNewPage)
                    {
                        //Draw Header
                        e.Graphics.DrawString(_ReportHeader,
                            new Font(gw.Font, FontStyle.Bold),
                            Brushes.Black, e.MarginBounds.Left,
                            e.MarginBounds.Top - e.Graphics.MeasureString(_ReportHeader,
                            new Font(gw.Font, FontStyle.Bold),
                            e.MarginBounds.Width).Height - 13);

                        String strDate = "";
                        //Draw Date
                        e.Graphics.DrawString(strDate,
                            new Font(gw.Font, FontStyle.Bold), Brushes.Black,
                            e.MarginBounds.Left +
                            (e.MarginBounds.Width - e.Graphics.MeasureString(strDate,
                            new Font(gw.Font, FontStyle.Bold),
                            e.MarginBounds.Width).Width),
                            e.MarginBounds.Top - e.Graphics.MeasureString(_ReportHeader,
                            new Font(new Font(gw.Font, FontStyle.Bold),
                            FontStyle.Bold), e.MarginBounds.Width).Height - 13);

                        //Draw Columns                 
                        iTopMargin = e.MarginBounds.Top;
                        DataGridViewColumn[] _GridCol = new DataGridViewColumn[gw.Columns.Count];
                        int colcount = 0;
                        //Convert ltr to rtl
                        foreach (DataGridViewColumn GridCol in gw.Columns)
                        {
                            _GridCol[colcount++] = GridCol;
                        }
                        //for (int i = (_GridCol.Count() - 1); i >= 0; i--)
                        for (int i = 0; i < (_GridCol.Count() - 1); i++)
                        {
                            e.Graphics.FillRectangle(new SolidBrush(Color.LightGray),
                            new Rectangle((int)arrColumnLefts[iCount], iTopMargin,
                            (int)arrColumnWidths[iCount], iHeaderHeight));

                            e.Graphics.DrawRectangle(Pens.Black,
                                new Rectangle((int)arrColumnLefts[iCount], iTopMargin,
                                (int)arrColumnWidths[iCount], iHeaderHeight));

                            e.Graphics.DrawString(_GridCol[i].HeaderText,
                                _GridCol[i].InheritedStyle.Font,
                                new SolidBrush(_GridCol[i].InheritedStyle.ForeColor),
                                new RectangleF((int)arrColumnLefts[iCount], iTopMargin,
                                (int)arrColumnWidths[iCount], iHeaderHeight), strFormat);
                            iCount++;
                        }
                        bNewPage = false;
                        iTopMargin += iHeaderHeight;
                    }
                    iCount = 0;
                    DataGridViewCell[] _GridCell = new DataGridViewCell[GridRow.Cells.Count];
                    int cellcount = 0;
                    //Convert ltr to rtl
                    foreach (DataGridViewCell Cel in GridRow.Cells)
                    {
                        _GridCell[cellcount++] = Cel;
                    }
                    //Draw Columns Contents                
                    //for (int i = (_GridCell.Count() - 1); i >= 0; i--)
                    for (int i = 0; i < (_GridCell.Count() - 1); i++)
                    {
                        if (_GridCell[i].Value != null)
                        {
                            e.Graphics.DrawString(_GridCell[i].FormattedValue.ToString(),
                                _GridCell[i].InheritedStyle.Font,
                                new SolidBrush(_GridCell[i].InheritedStyle.ForeColor),
                                new RectangleF((int)arrColumnLefts[iCount],
                                (float)iTopMargin,
                                (int)arrColumnWidths[iCount], (float)iCellHeight),
                                strFormat);
                        }
                        //Drawing Cells Borders 
                        e.Graphics.DrawRectangle(Pens.Black,
                            new Rectangle((int)arrColumnLefts[iCount], iTopMargin,
                            (int)arrColumnWidths[iCount], iCellHeight));
                        iCount++;
                    }
                }
                iRow++;
                iTopMargin += iCellHeight;
            }
            //If more lines exist, print another page.
            if (bMorePagesToPrint)
                e.HasMorePages = true;
            else
                e.HasMorePages = false;
            //}
            //catch (Exception exc)
            //{
            //    MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK,
            //       MessageBoxIcon.Error);
            //}
        }

        private void _printDocument_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                strFormat = new StringFormat();
                strFormat.Alignment = StringAlignment.Center;
                strFormat.LineAlignment = StringAlignment.Center;
                strFormat.Trimming = StringTrimming.EllipsisCharacter;

                arrColumnLefts.Clear();
                arrColumnWidths.Clear();
                iCellHeight = 0;
                iRow = 0;
                bFirstPage = true;
                bNewPage = true;

                // Calculating Total Widths
                iTotalWidth = 0;
                foreach (DataGridViewColumn dgvGridCol in gw.Columns)
                {
                    iTotalWidth += dgvGridCol.Width;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }

    class myLabel : System.Windows.Forms.Label
    {
        public int RotateAngle { get; set; }  // to rotate your text
        public string NewText { get; set; }   // to draw text
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            Brush b = new SolidBrush(this.ForeColor);
            e.Graphics.TranslateTransform(this.Width / 2, this.Height / 2);
            e.Graphics.RotateTransform(this.RotateAngle);
            e.Graphics.DrawString(this.NewText, this.Font, b, 0f, 0f);
            base.OnPaint(e);
        }
    }
}
