using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace S7.Controls.S7300
{
    public class DI16DO16 : S7300_32 
    {
        public DI16DO16()
        {
            this.TextType = "DI16/DO16";
            this.TextLeft = "IN";
            this.TextRight = "OUT";
        }
    }
}
