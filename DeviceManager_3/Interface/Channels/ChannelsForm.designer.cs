namespace DeviceManager
{
    partial class ChannelsForm
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
            this.edit_channel = new System.Windows.Forms.Button();
            this.remove_channel = new System.Windows.Forms.Button();
            this.insert_channel = new System.Windows.Forms.Button();
            this.listViewChannels = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // edit_channel
            // 
            this.edit_channel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.edit_channel.Location = new System.Drawing.Point(174, 481);
            this.edit_channel.Name = "edit_channel";
            this.edit_channel.Size = new System.Drawing.Size(75, 23);
            this.edit_channel.TabIndex = 7;
            this.edit_channel.Text = "Изменить";
            this.edit_channel.UseVisualStyleBackColor = true;
            this.edit_channel.Click += new System.EventHandler(this.edit_channel_Click);
            // 
            // remove_channel
            // 
            this.remove_channel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.remove_channel.Location = new System.Drawing.Point(93, 481);
            this.remove_channel.Name = "remove_channel";
            this.remove_channel.Size = new System.Drawing.Size(75, 23);
            this.remove_channel.TabIndex = 6;
            this.remove_channel.Text = "Удалить";
            this.remove_channel.UseVisualStyleBackColor = true;
            this.remove_channel.Click += new System.EventHandler(this.remove_channel_Click);
            // 
            // insert_channel
            // 
            this.insert_channel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.insert_channel.Location = new System.Drawing.Point(12, 481);
            this.insert_channel.Name = "insert_channel";
            this.insert_channel.Size = new System.Drawing.Size(75, 23);
            this.insert_channel.TabIndex = 5;
            this.insert_channel.Text = "Добавить";
            this.insert_channel.UseVisualStyleBackColor = true;
            this.insert_channel.Click += new System.EventHandler(this.insert_channel_Click);
            // 
            // listViewChannels
            // 
            this.listViewChannels.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewChannels.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.listViewChannels.FullRowSelect = true;
            this.listViewChannels.GridLines = true;
            this.listViewChannels.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewChannels.Location = new System.Drawing.Point(12, 12);
            this.listViewChannels.Name = "listViewChannels";
            this.listViewChannels.Size = new System.Drawing.Size(592, 463);
            this.listViewChannels.TabIndex = 4;
            this.listViewChannels.UseCompatibleStateImageBehavior = false;
            this.listViewChannels.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Канал";
            this.columnHeader1.Width = 72;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Устройство";
            this.columnHeader2.Width = 78;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Смещение";
            this.columnHeader3.Width = 76;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Размер";
            this.columnHeader4.Width = 92;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Описание";
            this.columnHeader5.Width = 210;
            // 
            // ChannelsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(616, 516);
            this.Controls.Add(this.edit_channel);
            this.Controls.Add(this.remove_channel);
            this.Controls.Add(this.insert_channel);
            this.Controls.Add(this.listViewChannels);
            this.Name = "ChannelsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Настройка каналов";
            this.Load += new System.EventHandler(this.ChannelsForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button edit_channel;
        private System.Windows.Forms.Button remove_channel;
        private System.Windows.Forms.Button insert_channel;
        private System.Windows.Forms.ListView listViewChannels;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
    }
}