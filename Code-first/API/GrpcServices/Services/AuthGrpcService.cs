

using API.Classes;
using Entities;
using Grpc.Core;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using ProtoBuf.Grpc;
using SharedContracts.Model;
using SharedContracts.Interface;

namespace API.GrpcServices.Services;

public class AuthGrpcService : AuthServiceBase
{

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly RoleManager<ApplicationRole> _roleManager;
    public AuthGrpcService(UserManager<ApplicationUser> userManager, IConfiguration configuration, RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _configuration = configuration;
        this._roleManager = roleManager;
    }


    public  async Task<AuthResponse> CheckAuth(AuthRequest request, CallContext context = default)
     => await Login(request);

    private async Task<AuthResponse> Login(AuthRequest request)
    {
        //For First Time
        // Register();

        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user == null) throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid user Credentials"));

        var check = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!check) throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid user Credentials"));

        var userRoles = await _userManager.GetRolesAsync(user);

        var _listClaim = HelperJwt.GetClaim(userRoles, user.UserName, user.Id.ToString());
        var token = HelperJwt.GetToken(_listClaim, _configuration);
        var tokennew = new JwtSecurityTokenHandler().WriteToken(token);
        AuthResponse authenticationResponse = new AuthResponse()
        {
            Token = tokennew,
            Expires = token.ValidTo.Ticks > int.MaxValue ? int.MaxValue : int.Parse(token.ValidTo.Ticks.ToString())
        };


        return authenticationResponse;
    }
    public void Register()
    {



        ApplicationUser user = new()
        {
            Email = "Test@test.com",
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = "test1",
            FirstName = "test",
            LastName = "test",
            Title = "test",
            BirthDate = DateTime.Now,
        };

        var dd = _userManager.CreateAsync(user, "test@Test1").Result; ;

        var identityRole = new ApplicationRole { Name = "PersondataRole" };

        var result = _roleManager.CreateAsync(identityRole).Result;
        var role = _roleManager.FindByNameAsync(identityRole.Name).Result;

        var b = _userManager.AddToRoleAsync(user, role.Name).Result;






    }

}