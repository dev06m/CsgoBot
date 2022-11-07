namespace CsgoBot
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.GetInventoryItems = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.FiyatAyarla = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.itemlerinFiyatiniGetir = new System.Windows.Forms.Button();
            this.CancelOfferBtn = new System.Windows.Forms.Button();
            this.SatisListesiBtn = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.satisIdText = new System.Windows.Forms.TextBox();
            this.hileTextBox = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.cancelItemBox = new System.Windows.Forms.TextBox();
            this.baslat = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // GetInventoryItems
            // 
            this.GetInventoryItems.Location = new System.Drawing.Point(859, 39);
            this.GetInventoryItems.Name = "GetInventoryItems";
            this.GetInventoryItems.Size = new System.Drawing.Size(137, 55);
            this.GetInventoryItems.TabIndex = 3;
            this.GetInventoryItems.Text = "Envanteri Goster (Itemler)";
            this.GetInventoryItems.UseVisualStyleBackColor = true;
            this.GetInventoryItems.Click += new System.EventHandler(this.GetInventoryItems_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(59, 188);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 29;
            this.dataGridView1.Size = new System.Drawing.Size(937, 340);
            this.dataGridView1.TabIndex = 5;
            // 
            // FiyatAyarla
            // 
            this.FiyatAyarla.Location = new System.Drawing.Point(433, 39);
            this.FiyatAyarla.Name = "FiyatAyarla";
            this.FiyatAyarla.Size = new System.Drawing.Size(143, 39);
            this.FiyatAyarla.TabIndex = 10;
            this.FiyatAyarla.Text = "Fiyat Ayarla";
            this.FiyatAyarla.UseVisualStyleBackColor = true;
            this.FiyatAyarla.Click += new System.EventHandler(this.FiyatAyarlaButton);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(59, 39);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(368, 27);
            this.textBox2.TabIndex = 9;
            this.textBox2.Text = "Satis Fiyati";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(59, 93);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(368, 27);
            this.textBox1.TabIndex = 8;
            this.textBox1.Text = "Item Adi Giriniz";
            // 
            // itemlerinFiyatiniGetir
            // 
            this.itemlerinFiyatiniGetir.AllowDrop = true;
            this.itemlerinFiyatiniGetir.Location = new System.Drawing.Point(433, 84);
            this.itemlerinFiyatiniGetir.Name = "itemlerinFiyatiniGetir";
            this.itemlerinFiyatiniGetir.Size = new System.Drawing.Size(143, 50);
            this.itemlerinFiyatiniGetir.TabIndex = 7;
            this.itemlerinFiyatiniGetir.Text = "Itemlerin Fiyatlarini Getir";
            this.itemlerinFiyatiniGetir.UseVisualStyleBackColor = true;
            this.itemlerinFiyatiniGetir.Click += new System.EventHandler(this.itemlerinFiyatiniGetirButton);
            // 
            // CancelOfferBtn
            // 
            this.CancelOfferBtn.Location = new System.Drawing.Point(713, 51);
            this.CancelOfferBtn.Name = "CancelOfferBtn";
            this.CancelOfferBtn.Size = new System.Drawing.Size(106, 43);
            this.CancelOfferBtn.TabIndex = 11;
            this.CancelOfferBtn.Text = "Cancel Offer";
            this.CancelOfferBtn.UseVisualStyleBackColor = true;
            this.CancelOfferBtn.Click += new System.EventHandler(this.CancelOfferButton);
            // 
            // SatisListesiBtn
            // 
            this.SatisListesiBtn.Location = new System.Drawing.Point(873, 119);
            this.SatisListesiBtn.Name = "SatisListesiBtn";
            this.SatisListesiBtn.Size = new System.Drawing.Size(113, 42);
            this.SatisListesiBtn.TabIndex = 12;
            this.SatisListesiBtn.Text = "Satış Listesi";
            this.SatisListesiBtn.UseVisualStyleBackColor = true;
            this.SatisListesiBtn.Click += new System.EventHandler(this.SatisListesiButton);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(713, 126);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 29);
            this.button1.TabIndex = 13;
            this.button1.Text = "Hile";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // satisIdText
            // 
            this.satisIdText.Location = new System.Drawing.Point(157, 142);
            this.satisIdText.Name = "satisIdText";
            this.satisIdText.Size = new System.Drawing.Size(125, 27);
            this.satisIdText.TabIndex = 14;
            this.satisIdText.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // hileTextBox
            // 
            this.hileTextBox.Location = new System.Drawing.Point(592, 128);
            this.hileTextBox.Name = "hileTextBox";
            this.hileTextBox.Size = new System.Drawing.Size(125, 27);
            this.hileTextBox.TabIndex = 15;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(59, 128);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(92, 54);
            this.button2.TabIndex = 16;
            this.button2.Text = "Satis ID Getir";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.satisIdGetir);
            // 
            // cancelItemBox
            // 
            this.cancelItemBox.Location = new System.Drawing.Point(592, 59);
            this.cancelItemBox.Name = "cancelItemBox";
            this.cancelItemBox.Size = new System.Drawing.Size(125, 27);
            this.cancelItemBox.TabIndex = 17;
            // 
            // baslat
            // 
            this.baslat.Location = new System.Drawing.Point(1067, 49);
            this.baslat.Name = "baslat";
            this.baslat.Size = new System.Drawing.Size(108, 106);
            this.baslat.TabIndex = 18;
            this.baslat.Text = "Başlat";
            this.baslat.UseVisualStyleBackColor = true;
            this.baslat.Click += new System.EventHandler(this.baslat_click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1482, 753);
            this.Controls.Add(this.baslat);
            this.Controls.Add(this.cancelItemBox);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.hileTextBox);
            this.Controls.Add(this.satisIdText);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.SatisListesiBtn);
            this.Controls.Add(this.CancelOfferBtn);
            this.Controls.Add(this.FiyatAyarla);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.itemlerinFiyatiniGetir);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.GetInventoryItems);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button GetInventoryItems;
        private DataGridView dataGridView1;
        private Button FiyatAyarla;
        private TextBox textBox2;
        private TextBox textBox1;
        private Button itemlerinFiyatiniGetir;
        private Button CancelOfferBtn;
        private Button SatisListesiBtn;
        private Button button1;
        private TextBox satisIdText;
        private TextBox hileTextBox;
        private Button button2;
        private TextBox cancelItemBox;
        private Button baslat;
    }
}