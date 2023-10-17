using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace S7.Controls.S7300
{
    public class DO32 : S7300_32 
    {
        public DO32()
        {
            this.TextType = "DO32";
            this.TextLeft = "OUT";
            this.TextRight = "OUT";
        }
    }
}
