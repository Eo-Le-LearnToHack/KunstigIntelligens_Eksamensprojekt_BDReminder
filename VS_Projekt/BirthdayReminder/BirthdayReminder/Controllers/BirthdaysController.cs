using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BirthdayReminder.Data;
using BirthdayReminder.Models;
using System.Linq;
using System.Threading.Tasks;

namespace BirthdayReminder.Controllers
{
    [Authorize] // Beskyt hele kontrolleren, så kun godkendte brugere har adgang
    public class BirthdaysController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BirthdaysController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Birthdays
        public async Task<IActionResult> Index()
        {
            return View(await _context.Birthdays.ToListAsync());
        }

        // GET: Birthdays/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var birthday = await _context.Birthdays
                .FirstOrDefaultAsync(m => m.Id == id);
            if (birthday == null)
            {
                return NotFound();
            }

            return View(birthday);
        }

        // GET: Birthdays/Create
        [Authorize(Roles = "admin")] // Kun admin kan oprette fødselsdage
        public IActionResult Create()
        {
            return View();
        }

        // POST: Birthdays/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")] // Kun admin kan oprette fødselsdage
        public async Task<IActionResult> Create([Bind("Id,Name,Date,Relationship")] Birthday birthday)
        {
            if (ModelState.IsValid)
            {
                _context.Add(birthday);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(birthday);
        }

        // GET: Birthdays/Edit/5
        [Authorize(Roles = "admin")] // Kun admin kan redigere fødselsdage
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var birthday = await _context.Birthdays.FindAsync(id);
            if (birthday == null)
            {
                return NotFound();
            }
            return View(birthday);
        }

        // POST: Birthdays/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")] // Kun admin kan redigere fødselsdage
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Date,Relationship")] Birthday birthday)
        {
            if (id != birthday.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(birthday);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BirthdayExists(birthday.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(birthday);
        }

        // GET: Birthdays/Delete/5
        [Authorize(Roles = "admin")] // Kun admin kan slette fødselsdage
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var birthday = await _context.Birthdays
                .FirstOrDefaultAsync(m => m.Id == id);
            if (birthday == null)
            {
                return NotFound();
            }

            return View(birthday);
        }

        // POST: Birthdays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")] // Kun admin kan slette fødselsdage
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var birthday = await _context.Birthdays.FindAsync(id);
            _context.Birthdays.Remove(birthday);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BirthdayExists(int id)
        {
            return _context.Birthdays.Any(e => e.Id == id);
        }
    }
}
