using SharpPcap;

namespace Switch.Model
{
    public class Rozhranie
    {
        public ICaptureDevice adapter { get; set; }
        public int cislo_rozhrania { get; set; }
        public int eth_in { get; set; } = 0;
        public int eth_out { get; set; } = 0;
        public int arp_in { get; set; } = 0;
        public int arp_out { get; set; }= 0;
        public int ip_in { get; set; } = 0;
        public int ip_out { get; set; } = 0;
        public int tcp_in { get; set; } = 0;
        public int tcp_out { get; set; } = 0;
        public int udp_in { get; set; } = 0;
        public int udp_out { get; set; } = 0;
        public int icmp_in { get; set; } = 0;
        public int icmp_out { get; set; } = 0;
        public int http_in { get; set; } = 0;
        public int http_out { get; set; } = 0;

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

        public void vynuluj_statistiky()
        {
            cislo_rozhrania = eth_in = eth_out = arp_in = arp_out = ip_in = ip_out = tcp_in = tcp_out = udp_in = udp_out = icmp_in = icmp_out = http_in = http_out = 0;

        }
    }
}
