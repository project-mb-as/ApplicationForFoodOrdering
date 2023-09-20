using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using WebApi.Helpers;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
//using WebApi.Entities;
using Domain.Models;
using WebApi.Models.Users;
using Service;
using System.Linq;
using WebApi.ViewModels;
using System.Threading.Tasks;
using Domain.Enums;
using Newtonsoft.Json.Linq;
using System.Net;

namespace WebApi.Controllers
{
    [Authorize]
    // [ApiController]
    // [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IOrderService _orderService;
        private IMapper _mapper;
        private IEmailService _emailService;
        private readonly AppSettings _appSettings;

        public UsersController(
            IUserService userService,
            IOrderService orderService,
            IMapper mapper,
            IEmailService emailService,
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _orderService = orderService;
            _mapper = mapper;
            _emailService = emailService;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Test()
        {
            return Ok("radi");
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            var user = _userService.Authenticate(model.Email, model.Password);

            if (user == null)
                return BadRequest(new { message = "Email ili lozinka su neispravni." });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var userRoles = user.Roles.Split(",");
            var clames = new List<Claim>();
            foreach (var role in userRoles)
            {
                clames.Add(new Claim(ClaimTypes.Role, role));
            }
            clames.Add(new Claim(ClaimTypes.Name, user.UserId.ToString()));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(clames),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                //Id = user.UserId,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = user.Roles,
                Activated = user.Activated,

                Token = tokenString
            });
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterModel viewModel)
        {
            IActionResult ret;

            if (!ModelState.IsValid)
            {
                ret = ValidationProblem("Greška! Molimo Vas pokušajte ponovo.");
            }
            else
            {
                var user = new User();
                MapUserVMToUser(viewModel, user);
                var isEmailTaken = _userService.GetAll().Any(o => o.Email == user.Email);
                if (isEmailTaken)
                {
                    ret = BadRequest(new { message = "Email zauzet." });
                }
                else
                {
                    var password = GetRandomPassword();

                    _userService.Create(user, password);

                    await SendEmailToNewUser(user, password);
                    ret = Ok(new { message = "Korisnik je uspješno kreiran!" });
                }
            }
            return ret;
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            var viewModel = MapUsersToUsersVM(users.ToList());
            return Ok(viewModel);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);

            var model = _mapper.Map<UserModel>(user);
            return Ok(model);
        }

        [HttpPost]
        public IActionResult UpdatePassword([FromBody] JToken jsonbody)
        {
            IActionResult ret;
            var password = jsonbody.Value<string>("password");

            if (string.IsNullOrEmpty(password) || password.Length < 10)
            {
                ret = ValidationProblem("Greška! Lozinka mora sadržati minimalno 10 karaktera. Molimo Vas pokušajte ponovo.");
            }
            else
            {
                var user = _userService.GetById(Convert.ToInt32(User.Identity.Name));
                user.Activated = true;
                _userService.Update(user, password);
                ret = Ok();
            }
            return ret;
        }

        [HttpGet]
        public IActionResult GetOptions()
        {
            var user = _userService.GetById(Convert.ToInt32(User.Identity.Name));
            var viewModel = new OptionsViewModel();
            MapUserToOptionVM(user, viewModel);

            return Ok(viewModel);
        }

        [HttpPost]
        public IActionResult SetOptions([FromBody] OptionsViewModel viewModel)
        {
            IActionResult ret;
            if (!ModelState.IsValid)
            {
                ret = ValidationProblem("Nevalidan zahtjev!");
            }
            else
            {
                var user = _userService.GetById(Convert.ToInt32(User.Identity.Name));
                user.LocationId = viewModel.LocationId;
                user.TimeId = viewModel.TimeId;
                user.ReceiveOrderConfirmationEmails = viewModel.ReceiveOrderConfirmationEmails;
                user.ReceiveOrderWarningEmails = viewModel.ReceiveOrderWarningEmails;
                _userService.Update(user);
                ret = Ok();
            }
            return ret;
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] JToken jsonbody)
        {
            var id = jsonbody.Value<int>("id");
            var user = _userService.GetById(id);
            user.Activated = false;
            var password = GetRandomPassword();
            _userService.Update(user, password);
            await SendEmailForNewPassword(user, password);
            return Ok(new { message = $"Korisniku {user.Email} je resetovana lozinka." });
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public IActionResult Delete([FromBody] JToken jsonbody)
        {
            var id = jsonbody.Value<int>("id");
            _orderService.DeleteAllForUser(id);
            _userService.Delete(id);
            return Ok();
        }

        private string GetRandomPassword()
        {
            string password = "";
            Random random = new Random();
            while (password.Length < 15)
            {
                password += (char)random.Next(65, 122);
            }
            return password;
        }

        #region mappers
        private void MapUserVMToUser(RegisterModel viewModel, User user)
        {
            user.Email = viewModel.Email.Trim().ToLower();
            user.Roles = viewModel.Roles;
        }

        private void MapUserToUserVM(User user, UserViewModel viewModel)
        {
            viewModel.UserId = user.UserId;
            viewModel.Activated = user.Activated;
            viewModel.Email = user.Email;
            viewModel.Roles = user.Roles;

        }

        private void MapUserToOptionVM(User user, OptionsViewModel viewModel)
        {
            viewModel.LocationId = user.LocationId;
            viewModel.TimeId = user.TimeId;
            viewModel.ReceiveOrderConfirmationEmails = user.ReceiveOrderConfirmationEmails;
            viewModel.ReceiveOrderWarningEmails = user.ReceiveOrderWarningEmails;
        }

        private List<UserViewModel> MapUsersToUsersVM(List<User> users)
        {
            var ret = new List<UserViewModel>();
            foreach (var user in users)
            {
                var viewModel = new UserViewModel();
                MapUserToUserVM(user, viewModel);
                ret.Add(viewModel);
            }
            return ret;
        }
        #endregion

        #region helpers 
        private async Task SendEmailToNewUser(User user, string password)
        {
            var emailBody = $"Poštovani,<br><br> Vama je kreiran nalog na portalu za narudžbu hrane. Da bi ga aktivirali morate se prijaviti " +
                $"na {_appSettings.URL} sa email-om: {user.Email} i lozinkom:<br><b>{password}</b><br>" +
                $"Da bi Vaš nalog postao aktivan, nakon prve prijave promijenite lozinku.<br><br>" +
                $"Srdačan pozdrav i prijatno.<br> ";
            await _emailService.SendEmailToRecipientAsinc(user.Email, "Registracija na portalu za narudžbu hrane", emailBody);
        }

        private async Task SendEmailForNewPassword(User user, string password)
        {
            var emailBody = $"Poštovani,<br><br> Vama je administrator resetovao lozinku. Vaša nova lozika je: <b>{password}</b> <br>" +
                $"Da bi reaktivirali svoj nalog prijavite se na portal i promjenite lozinku.<br><br>" +
                $"Srdačan pozdrav i prijatno.<br><br> ";
            await _emailService.SendEmailToRecipientAsinc(user.Email, "Promjena lozinke na portalu za nardžbu hrane", emailBody);
        }
        #endregion
    }
}
