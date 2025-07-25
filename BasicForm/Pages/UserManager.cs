
using AntdUI;
using Azure;
using BasicForm.BLL.Base;
using BasicForm.Heplers;
using BasicForm.Model;
using BasicForm.Model.Dtos;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Data;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BasicForm.Pages
{
    public partial class UserManager : UserControl
    {
        private readonly IBaseService<UserLoginInfoModel> _userService;
        private readonly IBaseService<RoleModel> _roleService;
        private readonly IBaseService<UserRoleModel> _userRoleService;
        private readonly CurrentUserContextDTO _currentUserContext;
        private List<UserLoginInfoModel> _users;

        public UserManager(IBaseService<UserLoginInfoModel> userService, CurrentUserContextDTO currentUserContext,
            IBaseService<RoleModel> roleService, IBaseService<UserRoleModel> userRoleService)
        {
            _userService = userService;
            _roleService = roleService;
            _userRoleService = userRoleService;
            _currentUserContext = currentUserContext;
            InitializeComponent();
            InitializeTableData();


        }
        private async void InitializeTableData()
        {
            userTable.Columns = new AntdUI.ColumnCollection {
                new AntdUI.ColumnCheck("check").SetFixed(),
                new AntdUI.Column("Id", "編號").SetColAlign().SetAlign(),
                new AntdUI.Column("Name", "姓名").SetColAlign().SetAlign(),
                new AntdUI.Column("LoginName","帳號").SetColAlign().SetAlign(),
                new AntdUI.ColumnSwitch("Enable", "啟用")
                {
                    Call =  (value, record, idRow, idCol) =>
                    {
                        var user = record as User;
                        if(user != null)
                        {

                            var entity =  _userService.GetByLongId(user.Id).GetAwaiter().GetResult();
                            entity.Enable = value;
                            entity.UpdateId = _currentUserContext.Id;
                            entity.UpdateTime = DateTime.Now;
                            bool isSuccess =  _userService.UpdateAsync(entity).GetAwaiter().GetResult();
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
                new AntdUI.Column("Options", "功能").SetWidth("auto").SetFixed(true).SetLocalizationTitleID("Table.Column."),
            };
            userTable.DataSource = (await GetPageData(1, 10));
            userTable.CellButtonClick += (sender, e) =>
            {
                if (e.Record is User record)
                {
                    if (e.Btn.Id != "")
                    {
                        ShowUserModal(e.Btn.Id, record.Name, record.Id, record.LoginName);
                    }

                }
            };
        }
        private async Task<List<User>> GetPageData(int current, int pageSize, string key = "")
        {
            List<User> users = new List<User>();
            _users = await _userService.GetAllAsync();
            if (!string.IsNullOrEmpty(key))
            {
                _users = _users.Where(x => x.Status && x.Name.Contains(key)).ToList();
            }
            foreach (var r in _users)
            {
                users.Add(new User(r.Id, r.Name, r.LoginName, r.Enable, r.CreateTime));
            }

            return users;
        }
        private async void ShowUserModal(string btnId, string name, long id, string loginName)
        {
            if (btnId == "edit")
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
                    Text = "姓名：",
                };
                AntdUI.Input inputName = new()
                {
                    Text = $"{name}",
                };
                gridPanelName.Controls.Add(inputName);
                gridPanelName.Controls.Add(lbName);
                stackPanel.Controls.Add(gridPanelName);
                gridPanelName.BringToFront();


                var result = AntdUI.Modal.open(new AntdUI.Modal.Config(ParentForm!, "編輯", uc, AntdUI.TType.None)
                {
                    CancelText = "取消",
                    OkText = "確定"
                });

                if(result == DialogResult.OK)
                {
                    var user = await _userService.GetByLongId(id);
                    user.Name = inputName.Text;
                    bool flag = await _userService.UpdateAsync(user);

                    if (flag)
                    {
                        AntdUI.Message.success(ParentForm!, "修改成功");
                        userTable.DataSource = (await GetPageData(1, 10));
                    }
                    else
                    {
                        AntdUI.Message.error(ParentForm!, "修改失敗");
                    }

                }
            }
            else if (btnId == "role")
            {
                UserControl uc = new()
                {
                    Size = new(400, 200),
                };
                AntdUI.StackPanel stackPanel = new()
                {
                    Dock = DockStyle.Fill,
                    Padding = new(10),
                    Vertical = true,
                    AutoScroll = true,
                };
                uc.Controls.Add(stackPanel);
                AntdUI.GridPanel gridPanel = new AntdUI.GridPanel()
                {
                    Height = 50,
                    Span = "100 100%"
                };
                AntdUI.Transfer transfer = new AntdUI.Transfer()
                {
                    Size = new(400,200)
                };
                AntdUI.Label lbSelect = new AntdUI.Label()
                {
                    Text = "當前角色"
                };

                AntdUI.Label lbSelectRole = new AntdUI.Label
                {
                    Text = "選擇角色"
                };
                AntdUI.SelectMultiple select = new AntdUI.SelectMultiple();
                select.MaxCount = 10;
                select.MaxChoiceCount = 10;
                #region 使用者角色邏輯這邊簡單處理 可自行寫成服務
                var roles = await _roleService.GetAllAsync();
                var userRoles = await _userRoleService.GetAllAsync();
                List<long> roleIds = new List<long>();
                if (userRoles.Count > 0) 
                {
                    roleIds = userRoles.Where(x=>x.UserId==id).Select(s=>s.RoleId).ToList();
                }
                if(roles.Count > 0)
                {
                    foreach (var role in roles)
                    {
                        var item = new AntdUI.SelectItem(role.Name, role);
                        select.Items.Add(item);
                    }
                }
                
                //選中的角色
                List<object> list = new List<object>();

                if (roleIds.Count > 0) 
                {
                    var theRoles = roles.Where(x => roleIds.Contains(x.Id)).ToList();
                    
                    if(theRoles.Count > 0)
                    {
                        object[] original = select.SelectedValue as object[] ?? new object[0];
                        list = original.ToList();

                        foreach (var role in theRoles)
                        {
                            list.Add(role);
                        }
                        select.SelectedValue = list.ToArray();
                    }
                }

                select.SelectedValueChanged += (s, e) =>
                {
                    list.Clear();
                    if (e.Value is object[] values)
                    {
                        var roles = values.Cast<RoleModel>().ToList();

                        foreach (var role in roles)
                        {
                            list.Add(role.Id);
                        }
                    }

                };
                #endregion
                gridPanel.Controls.Add(select);
                gridPanel.Controls.Add(lbSelect);
                stackPanel.Controls.Add(gridPanel);
                gridPanel.BringToFront();

                var result = AntdUI.Modal.open(new AntdUI.Modal.Config(ParentForm!, "分配角色", uc, AntdUI.TType.Info)
                {
                    CancelText = "取消",
                    OkText = "送出"
                });

                if(result == DialogResult.OK)
                {
                    var oldUserRole = userRoles.Where(x => x.UserId == id).ToList();
                    //建議加上事務處理
                    bool flag = await _userRoleService.DeleteAllAsync(oldUserRole);
                    //可以選擇軟刪除或實體刪除 這邊用實體
                    if (flag)
                    {
                        List<UserRoleModel> newModels = new List<UserRoleModel>();
                        var chUser = roles.Where(x => list.Contains(x.Id)).ToList();
                        foreach(var u in chUser)
                        {
                            var model = new UserRoleModel
                            {
                                UserId = id,
                                RoleId = u.Id,
                                Status = true,
                                CreateTime = DateTime.Now,
                                CreateId = _currentUserContext.Id
                            };
                            newModels.Add(model);
                        }
                        flag = await _userRoleService.AddAllAsync(newModels);
                    }

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
                    var entity = await _userService.GetByLongId(id);
                    entity.Status = false;
                    entity.UpdateId = _currentUserContext.Id;
                    entity.UpdateTime = DateTime.Now;

                    bool isSuccess = await _userService.UpdateAsync(entity);

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

        private async void AddUser_Click(object sender, EventArgs e)
        {
            UserDTO dto = new();


            UserControl uc = new()
            {
                Name = "userModal",
                Size = new Size(400, 300)
            };
            AntdUI.StackPanel stackPanel = new StackPanel()
            {
                Name = "newUserPanel",
                Dock = DockStyle.Fill,
                Padding = new(10),
                Vertical = true,
                AutoScroll = true,
            };
            uc.Controls.Add(stackPanel);


            GridPanel newNameGrid = new()
            {
                Height = 50,
                Span = "100 100%"
            };
            AntdUI.Label nameLabel = new()
            {
                Text = "姓名",
            };

            AntdUI.Input inputNewName = new()
            {
                Name = "txtName",
                Text = ""
            };
            newNameGrid.Controls.Add(inputNewName);
            newNameGrid.Controls.Add(nameLabel);
            stackPanel.Controls.Add(newNameGrid);
            newNameGrid.BringToFront();

            GridPanel newLoginNameGrid = new()
            {
                Height = 50,
                Span = "100 100%"
            };
            AntdUI.Label loginNameLabel = new()
            {
                Text = "帳號"
            };
            AntdUI.Input inputLoginName = new()
            {
                Name = "txtLoginName",
                Text = ""
            };
            newLoginNameGrid.Controls.Add(inputLoginName);
            newLoginNameGrid.Controls.Add(loginNameLabel);
            stackPanel.Controls.Add(newLoginNameGrid);
            newLoginNameGrid.BringToFront();

            GridPanel newPwdGrid = new()
            {
                Height = 50,
                Span = "100 100%"
            };
            AntdUI.Label pwdLabel = new()
            {
                Text = "密碼"
            };
            AntdUI.Input inputPwd = new()
            {
                Name = "txtPassword",
                Text = ""
            };
            newPwdGrid.Controls.Add(inputPwd);
            newPwdGrid.Controls.Add(pwdLabel);
            stackPanel.Controls.Add(newPwdGrid);
            newPwdGrid.BringToFront();
            FormBindingHepler.BindAllControl(uc, dto);

            var result = AntdUI.Modal.open(new AntdUI.Modal.Config(ParentForm!, "新增用戶", uc, TType.Info)
            {
                CancelText = "取消",
                OkType = AntdUI.TTypeMini.Primary,
                OkText = "送出"
            });

            if (result == DialogResult.OK)
            {
                bool isSuccess = false;
                var model = DtoToModel(dto, _currentUserContext.Id);
                if (model != null)
                {
                    isSuccess = await _userService.AddAsync(model);

                }

                if (isSuccess)
                {
                    AntdUI.Message.success(ParentForm!, "成功");
                    InitializeTableData();
                }
                else
                {
                    AntdUI.Message.error(ParentForm!, "失敗");
                }
            }
        }
        private UserLoginInfoModel DtoToModel(UserDTO dto, long userId)
        {
            UserLoginInfoModel model = new();
            model.LoginName = dto.LoginName;
            model.Name = dto.Name;
            model.Password = dto.Password;
            model.CreateId = userId;
            model.Enable = true;
            model.CreateTime = DateTime.Now;

            return model;
        }

        private async void Search_Click(object sender, EventArgs e)
        {
            string key = txtSearch.Text;

            userTable.DataSource = (await GetPageData(1, 10, key));

        }

        private async void Reload_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            userTable.DataSource = (await GetPageData(1, 10));
        }
    }
    public class User : AntdUI.NotifyProperty
    {
        public User(long id,string name,string login,bool enable,DateTime createTime)
        {
            _id = id;
            _name = name;
            _loginName = login;
            _enable = enable;
            _createTime = createTime;
            _options = new CellLink[]
            {
                new CellButton("edit"){Type= AntdUI.TTypeMini.Primary,IconSvg="EditFilled",Tooltip="編輯"},
                new CellButton("delete"){Type= AntdUI.TTypeMini.Primary,IconSvg="DeleteFilled",Tooltip="刪除"},
                new CellButton("role"){Type=AntdUI.TTypeMini.Success,IconSvg="UserSwitchOutlined",Tooltip="角色"}
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
        private string _loginName;

        public string LoginName
        {
            get => _loginName;
            set
            {
                if (_loginName != value) return;
                _loginName = value;
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
        private DateTime _createTime;
        public DateTime CreateTime
        {
            get => _createTime;
            set
            {
                if (_createTime != value) return;
                _createTime = value;
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
   
