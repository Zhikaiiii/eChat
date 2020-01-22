namespace eChat
{
    partial class Chat_Window
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button_send = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button_close = new System.Windows.Forms.Button();
            this.button_filesend = new System.Windows.Forms.Button();
            this.button_fileaccept = new System.Windows.Forms.Button();
            this.button_filerefuse = new System.Windows.Forms.Button();
            this.label_name = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button_voicechat = new System.Windows.Forms.Button();
            this.button_voicerefuse = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(47, 420);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(441, 74);
            this.textBox1.TabIndex = 1;
            // 
            // button_send
            // 
            this.button_send.Location = new System.Drawing.Point(396, 500);
            this.button_send.Name = "button_send";
            this.button_send.Size = new System.Drawing.Size(92, 33);
            this.button_send.TabIndex = 3;
            this.button_send.Text = "发送";
            this.button_send.UseVisualStyleBackColor = true;
            this.button_send.Click += new System.EventHandler(this.button_send_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(47, 69);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(441, 305);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Text = "";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // button_close
            // 
            this.button_close.Location = new System.Drawing.Point(540, 500);
            this.button_close.Name = "button_close";
            this.button_close.Size = new System.Drawing.Size(85, 34);
            this.button_close.TabIndex = 5;
            this.button_close.Text = "关闭";
            this.button_close.UseVisualStyleBackColor = true;
            this.button_close.Click += new System.EventHandler(this.button_close_Click);
            // 
            // button_filesend
            // 
            this.button_filesend.Location = new System.Drawing.Point(47, 382);
            this.button_filesend.Name = "button_filesend";
            this.button_filesend.Size = new System.Drawing.Size(70, 32);
            this.button_filesend.TabIndex = 6;
            this.button_filesend.Text = "文件";
            this.button_filesend.UseVisualStyleBackColor = true;
            this.button_filesend.Click += new System.EventHandler(this.button_filesend_Click);
            // 
            // button_fileaccept
            // 
            this.button_fileaccept.Location = new System.Drawing.Point(328, 382);
            this.button_fileaccept.Name = "button_fileaccept";
            this.button_fileaccept.Size = new System.Drawing.Size(78, 32);
            this.button_fileaccept.TabIndex = 7;
            this.button_fileaccept.Text = "接收";
            this.button_fileaccept.UseVisualStyleBackColor = true;
            this.button_fileaccept.Click += new System.EventHandler(this.button_fileaccept_Click);
            // 
            // button_filerefuse
            // 
            this.button_filerefuse.Location = new System.Drawing.Point(412, 381);
            this.button_filerefuse.Name = "button_filerefuse";
            this.button_filerefuse.Size = new System.Drawing.Size(76, 33);
            this.button_filerefuse.TabIndex = 8;
            this.button_filerefuse.Text = "拒绝";
            this.button_filerefuse.UseVisualStyleBackColor = true;
            this.button_filerefuse.Click += new System.EventHandler(this.button_filerefuse_Click);
            // 
            // label_name
            // 
            this.label_name.AutoSize = true;
            this.label_name.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_name.Location = new System.Drawing.Point(110, 35);
            this.label_name.MaximumSize = new System.Drawing.Size(120, 20);
            this.label_name.Name = "label_name";
            this.label_name.Size = new System.Drawing.Size(119, 20);
            this.label_name.TabIndex = 9;
            this.label_name.Text = "               ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(494, 381);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 20);
            this.label1.TabIndex = 10;
            this.label1.Text = "当前用户：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(494, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 20);
            this.label2.TabIndex = 11;
            this.label2.Text = "其他用户：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(506, 410);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 20);
            this.label3.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(506, 78);
            this.label4.MaximumSize = new System.Drawing.Size(0, 1000);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 20);
            this.label4.TabIndex = 13;
            // 
            // button_voicechat
            // 
            this.button_voicechat.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_voicechat.Location = new System.Drawing.Point(123, 382);
            this.button_voicechat.Name = "button_voicechat";
            this.button_voicechat.Size = new System.Drawing.Size(72, 32);
            this.button_voicechat.TabIndex = 14;
            this.button_voicechat.Text = "音频";
            this.button_voicechat.UseVisualStyleBackColor = true;
            this.button_voicechat.Click += new System.EventHandler(this.button_voicechat_Click);
            // 
            // button_voicerefuse
            // 
            this.button_voicerefuse.Location = new System.Drawing.Point(201, 382);
            this.button_voicerefuse.Name = "button_voicerefuse";
            this.button_voicerefuse.Size = new System.Drawing.Size(75, 32);
            this.button_voicerefuse.TabIndex = 15;
            this.button_voicerefuse.Text = "拒绝";
            this.button_voicerefuse.UseVisualStyleBackColor = true;
            this.button_voicerefuse.Click += new System.EventHandler(this.button_voicerefuse_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(235, 35);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 20);
            this.label5.TabIndex = 16;
            this.label5.Text = "正在通话";
            // 
            // textBox_name
            // 
            this.textBox_name.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_name.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_name.Location = new System.Drawing.Point(95, 32);
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.Size = new System.Drawing.Size(100, 23);
            this.textBox_name.TabIndex = 17;
            this.textBox_name.TextChanged += new System.EventHandler(this.textBox_name_TextChanged);
            // 
            // Chat_Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 559);
            this.ControlBox = false;
            this.Controls.Add(this.textBox_name);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button_voicerefuse);
            this.Controls.Add(this.button_voicechat);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label_name);
            this.Controls.Add(this.button_filerefuse);
            this.Controls.Add(this.button_fileaccept);
            this.Controls.Add(this.button_filesend);
            this.Controls.Add(this.button_close);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.button_send);
            this.Controls.Add(this.textBox1);
            this.Name = "Chat_Window";
            this.Text = "Chat_Window";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button_send;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button_close;
        private System.Windows.Forms.Button button_filesend;
        private System.Windows.Forms.Button button_fileaccept;
        private System.Windows.Forms.Button button_filerefuse;
        private System.Windows.Forms.Label label_name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button_voicechat;
        private System.Windows.Forms.Button button_voicerefuse;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_name;
    }
}