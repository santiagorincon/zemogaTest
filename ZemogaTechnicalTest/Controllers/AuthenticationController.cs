using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZemogaTechnicalTest.DTO;
using ZemogaTechnicalTest.Interfaces;

namespace ZemogaTechnicalTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserInterface _userRepository;

        public AuthenticationController(UserInterface userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        public ActionResult Login(LoginDTO model)
        {
            return Ok(new DefaultResponseDTO
            {
                token = _userRepository.Login(model),
                response = true
            });
        }

        [HttpPost("front", Name ="LoginFront")]
        public ActionResult LoginFront(LoginDTO model)
        {
            return Ok(new DefaultResponseDTO
            {
                token = _userRepository.LoginFront(model),
                response = true
            });
        }
    }
}