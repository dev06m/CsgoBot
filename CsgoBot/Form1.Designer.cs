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
            this.SatisListesiBtn = new System.Windows.Forms.Button();
            this.baslat = new System.Windows.Forms.Button();
            this.makeOffer = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // GetInventoryItems
            // 
            this.GetInventoryItems.Location = new System.Drawing.Point(59, 49);
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
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // SatisListesiBtn
            // 
            this.SatisListesiBtn.Location = new System.Drawing.Point(59, 126);
            this.SatisListesiBtn.Name = "SatisListesiBtn";
            this.SatisListesiBtn.Size = new System.Drawing.Size(137, 42);
            this.SatisListesiBtn.TabIndex = 12;
            this.SatisListesiBtn.Text = "Satış Listesi";
            this.SatisListesiBtn.UseVisualStyleBackColor = true;
            this.SatisListesiBtn.Click += new System.EventHandler(this.SatisListesiButton);
            // 
            // baslat
            // 
            this.baslat.Location = new System.Drawing.Point(241, 49);
            this.baslat.Name = "baslat";
            this.baslat.Size = new System.Drawing.Size(120, 119);
            this.baslat.TabIndex = 18;
            this.baslat.Text = "Başlat";
            this.baslat.UseVisualStyleBackColor = true;
            this.baslat.Click += new System.EventHandler(this.baslat_click);
            // 
            // makeOffer
            // 
            this.makeOffer.Location = new System.Drawing.Point(572, 94);
            this.makeOffer.Name = "makeOffer";
            this.makeOffer.Size = new System.Drawing.Size(117, 45);
            this.makeOffer.TabIndex = 19;
            this.makeOffer.Text = "Make Offer";
            this.makeOffer.UseVisualStyleBackColor = true;
            this.makeOffer.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1482, 753);
            this.Controls.Add(this.makeOffer);
            this.Controls.Add(this.baslat);
            this.Controls.Add(this.SatisListesiBtn);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.GetInventoryItems);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Button GetInventoryItems;
        private DataGridView dataGridView1;
        private Button SatisListesiBtn;
        private TextBox satisIdText;
        private Button button2;
        private Button baslat;
        private Button makeOffer;
    }
}