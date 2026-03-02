using System.ComponentModel.DataAnnotations;
using Coffer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<AuthController> _logger;
    public AuthController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ILogger<AuthController> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    public class LoginDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
    }
    // TODO: add ability to sign in with username
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user is null || user.UserName is null)
        {
            _logger.LogWarning("Could not find user with email: {Email}", dto.Email);
            return Unauthorized();
        }
        
        var result = await _signInManager.PasswordSignInAsync(user.UserName, dto.Password, dto.RememberMe, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            _logger.LogInformation("{User} signed in.", user.UserName);
            return Ok();
        }

        _logger.LogWarning("{User} was not signed in, LockedOut: {IsLockedOut}, NotAllowed: {IsNotAllowed}, Require2FA: {Require2FA}", user.UserName, result.IsLockedOut, result.IsNotAllowed, result.RequiresTwoFactor);
        return Unauthorized();
    }
}