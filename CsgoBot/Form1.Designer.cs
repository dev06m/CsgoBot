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
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // GetInventoryItems
            // 
            this.GetInventoryItems.Location = new System.Drawing.Point(59, 22);
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
            this.dataGridView1.Location = new System.Drawing.Point(59, 83);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 29;
            this.dataGridView1.Size = new System.Drawing.Size(1296, 580);
            this.dataGridView1.TabIndex = 5;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.CancelItem);
            // 
            // SatisListesiBtn
            // 
            this.SatisListesiBtn.Location = new System.Drawing.Point(229, 22);
            this.SatisListesiBtn.Name = "SatisListesiBtn";
            this.SatisListesiBtn.Size = new System.Drawing.Size(146, 55);
            this.SatisListesiBtn.TabIndex = 12;
            this.SatisListesiBtn.Text = "Satış Listesi";
            this.SatisListesiBtn.UseVisualStyleBackColor = true;
            this.SatisListesiBtn.Click += new System.EventHandler(this.SatisListesiButton);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(607, 36);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 29);
            this.button1.TabIndex = 13;
            this.button1.Text = "Fetch items";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1482, 753);
            this.Controls.Add(this.button1);
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
        private Button button1;
    }
}