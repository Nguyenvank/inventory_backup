// Decompiled with JetBrains decompiler
// Type: GridViewProgress.DataGridViewProgressColumn
// Assembly: Capacity Scanner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 307A2717-2A9D-43F6-AFEA-22C92945443F
// Assembly location: D:\Documents\Visual Studio 2015\_Programs\Capacity Scanner-20170829\Debug\Capacity Scanner.exe

using System.Windows.Forms;

namespace GridViewProgress
{
  public class DataGridViewProgressColumn : DataGridViewImageColumn
  {
    public DataGridViewProgressColumn()
    {
      this.CellTemplate = (DataGridViewCell) new DataGridViewProgressCell();
    }
  }
}
