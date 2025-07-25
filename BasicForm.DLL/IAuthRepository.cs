using BasicForm.Model;
using BasicForm.Model.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicForm.DAL
{
    public interface IAuthRepository
    {
        Task<UserLoginInfoModel> GetLoginData(AccountInfoDTO dto);
        Task<List<MenuModel>> GetRoleMenuPermission(long userId);
    }
}
