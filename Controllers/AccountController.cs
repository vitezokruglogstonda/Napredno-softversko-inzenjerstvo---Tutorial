using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Tutorial.Attributes;
using Tutorial.Exceptions;
using Tutorial.Models.Database;
using Tutorial.Models.Requests;
using Tutorial.Models.Response;
using Tutorial.Services.AccountService;

namespace Tutorial.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService AccountService)
        {
            _accountService = AccountService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<UserResponse>> Register([FromForm] string jsonDto)
        {
            try
            {
                RegisterRequest? userDto = JsonConvert.DeserializeObject<RegisterRequest>(jsonDto) ?? throw new CustomException(400, "Incorrect user information.");
                return Ok(await _accountService.AddNewUser(userDto, HttpContext));
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPut]
        [Route("log-in")]
        public async Task<ActionResult<UserResponse>> LogIn([FromBody] LogInRequest loginRequest)
        {
            try
            {
                if (loginRequest == null || String.IsNullOrEmpty(loginRequest.Email) || String.IsNullOrEmpty(loginRequest.Password))
                    throw new CustomException(400, "Incorrect user information.");
                return Ok(await _accountService.TryLogUserIn(loginRequest, HttpContext));
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [Auth]
        [HttpGet]
        [Route("log-out")]
        public async Task<ActionResult<bool>> LogOut()
        {
            try
            {
                return Ok(await _accountService.LogUserOut((HttpContext.Items["User"] as User)!));
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet]
        [Route("create-admin")]
        public async Task<IActionResult> CreateAdmin()
        {
            try
            {
                await _accountService.CreateAdmin();
                return Ok(new {message = "Admin successfully created." });
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
