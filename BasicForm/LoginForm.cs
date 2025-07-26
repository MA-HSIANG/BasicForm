using BasicForm.BLL.interfaces;
using BasicForm.Common.Helpers;
using BasicForm.Heplers;
using BasicForm.Model;
using BasicForm.Model.Dtos;
using BasicForm.Pages;
using System;


namespace BasicForm
{
    public partial class LoginForm : AntdUI.Window
    {
        private readonly AccountInfoDTO _accountInfoDTO;
        private readonly IAuthService _authService;
        public CurrentUserContextDTO currentUser = new();

        public LoginForm(IAuthService AuthService, AccountInfoDTO AccountInfoDTO)
        {
            _authService = AuthService;
            _accountInfoDTO = AccountInfoDTO;


            InitializeComponent();

            //自動綁定
            FormBindingHepler.BindAllControl(loginPanel, _accountInfoDTO);

        }

        private async void OnLogin(object sender, EventArgs e)
        {
            AntdUI.Button btn = (AntdUI.Button)sender;
            btn.Loading = true;
            btn.LoadingWaveValue = 0;
            ResponseDTO<UserLoginInfoModel> result = await _authService.LoginAsync(_accountInfoDTO);
            if (result.IsSuccess)
            {
                AntdUI.Message.success(this, result.Message);
                currentUser.Id = result.Data.Id;
                currentUser.UserName = result.Data.Name;

                //登入成功取得菜單
                var root = new MenuDTO("Root", "根結點", "")
                {
                    Id = 0,
                    ParentId = 0,
                    Closeable = false,
                    Sub = new List<MenuDTO>()
                };
                //固定加入首頁
                currentUser.Menus.Add(new MenuDTO("Home", "首頁", "HomeFilled")
                {
                    Id = 99,
                    ParentId = 0,
                    PageType = typeof(Home),
                    Closeable = false,
                    Sub = new List<MenuDTO>()
                });
                var menuModel = await _authService.RoleMenuMap(result.Data.Id);
                if (menuModel.Count > 0) 
                {
                    foreach(var menu in menuModel)
                    {
                        currentUser.Menus.Add(new MenuDTO(menu.Code, menu.Name, menu.IconSvg)
                        {
                            Id = menu.Id,
                            ParentId = menu.ParentId.Value,
                            Closeable = menu.Closeable,
                            PageType = RecursionHelper.GetPageType(menu.PageTypeName),
                            Sub = new List<MenuDTO>()
                        });
                    }
                }
                RecursionHelper.AppendChildrenMenu(currentUser.Menus, root);
                currentUser.Menus = root.Sub;
                //await Task.Delay(1000);
                btn.Loading = false;
                this.DialogResult = DialogResult.OK;
                this.Close();
                
            }
            else
            {
                AntdUI.Message.error(this, result.Message);
                btn.Loading = false;
            }
        }
       
        
        Random random = new Random();
        private void button1_Click(object sender, EventArgs e)
        {
            AntdUI.Button btn = (AntdUI.Button)sender;
            btn.LoadingWaveValue = 0;
            if (random.Next(0, 10) > 3)
            {
                btn.Loading = true;
                AntdUI.ITask.Run(() =>
                {
                    Thread.Sleep(1000);
                    for (int i = 0; i < 101; i++)
                    {
                        btn.LoadingWaveValue = i / 100F;
                        Thread.Sleep(20);
                    }
                    Thread.Sleep(2000);
                }, () =>
                {
                    if (btn.IsDisposed) return;
                    btn.Loading = false;
                });
            }
            else
            {
                btn.Loading = true;
                AntdUI.ITask.Run(() =>
                {
                    Thread.Sleep(2000);
                    if (btn.IsDisposed) return;
                    btn.Loading = false;
                });
            }
        }


    }
}
