using BasicForm.Common.DB.DbContexts;
using BasicForm.Common.Heplers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicForm.Common.DB.EF
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            try
            {
                string connStr = "";
                string encryptConn = EnvFileHelper.LoadEncryptedConnection() ?? "";
                if (!string.IsNullOrEmpty(encryptConn))
                {
                    connStr = EnvFileHelper.DecryptString(encryptConn);
                }
                else
                {
                    Environment.Exit(0);
                }
              
                optionsBuilder.UseSqlServer(
                    connStr
                );
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex);
            }
           

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
