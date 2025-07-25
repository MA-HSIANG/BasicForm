using BasicForm.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicForm.Model.Dtos
{
    public class RegisterDTO :BindableBase
    {
		private string _registerLoginName;

		public string RegisterLoginName
		{
			get => _registerLoginName;
			set => SetProperty(ref _registerLoginName, value);
		}
        private string _registerPwd;

        public string RegisterPwd
        {
            get => _registerPwd;
            set => SetProperty(ref _registerPwd, value);
        }
        private string _registerName;

        public string RegisterName
        {
            get => _registerName;
            set => SetProperty(ref _registerName, value);
        }
    }
}
