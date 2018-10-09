namespace SignatureApp
{
    partial class SignatureDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SignatureDialog));
            this.tblInvoice = new System.Windows.Forms.DataGridView();
            this.pdf = new AxAcroPDFLib.AxAcroPDF();
            this.btnSign = new System.Windows.Forms.Button();
            this.txtId = new System.Windows.Forms.TextBox();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnLogout = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnReload = new System.Windows.Forms.Button();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Customer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Enterprise = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TemplateForm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TemplateSerial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileUrl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LookupCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.tblInvoice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pdf)).BeginInit();
            this.SuspendLayout();
            // 
            // tblInvoice
            // 
            this.tblInvoice.AllowUserToAddRows = false;
            this.tblInvoice.AllowUserToDeleteRows = false;
            this.tblInvoice.AllowUserToResizeColumns = false;
            this.tblInvoice.AllowUserToResizeRows = false;
            this.tblInvoice.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tblInvoice.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.Customer,
            this.Enterprise,
            this.TemplateForm,
            this.TemplateSerial,
            this.Number,
            this.FileUrl,
            this.LookupCode});
            this.tblInvoice.Location = new System.Drawing.Point(12, 33);
            this.tblInvoice.Name = "tblInvoice";
            this.tblInvoice.Size = new System.Drawing.Size(625, 485);
            this.tblInvoice.TabIndex = 0;
            this.tblInvoice.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.tblInvoice.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.tblInvoice_RowHeaderMouseClick);
            // 
            // pdf
            // 
            this.pdf.Enabled = true;
            this.pdf.Location = new System.Drawing.Point(643, 33);
            this.pdf.Name = "pdf";
            this.pdf.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("pdf.OcxState")));
            this.pdf.Size = new System.Drawing.Size(373, 485);
            this.pdf.TabIndex = 1;
            // 
            // btnSign
            // 
            this.btnSign.Location = new System.Drawing.Point(800, 521);
            this.btnSign.Name = "btnSign";
            this.btnSign.Size = new System.Drawing.Size(75, 23);
            this.btnSign.TabIndex = 2;
            this.btnSign.Text = "Ký";
            this.btnSign.UseVisualStyleBackColor = true;
            this.btnSign.Click += new System.EventHandler(this.btnSign_Click);
            // 
            // txtId
            // 
            this.txtId.Location = new System.Drawing.Point(665, 523);
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(100, 20);
            this.txtId.TabIndex = 3;
            this.txtId.Visible = false;
            this.txtId.TextChanged += new System.EventHandler(this.txtId_TextChanged);
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(895, 521);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(100, 20);
            this.txtPath.TabIndex = 4;
            this.txtPath.Visible = false;
            this.txtPath.TextChanged += new System.EventHandler(this.txtPath_TextChanged);
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(12, 524);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(82, 20);
            this.btnLogout.TabIndex = 5;
            this.btnLogout.Text = "Đăng xuất";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(225, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "Danh sách hóa đơn";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(797, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Chi tiết hóa đơn";
            // 
            // btnReload
            // 
            this.btnReload.Location = new System.Drawing.Point(12, 3);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(82, 24);
            this.btnReload.TabIndex = 8;
            this.btnReload.Text = "Tải hóa đơn";
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // Id
            // 
            this.Id.DataPropertyName = "Id";
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.Visible = false;
            // 
            // Customer
            // 
            this.Customer.DataPropertyName = "Name";
            this.Customer.HeaderText = "Khách hàng";
            this.Customer.Name = "Customer";
            this.Customer.Width = 150;
            // 
            // Enterprise
            // 
            this.Enterprise.DataPropertyName = "Enterprise";
            this.Enterprise.HeaderText = "Đơn vị";
            this.Enterprise.Name = "Enterprise";
            this.Enterprise.Width = 220;
            // 
            // TemplateForm
            // 
            this.TemplateForm.DataPropertyName = "TemplateForm";
            this.TemplateForm.HeaderText = "Mấu hóa đơn";
            this.TemplateForm.Name = "TemplateForm";
            this.TemplateForm.Width = 80;
            // 
            // TemplateSerial
            // 
            this.TemplateSerial.DataPropertyName = "TemplateSerial";
            this.TemplateSerial.HeaderText = "Kí hiệu";
            this.TemplateSerial.Name = "TemplateSerial";
            this.TemplateSerial.Width = 60;
            // 
            // Number
            // 
            this.Number.DataPropertyName = "Number";
            this.Number.HeaderText = "Số hóa đơn";
            this.Number.Name = "Number";
            this.Number.Width = 60;
            // 
            // FileUrl
            // 
            this.FileUrl.DataPropertyName = "FileUrl";
            this.FileUrl.HeaderText = "File";
            this.FileUrl.Name = "FileUrl";
            this.FileUrl.Visible = false;
            // 
            // LookupCode
            // 
            this.LookupCode.DataPropertyName = "LookupCode";
            this.LookupCode.HeaderText = "LookupCode";
            this.LookupCode.Name = "LookupCode";
            this.LookupCode.Visible = false;
            // 
            // SignatureDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1026, 547);
            this.Controls.Add(this.btnReload);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.txtId);
            this.Controls.Add(this.btnSign);
            this.Controls.Add(this.pdf);
            this.Controls.Add(this.tblInvoice);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "SignatureDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HiEIS";
            ((System.ComponentModel.ISupportInitialize)(this.tblInvoice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pdf)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView tblInvoice;
        private AxAcroPDFLib.AxAcroPDF pdf;
        private System.Windows.Forms.Button btnSign;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Customer;
        private System.Windows.Forms.DataGridViewTextBoxColumn Enterprise;
        private System.Windows.Forms.DataGridViewTextBoxColumn TemplateForm;
        private System.Windows.Forms.DataGridViewTextBoxColumn TemplateSerial;
        private System.Windows.Forms.DataGridViewTextBoxColumn Number;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileUrl;
        private System.Windows.Forms.DataGridViewTextBoxColumn LookupCode;
    }
}