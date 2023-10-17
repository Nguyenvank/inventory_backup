// Decompiled with JetBrains decompiler
// Type: PIEBALD.Lib.LibWin
// Assembly: Capacity Scanner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 307A2717-2A9D-43F6-AFEA-22C92945443F
// Assembly location: D:\Documents\Visual Studio 2015\_Programs\Capacity Scanner-20170829\Debug\Capacity Scanner.exe

using System;
using System.Drawing;
using System.Windows.Forms;

namespace PIEBALD.Lib
{
  public static class LibWin
  {
    private const int lagniappe = 2;

    public static bool ResizeFormToFitDataGridView(DataGridView Grid, Rectangle Constraint, LibWin.ResizeFormOption Options)
    {
      bool flag = true;
      Form form = Grid.FindForm();
      if (form.FormBorderStyle != FormBorderStyle.FixedSingle && form.FormBorderStyle != FormBorderStyle.FixedToolWindow)
      {
        int num1 = 2;
        int num2 = 2;
        int width1 = num1 + (form.Width - Grid.Width) + Grid.RowHeadersWidth;
        for (int index = 0; index < Grid.ColumnCount; ++index)
        {
          if (Grid.Columns[index].Visible)
            width1 += Grid.Columns[index].Width;
        }
        int height1 = num2 + (form.Height - Grid.Height) + Grid.ColumnHeadersHeight;
        for (int index = 0; index < Grid.RowCount; ++index)
        {
          if (Grid.Rows[index].Visible)
            height1 += Grid.Rows[index].Height;
        }
        Size size1;
        if ((Options & LibWin.ResizeFormOption.HonorMinimumSize) == LibWin.ResizeFormOption.HonorMinimumSize)
        {
          int num3 = width1;
          size1 = form.MinimumSize;
          int width2 = size1.Width;
          if (num3 < width2)
          {
            size1 = form.MinimumSize;
            width1 = size1.Width;
          }
          int num4 = height1;
          size1 = form.MinimumSize;
          int height2 = size1.Height;
          if (num4 < height2)
          {
            size1 = form.MinimumSize;
            height1 = size1.Height;
          }
        }
        if ((Options & LibWin.ResizeFormOption.HonorMaximumSize) == LibWin.ResizeFormOption.HonorMaximumSize)
        {
          size1 = form.MaximumSize;
          int num3;
          if (size1.Width > 0)
          {
            int num4 = width1;
            size1 = form.MaximumSize;
            int width2 = size1.Width;
            num3 = num4 > width2 ? 1 : 0;
          }
          else
            num3 = 0;
          if (num3 != 0)
          {
            size1 = form.MaximumSize;
            width1 = size1.Width;
          }
          size1 = form.MaximumSize;
          int num5;
          if (size1.Height > 0)
          {
            int num4 = height1;
            size1 = form.MaximumSize;
            int height2 = size1.Height;
            num5 = num4 > height2 ? 1 : 0;
          }
          else
            num5 = 0;
          if (num5 != 0)
          {
            size1 = form.MaximumSize;
            height1 = size1.Height;
          }
        }
        Size size2 = new Size(width1, height1);
        if (form.Size != size2)
        {
          if (Constraint != Rectangle.Empty)
          {
            if (width1 > Constraint.Width)
            {
              form.Left = Constraint.X;
              width1 = Constraint.Left + Constraint.Width - form.Left;
            }
            else if (width1 > Constraint.Left + Constraint.Width - form.Left)
              form.Left -= width1 - (Constraint.Left + Constraint.Width - form.Left);
            if (height1 > Constraint.Height)
            {
              form.Top = Constraint.Y;
              height1 = Constraint.Y + Constraint.Height - form.Top;
            }
            else if (height1 > Constraint.Top + Constraint.Height - form.Top)
              form.Top -= height1 - (Constraint.Y + Constraint.Height - form.Top);
          }
          Size size3 = new Size(width1, height1);
          if (form.Size != size3)
          {
            try
            {
              form.Size = size3;
              if ((Options & LibWin.ResizeFormOption.Freeze) == LibWin.ResizeFormOption.Freeze)
              {
                form.MaximumSize = size2;
                form.MinimumSize = size3;
                if (size3 == size2)
                {
                  switch (form.FormBorderStyle)
                  {
                    case FormBorderStyle.Sizable:
                      form.FormBorderStyle = FormBorderStyle.FixedSingle;
                      break;
                    case FormBorderStyle.SizableToolWindow:
                      form.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                      break;
                  }
                }
              }
            }
            catch (ArgumentOutOfRangeException ex)
            {
              if ((Options & LibWin.ResizeFormOption.Throw) == LibWin.ResizeFormOption.Throw)
              {
                ex.Data[(object) "Top"] = (object) form.Top;
                ex.Data[(object) "Left"] = (object) form.Left;
                ex.Data[(object) "Width"] = (object) width1;
                ex.Data[(object) "Height"] = (object) height1;
                throw;
              }
              else
                flag = false;
            }
          }
        }
      }
      return flag;
    }

    public enum ResizeFormOption
    {
      None = 0,
      Freeze = 1,
      HonorMinimumSize = 2,
      HonorMaximumSize = 4,
      Throw = 8,
    }
  }
}
