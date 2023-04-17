using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GripFoodExamBackEnd.Entities;
using Microsoft.AspNetCore.Authorization;
using static OpenIddict.Abstractions.OpenIddictConstants;
using GripFoodExamBackEnd.Models;

namespace GripFoodExamBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Carts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCarts()
        {
          if (_context.Carts == null)
          {
              return NotFound();
          }
            return await _context.Carts.ToListAsync();
        }

        // GET: api/Carts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetCart(string id)
        {
          if (_context.Carts == null)
          {
              return NotFound();
          }
            var cart = await _context.Carts.FindAsync(id);

            if (cart == null)
            {
                return NotFound();
            }

            return cart;
        }

        // PUT: api/Carts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCart(string id, Cart cart)
        {
            if (id != cart.Id)
            {
                return BadRequest();
            }

            _context.Entry(cart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Carts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost(Name = "AddToCart")]
        [Authorize("api")]
        public async Task<ActionResult<bool>> PostCart(AddToCartModel cart)
        {
            if (_context.Carts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Carts'  is null.");
            }


            var userId = User.FindFirst(Claims.Subject)?.Value ?? throw new InvalidOperationException("User ID not found");

            var existing = await _context.Carts
                .Where(Q => Q.RestaurantId == cart.RestaurantId && Q.UserId == userId)
                .FirstOrDefaultAsync();

            if (existing != null)
            {
                var cartDetail = await _context.CartDetails
                    .Where(Q => Q.CartId == existing.Id && Q.FoodItemId == cart.FoodItemId)
                    .FirstOrDefaultAsync();

                if (cartDetail != null)
                {
                    cartDetail.Qty += cart.Qty;
                }
                else
                {
                    var insert = new CartDetail
                    {
                        Id = Ulid.NewUlid().ToString(),
                        CartId = existing.Id,
                        FoodItemId = cart.FoodItemId,
                        Qty = cart.Qty
                    };

                    _context.CartDetails.Add(insert);
                }
            }
            else
            {
                var insert = new Cart
                {
                    UserId = userId,
                    CreatedAt = DateTimeOffset.UtcNow,
                    RestaurantId = cart.RestaurantId,
                    Id = Ulid.NewUlid().ToString()
                };
                _context.Carts.Add(insert);

                await _context.SaveChangesAsync();

                var insertDetails = new CartDetail
                {
                    Id = Ulid.NewUlid().ToString(),
                    CartId = insert.Id,
                    FoodItemId = cart.FoodItemId,
                    Qty = cart.Qty
                };

                _context.CartDetails.Add(insertDetails);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        // DELETE: api/Carts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(string id)
        {
            if (_context.Carts == null)
            {
                return NotFound();
            }
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CartExists(string id)
        {
            return (_context.Carts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
