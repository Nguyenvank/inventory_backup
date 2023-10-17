// Decompiled with JetBrains decompiler
// Type: Inventory_Data.frmCapcityMonitorScanner3
// Assembly: Capacity Scanner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 307A2717-2A9D-43F6-AFEA-22C92945443F
// Assembly location: D:\Documents\Visual Studio 2015\_Programs\Capacity Scanner-20170829\Debug\Capacity Scanner.exe

using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Inventory_Data
{
  public class frmCapcityMonitorScanner3 : Form
  {
    private static int VALIDATION_DELAY = 200;
    private Ini ini = new Ini(Application.StartupPath + "\\" + Application.ProductName + ".ini");
    private System.Threading.Timer timer = (System.Threading.Timer) null;
    private System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
    private IContainer components = (IContainer) null;
    public string _date;
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
    private TableLayoutPanel tableLayoutPanel1;
    private Label lblStatus;
    private Label lblMessage;
    private Label lblLinename;
    private Label label4;
    private Label lblPartname;
    private Label lblDateTime;
    private Label label7;
    private Label lblPartcode;
    private Label label19;
    private Label label20;
    private Label lblOrderUPH;
    private Label lblOrderTime;
    private Label lblOrderTotal;
    private Label label24;
    private Label label25;
    private Label label9;
    private Label label11;
    private Label lblAchieveOK;
    private Label lblAchieveNG;
    private Label label12;
    private Label label13;
    private Label lblAchieveRate;
    private Label label17;
    private TextBox txtItemCode;
    private Label label8;
    private Label lblNextOrder;
    private System.Windows.Forms.Timer timer1;
    private TableLayoutPanel tableLayoutPanel2;

    public frmCapcityMonitorScanner3()
    {
      this.InitializeComponent();
    }

    private void frmCapcityMonitorScanner3_Load(object sender, EventArgs e)
    {
      this.init();
      this.fnGetOrderData();
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      this.fnGetDate();
      this.fnGetNextOrder();
      string productInfo = cls.getProductInfo("dd/MM/yyyy");
      string shiftNo = cls.getShiftNo();
      string shiftName = cls.getShiftName();
      string str = cls.getValue("select idx from BASE_CapacityModelOrder2 where (datediff(day,orderDate,getdate())=0) AND orderLine='" + this._machine + "' AND orderShift='" + shiftName + "' AND (cast(getdate() as time) between cast(timeFrom as time) and cast(timeTo as time))");
      if (str != this._idx)
      {
        this._date = productInfo;
        this._shiftname = shiftName;
        this._shiftno = shiftNo;
        this._idx = str;
        this.fnGetOrderData();
      }
      else
      {
        this.lblLinename.Text = this._machine;
        this.lblPartname.Text = this._partname;
        this.lblPartcode.Text = this._partcode;
        this.lblOrderUPH.Text = this._partuph;
        this.lblOrderTime.Text = this._partordertime;
        this.lblOrderTotal.Text = this._partordertotal;
        this.lblAchieveOK.Text = this._statusOK.ToString();
        this.lblAchieveNG.Text = this._statusNG.ToString();
        this.lblAchieveRate.Text = string.Format("{0:0.0}", (object) this._rate) + "%";
      }
      this.txtItemCode.Focus();
    }

    public void init()
    {
      this.fnGetDate();
      this.lblMessage.Text = "";
      this.BackColor = Color.FromKnownColor(KnownColor.Control);
      this.lblStatus.BackColor = Color.Gray;
      this.lblStatus.ForeColor = Color.Gray;
      this.lblMessage.BackColor = Color.Gray;
      this._date = cls.getProductInfo("dd/MM/yyyy");
      this._shiftname = cls.getShiftName();
      this._shiftno = cls.getShiftNo();
      this._machine = this.ini.GetIniValue("MACHINE", "NAME", "DISPENSER").Trim() + " " + this.ini.GetIniValue("MACHINE", "NO", "01").Trim();
      this._idx = cls.getValue("select idx from BASE_CapacityModelOrder2 where (datediff(day,orderDate,getdate())=0) AND orderLine='" + this._machine + "' AND orderShift='" + this._shiftname + "' AND (cast(getdate() as time) between cast(timeFrom as time) and cast(timeTo as time))");
      this.fnGetNextOrder();
    }

    public void fnGetDate()
    {
      this.lblDateTime.Text = cls.getProductInfo("dd/MM/yyyy HH:mm:ss");
    }

    public void fnGetNextOrder()
    {
      this._nextIdx = cls.getValue("select top 1 idx from BASE_CapacityModelOrder2 where (datediff(day,orderDate,getdate())=0) and (orderShift='" + this._shiftname + "') and (orderLine='" + this._machine + "') and (cast(getdate() as time) <= cast(timeFrom as time))");
      this._nextPartname = cls.getValue("select top 1 partName from BASE_CapacityModelOrder2 where idx=" + this._nextIdx);
      this._nextTime = cls.getValue("select top 1 cast(timeFrom as time) from BASE_CapacityModelOrder2 where idx=" + this._nextIdx);
      if (this._nextIdx != "")
      {
        this.lblNextOrder.Text = "(" + this._nextTime.Substring(0, 5) + ") " + this._nextPartname;
        this.lblNextOrder.BackColor = Color.Yellow;
        this.lblNextOrder.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
      }
      else
      {
        this.lblNextOrder.Text = "-";
        this.lblNextOrder.BackColor = Color.Gray;
        this.lblNextOrder.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
      }
    }

    public void fnGetOrderData()
    {
      string str = "SELECT dbo.BASE_CapacityModelOrder2.idx, dbo.BASE_CapacityModelOrder2.partID, dbo.BASE_CapacityModelOrder2.partName, dbo.BASE_CapacityModelOrder2.partCode, dbo.BASE_CapacityModelOrder2.partUPH, " + "cast(dbo.BASE_CapacityModelOrder2.orderTime as numeric(10,0)), cast(dbo.BASE_CapacityModelOrder2.orderTotal as numeric(10,0)), dbo.BASE_CapacityModelDefine.SubCode01, dbo.BASE_CapacityModelDefine.SubCode02, dbo.BASE_CapacityModelDefine.SubCode03, " + "dbo.BASE_CapacityModelOrder2.achieveOK, dbo.BASE_CapacityModelOrder2.achieveNG, dbo.BASE_CapacityModelOrder2.achieveTotal, dbo.BASE_CapacityModelOrder2.orderLine, dbo.BASE_CapacityModelOrder2.achieveRate " + "FROM dbo.BASE_CapacityModelDefine INNER JOIN dbo.BASE_CapacityModelOrder2 ON dbo.BASE_CapacityModelDefine.ProdId = dbo.BASE_CapacityModelOrder2.partID " + "WHERE dbo.BASE_CapacityModelOrder2.idx=" + this._idx;
      if (this._idx != "")
      {
        this.txtItemCode.Enabled = true;
        this.txtItemCode.Focus();
        this.txtItemCode.BackColor = Color.DarkKhaki;
        SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        sqlConnection.Open();
        SqlCommand selectCommand = new SqlCommand();
        selectCommand.CommandText = str;
        selectCommand.Connection = sqlConnection;
        try
        {
          selectCommand.ExecuteNonQuery();
          DataSet dataSet = new DataSet();
          new SqlDataAdapter(selectCommand).Fill(dataSet, "Sum");
          this._idx = dataSet.Tables["Sum"].Rows[0][0].ToString();
          this._partname = dataSet.Tables["Sum"].Rows[0][2].ToString();
          this._partcode = dataSet.Tables["Sum"].Rows[0][3].ToString();
          this._partuph = dataSet.Tables["Sum"].Rows[0][4].ToString();
          this._partordertime = dataSet.Tables["Sum"].Rows[0][5].ToString();
          this._partordertotal = dataSet.Tables["Sum"].Rows[0][6].ToString();
          this._partsubcode01 = dataSet.Tables["Sum"].Rows[0][7].ToString();
          this._partsubcode02 = dataSet.Tables["Sum"].Rows[0][8].ToString();
          this._partsubcode03 = dataSet.Tables["Sum"].Rows[0][9].ToString();
          this._statusOK = Convert.ToInt32(dataSet.Tables["Sum"].Rows[0][10].ToString());
          this._statusNG = Convert.ToInt32(dataSet.Tables["Sum"].Rows[0][11].ToString());
          this._total = Convert.ToInt32(dataSet.Tables["Sum"].Rows[0][12].ToString());
          this._machine = dataSet.Tables["Sum"].Rows[0][13].ToString().ToUpper();
          this._rate = Convert.ToDecimal(dataSet.Tables["Sum"].Rows[0][14].ToString());
          this.lblPartname.Text = this._partname;
          this.lblPartcode.Text = this._partcode;
          this.lblOrderUPH.Text = this._partuph;
          this.lblOrderTime.Text = this._partordertime;
          this.lblOrderTotal.Text = this._partordertotal;
          this.lblAchieveOK.Text = this._statusOK.ToString();
          this.lblAchieveNG.Text = this._statusNG.ToString();
          this.lblLinename.Text = this._machine;
          this.lblAchieveRate.Text = string.Format("{0:0}", (object) this._rate) + "%";
        }
        catch
        {
        }
        finally
        {
          sqlConnection.Close();
        }
      }
      else
      {
        this.txtItemCode.Enabled = false;
        this.txtItemCode.BackColor = Color.Gray;
        this.lblPartname.Text = "-";
        this.lblPartcode.Text = "-";
        this.lblOrderUPH.Text = "-";
        this.lblOrderTime.Text = "-";
        this.lblOrderTotal.Text = "-";
        this.lblAchieveOK.Text = "-";
        this.lblAchieveNG.Text = "-";
        this.lblLinename.Text = this._machine;
        this.lblAchieveRate.Text = "-";
      }
    }

    private void TimerElapsed(object obj)
    {
      this.CheckSyntaxAndReport();
      this.DisposeTimer();
    }

    private void DisposeTimer()
    {
      if (this.timer == null)
        return;
      this.timer.Dispose();
      this.timer = (System.Threading.Timer) null;
    }

    private void CheckSyntaxAndReport()
    {
      this.Invoke((Delegate) (() =>
      {
        this.fnDisplayMsg();
        string upper = this.txtItemCode.Text.ToUpper();
        string str1 = upper.Substring(0, 4);
        if (str1 == "NG-1" || str1 == "NG+1" || str1 == "OK-1" || str1 == "OK+1")
        {
          if (this._statusOK >= 1 && this._statusNG >= 1)
          {
            string str2 = str1;
            if (!(str2 == "NG-1"))
            {
              if (!(str2 == "NG+1"))
              {
                if (!(str2 == "OK-1"))
                {
                  if (str2 == "OK+1")
                    this.fnResetCapacityLine((byte) 4);
                }
                else
                  this.fnResetCapacityLine((byte) 3);
              }
              else
                this.fnResetCapacityLine((byte) 2);
            }
            else
              this.fnResetCapacityLine((byte) 1);
          }
          this.txtItemCode.Text = "";
          this.txtItemCode.Focus();
        }
        else
        {
          try
          {
            this.fnInsertDB(upper);
          }
          catch (Exception ex)
          {
            int num = (int) MessageBox.Show(ex.ToString());
          }
          finally
          {
          }
        }
      }));
    }

    public void fnResetCapacityLine(byte type)
    {
      SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
      sqlConnection.Open();
      SqlCommand sqlCommand = new SqlCommand();
      sqlCommand.CommandType = CommandType.StoredProcedure;
      sqlCommand.CommandText = "BASE_CapacityModelTotal2ResetQuantity_Addnew";
      sqlCommand.Parameters.Add("@type", SqlDbType.Int).Value = (object) type;
      sqlCommand.Parameters.Add("@idx", SqlDbType.Int).Value = (object) this._idx;
      sqlCommand.Connection = sqlConnection;
      try
      {
        sqlCommand.ExecuteNonQuery();
        this.fnGetOrderData();
        this.txtItemCode.Text = "";
        this.txtItemCode.Focus();
      }
      catch
      {
      }
      finally
      {
        sqlConnection.Close();
        sqlConnection.Dispose();
      }
    }

    public void fnInsertDB(string s)
    {
      if ((!(this._partsubcode01 != "") ? 0 : (!(this._partsubcode02 != "") ? 1 : (!(this._partsubcode03 != "") ? 2 : 3))) > 1)
      {
        if (s.Contains(this._partsubcode01) && this._partsubcode01 != "" || s.Contains(this._partsubcode02) && this._partsubcode02 != "" || s.Contains(this._partsubcode03) && this._partsubcode03 != "")
        {
          string str = "";
          if (s.Contains(this._partsubcode01) && this._partsubcode01 != "")
            str = this._partsubcode01;
          else if (s.Contains(this._partsubcode02) && this._partsubcode02 != "")
            str = this._partsubcode02;
          else if (s.Contains(this._partsubcode03) && this._partsubcode03 != "")
            str = this._partsubcode03;
          if (str != this._prevBarcode)
          {
            this.OKStatus();
            if (s.Contains(this._partsubcode01))
            {
              this._statusOK = this._statusOK + 1;
              this._prevBarcode = s;
            }
            this.lblAchieveOK.Text = this._statusOK.ToString();
            this.lblMessage.Text = "";
          }
          else
          {
            this.NGStatus();
            this._statusNG = this._statusNG + 1;
            this.lblAchieveNG.Text = this._statusNG.ToString();
            this.lblMessage.Text = "CANNOT SCAN THE QR BARCODE ON PREVIOUS VALVE   /   KHÔNG QUÉT ĐƯỢC MÃ VẠCH TRÊN THÂN VAN NGAY TRƯỚC ĐÓ";
          }
          this._prevBarcode = str;
        }
        else
        {
          this.NGStatus();
          this._statusNG = this._statusNG + 1;
          this.lblAchieveNG.Text = this._statusNG.ToString();
          this.lblMessage.Text = "ITEM NG BECAUSE WRONG TYPE   /   HÀNG LỖI DO SAI LOẠI VAN";
        }
        this._total = this._statusOK + this._statusNG;
        this.fnSaveTotal(s);
      }
      else
      {
        if (s.Contains(this._partsubcode01))
        {
          this.OKStatus();
          this._statusOK = this._statusOK + 1;
          this.lblAchieveOK.Text = this._statusOK.ToString();
          this.lblMessage.Text = "";
        }
        else
        {
          this.NGStatus();
          this._statusNG = this._statusNG + 1;
          this.lblAchieveNG.Text = this._statusNG.ToString();
          this.lblMessage.Text = "ITEM NG BECAUSE WRONG TYPE   /   HÀNG LỖI DO SAI LOẠI VAN";
        }
        this._total = this._statusOK + this._statusNG;
        this.fnSaveTotal(s);
      }
      this.txtItemCode.Text = "";
      this.txtItemCode.Focus();
    }

    public void OKStatus()
    {
      this.lblStatus.BackColor = Color.DodgerBlue;
      this.lblStatus.ForeColor = Color.White;
      this.lblStatus.Text = "OK";
      this.lblMessage.BackColor = Color.DodgerBlue;
      this.lblMessage.ForeColor = Color.White;
    }

    public void NGStatus()
    {
      this.lblStatus.BackColor = Color.Red;
      this.lblStatus.ForeColor = Color.White;
      this.lblStatus.Text = "NG";
      this.lblMessage.BackColor = Color.Red;
      this.lblMessage.ForeColor = Color.White;
    }

    public void fnSaveTotal(string barcode)
    {
      SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
      sqlConnection.Open();
      SqlCommand sqlCommand = new SqlCommand();
      sqlCommand.CommandType = CommandType.StoredProcedure;
      sqlCommand.CommandText = "BASE_CapacityModelTotal2_Addnew";
      sqlCommand.Parameters.Add("@totalOK", SqlDbType.Int).Value = (object) this._statusOK;
      sqlCommand.Parameters.Add("@totalNG", SqlDbType.Int).Value = (object) this._statusNG;
      sqlCommand.Parameters.Add("@total", SqlDbType.Int).Value = (object) this._total;
      sqlCommand.Parameters.Add("@idx", SqlDbType.Int).Value = (object) this._idx;
      sqlCommand.Parameters.Add("@barcode", SqlDbType.VarChar).Value = (object) barcode;
      sqlCommand.Connection = sqlConnection;
      try
      {
        sqlCommand.ExecuteNonQuery();
        this.fnGetOrderData();
      }
      catch
      {
      }
      finally
      {
        sqlConnection.Close();
        sqlConnection.Dispose();
      }
    }

    private void txtBarcode_TextChanged(object sender, EventArgs e)
    {
      if (!(sender as TextBox).ContainsFocus)
        return;
      this.DisposeTimer();
      this.timer = new System.Threading.Timer(new TimerCallback(this.TimerElapsed), (object) null, frmCapcityMonitorScanner3.VALIDATION_DELAY, frmCapcityMonitorScanner3.VALIDATION_DELAY);
    }

    public void fnDisplayMsg()
    {
      this.t.Interval = 1500;
      this.t.Tick += new EventHandler(this.fnChangeStatusBackColor);
      this.t.Enabled = true;
      this.t.Start();
    }

    public void fnChangeStatusBackColor(object sender, EventArgs e)
    {
      this.lblMessage.Text = "Waiting for checking...   /   Đang chờ để kiểm tra...";
      this.lblMessage.ForeColor = Color.White;
      this.lblStatus.Text = "";
      this.t.Stop();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (frmCapcityMonitorScanner3));
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.lblStatus = new Label();
      this.label4 = new Label();
      this.lblPartname = new Label();
      this.lblMessage = new Label();
      this.label7 = new Label();
      this.lblPartcode = new Label();
      this.label19 = new Label();
      this.label20 = new Label();
      this.lblOrderUPH = new Label();
      this.lblOrderTime = new Label();
      this.lblOrderTotal = new Label();
      this.label24 = new Label();
      this.label25 = new Label();
      this.label9 = new Label();
      this.label11 = new Label();
      this.lblAchieveOK = new Label();
      this.lblAchieveNG = new Label();
      this.label12 = new Label();
      this.label13 = new Label();
      this.lblAchieveRate = new Label();
      this.label17 = new Label();
      this.label8 = new Label();
      this.lblNextOrder = new Label();
      this.lblLinename = new Label();
      this.lblDateTime = new Label();
      this.tableLayoutPanel2 = new TableLayoutPanel();
      this.txtItemCode = new TextBox();
      this.timer1 = new System.Windows.Forms.Timer(this.components);
      this.tableLayoutPanel1.SuspendLayout();
      this.tableLayoutPanel2.SuspendLayout();
      this.SuspendLayout();
      this.tableLayoutPanel1.ColumnCount = 25;
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 3f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 3f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 3f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 3f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.Controls.Add((Control) this.lblStatus, 0, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.label4, 4, 22);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblPartname, 4, 23);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblMessage, 0, 21);
      this.tableLayoutPanel1.Controls.Add((Control) this.label7, 8, 22);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblPartcode, 8, 23);
      this.tableLayoutPanel1.Controls.Add((Control) this.label19, 11, 22);
      this.tableLayoutPanel1.Controls.Add((Control) this.label20, 11, 23);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblOrderUPH, 11, 24);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblOrderTime, 12, 24);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblOrderTotal, 13, 24);
      this.tableLayoutPanel1.Controls.Add((Control) this.label24, 12, 23);
      this.tableLayoutPanel1.Controls.Add((Control) this.label25, 13, 23);
      this.tableLayoutPanel1.Controls.Add((Control) this.label9, 15, 22);
      this.tableLayoutPanel1.Controls.Add((Control) this.label11, 15, 23);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblAchieveOK, 15, 24);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblAchieveNG, 16, 24);
      this.tableLayoutPanel1.Controls.Add((Control) this.label12, 16, 23);
      this.tableLayoutPanel1.Controls.Add((Control) this.label13, 17, 23);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblAchieveRate, 17, 24);
      this.tableLayoutPanel1.Controls.Add((Control) this.label17, 19, 22);
      this.tableLayoutPanel1.Controls.Add((Control) this.label8, 19, 24);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblNextOrder, 21, 24);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblLinename, 0, 23);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblDateTime, 0, 22);
      this.tableLayoutPanel1.Controls.Add((Control) this.tableLayoutPanel2, 19, 23);
      this.tableLayoutPanel1.Dock = DockStyle.Fill;
      this.tableLayoutPanel1.Location = new Point(0, 0);
      this.tableLayoutPanel1.Margin = new Padding(0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 25;
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 4f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
      this.tableLayoutPanel1.Size = new Size(1350, 729);
      this.tableLayoutPanel1.TabIndex = 0;
      this.lblStatus.AutoSize = true;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.lblStatus, 25);
      this.lblStatus.Dock = DockStyle.Fill;
      this.lblStatus.Font = new Font("Times New Roman", 500f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblStatus.Location = new Point(1, 1);
      this.lblStatus.Margin = new Padding(1, 1, 1, 0);
      this.lblStatus.Name = "lblStatus";
      this.tableLayoutPanel1.SetRowSpan((Control) this.lblStatus, 21);
      this.lblStatus.Size = new Size(1348, 608);
      this.lblStatus.TabIndex = 0;
      this.lblStatus.Text = "OK";
      this.lblStatus.TextAlign = ContentAlignment.TopCenter;
      this.label4.AutoSize = true;
      this.label4.BackColor = Color.DeepSkyBlue;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.label4, 4);
      this.label4.Dock = DockStyle.Fill;
      this.label4.Font = new Font("Times New Roman", 15f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(217, 639);
      this.label4.Margin = new Padding(1);
      this.label4.Name = "label4";
      this.label4.Size = new Size(214, 27);
      this.label4.TabIndex = 0;
      this.label4.Text = "PART NAME";
      this.label4.TextAlign = ContentAlignment.MiddleCenter;
      this.lblPartname.AutoSize = true;
      this.lblPartname.BackColor = Color.DeepSkyBlue;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.lblPartname, 4);
      this.lblPartname.Dock = DockStyle.Fill;
      this.lblPartname.Font = new Font("Times New Roman", 15f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblPartname.Location = new Point(217, 668);
      this.lblPartname.Margin = new Padding(1);
      this.lblPartname.Name = "lblPartname";
      this.tableLayoutPanel1.SetRowSpan((Control) this.lblPartname, 2);
      this.lblPartname.Size = new Size(214, 60);
      this.lblPartname.TabIndex = 0;
      this.lblPartname.Text = "DISPENSER 43 (27\")";
      this.lblPartname.TextAlign = ContentAlignment.MiddleCenter;
      this.lblMessage.AutoSize = true;
      this.lblMessage.BackColor = SystemColors.Control;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.lblMessage, 25);
      this.lblMessage.Dock = DockStyle.Fill;
      this.lblMessage.Font = new Font("Times New Roman", 15f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblMessage.ForeColor = SystemColors.Window;
      this.lblMessage.Location = new Point(1, 609);
      this.lblMessage.Margin = new Padding(1, 0, 1, 1);
      this.lblMessage.Name = "lblMessage";
      this.lblMessage.Size = new Size(1348, 28);
      this.lblMessage.TabIndex = 0;
      this.lblMessage.Text = "label1";
      this.lblMessage.TextAlign = ContentAlignment.MiddleCenter;
      this.label7.AutoSize = true;
      this.label7.BackColor = Color.DeepSkyBlue;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.label7, 3);
      this.label7.Dock = DockStyle.Fill;
      this.label7.Font = new Font("Times New Roman", 15f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label7.Location = new Point(433, 639);
      this.label7.Margin = new Padding(1);
      this.label7.Name = "label7";
      this.label7.Size = new Size(160, 27);
      this.label7.TabIndex = 0;
      this.label7.Text = "PART CODE";
      this.label7.TextAlign = ContentAlignment.MiddleCenter;
      this.lblPartcode.AutoSize = true;
      this.lblPartcode.BackColor = Color.DeepSkyBlue;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.lblPartcode, 3);
      this.lblPartcode.Dock = DockStyle.Fill;
      this.lblPartcode.Font = new Font("Times New Roman", 15f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblPartcode.Location = new Point(433, 668);
      this.lblPartcode.Margin = new Padding(1);
      this.lblPartcode.Name = "lblPartcode";
      this.tableLayoutPanel1.SetRowSpan((Control) this.lblPartcode, 2);
      this.lblPartcode.Size = new Size(160, 60);
      this.lblPartcode.TabIndex = 0;
      this.lblPartcode.Text = "label1";
      this.lblPartcode.TextAlign = ContentAlignment.MiddleCenter;
      this.label19.AutoSize = true;
      this.label19.BackColor = Color.Orange;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.label19, 4);
      this.label19.Dock = DockStyle.Fill;
      this.label19.Font = new Font("Times New Roman", 15f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label19.Location = new Point(595, 639);
      this.label19.Margin = new Padding(1);
      this.label19.Name = "label19";
      this.label19.Size = new Size(212, 27);
      this.label19.TabIndex = 0;
      this.label19.Text = "ORDER";
      this.label19.TextAlign = ContentAlignment.MiddleCenter;
      this.label20.AutoSize = true;
      this.label20.BackColor = Color.Orange;
      this.label20.Dock = DockStyle.Fill;
      this.label20.Font = new Font("Times New Roman", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label20.Location = new Point(595, 668);
      this.label20.Margin = new Padding(1);
      this.label20.Name = "label20";
      this.label20.Size = new Size(65, 27);
      this.label20.TabIndex = 0;
      this.label20.Text = "UPH";
      this.label20.TextAlign = ContentAlignment.MiddleCenter;
      this.lblOrderUPH.AutoSize = true;
      this.lblOrderUPH.BackColor = Color.Orange;
      this.lblOrderUPH.Dock = DockStyle.Fill;
      this.lblOrderUPH.Font = new Font("Times New Roman", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblOrderUPH.Location = new Point(595, 697);
      this.lblOrderUPH.Margin = new Padding(1);
      this.lblOrderUPH.Name = "lblOrderUPH";
      this.lblOrderUPH.Size = new Size(65, 31);
      this.lblOrderUPH.TabIndex = 0;
      this.lblOrderUPH.Text = "0";
      this.lblOrderUPH.TextAlign = ContentAlignment.MiddleCenter;
      this.lblOrderTime.AutoSize = true;
      this.lblOrderTime.BackColor = Color.Orange;
      this.lblOrderTime.Dock = DockStyle.Fill;
      this.lblOrderTime.Font = new Font("Times New Roman", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblOrderTime.Location = new Point(662, 697);
      this.lblOrderTime.Margin = new Padding(1);
      this.lblOrderTime.Name = "lblOrderTime";
      this.lblOrderTime.Size = new Size(65, 31);
      this.lblOrderTime.TabIndex = 0;
      this.lblOrderTime.Text = "0";
      this.lblOrderTime.TextAlign = ContentAlignment.MiddleCenter;
      this.lblOrderTotal.AutoSize = true;
      this.lblOrderTotal.BackColor = Color.Orange;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.lblOrderTotal, 2);
      this.lblOrderTotal.Dock = DockStyle.Fill;
      this.lblOrderTotal.Font = new Font("Times New Roman", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblOrderTotal.Location = new Point(729, 697);
      this.lblOrderTotal.Margin = new Padding(1);
      this.lblOrderTotal.Name = "lblOrderTotal";
      this.lblOrderTotal.Size = new Size(78, 31);
      this.lblOrderTotal.TabIndex = 0;
      this.lblOrderTotal.Text = "0";
      this.lblOrderTotal.TextAlign = ContentAlignment.MiddleCenter;
      this.label24.AutoSize = true;
      this.label24.BackColor = Color.Orange;
      this.label24.Dock = DockStyle.Fill;
      this.label24.Font = new Font("Times New Roman", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label24.Location = new Point(662, 668);
      this.label24.Margin = new Padding(1);
      this.label24.Name = "label24";
      this.label24.Size = new Size(65, 27);
      this.label24.TabIndex = 0;
      this.label24.Text = "TIME";
      this.label24.TextAlign = ContentAlignment.MiddleCenter;
      this.label25.AutoSize = true;
      this.label25.BackColor = Color.Orange;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.label25, 2);
      this.label25.Dock = DockStyle.Fill;
      this.label25.Font = new Font("Times New Roman", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label25.Location = new Point(729, 668);
      this.label25.Margin = new Padding(1);
      this.label25.Name = "label25";
      this.label25.Size = new Size(78, 27);
      this.label25.TabIndex = 0;
      this.label25.Text = "TOTAL";
      this.label25.TextAlign = ContentAlignment.MiddleCenter;
      this.label9.AutoSize = true;
      this.label9.BackColor = Color.YellowGreen;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.label9, 4);
      this.label9.Dock = DockStyle.Fill;
      this.label9.Font = new Font("Times New Roman", 15f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label9.Location = new Point(809, 639);
      this.label9.Margin = new Padding(1);
      this.label9.Name = "label9";
      this.label9.Size = new Size(212, 27);
      this.label9.TabIndex = 0;
      this.label9.Text = "ACHIEVE";
      this.label9.TextAlign = ContentAlignment.MiddleCenter;
      this.label11.AutoSize = true;
      this.label11.BackColor = Color.YellowGreen;
      this.label11.Dock = DockStyle.Fill;
      this.label11.Font = new Font("Times New Roman", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label11.Location = new Point(809, 668);
      this.label11.Margin = new Padding(1);
      this.label11.Name = "label11";
      this.label11.Size = new Size(65, 27);
      this.label11.TabIndex = 0;
      this.label11.Text = "OK";
      this.label11.TextAlign = ContentAlignment.MiddleCenter;
      this.lblAchieveOK.AutoSize = true;
      this.lblAchieveOK.BackColor = Color.YellowGreen;
      this.lblAchieveOK.Dock = DockStyle.Fill;
      this.lblAchieveOK.Font = new Font("Times New Roman", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblAchieveOK.Location = new Point(809, 697);
      this.lblAchieveOK.Margin = new Padding(1);
      this.lblAchieveOK.Name = "lblAchieveOK";
      this.lblAchieveOK.Size = new Size(65, 31);
      this.lblAchieveOK.TabIndex = 0;
      this.lblAchieveOK.Text = "0";
      this.lblAchieveOK.TextAlign = ContentAlignment.MiddleCenter;
      this.lblAchieveNG.AutoSize = true;
      this.lblAchieveNG.BackColor = Color.YellowGreen;
      this.lblAchieveNG.Dock = DockStyle.Fill;
      this.lblAchieveNG.Font = new Font("Times New Roman", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblAchieveNG.Location = new Point(876, 697);
      this.lblAchieveNG.Margin = new Padding(1);
      this.lblAchieveNG.Name = "lblAchieveNG";
      this.lblAchieveNG.Size = new Size(65, 31);
      this.lblAchieveNG.TabIndex = 0;
      this.lblAchieveNG.Text = "0";
      this.lblAchieveNG.TextAlign = ContentAlignment.MiddleCenter;
      this.label12.AutoSize = true;
      this.label12.BackColor = Color.YellowGreen;
      this.label12.Dock = DockStyle.Fill;
      this.label12.Font = new Font("Times New Roman", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label12.Location = new Point(876, 668);
      this.label12.Margin = new Padding(1);
      this.label12.Name = "label12";
      this.label12.Size = new Size(65, 27);
      this.label12.TabIndex = 0;
      this.label12.Text = "NG";
      this.label12.TextAlign = ContentAlignment.MiddleCenter;
      this.label13.AutoSize = true;
      this.label13.BackColor = Color.YellowGreen;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.label13, 2);
      this.label13.Dock = DockStyle.Fill;
      this.label13.Font = new Font("Times New Roman", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label13.Location = new Point(943, 668);
      this.label13.Margin = new Padding(1);
      this.label13.Name = "label13";
      this.label13.Size = new Size(78, 27);
      this.label13.TabIndex = 0;
      this.label13.Text = "RATE";
      this.label13.TextAlign = ContentAlignment.MiddleCenter;
      this.lblAchieveRate.AutoSize = true;
      this.lblAchieveRate.BackColor = Color.YellowGreen;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.lblAchieveRate, 2);
      this.lblAchieveRate.Dock = DockStyle.Fill;
      this.lblAchieveRate.Font = new Font("Times New Roman", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblAchieveRate.Location = new Point(943, 697);
      this.lblAchieveRate.Margin = new Padding(1);
      this.lblAchieveRate.Name = "lblAchieveRate";
      this.lblAchieveRate.Size = new Size(78, 31);
      this.lblAchieveRate.TabIndex = 0;
      this.lblAchieveRate.Text = "0";
      this.lblAchieveRate.TextAlign = ContentAlignment.MiddleCenter;
      this.label17.AutoSize = true;
      this.label17.BackColor = Color.DarkKhaki;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.label17, 6);
      this.label17.Dock = DockStyle.Fill;
      this.label17.Font = new Font("Times New Roman", 15f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label17.Location = new Point(1023, 639);
      this.label17.Margin = new Padding(1);
      this.label17.Name = "label17";
      this.label17.Size = new Size(326, 27);
      this.label17.TabIndex = 0;
      this.label17.Text = "ITEM BARCODE";
      this.label17.TextAlign = ContentAlignment.MiddleCenter;
      this.label8.AutoSize = true;
      this.label8.BackColor = Color.Yellow;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.label8, 2);
      this.label8.Dock = DockStyle.Fill;
      this.label8.Font = new Font("Times New Roman", 15f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label8.Location = new Point(1023, 697);
      this.label8.Margin = new Padding(1);
      this.label8.Name = "label8";
      this.label8.Size = new Size(106, 31);
      this.label8.TabIndex = 0;
      this.label8.Text = "Next order:";
      this.label8.TextAlign = ContentAlignment.MiddleCenter;
      this.lblNextOrder.AutoSize = true;
      this.lblNextOrder.BackColor = Color.Yellow;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.lblNextOrder, 4);
      this.lblNextOrder.Dock = DockStyle.Fill;
      this.lblNextOrder.Font = new Font("Times New Roman", 15f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblNextOrder.Location = new Point(1131, 697);
      this.lblNextOrder.Margin = new Padding(1);
      this.lblNextOrder.Name = "lblNextOrder";
      this.lblNextOrder.Size = new Size(218, 31);
      this.lblNextOrder.TabIndex = 0;
      this.lblNextOrder.TextAlign = ContentAlignment.MiddleCenter;
      this.lblLinename.AutoSize = true;
      this.lblLinename.BackColor = Color.Green;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.lblLinename, 4);
      this.lblLinename.Dock = DockStyle.Fill;
      this.lblLinename.Font = new Font("Times New Roman", 21f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblLinename.ForeColor = SystemColors.Window;
      this.lblLinename.Location = new Point(1, 668);
      this.lblLinename.Margin = new Padding(1);
      this.lblLinename.Name = "lblLinename";
      this.tableLayoutPanel1.SetRowSpan((Control) this.lblLinename, 2);
      this.lblLinename.Size = new Size(214, 60);
      this.lblLinename.TabIndex = 0;
      this.lblLinename.Text = "DISPENSER 01";
      this.lblLinename.TextAlign = ContentAlignment.MiddleCenter;
      this.lblDateTime.AutoSize = true;
      this.lblDateTime.BackColor = Color.Green;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.lblDateTime, 4);
      this.lblDateTime.Dock = DockStyle.Fill;
      this.lblDateTime.Font = new Font("Times New Roman", 15f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblDateTime.ForeColor = SystemColors.Window;
      this.lblDateTime.Location = new Point(1, 639);
      this.lblDateTime.Margin = new Padding(1);
      this.lblDateTime.Name = "lblDateTime";
      this.lblDateTime.Size = new Size(214, 27);
      this.lblDateTime.TabIndex = 0;
      this.lblDateTime.Text = "19/08/2017 14:26:15";
      this.lblDateTime.TextAlign = ContentAlignment.MiddleCenter;
      this.tableLayoutPanel2.BackColor = Color.DarkKhaki;
      this.tableLayoutPanel2.ColumnCount = 1;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.tableLayoutPanel2, 6);
      this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20f));
      this.tableLayoutPanel2.Controls.Add((Control) this.txtItemCode, 0, 0);
      this.tableLayoutPanel2.Dock = DockStyle.Fill;
      this.tableLayoutPanel2.Location = new Point(1022, 667);
      this.tableLayoutPanel2.Margin = new Padding(0);
      this.tableLayoutPanel2.Name = "tableLayoutPanel2";
      this.tableLayoutPanel2.RowCount = 1;
      this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 29f));
      this.tableLayoutPanel2.Size = new Size(328, 29);
      this.tableLayoutPanel2.TabIndex = 2;
      this.txtItemCode.BackColor = Color.DarkKhaki;
      this.txtItemCode.BorderStyle = BorderStyle.None;
      this.txtItemCode.CharacterCasing = CharacterCasing.Upper;
      this.tableLayoutPanel2.SetColumnSpan((Control) this.txtItemCode, 6);
      this.txtItemCode.Dock = DockStyle.Fill;
      this.txtItemCode.Font = new Font("Times New Roman", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtItemCode.Location = new Point(1, 1);
      this.txtItemCode.Margin = new Padding(1);
      this.txtItemCode.Name = "txtItemCode";
      this.txtItemCode.Size = new Size(326, 31);
      this.txtItemCode.TabIndex = 1;
      this.txtItemCode.TextAlign = HorizontalAlignment.Center;
      this.txtItemCode.TextChanged += new EventHandler(this.txtBarcode_TextChanged);
      this.timer1.Enabled = true;
      this.timer1.Interval = 1000;
      this.timer1.Tick += new EventHandler(this.timer1_Tick);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1350, 729);
      this.Controls.Add((Control) this.tableLayoutPanel1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MinimumSize = new Size(1366, 768);
      this.Name = nameof (frmCapcityMonitorScanner3);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "CAPCITY MONITORS CANNER v3";
      this.WindowState = FormWindowState.Maximized;
      this.Load += new EventHandler(this.frmCapcityMonitorScanner3_Load);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.tableLayoutPanel2.ResumeLayout(false);
      this.tableLayoutPanel2.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
