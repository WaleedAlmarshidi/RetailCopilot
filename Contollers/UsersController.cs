using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using RetailCopilot;
using System;
using System.Text.Json;
using RetailCopilot.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.EntityFrameworkCore;

public class RegisterUserInputModel
{
    [Required]
    public string UserName { get; set; } = string.Empty;
    // [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    public string ExternalId { get; set; }

    // Add other properties as needed
}
public class UserWithFullNameModel
{
    public string ExternalId { get; set; }
    public string FullName { get; set; }
}
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IUserEmailStore<ApplicationUser> _emailStore;
    private readonly IUserStore<ApplicationUser> _userStore;
    private readonly ILogger<UsersController> _logger;

    public UsersController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IUserStore<ApplicationUser> userStore,
        ILogger<UsersController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userStore = userStore;
        _emailStore = GetEmailStore();
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserInputModel input)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = CreateUser(input);
        // if (string.IsNullOrEmpty(input.Email))
        //     if(string.input.UserName.All)
        //     {
        //         await _userStore.SetUserNameAsync(user, input.UserName, CancellationToken.None);
        //     }
        //     catch
        //     {
        //         await _userStore.SetUserNameAsync(user, input.ExternalId, CancellationToken.None);
        //     }
        // else
        //     await _userStore.SetUserNameAsync(user, input.Email, CancellationToken.None);
        await _userStore.SetUserNameAsync(user, input.UserName, CancellationToken.None);
        await _emailStore.SetEmailAsync(user, input.Email, CancellationToken.None);

        var result = await _userManager.CreateAsync(user, input.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        _logger.LogInformation("User created a new account with password.");
        
        return Ok();
    }
    [HttpPatch("fullname")]
    public async Task<IActionResult> PathFullNameUser([FromBody] UserWithFullNameModel input)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.Users.Where(u => u.UserName == input.ExternalId).FirstOrDefaultAsync();

        if (user is null)
        {
            return NotFound();
        }
        user.FullName = input.FullName;
        await _userManager.UpdateAsync(user);
        _logger.LogInformation("User created a new account with password.");
        
        return Ok();
    }

    private ApplicationUser CreateUser(RegisterUserInputModel input)
    {
        try
        {
            return new ApplicationUser
            {
                FullName = input.UserName,
                UserName = input.Email ?? input.UserName,
                ExternalId = input.ExternalId,
                Email = input.Email,
                // Initialize other properties as needed
            };
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor.");
        }
    }

    private IUserEmailStore<ApplicationUser> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<ApplicationUser>)_userStore;
    }
}