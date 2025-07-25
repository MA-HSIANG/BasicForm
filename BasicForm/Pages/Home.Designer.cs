namespace BasicForm.Pages
{
    partial class Home
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new AntdUI.Panel();
            txtUser = new AntdUI.Label();
            lbUser = new AntdUI.Label();
            button1 = new AntdUI.Button();
            panel2 = new AntdUI.Panel();
            txtAdmin = new AntdUI.Label();
            lbAdmin = new AntdUI.Label();
            button2 = new AntdUI.Button();
            label2 = new AntdUI.Label();
            gridPanel1 = new AntdUI.GridPanel();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            gridPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(txtUser);
            panel1.Controls.Add(lbUser);
            panel1.Controls.Add(button1);
            panel1.Location = new Point(295, 3);
            panel1.Name = "panel1";
            panel1.Shadow = 12;
            panel1.Size = new Size(286, 102);
            panel1.TabIndex = 0;
            panel1.Text = "panel1";
            // 
            // txtUser
            // 
            txtUser.BackColor = SystemColors.ControlLightLight;
            txtUser.Font = new Font("Microsoft JhengHei UI", 22F);
            txtUser.Location = new Point(102, 50);
            txtUser.Name = "txtUser";
            txtUser.Size = new Size(44, 29);
            txtUser.TabIndex = 3;
            txtUser.Text = "0";
            // 
            // lbUser
            // 
            lbUser.BackColor = SystemColors.ControlLightLight;
            lbUser.Location = new Point(80, 18);
            lbUser.Name = "lbUser";
            lbUser.Size = new Size(66, 26);
            lbUser.TabIndex = 2;
            lbUser.Text = "使用者:";
            // 
            // button1
            // 
            button1.IconSvg = "UserOutlined";
            button1.Location = new Point(18, 18);
            button1.Name = "button1";
            button1.Size = new Size(60, 66);
            button1.TabIndex = 1;
            button1.Type = AntdUI.TTypeMini.Primary;
            // 
            // panel2
            // 
            panel2.Controls.Add(txtAdmin);
            panel2.Controls.Add(lbAdmin);
            panel2.Controls.Add(button2);
            panel2.Location = new Point(3, 3);
            panel2.Name = "panel2";
            panel2.Shadow = 12;
            panel2.Size = new Size(286, 102);
            panel2.TabIndex = 1;
            panel2.Text = "panel2";
            // 
            // txtAdmin
            // 
            txtAdmin.BackColor = SystemColors.ControlLightLight;
            txtAdmin.Font = new Font("Microsoft JhengHei UI", 22F);
            txtAdmin.Location = new Point(102, 50);
            txtAdmin.Name = "txtAdmin";
            txtAdmin.Size = new Size(44, 29);
            txtAdmin.TabIndex = 3;
            txtAdmin.Text = "0";
            // 
            // lbAdmin
            // 
            lbAdmin.BackColor = SystemColors.ControlLightLight;
            lbAdmin.Location = new Point(80, 18);
            lbAdmin.Name = "lbAdmin";
            lbAdmin.Size = new Size(66, 26);
            lbAdmin.TabIndex = 2;
            lbAdmin.Text = "管理員:";
            // 
            // button2
            // 
            button2.IconSvg = "TeamOutlined";
            button2.Location = new Point(18, 18);
            button2.Name = "button2";
            button2.Size = new Size(60, 66);
            button2.TabIndex = 1;
            button2.Type = AntdUI.TTypeMini.Primary;
            // 
            // label2
            // 
            label2.Font = new Font("Microsoft JhengHei UI", 32F, FontStyle.Bold);
            label2.Location = new Point(3, 3);
            label2.Name = "label2";
            label2.Size = new Size(522, 23);
            label2.TabIndex = 2;
            label2.Text = "歡迎使用 MarkForm";
            // 
            // gridPanel1
            // 
            gridPanel1.Controls.Add(panel1);
            gridPanel1.Controls.Add(panel2);
            gridPanel1.Dock = DockStyle.Top;
            gridPanel1.Location = new Point(0, 0);
            gridPanel1.Name = "gridPanel1";
            gridPanel1.Size = new Size(585, 108);
            gridPanel1.Span = "50% 50% 100%";
            gridPanel1.TabIndex = 3;
            gridPanel1.Text = "gridPanel1";
            // 
            // Home
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gridPanel1);
            Controls.Add(label2);
            Name = "Home";
            Size = new Size(585, 333);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            gridPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Panel panel1;
        private AntdUI.Button button1;
        private AntdUI.Label txtUser;
        private AntdUI.Label lbUser;
        private AntdUI.Panel panel2;
        private AntdUI.Label txtAdmin;
        private AntdUI.Label lbAdmin;
        private AntdUI.Button button2;
        private AntdUI.Label label2;
        private AntdUI.GridPanel gridPanel1;
    }
}
