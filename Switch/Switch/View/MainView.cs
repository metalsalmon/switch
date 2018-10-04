using Switch.Presenter;
using Switch.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Switch
{
    public partial class MainView : Form, IView
    {
        private MainController presenter;
        internal MainController Presenter { get => presenter; set => presenter = value; }
        public string adapter1 { get =>cb_rozhranie1.SelectedItem.ToString(); set => cb_rozhranie1.Items.Add(value); }
        public int adapter1_index { get => cb_rozhranie1.SelectedIndex; set => throw new NotImplementedException(); }
        public string adapter2 { get => cb_rozhranie2.SelectedItem.ToString(); set => cb_rozhranie2.Items.Add(value); }
        public int adapter2_index { get => cb_rozhranie2.SelectedIndex; set => throw new NotImplementedException(); }

        private Thread vlakno_rozhranie1 = null, vlakno_rozhranie2 = null;

        public MainView()
        {
            AllocConsole();
            InitializeComponent();
            presenter = new MainController(this);
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        private void btn_rozhrania_Click(object sender, EventArgs e)
        {
            if (adapter1_index >= 0 && adapter2_index >= 0)
            {
                presenter.rozhranie1 = presenter.nastav_adapter(presenter.rozhranie1, 1);
                presenter.rozhranie2 = presenter.nastav_adapter(presenter.rozhranie2,2);
                try
                {
                    if (vlakno_rozhranie1 == null)
                    {
                        vlakno_rozhranie1 = new Thread(() => presenter.pocuvaj_rozhranie1(presenter.rozhranie1));
                        vlakno_rozhranie1.Start();
                    }

                    if (vlakno_rozhranie2 == null)
                    {
                        vlakno_rozhranie2 = new Thread(() => presenter.pocuvaj_rozhranie2(presenter.rozhranie2));
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
            presenter.vypis_statistiky(presenter.rozhranie1,1);
            presenter.vypis_statistiky(presenter.rozhranie2,2);
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            presenter.vymaz_statistiky();
        }

        private void MainView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (vlakno_rozhranie1 != null)
            {
                presenter.zastav_vlakno = true;
                vlakno_rozhranie1.Join();
            }
            if (vlakno_rozhranie2 != null)
            {
                presenter.zastav_vlakno = true;
                vlakno_rozhranie2.Join();
            }
        }
    }
}
