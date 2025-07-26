using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using BasicForm.BLL;
using BasicForm.BLL.Base;
using BasicForm.BLL.interfaces;
using BasicForm.Common.DB.DbContexts;
using BasicForm.Common.Helpers;
using BasicForm.Common.Settings;
using BasicForm.Controls;
using BasicForm.DAL;
using BasicForm.DAL.Base;
using BasicForm.Model;
using BasicForm.Model.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;


namespace BasicForm
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            #region 普通注入
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);

            //var configuration = new ConfigurationBuilder()
            //    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            //    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //    .Build();

            //var services = new ServiceCollection();
            //ConfigureServices(services, configuration);

            //var builder = new ContainerBuilder();

            //// 將 Microsoft DI 的 ServiceCollection 交給 Autofac 管理
            //builder.Populate(services);

            //// 掃描 BLL 中的 Service 類型
            //var bllAssembly = Assembly.Load("BasicForm.BLL");
            //builder.RegisterAssemblyTypes(bllAssembly)
            //       .Where(t => t.Name.EndsWith("Service"))
            //       .AsImplementedInterfaces()
            //       .InstancePerLifetimeScope();

            //// 掃描 DAL 中的 Repository 類型
            //var dalAssembly = Assembly.Load("BasicForm.DAL");
            //builder.RegisterAssemblyTypes(dalAssembly)
            //       .Where(t => t.Name.EndsWith("Repository"))
            //       .AsImplementedInterfaces()
            //       .InstancePerLifetimeScope();

            //// 掃描並註冊 WinForms 表單
            //var formTypes = Assembly.GetExecutingAssembly()
            //    .GetTypes()
            //    .Where(t => t.IsSubclassOf(typeof(Form)) && !t.IsAbstract);
            //foreach (var formType in formTypes)
            //{
            //    builder.RegisterType(formType).AsSelf().SingleInstance();
            //}

            //var container = builder.Build();

            //using var scope = container.BeginLifetimeScope();
            //var form = scope.Resolve<Form1>();


            //Application.Run(form);
            #endregion 普通注入

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //解密connectionStr
            string connStr = "";
            string encryptConn = EnvFileHelper.LoadEncryptedConnection() ?? "";
            if (!string.IsNullOrEmpty(encryptConn)) 
            {
                connStr = EnvFileHelper.DecryptString(encryptConn);
            }
            if(string.IsNullOrEmpty(encryptConn) || string.IsNullOrEmpty(connStr))
            {
                MessageBox.Show("啟動失敗!可能缺少連線資訊或無權限");
                return;
            }

            var service = new ServiceCollection();
            //注入EF DB
            service.AddDbContext<AppDbContext>(options =>
                       options.UseSqlServer(connStr));

            service.AddSingleton<IAuthService, AuthService>();
            service.AddSingleton<IAuthRepository, AuthRepository>();
            //單獨注入登入和註冊的dto和實體
            service.AddSingleton<UserLoginInfoModel>();
            service.AddSingleton<AccountInfoDTO>();
            service.AddSingleton<RegisterDTO>();

            service.AddSingleton<LoginForm>();


            var tempContainerBuilder = new ContainerBuilder();
            tempContainerBuilder.Populate(service);
            var tempContainer = tempContainerBuilder.Build();
         

            using var scope = tempContainer.BeginLifetimeScope();
       
            var loginForm = scope.Resolve<LoginForm>();

            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                // 紀錄登入
                var currentUserText = new CurrentUserContextDTO
                {
                    Id = loginForm.currentUser.Id,
                    UserName = loginForm.currentUser.UserName,
                    Menus = loginForm.currentUser.Menus,
                };

                // 建立正式 Autofac 容器
                var fullContainer = BuildFullContainer(currentUserText, connStr);

                var fullScope = fullContainer.BeginLifetimeScope();

                var mainForm = fullScope.Resolve<Form1>();

                // 啟動主畫面
                Application.Run(mainForm);

                // 在主畫面關閉後再釋放容器
                fullScope.Dispose();
            }

        }
        static IContainer BuildFullContainer(CurrentUserContextDTO currentUserText,string connStr)
        {

            var services = new ServiceCollection();
            //正是容器產生 再次入
            services.AddDbContext<AppDbContext>(options =>
                       options.UseSqlServer(connStr));

            var builder = new ContainerBuilder();

            // 將 Microsoft DI 的 ServiceCollection 交給 Autofac 管理
            builder.Populate(services);

            // 掃描 BLL 中的 Service 類型
            var bllAssembly = Assembly.Load("BasicForm.BLL");
            builder.RegisterAssemblyTypes(bllAssembly)
                   .Where(t => t.Name.EndsWith("Service"))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            // 掃描 DAL 中的 Repository 類型
            var dalAssembly = Assembly.Load("BasicForm.DAL");
            builder.RegisterAssemblyTypes(dalAssembly)
                   .Where(t => t.Name.EndsWith("Repository"))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            // 註冊泛型 Repository & Service
            builder.RegisterGeneric(typeof(BaseRepository<>))
                   .As(typeof(IBaseRepository<>))
                   .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(BaseService<>))
                   .As(typeof(IBaseService<>))
                   .InstancePerLifetimeScope();

            // 掃描 Model（DTO / ViewModel）
            var modelAssembly = Assembly.Load("BasicForm.Model");
            builder.RegisterAssemblyTypes(modelAssembly)
                .Where(t => t.Name.EndsWith("Dto") || t.Name.EndsWith("Model"))
                .AsSelf()
                .InstancePerDependency();

            var basePagesAssembly = Assembly.Load("BasicForm");

            builder.RegisterAssemblyTypes(basePagesAssembly)
                .Where(t =>
                    typeof(UserControl).IsAssignableFrom(t) &&
                    !t.IsAbstract &&
                    t.Namespace != null &&
                    t.Namespace.StartsWith("BasicForm.Pages"))
                .AsSelf()
                .InstancePerDependency();
            // 注入使用者上下文為單例
            builder.RegisterInstance(currentUserText).As<CurrentUserContextDTO>().SingleInstance();

            // 掃描並註冊 WinForms 表單
            var uiTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t =>!t.IsAbstract && typeof(Control).IsAssignableFrom(t)); // 支援 Form、UserControl、Panel 等

            foreach (var uiType in uiTypes)
            {
                builder.RegisterType(uiType).AsSelf().InstancePerDependency(); // 不要 SingleInstance，避免畫面共用狀態問題
            }

            return builder.Build();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration);

            services.Configure<ConnectionStrings>(configuration.GetSection("ConnectionStrings"));
            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
        }
    }
}