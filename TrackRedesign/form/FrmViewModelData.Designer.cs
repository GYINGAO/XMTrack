namespace TrackRedesign {
    partial class FrmViewModelData {
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
            this.dgvModelData = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvModelData)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvModelData
            // 
            this.dgvModelData.AllowUserToAddRows = false;
            this.dgvModelData.AllowUserToDeleteRows = false;
            this.dgvModelData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvModelData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvModelData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvModelData.Location = new System.Drawing.Point(0, 0);
            this.dgvModelData.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dgvModelData.Name = "dgvModelData";
            this.dgvModelData.RowHeadersWidth = 51;
            this.dgvModelData.RowTemplate.Height = 27;
            this.dgvModelData.Size = new System.Drawing.Size(986, 633);
            this.dgvModelData.TabIndex = 0;
            this.dgvModelData.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvModelData_RowPostPaint);
            // 
            // FrmViewModelData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(986, 633);
            this.Controls.Add(this.dgvModelData);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "FrmViewModelData";
            this.Text = "查看型号数据";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmViewModelData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvModelData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvModelData;
    }
}