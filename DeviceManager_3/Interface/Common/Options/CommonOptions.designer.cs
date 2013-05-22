namespace DeviceManager
{
    partial class CommonOptions
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonEmulated = new System.Windows.Forms.RadioButton();
            this.radioButtonPassive = new System.Windows.Forms.RadioButton();
            this.radioButtonActived = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButtonTwoByte = new System.Windows.Forms.RadioButton();
            this.radioButtonCRC16 = new System.Windows.Forms.RadioButton();
            this.radioButtonOneByte = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.numericUpDownVremaMesdyOtpravkoiPaketov = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDownVremaNaZapis = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownVremaNaStenie = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.numericUpDownPopitokSteniaDannih = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDownPopitokSteniaPacketa = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownPeriodPosilkiKomand = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBoxAutoStart = new System.Windows.Forms.CheckBox();
            this.accept = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVremaMesdyOtpravkoiPaketov)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVremaNaZapis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVremaNaStenie)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPopitokSteniaDannih)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPopitokSteniaPacketa)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPeriodPosilkiKomand)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonEmulated);
            this.groupBox1.Controls.Add(this.radioButtonPassive);
            this.groupBox1.Controls.Add(this.radioButtonActived);
            this.groupBox1.Location = new System.Drawing.Point(277, 131);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(291, 52);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Режим работы";
            // 
            // radioButtonEmulated
            // 
            this.radioButtonEmulated.AutoSize = true;
            this.radioButtonEmulated.Location = new System.Drawing.Point(187, 19);
            this.radioButtonEmulated.Name = "radioButtonEmulated";
            this.radioButtonEmulated.Size = new System.Drawing.Size(75, 17);
            this.radioButtonEmulated.TabIndex = 12;
            this.radioButtonEmulated.Text = "Эмуляция";
            this.radioButtonEmulated.UseVisualStyleBackColor = true;
            this.radioButtonEmulated.CheckedChanged += new System.EventHandler(this.radioButtonEmulated_CheckedChanged);
            // 
            // radioButtonPassive
            // 
            this.radioButtonPassive.AutoSize = true;
            this.radioButtonPassive.Location = new System.Drawing.Point(98, 19);
            this.radioButtonPassive.Name = "radioButtonPassive";
            this.radioButtonPassive.Size = new System.Drawing.Size(83, 17);
            this.radioButtonPassive.TabIndex = 11;
            this.radioButtonPassive.Text = "Пассивный";
            this.radioButtonPassive.UseVisualStyleBackColor = true;
            this.radioButtonPassive.CheckedChanged += new System.EventHandler(this.radioButtonPassive_CheckedChanged);
            // 
            // radioButtonActived
            // 
            this.radioButtonActived.AutoSize = true;
            this.radioButtonActived.Checked = true;
            this.radioButtonActived.Location = new System.Drawing.Point(17, 19);
            this.radioButtonActived.Name = "radioButtonActived";
            this.radioButtonActived.Size = new System.Drawing.Size(75, 17);
            this.radioButtonActived.TabIndex = 10;
            this.radioButtonActived.TabStop = true;
            this.radioButtonActived.Text = "Активный";
            this.radioButtonActived.UseVisualStyleBackColor = true;
            this.radioButtonActived.CheckedChanged += new System.EventHandler(this.radioButtonActived_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButtonTwoByte);
            this.groupBox2.Controls.Add(this.radioButtonCRC16);
            this.groupBox2.Controls.Add(this.radioButtonOneByte);
            this.groupBox2.Location = new System.Drawing.Point(277, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(291, 113);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Тип CRC";
            // 
            // radioButtonTwoByte
            // 
            this.radioButtonTwoByte.AutoSize = true;
            this.radioButtonTwoByte.Location = new System.Drawing.Point(17, 74);
            this.radioButtonTwoByte.Name = "radioButtonTwoByte";
            this.radioButtonTwoByte.Size = new System.Drawing.Size(158, 17);
            this.radioButtonTwoByte.TabIndex = 9;
            this.radioButtonTwoByte.Text = "Циклическая двухбайтная";
            this.radioButtonTwoByte.UseVisualStyleBackColor = true;
            this.radioButtonTwoByte.CheckedChanged += new System.EventHandler(this.radioButtonTwoByte_CheckedChanged);
            // 
            // radioButtonCRC16
            // 
            this.radioButtonCRC16.AutoSize = true;
            this.radioButtonCRC16.Location = new System.Drawing.Point(17, 28);
            this.radioButtonCRC16.Name = "radioButtonCRC16";
            this.radioButtonCRC16.Size = new System.Drawing.Size(114, 17);
            this.radioButtonCRC16.TabIndex = 7;
            this.radioButtonCRC16.Text = "Алгоритм CRC-16";
            this.radioButtonCRC16.UseVisualStyleBackColor = true;
            this.radioButtonCRC16.CheckedChanged += new System.EventHandler(this.radioButtonCRC16_CheckedChanged);
            // 
            // radioButtonOneByte
            // 
            this.radioButtonOneByte.AutoSize = true;
            this.radioButtonOneByte.Checked = true;
            this.radioButtonOneByte.Location = new System.Drawing.Point(17, 51);
            this.radioButtonOneByte.Name = "radioButtonOneByte";
            this.radioButtonOneByte.Size = new System.Drawing.Size(160, 17);
            this.radioButtonOneByte.TabIndex = 8;
            this.radioButtonOneByte.TabStop = true;
            this.radioButtonOneByte.Text = "Циклическая однобайтная";
            this.radioButtonOneByte.UseVisualStyleBackColor = true;
            this.radioButtonOneByte.CheckedChanged += new System.EventHandler(this.radioButtonOneByte_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.numericUpDownVremaMesdyOtpravkoiPaketov);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.numericUpDownVremaNaZapis);
            this.groupBox3.Controls.Add(this.numericUpDownVremaNaStenie);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.numericUpDownPopitokSteniaDannih);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.numericUpDownPopitokSteniaPacketa);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.numericUpDownPeriodPosilkiKomand);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(259, 194);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Настройка обмена с устройствами через порт";
            // 
            // numericUpDownVremaMesdyOtpravkoiPaketov
            // 
            this.numericUpDownVremaMesdyOtpravkoiPaketov.Location = new System.Drawing.Point(190, 159);
            this.numericUpDownVremaMesdyOtpravkoiPaketov.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownVremaMesdyOtpravkoiPaketov.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownVremaMesdyOtpravkoiPaketov.Name = "numericUpDownVremaMesdyOtpravkoiPaketov";
            this.numericUpDownVremaMesdyOtpravkoiPaketov.Size = new System.Drawing.Size(54, 20);
            this.numericUpDownVremaMesdyOtpravkoiPaketov.TabIndex = 6;
            this.numericUpDownVremaMesdyOtpravkoiPaketov.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 161);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(176, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Время между отправкой пакетов";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 109);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(114, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Время на запись(мс)";
            // 
            // numericUpDownVremaNaZapis
            // 
            this.numericUpDownVremaNaZapis.Location = new System.Drawing.Point(190, 107);
            this.numericUpDownVremaNaZapis.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDownVremaNaZapis.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownVremaNaZapis.Name = "numericUpDownVremaNaZapis";
            this.numericUpDownVremaNaZapis.Size = new System.Drawing.Size(54, 20);
            this.numericUpDownVremaNaZapis.TabIndex = 4;
            this.numericUpDownVremaNaZapis.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // numericUpDownVremaNaStenie
            // 
            this.numericUpDownVremaNaStenie.Location = new System.Drawing.Point(190, 81);
            this.numericUpDownVremaNaStenie.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownVremaNaStenie.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownVremaNaStenie.Name = "numericUpDownVremaNaStenie";
            this.numericUpDownVremaNaStenie.Size = new System.Drawing.Size(54, 20);
            this.numericUpDownVremaNaStenie.TabIndex = 3;
            this.numericUpDownVremaNaStenie.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 83);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Время на чтение(мс)";
            // 
            // numericUpDownPopitokSteniaDannih
            // 
            this.numericUpDownPopitokSteniaDannih.Location = new System.Drawing.Point(190, 55);
            this.numericUpDownPopitokSteniaDannih.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numericUpDownPopitokSteniaDannih.Minimum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.numericUpDownPopitokSteniaDannih.Name = "numericUpDownPopitokSteniaDannih";
            this.numericUpDownPopitokSteniaDannih.Size = new System.Drawing.Size(54, 20);
            this.numericUpDownPopitokSteniaDannih.TabIndex = 2;
            this.numericUpDownPopitokSteniaDannih.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(167, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Попыток чтения данных пакета";
            // 
            // numericUpDownPopitokSteniaPacketa
            // 
            this.numericUpDownPopitokSteniaPacketa.Location = new System.Drawing.Point(190, 29);
            this.numericUpDownPopitokSteniaPacketa.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDownPopitokSteniaPacketa.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownPopitokSteniaPacketa.Name = "numericUpDownPopitokSteniaPacketa";
            this.numericUpDownPopitokSteniaPacketa.Size = new System.Drawing.Size(54, 20);
            this.numericUpDownPopitokSteniaPacketa.TabIndex = 1;
            this.numericUpDownPopitokSteniaPacketa.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Попыток чтения пакета";
            // 
            // numericUpDownPeriodPosilkiKomand
            // 
            this.numericUpDownPeriodPosilkiKomand.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownPeriodPosilkiKomand.Location = new System.Drawing.Point(190, 133);
            this.numericUpDownPeriodPosilkiKomand.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownPeriodPosilkiKomand.Name = "numericUpDownPeriodPosilkiKomand";
            this.numericUpDownPeriodPosilkiKomand.Size = new System.Drawing.Size(54, 20);
            this.numericUpDownPeriodPosilkiKomand.TabIndex = 5;
            this.numericUpDownPeriodPosilkiKomand.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 135);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(159, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Период посылки команд(мс) :";
            // 
            // checkBoxAutoStart
            // 
            this.checkBoxAutoStart.AutoSize = true;
            this.checkBoxAutoStart.Location = new System.Drawing.Point(277, 189);
            this.checkBoxAutoStart.Name = "checkBoxAutoStart";
            this.checkBoxAutoStart.Size = new System.Drawing.Size(85, 17);
            this.checkBoxAutoStart.TabIndex = 13;
            this.checkBoxAutoStart.Text = "Автозапуск";
            this.checkBoxAutoStart.UseVisualStyleBackColor = true;
            // 
            // accept
            // 
            this.accept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.accept.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.accept.Location = new System.Drawing.Point(416, 218);
            this.accept.Name = "accept";
            this.accept.Size = new System.Drawing.Size(75, 23);
            this.accept.TabIndex = 14;
            this.accept.Text = "Принять";
            this.accept.UseVisualStyleBackColor = true;
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(497, 218);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 15;
            this.cancel.Text = "Отмена";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(365, 189);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(126, 17);
            this.checkBox1.TabIndex = 17;
            this.checkBox1.Text = "Сворачивать в трей";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // CommonOptions
            // 
            this.AcceptButton = this.accept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(584, 253);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.accept);
            this.Controls.Add(this.checkBoxAutoStart);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CommonOptions";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Общие настройки обмена";
            this.Shown += new System.EventHandler(this.CommonOptions_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVremaMesdyOtpravkoiPaketov)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVremaNaZapis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVremaNaStenie)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPopitokSteniaDannih)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPopitokSteniaPacketa)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPeriodPosilkiKomand)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButtonTwoByte;
        private System.Windows.Forms.RadioButton radioButtonCRC16;
        private System.Windows.Forms.RadioButton radioButtonOneByte;
        private System.Windows.Forms.RadioButton radioButtonEmulated;
        private System.Windows.Forms.RadioButton radioButtonPassive;
        private System.Windows.Forms.RadioButton radioButtonActived;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown numericUpDownPeriodPosilkiKomand;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownPopitokSteniaPacketa;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownPopitokSteniaDannih;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDownVremaNaZapis;
        private System.Windows.Forms.NumericUpDown numericUpDownVremaNaStenie;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBoxAutoStart;
        private System.Windows.Forms.Button accept;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.NumericUpDown numericUpDownVremaMesdyOtpravkoiPaketov;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}