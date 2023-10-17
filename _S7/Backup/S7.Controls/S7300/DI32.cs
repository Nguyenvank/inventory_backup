using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace S7.Controls.S7300
{
    public class DI32 : S7300_32 
    {
        public DI32()
        {
            this.TextType = "DI32";
            this.TextLeft = "IN";
            this.TextRight = "IN";
        }
    }
}
