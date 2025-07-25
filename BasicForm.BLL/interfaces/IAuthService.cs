using BasicForm.BLL.Base;
using BasicForm.Model;
using BasicForm.Model.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicForm.BLL.interfaces
{
    public interface IAuthService 
    {
        Task<ResponseDTO<UserLoginInfoModel>> LoginAsync(AccountInfoDTO dto);
        Task<bool> RegisterAsync(RegisterDTO registerDTO);
        Task<List<MenuModel>> RoleMenuMap(long userId);
    }
}
