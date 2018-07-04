using DataBrowser.Service.Interface;
using DataBrowser.Service.Services;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrowser.Service.Configurator
{
    public static class StructureMapperConfigurator
    {
        private static Container container = new Container();
        private static bool isInitialized = false;

        private static void Configure()
        {
            container.Configure(x =>
            {
                x.For<IDataBaseConnectionService>().Use<DataBaseConnectionService>();
                x.For<ITableConfigurationService>().Use<TableConfigurationService>();
                x.For<IDataBrowserService>().Use<DataBrowserService>();
                x.For<IUserService>().Use<UserService>();
            });
            isInitialized = true;
        }

        public static T GetInstance<T>()
        {
            if (!isInitialized)
            {
                Configure();
            }
            return container.GetInstance<T>();
        }
    }
}
