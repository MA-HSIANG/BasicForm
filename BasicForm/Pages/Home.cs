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

namespace BasicForm.Pages
{
    public partial class Home : UserControl
    {
        private readonly IBaseService<UserLoginInfoModel> _userService;
        private readonly IBaseService<UserRoleModel> _userRoleService;
        private readonly IBaseService<RoleModel> _roleService;
        public Home(IBaseService<UserLoginInfoModel> userService, IBaseService<UserRoleModel> userRoleService, IBaseService<RoleModel> roleService)
        {
            _userService = userService;
            _userRoleService = userRoleService;
            _roleService = roleService;

            InitializeComponent();
            OnLoding();
        }

        private async void OnLoding()
        {
            var roles = await _roleService.GetAllAsync();
            var theUserRole = roles.Where(x=>!x.Code.Contains("Admin")).Distinct().ToList();
            var theAdminRole = roles.Where(x => x.Code.Contains("Admin")).Distinct().ToList();


            txtUser.Text = theUserRole.Count.ToString();
            txtAdmin.Text = theAdminRole.Count.ToString();


        }    
    }
}
