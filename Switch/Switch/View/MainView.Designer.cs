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
            this.btn_rozhrania = new System.Windows.Forms.Button();
            this.cb_rozhranie1 = new System.Windows.Forms.ComboBox();
            this.cb_rozhranie2 = new System.Windows.Forms.ComboBox();
            this.btn_statistiky = new System.Windows.Forms.Button();
            this.btn_reset = new System.Windows.Forms.Button();
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
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_reset);
            this.Controls.Add(this.btn_statistiky);
            this.Controls.Add(this.cb_rozhranie2);
            this.Controls.Add(this.cb_rozhranie1);
            this.Controls.Add(this.btn_rozhrania);
            this.Name = "MainView";
            this.Text = "Switch";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_rozhrania;
        private System.Windows.Forms.ComboBox cb_rozhranie1;
        private System.Windows.Forms.ComboBox cb_rozhranie2;
        private System.Windows.Forms.Button btn_statistiky;
        private System.Windows.Forms.Button btn_reset;
    }
}

