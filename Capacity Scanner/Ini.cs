// Decompiled with JetBrains decompiler
// Type: Inventory_Data.Ini
// Assembly: Capacity Scanner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 307A2717-2A9D-43F6-AFEA-22C92945443F
// Assembly location: D:\Documents\Visual Studio 2015\_Programs\Capacity Scanner-20170829\Debug\Capacity Scanner.exe

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Inventory_Data
{
  internal class Ini
  {
    private string iniPath;
    private bool factory_index;

    public Ini(string path)
    {
      this.iniPath = path;
    }

    [DllImport("kernel32.dll")]
    private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filepath);

    [DllImport("kernel32.dll")]
    private static extern int WritePrivateProfileString(string section, string key, string val, string filepath);

    public bool IniExists()
    {
      this.factory_index = File.Exists(this.iniPath);
      return this.factory_index;
    }

    public void CreateIni()
    {
      File.Create(this.iniPath).Close();
    }

    public string GetIniValue(string section, string key)
    {
      StringBuilder retVal = new StringBuilder((int) byte.MaxValue);
      Ini.GetPrivateProfileString(section, key, "", retVal, (int) byte.MaxValue, this.iniPath);
      return retVal.ToString();
    }

    public string GetIniValue(string section, string key, string value)
    {
      StringBuilder retVal = new StringBuilder((int) byte.MaxValue);
      try
      {
        Ini.GetPrivateProfileString(section, key, "", retVal, (int) byte.MaxValue, this.iniPath);
        if (!(retVal.ToString() == ""))
          return retVal.ToString();
        this.SetIniValue(section, key, value);
        return value.ToString();
      }
      catch (Exception ex)
      {
        this.SetIniValue(section, key, value);
        return value.ToString();
      }
    }

    public void SetIniValue(string section, string key, string val)
    {
      Ini.WritePrivateProfileString(section, key, val, this.iniPath);
    }
  }
}
