using SharpPcap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Switch.Model
{
    class Rozhranie
    {
        public ICaptureDevice adapter { get; set; }
        public int cislo_rozhrania { get; set; }

        public Rozhranie(ICaptureDevice adapter, int cislo_rozhrania)
        {
            this.adapter = adapter;
            this.cislo_rozhrania = cislo_rozhrania;


        }

        public void nastav_adapter(ICaptureDevice adapter, int cislo_rozhrania)
        {
            this.adapter = adapter;
            this.cislo_rozhrania = cislo_rozhrania;
        }

    }
}
