namespace DeviceManager
{
    partial class InsertCommand
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.result = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxNumberHex = new System.Windows.Forms.TextBox();
            this.textBoxDataLenHex = new System.Windows.Forms.TextBox();
            this.Accept = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.textBoxResult = new System.Windows.Forms.TextBox();
            this.checkBoxActived = new System.Windows.Forms.CheckBox();
            this.numericUpDownDataLen = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownDevNumber = new System.Windows.Forms.NumericUpDown();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.numericUpDownInterval = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDataLen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDevNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Номер устройства";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Длинна данных";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Порт";
            // 
            // result
            // 
            this.result.AutoSize = true;
            this.result.Location = new System.Drawing.Point(15, 133);
            this.result.Name = "result";
            this.result.Size = new System.Drawing.Size(59, 13);
            this.result.TabIndex = 6;
            this.result.Text = "Результат";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(249, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Hex ->";
            // 
            // textBoxNumberHex
            // 
            this.textBoxNumberHex.Location = new System.Drawing.Point(293, 24);
            this.textBoxNumberHex.Name = "textBoxNumberHex";
            this.textBoxNumberHex.ReadOnly = true;
            this.textBoxNumberHex.Size = new System.Drawing.Size(146, 20);
            this.textBoxNumberHex.TabIndex = 8;
            this.textBoxNumberHex.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxDataLenHex
            // 
            this.textBoxDataLenHex.Location = new System.Drawing.Point(293, 51);
            this.textBoxDataLenHex.Name = "textBoxDataLenHex";
            this.textBoxDataLenHex.ReadOnly = true;
            this.textBoxDataLenHex.Size = new System.Drawing.Size(146, 20);
            this.textBoxDataLenHex.TabIndex = 9;
            this.textBoxDataLenHex.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Accept
            // 
            this.Accept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Accept.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Accept.Location = new System.Drawing.Point(283, 162);
            this.Accept.Name = "Accept";
            this.Accept.Size = new System.Drawing.Size(75, 23);
            this.Accept.TabIndex = 10;
            this.Accept.Text = "Принять";
            this.Accept.UseVisualStyleBackColor = true;
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(364, 162);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 11;
            this.Cancel.Text = "Отмена";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // textBoxResult
            // 
            this.textBoxResult.Location = new System.Drawing.Point(122, 130);
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.ReadOnly = true;
            this.textBoxResult.Size = new System.Drawing.Size(317, 20);
            this.textBoxResult.TabIndex = 12;
            this.textBoxResult.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // checkBoxActived
            // 
            this.checkBoxActived.AutoSize = true;
            this.checkBoxActived.Location = new System.Drawing.Point(122, 156);
            this.checkBoxActived.Name = "checkBoxActived";
            this.checkBoxActived.Size = new System.Drawing.Size(74, 17);
            this.checkBoxActived.TabIndex = 13;
            this.checkBoxActived.Text = "Активная";
            this.checkBoxActived.UseVisualStyleBackColor = true;
            // 
            // numericUpDownDataLen
            // 
            this.numericUpDownDataLen.Location = new System.Drawing.Point(122, 51);
            this.numericUpDownDataLen.Maximum = new decimal(new int[] {
            28,
            0,
            0,
            0});
            this.numericUpDownDataLen.Name = "numericUpDownDataLen";
            this.numericUpDownDataLen.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownDataLen.TabIndex = 14;
            this.numericUpDownDataLen.ValueChanged += new System.EventHandler(this.numericUpDownDataLen_ValueChanged);
            // 
            // numericUpDownDevNumber
            // 
            this.numericUpDownDevNumber.Location = new System.Drawing.Point(122, 25);
            this.numericUpDownDevNumber.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.numericUpDownDevNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownDevNumber.Name = "numericUpDownDevNumber";
            this.numericUpDownDevNumber.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownDevNumber.TabIndex = 15;
            this.numericUpDownDevNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownDevNumber.ValueChanged += new System.EventHandler(this.numericUpDownDevNumber_ValueChanged);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Основной",
            "Вспомогательный",
            "Порт не определен"});
            this.comboBox1.Location = new System.Drawing.Point(122, 103);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(317, 21);
            this.comboBox1.TabIndex = 16;
            // 
            // numericUpDownInterval
            // 
            this.numericUpDownInterval.Location = new System.Drawing.Point(122, 77);
            this.numericUpDownInterval.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownInterval.Name = "numericUpDownInterval";
            this.numericUpDownInterval.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownInterval.TabIndex = 17;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Частота отправки";
            // 
            // InsertCommand
            // 
            this.AcceptButton = this.Accept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(451, 197);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.numericUpDownInterval);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.numericUpDownDevNumber);
            this.Controls.Add(this.numericUpDownDataLen);
            this.Controls.Add(this.checkBoxActived);
            this.Controls.Add(this.textBoxResult);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Accept);
            this.Controls.Add(this.textBoxDataLenHex);
            this.Controls.Add(this.textBoxNumberHex);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.result);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InsertCommand";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Добавить команду";
            this.Load += new System.EventHandler(this.InsertCommand_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDataLen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDevNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInterval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label result;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxNumberHex;
        private System.Windows.Forms.TextBox textBoxDataLenHex;
        private System.Windows.Forms.Button Accept;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.TextBox textBoxResult;
        private System.Windows.Forms.CheckBox checkBoxActived;
        private System.Windows.Forms.NumericUpDown numericUpDownDataLen;
        private System.Windows.Forms.NumericUpDown numericUpDownDevNumber;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.NumericUpDown numericUpDownInterval;
        private System.Windows.Forms.Label label5;
    }
}