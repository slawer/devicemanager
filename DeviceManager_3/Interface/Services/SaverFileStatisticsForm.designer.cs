namespace DeviceManager
{
    partial class SaverFileStatisticsForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxLabels = new System.Windows.Forms.TextBox();
            this.textBoxBlocks = new System.Windows.Forms.TextBox();
            this.close = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxURI = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxPCount = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxPeriod = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxSPeriod = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxBlockSize = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxDataAreaSize = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxDataOffset = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxStampsCount = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBoxStampsOffset = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBoxIsLoad = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 302);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(256, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Количество сохраненых меток время/смещение";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 328);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(208, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Количество сохраненых блоков данных";
            // 
            // textBoxLabels
            // 
            this.textBoxLabels.Location = new System.Drawing.Point(274, 299);
            this.textBoxLabels.Name = "textBoxLabels";
            this.textBoxLabels.ReadOnly = true;
            this.textBoxLabels.Size = new System.Drawing.Size(173, 20);
            this.textBoxLabels.TabIndex = 2;
            // 
            // textBoxBlocks
            // 
            this.textBoxBlocks.Location = new System.Drawing.Point(274, 325);
            this.textBoxBlocks.Name = "textBoxBlocks";
            this.textBoxBlocks.ReadOnly = true;
            this.textBoxBlocks.Size = new System.Drawing.Size(173, 20);
            this.textBoxBlocks.TabIndex = 3;
            // 
            // close
            // 
            this.close.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.close.Location = new System.Drawing.Point(372, 376);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 4;
            this.close.Text = "Закрыть";
            this.close.UseVisualStyleBackColor = true;
            this.close.Click += new System.EventHandler(this.close_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Файл";
            // 
            // textBoxURI
            // 
            this.textBoxURI.Location = new System.Drawing.Point(52, 12);
            this.textBoxURI.Name = "textBoxURI";
            this.textBoxURI.ReadOnly = true;
            this.textBoxURI.Size = new System.Drawing.Size(395, 20);
            this.textBoxURI.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(201, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Количество сохраняемых параметров";
            // 
            // textBoxPCount
            // 
            this.textBoxPCount.Location = new System.Drawing.Point(223, 51);
            this.textBoxPCount.Name = "textBoxPCount";
            this.textBoxPCount.ReadOnly = true;
            this.textBoxPCount.Size = new System.Drawing.Size(100, 20);
            this.textBoxPCount.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 236);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(189, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Частота сохранения данных в файл";
            // 
            // textBoxPeriod
            // 
            this.textBoxPeriod.Location = new System.Drawing.Point(223, 233);
            this.textBoxPeriod.Name = "textBoxPeriod";
            this.textBoxPeriod.ReadOnly = true;
            this.textBoxPeriod.Size = new System.Drawing.Size(100, 20);
            this.textBoxPeriod.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 210);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(185, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Частота сохранения меток данных";
            // 
            // textBoxSPeriod
            // 
            this.textBoxSPeriod.Location = new System.Drawing.Point(223, 207);
            this.textBoxSPeriod.Name = "textBoxSPeriod";
            this.textBoxSPeriod.ReadOnly = true;
            this.textBoxSPeriod.Size = new System.Drawing.Size(100, 20);
            this.textBoxSPeriod.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 184);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(194, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Размер сохраняемого блока данных";
            // 
            // textBoxBlockSize
            // 
            this.textBoxBlockSize.Location = new System.Drawing.Point(223, 181);
            this.textBoxBlockSize.Name = "textBoxBlockSize";
            this.textBoxBlockSize.ReadOnly = true;
            this.textBoxBlockSize.Size = new System.Drawing.Size(100, 20);
            this.textBoxBlockSize.TabIndex = 14;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 80);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(192, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Размер области сохранения данных";
            // 
            // textBoxDataAreaSize
            // 
            this.textBoxDataAreaSize.Location = new System.Drawing.Point(223, 77);
            this.textBoxDataAreaSize.Name = "textBoxDataAreaSize";
            this.textBoxDataAreaSize.ReadOnly = true;
            this.textBoxDataAreaSize.Size = new System.Drawing.Size(100, 20);
            this.textBoxDataAreaSize.TabIndex = 16;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 106);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(207, 13);
            this.label9.TabIndex = 17;
            this.label9.Text = "Смещение области сохранения данных";
            // 
            // textBoxDataOffset
            // 
            this.textBoxDataOffset.Location = new System.Drawing.Point(223, 103);
            this.textBoxDataOffset.Name = "textBoxDataOffset";
            this.textBoxDataOffset.ReadOnly = true;
            this.textBoxDataOffset.Size = new System.Drawing.Size(100, 20);
            this.textBoxDataOffset.TabIndex = 18;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 132);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(193, 13);
            this.label10.TabIndex = 19;
            this.label10.Text = "Количество меток время/смещение";
            // 
            // textBoxStampsCount
            // 
            this.textBoxStampsCount.Location = new System.Drawing.Point(223, 129);
            this.textBoxStampsCount.Name = "textBoxStampsCount";
            this.textBoxStampsCount.ReadOnly = true;
            this.textBoxStampsCount.Size = new System.Drawing.Size(100, 20);
            this.textBoxStampsCount.TabIndex = 20;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(10, 158);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(188, 13);
            this.label11.TabIndex = 21;
            this.label11.Text = "Смещение меток время/смещение";
            // 
            // textBoxStampsOffset
            // 
            this.textBoxStampsOffset.Location = new System.Drawing.Point(223, 155);
            this.textBoxStampsOffset.Name = "textBoxStampsOffset";
            this.textBoxStampsOffset.ReadOnly = true;
            this.textBoxStampsOffset.Size = new System.Drawing.Size(100, 20);
            this.textBoxStampsOffset.TabIndex = 22;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 262);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(123, 13);
            this.label12.TabIndex = 23;
            this.label12.Text = "Текущий статус файла";
            // 
            // textBoxIsLoad
            // 
            this.textBoxIsLoad.Location = new System.Drawing.Point(223, 259);
            this.textBoxIsLoad.Name = "textBoxIsLoad";
            this.textBoxIsLoad.ReadOnly = true;
            this.textBoxIsLoad.Size = new System.Drawing.Size(100, 20);
            this.textBoxIsLoad.TabIndex = 24;
            // 
            // SaverFileStatisticsForm
            // 
            this.AcceptButton = this.close;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 411);
            this.Controls.Add(this.textBoxIsLoad);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.textBoxStampsOffset);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.textBoxStampsCount);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBoxDataOffset);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBoxDataAreaSize);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBoxBlockSize);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxSPeriod);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxPeriod);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxPCount);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxURI);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.close);
            this.Controls.Add(this.textBoxBlocks);
            this.Controls.Add(this.textBoxLabels);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SaverFileStatisticsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Статистика сохранения данных в файл";
            this.Load += new System.EventHandler(this.SaverFileStatisticsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxLabels;
        private System.Windows.Forms.TextBox textBoxBlocks;
        private System.Windows.Forms.Button close;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxURI;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxPCount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxPeriod;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxSPeriod;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxBlockSize;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxDataAreaSize;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxDataOffset;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxStampsCount;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBoxStampsOffset;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBoxIsLoad;
    }
}