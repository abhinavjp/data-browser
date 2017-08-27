using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataBrowser.Service.Models;

namespace DataBrowser.Service.Configurator
{
    public static class AutoMapperConfigurator
    {
        private static bool _isInitialized = false;

        public static void Configure()
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<ConnectionConfigurationServiceModel, ConnectionConfigurationViewServiceModel>();

            });
        }
    }
}
