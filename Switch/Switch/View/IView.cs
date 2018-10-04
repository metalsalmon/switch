using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Switch.View
{
    interface IView
    {
        string adapter1 { get; set; }
        int adapter1_index { get; set; }
        string adapter2 { get; set; }
        int adapter2_index { get; set; }
    }
}
