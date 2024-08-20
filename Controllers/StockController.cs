using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    //Used for processing requests and giving responses in json format
    public class StockController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IStockRepository _stockRepo;
        //private readonly UserManager<AppUser> _userManager;
        public StockController(ApplicationDbContext context, IStockRepository stockRepo, UserManager<AppUser> userManager)
        {
            _stockRepo = stockRepo;
            _context = context;
           // _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var stocks = await _stockRepo.GetAllAsync(query);
            //selects Stock and uses mapper to convert to stockdto
            var stockDto = stocks.Select(s => s.ToStockDto()).ToList();
            //var stockDto = stocks.Select(s => s.ToStockDto());
            return Ok(stockDto);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var stock = await _context.Stocks.FindAsync(id);
            //var stock = await _stockRepo.GetByIdAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var stockModel = stockDto.ToStockFromCreateDTO();
            //await _context.Stocks.AddAsync(stockModel);
            //await _context.SaveChangesAsync();
            await _stockRepo.CreateAsync(stockModel);
            
            //returns status201created
            //shows new page with created data in stockdto format
            return CreatedAtAction(nameof(GetById), new {id = stockModel.Id}, stockModel.ToStockDto());
        }
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto update)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //gets stock from the database
            //var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            var stockModel = await _stockRepo.UpdateAsync(id, update);
            if(stockModel == null)
            {
                return NotFound();
            }
            //updates stock data
            

           await _context.SaveChangesAsync();
            //returns user format json with ok status code
            return Ok(stockModel.ToStockDto());
        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //var stockModel = _context.Stocks.FirstOrDefault(x => x.Id == id);
            var stockModel = await _stockRepo.DeleteAsync(id);
            if (stockModel == null)
            {
                return NotFound();
            }
           // _context.Stocks.Remove(stockModel);
            //await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
