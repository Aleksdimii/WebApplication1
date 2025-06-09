using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

namespace WebApplication1.Controllers
{
    public class CrewMembersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CrewMembersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CrewMembers
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CrewMembers.Include(c => c.Flight);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CrewMembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var crewMember = await _context.CrewMembers
                .Include(c => c.Flight)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (crewMember == null)
            {
                return NotFound();
            }

            return View(crewMember);
        }

        // GET: CrewMembers/Create
        public IActionResult Create()
        {
            ViewData["FlightId"] = new SelectList(_context.Flights, "Id", "Id");
            return View();
        }

        // POST: CrewMembers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Role,FlightId")] CrewMember crewMember)
        {
            if (ModelState.IsValid)
            {
                _context.Add(crewMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FlightId"] = new SelectList(_context.Flights, "Id", "Id", crewMember.FlightId);
            return View(crewMember);
        }

        // GET: CrewMembers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var crewMember = await _context.CrewMembers.FindAsync(id);
            if (crewMember == null)
            {
                return NotFound();
            }
            ViewData["FlightId"] = new SelectList(_context.Flights, "Id", "Id", crewMember.FlightId);
            return View(crewMember);
        }

        // POST: CrewMembers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Role,FlightId")] CrewMember crewMember)
        {
            if (id != crewMember.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(crewMember);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CrewMemberExists(crewMember.Id))
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
            ViewData["FlightId"] = new SelectList(_context.Flights, "Id", "Id", crewMember.FlightId);
            return View(crewMember);
        }

        // GET: CrewMembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var crewMember = await _context.CrewMembers
                .Include(c => c.Flight)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (crewMember == null)
            {
                return NotFound();
            }

            return View(crewMember);
        }

        // POST: CrewMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var crewMember = await _context.CrewMembers.FindAsync(id);
            if (crewMember != null)
            {
                _context.CrewMembers.Remove(crewMember);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CrewMemberExists(int id)
        {
            return _context.CrewMembers.Any(e => e.Id == id);
        }
    }
}
