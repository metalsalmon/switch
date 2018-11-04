namespace Switch
{
    partial class MainView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btn_rozhrania = new System.Windows.Forms.Button();
            this.cb_rozhranie1 = new System.Windows.Forms.ComboBox();
            this.cb_rozhranie2 = new System.Windows.Forms.ComboBox();
            this.btn_statistiky = new System.Windows.Forms.Button();
            this.btn_reset = new System.Windows.Forms.Button();
            this.Dg_CAM = new System.Windows.Forms.DataGridView();
            this.btn_casovac = new System.Windows.Forms.Button();
            this.txt_timer = new System.Windows.Forms.NumericUpDown();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.Dg_CAM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_timer)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_rozhrania
            // 
            this.btn_rozhrania.Location = new System.Drawing.Point(417, 58);
            this.btn_rozhrania.Name = "btn_rozhrania";
            this.btn_rozhrania.Size = new System.Drawing.Size(75, 23);
            this.btn_rozhrania.TabIndex = 0;
            this.btn_rozhrania.Text = "nastav";
            this.btn_rozhrania.UseVisualStyleBackColor = true;
            this.btn_rozhrania.Click += new System.EventHandler(this.btn_rozhrania_Click);
            // 
            // cb_rozhranie1
            // 
            this.cb_rozhranie1.FormattingEnabled = true;
            this.cb_rozhranie1.Location = new System.Drawing.Point(45, 33);
            this.cb_rozhranie1.Name = "cb_rozhranie1";
            this.cb_rozhranie1.Size = new System.Drawing.Size(353, 24);
            this.cb_rozhranie1.TabIndex = 1;
            // 
            // cb_rozhranie2
            // 
            this.cb_rozhranie2.FormattingEnabled = true;
            this.cb_rozhranie2.Location = new System.Drawing.Point(45, 73);
            this.cb_rozhranie2.Name = "cb_rozhranie2";
            this.cb_rozhranie2.Size = new System.Drawing.Size(353, 24);
            this.cb_rozhranie2.TabIndex = 2;
            // 
            // btn_statistiky
            // 
            this.btn_statistiky.Location = new System.Drawing.Point(454, 176);
            this.btn_statistiky.Name = "btn_statistiky";
            this.btn_statistiky.Size = new System.Drawing.Size(75, 23);
            this.btn_statistiky.TabIndex = 3;
            this.btn_statistiky.Text = "statistiky";
            this.btn_statistiky.UseVisualStyleBackColor = true;
            this.btn_statistiky.Click += new System.EventHandler(this.btn_statistiky_Click);
            // 
            // btn_reset
            // 
            this.btn_reset.Location = new System.Drawing.Point(554, 193);
            this.btn_reset.Name = "btn_reset";
            this.btn_reset.Size = new System.Drawing.Size(75, 23);
            this.btn_reset.TabIndex = 4;
            this.btn_reset.Text = "reset";
            this.btn_reset.UseVisualStyleBackColor = true;
            this.btn_reset.Click += new System.EventHandler(this.btn_reset_Click);
            // 
            // Dg_CAM
            // 
            this.Dg_CAM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Dg_CAM.Location = new System.Drawing.Point(101, 205);
            this.Dg_CAM.Name = "Dg_CAM";
            this.Dg_CAM.RowHeadersVisible = false;
            this.Dg_CAM.RowTemplate.Height = 24;
            this.Dg_CAM.Size = new System.Drawing.Size(428, 150);
            this.Dg_CAM.TabIndex = 5;
            // 
            // btn_casovac
            // 
            this.btn_casovac.Location = new System.Drawing.Point(690, 257);
            this.btn_casovac.Name = "btn_casovac";
            this.btn_casovac.Size = new System.Drawing.Size(75, 23);
            this.btn_casovac.TabIndex = 6;
            this.btn_casovac.Text = "nastav";
            this.btn_casovac.UseVisualStyleBackColor = true;
            this.btn_casovac.Click += new System.EventHandler(this.btn_casovac_Click);
            // 
            // txt_timer
            // 
            this.txt_timer.Location = new System.Drawing.Point(564, 258);
            this.txt_timer.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txt_timer.Name = "txt_timer";
            this.txt_timer.Size = new System.Drawing.Size(120, 22);
            this.txt_timer.TabIndex = 7;
            this.txt_timer.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txt_timer);
            this.Controls.Add(this.btn_casovac);
            this.Controls.Add(this.Dg_CAM);
            this.Controls.Add(this.btn_reset);
            this.Controls.Add(this.btn_statistiky);
            this.Controls.Add(this.cb_rozhranie2);
            this.Controls.Add(this.cb_rozhranie1);
            this.Controls.Add(this.btn_rozhrania);
            this.Name = "MainView";
            this.Text = "Switch";
            ((System.ComponentModel.ISupportInitialize)(this.Dg_CAM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_timer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_rozhrania;
        private System.Windows.Forms.ComboBox cb_rozhranie1;
        private System.Windows.Forms.ComboBox cb_rozhranie2;
        private System.Windows.Forms.Button btn_statistiky;
        private System.Windows.Forms.Button btn_reset;
        private System.Windows.Forms.DataGridView Dg_CAM;
        private System.Windows.Forms.Button btn_casovac;
        private System.Windows.Forms.NumericUpDown txt_timer;
        private System.Windows.Forms.Timer timer1;
    }
}

