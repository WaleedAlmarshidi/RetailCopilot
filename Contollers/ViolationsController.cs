using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using RetailCopilot;
using System;
using System.Text.Json;
using RetailCopilot.Data;


namespace RetailCopilot
{
    [ApiController]
    [Route("api/violations")]
    public class ViolationsController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public ViolationsController(ApplicationDbContext dbContext){
            this.dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IResult> Insert([FromBody] Violation violation)
        {
            try{
                dbContext.Violations.Add(violation);
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