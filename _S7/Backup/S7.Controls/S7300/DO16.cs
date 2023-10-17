using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace S7.Controls.S7300
{
    public class DO16 : S7300_16 
    {
        public DO16()
        {
            this.TextType = "DO16";
            this.TextLeft = "IN";
        }
    }
}
