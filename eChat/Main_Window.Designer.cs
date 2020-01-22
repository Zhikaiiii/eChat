namespace eChat
{
    partial class Main_Window
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
            this.button_friend = new System.Windows.Forms.Button();
            this.button_group = new System.Windows.Forms.Button();
            this.label_info = new System.Windows.Forms.Label();
            this.label_username = new System.Windows.Forms.Label();
            this.textBox_friendname = new System.Windows.Forms.TextBox();
            this.label_info2 = new System.Windows.Forms.Label();
            this.button_addfriend = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.friendid = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.状态 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button_chat = new System.Windows.Forms.Button();
            this.button_logout = new System.Windows.Forms.Button();
            this.button_msg = new System.Windows.Forms.Button();
            this.listView_unread = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button_msglist = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button_approve = new System.Windows.Forms.Button();
            this.button_refuse = new System.Windows.Forms.Button();
            this.listView_group = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // button_friend
            // 
            this.button_friend.Location = new System.Drawing.Point(106, 83);
            this.button_friend.Name = "button_friend";
            this.button_friend.Size = new System.Drawing.Size(102, 39);
            this.button_friend.TabIndex = 0;
            this.button_friend.Text = "好友列表";
            this.button_friend.UseVisualStyleBackColor = true;
            this.button_friend.Click += new System.EventHandler(this.button_friend_Click);
            // 
            // button_group
            // 
            this.button_group.Location = new System.Drawing.Point(203, 83);
            this.button_group.Name = "button_group";
            this.button_group.Size = new System.Drawing.Size(108, 39);
            this.button_group.TabIndex = 1;
            this.button_group.Text = "群聊列表";
            this.button_group.UseVisualStyleBackColor = true;
            this.button_group.Click += new System.EventHandler(this.button_group_Click);
            // 
            // label_info
            // 
            this.label_info.AutoSize = true;
            this.label_info.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_info.Location = new System.Drawing.Point(80, 18);
            this.label_info.Name = "label_info";
            this.label_info.Size = new System.Drawing.Size(59, 20);
            this.label_info.TabIndex = 2;
            this.label_info.Text = "用户:";
            // 
            // label_username
            // 
            this.label_username.AutoSize = true;
            this.label_username.Location = new System.Drawing.Point(168, 23);
            this.label_username.Name = "label_username";
            this.label_username.Size = new System.Drawing.Size(0, 15);
            this.label_username.TabIndex = 3;
            // 
            // textBox_friendname
            // 
            this.textBox_friendname.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_friendname.Location = new System.Drawing.Point(130, 466);
            this.textBox_friendname.Name = "textBox_friendname";
            this.textBox_friendname.Size = new System.Drawing.Size(162, 30);
            this.textBox_friendname.TabIndex = 4;
            // 
            // label_info2
            // 
            this.label_info2.AutoSize = true;
            this.label_info2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_info2.Location = new System.Drawing.Point(8, 466);
            this.label_info2.Name = "label_info2";
            this.label_info2.Size = new System.Drawing.Size(109, 20);
            this.label_info2.TabIndex = 5;
            this.label_info2.Text = "添加好友：";
            // 
            // button_addfriend
            // 
            this.button_addfriend.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_addfriend.Location = new System.Drawing.Point(203, 513);
            this.button_addfriend.Name = "button_addfriend";
            this.button_addfriend.Size = new System.Drawing.Size(83, 31);
            this.button_addfriend.TabIndex = 6;
            this.button_addfriend.Text = " 添加";
            this.button_addfriend.UseVisualStyleBackColor = true;
            this.button_addfriend.Click += new System.EventHandler(this.button_addfriend_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.friendid,
            this.状态});
            this.listView1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(12, 128);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(299, 233);
            this.listView1.TabIndex = 7;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // friendid
            // 
            this.friendid.Text = "ID";
            this.friendid.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.friendid.Width = 122;
            // 
            // 状态
            // 
            this.状态.Text = "状态";
            this.状态.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.状态.Width = 151;
            // 
            // button_chat
            // 
            this.button_chat.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_chat.Location = new System.Drawing.Point(184, 394);
            this.button_chat.Name = "button_chat";
            this.button_chat.Size = new System.Drawing.Size(75, 37);
            this.button_chat.TabIndex = 8;
            this.button_chat.Text = "聊天";
            this.button_chat.UseVisualStyleBackColor = true;
            this.button_chat.Click += new System.EventHandler(this.button_chat_Click);
            // 
            // button_logout
            // 
            this.button_logout.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_logout.Location = new System.Drawing.Point(209, 634);
            this.button_logout.Name = "button_logout";
            this.button_logout.Size = new System.Drawing.Size(83, 31);
            this.button_logout.TabIndex = 9;
            this.button_logout.Text = "注销";
            this.button_logout.UseVisualStyleBackColor = true;
            this.button_logout.Click += new System.EventHandler(this.button_logout_Click);
            // 
            // button_msg
            // 
            this.button_msg.Enabled = false;
            this.button_msg.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_msg.Location = new System.Drawing.Point(56, 394);
            this.button_msg.Name = "button_msg";
            this.button_msg.Size = new System.Drawing.Size(83, 37);
            this.button_msg.TabIndex = 10;
            this.button_msg.Text = "查看";
            this.button_msg.UseVisualStyleBackColor = true;
            this.button_msg.Click += new System.EventHandler(this.button_msg_Click);
            // 
            // listView_unread
            // 
            this.listView_unread.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listView_unread.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listView_unread.HideSelection = false;
            this.listView_unread.Location = new System.Drawing.Point(12, 128);
            this.listView_unread.Name = "listView_unread";
            this.listView_unread.Size = new System.Drawing.Size(299, 233);
            this.listView_unread.TabIndex = 12;
            this.listView_unread.UseCompatibleStateImageBehavior = false;
            this.listView_unread.View = System.Windows.Forms.View.List;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "用户";
            this.columnHeader1.Width = 180;
            // 
            // button_msglist
            // 
            this.button_msglist.Location = new System.Drawing.Point(16, 83);
            this.button_msglist.Name = "button_msglist";
            this.button_msglist.Size = new System.Drawing.Size(91, 39);
            this.button_msglist.TabIndex = 13;
            this.button_msglist.Text = "新消息";
            this.button_msglist.UseVisualStyleBackColor = true;
            this.button_msglist.Click += new System.EventHandler(this.button_msglist_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(12, 558);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 20);
            this.label1.TabIndex = 14;
            this.label1.Text = "好友申请：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(31, 595);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 20);
            this.label2.TabIndex = 15;
            // 
            // button_approve
            // 
            this.button_approve.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_approve.Location = new System.Drawing.Point(12, 634);
            this.button_approve.Name = "button_approve";
            this.button_approve.Size = new System.Drawing.Size(78, 28);
            this.button_approve.TabIndex = 16;
            this.button_approve.Text = "同意";
            this.button_approve.UseVisualStyleBackColor = true;
            this.button_approve.Click += new System.EventHandler(this.button_approve_Click);
            // 
            // button_refuse
            // 
            this.button_refuse.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_refuse.Location = new System.Drawing.Point(106, 634);
            this.button_refuse.Name = "button_refuse";
            this.button_refuse.Size = new System.Drawing.Size(75, 28);
            this.button_refuse.TabIndex = 17;
            this.button_refuse.Text = "拒绝";
            this.button_refuse.UseVisualStyleBackColor = true;
            this.button_refuse.Click += new System.EventHandler(this.button_refuse_Click);
            // 
            // listView_group
            // 
            this.listView_group.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listView_group.HideSelection = false;
            this.listView_group.Location = new System.Drawing.Point(12, 128);
            this.listView_group.Name = "listView_group";
            this.listView_group.Size = new System.Drawing.Size(299, 233);
            this.listView_group.TabIndex = 18;
            this.listView_group.UseCompatibleStateImageBehavior = false;
            this.listView_group.View = System.Windows.Forms.View.List;
            // 
            // Main_Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 686);
            this.ControlBox = false;
            this.Controls.Add(this.listView_group);
            this.Controls.Add(this.button_refuse);
            this.Controls.Add(this.button_approve);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_msglist);
            this.Controls.Add(this.listView_unread);
            this.Controls.Add(this.button_msg);
            this.Controls.Add(this.button_logout);
            this.Controls.Add(this.button_chat);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.button_addfriend);
            this.Controls.Add(this.label_info2);
            this.Controls.Add(this.textBox_friendname);
            this.Controls.Add(this.label_username);
            this.Controls.Add(this.label_info);
            this.Controls.Add(this.button_group);
            this.Controls.Add(this.button_friend);
            this.Name = "Main_Window";
            this.Text = "Main_Window";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_Window_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_friend;
        private System.Windows.Forms.Button button_group;
        private System.Windows.Forms.Label label_info;
        private System.Windows.Forms.Label label_username;
        private System.Windows.Forms.TextBox textBox_friendname;
        private System.Windows.Forms.Label label_info2;
        private System.Windows.Forms.Button button_addfriend;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button button_chat;
        private System.Windows.Forms.Button button_logout;
        private System.Windows.Forms.ColumnHeader 状态;
        private System.Windows.Forms.ColumnHeader friendid;
        private System.Windows.Forms.Button button_msg;
        private System.Windows.Forms.ListView listView_unread;
        private System.Windows.Forms.Button button_msglist;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_approve;
        private System.Windows.Forms.Button button_refuse;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ListView listView_group;
    }
}