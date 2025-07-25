namespace BasicForm.Pages
{
    partial class RoleManager
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
            headerPanel1 = new AntdUI.GridPanel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            sendSearchBtn = new AntdUI.Button();
            txtSearch = new AntdUI.Input();
            addBtn = new AntdUI.Button();
            reloadBtn = new AntdUI.Button();
            roleTable = new AntdUI.Table();
            headerPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // headerPanel1
            // 
            headerPanel1.Controls.Add(flowLayoutPanel1);
            headerPanel1.Controls.Add(sendSearchBtn);
            headerPanel1.Controls.Add(txtSearch);
            headerPanel1.Controls.Add(addBtn);
            headerPanel1.Controls.Add(reloadBtn);
            headerPanel1.Dock = DockStyle.Top;
            headerPanel1.Location = new Point(0, 0);
            headerPanel1.Name = "headerPanel1";
            headerPanel1.Size = new Size(585, 56);
            headerPanel1.Span = "15% 15% 55% 15%\r\n\r\n";
            headerPanel1.TabIndex = 3;
            headerPanel1.Text = "gridPanel1";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Location = new Point(585, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(82, 49);
            flowLayoutPanel1.TabIndex = 4;
            // 
            // sendSearchBtn
            // 
            sendSearchBtn.Location = new Point(501, 3);
            sendSearchBtn.Name = "sendSearchBtn";
            sendSearchBtn.Size = new Size(82, 50);
            sendSearchBtn.TabIndex = 3;
            sendSearchBtn.Text = "送出";
            sendSearchBtn.Type = AntdUI.TTypeMini.Success;
            sendSearchBtn.Click += Search_Click;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(179, 3);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "搜尋...";
            txtSearch.PrefixSvg = "SearchOutlined";
            txtSearch.Size = new Size(316, 50);
            txtSearch.TabIndex = 2;
            // 
            // addBtn
            // 
            addBtn.IconSvg = "PlusOutlined";
            addBtn.Location = new Point(91, 3);
            addBtn.Name = "addBtn";
            addBtn.Size = new Size(82, 50);
            addBtn.TabIndex = 1;
            addBtn.Type = AntdUI.TTypeMini.Primary;
            // 
            // reloadBtn
            // 
            reloadBtn.IconSvg = "RedoOutlined";
            reloadBtn.Location = new Point(3, 3);
            reloadBtn.Name = "reloadBtn";
            reloadBtn.Size = new Size(82, 50);
            reloadBtn.TabIndex = 0;
            reloadBtn.Click += Reload_Click;
            // 
            // roleTable
            // 
            roleTable.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            roleTable.Location = new Point(0, 56);
            roleTable.Name = "roleTable";
            roleTable.Size = new Size(585, 274);
            roleTable.TabIndex = 4;
            roleTable.Text = "table1";
            // 
            // RoleManager
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(roleTable);
            Controls.Add(headerPanel1);
            Name = "RoleManager";
            Size = new Size(585, 333);
            headerPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.GridPanel headerPanel1;
        private FlowLayoutPanel flowLayoutPanel1;
        private AntdUI.Button sendSearchBtn;
        private AntdUI.Input txtSearch;
        private AntdUI.Button addBtn;
        private AntdUI.Button reloadBtn;
        private AntdUI.Table roleTable;
    }
}
