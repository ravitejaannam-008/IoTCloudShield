using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.Db;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : Controller
{
    IConfiguration _config;

    public UsersController(IConfiguration config)
    {
        _config = config;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        AppDb _db = new AppDb(_config["ConnectionStrings:DefaultConnection"]);
        await _db.Connection.OpenAsync();

        using var cmd = _db.Connection.CreateCommand();
        var sql = "SELECT * FROM user";
        cmd.CommandText = sql;
        var reader = await cmd.ExecuteReaderAsync();

        var users = new List<User>();

        using(reader)
        {
            while(await reader.ReadAsync())
            {
                var user = new User();
                user.Id = reader.GetInt32("id");
                user.Username = reader.GetString("username");
                user.Name = reader.GetString("name");
                user.Email = reader.GetString("email");
//                user.Password = reader.GetString("password");
                users.Add(user);
            }
        }

        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginData login)
    {
        AppDb _db = new AppDb(_config["ConnectionStrings:DefaultConnection"]);
        await _db.Connection.OpenAsync();

        using var cmd = _db.Connection.CreateCommand();
        var sql = "SELECT * FROM user WHERE username='" + login.Username +
                        "' AND password='" + login.Password + "'";
        Console.WriteLine(sql);
        cmd.CommandText = sql;
        var reader = await cmd.ExecuteReaderAsync();
        var user = new User();

        if (reader.HasRows)
        {
            using(reader)
            {
                await reader.ReadAsync();
                user.Id = reader.GetInt32("id");
                user.Username = reader.GetString("username");
                user.Name = reader.GetString("name");
                user.Email = reader.GetString("email");
        //            user.Password = reader.GetString("password");
            }

            var authClaim = new List<Claim>
            {
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, "ADMIN")
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                expires: DateTime.Now.AddHours(5),
                claims: authClaim,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            user.Token = new JwtSecurityTokenHandler().WriteToken(token);


            
        }


        return Ok(user);
    }

}