using Autofac;
using Autofac.Extras.DynamicProxy;
using BusinessManagers.Interfaces;
using BusinessManagers.Managers;
using DataAccess;
using DataAccess.Repositories;
using DataAccess.Repositories.Interfaces;
using DataAccess.Repositories.Managers;

namespace DanalakshmiChitsApi.App_Start
{
    public class AutofacModule : Module
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<ConnectionFactory>().As<IConnectionFactory>();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<UserManager>().As<IUserManager>();
            builder.RegisterType<RepositoryBase>().As<IRepository>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<AdminManger>().As<IAdminManger>();
            builder.RegisterType<AdminRepository>().As<IAdminRepository>();
        }
    }
}
