namespace DeviceManager
{
    partial class InsertBOForm
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.accept = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownDevice = new System.Windows.Forms.NumericUpDown();
            this.selectParameters = new System.Windows.Forms.Button();
            this.removeParameter = new System.Windows.Forms.Button();
            this.editParameter = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numericUpDownPeriod = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.toDown = new System.Windows.Forms.Button();
            this.toUp = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxComment = new System.Windows.Forms.TextBox();
            this.checkBoxToPort = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDevice)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPeriod)).BeginInit();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader2});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(6, 19);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(476, 293);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Размер(байт)";
            this.columnHeader3.Width = 97;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Описание";
            this.columnHeader2.Width = 357;
            // 
            // accept
            // 
            this.accept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.accept.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.accept.Location = new System.Drawing.Point(432, 500);
            this.accept.Name = "accept";
            this.accept.Size = new System.Drawing.Size(75, 23);
            this.accept.TabIndex = 1;
            this.accept.Text = "Принять";
            this.accept.UseVisualStyleBackColor = true;
            this.accept.Click += new System.EventHandler(this.accept_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 349);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Номер устройства в пакете";
            // 
            // numericUpDownDevice
            // 
            this.numericUpDownDevice.Location = new System.Drawing.Point(258, 347);
            this.numericUpDownDevice.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.numericUpDownDevice.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownDevice.Name = "numericUpDownDevice";
            this.numericUpDownDevice.Size = new System.Drawing.Size(64, 20);
            this.numericUpDownDevice.TabIndex = 3;
            this.numericUpDownDevice.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // selectParameters
            // 
            this.selectParameters.Location = new System.Drawing.Point(6, 318);
            this.selectParameters.Name = "selectParameters";
            this.selectParameters.Size = new System.Drawing.Size(155, 23);
            this.selectParameters.TabIndex = 4;
            this.selectParameters.Text = "Определить параметры";
            this.selectParameters.UseVisualStyleBackColor = true;
            this.selectParameters.Click += new System.EventHandler(this.selectParameters_Click);
            // 
            // removeParameter
            // 
            this.removeParameter.Location = new System.Drawing.Point(167, 318);
            this.removeParameter.Name = "removeParameter";
            this.removeParameter.Size = new System.Drawing.Size(155, 23);
            this.removeParameter.TabIndex = 5;
            this.removeParameter.Text = "Удалить параметр";
            this.removeParameter.UseVisualStyleBackColor = true;
            this.removeParameter.Click += new System.EventHandler(this.removeParameter_Click);
            // 
            // editParameter
            // 
            this.editParameter.Location = new System.Drawing.Point(328, 318);
            this.editParameter.Name = "editParameter";
            this.editParameter.Size = new System.Drawing.Size(154, 23);
            this.editParameter.TabIndex = 6;
            this.editParameter.Text = "Изменить параметр";
            this.editParameter.UseVisualStyleBackColor = true;
            this.editParameter.Click += new System.EventHandler(this.editParameter_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.numericUpDownPeriod);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.toDown);
            this.groupBox1.Controls.Add(this.toUp);
            this.groupBox1.Controls.Add(this.listView1);
            this.groupBox1.Controls.Add(this.editParameter);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.removeParameter);
            this.groupBox1.Controls.Add(this.numericUpDownDevice);
            this.groupBox1.Controls.Add(this.selectParameters);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(496, 398);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // numericUpDownPeriod
            // 
            this.numericUpDownPeriod.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownPeriod.Location = new System.Drawing.Point(258, 370);
            this.numericUpDownPeriod.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownPeriod.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownPeriod.Name = "numericUpDownPeriod";
            this.numericUpDownPeriod.Size = new System.Drawing.Size(64, 20);
            this.numericUpDownPeriod.TabIndex = 10;
            this.numericUpDownPeriod.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 372);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(207, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Частота отправки пакета в порт (мсек)";
            // 
            // toDown
            // 
            this.toDown.Location = new System.Drawing.Point(369, 347);
            this.toDown.Name = "toDown";
            this.toDown.Size = new System.Drawing.Size(35, 23);
            this.toDown.TabIndex = 8;
            this.toDown.Text = ">";
            this.toDown.UseVisualStyleBackColor = true;
            this.toDown.Click += new System.EventHandler(this.toDown_Click);
            // 
            // toUp
            // 
            this.toUp.Location = new System.Drawing.Point(328, 347);
            this.toUp.Name = "toUp";
            this.toUp.Size = new System.Drawing.Size(35, 23);
            this.toUp.TabIndex = 7;
            this.toUp.Text = "<";
            this.toUp.UseVisualStyleBackColor = true;
            this.toUp.Click += new System.EventHandler(this.toUp_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 414);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Описание";
            // 
            // textBoxComment
            // 
            this.textBoxComment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxComment.Location = new System.Drawing.Point(36, 430);
            this.textBoxComment.Multiline = true;
            this.textBoxComment.Name = "textBoxComment";
            this.textBoxComment.Size = new System.Drawing.Size(298, 72);
            this.textBoxComment.TabIndex = 9;
            // 
            // checkBoxToPort
            // 
            this.checkBoxToPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxToPort.AutoSize = true;
            this.checkBoxToPort.Location = new System.Drawing.Point(340, 430);
            this.checkBoxToPort.Name = "checkBoxToPort";
            this.checkBoxToPort.Size = new System.Drawing.Size(121, 17);
            this.checkBoxToPort.TabIndex = 10;
            this.checkBoxToPort.Text = "Отправлять в порт";
            this.checkBoxToPort.UseVisualStyleBackColor = true;
            this.checkBoxToPort.CheckedChanged += new System.EventHandler(this.checkBoxToPort_CheckedChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(340, 459);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Порт";
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Основной",
            "Вспомогательный",
            "Не определен"});
            this.comboBox1.Location = new System.Drawing.Point(378, 456);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(130, 21);
            this.comboBox1.TabIndex = 12;
            // 
            // InsertBOForm
            // 
            this.AcceptButton = this.accept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 535);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkBoxToPort);
            this.Controls.Add(this.textBoxComment);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.accept);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InsertBOForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Добавить данные для блока отображения";
            this.Load += new System.EventHandler(this.InsertBOForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDevice)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPeriod)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button accept;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.NumericUpDown numericUpDownDevice;
        public System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button selectParameters;
        private System.Windows.Forms.Button removeParameter;
        private System.Windows.Forms.Button editParameter;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxComment;
        private System.Windows.Forms.Button toDown;
        private System.Windows.Forms.Button toUp;
        private System.Windows.Forms.CheckBox checkBoxToPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.NumericUpDown numericUpDownPeriod;
        private System.Windows.Forms.Label label4;
    }
}