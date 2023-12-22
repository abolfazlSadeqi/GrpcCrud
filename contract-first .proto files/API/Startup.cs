
using API.Dto;
using AutoMapper;
using DAL.Contexts;
using DAL.Repository;
using Microsoft.EntityFrameworkCore;
using API.GrpcServices;
using API.Classes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Entities;

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

        services.AddGrpc();

        //add Interceptor
        //services.AddGrpc(options =>
        //{
        //    options.Interceptors.Add<ServerTestInterceptor>();
        //});

        //Add the default identity system configuration for the specified User and Role types.
        services.AddIdentity<ApplicationUser, ApplicationRole>()
                        .AddEntityFrameworkStores<TestCurdContext>()
                        .AddDefaultTokenProviders();

        services.AddDbContext<TestCurdContext>(options =>
        {
            options.UseSqlServer(Configuration.GetConnectionString("TestCurdContextConnection"));
        });


        HelperAuthentication.ConfigureService(services, Configuration);
       

        services.AddAuthorization();

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


        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();


        app.UseEndpoints(endpoints =>
        {

            endpoints.MapGrpcService<PersonGrpcService>();
            endpoints.MapGrpcService<AuthGrpcService>();

        });


    }
}
