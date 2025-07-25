using AntdUI;
using Autofac;
using Autofac.Core.Lifetime;
using BasicForm.BLL.Base;
using BasicForm.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MenuDTO = BasicForm.Model.Dtos.MenuDTO;

namespace BasicForm.Controls
{
    public partial class MenuBarControl : UserControl
    {
        /// <summary>
        /// TabPage 選擇模式: 0 = 默認 1=自定義程序控制
        /// </summary>
        int tabPageSelectMode = 0;

        /// <summary>
        /// MenuItem 選擇模式：0 = 默認；1=自定義程序控制；
        /// </summary>
        int menuSelectMode = 0;

        /// <summary>
        /// 主菜單
        /// </summary>
        private List<MenuDTO> _menus = [];
        private readonly HashSet<string> menuCodes = [];

        private readonly ILifetimeScope _lifetimeScope;
        public MenuBarControl(ILifetimeScope lifetimeScope)
        {
            InitializeComponent();
            IninMenu();
            _lifetimeScope = lifetimeScope;
        }

        private void IninMenu()
        {
            SetMenu(null);

            #region 搜縮/展開
            btnMenuCollapsed.Click += (s, e) =>
            {
                if (!menu1.Collapsed)
                {
                    MenuCollapsedAll(null);
                    inputMenuSearch.Visible = false;
                    menu1.Collapsed = true;
                    panelMenu.Width = (int)((40 + 12) * AntdUI.Config.Dpi);
                }
                else
                {
                    inputMenuSearch.Visible = true;
                    menu1.Collapsed = false;
                    panelMenu.Width = (int)(210 * AntdUI.Config.Dpi);
                }
            };
            #endregion
            #region 菜單搜索輸入框

            DateTime searchTextChangedTime = DateTime.Now;
            bool searchFinished = true;
            int searchDelayMs = 500;    

            System.Windows.Forms.Timer timerMenuSearch = new() { Interval = 10 };

            timerMenuSearch.Tick += (s, e) =>
            {
                if ((DateTime.Now - searchTextChangedTime).TotalMilliseconds > searchDelayMs && !searchFinished)
                {
                    MenuSearch(null);
                    if (inputMenuSearch.Text == "") MenuCollapsedAll(null);
                    searchFinished = true;
                    timerMenuSearch.Stop();
                }
            };

            timerMenuSearch.Start();

            // 輸入框文本變更觸發菜單搜索
            inputMenuSearch.TextChanged += (s, e) =>
            {
                searchTextChangedTime = DateTime.Now;
                searchFinished = false;
                timerMenuSearch.Start();
            };
            #endregion
            #region 右鍵菜單

            AntdUI.IContextMenuStripItem[] cMenu1 = [
                new AntdUI.ContextMenuStripItem("全部折疊", "")
                {
                    IconSvg = "MenuFoldOutlined",
                },
                new AntdUI.ContextMenuStripItem("全部展開", "")
                {
                    IconSvg = "MenuUnfoldOutlined",
                },
            ];

            menu1.MouseClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    AntdUI.ContextMenuStrip.open(this, it =>
                    {
                        switch (it.Text)
                        {
                            case "全部折疊":
                                MenuCollapsedAll(null);
                                break;
                            case "全部展開":
                                MenuExpandAll(null);
                                break;
                        }
                    }, cMenu1);
                }
            };

            #endregion

            menu1.SelectChanged += (s, e) =>
            {
                string title = e.Value.Text!;
                string path = string.Empty;
                string path2 = string.Empty;
                string? svg = string.Empty;
                bool closeable = true;
                Type? pageType = null;
                if(e.Value.Tag is MenuTagDTO tag){
                    if (tag.Path != null) path = tag.Path;
                    if (tag.Path2 != null) path2 = tag.Path2;
                    if (tag.PageType != null) pageType = tag.PageType;
                    svg = e.Value.IconSvg;
                    closeable = tag.Closeable;
                }
                LoadTabPage(title, path, path2, pageType, svg, closeable);
            };

            //手動切換頁面後定位菜單項
            tabs1.SelectedIndexChanged += (s, e) =>
            {
                if(tabPageSelectMode == 0)
                {
                    MenuLocateAfterTabPageSelected();
                }
            };
        }
        private void CreateMenu(MenuDTO? parent1, AntdUI.MenuItem? parent2)
        {
            List<MenuDTO> _menus;

            if (parent1 == null)
            {
                _menus = this._menus ?? [];
            }
            else
            {
                _menus = parent1.Sub;
            }

            foreach(MenuDTO _item in _menus)
            {
                if (menuCodes.Contains(_item.Code))
                {
                    throw new Exception($"菜單唯一編碼({_item.Code})重複");
                }
                else
                {
                    menuCodes.Add(_item.Code);
                }

                AntdUI.MenuItem item = new(_item.Name)
                {
                    IconSvg = _item.IconSvg,
                    Tag = new MenuTagDTO(_item.Code) { PageType = _item.PageType, Closeable = _item.Closeable, },
                };

                if(parent2 == null)
                {
                    menu1.Items.Add(item);
                }
                else
                {
                    parent2.Sub.Add(item);
                }

                if (_item.Sub.Count > 0) CreateMenu(_item, item);
            }
            
        }
        /// <summary>
        /// 設置菜單項
        /// </summary>
        /// <param name="items"></param>
        public void SetMenu(List<MenuDTO>? menus)
        {
            if(menus != null) _menus = menus;
            menu1.Items.Clear();
            menuCodes.Clear();
            CreateMenu(null, null);
            MenuCollapsedAll(null);
            SetMenuItemParent(null);
            SetMenuItemPath(null);
        }
        /// <summary>
        /// 菜單默認選中項
        /// </summary>
        public void MenuSelectDefault()
        {
            var item = menu1.Items.FirstOrDefault();
            if (item != null)
            {
                string title = item.Text!;
                string path = "";
                string path2 = "";
                string? svg = "";
                bool closeable = true;
                Type? pageType = null;
                if (item.Tag is MenuTagDTO tag)
                {
                    if (tag.Path != null) path = tag.Path;
                    if (tag.Path2 != null) path2 = tag.Path2;
                    if (tag.PageType != null) pageType = tag.PageType;
                    svg = item.IconSvg;
                    closeable = tag.Closeable;
                }

                LoadTabPage(title, path, path2, pageType, svg, closeable);
            }
        }
        /// <summary>
        /// 設置所有菜單父級
        /// </summary>
        /// <param name="parent"></param>
        private void SetMenuItemParent(AntdUI.MenuItem? parent)
        {
            AntdUI.MenuItemCollection items;
            if (parent == null) items = menu1.Items;
            else items = parent.Sub;
            foreach(AntdUI.MenuItem item in items)
            {
                if(item.Tag is MenuTagDTO tag)
                {
                    tag.Parent = parent;
                }
                if (item.Sub != null) SetMenuItemParent(item);
            }
        }
        /// <summary>
        /// 設置所有菜單路徑
        /// </summary>
        /// <param name="parent"></param>
        private void SetMenuItemPath(AntdUI.MenuItem? parent)
        {
            AntdUI.MenuItemCollection items;
            if (parent == null) items = menu1.Items;
            else items = parent.Sub;
            foreach (AntdUI.MenuItem item in items)
            {
                if (item.Tag is MenuTagDTO tag)
                {
                    if (parent == null)
                    {
                        if (tag.Path == null)
                        {
                            tag.Path = $"/{tag.Code}";
                            tag.Path2 = $"/{item.Text}";
                        }
                        else
                        {
                            tag.Path = $"/{tag.Code}";
                            tag.Path2 = $"/{tag.Path2}";
                        }
                    }
                    else
                    {
                        if (parent.Tag is MenuTagDTO tag2)
                        {
                            if (tag.Path == null)
                            {
                                tag.Path = $"{tag2.Path}/{tag.Code}";
                                tag.Path2 = $"{tag2.Path2}/{item.Text}";
                            }
                            else
                            {
                                tag.Path = $"{tag2.Path}/{tag.Path}";
                                tag.Path = $"{tag2.Path2}/{tag.Path2}";
                            }
                        }
                    }
                }
                if (item.Sub != null) SetMenuItemPath(item);
            }
        }
        /// <summary>
        /// 收縮所有菜單項
        /// </summary>
        /// <param name="parent"></param>
        private void MenuCollapsedAll(AntdUI.MenuItem? parent)
        {
            AntdUI.MenuItemCollection items;
            if (parent == null) items = menu1.Items;
            else items = parent.Sub;
            foreach(AntdUI.MenuItem item in items)
            {
                if(item.Expand) item.Expand = false;
                if (item.Sub != null) MenuCollapsedAll(item);
            }
        }
        /// <summary>
        /// 展開所有菜單
        /// </summary>
        /// <param name="parent"></param>
        private void MenuExpandAll(AntdUI.MenuItem? parent)
        {
            AntdUI.MenuItemCollection items;
            if (parent == null) items = menu1.Items;
            else items = parent.Sub;
            foreach (AntdUI.MenuItem item in items)
            {
                if (item.CanExpand && !item.Expand) item.Expand = true;
                if (item.Sub != null) MenuExpandAll(item);
            }
        }
        /// <summary>
        /// 所有菜單項的父級菜單設可見
        /// </summary>
        /// <param name="item"></param>
        private static void MenuParentVisiable(AntdUI.MenuItem item)
        {
            if (item.Tag is MenuTagDTO tag)
            {
                if (tag.Parent != null)
                {
                    if (!tag.Parent.Visible) tag.Parent.Visible = true;
                    if (!tag.Parent.Expand) tag.Parent.Expand = true;
                    MenuParentVisiable(tag.Parent);
                }
            }
        }
        /// <summary>
        /// 搜索菜單
        /// </summary>
        /// <param name="parent"></param>
        private void MenuSearch(AntdUI.MenuItem? parent)
        {
            string text = inputMenuSearch.Text;
            AntdUI.MenuItemCollection items;
            if(parent == null) items = menu1.Items; 
            else items = parent.Sub;
            foreach(AntdUI.MenuItem item in items)
            {
                if (string.IsNullOrEmpty(text) && !item.Visible)
                {
                    item.Visible = true;
                }
                else
                {
                    if(item.Text!.IndexOf(text,StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        item.Visible = true;
                        if(!item.Expand) item.Expand = true;
                        MenuParentVisiable(item);
                    }
                    else
                    {
                        item.Visible = false;
                    }
                }
                if (item.Sub != null) MenuSearch(item);
            }
        }
        /// <summary>
        /// 加載頁面
        /// </summary>
        /// <param name="title"></param>
        /// <param name="path"></param>
        /// <param name="path2"></param>
        /// <param name="pageType"></param>
        /// <param name="svg"></param>
        /// <param name="closeable"></param>
        private void LoadTabPage(string title,string path,string path2,Type? pageType, string?svg, bool closeable)
        {
            tabPageSelectMode = 1;

            bool findInstance = false;

            foreach (AntdUI.TabPage _page in tabs1.Pages)
            {
                if (_page.Tag is TabPageTag tag)
                {
                    if (string.Equals(tag.MenuPath, path, StringComparison.OrdinalIgnoreCase))
                    {
                        tabs1.SelectedTab = _page;
                        findInstance = true;
                        break;
                    }
                }
            }

            if (!findInstance)
            {
                AntdUI.TabPage page = new()
                {
                    Text = title,
                    IconSvg = svg,
                    Tag = new TabPageTag(path) { Closeable = closeable },
                };
                if (!closeable)
                {
                    page.ReadOnly = true;
                }
                page.Disposed += (s, e) =>
                {
                    if (ParentForm != null)
                    {
                        AntdUI.Message.info(ParentForm, $"頁面【{page.Text}】已關閉。");
                    }
                };

                AntdUI.Panel panel = new()
                {
                    Dock = DockStyle.Fill,
                    Padding = new(2),
                };
                page.Controls.Add(panel);

                if (pageType != null)
                {
                    UserControl pageContent;

                    try
                    {
                        // 嘗試使用 Autofac 解析帶參數建構式
                        pageContent = (UserControl)_lifetimeScope.Resolve(pageType, new List<NamedParameter>
                        {
                            new NamedParameter("title", title),
                            new NamedParameter("path", path),
                            new NamedParameter("path2", path2),
                        });
                    }
                    catch (Exception)
                    {
                        try
                        {
                            // 嘗試無參數解析
                            pageContent = (UserControl)_lifetimeScope.Resolve(pageType);
                        }
                        catch (Exception ex)
                        {
                            // 最後保險措施：仍然 Activator 也失敗就 throw
                            throw new Exception($"頁面載入失敗: {pageType.Name}", ex);
                        }
                    }

                    pageContent.Dock = DockStyle.Fill;
                    panel.Controls.Add(pageContent);
                }
                else
                {
                    var alert = new AntdUI.Alert()
                    {
                        TextTitle = "警告",
                        Text = "菜單未配置關聯頁面！",
                        Icon = AntdUI.TType.Warn,
                        Height = 100,
                        Dock = DockStyle.Top,
                    };
                    panel.Padding = new(10);
                    panel.Controls.Add(alert);
                }

                tabs1.Pages.Add(page);
                tabs1.SelectedTab = page;
            }

            tabPageSelectMode = 0;
        }
        /// <summary>
        /// 展開指定菜單上級菜單
        /// </summary>
        /// <param name="item"></param>
        private static void MenuExpandAllParent(AntdUI.MenuItem item)
        {
            if (item.Tag is MenuTagDTO tag)
            {
                if (tag.Parent != null)
                {
                    if (tag.Parent.CanExpand && !tag.Parent.Expand)
                    {
                        tag.Parent.Expand = true;
                        // 選中上級菜單，當 Menu.Collapsed = true 時可以看到效果
                        tag.Parent.Select = true;
                    }
                    MenuExpandAllParent(tag.Parent);
                }
            }
        }
        /// <summary>
        /// 手動切換頁面後定位菜單項
        /// </summary>
        private void MenuLocateAfterTabPageSelected()
        {
            AntdUI.TabPage? page = tabs1.SelectedTab;

            if (page != null)
            {
                string path = "";
                if (page.Tag is TabPageTag tag)
                {
                    path = tag.MenuPath!;
                }
                MenuCollapsedAll(null);
                menuSelectMode = 1;
                SelectMenu(path, null);
                menuSelectMode = 0;
                menu1.Refresh();
            }
        }
        /// <summary>
        /// 選中指定的菜單
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        private void SelectMenu(string path, AntdUI.MenuItem? parent)
        {
            AntdUI.MenuItemCollection items;
            if (parent == null) items = menu1.Items;
            else items = parent.Sub;
            foreach (AntdUI.MenuItem item in items)
            {
                if (item.Tag is MenuTagDTO tag)
                {
                    if (tag.Path != null)
                    {
                        if (string.Equals(tag.Path, path, StringComparison.OrdinalIgnoreCase))
                        {
                            item.Select = true;
                            MenuExpandAllParent(item);
                        }
                        else
                        {
                            item.Select = false;
                        }
                    }
                }
                if (item.Sub != null) SelectMenu(path, item);
            }
        }
        /// <summary>
        /// menu 自定義資料模型
        /// </summary>
        /// <param name="code"></param>
        public class MenuTagDTO(string code)
        {
            public string Code { get; set; } = code;
            public string? Path { get; set; }
            public string? Path2 { get; set; }
            public AntdUI.MenuItem? Parent { get; set; }
            public Type? PageType { get; set; }

            /// <summary>
            /// 是否允許關閉頁面
            /// </summary>
            public bool Closeable { get; set; } = true;
        }
        class TabPageTag(string? menuPath)
        {
            /// <summary>
            /// 關聯菜單路徑
            /// </summary>
            public string? MenuPath { get; set; } = menuPath;

            /// <summary>
            /// 是否允許關閉頁面
            /// </summary>
            public bool Closeable { get; set; } = true;
        }
    }
}
