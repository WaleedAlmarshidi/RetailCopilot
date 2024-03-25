using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using RetailCopilot;
using System;
using System.Text.Json;
using RetailCopilot.Data;


namespace RetailCopilot
{
    [ApiController]
    [Route("api/shopvisits")]
    public class ShopVisitCountController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public ShopVisitCountController(ApplicationDbContext dbContext){
            this.dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IResult> Insert([FromBody] ShopVisitCount shopVisitCount)
        {
            try{
                dbContext.ShopVisitsCount.Add(shopVisitCount);
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