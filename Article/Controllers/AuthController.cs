﻿using Article.Application.Services.IAuthServices;
using Article.Domain.MainModels.UserModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

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
        public async Task<IActionResult> SignIn([FromBody] SignInDTO signInDTO)
        {
            try
            {
                var result = await _authService.SignInService(signInDTO);

                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Server xatosi", Error = ex.Message });
            }
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Kodni tasdiqlash",
            Description = "Emailingiz, yuborilgan kodni kiriting va bosing"
            )]
        public async Task<IActionResult> VerificationCode([FromBody] VerificationCodeDTO verificationCodeDTO)
        {
            try
            {
                var result = await _authService.VerificationCodeService(verificationCodeDTO);

                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Server xatosi", Error = ex.Message });
            }
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Parolni unutdingizmi?",
            Description = "Ro'yhatdan o'tgan emailingiz orqali parolingizni almashtiring"
            )]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO forgotPasswordDTO)
        {
            try
            {
                var result = await _authService.ForgotPasswordService(forgotPasswordDTO);

                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Parolni yangilash?",
            Description = "Token, yangi parolingizni kiriting va parolingizni almashtiring"
            )]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDTO)
        {
            try
            {
                var result = await _authService.ResetPasswordService(resetPasswordDTO);

                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Token yangilash",
            Description = "Refresh token kiriting va bosing"
            )]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO refreshToken)
        {
            try
            {
                var result = await _authService.RefreshTokenService(refreshToken);       

                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Server xatosi", Error = ex.Message });
            }
        }
    }
}
