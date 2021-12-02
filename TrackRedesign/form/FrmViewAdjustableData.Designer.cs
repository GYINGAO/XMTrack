namespace TrackRedesign {
    partial class FrmViewAdjustableData {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.dgvAdjustableData = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdjustableData)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvAdjustableData
            // 
            this.dgvAdjustableData.AllowUserToAddRows = false;
            this.dgvAdjustableData.AllowUserToDeleteRows = false;
            this.dgvAdjustableData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAdjustableData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAdjustableData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAdjustableData.Location = new System.Drawing.Point(0, 0);
            this.dgvAdjustableData.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dgvAdjustableData.Name = "dgvAdjustableData";
            this.dgvAdjustableData.RowHeadersWidth = 51;
            this.dgvAdjustableData.RowTemplate.Height = 27;
            this.dgvAdjustableData.Size = new System.Drawing.Size(669, 414);
            this.dgvAdjustableData.TabIndex = 0;
            this.dgvAdjustableData.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            // 
            // FrmViewAdjustableData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 414);
            this.Controls.Add(this.dgvAdjustableData);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "FrmViewAdjustableData";
            this.Text = "查看可调量数据";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmViewAdjustableData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdjustableData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvAdjustableData;
    }
}