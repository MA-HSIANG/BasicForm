using BasicForm.BLL.interfaces;
using BasicForm.DAL;
using BasicForm.DAL.Base;
using BasicForm.Model;
using BasicForm.Model.Dtos;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicForm.BLL
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        public AuthService(IAuthRepository authRepository) 
        { 
            _authRepository = authRepository;
        }
        public async Task<ResponseDTO<UserLoginInfoModel>> LoginAsync(AccountInfoDTO dto)
        {
            var result = new ResponseDTO<UserLoginInfoModel>();
            result.Data = await _authRepository.GetLoginData(dto);

            if (result.Data != null) 
            {
                result.IsSuccess = true;
                result.Message = "登入成功";
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "登入失敗";
            }

            return result;
        }
        public Task<bool> RegisterAsync(RegisterDTO registerDTO)
        {
            if(string.IsNullOrEmpty(registerDTO.RegisterLoginName) || string.IsNullOrEmpty(registerDTO.RegisterPwd) || string.IsNullOrEmpty(registerDTO.RegisterName))
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
        public async Task<List<MenuModel>> RoleMenuMap(long userId)
        {

            var menuModels = await _authRepository.GetRoleMenuPermission(userId);

            return menuModels;
        }
    }
}
