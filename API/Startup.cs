
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using API.Dto;
using AutoMapper;
using DAL.Contexts;
using DAL.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using API.GrpcServices;

namespace API;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddHttpContextAccessor();


        services.AddGrpc();

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();



        // Add services to the container.
        services.AddDbContext<TestCurdContext>(options =>
        {
            options.UseSqlServer( Configuration.GetConnectionString("TestCurdContextConnection"));
        });


        services.AddTransient<IUnitOfWork, UnitOfWork>();


        // Auto Mapper Configurations
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
        });

        IMapper mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);



    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }


        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        
        //app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseSwagger();


        app.UseSwagger();
        //app.UseSwaggerUI();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            c.RoutePrefix = "";
        });
        
        app.UseEndpoints(endpoints =>
        {
           // endpoints.MapControllers();
           // endpoints.MapGrpcService<CalculatorGrpcService>();
            endpoints.MapGrpcService<PersonGrpcService>();

        });
        // Configure the HTTP request pipeline.

    }
}
