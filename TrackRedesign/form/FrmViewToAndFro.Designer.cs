namespace TrackRedesign {
    partial class FrmViewToAndFro {
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabpToDate = new System.Windows.Forms.TabPage();
            this.dgvToDate = new System.Windows.Forms.DataGridView();
            this.tabpFroDate = new System.Windows.Forms.TabPage();
            this.dgvFroDate = new System.Windows.Forms.DataGridView();
            this.tabControl1.SuspendLayout();
            this.tabpToDate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvToDate)).BeginInit();
            this.tabpFroDate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFroDate)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl1.Controls.Add(this.tabpToDate);
            this.tabControl1.Controls.Add(this.tabpFroDate);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(686, 451);
            this.tabControl1.TabIndex = 0;
            // 
            // tabpToDate
            // 
            this.tabpToDate.Controls.Add(this.dgvToDate);
            this.tabpToDate.Location = new System.Drawing.Point(4, 4);
            this.tabpToDate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabpToDate.Name = "tabpToDate";
            this.tabpToDate.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabpToDate.Size = new System.Drawing.Size(678, 425);
            this.tabpToDate.TabIndex = 0;
            this.tabpToDate.Text = "往测";
            this.tabpToDate.UseVisualStyleBackColor = true;
            // 
            // dgvToDate
            // 
            this.dgvToDate.AllowUserToAddRows = false;
            this.dgvToDate.AllowUserToDeleteRows = false;
            this.dgvToDate.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvToDate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvToDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvToDate.Location = new System.Drawing.Point(2, 2);
            this.dgvToDate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dgvToDate.Name = "dgvToDate";
            this.dgvToDate.RowHeadersWidth = 51;
            this.dgvToDate.RowTemplate.Height = 27;
            this.dgvToDate.Size = new System.Drawing.Size(674, 421);
            this.dgvToDate.TabIndex = 0;
            this.dgvToDate.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvToDate_RowPostPaint);
            // 
            // tabpFroDate
            // 
            this.tabpFroDate.Controls.Add(this.dgvFroDate);
            this.tabpFroDate.Location = new System.Drawing.Point(4, 4);
            this.tabpFroDate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabpFroDate.Name = "tabpFroDate";
            this.tabpFroDate.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabpFroDate.Size = new System.Drawing.Size(678, 425);
            this.tabpFroDate.TabIndex = 1;
            this.tabpFroDate.Text = "返测";
            this.tabpFroDate.UseVisualStyleBackColor = true;
            // 
            // dgvFroDate
            // 
            this.dgvFroDate.AllowUserToAddRows = false;
            this.dgvFroDate.AllowUserToDeleteRows = false;
            this.dgvFroDate.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvFroDate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFroDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFroDate.Location = new System.Drawing.Point(2, 2);
            this.dgvFroDate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dgvFroDate.Name = "dgvFroDate";
            this.dgvFroDate.RowHeadersWidth = 51;
            this.dgvFroDate.RowTemplate.Height = 27;
            this.dgvFroDate.Size = new System.Drawing.Size(674, 421);
            this.dgvFroDate.TabIndex = 1;
            this.dgvFroDate.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvFroDate_RowPostPaint);
            // 
            // FrmViewToAndFro
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 451);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "FrmViewToAndFro";
            this.Text = "查看往返测数据";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmViewToAndFro_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabpToDate.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvToDate)).EndInit();
            this.tabpFroDate.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFroDate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabpToDate;
        private System.Windows.Forms.DataGridView dgvToDate;
        private System.Windows.Forms.TabPage tabpFroDate;
        private System.Windows.Forms.DataGridView dgvFroDate;
    }
}