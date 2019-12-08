﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Safety4Children.Entities;
using Safety4Children.Repository;
using Safety4Children.Repository.IdentityEntities;
using Safety4Children.ViewModels.Usuario;

namespace Safety4Children.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly AppDbContext Db;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _config;

        public UserController(
            ILogger<WeatherForecastController> logger,
            AppDbContext context,
            IConfiguration config,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager
        )
        {
            _logger = logger;
            Db = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        [HttpGet]
        public IEnumerable<UsuarioPai> Get()
        {
            var usuariosPai = Db.Set<UsuarioPai>().ToArray();
            return usuariosPai;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel userLogin)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(userLogin.Email);

                if (user != null)
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(user, userLogin.Password, false);

                    if (result.Succeeded)
                    {
                        var appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Email.ToUpper() == userLogin.Email.ToUpper());
                        return Ok(GenerateJWToken(appUser).Result);
                    }
                }

                return Ok(new
                {
                    sucesso = false,
                    Erro = "Senha inválida ou usuário não existe"
                });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Banco Dados Falhou {e.Message}.");
            }
        }

        private async Task<object> GenerateJWToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new
            {
                sucesso = true,
                token = tokenHandler.WriteToken(token),
                tokenDescriptor.Expires,
                (user as UsuarioPai).Nome,
                user.Email,
                // TipoUsuario = (user is UsuarioPaciente) ? 0 : (user is UsuarioProfissional) ? 1 : 2
            };
        }
    }
}