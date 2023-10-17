// Decompiled with JetBrains decompiler
// Type: drawingstyle.RotatingLabel
// Assembly: Capacity Scanner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 307A2717-2A9D-43F6-AFEA-22C92945443F
// Assembly location: D:\Documents\Visual Studio 2015\_Programs\Capacity Scanner-20170829\Debug\Capacity Scanner.exe

using System;
using System.Drawing;
using System.Windows.Forms;

namespace drawingstyle
{
  public class RotatingLabel : Label
  {
    private int m_RotateAngle = 0;
    private string m_NewText = string.Empty;

    public int RotateAngle
    {
      get
      {
        return this.m_RotateAngle;
      }
      set
      {
        this.m_RotateAngle = value;
        this.Invalidate();
      }
    }

    public string NewText
    {
      get
      {
        return this.m_NewText;
      }
      set
      {
        this.m_NewText = value;
        this.Invalidate();
      }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      Func<double, double> func = (Func<double, double>) (angle => Math.PI * angle / 180.0);
      Brush brush = (Brush) new SolidBrush(this.ForeColor);
      SizeF sizeF = e.Graphics.MeasureString(this.NewText, this.Font, this.Parent.Width);
      int num1 = (this.RotateAngle % 360 + 360) % 360;
      double num2 = func((double) num1);
      int num3 = (int) Math.Ceiling((double) sizeF.Height * Math.Sin(num2));
      int num4 = (int) Math.Ceiling((double) sizeF.Width * Math.Cos(num2));
      int num5 = (int) Math.Ceiling((double) sizeF.Width * Math.Sin(num2));
      int num6 = (int) Math.Ceiling((double) sizeF.Height * Math.Cos(num2));
      int num7 = Math.Abs(num3) + Math.Abs(num4);
      int num8 = Math.Abs(num5) + Math.Abs(num6);
      this.Width = num7;
      this.Height = num8;
      int num9 = num1 < 0 || num1 >= 90 ? (num1 < 90 || num1 >= 180 ? (num1 < 180 || num1 >= 270 ? (num1 < 270 || num1 >= 360 ? 0 : 4) : 3) : 2) : 1;
      int num10 = 0;
      int num11 = 0;
      if (num9 == 1)
        num10 = Math.Abs(num3);
      else if (num9 == 2)
      {
        num10 = num7;
        num11 = Math.Abs(num6);
      }
      else if (num9 == 3)
      {
        num10 = Math.Abs(num4);
        num11 = num8;
      }
      else if (num9 == 4)
        num11 = Math.Abs(num5);
      e.Graphics.TranslateTransform((float) num10, (float) num11);
      e.Graphics.RotateTransform((float) this.RotateAngle);
      e.Graphics.DrawString(this.NewText, this.Font, brush, 0.0f, 0.0f);
      base.OnPaint(e);
    }
  }
}
