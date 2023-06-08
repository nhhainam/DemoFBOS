using DemoFBOS.Auth;
using DemoFBOS.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoFBOS.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class MealsController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		private readonly DemofbosContext _context;


		public MealsController(DemofbosContext context)
		{
			_context = context;
		}

		[AllowAnonymous]
		// GET: api/meals
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Meal>>> GetMeals()
		{
			var meals = await _context.Meals.ToListAsync();
			return Ok(meals);
		}

		[AllowAnonymous]
		// GET: api/meals/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Meal>> GetMeal(int id)
		{
			var meal = await _context.Meals.FindAsync(id);

			if (meal == null)
			{
				return NotFound();
			}

			return Ok(meal);
		}

		[HasPermission("Create Meal")]
		// POST: api/meals
		[HttpPost]
		public async Task<ActionResult<Meal>> CreateMeal(Meal meal)
		{
			_context.Meals.Add(meal);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetMeal), new { id = meal.MealId }, meal);
		}

		[HasPermission("Update Meal")]
		// PUT: api/meals/5
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateMeal(int id, Meal meal)
		{
			if (id != meal.MealId)
			{
				return BadRequest();
			}

			_context.Entry(meal).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MealExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return Ok();
		}

		[HasPermission("Delete Meal")]
		// DELETE: api/meals/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteMeal(int id)
		{
			var meal = await _context.Meals.FindAsync(id);
			if (meal == null)
			{
				return NotFound();
			}

			_context.Meals.Remove(meal);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		[AllowAnonymous]
		private bool MealExists(int id)
		{
			return _context.Meals.Any(e => e.MealId == id);
		}
	}
}
