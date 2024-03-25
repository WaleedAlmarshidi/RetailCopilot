// using Microsoft.AspNetCore.Mvc;
// using System.Collections.Generic;
// using RetailCopilot;
// using System;
// using System.Text.Json;
// using RetailCopilot.Data;
// using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

// namespace RetailCopilot
// {
//     [ApiController]
//     [Route("api/Contacts")]
//     internal class UsersController : Controller
//     {
//         private readonly ApplicationDbContext dbContext;
//         private readonly UserStore userStore;
//         public UsersController(ApplicationDbContext dbContext, UserStore userStore){
//             this.dbContext = dbContext;
//             this.userStore = userStore;
//         }

//         // action methods go here
//         // GET api/Products
//         [HttpGet]
//         public IActionResult GetAll()
//         {
//             return Ok("hi");
//         }
//         [HttpPost]
//         public IActionResult newUser()
//         {
//             var user = CreateUser();

//             await userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
//             var emailStore = GetEmailStore();
//             await emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
//             var result = await UserManager.CreateAsync(user, Input.Password);

//             if (!result.Succeeded)
//             {
//                 identityErrors = result.Errors;
//                 return;
//             }
//         }
//         private ApplicationUser CreateUser()
//         {
//             try
//             {
//                 return Activator.CreateInstance<ApplicationUser>();
//             }
//             catch
//             {
//                 throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
//                     $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor.");
//             }
//         }
//     }
// }