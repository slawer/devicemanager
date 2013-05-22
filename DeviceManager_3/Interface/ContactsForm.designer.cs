namespace DeviceManager
{
    partial class ContactsForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxContactPhone = new System.Windows.Forms.TextBox();
            this.textBoxContactEMail = new System.Windows.Forms.TextBox();
            this.textBoxContactWebSite = new System.Windows.Forms.TextBox();
            this.closeFrm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Контактный телефон";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Электронная почта";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Официальный сайт";
            // 
            // textBoxContactPhone
            // 
            this.textBoxContactPhone.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxContactPhone.Location = new System.Drawing.Point(128, 22);
            this.textBoxContactPhone.Name = "textBoxContactPhone";
            this.textBoxContactPhone.ReadOnly = true;
            this.textBoxContactPhone.Size = new System.Drawing.Size(264, 21);
            this.textBoxContactPhone.TabIndex = 3;
            this.textBoxContactPhone.Text = "8 (095) 954-26-61";
            this.textBoxContactPhone.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxContactEMail
            // 
            this.textBoxContactEMail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxContactEMail.Location = new System.Drawing.Point(128, 48);
            this.textBoxContactEMail.Name = "textBoxContactEMail";
            this.textBoxContactEMail.ReadOnly = true;
            this.textBoxContactEMail.Size = new System.Drawing.Size(264, 21);
            this.textBoxContactEMail.TabIndex = 4;
            this.textBoxContactEMail.Text = "skboreol@mail.ru";
            this.textBoxContactEMail.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxContactWebSite
            // 
            this.textBoxContactWebSite.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxContactWebSite.Location = new System.Drawing.Point(128, 74);
            this.textBoxContactWebSite.Name = "textBoxContactWebSite";
            this.textBoxContactWebSite.ReadOnly = true;
            this.textBoxContactWebSite.Size = new System.Drawing.Size(264, 21);
            this.textBoxContactWebSite.TabIndex = 5;
            this.textBoxContactWebSite.Text = "http://www.skboreol.ru/";
            this.textBoxContactWebSite.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // closeFrm
            // 
            this.closeFrm.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.closeFrm.Location = new System.Drawing.Point(317, 114);
            this.closeFrm.Name = "closeFrm";
            this.closeFrm.Size = new System.Drawing.Size(75, 23);
            this.closeFrm.TabIndex = 6;
            this.closeFrm.Text = "Закрыть";
            this.closeFrm.UseVisualStyleBackColor = true;
            // 
            // ContactsForm
            // 
            this.AcceptButton = this.closeFrm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 154);
            this.Controls.Add(this.closeFrm);
            this.Controls.Add(this.textBoxContactWebSite);
            this.Controls.Add(this.textBoxContactEMail);
            this.Controls.Add(this.textBoxContactPhone);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ContactsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Контактная информация";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxContactPhone;
        private System.Windows.Forms.TextBox textBoxContactEMail;
        private System.Windows.Forms.TextBox textBoxContactWebSite;
        private System.Windows.Forms.Button closeFrm;
    }
}