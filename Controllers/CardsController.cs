using CardsAPI.Data;
using CardsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CardsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardsController : Controller
    {
        private readonly CardsDbContext cardsDbContext;
        public CardsController(CardsDbContext cardsDbContext)
        {
            this.cardsDbContext = cardsDbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCards()
        {
            var cards = await cardsDbContext.Cards.ToArrayAsync();
            return Ok(cards);
        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetCard")]
        public async Task<IActionResult> GetCard([FromRoute] Guid id)
        {
            var cards = await cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if (id != null)
            {
                return Ok(cards);
            }
            return NotFound("Cards not found");
        }
        [HttpPost]
        public async Task<IActionResult> AddCard([FromBody] Cards card)
        {
            card.Id = Guid.NewGuid();
            await cardsDbContext.Cards.AddAsync(card);
            await cardsDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(AddCard), new { id = card.Id }, card);
        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateCard([FromRoute]Guid id, [FromBody] Cards card)
        {
            var ExistingCard = await cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if(ExistingCard != null)
            {
                ExistingCard.CardholderName = card.CardholderName;  
                ExistingCard.CardNumber = card.CardNumber;
                ExistingCard.ExpiryMonth = card.ExpiryMonth;
                ExistingCard.ExpiryYear = card.ExpiryYear;
                ExistingCard.CVC = card.CVC;
                await cardsDbContext.SaveChangesAsync();
                return Ok(ExistingCard);
            }
            return NotFound("Card not found");
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteCard([FromRoute] Guid id)
        {
            var ExistingCard = await cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if (ExistingCard != null)
            {
                cardsDbContext.Cards.Remove(ExistingCard);
                await cardsDbContext.SaveChangesAsync();
                return Ok(ExistingCard);
            } 
            return NotFound("Card not found");
        }
    }

}
