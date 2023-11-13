//using Microsoft.EntityFrameworkCore.Storage;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;

//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Extensions.DependencyInjection;
//using Autofac;
//using DAL.Repository;
//using AutoMapper;
//using API.Dto;

//namespace TestCurd;

//public class ContainerInit
//    {
//        public static IContainer BuildContainer()
//        {
//            var builder = new ContainerBuilder();
           
//            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().SingleInstance();
           


//            // Auto Mapper Configurations
//            var mapperConfig = new MapperConfiguration(mc =>
//            {
//                mc.AddProfile(new MappingProfile());
//            });

//            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
//            config.AssertConfigurationIsValid();

//            IMapper mapper = mapperConfig.CreateMapper();
            

//            builder.RegisterType<Mapper>().As<IMapper>().SingleInstance();

//            var container = builder.Build();
//            return container;
//        }
//    }

