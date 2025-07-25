using BasicForm.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicForm.Common.DB.DbContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
             : base(options)
        {

        }
        public DbSet<UserLoginInfoModel> UserLoginInfoEntity { get; set; }
        public DbSet<MenuModel> MenuEntity {  get; set; }
        public DbSet<RoleModel> RoleEntity { get; set; }
        public DbSet<UserRoleModel> UserRoleEntity { get; set; }
        public DbSet<RoleMenuPermissionModel> RoleMenuPermissionEntity {  get; set; }   
    }

}
