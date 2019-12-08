using System;
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
        private readonly AppDbContext Db;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _config;

        public UserController(
            AppDbContext context,
            IConfiguration config,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager
        )
        {
            Db = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        [HttpGet]
        public JsonResult Get()
        {
            var usuariosFilho = Db.Set<UsuarioFilho>()
                .Include(f => f.UsuarioPai)
                .ToArray();

            return new JsonResult(usuariosFilho
                .Select(f => new
                {
                    f.Id,
                    f.NomeCompleto,
                    f.Idade,
                    f.Sexo,
                    f.PhoneNumber,
                    Pai = new
                    {
                        Id = f.UsuarioPaiId,
                        f.UsuarioPai.NomeCompleto,
                        f.UsuarioPai.PhoneNumber,
                        f.UsuarioPai.Cpf
                    }
                })
            );
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel userLogin)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userLogin.PhoneNumber);

                if (user != null)
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(user, userLogin.Password, false);

                    if (result.Succeeded)
                    {
                        var appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber.ToUpper() == userLogin.PhoneNumber);
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
                user.NomeCompleto,
                user.Email,
                // TipoUsuario = (user is UsuarioPaciente) ? 0 : (user is UsuarioProfissional) ? 1 : 2
            };
        }
    }
}
