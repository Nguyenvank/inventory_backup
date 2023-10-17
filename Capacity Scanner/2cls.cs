// Decompiled with JetBrains decompiler
// Type: GridViewProgress.DataGridViewProgressCell
// Assembly: Capacity Scanner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 307A2717-2A9D-43F6-AFEA-22C92945443F
// Assembly location: D:\Documents\Visual Studio 2015\_Programs\Capacity Scanner-20170829\Debug\Capacity Scanner.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace GridViewProgress
{
  internal class DataGridViewProgressCell : DataGridViewImageCell
  {
    private static Image emptyImage = (Image) new Bitmap(1, 1, PixelFormat.Format32bppArgb);

    public DataGridViewProgressCell()
    {
      this.ValueType = typeof (int);
    }

    protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
    {
      return (object) DataGridViewProgressCell.emptyImage;
    }

    protected override void Paint(Graphics g, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
    {
      try
      {
        int num1 = (int) value;
        float num2 = (float) num1 / 100f;
        Brush brush1 = (Brush) new SolidBrush(cellStyle.BackColor);
        Brush brush2 = (Brush) new SolidBrush(cellStyle.ForeColor);
        base.Paint(g, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts & ~DataGridViewPaintParts.ContentForeground);
        if ((double) num2 > 0.0)
        {
          g.FillRectangle((Brush) new SolidBrush(Color.FromArgb(203, 235, 108)), cellBounds.X + 2, cellBounds.Y + 2, Convert.ToInt32((float) ((double) num2 * (double) cellBounds.Width - 4.0)), cellBounds.Height - 4);
          g.DrawString(num1.ToString() + "%", cellStyle.Font, brush2, (float) (cellBounds.X + cellBounds.Width / 2 - 5), (float) (cellBounds.Y + 2));
        }
        else if (this.DataGridView.CurrentRow.Index == rowIndex)
          g.DrawString(num1.ToString() + "%", cellStyle.Font, (Brush) new SolidBrush(cellStyle.SelectionForeColor), (float) (cellBounds.X + 6), (float) (cellBounds.Y + 2));
        else
          g.DrawString(num1.ToString() + "%", cellStyle.Font, brush2, (float) (cellBounds.X + 6), (float) (cellBounds.Y + 2));
      }
      catch (Exception ex)
      {
      }
    }
  }
}
