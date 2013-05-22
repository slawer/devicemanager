namespace DeviceManager
{
    partial class ConverterForm
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
            this.comboBoxFormuls = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.edit_formula = new System.Windows.Forms.Button();
            this.remove_formula = new System.Windows.Forms.Button();
            this.insert_formula = new System.Windows.Forms.Button();
            this.listViewFormuls = new System.Windows.Forms.ListView();
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // comboBoxFormuls
            // 
            this.comboBoxFormuls.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxFormuls.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFormuls.FormattingEnabled = true;
            this.comboBoxFormuls.Items.AddRange(new object[] {
            "Константа",
            "Присваивание",
            "Сложение",
            "Вычитание",
            "Умножение",
            "Деление",
            "Приращение",
            "Максимальное значение параметра",
            "Минимальное значение параметра",
            "10 в степени X",
            "Суммирование (сумма всех значений параметра)",
            "Скользящее среднее (цифровой фильтр 1-го порядка)",
            "Кусочно-линейная апроксимация",
            "Захват канала",
            "Команда СГТ Газы",
            "Сценарий"});
            this.comboBoxFormuls.Location = new System.Drawing.Point(73, 537);
            this.comboBoxFormuls.Name = "comboBoxFormuls";
            this.comboBoxFormuls.Size = new System.Drawing.Size(360, 21);
            this.comboBoxFormuls.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 540);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Формула";
            // 
            // edit_formula
            // 
            this.edit_formula.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.edit_formula.Location = new System.Drawing.Point(601, 535);
            this.edit_formula.Name = "edit_formula";
            this.edit_formula.Size = new System.Drawing.Size(75, 23);
            this.edit_formula.TabIndex = 9;
            this.edit_formula.Text = "Изменить";
            this.edit_formula.UseVisualStyleBackColor = true;
            this.edit_formula.Click += new System.EventHandler(this.edit_formula_Click);
            // 
            // remove_formula
            // 
            this.remove_formula.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.remove_formula.Location = new System.Drawing.Point(520, 535);
            this.remove_formula.Name = "remove_formula";
            this.remove_formula.Size = new System.Drawing.Size(75, 23);
            this.remove_formula.TabIndex = 8;
            this.remove_formula.Text = "Удалить";
            this.remove_formula.UseVisualStyleBackColor = true;
            this.remove_formula.Click += new System.EventHandler(this.remove_formula_Click);
            // 
            // insert_formula
            // 
            this.insert_formula.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.insert_formula.Location = new System.Drawing.Point(439, 535);
            this.insert_formula.Name = "insert_formula";
            this.insert_formula.Size = new System.Drawing.Size(75, 23);
            this.insert_formula.TabIndex = 7;
            this.insert_formula.Text = "Добавить";
            this.insert_formula.UseVisualStyleBackColor = true;
            this.insert_formula.Click += new System.EventHandler(this.insert_formula_Click);
            // 
            // listViewFormuls
            // 
            this.listViewFormuls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewFormuls.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader1});
            this.listViewFormuls.FullRowSelect = true;
            this.listViewFormuls.GridLines = true;
            this.listViewFormuls.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewFormuls.HideSelection = false;
            this.listViewFormuls.Location = new System.Drawing.Point(12, 12);
            this.listViewFormuls.Name = "listViewFormuls";
            this.listViewFormuls.Size = new System.Drawing.Size(664, 517);
            this.listViewFormuls.TabIndex = 5;
            this.listViewFormuls.UseCompatibleStateImageBehavior = false;
            this.listViewFormuls.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Номер параметра";
            this.columnHeader6.Width = 114;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Формула";
            this.columnHeader7.Width = 125;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Описание ";
            this.columnHeader8.Width = 276;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Описание формулы";
            this.columnHeader1.Width = 131;
            // 
            // ConverterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 570);
            this.Controls.Add(this.comboBoxFormuls);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.edit_formula);
            this.Controls.Add(this.remove_formula);
            this.Controls.Add(this.insert_formula);
            this.Controls.Add(this.listViewFormuls);
            this.Name = "ConverterForm";
            this.ShowInTaskbar = false;
            this.Text = "Конвертор данных";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConverterForm_FormClosing);
            this.Load += new System.EventHandler(this.ConverterForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxFormuls;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button edit_formula;
        private System.Windows.Forms.Button remove_formula;
        private System.Windows.Forms.Button insert_formula;
        private System.Windows.Forms.ListView listViewFormuls;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}