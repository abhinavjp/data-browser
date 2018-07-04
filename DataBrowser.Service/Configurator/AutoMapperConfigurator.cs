using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataBrowser.Service.Models;
using DataBrowser.Data;

namespace DataBrowser.Service.Configurator
{
    public static class AutoMapperConfigurator
    {

        public static void Configure()
        {
            Mapper.Initialize(cfg => Init(cfg));
        }

        public static void Init(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<ConnectionConfigurationServiceModel, ConnectionConfigurationViewServiceModel>();
            cfg.CreateMap<DatabaseConnection, DataBaseConnectionServiceModel>().ReverseMap();

            cfg.CreateMap<TableConfiguration, TableConfiguratonServiceModel>();
            cfg.CreateMap<TableConfiguratonServiceModel, TableConfiguration>();
            cfg.CreateMap<FieldConfiguration, FieldConfigurationServiceModel>().ReverseMap();
            cfg.CreateMap<User, UserServiceModel>().ReverseMap();
        }
    }
}
