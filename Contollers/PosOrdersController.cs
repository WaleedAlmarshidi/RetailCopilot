using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using RetailCopilot;
using System;
using System.Text.Json;
using RetailCopilot.Data;
using Microsoft.AspNetCore.Identity;

namespace RetailCopilot
{
    [ApiController]
    [Route("api/Pos")]
    public class PosOrdersController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public PosOrdersController(ApplicationDbContext dbContext){
            this.dbContext = dbContext;
        }
        [HttpPatch]
        public async Task<IResult> Patch([FromBody] PosSale posSale){
            try {
                var contactWithSamePhone = dbContext.Contacts.Where(c => c.Phone.Contains(posSale.CustomerPhone)).FirstOrDefault();
                posSale.PurchasedProducts = JsonSerializer.Serialize(posSale.PurchasedProductsSerialised);
                if (contactWithSamePhone is not null)
                {
                    if (contactWithSamePhone.LastPosSaleId == posSale.Id)
                        return Results.BadRequest("Duplicated Patch Request, No change");

                    contactWithSamePhone.LastPosSaleId = posSale.Id;
                    contactWithSamePhone.TotalSpentAmount = contactWithSamePhone.TotalSpentAmount is null ? (decimal)posSale.Total : contactWithSamePhone.TotalSpentAmount + (decimal)posSale.Total;
                    contactWithSamePhone.LastVisitDate = posSale.Date;
                    contactWithSamePhone.AverageTicketAmount = contactWithSamePhone.TotalSpentAmount/(contactWithSamePhone.PosOrderCount + contactWithSamePhone.SaleOrderCount);
                    contactWithSamePhone.LastPosSaleId = posSale.Id;
                }
                else{
                    contactWithSamePhone = dbContext.Contacts.Where(c => posSale.CustomerPhone.Contains(c.Phone)).FirstOrDefault();
                    if (contactWithSamePhone is not null)
                    {
                        if (contactWithSamePhone.LastPosSaleId == posSale.Id)
                            return Results.BadRequest("Duplicated Patch Request, No change");

                        contactWithSamePhone.LastPosSaleId = posSale.Id;
                        contactWithSamePhone.TotalSpentAmount += (decimal)posSale.Total;
                        contactWithSamePhone.LastVisitDate = posSale.Date;
                        contactWithSamePhone.AverageTicketAmount = contactWithSamePhone.TotalSpentAmount/(contactWithSamePhone.PosOrderCount + contactWithSamePhone.SaleOrderCount);
                        contactWithSamePhone.LastPosSaleId = posSale.Id;
                    }
                    else
                        if (posSale.CustomerID != -1)
                        {
                            dbContext.Contacts.Add(new Contact{
                                Id = posSale.CustomerID,
                                Name = posSale.CustomerName, 
                                Phone = posSale.CustomerPhone,
                                PosOrderCount = 1,
                                TotalSpentAmount = (decimal)posSale.Total.GetValueOrDefault(),
                                AverageTicketAmount = (decimal)posSale.Total.GetValueOrDefault(),
                                LastPosSaleId = posSale.Id,
                                LastVisitDate = posSale.Date,
                            });
                        }
                }
                var existingPosSale = dbContext.PosSales.Find(posSale.Id);
                if (existingPosSale is null){
                    dbContext.PosSales.Add(posSale);
                    await dbContext.SaveChangesAsync();
                    return Results.BadRequest("No such sale, Added");
                }
                existingPosSale.PosId = posSale.PosId;
                existingPosSale.PurchasedProducts = posSale.PurchasedProducts;
                await dbContext.SaveChangesAsync();
                return Results.Accepted("Contact Updated");
            }
            catch (Exception e){
                return Results.BadRequest($"{e.Message}\n{e.StackTrace}\n{e.InnerException}");
            }
        }
        private void UpdateContact(Contact contact, PosSale posSale)
        {
            contact.PosOrderCount += 1;
            contact.TotalSpentAmount += (decimal)posSale.Total;
            contact.AverageTicketAmount = contact.TotalSpentAmount/(contact.PosOrderCount + contact.SaleOrderCount);
            contact.LastPosSaleId = posSale.Id;
            contact.LastVisitDate = posSale.Date;
            contact.AverageSessionTimeInMinutes = (int)posSale.Date.GetValueOrDefault().Subtract(contact.LastInquiryAt.GetValueOrDefault()).TotalMinutes;
        }
        [HttpPost]
        public async Task<IResult> insert([FromBody] PosSale posSale)
        {
            try{
                if (posSale.Id == 0)
                    return Results.BadRequest();
                if (posSale.CustomerPhone is not null)
                {
                    Console.WriteLine(posSale.CustomerPhone);
                    var contactWithSamePhone = dbContext.Contacts.Where(c => c.Phone.Contains(posSale.CustomerPhone)).SingleOrDefault();
                    posSale.PurchasedProducts = JsonSerializer.Serialize(posSale.PurchasedProductsSerialised);
                    if (contactWithSamePhone is not null)
                    {
                        UpdateContact(contactWithSamePhone, posSale);
                    }
                    else {
                        if (posSale.CustomerID != -1)
                            dbContext.Contacts.Add(new Contact{
                                Id = posSale.CustomerID, 
                                Name = posSale.CustomerName, 
                                Phone = posSale.CustomerPhone,
                                PosOrderCount = 1,
                                TotalSpentAmount = (decimal)posSale.Total,
                                AverageTicketAmount = (decimal)posSale.Total,
                                LastPosSaleId = posSale.Id,
                                LastVisitDate = posSale.Date,
                            });
                    }
                }
                
                dbContext.PosSales.Add(posSale);
                await dbContext.SaveChangesAsync();
                return Results.Created();
            }
            catch (Exception e){
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                }

                return Results.BadRequest(posSale.CustomerPhone + "\n" + e.Message + "\n" + e.StackTrace + "\n" + e.InnerException);
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