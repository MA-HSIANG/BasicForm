namespace BasicForm
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
            headerBar = new AntdUI.PageHeader();
            mainPanel = new AntdUI.Panel();
            SuspendLayout();
            // 
            // headerBar
            // 
            headerBar.BackColor = SystemColors.ButtonFace;
            headerBar.DividerShow = true;
            headerBar.Dock = DockStyle.Top;
            headerBar.Font = new Font("標楷體", 12F, FontStyle.Bold, GraphicsUnit.Point, 136);
            headerBar.LocalizationSubText = "";
            headerBar.Location = new Point(0, 0);
            headerBar.Name = "headerBar";
            headerBar.RightToLeft = RightToLeft.Yes;
            headerBar.ShowButton = true;
            headerBar.Size = new Size(1024, 46);
            headerBar.TabIndex = 0;
            headerBar.Text = "";
            // 
            // mainPanel
            // 
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Location = new Point(0, 46);
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new Size(1024, 530);
            mainPanel.TabIndex = 1;
            mainPanel.Text = "panel1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1024, 576);
            Controls.Add(mainPanel);
            Controls.Add(headerBar);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader headerBar;
        private AntdUI.Panel mainPanel;
    }
}
