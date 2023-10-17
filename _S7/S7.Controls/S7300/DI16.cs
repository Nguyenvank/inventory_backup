using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace S7.Controls.S7300
{
    public class DI16 : S7300_16 
    {
        public DI16()
        {
            this.TextType = "DI16";
            this.TextLeft = "IN";
        }
    }
}
