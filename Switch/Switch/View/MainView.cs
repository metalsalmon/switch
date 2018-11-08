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
            dg_statistiky.Rows.Add("rozhranie 1 in", 0, 0, 0, 0, 0, 0);
            dg_statistiky.Rows.Add("rozhranie 1 out", 0, 0, 0, 0, 0, 0);
            dg_statistiky.Rows.Add("rozhranie 2 in", 0, 0, 0, 0, 0, 0);
            dg_statistiky.Rows.Add("rozhranie 2 out", 0, 0, 0, 0, 0, 0);

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

        private void btn_reset_Click(object sender, EventArgs e)
        {
            lock(this){

            dg_statistiky.Rows.Clear();
            dg_statistiky.Rows.Add("rozhranie 1 in", 0, 0, 0, 0, 0, 0);
            dg_statistiky.Rows.Add("rozhranie 1 out", 0, 0, 0, 0, 0, 0);
            dg_statistiky.Rows.Add("rozhranie 2 in", 0, 0, 0, 0, 0, 0);
            dg_statistiky.Rows.Add("rozhranie 2 out", 0, 0, 0, 0, 0, 0);
            }

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
        public void analyzuj(Rozhranie rozhranie, int cislo_rozhrania, EthernetPacket eth, Packet paket)   //2
        {
            bool vloz = true;
            int vystupne_rozhranie = -1;
            Thread posielanie = null;
            Cam_zaznam novy_zaznam = new Cam_zaznam(eth.SourceHwAddress, cislo_rozhrania, timer);   //2
            statistiky(rozhranie, eth, true);

            lock (this)
            {

            foreach (var zaznam in CAM_tabulka)
            {
                if (zaznam.MAC.Equals(novy_zaznam.MAC))
                {
                    if (zaznam.rozhranie != novy_zaznam.rozhranie)
                    {
                        zaznam.rozhranie = novy_zaznam.rozhranie;
                    }
                    vloz = false;
                    zaznam.timer = timer;
                }
                if(zaznam.MAC == eth.DestinationHwAddress)
                {
                    vystupne_rozhranie = zaznam.rozhranie;  //2
                }

            }

            if (vloz)
            {
                CAM_tabulka.Add(novy_zaznam);
            }

            if(vystupne_rozhranie != cislo_rozhrania)
            {
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
                posielanie.Start();
            }
            
            }
        }

        public void statistiky(Rozhranie rozhranie, EthernetPacket eth, bool in_nout)
        {
            IPv4Packet ip;
            int riadok;

            lock (this)
            {

            if (rozhranie.cislo_rozhrania == 1 && in_nout) riadok = 0;
            else if (rozhranie.cislo_rozhrania == 1 && !in_nout) riadok = 1;
            else if (rozhranie.cislo_rozhrania == 2 && in_nout) riadok = 2;
            else riadok = 3;
                dg_statistiky[1, riadok].Value = (int)(dg_statistiky[1, riadok].Value) + 1;
                if (eth.PayloadPacket is ARPPacket) dg_statistiky[2, riadok].Value = (int)(dg_statistiky[2, riadok].Value) + 1;
                if(eth.PayloadPacket is IPv4Packet)
                {
                    dg_statistiky[3, riadok].Value = (int)(dg_statistiky[3, riadok].Value) + 1;
                    ip = (IPv4Packet)eth.PayloadPacket;
                    if (ip.PayloadPacket is TcpPacket)
                    {
                        dg_statistiky[4, riadok].Value = (int)(dg_statistiky[4, riadok].Value) + 1;
                   //   if (ip.PayloadPacket.PayloadPacket. is ) dg_statistiky[7, 0].Value = (int)(dg_statistiky[7, 0].Value) + 1;
                    }
                    if(ip.PayloadPacket is UdpPacket) dg_statistiky[5, riadok].Value = (int)(dg_statistiky[5, riadok].Value) + 1;
                    if(ip.PayloadPacket is ICMPv4Packet) dg_statistiky[6, riadok].Value = (int)(dg_statistiky[6, riadok].Value) + 1;
            }
            }
        }
         

        public void preposli(Rozhranie rozhranie, EthernetPacket eth)
        {
            statistiky(rozhranie, eth, false);
            rozhranie.adapter.SendPacket(eth);
            Thread.Sleep(500);
            Thread.CurrentThread.Abort();
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

        private void btn_reset_MAC_Click(object sender, EventArgs e)
        {
            lock (this)
            {
                foreach (var zaznam in CAM_tabulka.ToList())
                {
                    CAM_tabulka.Remove(zaznam);
                }
            }

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dg_statistiky_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void MainView_Load(object sender, EventArgs e)
        {

        }
    }
}
