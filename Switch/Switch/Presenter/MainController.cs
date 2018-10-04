using PacketDotNet;
using SharpPcap;
using SharpPcap.AirPcap;
using SharpPcap.LibPcap;
using SharpPcap.WinPcap;
using Switch.Model;
using Switch.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Switch.Presenter
{

    class MainController
    {
        IView main_view;
        public Rozhranie rozhranie1 { get; set; }
        public Rozhranie rozhranie2 { get; set; }
        List<ICaptureDevice> zoznam_adapterov;
        public CaptureDeviceList adaptery { get; set; }
        public bool zastav_vlakno { get; set; }

        public MainController(IView view)
        {
            main_view = view;
            adaptery = CaptureDeviceList.Instance;
            zoznam_adapterov = new List<ICaptureDevice>();
            zobraz_sietove_adaptery();
        }

        public void zobraz_sietove_adaptery()
        {
            foreach (ICaptureDevice adapter in adaptery)
            {
                zoznam_adapterov.Add(adapter);
                main_view.adapter1 = adapter.Description;
                main_view.adapter2 = adapter.Description;
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

        public void  otvor_na_prijimanie(ICaptureDevice device) {
            int timeout = 1000;

            if (device is AirPcapDevice)
            {
                ((AirPcapDevice)device).Open(OpenFlags.Promiscuous,timeout);
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
           if(cislo_rozhrania == 1) rozhranie = new Rozhranie(zoznam_adapterov[main_view.adapter1_index], cislo_rozhrania);
           if(cislo_rozhrania == 2) rozhranie = new Rozhranie(zoznam_adapterov[main_view.adapter2_index], cislo_rozhrania);
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
            statistiky(rozhranie, eth, true);

            Thread posielanie;
            if (cislo_rozhrania == 1)
            {
                posielanie = new Thread(() => preposli(rozhranie2, eth));
            }

            else posielanie = new Thread(() => preposli(rozhranie1, eth));
            Console.WriteLine(cislo_rozhrania);
            posielanie.Start();
        }

        public void statistiky(Rozhranie rozhranie, EthernetPacket eth,bool in_nout)
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
            statistiky(rozhranie, eth, false);
            rozhranie.adapter.SendPacket(eth);
            System.Threading.Thread.Sleep(500);
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

        public void vymaz_statistiky()
        {
            rozhranie1.vynuluj_statistiky();
            rozhranie2.vynuluj_statistiky();
        }

    }
}
