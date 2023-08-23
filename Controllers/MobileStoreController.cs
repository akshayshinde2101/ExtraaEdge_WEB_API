using ExtraaEdge_WEB_API.DataContext;
using ExtraaEdge_WEB_API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ExtraaEdge_WEB_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MobileStoreController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly CollectionContext _ctx;

        public MobileStoreController(IConfiguration config, CollectionContext ctx)
        {
            _configuration = config;
            _ctx = ctx;

        }

        [AllowAnonymous]

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            List<LoginModel> loginModels = _ctx.login.ToList();
            foreach(LoginModel login in loginModels)
            {
                if (login.StoreOwnerName == loginModel.StoreOwnerName && login.Password == loginModel.Password)
                {
                    var token = GenerateToken(loginModel.StoreOwnerName);
                    return Ok(token);
                }
            }
            
            return BadRequest("Login Failed");


        }
        private string GenerateToken(string userName)
        {
             

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userName),
            };
            var token = new JwtSecurityToken(
                _configuration.GetSection("Jwt:Issuer").Value, _configuration.GetSection("Jwt:Audience").Value, claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);

        }


        [HttpGet("TestToken")]

        public IActionResult Test()
        {
            return Ok("Token Validated Successfully");
        }


        [HttpPost("InsertCustomers")]
        public Customer InsertCustomers(Customer c)
        {
            _ctx.customers.Add(c);
            _ctx.SaveChanges();
            return c;
        }

        [HttpGet("ShowAllCustomers")]
        public IActionResult ShowAllCustomers()
        {
            List<Customer> customers = _ctx.customers.ToList();

            return Ok(customers);
        }


        [HttpGet("monthly-sales")]
        public IActionResult GetMonthlySalesReport(DateTime fromDate, DateTime toDate)
        {
            var salesReport = _ctx.sales
                .Where(s => s.SaleDate >= fromDate && s.SaleDate <= toDate)
                .ToList();

            return Ok(salesReport);
        }

       

        [HttpGet("monthly-brand-wise-sales")]
        public IActionResult GetMonthlyBrandWiseSalesReport(DateTime fromDate, DateTime toDate)
        {
            var brandWiseSalesReport = _ctx.brands
                .Include(b => b.Mobiles)
                .Select(brand => new
                {
                    BrandName = brand.BrandName,
                    TotalSales = brand.Mobiles
                        .SelectMany(m => m.Sales)
                        .Where(s => s.SaleDate >= fromDate && s.SaleDate <= toDate)
                        .Sum(s => s.SaleAmount)
                })
                .ToList();

            return Ok(brandWiseSalesReport);
        }

        [HttpGet("monthly-profit")]
        public IActionResult GetMonthlyProfitReport(DateTime fromDate, DateTime toDate)
        {
            var sales = _ctx.sales
                            .Where(s => s.SaleDate >= fromDate && s.SaleDate <= toDate)
                            .ToList();

            decimal totalRevenue = sales.Sum(s => s.SaleAmount); 
            decimal totalCost = sales.Sum(s => s.DiscountApplied);      

            decimal totalProfit = totalRevenue - totalCost;

            return Ok(new { TotalProfit = totalProfit });
        }

        [HttpGet("monthly-loss")]
        public IActionResult GetMonthlyLossReport(DateTime fromDate, DateTime toDate)
        {
            var sales = _ctx.sales
                            .Where(s => s.SaleDate >= fromDate && s.SaleDate <= toDate)
                            .ToList();

            decimal totalRevenue = sales.Sum(s => s.SaleAmount); 
            decimal totalCost = sales.Sum(s => s.DiscountApplied);

            decimal totalLoss = totalCost - totalRevenue;    
            

            if (totalRevenue >= totalCost)
            {
                
                return Ok(new { Message = "No losses occurred." });
            }
            else
            {
                return Ok(new { TotalLoss = totalLoss });
            }
           
        }


    }
}
