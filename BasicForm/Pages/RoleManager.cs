using AntdUI;
using Autofac.Core;
using BasicForm.BLL.Base;
using BasicForm.BLL.interfaces;
using BasicForm.Heplers;
using BasicForm.Model;
using BasicForm.Model.Dtos;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BasicForm.Pages
{
    public partial class RoleManager : UserControl
    {
        private IAuthService _authService;
        private IBaseService<RoleMenuPermissionModel> _rmpService;
        private IBaseService<RoleModel> _roleService;
        private IBaseService<MenuModel> _menuService;
        private List<RoleModel> _role;
        private readonly CurrentUserContextDTO _currentUser;
        public RoleManager(IBaseService<RoleModel> roleService, IBaseService<MenuModel> menuService,
            IAuthService authService, IBaseService<RoleMenuPermissionModel> rmpService, CurrentUserContextDTO currentUser)
        {
            _roleService = roleService;
            _menuService = menuService;
            _authService = authService;
            _rmpService = rmpService;
            _currentUser = currentUser;

            InitializeComponent();
            InitializeTableData();
        }

        private async void InitializeTableData()
        {
            roleTable.Columns = new AntdUI.ColumnCollection()
            {
                new AntdUI.Column("Id","編號").SetFixed(),
                new AntdUI.Column("Code","角色識別").SetColAlign().SetAlign(),
                new AntdUI.Column("Name","角色名稱").SetColAlign().SetAlign(),
                new AntdUI.ColumnSwitch("Enable", "啟用")
                {
                    Call =  (value, record, idRow, idCol) =>
                    {
                        var role = record as Role;
                        if(role  != null)
                        {

                            var entity =  _roleService.GetByLongId(role .Id).GetAwaiter().GetResult();
                            entity.Enable = value;
                            entity.UpdateId = _currentUser.Id;
                            entity.UpdateTime = DateTime.Now;
                            bool isSuccess =  _roleService.UpdateAsync(entity).GetAwaiter().GetResult();
                            if (isSuccess)
                            {
                                AntdUI.Message.success(ParentForm!,"成功");
                            }
                            else
                            {
                                AntdUI.Message.error(ParentForm!,"失敗");
                            }

                        }
                        return value;
                    }
                },
                new AntdUI.Column("CreateTime","創建時間").SetColAlign().SetAlign(),
                new AntdUI.Column("Options","功能").SetFixed().SetWidth("auto"),
            };
            roleTable.DataSource = (await GetPageData(1, 10));

            roleTable.CellButtonClick += (sender, e) =>
            {
                if(e.Record is Role record)
                {
                    if(e.Btn.Id != "")
                    {
                        ShowRoleModal(e.Btn.Id, record.Name, record.Id, record.Code);
                    }
                }
            };
        }
        private async void ShowRoleModal(string btnId, string name, long id, string code)
        {
            if(btnId == "edit")
            {
                UserControl uc = new()
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

                AntdUI.GridPanel gridPanelId = new()
                {
                    Height = 50,
                    Span = "100 100%"
                };
                AntdUI.Label lbId = new()
                {
                    Text = "編號:",
                };
                AntdUI.Input inputId = new()
                {
                    Text = $"{id}",
                };
                inputId.ReadOnly = true;
                gridPanelId.Controls.Add(inputId);
                gridPanelId.Controls.Add(lbId);
                stackPanel.Controls.Add(gridPanelId);
                gridPanelId.BringToFront();

                AntdUI.GridPanel gridPanelName = new()
                {
                    Height = 50,
                    Span = "100 100%",
                };
                AntdUI.Label lbName = new()
                {
                    Text = "名稱：",
                };
                AntdUI.Input inputName = new()
                {
                    Text = $"{name}",
                };
                gridPanelName.Controls.Add(inputName);
                gridPanelName.Controls.Add(lbName);
                stackPanel.Controls.Add(gridPanelName);
                gridPanelName.BringToFront();

                AntdUI.GridPanel gridPanelCode = new()
                {
                    Height = 50,
                    Span = "100 100%",
                };
                AntdUI.Label lbCode = new()
                {
                    Text = "識別：",
                };
                AntdUI.Input inputCode = new()
                {
                    Text = $"{code}",
                };
                gridPanelCode.Controls.Add(inputCode);
                gridPanelCode.Controls.Add(lbCode);
                stackPanel.Controls.Add(gridPanelCode);
                gridPanelCode.BringToFront();

                var result = AntdUI.Modal.open(new AntdUI.Modal.Config(ParentForm!, "編輯", uc, AntdUI.TType.None)
                {
                    CancelText = "取消",
                    OkText = "確定"
                });

                if(result == DialogResult.OK)
                {
                    var role = await _roleService.GetByLongId(id);
                    role.Name = inputName.Text;
                    role.Code = inputCode.Text;
                    
                    bool flag = await _roleService.UpdateAsync(role);

                    if (flag)
                    {
                        AntdUI.Message.success(ParentForm!, "完成");
                        roleTable.DataSource = (await GetPageData(1, 10));
                    }
                    else
                    {
                        AntdUI.Message.error(ParentForm!, "失敗");
                    }
                }
            }
            else if(btnId == "permission")
            {
                UserControl uc = new()
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

                AntdUI.VirtualPanel vp = new AntdUI.VirtualPanel()
                {
                    Dock = DockStyle.Fill,
                    Padding = new(10),
                    Size = new Size(1200, 300),
                };

                AntdUI.Tree tree = new AntdUI.Tree();
                tree = await GetMenuTree(id);
           
                tree.ResumeLayout();
                tree.Dock = DockStyle.Fill;
                tree.Checkable = true;
                tree.CheckStrictly = true;
                tree.ExpandAll();

                vp.Controls.Add(tree);
                stackPanel.Controls.Add(vp);
                vp.BringToFront();

                var result = AntdUI.Modal.open(new AntdUI.Modal.Config(ParentForm!,"菜單", uc)
                {
                    CancelText = "取消",
                    OkText = "送出"
                });

                if(result == DialogResult.OK)
                {
                    var ckAll = tree.GetCheckeds();
                    var roleMenu = await _rmpService.GetAllAsync();
                    roleMenu = roleMenu.Where(x => x.RoleId == id && x.Status).ToList();
                    //這邊用軟刪除(可優化加上事務)
                    bool flag = true;
                    foreach (var role in roleMenu)
                    {
                        role.Status = false;
                        role.UpdateId = _currentUser.Id;
                        role.UpdateTime = DateTime.Now;

                        flag = await _rmpService.UpdateAsync(role);
                    }

                    if (flag)
                    {
                        if (ckAll.Count > 0) 
                        {
                            var models = new List<RoleMenuPermissionModel>();
                            foreach(var c in ckAll)
                            {
                                long menuId = long.Parse(c.ID);
                                var menu = await _menuService.GetByLongId(menuId);
                                var model = new RoleMenuPermissionModel
                                {
                                    RoleId = id,
                                    MenuId = menu.Id,
                                    Status = true,
                                    CreateId = _currentUser.Id,
                                    CreateTime = DateTime.Now
                                };
                                models.Add(model);
                            }
                            flag = await _rmpService.AddAllAsync(models);

                            if (flag)
                            {
                                AntdUI.Message.success(ParentForm!, "成功");
                                
                            }
                            else
                            {
                                AntdUI.Message.error(ParentForm!, "失敗");
                            }
                        }
                    }

                }
            }
            else
            {
                var result = AntdUI.Modal.open(new AntdUI.Modal.Config(ParentForm!, "警告", new AntdUI.Modal.TextLine[]
                {
                            new("是否刪除？", 6, AntdUI.Style.Db.Warning),
                            new($"ID: {id}", 6, AntdUI.Style.Db.Primary),
                            new($"Name: {name}", 6, AntdUI.Style.Db.Primary),
                }, AntdUI.TType.Warn));

                if (result == DialogResult.OK)
                {
                    var entity = await _roleService.GetByLongId(id);
                    entity.Status = false;
                    entity.UpdateId = _currentUser.Id;
                    entity.UpdateTime = DateTime.Now;

                    bool isSuccess = await _roleService.UpdateAsync(entity);

                    if (isSuccess)
                    {
                        AntdUI.Message.success(ParentForm!, "刪除成功");
                        InitializeTableData();
                    }
                    else
                    {
                        AntdUI.Message.error(ParentForm!, "刪除失敗");
                    }
                }
            }
        }
        private async Task<Tree> GetMenuTree(long roleId)
        {
            Tree tree = new Tree();

            var menus = await _menuService.GetAllAsync();
            var roleMenu = await _rmpService.GetAllAsync();
            roleMenu = roleMenu.Where(x => x.RoleId == roleId && x.Status).ToList();

            var dtos = new List<MenuDTO>();
            foreach (var menu in menus)
            {
                var dto = new MenuDTO(menu.Code, menu.Name, menu.IconSvg)
                {
                    Id = menu.Id,
                    ParentId = menu.ParentId.Value,
                    Closeable = menu.Closeable,
                    PageType = RecursionHelper.GetPageType(menu.PageTypeName),
                    Sub = new List<MenuDTO>()
                };
                dtos.Add(dto);
            }
            var menuDTO = new MenuDTO("Root", "根結點", "")
            {
                Id = 0,
                ParentId = 0,
                Closeable = false,
                Sub = new List<MenuDTO>()
            };
            RecursionHelper.AppendChildrenMenu(dtos, menuDTO);

            AntdUI.TreeItem item;

            foreach (var menu in menuDTO.Sub)
            {
                if (menu.Sub.Count > 0)
                {
                    item = new AntdUI.TreeItem(menu.Name) { ID = menu.Id.ToString(), IconSvg = menu.IconSvg };
                    var theMenu = roleMenu.FirstOrDefault(x => x.MenuId == menu.Id);
                    if (theMenu != null)
                    {
                        item.Checked = true;
                        item.CheckState = CheckState.Checked;
                    }

                    foreach (var s in menu.Sub)
                    {
                        var subItem = new AntdUI.TreeItem(s.Name) { ID = s.Id.ToString(), IconSvg = s.IconSvg };
                        theMenu = roleMenu.FirstOrDefault(x => x.MenuId == s.Id);
                        if(theMenu != null)
                        {
                            subItem.Checked = true;
                            subItem.CheckState = CheckState.Checked;
                        }
                        
                        item.Sub.Add(subItem);
                    }
                    
                    tree.Items.Add(item);
                }
                else
                {
                    item = new AntdUI.TreeItem(menu.Name) { ID = menu.Id.ToString(), IconSvg = menu.IconSvg };
                    tree.Items.Add(item);
                }
            }


            return tree;
        }
        private async Task<List<Role>> GetPageData(int current, int pageSize, string key = "")
        {
            List<Role> roles = new List<Role>();
            _role = await _roleService.GetAllAsync();
            if (key != "")
            {
                _role = _role.Where(x => x.Name.Contains(key)).ToList();
            }
            foreach (var r in _role.Where(x=> x.Status))
            {
                roles.Add(new Role(r));
            }

            return roles;
        }

        private async void Search_Click(object sender, EventArgs e)
        {
            string key = txtSearch.Text;

            roleTable.DataSource = (await GetPageData(1, 10, key));
        }

        private async void Reload_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            roleTable.DataSource = (await GetPageData(1, 10));
        }
    }
    public class Role : AntdUI.NotifyProperty
    {
        public Role(RoleModel role)
        {
            _id = role.Id;
            _code = role.Code;
            _name = role.Name;
            _enable = role.Enable;

            _options = new CellLink[]
           {
                new CellButton("edit"){Type= AntdUI.TTypeMini.Primary,IconSvg="EditFilled",Tooltip="編輯"},
                new CellButton("delete"){Type= AntdUI.TTypeMini.Error,IconSvg="DeleteFilled",Tooltip="刪除"},
                new CellButton("permission"){Type= AntdUI.TTypeMini.Success,IconSvg="SafetyCertificateOutlined",Tooltip="權限"}
           };
        }

        private long _id;

        public long Id
        {
            get => _id;
            set 
            {
                if (_id != value) return;
                _id = value;
                OnPropertyChanged();
            }
        }
        private string _code;

        public string Code
        {
            get => _code;
            set
            {
                if (_code != value) return;
                _code = value;
                OnPropertyChanged();
            }
        }
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value) return;
                _name = value;
                OnPropertyChanged();
            }
        }
        private bool _enable;

        public bool Enable
        {
            get => _enable;
            set
            {
                if (_enable != value) return;
                _enable = value;
                OnPropertyChanged();
            }
        }
        AntdUI.CellLink[] _options;
        public AntdUI.CellLink[] Options
        {
            get => _options;
            set
            {
                _options = value;
                OnPropertyChanged();
            }
        }
    }    
}
