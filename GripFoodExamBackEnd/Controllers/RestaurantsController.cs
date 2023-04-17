using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GripFoodExamBackEnd.Entities;
using GripFoodExamBackEnd.Models;

namespace GripFoodExamBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RestaurantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Restaurants
        [HttpGet]
        public async Task<ActionResult<List<Restaurant>>> GetRestaurants(string? search)
        {
          if (_context.Restaurants == null)
          {
              return NotFound();
          }

            var query = _context.Restaurants.AsNoTracking();

            if (string.IsNullOrEmpty(search))
            {
                return await query.ToListAsync();
            }

            return await query
                .Where(Q => Q.Name.ToLower().Contains(search.ToLower()))
                .ToListAsync();
        }

        // GET: api/Restaurants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Restaurant>> GetRestaurant(string id)
        {
          if (_context.Restaurants == null)
          {
              return NotFound();
          }
            var restaurant = await _context.Restaurants.FindAsync(id);

            if (restaurant == null)
            {
                return NotFound();
            }

            return restaurant;
        }

        // PUT: api/Restaurants/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}", Name = "UpdateRestaurant")]
        public async Task<IActionResult> PutRestaurant(string id, RestaurantUpdateModel restaurant)
        {
            var update = await _context.Restaurants.Where(Q => Q.Id == id).FirstOrDefaultAsync();

            if (update == null)
            {
                return NotFound();
            }

            update.Name = restaurant.Name;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RestaurantExists(id))
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

        // POST: api/Restaurants
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost(Name = "CreateRestaurant")]
        public async Task<ActionResult<Restaurant>> PostRestaurant(RestaurantCreateModel restaurant)
        {
          if (_context.Restaurants == null)
          {
              return Problem("Entity set 'ApplicationDbContext.Restaurants'  is null.");
          }

            var insert = new Restaurant
            {
                Id = Ulid.NewUlid().ToString(),
                Name = restaurant.Name,
                CreatedAt = DateTimeOffset.UtcNow,
            };
            
            _context.Restaurants.Add(insert);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RestaurantExists(insert.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return insert;
        }

        // DELETE: api/Restaurants/5
        [HttpDelete("{id}", Name = "DeleteRestaurant")]
        public async Task<IActionResult> DeleteRestaurant(string id)
        {
            if (_context.Restaurants == null)
            {
                return NotFound();
            }
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null)
            {
                return NotFound();
            }

            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RestaurantExists(string id)
        {
            return (_context.Restaurants?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
