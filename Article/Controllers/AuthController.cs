using Article.Application.Services.IAuthServices;
using Article.Domain.MainModels.UserModel;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Article.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "User registratsiya qilish",
            Description = "User registratsiya qilish uchun ma'lumotlarni to'ldiring va bosing"
            )]
        public async Task<IActionResult> SignUp([FromBody] RegisterDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _authService.SignUpService(model);

                    return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Server xatosi", Error = ex.Message });
            }
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Userning tizimga kirishi",
            Description = "Userning tizimga kirishi uchun email va parolingizni kiriting va bosing"
            )]
        public async Task<IActionResult> SignIn()
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Parolni restart qilish",
            Description = "Ro'yhatdan o'tgan emailingiz orqali parolingizni restart qiling"
            )]
        public async Task<IActionResult> UserRessetPassword()
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Kodni tasdiqlash",
            Description = "Emailingizga yuborilgan kodni kiriting va bosing"
            )]
        public async Task<IActionResult> Verification()
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
