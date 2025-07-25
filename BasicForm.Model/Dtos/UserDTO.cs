using BasicForm.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicForm.Model.Dtos
{
    public class UserDTO:BindableBase
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        private string _loginName;
        public string LoginName
        {
            get => _loginName;
            set => SetProperty(ref _loginName, value);
        }

        private string _password;

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
    }
}
