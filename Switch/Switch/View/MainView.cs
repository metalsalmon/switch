using PacketDotNet;
using SharpPcap;
using SharpPcap.AirPcap;
using SharpPcap.LibPcap;
using SharpPcap.WinPcap;
using Switch.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Switch
{
    public partial class MainView : Form
    {
        private Rozhranie rozhranie1;
        private Rozhranie rozhranie2;
        List<ICaptureDevice> zoznam_adapterov;
        private CaptureDeviceList adaptery;
        private Thread vlakno_rozhranie1 = null, vlakno_rozhranie2 = null;
        private BindingList<Cam_zaznam> CAM_tabulka;
        private int timer = 30;

        public MainView()
        {
            CAM_tabulka = new BindingList<Cam_zaznam>();
            AllocConsole();
            InitializeComponent();
            adaptery = CaptureDeviceList.Instance;
            zoznam_adapterov = new List<ICaptureDevice>();
            zobraz_sietove_adaptery();
            Dg_CAM.DataSource = CAM_tabulka;
            
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        private void btn_rozhrania_Click(object sender, EventArgs e)
        {
            if (cb_rozhranie1.SelectedIndex >= 0 && cb_rozhranie2.SelectedIndex >= 0)
            {
                rozhranie1 = nastav_adapter(rozhranie1, 1);
                rozhranie2 = nastav_adapter(rozhranie2,2);
                try
                {
                    if (vlakno_rozhranie1 == null)
                    {
                        vlakno_rozhranie1 = new Thread(() => pocuvaj_rozhranie1(rozhranie1));
                        vlakno_rozhranie1.Start();
                    }

                    if (vlakno_rozhranie2 == null)
                    {
                        vlakno_rozhranie2 = new Thread(() => pocuvaj_rozhranie2(rozhranie2));
                        vlakno_rozhranie2.Start();
                    }

                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.ToString());
                }
            }
            else
            {
                MessageBox.Show("zvol adatper!");
            }

        }

        private void btn_statistiky_Click(object sender, EventArgs e)
        {
            vypis_statistiky(rozhranie1,1);
            vypis_statistiky(rozhranie2,2);
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            vymaz_statistiky();
        }

        private void MainView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (vlakno_rozhranie1 != null)
            {
                vlakno_rozhranie1.Join();
            }
            if (vlakno_rozhranie2 != null)
            {
                vlakno_rozhranie2.Join();
            }
        }

        public void zobraz_sietove_adaptery()
        {
            foreach (ICaptureDevice adapter in adaptery)
            {
                zoznam_adapterov.Add(adapter);
                cb_rozhranie1.Items.Add(adapter.Description);
                cb_rozhranie2.Items.Add(adapter.Description);
            }
        }


        public void pocuvaj_rozhranie1(Rozhranie rozhranie)
        {
            rozhranie.adapter.OnPacketArrival += new PacketArrivalEventHandler(zachytenie_rozhranie1);
            otvor_na_prijimanie(rozhranie.adapter);
        }

        public void pocuvaj_rozhranie2(Rozhranie rozhranie)
        {
            rozhranie.adapter.OnPacketArrival += new PacketArrivalEventHandler(zachytenie_rozhranie2);
            otvor_na_prijimanie(rozhranie.adapter);

        }

        public void otvor_na_prijimanie(ICaptureDevice device)
        {
            int timeout = 1000;

            if (device is AirPcapDevice)
            {
                ((AirPcapDevice)device).Open(OpenFlags.Promiscuous, timeout);
            }
            else if (device is WinPcapDevice)
            {
                ((WinPcapDevice)device).Open(OpenFlags.Promiscuous | OpenFlags.NoCaptureLocal, timeout);
            }
            else if (device is LibPcapLiveDevice)
            {
                ((LibPcapLiveDevice)device).Open(DeviceMode.Promiscuous, timeout);
            }
            device.StartCapture();
        }

        public void zachytenie_rozhranie1(object sender, CaptureEventArgs e)
        {
            Packet paket = Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);
            zachytenie(paket, 1);

        }
        public void zachytenie_rozhranie2(object sender, CaptureEventArgs e)
        {
            Packet paket = Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);
            zachytenie(paket, 2);
        }
        public Rozhranie nastav_adapter(Rozhranie rozhranie, int cislo_rozhrania)
        {
            if (cislo_rozhrania == 1) rozhranie = new Rozhranie(zoznam_adapterov[cb_rozhranie1.SelectedIndex], cislo_rozhrania);
            if (cislo_rozhrania == 2) rozhranie = new Rozhranie(zoznam_adapterov[cb_rozhranie2.SelectedIndex], cislo_rozhrania);
            return rozhranie;
        }


        public void zachytenie(Packet paket, int rozhranie)
        {
            if (paket is EthernetPacket)
            {
                EthernetPacket eth = (EthernetPacket)paket;

                if (rozhranie1 != null && rozhranie2 != null)
                {
                    if (rozhranie == 1)
                    {
                        analyzuj(rozhranie1, 1, eth, paket);
                    }

                    if (rozhranie == 2)
                    {
                        analyzuj(rozhranie2, 2, eth, paket);
                    }
                }
            }

        }
        public void analyzuj(Rozhranie rozhranie, int cislo_rozhrania, EthernetPacket eth, Packet paket)
        {
            bool vloz = true;
            int vystupne_rozhranie = -1;
            Thread posielanie;
            Cam_zaznam novy_zaznam = new Cam_zaznam(eth.SourceHwAddress, cislo_rozhrania, timer);
            statistiky(rozhranie, eth, true);
        

            foreach (var zaznam in CAM_tabulka)
            {
                if (zaznam.MAC.Equals(novy_zaznam.MAC) && zaznam.rozhranie.Equals(novy_zaznam.rozhranie))
                {
                    vloz = false;
                    zaznam.timer = timer;
                }
                if(zaznam.MAC == eth.DestinationHwAddress)
                {
                    vystupne_rozhranie = zaznam.rozhranie;
                }

            }

            if (vloz)
            {
                CAM_tabulka.Add(novy_zaznam);
            }

            switch (vystupne_rozhranie)
            {
                case 1:
                    posielanie = new Thread(() => preposli(rozhranie1, eth));
                    break;
                case 2:
                    posielanie = new Thread(() => preposli(rozhranie2, eth));
                    break;
                default:
                    if(cislo_rozhrania == 1) posielanie = new Thread(() => preposli(rozhranie2, eth));
                    else posielanie = new Thread(() => preposli(rozhranie1, eth));
                    break;
            }
            Console.WriteLine(CAM_tabulka.Count);
            posielanie.Start();
        }

        public void statistiky(Rozhranie rozhranie, EthernetPacket eth, bool in_nout)
        {
            //   if(eth.PayloadPacket ==  )
            if (in_nout)
            {
                rozhranie.eth_in++;

                if (eth.Type.ToString().Equals("Arp")) rozhranie.arp_in++;
                if (eth.Type.ToString().Equals("IpV4"))
                {
                    rozhranie.ip_in++;
                }
            }
            else
            {
                rozhranie.eth_out++;
                if (eth.Type.ToString().Equals("Arp")) rozhranie.arp_out++;
                if (eth.Type.ToString().Equals("IpV4"))
                {
                    rozhranie.ip_out++;
                }
            }
        }

        public void preposli(Rozhranie rozhranie, EthernetPacket eth)
        {
            Console.WriteLine("joooooooooooooooooooooooooooooooooooooooooooj");
            statistiky(rozhranie, eth, false);
            rozhranie.adapter.SendPacket(eth);
            Thread.Sleep(500);
            Thread.CurrentThread.Abort();
        }

        public void vypis_statistiky(Rozhranie rozhranie, int cislo)
        {
            Console.WriteLine("rozhranie " + cislo);
            Console.WriteLine("ethernet in: " + rozhranie.eth_in);
            Console.WriteLine("ethernet out: " + rozhranie.eth_out);
            Console.WriteLine("arp in: " + rozhranie.arp_in);
            Console.WriteLine("arp out: " + rozhranie.arp_out);
            Console.WriteLine("IPv4 in: " + rozhranie.ip_in);
            Console.WriteLine("IPv4 out: " + rozhranie.ip_out);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (rozhranie1 != null && rozhranie2 != null)
            {
                lock (this) {
                    foreach (var zaznam in CAM_tabulka.ToList())
                    {
                        if (zaznam.timer-- == 0)
                        {
                            CAM_tabulka.Remove(zaznam);
                        }
                    }
                }
            }
        }

        private void btn_casovac_Click(object sender, EventArgs e)
        {
            timer = Convert.ToInt32(txt_timer.Value);
        }

        public void vymaz_statistiky()
        {
            rozhranie1.vynuluj_statistiky();
            rozhranie2.vynuluj_statistiky();
        }
    }
}
