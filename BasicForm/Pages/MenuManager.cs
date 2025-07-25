using AntdUI;
using BasicForm.BLL.Base;
using BasicForm.Common.Heplers;
using BasicForm.Heplers;
using BasicForm.Model;
using BasicForm.Model.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;

namespace BasicForm.Pages
{
    public partial class MenuManager : UserControl
    {
        private readonly IBaseService<MenuModel> _menuService;
        private MenuDTO _menuDTO;
        private List<MenuModel> _menus = new List<MenuModel>();
        private List<MenuDTO> _dtos = new List<MenuDTO>();

        public MenuManager(IBaseService<MenuModel> menuService)
        {
            _menuService = menuService;

            InitializeComponent();

            tree1.BlockNode = true;
            InitializeTree();

            tree1.NodeMouseClick += async (s, e) =>
            {
                var item = e.Item;
                if (item == null) return;

                long id = long.Parse(item.ID);

                var allMenu = await _menuService.GetAllAsync();
                var theMenu = await _menuService.GetByLongId(id);
                if (theMenu == null) return;
                UserControl uc = new UserControl()
                {
                    Size = new(400, 300),
                };
                AntdUI.StackPanel stackPanel = new()
                {
                    Dock = DockStyle.Fill,
                    Padding = new(10),
                    Vertical = true,
                    AutoScroll = true,
                };
                uc.Controls.Add(stackPanel);



                AntdUI.GridPanel gridParentPanel = new AntdUI.GridPanel()
                {
                    Height = 50,
                    Span = "100 100%",
                };
                AntdUI.Label lbParent = new AntdUI.Label()
                {
                    Text = "父級菜單"
                };
                AntdUI.Select scParent = new AntdUI.Select();

                foreach (var menu in _menuDTO.Sub)
                {

                    var it = new AntdUI.SelectItem(menu.Name, menu);

                    if (menu.Sub.Count > 0)
                    {
                        it.Sub = new List<object>();

                        foreach (var m in menu.Sub)
                        {
                            it.Sub.Add(new AntdUI.SelectItem(m.Name, m));
                        }
                    }

                    scParent.Items.Add(it);
                }
                //取得父級菜單
                var p = allMenu.FirstOrDefault(x => x.Id == theMenu.ParentId);
                scParent.SelectedValue = p?.Name ?? "根結點";
                if (p == null)
                {
                    scParent.ReadOnly = true;
                }

                gridParentPanel.Controls.Add(scParent);
                gridParentPanel.Controls.Add(lbParent);
                stackPanel.Controls.Add(gridParentPanel);
                gridParentPanel.BringToFront();

                AntdUI.GridPanel gridNamePanel = new AntdUI.GridPanel()
                {
                    Height = 50,
                    Span = "100 100%",
                };
                AntdUI.Label lbName = new AntdUI.Label()
                {
                    Text = "菜單名稱"
                };
                AntdUI.Input inputName = new AntdUI.Input()
                {
                    Text = $"{theMenu.Name}",
                    Name = "txtName"
                };
                gridNamePanel.Controls.Add(inputName);
                gridNamePanel.Controls.Add(lbName);
                stackPanel.Controls.Add(gridNamePanel);
                gridNamePanel.BringToFront();

                AntdUI.GridPanel gridCodePanel = new GridPanel()
                {
                    Height = 50,
                    Span = "100 100%"
                };
                AntdUI.Label lbCode = new AntdUI.Label()
                {
                    Text = "唯一編碼"
                };
                AntdUI.Input inputCode = new AntdUI.Input()
                {
                    Text = $"{theMenu.Code}",
                    Name = "txtCode"
                };
                gridCodePanel.Controls.Add(inputCode);
                gridCodePanel.Controls.Add(lbCode);
                stackPanel.Controls.Add(gridCodePanel);
                gridCodePanel.BringToFront();


                var result = AntdUI.Modal.open(new AntdUI.Modal.Config(ParentForm!, "編輯", uc, AntdUI.TType.None)
                {
                    CancelText = "取消",
                    OkText = "確定"
                });

                if (result == DialogResult.OK)
                {
                    var model = await _menuService.GetByLongId(id);
                    if (model != null)
                    {
                        model.Name = inputName.Text;
                        model.Code = inputCode.Text;

                        bool flag = await _menuService.UpdateAsync(model);

                        if (flag)
                        {
                            AntdUI.Message.success(ParentForm!, "修改成功");
                        }
                        else
                        {
                            AntdUI.Message.success(ParentForm!, "修改失敗");
                        }
                    }
                }
            };
        }
        private void ModelToDto()
        {
            foreach (var menu in _menus)
            {
                var dto = new MenuDTO(menu.Code, menu.Name, menu.IconSvg)
                {
                    Id = menu.Id,
                    ParentId = menu.ParentId.Value,
                    Closeable = menu.Closeable,
                    PageType = RecursionHelper.GetPageType(menu.PageTypeName),
                    Sub = new List<MenuDTO>()
                };
                _dtos.Add(dto);
            }
        }
        private async void InitializeTree(string key = "")
        {
            _menus = await _menuService.GetAllAsync();
            ModelToDto();
            _menuDTO = new MenuDTO("Root", "根結點", "")
            {
                Id = 0,
                ParentId = 0,
                Closeable = false,
                Sub = new List<MenuDTO>()
            };
            RecursionHelper.AppendChildrenMenu(_dtos, _menuDTO);
            if (key != "")
            {
                var temp = _menuDTO.Sub.Where(x => x.Name.Contains(key)).ToList();
                if (temp.Count <= 0)
                {
                    foreach (var menu in _menuDTO.Sub)
                    {
                        menu.Sub = menu.Sub.Where(s => s.Name.Contains(key)).ToList();
                    }
                }
                else
                {
                    _menuDTO.Sub = temp;
                }
            }
            GetMenuTree();
            tree1.ExpandAll();
        }

        private void GetMenuTree()
        {
            AntdUI.TreeItem item;
            foreach (var menu in _menuDTO.Sub)
            {
                if (menu.Sub.Count > 0)
                {
                    item = new AntdUI.TreeItem(menu.Name) { ID = menu.Id.ToString(), IconSvg = menu.IconSvg };
                    foreach (var s in menu.Sub)
                    {
                        item.Sub.Add(new AntdUI.TreeItem(s.Name) { ID = s.Id.ToString(), IconSvg = s.IconSvg });
                    }

                    tree1.Items.Add(item);
                }
                else
                {
                    item = new AntdUI.TreeItem(menu.Name) { ID = menu.Id.ToString(), IconSvg = menu.IconSvg };
                    tree1.Items.Add(item);
                }
            }


            tree1.ResumeLayout();
        }

        private async void Add_Click(object sender, EventArgs e)
        {

            UserControl uc = new UserControl
            {
                Size = new(400, 300),
            };
            AntdUI.StackPanel stackPanel = new StackPanel
            {
                Dock = DockStyle.Fill,
                Padding = new(10),
                Vertical = true,
                AutoScroll = true,
            };
            uc.Controls.Add(stackPanel);

            AntdUI.GridPanel gridParentPanel = new AntdUI.GridPanel()
            {
                Height = 50,
                Span = "100 80% 20%"
            };
            AntdUI.Label lbParent = new AntdUI.Label
            {
                Text = "父選單"
            };
            AntdUI.Input inputParent = new AntdUI.Input
            {
                Text = "",
                Name = "txtParent",
                ReadOnly = true,
            };
            AntdUI.Dropdown dropParent = new AntdUI.Dropdown
            {
                Text = "選擇",
                Type = TTypeMini.Primary
            };
            dropParent.Click += (s, e) =>
            {
                dropParent.Items = [];

                foreach (var menu in _menuDTO.Sub)
                {

                    var item = new AntdUI.SelectItem(menu.Name, menu);

                    if (menu.Sub.Count > 0)
                    {
                        item.Sub = new List<object>();

                        foreach (var m in menu.Sub)
                        {
                            item.Sub.Add(new AntdUI.SelectItem(m.Name, m));
                        }
                    }

                    dropParent.Items.Add(item);
                }
            };
            dropParent.SelectedValueChanged += (s, e) =>
            {
                inputParent.Text = e.Value.ToString();
                AntdUI.Message.success(ParentForm!, "選擇完畢");
            };


            gridParentPanel.Controls.Add(dropParent);
            gridParentPanel.Controls.Add(inputParent);
            gridParentPanel.Controls.Add(lbParent);
            stackPanel.Controls.Add(gridParentPanel);
            gridParentPanel.BringToFront();

            AntdUI.GridPanel gridNamePanel = new GridPanel
            {
                Height = 50,
                Span = "100 100%"
            };
            AntdUI.Label lbName = new AntdUI.Label
            {
                Text = "菜單名稱",

            };
            AntdUI.Input inputName = new AntdUI.Input
            {
                Text = "",
                Name = "txtName"
            };
            gridNamePanel.Controls.Add(inputName);
            gridNamePanel.Controls.Add(lbName);
            stackPanel.Controls.Add(gridNamePanel);
            gridNamePanel.BringToFront();

            AntdUI.GridPanel gridCodePanel = new GridPanel
            {
                Height = 50,
                Span = "100 100%"
            };
            AntdUI.Label lbCode = new AntdUI.Label
            {
                Text = "唯一編碼"
            };
            AntdUI.Input inputCode = new AntdUI.Input
            {
                Text = "",
                Name = "txtCode"
            };
            gridCodePanel.Controls.Add(inputCode);
            gridCodePanel.Controls.Add(lbCode);
            stackPanel.Controls.Add(gridCodePanel);
            gridCodePanel.BringToFront();

            AntdUI.GridPanel gridIconPanel = new GridPanel
            {
                Height = 50,
                Span = "100 80% 20%"
            };
            AntdUI.Label lbIcon = new AntdUI.Label
            {
                Text = "icon"
            };
            AntdUI.Input inputIcon = new AntdUI.Input
            {
                Text = "",
                Name = "txtIcon",
                ReadOnly = true
            };
            AntdUI.Dropdown dropIcon = new AntdUI.Dropdown
            {
                Text = "選擇",
                Type = TTypeMini.Primary
            };
            dropIcon.Click += (s, e) =>
            {
                var data = GetData();
                var svgs = new List<AntdUI.VirtualItem>(data.Count);
                foreach (var it in data)
                {
                    svgs.Add(new TItem(it.Key, it.Value));
                    svgs.AddRange(it.Value);
                }
                UserControl vuc = new UserControl()
                {
                    Size = new Size(1200, 300)
                };
                AntdUI.VirtualPanel vp = new AntdUI.VirtualPanel()
                {
                    Dock = DockStyle.Fill,
                    Padding = new(10),
                    Size = new Size(1200, 300),
                };
                vp.ItemClick += (s, e) =>
                {
                    if (e.Item is VItem item)
                    {
                        inputIcon.Text = item.Key;
                        AntdUI.Message.success(ParentForm!, "選擇完畢");
                    }
                };
                vuc.Controls.Add(vp);
                vp.Items.AddRange(svgs);

                AntdUI.Modal.open(new Modal.Config(ParentForm!, "Icon", vuc, AntdUI.TType.None)
                {
                    OkText = "確定",
                    CancelText = null,
                });

            };
            gridIconPanel.Controls.Add(dropIcon);
            gridIconPanel.Controls.Add(inputIcon);
            gridIconPanel.Controls.Add(lbIcon);
            stackPanel.Controls.Add(gridIconPanel);
            gridIconPanel.BringToFront();

            AntdUI.GridPanel gridPagePanel = new AntdUI.GridPanel()
            {
                Height = 50,
                Span = "100 100%"
            };
            AntdUI.Label lbPage = new AntdUI.Label
            {
                Text = "頁面"
            };
            AntdUI.Input inputPage = new AntdUI.Input
            {
                Text = "",
                PlaceholderText = "留白默認為父選單",
                Name = "txtPage",
            };
            gridPagePanel.Controls.Add(inputPage);
            gridPagePanel.Controls.Add(lbPage);
            stackPanel.Controls.Add(gridPagePanel);
            gridPagePanel.BringToFront();


            AntdUI.GridPanel gridCloseablePanel = new GridPanel
            {
                Height = 50,
                Span = "100 100%"
            };
            AntdUI.Label lbCloseable = new AntdUI.Label
            {
                Text = "是否關閉頁面"
            };
            AntdUI.Checkbox ckCloseable = new AntdUI.Checkbox
            {
                Checked = false,
                Name = "ckCloseable"
            };
            gridCloseablePanel.Controls.Add(ckCloseable);
            gridCloseablePanel.Controls.Add(lbCloseable);
            stackPanel.Controls.Add(gridCloseablePanel);
            gridCloseablePanel.BringToFront();


            var result = AntdUI.Modal.open(new AntdUI.Modal.Config(ParentForm!, "新增", uc, AntdUI.TType.None)
            {
                CancelText = "取消",
                OkText = "送出",
            });

            if (result == DialogResult.OK)
            {
                var pid = _menus.FirstOrDefault(x => x.Name == inputParent.Text);
                var menu = new MenuModel
                {
                    ParentId = pid?.Id ?? 0,
                    Name = inputName.Text,
                    Code = inputCode.Text,
                    IconSvg = inputIcon.Text,
                    PageTypeName = inputPage.Text,
                    Closeable = ckCloseable.Checked
                };
                bool flag = await _menuService.AddAsync(menu);

                if (flag)
                {
                    AntdUI.Message.success(ParentForm!, "新增成功");

                    Reload();
                }
                else
                {
                    AntdUI.Message.error(ParentForm!, "新增失敗");
                }
            }
        }
        private void Reload()
        {
            txtSearch.Text = "";
            tree1.Items.Clear();
            _menus = new List<MenuModel>();
            _dtos = new List<MenuDTO>();
            InitializeTree();
        }
        Dictionary<string, List<VItem>> GetData()
        {
            var dir = new Dictionary<string, List<VItem>>(AntdUI.SvgDb.Custom.Count);
            var tmp = new List<VItem>(AntdUI.SvgDb.Custom.Count);
            foreach (var it in AntdUI.SvgDb.Custom)
            {
                if (it.Key == "QuestionOutlined")
                {
                    dir.Add(AntdUI.Localization.Get("Icon.Directional", "方向性圖標"), new List<VItem>(tmp));
                    tmp.Clear();
                }
                else if (it.Key == "EditOutlined")
                {
                    dir.Add(AntdUI.Localization.Get("Icon.Suggested", "提示建議性圖標"), new List<VItem>(tmp));
                    tmp.Clear();
                }
                else if (it.Key == "AreaChartOutlined")
                {
                    dir.Add(AntdUI.Localization.Get("Icon.Editor", "編輯類圖標"), new List<VItem>(tmp));
                    tmp.Clear();
                }
                else if (it.Key == "AndroidOutlined")
                {
                    dir.Add(AntdUI.Localization.Get("Icon.Data", "資料類圖標"), new List<VItem>(tmp));
                    tmp.Clear();
                }
                else if (it.Key == "AccountBookOutlined")
                {
                    dir.Add(AntdUI.Localization.Get("Icon.Logos", "品牌和標誌"), new List<VItem>(tmp));
                    tmp.Clear();
                }
                else if (it.Key == "StepBackwardFilled")
                {
                    dir.Add(AntdUI.Localization.Get("Icon.Application", "網站通用圖標"), new List<VItem>(tmp));
                    tmp.Clear();
                    return dir;
                }
                tmp.Add(new VItem(it.Key, it.Value));
            }
            dir.Add(AntdUI.Localization.Get("Icon.Application", "網站通用圖標"), new List<VItem>(tmp));
            tmp.Clear();

            return dir;
        }

        private void Reload_Click(object sender, EventArgs e)
        {
            Reload();
        }

        private void Search_Click(object sender, EventArgs e)
        {
            string key = txtSearch.Text;

            tree1.Items.Clear();
            _menus = new List<MenuModel>();
            _dtos = new List<MenuDTO>();
            InitializeTree(key);
        }
    }

    class VItem : AntdUI.VirtualItem
    {
        public string Key, Value;
        public VItem(string key, string value) { Tag = Key = key; Value = value; }

        StringFormat s_f = AntdUI.Helper.SF_NoWrap();
        internal Bitmap bmp = null, bmp_ac = null;
        public override void Paint(AntdUI.Canvas g, AntdUI.VirtualPanelArgs e)
        {
            var dpi = AntdUI.Config.Dpi;
            int icon_size = (int)(36 * dpi), text_size = (int)(24 * dpi), y = e.Rect.Y + (e.Rect.Height - (icon_size + text_size)) / 2;
            var rect_icon = new Rectangle(e.Rect.X + (e.Rect.Width - icon_size) / 2, y, icon_size, icon_size);
            var rect_text = new Rectangle(e.Rect.X, y + icon_size / 2 + text_size, e.Rect.Width, text_size);
            if (Hover)
            {
                using (var path = AntdUI.Helper.RoundPath(e.Rect, e.Radius))
                {
                    g.Fill(AntdUI.Style.Db.Primary, path);
                }
                if (bmp_ac == null) bmp_ac = AntdUI.SvgExtend.SvgToBmp(Value, icon_size, icon_size, AntdUI.Style.Db.PrimaryColor);
                g.Image(bmp_ac, rect_icon);

                g.String(Key, e.Panel.Font, AntdUI.Style.Db.PrimaryColor, rect_text, s_f);
            }
            else
            {
                if (bmp == null) bmp = AntdUI.SvgExtend.SvgToBmp(Value, icon_size, icon_size, AntdUI.Style.Db.Text);
                g.Image(bmp, rect_icon);
                g.String(Key, e.Panel.Font, AntdUI.Style.Db.Text, rect_text, s_f);

            }

        }
        public override Size Size(AntdUI.Canvas g, AntdUI.VirtualPanelArgs e)
        {
            var dpi = AntdUI.Config.Dpi;
            return new Size((int)(200 * dpi), (int)(100 * dpi));
        }
    }
    class TItem : AntdUI.VirtualItem
    {
        string title, count;
        public List<VItem> data;
        public TItem(string t, List<VItem> d)
        {
            CanClick = false;
            data = d;
            title = t;
            count = d.Count.ToString();
        }

        StringFormat s_f = AntdUI.Helper.SF_NoWrap(lr: StringAlignment.Near);
        StringFormat s_c = AntdUI.Helper.SF_NoWrap();
        public override void Paint(AntdUI.Canvas g, AntdUI.VirtualPanelArgs e)
        {
            var dpi = AntdUI.Config.Dpi;

            using (var fore = new SolidBrush(AntdUI.Style.Db.Text))
            {
                using (var font_title = new Font(e.Panel.Font, FontStyle.Bold))
                using (var font_count = new Font(e.Panel.Font.FontFamily, e.Panel.Font.Size * .74F, e.Panel.Font.Style))
                {
                    var size = AntdUI.Helper.Size(g.MeasureString(title, font_title));
                    g.String(title, font_title, fore, e.Rect, s_f);

                    var rect_count = new Rectangle(e.Rect.X + size.Width, e.Rect.Y + (e.Rect.Height - size.Height) / 2, size.Height, size.Height);
                    using (var path = AntdUI.Helper.RoundPath(rect_count, e.Radius))
                    {
                        g.Fill(AntdUI.Style.Db.TagDefaultBg, path);
                        g.Draw(AntdUI.Style.Db.DefaultBorder, 1 * dpi, path);
                    }
                    g.String(count, font_count, fore, rect_count, s_c);
                }
            }
        }

        public override Size Size(AntdUI.Canvas g, AntdUI.VirtualPanelArgs e)
        {
            var dpi = AntdUI.Config.Dpi;
            return new Size(e.Rect.Width, (int)(44 * dpi));
        }
    }
}
