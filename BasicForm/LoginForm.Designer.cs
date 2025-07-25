namespace BasicForm
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            LoginBtn = new AntdUI.Button();
            txtLoginName = new AntdUI.Input();
            label1 = new AntdUI.Label();
            label2 = new AntdUI.Label();
            txtPassword = new AntdUI.Input();
            image3d1 = new AntdUI.Image3D();
            loginPanel = new Panel();
            virtualPanel1 = new AntdUI.VirtualPanel();
            loginTitle = new AntdUI.PageHeader();
            loginPanel.SuspendLayout();
            SuspendLayout();
            // 
            // LoginBtn
            // 
            LoginBtn.Location = new Point(8, 270);
            LoginBtn.Name = "LoginBtn";
            LoginBtn.Size = new Size(335, 64);
            LoginBtn.TabIndex = 5;
            LoginBtn.Text = "登入";
            LoginBtn.Type = AntdUI.TTypeMini.Primary;
            LoginBtn.Click += OnLogin;
            // 
            // txtLoginName
            // 
            txtLoginName.Location = new Point(8, 75);
            txtLoginName.Name = "txtLoginName";
            txtLoginName.Size = new Size(335, 52);
            txtLoginName.TabIndex = 6;
            // 
            // label1
            // 
            label1.Font = new Font("Microsoft JhengHei UI", 12F);
            label1.Location = new Point(12, 31);
            label1.Name = "label1";
            label1.Size = new Size(94, 29);
            label1.TabIndex = 8;
            label1.Text = "帳號";
            // 
            // label2
            // 
            label2.Font = new Font("Microsoft JhengHei UI", 12F);
            label2.Location = new Point(12, 144);
            label2.Name = "label2";
            label2.Size = new Size(125, 40);
            label2.TabIndex = 9;
            label2.Text = "密碼";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(3, 190);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.PasswordPaste = false;
            txtPassword.Size = new Size(340, 52);
            txtPassword.TabIndex = 10;
            // 
            // image3d1
            // 
            image3d1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            image3d1.Image = (Image)resources.GetObject("image3d1.Image");
            image3d1.Location = new Point(12, 12);
            image3d1.Name = "image3d1";
            image3d1.Size = new Size(421, 426);
            image3d1.TabIndex = 11;
            image3d1.Text = "image3d1";
            // 
            // loginPanel
            // 
            loginPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            loginPanel.BackColor = SystemColors.InactiveBorder;
            loginPanel.Controls.Add(label1);
            loginPanel.Controls.Add(txtLoginName);
            loginPanel.Controls.Add(LoginBtn);
            loginPanel.Controls.Add(txtPassword);
            loginPanel.Controls.Add(label2);
            loginPanel.Controls.Add(virtualPanel1);
            loginPanel.Location = new Point(439, 65);
            loginPanel.Name = "loginPanel";
            loginPanel.Size = new Size(349, 373);
            loginPanel.TabIndex = 12;
            // 
            // virtualPanel1
            // 
            virtualPanel1.Location = new Point(106, 16);
            virtualPanel1.Name = "virtualPanel1";
            virtualPanel1.Size = new Size(94, 29);
            virtualPanel1.TabIndex = 12;
            virtualPanel1.Text = "virtualPanel1";
            // 
            // loginTitle
            // 
            loginTitle.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            loginTitle.CancelButton = true;
            loginTitle.CloseSize = 32;
            loginTitle.Font = new Font("Microsoft JhengHei UI", 16F, FontStyle.Bold);
            loginTitle.Location = new Point(429, 12);
            loginTitle.Margin = new Padding(3, 3, 50, 3);
            loginTitle.MaximizeBox = false;
            loginTitle.Name = "loginTitle";
            loginTitle.ShowButton = true;
            loginTitle.Size = new Size(359, 46);
            loginTitle.TabIndex = 13;
            loginTitle.Text = "歡迎使用MarkForm";
            // 
            // LoginForm
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = SystemColors.ControlLightLight;
            ClientSize = new Size(800, 450);
            Controls.Add(loginTitle);
            Controls.Add(image3d1);
            Controls.Add(loginPanel);
            EnableHitTest = false;
            FormBorderStyle = FormBorderStyle.None;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "LoginForm";
            TransparencyKey = Color.Fuchsia;
            loginPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private AntdUI.Button LoginBtn;
        private AntdUI.Input txtLoginName;
        private AntdUI.Label label1;
        private AntdUI.Label label2;
        private AntdUI.Input txtPassword;
        private AntdUI.Image3D image3d1;
        private Panel loginPanel;
        private AntdUI.PageHeader loginTitle;
        private AntdUI.VirtualPanel virtualPanel1;
    }
}