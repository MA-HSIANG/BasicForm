using AntdUI;
using Autofac;
using BasicForm.Controls;
using BasicForm.Model;
using BasicForm.Model.Dtos;
using System.Drawing.Printing;

namespace BasicForm
{
    public partial class Form1 : AntdUI.Window
    {
        private readonly MenuBarControl _menuBarControl;
        private readonly CurrentUserContextDTO _userContext;
        private readonly ILifetimeScope _scope;
        public Form1(CurrentUserContextDTO userContext, ILifetimeScope scope)
        {

            InitializeComponent();

            _scope = scope;
            _userContext = userContext;


            Text = Common.App.Settings.AppName;
            headerBar.Text = Common.App.Settings.AppName;
            headerBar.SubText =$"當前使用者: {userContext.UserName}";
            headerBar.SubGap = 10;


            _menuBarControl = _scope.Resolve<MenuBarControl>();
            _menuBarControl.Dock = DockStyle.Fill;

            mainPanel.Controls.Add(_menuBarControl);
            InitHamburgerMenuTabs();
        }
        private void InitHamburgerMenuTabs()
        {
            
            _menuBarControl.SetMenu(_userContext.Menus);
            _menuBarControl.MenuSelectDefault();

        }

    }
}
