namespace BasicForm.Controls
{
    partial class MenuBarControl
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
            AntdUI.Tabs.StyleCard styleCard1 = new AntdUI.Tabs.StyleCard();
            panelMenu = new AntdUI.Panel();
            menu1 = new AntdUI.Menu();
            divider1 = new AntdUI.Divider();
            panelMenuHeader = new AntdUI.Panel();
            inputMenuSearch = new AntdUI.Input();
            btnMenuCollapsed = new AntdUI.Button();
            tabs1 = new AntdUI.Tabs();
            panelMenu.SuspendLayout();
            panelMenuHeader.SuspendLayout();
            SuspendLayout();
            // 
            // panelMenu
            // 
            panelMenu.BorderWidth = 1F;
            panelMenu.Controls.Add(menu1);
            panelMenu.Controls.Add(divider1);
            panelMenu.Controls.Add(panelMenuHeader);
            panelMenu.Dock = DockStyle.Left;
            panelMenu.Location = new Point(0, 0);
            panelMenu.Name = "panelMenu";
            panelMenu.Padding = new Padding(5);
            panelMenu.Size = new Size(210, 472);
            panelMenu.TabIndex = 1;
            // 
            // menu1
            // 
            menu1.Dock = DockStyle.Fill;
            menu1.Indent = true;
            menu1.Location = new Point(7, 57);
            menu1.Name = "menu1";
            menu1.ShowSubBack = true;
            menu1.Size = new Size(196, 408);
            menu1.TabIndex = 1;
            // 
            // divider1
            // 
            divider1.Dock = DockStyle.Top;
            divider1.Location = new Point(7, 47);
            divider1.Name = "divider1";
            divider1.Size = new Size(196, 10);
            divider1.TabIndex = 0;
            // 
            // panelMenuHeader
            // 
            panelMenuHeader.Controls.Add(inputMenuSearch);
            panelMenuHeader.Controls.Add(btnMenuCollapsed);
            panelMenuHeader.Dock = DockStyle.Top;
            panelMenuHeader.Location = new Point(7, 7);
            panelMenuHeader.Name = "panelMenuHeader";
            panelMenuHeader.Radius = 0;
            panelMenuHeader.Size = new Size(196, 40);
            panelMenuHeader.TabIndex = 1;
            // 
            // inputMenuSearch
            // 
            inputMenuSearch.AllowClear = true;
            inputMenuSearch.Dock = DockStyle.Fill;
            inputMenuSearch.IconRatio = 1F;
            inputMenuSearch.Location = new Point(40, 0);
            inputMenuSearch.Name = "inputMenuSearch";
            inputMenuSearch.PlaceholderText = "搜索菜單";
            inputMenuSearch.PrefixSvg = "SearchOutlined";
            inputMenuSearch.Size = new Size(156, 40);
            inputMenuSearch.SuffixSvg = "";
            inputMenuSearch.TabIndex = 1;
            // 
            // btnMenuCollapsed
            // 
            btnMenuCollapsed.Dock = DockStyle.Left;
            btnMenuCollapsed.IconRatio = 1F;
            btnMenuCollapsed.IconSvg = "MenuOutlined";
            btnMenuCollapsed.Location = new Point(0, 0);
            btnMenuCollapsed.Name = "btnMenuCollapsed";
            btnMenuCollapsed.Size = new Size(40, 40);
            btnMenuCollapsed.TabIndex = 0;
            // 
            // tabs1
            // 
            tabs1.Dock = DockStyle.Fill;
            tabs1.Gap = 12;
            tabs1.Location = new Point(210, 0);
            tabs1.Name = "tabs1";
            tabs1.Size = new Size(558, 472);
            styleCard1.Closable = true;
            tabs1.Style = styleCard1;
            tabs1.TabIndex = 2;
            tabs1.Type = AntdUI.TabType.Card;
            // 
            // MenuBarControl
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tabs1);
            Controls.Add(panelMenu);
            Name = "MenuBarControl";
            Size = new Size(768, 472);
            panelMenu.ResumeLayout(false);
            panelMenuHeader.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Panel panelMenu;
        private AntdUI.Menu menu1;
        private AntdUI.Divider divider1;
        private AntdUI.Panel panelMenuHeader;
        private AntdUI.Input inputMenuSearch;
        private AntdUI.Button btnMenuCollapsed;
        private AntdUI.Tabs tabs1;
    }
}
