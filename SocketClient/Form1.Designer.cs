namespace SocketClient
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            content = new RichTextBox();
            send = new Button();
            portTo = new TextBox();
            serverBase = new Label();
            toSend = new TextBox();
            label1 = new Label();
            sendFile = new Button();
            SuspendLayout();
            // 
            // content
            // 
            content.Dock = DockStyle.Bottom;
            content.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            content.Location = new Point(0, 104);
            content.Name = "content";
            content.Size = new Size(926, 379);
            content.TabIndex = 0;
            content.Text = "";
            // 
            // send
            // 
            send.Location = new Point(780, 46);
            send.Name = "send";
            send.Size = new Size(94, 29);
            send.TabIndex = 1;
            send.Text = "发送";
            send.UseVisualStyleBackColor = true;
            send.Click += send_Click;
            // 
            // portTo
            // 
            portTo.Location = new Point(157, 52);
            portTo.Name = "portTo";
            portTo.Size = new Size(115, 27);
            portTo.TabIndex = 2;
            // 
            // serverBase
            // 
            serverBase.AutoSize = true;
            serverBase.Location = new Point(37, 22);
            serverBase.Name = "serverBase";
            serverBase.Size = new Size(53, 20);
            serverBase.TabIndex = 3;
            serverBase.Text = "label1";
            // 
            // toSend
            // 
            toSend.Location = new Point(404, 46);
            toSend.Name = "toSend";
            toSend.Size = new Size(370, 27);
            toSend.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(37, 55);
            label1.Name = "label1";
            label1.Size = new Size(114, 20);
            label1.TabIndex = 5;
            label1.Text = "发送至（端口）";
            // 
            // sendFile
            // 
            sendFile.Location = new Point(783, 12);
            sendFile.Name = "sendFile";
            sendFile.Size = new Size(94, 29);
            sendFile.TabIndex = 6;
            sendFile.Text = "发送文件";
            sendFile.UseVisualStyleBackColor = true;
            sendFile.Click += sendFile_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(926, 483);
            Controls.Add(sendFile);
            Controls.Add(label1);
            Controls.Add(toSend);
            Controls.Add(serverBase);
            Controls.Add(portTo);
            Controls.Add(send);
            Controls.Add(content);
            Name = "Form1";
            Text = "Form1";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox content;
        private Button send;
        private TextBox portTo;
        private Label serverBase;
        private TextBox toSend;
        private Label label1;
        private Button sendFile;
    }
}