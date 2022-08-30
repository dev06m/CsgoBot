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
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.itemleriGetir = new System.Windows.Forms.Button();
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
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
            this.dataGridView1.Location = new System.Drawing.Point(59, 188);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 29;
            this.dataGridView1.Size = new System.Drawing.Size(937, 340);
            this.dataGridView1.TabIndex = 5;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Column1";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            this.Column1.Width = 125;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(433, 39);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(143, 39);
            this.button3.TabIndex = 10;
            this.button3.Text = "Fiyat Ayarla";
            this.button3.UseVisualStyleBackColor = true;
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
            // itemleriGetir
            // 
            this.itemleriGetir.AllowDrop = true;
            this.itemleriGetir.Location = new System.Drawing.Point(433, 84);
            this.itemleriGetir.Name = "itemleriGetir";
            this.itemleriGetir.Size = new System.Drawing.Size(143, 50);
            this.itemleriGetir.TabIndex = 7;
            this.itemleriGetir.Text = "Itemleri Getir (Envanter)";
            this.itemleriGetir.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1079, 593);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.itemleriGetir);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.GetInventoryItems);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button GetInventoryItems;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn Column1;
        private Button button3;
        private TextBox textBox2;
        private TextBox textBox1;
        private Button itemleriGetir;
    }
}