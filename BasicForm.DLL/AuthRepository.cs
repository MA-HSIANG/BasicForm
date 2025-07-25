using BasicForm.Common.DB.DbContexts;
using BasicForm.Model;
using BasicForm.Model.Dtos;
using Microsoft.EntityFrameworkCore;
using System;

namespace BasicForm.DAL
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _dbContext;
        public AuthRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserLoginInfoModel> GetLoginData(AccountInfoDTO dto)
        {
            UserLoginInfoModel result = new();

            result = await _dbContext.UserLoginInfoEntity.FirstOrDefaultAsync(x => x.LoginName == dto.LoginName && x.Password == dto.Password);
           
            return result;

        }
        public async Task<List<MenuModel>> GetRoleMenuPermission(long userId)
        {
                        var menuList = await (
                                    from rmp in _dbContext.RoleMenuPermissionEntity
                                    join menu in _dbContext.MenuEntity on rmp.MenuId equals menu.Id
                                    join ur in _dbContext.UserRoleEntity on rmp.RoleId equals ur.RoleId
                                    where ur.UserId == userId
                                    select new MenuModel
                                    {
                                        Id = menu.Id,
                                        ParentId = menu.ParentId,
                                        Code = menu.Code,
                                        Name = menu.Name,
                                        IconSvg = menu.IconSvg,
                                        PageTypeName = menu.PageTypeName,
                                        Closeable = menu.Closeable,
                                    })
                                    .Distinct()
                                    .ToListAsync();

            return menuList;

        }
    }
}
