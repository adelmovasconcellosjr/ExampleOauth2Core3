using ExampleOauth2.Domain.Containers;
using ExampleOauth2.Models;
using ExampleOauth2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ExampleOauth2.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromServices] ITokenService service, [FromBody] LoginRequestViewModel request)
        {
            try
            {
                var messages = new List<string>();
                if (string.IsNullOrEmpty(request.UserName))
                {
                    messages.Add("UserName or Password invalid!");
                }
                if (string.IsNullOrEmpty(request.Password))
                {
                    messages.Add("UserName or Password invalid!");
                }

                if (messages.Count > 0)
                {
                    return NotFound(new GenericCommandResult(false, "Error Login", messages));
                }
                var result = service.GenerateToken(request);

                if (result.Equals(""))
                    return Unauthorized();

                return Ok(new GenericCommandResult(true, "Successfully logged in", result));
            }
            catch (Exception ex)
            {
                return Ok(new GenericCommandResult(true, "Error authentication", ex.Message));
            }

        }
    }
}
