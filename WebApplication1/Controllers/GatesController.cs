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
    public class GatesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Gates
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Gates.Include(g => g.Terminal);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Gates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gate = await _context.Gates
                .Include(g => g.Terminal)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gate == null)
            {
                return NotFound();
            }

            return View(gate);
        }

        // GET: Gates/Create
        public IActionResult Create()
        {
            ViewData["TerminalId"] = new SelectList(_context.Terminals, "Id", "Id");
            return View();
        }

        // POST: Gates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,TerminalId")] Gate gate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TerminalId"] = new SelectList(_context.Terminals, "Id", "Id", gate.TerminalId);
            return View(gate);
        }

        // GET: Gates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gate = await _context.Gates.FindAsync(id);
            if (gate == null)
            {
                return NotFound();
            }
            ViewData["TerminalId"] = new SelectList(_context.Terminals, "Id", "Id", gate.TerminalId);
            return View(gate);
        }

        // POST: Gates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,TerminalId")] Gate gate)
        {
            if (id != gate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GateExists(gate.Id))
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
            ViewData["TerminalId"] = new SelectList(_context.Terminals, "Id", "Id", gate.TerminalId);
            return View(gate);
        }

        // GET: Gates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gate = await _context.Gates
                .Include(g => g.Terminal)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gate == null)
            {
                return NotFound();
            }

            return View(gate);
        }

        // POST: Gates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gate = await _context.Gates.FindAsync(id);
            if (gate != null)
            {
                _context.Gates.Remove(gate);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GateExists(int id)
        {
            return _context.Gates.Any(e => e.Id == id);
        }
    }
}
