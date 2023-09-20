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
using WebApi.Entities;
using WebApi.Models.Users;
using Service;
using System.Linq;

namespace WebApi.Controllers
{
    [Authorize]
    //[ApiController]
    //[Route("[controller]")]
    public class BookController : ControllerBase
    {
        private IUserService _userService;
       
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public BookController(
            IUserService userService,
            
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
         
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Test()
        {
            return Ok("radi");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll()
        {
            

            return Ok();
        }

    }
}
