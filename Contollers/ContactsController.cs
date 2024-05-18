using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using RetailCopilot;
using System;
using System.Text.Json;
using RetailCopilot.Data;


namespace RetailCopilot
{
    [ApiController]
    [Route("api/Contacts")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public ProductsController(ApplicationDbContext dbContext){
            this.dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IResult> Upsert([FromBody] Contact contact)
        {
            try{
                if (contact.Phone is null)
                    return Results.BadRequest();
                var contactWithSamePhone = dbContext.Contacts.Where(c => c.Phone.Trim().Contains(contact.Phone)).FirstOrDefault();
                if (contactWithSamePhone is not null)
                {
                    contactWithSamePhone.Name = contact.Name;
                    contactWithSamePhone.Phone = contact.Phone;
                    contactWithSamePhone.Email = contact.Email;
                    contactWithSamePhone.Country = contact.Country;
                    await dbContext.SaveChangesAsync();
                    return Results.Accepted("Contact Updated.");
                }
                dbContext.Contacts.Add(contact);
                await dbContext.SaveChangesAsync();
                return Results.Created();
            }
            catch {
                return Results.BadRequest("bad");
            }
        }
        // action methods go here
            // GET api/Products
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok("hi");
        }
    }
}