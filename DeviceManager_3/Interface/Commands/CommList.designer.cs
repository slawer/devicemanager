namespace DeviceManager
{
    partial class CommList
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
            this.components = new System.ComponentModel.Container();
            this.listView1 = new System.Windows.Forms.ListView();
            this.IsActived = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fl = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ladd = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.l_pak = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cmd = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.adr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ldata = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.data = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.crc = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.commandName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Insert = new System.Windows.Forms.ToolStripMenuItem();
            this.удалитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.редактироватьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Save = new System.Windows.Forms.Button();
            this.CloseFrm = new System.Windows.Forms.Button();
            this.Remove = new System.Windows.Forms.Button();
            this.Add = new System.Windows.Forms.Button();
            this.Edit = new System.Windows.Forms.Button();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.CheckBoxes = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.IsActived,
            this.fl,
            this.ladd,
            this.l_pak,
            this.cmd,
            this.adr,
            this.ldata,
            this.data,
            this.status,
            this.crc,
            this.commandName,
            this.columnHeader1});
            this.listView1.ContextMenuStrip = this.contextMenuStrip;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView1.Location = new System.Drawing.Point(12, 12);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(725, 374);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked);
            // 
            // IsActived
            // 
            this.IsActived.Text = "Активная";
            this.IsActived.Width = 63;
            // 
            // fl
            // 
            this.fl.Text = "FL";
            this.fl.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.fl.Width = 36;
            // 
            // ladd
            // 
            this.ladd.Text = "LADD";
            this.ladd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ladd.Width = 44;
            // 
            // l_pak
            // 
            this.l_pak.Text = "L_PAK";
            this.l_pak.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.l_pak.Width = 50;
            // 
            // cmd
            // 
            this.cmd.Text = "CMD";
            this.cmd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.cmd.Width = 42;
            // 
            // adr
            // 
            this.adr.Text = "ADR";
            this.adr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.adr.Width = 47;
            // 
            // ldata
            // 
            this.ldata.Text = "LDATA";
            this.ldata.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ldata.Width = 50;
            // 
            // data
            // 
            this.data.Text = "DATA";
            this.data.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.data.Width = 46;
            // 
            // status
            // 
            this.status.Text = "STATUS";
            this.status.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // crc
            // 
            this.crc.Text = "CRC";
            this.crc.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.crc.Width = 52;
            // 
            // commandName
            // 
            this.commandName.Text = "Порт";
            this.commandName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.commandName.Width = 129;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Интервал (мс)";
            this.columnHeader1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader1.Width = 85;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Insert,
            this.удалитьToolStripMenuItem,
            this.редактироватьToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(155, 70);
            // 
            // Insert
            // 
            this.Insert.Name = "Insert";
            this.Insert.Size = new System.Drawing.Size(154, 22);
            this.Insert.Text = "Добавить";
            this.Insert.Click += new System.EventHandler(this.Add_Click);
            // 
            // удалитьToolStripMenuItem
            // 
            this.удалитьToolStripMenuItem.Name = "удалитьToolStripMenuItem";
            this.удалитьToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.удалитьToolStripMenuItem.Text = "Удалить";
            this.удалитьToolStripMenuItem.Click += new System.EventHandler(this.Remove_Click);
            // 
            // редактироватьToolStripMenuItem
            // 
            this.редактироватьToolStripMenuItem.Name = "редактироватьToolStripMenuItem";
            this.редактироватьToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.редактироватьToolStripMenuItem.Text = "Редактировать";
            this.редактироватьToolStripMenuItem.Click += new System.EventHandler(this.Edit_Click);
            // 
            // Save
            // 
            this.Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Save.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Save.Location = new System.Drawing.Point(581, 392);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(75, 23);
            this.Save.TabIndex = 2;
            this.Save.Text = "Сохранить";
            this.Save.UseVisualStyleBackColor = true;
            // 
            // CloseFrm
            // 
            this.CloseFrm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseFrm.DialogResult = System.Windows.Forms.DialogResult.No;
            this.CloseFrm.Location = new System.Drawing.Point(662, 392);
            this.CloseFrm.Name = "CloseFrm";
            this.CloseFrm.Size = new System.Drawing.Size(75, 23);
            this.CloseFrm.TabIndex = 3;
            this.CloseFrm.Text = "Закрыть";
            this.CloseFrm.UseVisualStyleBackColor = true;
            // 
            // Remove
            // 
            this.Remove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Remove.Location = new System.Drawing.Point(93, 392);
            this.Remove.Name = "Remove";
            this.Remove.Size = new System.Drawing.Size(75, 23);
            this.Remove.TabIndex = 5;
            this.Remove.Text = "Удалить";
            this.Remove.UseVisualStyleBackColor = true;
            this.Remove.Click += new System.EventHandler(this.Remove_Click);
            // 
            // Add
            // 
            this.Add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Add.Location = new System.Drawing.Point(12, 392);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(75, 23);
            this.Add.TabIndex = 6;
            this.Add.Text = "Добавить";
            this.Add.UseVisualStyleBackColor = true;
            this.Add.Click += new System.EventHandler(this.Add_Click);
            // 
            // Edit
            // 
            this.Edit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Edit.Location = new System.Drawing.Point(174, 392);
            this.Edit.Name = "Edit";
            this.Edit.Size = new System.Drawing.Size(75, 23);
            this.Edit.TabIndex = 7;
            this.Edit.Text = "Изменить";
            this.Edit.UseVisualStyleBackColor = true;
            this.Edit.Click += new System.EventHandler(this.Edit_Click);
            // 
            // CommList
            // 
            this.AcceptButton = this.Save;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseFrm;
            this.ClientSize = new System.Drawing.Size(749, 427);
            this.Controls.Add(this.Edit);
            this.Controls.Add(this.Add);
            this.Controls.Add(this.Remove);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.CloseFrm);
            this.Controls.Add(this.Save);
            this.MinimizeBox = false;
            this.Name = "CommList";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Список команд опроса";
            this.Load += new System.EventHandler(this.CommList_Load);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader IsActived;
        private System.Windows.Forms.ColumnHeader fl;
        private System.Windows.Forms.ColumnHeader ladd;
        private System.Windows.Forms.ColumnHeader l_pak;
        private System.Windows.Forms.ColumnHeader cmd;
        private System.Windows.Forms.ColumnHeader adr;
        private System.Windows.Forms.ColumnHeader ldata;
        private System.Windows.Forms.ColumnHeader data;
        private System.Windows.Forms.ColumnHeader status;
        private System.Windows.Forms.ColumnHeader crc;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem Insert;
        private System.Windows.Forms.ToolStripMenuItem удалитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem редактироватьToolStripMenuItem;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.Button CloseFrm;
        private System.Windows.Forms.Button Remove;
        private System.Windows.Forms.Button Add;
        private System.Windows.Forms.Button Edit;
        private System.Windows.Forms.ColumnHeader commandName;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}