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
            #region ���q�`�J
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);

            //var configuration = new ConfigurationBuilder()
            //    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            //    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //    .Build();

            //var services = new ServiceCollection();
            //ConfigureServices(services, configuration);

            //var builder = new ContainerBuilder();

            //// �N Microsoft DI �� ServiceCollection �浹 Autofac �޲z
            //builder.Populate(services);

            //// ���y BLL ���� Service ����
            //var bllAssembly = Assembly.Load("BasicForm.BLL");
            //builder.RegisterAssemblyTypes(bllAssembly)
            //       .Where(t => t.Name.EndsWith("Service"))
            //       .AsImplementedInterfaces()
            //       .InstancePerLifetimeScope();

            //// ���y DAL ���� Repository ����
            //var dalAssembly = Assembly.Load("BasicForm.DAL");
            //builder.RegisterAssemblyTypes(dalAssembly)
            //       .Where(t => t.Name.EndsWith("Repository"))
            //       .AsImplementedInterfaces()
            //       .InstancePerLifetimeScope();

            //// ���y�õ��U WinForms ���
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
            #endregion ���q�`�J

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //�ѱKconnectionStr
            string connStr = "";
            string encryptConn = EnvFileHelper.LoadEncryptedConnection() ?? "";
            if (!string.IsNullOrEmpty(encryptConn)) 
            {
                connStr = EnvFileHelper.DecryptString(encryptConn);
            }
            if(string.IsNullOrEmpty(encryptConn) || string.IsNullOrEmpty(connStr))
            {
                MessageBox.Show("�Ұʥ���!�i��ʤֳs�u��T�εL�v��");
                return;
            }

            var service = new ServiceCollection();
            //�`�JEF DB
            service.AddDbContext<AppDbContext>(options =>
                       options.UseSqlServer(connStr));

            service.AddSingleton<IAuthService, AuthService>();
            service.AddSingleton<IAuthRepository, AuthRepository>();
            //��W�`�J�n�J�M���U��dto�M����
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
                // �����n�J
                var currentUserText = new CurrentUserContextDTO
                {
                    Id = loginForm.currentUser.Id,
                    UserName = loginForm.currentUser.UserName,
                    Menus = loginForm.currentUser.Menus,
                };

                // �إߥ��� Autofac �e��
                var fullContainer = BuildFullContainer(currentUserText, connStr);

                var fullScope = fullContainer.BeginLifetimeScope();

                var mainForm = fullScope.Resolve<Form1>();

                // �ҰʥD�e��
                Application.Run(mainForm);

                // �b�D�e��������A����e��
                fullScope.Dispose();
            }

        }
        static IContainer BuildFullContainer(CurrentUserContextDTO currentUserText,string connStr)
        {

            var services = new ServiceCollection();
            //���O�e������ �A���J
            services.AddDbContext<AppDbContext>(options =>
                       options.UseSqlServer(connStr));

            var builder = new ContainerBuilder();

            // �N Microsoft DI �� ServiceCollection �浹 Autofac �޲z
            builder.Populate(services);

            // ���y BLL ���� Service ����
            var bllAssembly = Assembly.Load("BasicForm.BLL");
            builder.RegisterAssemblyTypes(bllAssembly)
                   .Where(t => t.Name.EndsWith("Service"))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            // ���y DAL ���� Repository ����
            var dalAssembly = Assembly.Load("BasicForm.DAL");
            builder.RegisterAssemblyTypes(dalAssembly)
                   .Where(t => t.Name.EndsWith("Repository"))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            // ���U�x�� Repository & Service
            builder.RegisterGeneric(typeof(BaseRepository<>))
                   .As(typeof(IBaseRepository<>))
                   .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(BaseService<>))
                   .As(typeof(IBaseService<>))
                   .InstancePerLifetimeScope();

            // ���y Model�]DTO / ViewModel�^
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
            // �`�J�ϥΪ̤W�U�嬰���
            builder.RegisterInstance(currentUserText).As<CurrentUserContextDTO>().SingleInstance();

            // ���y�õ��U WinForms ���
            var uiTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t =>!t.IsAbstract && typeof(Control).IsAssignableFrom(t)); // �䴩 Form�BUserControl�BPanel ��

            foreach (var uiType in uiTypes)
            {
                builder.RegisterType(uiType).AsSelf().InstancePerDependency(); // ���n SingleInstance�A�קK�e���@�Ϊ��A���D
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