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
            otvor_na_prijimanie((WinPcapDevice)rozhranie.adapter);
        }

        public void pocuvaj_rozhranie2(Rozhranie rozhranie)
        {
            rozhranie.adapter.OnPacketArrival += new PacketArrivalEventHandler(zachytenie_rozhranie2);
            otvor_na_prijimanie((WinPcapDevice)rozhranie.adapter);

        }

        public void  otvor_na_prijimanie(WinPcapDevice device) {
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
            rozhranie = new Rozhranie(zoznam_adapterov[main_view.adapter1_index], cislo_rozhrania);
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
                        Console.WriteLine("omg");
                    }

                    if (rozhranie == 2)
                    {
                        analyzuj(rozhranie2, 2, eth, paket);
                        Console.WriteLine("kurva");
                    }
                }
            }
        }
        public void analyzuj(Rozhranie rozhranie, int cislo_rozhrania, EthernetPacket eth, Packet paket)
        {
            Thread posielanie;
           if (cislo_rozhrania == 1) posielanie = new Thread(() => preposli(rozhranie2, eth));
           else posielanie = new Thread(() => preposli(rozhranie1, eth));
            posielanie.Start();
        }

        public void preposli(Rozhranie rozhranie, EthernetPacket eth)
        {
            rozhranie.adapter.SendPacket(eth);
            System.Threading.Thread.Sleep(500);
            Thread.CurrentThread.Abort();
        }

    }
}
