// Decompiled with JetBrains decompiler
// Type: Inventory_Data.frmCapcityMonitorScanner2
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
  public class frmCapcityMonitorScanner2 : Form
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
    public string _prevBarcode;
    public string _code;
    private TableLayoutPanel tableLayoutPanel1;
    private Label lblStatus;
    private Label lblMachine;
    private Label lblPartOKTitle;
    private Label lblPartTotal;
    private Label lblPartTotalTitle;
    private Label lblPartTimeTitle;
    private Label lblPartTime;
    private Label lblPartUPHTitle;
    private Label lblPartUPH;
    private Label lblItemcodeTitle;
    private Label lblItemcode;
    private Label lblPartname;
    private Label lblPartnameTitle;
    private Label lblPartOK;
    private Label lblPartNGTitle;
    private Label lblPartNG;
    private Label lblDateTime;
    private System.Windows.Forms.Timer timer1;
    private Label lblBarcodeTitle;
    private TextBox txtBarcode;
    private Label lblMessage;
    private Label lblRate;
    private Label lblRateTotal;

    public frmCapcityMonitorScanner2()
    {
      this.InitializeComponent();
    }

    private void frmCapcityMonitorScanner2_Load(object sender, EventArgs e)
    {
      this.init();
      this.fnGetdate();
      this.fnGetOrderData();
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      this.fnGetdate();
      string productInfo = cls.getProductInfo("dd/MM/yyyy");
      string shiftNo = cls.getShiftNo();
      string shiftName = cls.getShiftName();
      string str = cls.getValue("select partID from BASE_CapacityModelOrder2 where datediff(day,orderDate,getdate())=0 and orderShift='" + shiftName + "' and orderLine='" + this._machine + "'");
      if (this._shiftno != shiftNo || this._partID != str)
      {
        this._date = productInfo;
        this._shiftname = shiftName;
        this._shiftno = shiftNo;
        this._partID = str;
        this.fnGetOrderData();
      }
      else
      {
        this.lblPartname.Text = this._partname;
        this.lblItemcode.Text = this._partcode;
        this.lblPartUPH.Text = this._partuph;
        this.lblPartTime.Text = this._partordertime;
        this.lblPartTotal.Text = this._partordertotal;
        this.lblPartOK.Text = this._statusOK.ToString();
        this.lblPartNG.Text = this._statusNG.ToString();
        this.lblMachine.Text = this._machine;
        this.lblRateTotal.Text = string.Format("{0:0}", (object) this._rate) + "%";
      }
    }

    public void init()
    {
      this._date = cls.getProductInfo("dd/MM/yyyy");
      this._shiftname = cls.getShiftName();
      this._shiftno = cls.getShiftNo();
      this._machine = this.ini.GetIniValue("MACHINE", "NAME", "DISPENSER").Trim() + " " + this.ini.GetIniValue("MACHINE", "NO", "01").Trim();
      this._partID = cls.getValue("select partID from BASE_CapacityModelOrder2 where datediff(day,orderDate,getdate())=0 and orderShift='" + this._shiftname + "' and orderLine='" + this._machine + "'");
      this.lblStatus.BackColor = Color.Gray;
      this.lblStatus.ForeColor = Color.Gray;
      this.tableLayoutPanel1.BackColor = Color.Gray;
      this.lblMessage.BackColor = Color.Gray;
      this.lblMessage.Text = "";
      this.fnGetOrderData();
    }

    public void fnGetdate()
    {
      this.lblDateTime.Text = cls.getProductInfo("dd/MM/yyyy HH:mm:ss");
    }

    public void fnGetOrderData()
    {
      string lower = this._machine.ToLower();
      Convert.ToDateTime(this._date);
      string sql = "SELECT dbo.BASE_CapacityModelOrder2.idx, dbo.BASE_CapacityModelOrder2.partID, dbo.BASE_CapacityModelOrder2.partName, dbo.BASE_CapacityModelOrder2.partCode, dbo.BASE_CapacityModelOrder2.partUPH, " + "cast(dbo.BASE_CapacityModelOrder2.orderTime as numeric(10,0)), cast(dbo.BASE_CapacityModelOrder2.orderTotal as numeric(10,0)), dbo.BASE_CapacityModelDefine.SubCode01, dbo.BASE_CapacityModelDefine.SubCode02, dbo.BASE_CapacityModelDefine.SubCode03, " + "dbo.BASE_CapacityModelOrder2.achieveOK, dbo.BASE_CapacityModelOrder2.achieveNG, dbo.BASE_CapacityModelOrder2.achieveTotal, dbo.BASE_CapacityModelOrder2.orderLine, dbo.BASE_CapacityModelOrder2.achieveRate " + "FROM dbo.BASE_CapacityModelDefine INNER JOIN dbo.BASE_CapacityModelOrder2 ON dbo.BASE_CapacityModelDefine.ProdId = dbo.BASE_CapacityModelOrder2.partID " + "WHERE (DATEDIFF(DAY, dbo.BASE_CapacityModelOrder2.orderDate, GETDATE()) = 0) AND (LOWER(dbo.BASE_CapacityModelOrder2.orderShift) = '" + this._shiftname.ToLower() + "') AND (LOWER(dbo.BASE_CapacityModelOrder2.orderLine) = '" + lower + "')";
      if (cls.getCount(sql) > 0)
      {
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
          this.lblItemcode.Text = this._partcode;
          this.lblPartUPH.Text = this._partuph;
          this.lblPartTime.Text = this._partordertime;
          this.lblPartTotal.Text = this._partordertotal;
          this.lblPartOK.Text = this._statusOK.ToString();
          this.lblPartNG.Text = this._statusNG.ToString();
          this.lblMachine.Text = this._machine;
          this.lblRateTotal.Text = string.Format("{0:0}", (object) this._rate) + "%";
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
        this.lblPartname.Text = "-";
        this.lblItemcode.Text = "-";
        this.lblPartUPH.Text = "-";
        this.lblPartTime.Text = "-";
        this.lblPartTotal.Text = "-";
        this.lblPartOK.Text = "-";
        this.lblPartNG.Text = "-";
        this.lblMachine.Text = this._machine;
        this.lblRateTotal.Text = "-";
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
        string upper = this.txtBarcode.Text.ToUpper();
        string str = upper.Substring(0, 4);
        if (!(str == "NG-1"))
        {
          if (!(str == "NG+1"))
          {
            if (!(str == "OK-1"))
            {
              if (str == "OK+1")
              {
                this.fnResetCapacityLine((byte) 4);
                this._statusOK = this._statusOK + 1;
                this._statusNG = this._statusNG - 1;
              }
              else
                this.fnInsertDB(upper);
            }
            else
            {
              this.fnResetCapacityLine((byte) 3);
              this._statusOK = this._statusOK - 1;
              this._statusNG = this._statusNG + 1;
            }
          }
          else
          {
            this.fnResetCapacityLine((byte) 2);
            this._statusOK = this._statusOK - 1;
            this._statusNG = this._statusNG + 1;
          }
        }
        else
        {
          this.fnResetCapacityLine((byte) 1);
          this._statusOK = this._statusOK + 1;
          this._statusNG = this._statusNG - 1;
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
        this.txtBarcode.Text = "";
        this.txtBarcode.Focus();
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
            this.lblPartOK.Text = this._statusOK.ToString();
            this.lblMessage.Text = "";
          }
          else
          {
            this.NGStatus();
            this._statusNG = this._statusNG + 1;
            this.lblPartNG.Text = this._statusNG.ToString();
            this.lblMessage.Text = "CANNOT SCAN THE QR BARCODE ON PREVIOUS VALVE   /   KHÔNG QUÉT ĐƯỢC MÃ VẠCH TRÊN THÂN VAN NGAY TRƯỚC ĐÓ";
          }
          this._prevBarcode = str;
        }
        else
        {
          this.NGStatus();
          this._statusNG = this._statusNG + 1;
          this.lblPartNG.Text = this._statusNG.ToString();
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
          this.lblPartOK.Text = this._statusOK.ToString();
          this.lblMessage.Text = "";
        }
        else
        {
          this.NGStatus();
          this._statusNG = this._statusNG + 1;
          this.lblPartNG.Text = this._statusNG.ToString();
          this.lblMessage.Text = "ITEM NG BECAUSE WRONG TYPE   /   HÀNG LỖI DO SAI LOẠI VAN";
        }
        this._total = this._statusOK + this._statusNG;
        this.fnSaveTotal(s);
      }
      this.txtBarcode.Text = "";
      this.txtBarcode.Focus();
    }

    public void OKStatus()
    {
      this.lblStatus.BackColor = Color.DodgerBlue;
      this.lblStatus.ForeColor = Color.White;
      this.lblStatus.Text = "OK";
      this.tableLayoutPanel1.BackColor = Color.DodgerBlue;
      this.lblMessage.BackColor = Color.DodgerBlue;
    }

    public void NGStatus()
    {
      this.lblStatus.BackColor = Color.Red;
      this.lblStatus.ForeColor = Color.White;
      this.lblStatus.Text = "NG";
      this.tableLayoutPanel1.BackColor = Color.Red;
      this.lblMessage.BackColor = Color.Red;
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
      this.timer = new System.Threading.Timer(new TimerCallback(this.TimerElapsed), (object) null, frmCapcityMonitorScanner2.VALIDATION_DELAY, frmCapcityMonitorScanner2.VALIDATION_DELAY);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (frmCapcityMonitorScanner2));
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.lblStatus = new Label();
      this.lblMachine = new Label();
      this.lblPartUPHTitle = new Label();
      this.lblPartUPH = new Label();
      this.lblItemcodeTitle = new Label();
      this.lblItemcode = new Label();
      this.lblPartname = new Label();
      this.lblPartnameTitle = new Label();
      this.lblDateTime = new Label();
      this.lblMessage = new Label();
      this.lblPartTimeTitle = new Label();
      this.lblPartTime = new Label();
      this.lblPartTotalTitle = new Label();
      this.lblPartTotal = new Label();
      this.lblPartOKTitle = new Label();
      this.lblPartOK = new Label();
      this.lblPartNGTitle = new Label();
      this.lblPartNG = new Label();
      this.lblRate = new Label();
      this.lblRateTotal = new Label();
      this.lblBarcodeTitle = new Label();
      this.txtBarcode = new TextBox();
      this.timer1 = new System.Windows.Forms.Timer(this.components);
      this.tableLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      this.tableLayoutPanel1.BackColor = Color.Red;
      this.tableLayoutPanel1.ColumnCount = 20;
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.Controls.Add((Control) this.lblStatus, 0, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblMachine, 0, 18);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblPartUPHTitle, 9, 18);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblPartUPH, 9, 19);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblItemcodeTitle, 6, 18);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblItemcode, 6, 19);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblPartname, 3, 19);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblPartnameTitle, 3, 18);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblDateTime, 0, 19);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblMessage, 0, 17);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblPartTimeTitle, 10, 18);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblPartTime, 10, 19);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblPartTotalTitle, 11, 18);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblPartTotal, 11, 19);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblPartOKTitle, 13, 18);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblPartOK, 13, 19);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblPartNGTitle, 14, 18);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblPartNG, 14, 19);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblRate, 15, 18);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblRateTotal, 15, 19);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblBarcodeTitle, 17, 18);
      this.tableLayoutPanel1.Controls.Add((Control) this.txtBarcode, 17, 19);
      this.tableLayoutPanel1.Dock = DockStyle.Fill;
      this.tableLayoutPanel1.Location = new Point(0, 0);
      this.tableLayoutPanel1.Margin = new Padding(0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 20;
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
      this.tableLayoutPanel1.Size = new Size(1350, 729);
      this.tableLayoutPanel1.TabIndex = 0;
      this.lblStatus.AutoSize = true;
      this.lblStatus.BackColor = Color.Red;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.lblStatus, 20);
      this.lblStatus.Dock = DockStyle.Fill;
      this.lblStatus.Font = new Font("Times New Roman", 480f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblStatus.ForeColor = SystemColors.Window;
      this.lblStatus.Location = new Point(3, 3);
      this.lblStatus.Margin = new Padding(3);
      this.lblStatus.Name = "lblStatus";
      this.tableLayoutPanel1.SetRowSpan((Control) this.lblStatus, 17);
      this.lblStatus.Size = new Size(1344, 606);
      this.lblStatus.TabIndex = 0;
      this.lblStatus.Text = "NG";
      this.lblStatus.TextAlign = ContentAlignment.MiddleCenter;
      this.lblMachine.AutoSize = true;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.lblMachine, 3);
      this.lblMachine.Dock = DockStyle.Fill;
      this.lblMachine.Font = new Font("Times New Roman", 18f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblMachine.ForeColor = SystemColors.Window;
      this.lblMachine.Location = new Point(3, 651);
      this.lblMachine.Margin = new Padding(3);
      this.lblMachine.Name = "lblMachine";
      this.lblMachine.Size = new Size(195, 30);
      this.lblMachine.TabIndex = 1;
      this.lblMachine.Text = "DISPENSER 01";
      this.lblMachine.TextAlign = ContentAlignment.MiddleCenter;
      this.lblPartUPHTitle.AutoSize = true;
      this.lblPartUPHTitle.Dock = DockStyle.Fill;
      this.lblPartUPHTitle.Font = new Font("Times New Roman", 15f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblPartUPHTitle.ForeColor = SystemColors.Window;
      this.lblPartUPHTitle.Location = new Point(606, 651);
      this.lblPartUPHTitle.Margin = new Padding(3);
      this.lblPartUPHTitle.Name = "lblPartUPHTitle";
      this.lblPartUPHTitle.Size = new Size(61, 30);
      this.lblPartUPHTitle.TabIndex = 1;
      this.lblPartUPHTitle.Text = "UPH";
      this.lblPartUPHTitle.TextAlign = ContentAlignment.MiddleCenter;
      this.lblPartUPH.AutoSize = true;
      this.lblPartUPH.Dock = DockStyle.Fill;
      this.lblPartUPH.Font = new Font("Times New Roman", 15f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblPartUPH.ForeColor = SystemColors.Window;
      this.lblPartUPH.Location = new Point(606, 687);
      this.lblPartUPH.Margin = new Padding(3);
      this.lblPartUPH.Name = "lblPartUPH";
      this.lblPartUPH.Size = new Size(61, 39);
      this.lblPartUPH.TabIndex = 1;
      this.lblPartUPH.Text = "label2";
      this.lblPartUPH.TextAlign = ContentAlignment.MiddleCenter;
      this.lblItemcodeTitle.AutoSize = true;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.lblItemcodeTitle, 3);
      this.lblItemcodeTitle.Dock = DockStyle.Fill;
      this.lblItemcodeTitle.Font = new Font("Times New Roman", 15f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblItemcodeTitle.ForeColor = SystemColors.Window;
      this.lblItemcodeTitle.Location = new Point(405, 651);
      this.lblItemcodeTitle.Margin = new Padding(3);
      this.lblItemcodeTitle.Name = "lblItemcodeTitle";
      this.lblItemcodeTitle.Size = new Size(195, 30);
      this.lblItemcodeTitle.TabIndex = 1;
      this.lblItemcodeTitle.Text = "Item code";
      this.lblItemcodeTitle.TextAlign = ContentAlignment.MiddleCenter;
      this.lblItemcode.AutoSize = true;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.lblItemcode, 3);
      this.lblItemcode.Dock = DockStyle.Fill;
      this.lblItemcode.Font = new Font("Times New Roman", 15f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblItemcode.ForeColor = SystemColors.Window;
      this.lblItemcode.Location = new Point(405, 687);
      this.lblItemcode.Margin = new Padding(3);
      this.lblItemcode.Name = "lblItemcode";
      this.lblItemcode.Size = new Size(195, 39);
      this.lblItemcode.TabIndex = 1;
      this.lblItemcode.Text = "label2";
      this.lblItemcode.TextAlign = ContentAlignment.MiddleCenter;
      this.lblPartname.AutoSize = true;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.lblPartname, 3);
      this.lblPartname.Dock = DockStyle.Fill;
      this.lblPartname.Font = new Font("Times New Roman", 15f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblPartname.ForeColor = SystemColors.Window;
      this.lblPartname.Location = new Point(204, 687);
      this.lblPartname.Margin = new Padding(3);
      this.lblPartname.Name = "lblPartname";
      this.lblPartname.Size = new Size(195, 39);
      this.lblPartname.TabIndex = 1;
      this.lblPartname.Text = "label2";
      this.lblPartname.TextAlign = ContentAlignment.MiddleCenter;
      this.lblPartnameTitle.AutoSize = true;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.lblPartnameTitle, 3);
      this.lblPartnameTitle.Dock = DockStyle.Fill;
      this.lblPartnameTitle.Font = new Font("Times New Roman", 15f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblPartnameTitle.ForeColor = SystemColors.Window;
      this.lblPartnameTitle.Location = new Point(204, 651);
      this.lblPartnameTitle.Margin = new Padding(3);
      this.lblPartnameTitle.Name = "lblPartnameTitle";
      this.lblPartnameTitle.Size = new Size(195, 30);
      this.lblPartnameTitle.TabIndex = 1;
      this.lblPartnameTitle.Text = "Part name";
      this.lblPartnameTitle.TextAlign = ContentAlignment.MiddleCenter;
      this.lblDateTime.AutoSize = true;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.lblDateTime, 3);
      this.lblDateTime.Dock = DockStyle.Fill;
      this.lblDateTime.Font = new Font("Times New Roman", 15f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblDateTime.ForeColor = SystemColors.Window;
      this.lblDateTime.Location = new Point(3, 687);
      this.lblDateTime.Margin = new Padding(3);
      this.lblDateTime.Name = "lblDateTime";
      this.lblDateTime.Size = new Size(195, 39);
      this.lblDateTime.TabIndex = 1;
      this.lblDateTime.Text = "15/08/2017 16:05:50";
      this.lblDateTime.TextAlign = ContentAlignment.MiddleCenter;
      this.lblMessage.AutoSize = true;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.lblMessage, 20);
      this.lblMessage.Dock = DockStyle.Fill;
      this.lblMessage.Font = new Font("Times New Roman", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblMessage.ForeColor = SystemColors.Window;
      this.lblMessage.Location = new Point(3, 615);
      this.lblMessage.Margin = new Padding(3);
      this.lblMessage.Name = "lblMessage";
      this.lblMessage.Size = new Size(1344, 30);
      this.lblMessage.TabIndex = 1;
      this.lblMessage.Text = "Item code";
      this.lblMessage.TextAlign = ContentAlignment.MiddleCenter;
      this.lblPartTimeTitle.AutoSize = true;
      this.lblPartTimeTitle.Dock = DockStyle.Fill;
      this.lblPartTimeTitle.Font = new Font("Times New Roman", 15f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblPartTimeTitle.ForeColor = SystemColors.Window;
      this.lblPartTimeTitle.Location = new Point(673, 651);
      this.lblPartTimeTitle.Margin = new Padding(3);
      this.lblPartTimeTitle.Name = "lblPartTimeTitle";
      this.lblPartTimeTitle.Size = new Size(61, 30);
      this.lblPartTimeTitle.TabIndex = 1;
      this.lblPartTimeTitle.Text = "Time";
      this.lblPartTimeTitle.TextAlign = ContentAlignment.MiddleCenter;
      this.lblPartTime.AutoSize = true;
      this.lblPartTime.Dock = DockStyle.Fill;
      this.lblPartTime.Font = new Font("Times New Roman", 15f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblPartTime.ForeColor = SystemColors.Window;
      this.lblPartTime.Location = new Point(673, 687);
      this.lblPartTime.Margin = new Padding(3);
      this.lblPartTime.Name = "lblPartTime";
      this.lblPartTime.Size = new Size(61, 39);
      this.lblPartTime.TabIndex = 1;
      this.lblPartTime.Text = "label2";
      this.lblPartTime.TextAlign = ContentAlignment.MiddleCenter;
      this.lblPartTotalTitle.AutoSize = true;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.lblPartTotalTitle, 2);
      this.lblPartTotalTitle.Dock = DockStyle.Fill;
      this.lblPartTotalTitle.Font = new Font("Times New Roman", 15f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblPartTotalTitle.ForeColor = SystemColors.Window;
      this.lblPartTotalTitle.Location = new Point(740, 651);
      this.lblPartTotalTitle.Margin = new Padding(3);
      this.lblPartTotalTitle.Name = "lblPartTotalTitle";
      this.lblPartTotalTitle.Size = new Size(128, 30);
      this.lblPartTotalTitle.TabIndex = 1;
      this.lblPartTotalTitle.Text = "TOTAL";
      this.lblPartTotalTitle.TextAlign = ContentAlignment.MiddleCenter;
      this.lblPartTotal.AutoSize = true;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.lblPartTotal, 2);
      this.lblPartTotal.Dock = DockStyle.Fill;
      this.lblPartTotal.Font = new Font("Times New Roman", 15f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblPartTotal.ForeColor = SystemColors.Window;
      this.lblPartTotal.Location = new Point(740, 687);
      this.lblPartTotal.Margin = new Padding(3);
      this.lblPartTotal.Name = "lblPartTotal";
      this.lblPartTotal.Size = new Size(128, 39);
      this.lblPartTotal.TabIndex = 1;
      this.lblPartTotal.Text = "label2";
      this.lblPartTotal.TextAlign = ContentAlignment.MiddleCenter;
      this.lblPartOKTitle.AutoSize = true;
      this.lblPartOKTitle.Dock = DockStyle.Fill;
      this.lblPartOKTitle.Font = new Font("Times New Roman", 15f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblPartOKTitle.ForeColor = SystemColors.Window;
      this.lblPartOKTitle.Location = new Point(874, 651);
      this.lblPartOKTitle.Margin = new Padding(3);
      this.lblPartOKTitle.Name = "lblPartOKTitle";
      this.lblPartOKTitle.Size = new Size(61, 30);
      this.lblPartOKTitle.TabIndex = 1;
      this.lblPartOKTitle.Text = "OK";
      this.lblPartOKTitle.TextAlign = ContentAlignment.MiddleCenter;
      this.lblPartOK.AutoSize = true;
      this.lblPartOK.BackColor = Color.DeepSkyBlue;
      this.lblPartOK.Dock = DockStyle.Fill;
      this.lblPartOK.Font = new Font("Times New Roman", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblPartOK.ForeColor = SystemColors.Window;
      this.lblPartOK.Location = new Point(874, 687);
      this.lblPartOK.Margin = new Padding(3);
      this.lblPartOK.Name = "lblPartOK";
      this.lblPartOK.Size = new Size(61, 39);
      this.lblPartOK.TabIndex = 1;
      this.lblPartOK.Text = "0";
      this.lblPartOK.TextAlign = ContentAlignment.MiddleCenter;
      this.lblPartNGTitle.AutoSize = true;
      this.lblPartNGTitle.Dock = DockStyle.Fill;
      this.lblPartNGTitle.Font = new Font("Times New Roman", 15f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblPartNGTitle.ForeColor = SystemColors.Window;
      this.lblPartNGTitle.Location = new Point(941, 651);
      this.lblPartNGTitle.Margin = new Padding(3);
      this.lblPartNGTitle.Name = "lblPartNGTitle";
      this.lblPartNGTitle.Size = new Size(61, 30);
      this.lblPartNGTitle.TabIndex = 1;
      this.lblPartNGTitle.Text = "NG";
      this.lblPartNGTitle.TextAlign = ContentAlignment.MiddleCenter;
      this.lblPartNG.AutoSize = true;
      this.lblPartNG.BackColor = Color.DarkOrange;
      this.lblPartNG.Dock = DockStyle.Fill;
      this.lblPartNG.Font = new Font("Times New Roman", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblPartNG.ForeColor = SystemColors.Window;
      this.lblPartNG.Location = new Point(941, 687);
      this.lblPartNG.Margin = new Padding(3);
      this.lblPartNG.Name = "lblPartNG";
      this.lblPartNG.Size = new Size(61, 39);
      this.lblPartNG.TabIndex = 1;
      this.lblPartNG.Text = "0";
      this.lblPartNG.TextAlign = ContentAlignment.MiddleCenter;
      this.lblRate.AutoSize = true;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.lblRate, 2);
      this.lblRate.Dock = DockStyle.Fill;
      this.lblRate.Font = new Font("Times New Roman", 15f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblRate.ForeColor = SystemColors.Window;
      this.lblRate.Location = new Point(1008, 651);
      this.lblRate.Margin = new Padding(3);
      this.lblRate.Name = "lblRate";
      this.lblRate.Size = new Size(128, 30);
      this.lblRate.TabIndex = 1;
      this.lblRate.Text = "RATE";
      this.lblRate.TextAlign = ContentAlignment.MiddleCenter;
      this.lblRateTotal.AutoSize = true;
      this.lblRateTotal.BackColor = Color.Violet;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.lblRateTotal, 2);
      this.lblRateTotal.Dock = DockStyle.Fill;
      this.lblRateTotal.Font = new Font("Times New Roman", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblRateTotal.ForeColor = SystemColors.Window;
      this.lblRateTotal.Location = new Point(1008, 687);
      this.lblRateTotal.Margin = new Padding(3);
      this.lblRateTotal.Name = "lblRateTotal";
      this.lblRateTotal.Size = new Size(128, 39);
      this.lblRateTotal.TabIndex = 1;
      this.lblRateTotal.Text = "0%";
      this.lblRateTotal.TextAlign = ContentAlignment.MiddleCenter;
      this.lblBarcodeTitle.AutoSize = true;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.lblBarcodeTitle, 3);
      this.lblBarcodeTitle.Dock = DockStyle.Fill;
      this.lblBarcodeTitle.Font = new Font("Times New Roman", 15f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblBarcodeTitle.ForeColor = SystemColors.Window;
      this.lblBarcodeTitle.Location = new Point(1142, 651);
      this.lblBarcodeTitle.Margin = new Padding(3);
      this.lblBarcodeTitle.Name = "lblBarcodeTitle";
      this.lblBarcodeTitle.Size = new Size(205, 30);
      this.lblBarcodeTitle.TabIndex = 1;
      this.lblBarcodeTitle.Text = "ITEM BARCODE";
      this.lblBarcodeTitle.TextAlign = ContentAlignment.MiddleCenter;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.txtBarcode, 3);
      this.txtBarcode.Dock = DockStyle.Fill;
      this.txtBarcode.Font = new Font("Times New Roman", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtBarcode.Location = new Point(1142, 687);
      this.txtBarcode.Name = "txtBarcode";
      this.txtBarcode.Size = new Size(205, 38);
      this.txtBarcode.TabIndex = 2;
      this.txtBarcode.TextAlign = HorizontalAlignment.Center;
      this.txtBarcode.TextChanged += new EventHandler(this.txtBarcode_TextChanged);
      this.timer1.Enabled = true;
      this.timer1.Interval = 1000;
      this.timer1.Tick += new EventHandler(this.timer1_Tick);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1350, 729);
      this.Controls.Add((Control) this.tableLayoutPanel1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MinimumSize = new Size(1366, 768);
      this.Name = nameof (frmCapcityMonitorScanner2);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "CAPCITY MONITOR SCANNER2";
      this.WindowState = FormWindowState.Maximized;
      this.Load += new EventHandler(this.frmCapcityMonitorScanner2_Load);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
