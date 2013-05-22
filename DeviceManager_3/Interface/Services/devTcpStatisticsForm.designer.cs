namespace DeviceManager
{
    partial class devTcpStatisticsForm
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
            this.components = new System.ComponentModel.Container();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBoxCountFaillClients = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.textBoxSendingBytesTCP = new System.Windows.Forms.TextBox();
            this.textBoxCountConnectedClients = new System.Windows.Forms.TextBox();
            this.textBoxReceivedBytesTCP = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.textBoxReceivedPacketsTCP = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.textBoxSendingPacketsTCP = new System.Windows.Forms.TextBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.textBoxCountFaillClients);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.textBoxSendingBytesTCP);
            this.groupBox2.Controls.Add(this.textBoxCountConnectedClients);
            this.groupBox2.Controls.Add(this.textBoxReceivedBytesTCP);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.textBoxReceivedPacketsTCP);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.textBoxSendingPacketsTCP);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(487, 106);
            this.groupBox2.TabIndex = 39;
            this.groupBox2.TabStop = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(18, 22);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(81, 13);
            this.label11.TabIndex = 24;
            this.label11.Text = "Получено байт";
            // 
            // textBoxCountFaillClients
            // 
            this.textBoxCountFaillClients.Location = new System.Drawing.Point(382, 71);
            this.textBoxCountFaillClients.Name = "textBoxCountFaillClients";
            this.textBoxCountFaillClients.ReadOnly = true;
            this.textBoxCountFaillClients.Size = new System.Drawing.Size(100, 20);
            this.textBoxCountFaillClients.TabIndex = 35;
            this.textBoxCountFaillClients.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(18, 48);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(94, 13);
            this.label12.TabIndex = 25;
            this.label12.Text = "Отправлено байт";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(250, 74);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(126, 13);
            this.label16.TabIndex = 34;
            this.label16.Text = "Аварийных отключений";
            // 
            // textBoxSendingBytesTCP
            // 
            this.textBoxSendingBytesTCP.Location = new System.Drawing.Point(144, 45);
            this.textBoxSendingBytesTCP.Name = "textBoxSendingBytesTCP";
            this.textBoxSendingBytesTCP.ReadOnly = true;
            this.textBoxSendingBytesTCP.Size = new System.Drawing.Size(100, 20);
            this.textBoxSendingBytesTCP.TabIndex = 26;
            this.textBoxSendingBytesTCP.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxCountConnectedClients
            // 
            this.textBoxCountConnectedClients.Location = new System.Drawing.Point(144, 71);
            this.textBoxCountConnectedClients.Name = "textBoxCountConnectedClients";
            this.textBoxCountConnectedClients.ReadOnly = true;
            this.textBoxCountConnectedClients.Size = new System.Drawing.Size(100, 20);
            this.textBoxCountConnectedClients.TabIndex = 33;
            this.textBoxCountConnectedClients.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxReceivedBytesTCP
            // 
            this.textBoxReceivedBytesTCP.Location = new System.Drawing.Point(144, 19);
            this.textBoxReceivedBytesTCP.Name = "textBoxReceivedBytesTCP";
            this.textBoxReceivedBytesTCP.ReadOnly = true;
            this.textBoxReceivedBytesTCP.Size = new System.Drawing.Size(100, 20);
            this.textBoxReceivedBytesTCP.TabIndex = 27;
            this.textBoxReceivedBytesTCP.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(18, 74);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(120, 13);
            this.label15.TabIndex = 32;
            this.label15.Text = "Подключено клиентов";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(250, 22);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(99, 13);
            this.label13.TabIndex = 28;
            this.label13.Text = "Получено пакетов";
            // 
            // textBoxReceivedPacketsTCP
            // 
            this.textBoxReceivedPacketsTCP.Location = new System.Drawing.Point(382, 19);
            this.textBoxReceivedPacketsTCP.Name = "textBoxReceivedPacketsTCP";
            this.textBoxReceivedPacketsTCP.ReadOnly = true;
            this.textBoxReceivedPacketsTCP.Size = new System.Drawing.Size(100, 20);
            this.textBoxReceivedPacketsTCP.TabIndex = 31;
            this.textBoxReceivedPacketsTCP.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(250, 48);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(112, 13);
            this.label14.TabIndex = 29;
            this.label14.Text = "Отправлено пакетов";
            // 
            // textBoxSendingPacketsTCP
            // 
            this.textBoxSendingPacketsTCP.Location = new System.Drawing.Point(382, 45);
            this.textBoxSendingPacketsTCP.Name = "textBoxSendingPacketsTCP";
            this.textBoxSendingPacketsTCP.ReadOnly = true;
            this.textBoxSendingPacketsTCP.Size = new System.Drawing.Size(100, 20);
            this.textBoxSendingPacketsTCP.TabIndex = 30;
            this.textBoxSendingPacketsTCP.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // timer
            // 
            this.timer.Interval = 50;
            this.timer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // devTcpStatisticsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 135);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "devTcpStatisticsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Статистика TCP";
            this.Shown += new System.EventHandler(this.devTcpStatisticsForm_Shown);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBoxCountFaillClients;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox textBoxSendingBytesTCP;
        private System.Windows.Forms.TextBox textBoxCountConnectedClients;
        private System.Windows.Forms.TextBox textBoxReceivedBytesTCP;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBoxReceivedPacketsTCP;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBoxSendingPacketsTCP;
        private System.Windows.Forms.Timer timer;
    }
}