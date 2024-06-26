using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BirthdayReminder.Data;
using BirthdayReminder.Models;

namespace BirthdayReminder.Controllers
{
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Birthdays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var birthday = await _context.Birthdays.FindAsync(id);
            if (birthday != null)
            {
                _context.Birthdays.Remove(birthday);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BirthdayExists(int id)
        {
            return _context.Birthdays.Any(e => e.Id == id);
        }
    }
}
