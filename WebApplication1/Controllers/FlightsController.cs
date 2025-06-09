using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class FlightsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FlightsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Flights
        public async Task<IActionResult> Index()
        {
            var now = DateTime.Now;

            var departures = await _context.Flights
                .Include(f => f.Aircraft)
                .Include(f => f.ArrivalAirport)
                .Include(f => f.DepartureAirport)
                .Where(f => f.DepartureTime >= now)
                .OrderBy(f => f.DepartureTime)
                .ToListAsync();

            var arrivals = await _context.Flights
                .Include(f => f.Aircraft)
                .Include(f => f.ArrivalAirport)
                .Include(f => f.DepartureAirport)
                .Where(f => f.ArrivalTime >= now)
                .OrderBy(f => f.ArrivalTime)
                .ToListAsync();

            var viewModel = new FlightIndexViewModel
            {
                Departures = departures,
                Arrivals = arrivals
            };

            return View(viewModel);
        }

        // GET: Flights/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .Include(f => f.Aircraft)
                .Include(f => f.ArrivalAirport)
                .Include(f => f.DepartureAirport)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        // GET: Flights/Create
        public IActionResult Create()
        {
            ViewData["AircraftId"] = new SelectList(_context.Aircrafts, "Id", "Id");
            ViewData["ArrivalAirportId"] = new SelectList(_context.Airports, "Id", "Id");
            ViewData["DepartureAirportId"] = new SelectList(_context.Airports, "Id", "Id");
            return View();
        }

        // POST: Flights/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FlightNumber,Airline,DepartureAirportId,ArrivalAirportId,DepartureTime,ArrivalTime,Status,AircraftId")] Flight flight)
        {
            if (ModelState.IsValid)
            {
                _context.Add(flight);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AircraftId"] = new SelectList(_context.Aircrafts, "Id", "Id", flight.AircraftId);
            ViewData["ArrivalAirportId"] = new SelectList(_context.Airports, "Id", "Id", flight.ArrivalAirportId);
            ViewData["DepartureAirportId"] = new SelectList(_context.Airports, "Id", "Id", flight.DepartureAirportId);
            return View(flight);
        }

        // GET: Flights/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights.FindAsync(id);
            if (flight == null)
            {
                return NotFound();
            }
            ViewData["AircraftId"] = new SelectList(_context.Aircrafts, "Id", "Id", flight.AircraftId);
            ViewData["ArrivalAirportId"] = new SelectList(_context.Airports, "Id", "Id", flight.ArrivalAirportId);
            ViewData["DepartureAirportId"] = new SelectList(_context.Airports, "Id", "Id", flight.DepartureAirportId);
            return View(flight);
        }

        // POST: Flights/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FlightNumber,Airline,DepartureAirportId,ArrivalAirportId,DepartureTime,ArrivalTime,Status,AircraftId")] Flight flight)
        {
            if (id != flight.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(flight);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlightExists(flight.Id))
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
            ViewData["AircraftId"] = new SelectList(_context.Aircrafts, "Id", "Id", flight.AircraftId);
            ViewData["ArrivalAirportId"] = new SelectList(_context.Airports, "Id", "Id", flight.ArrivalAirportId);
            ViewData["DepartureAirportId"] = new SelectList(_context.Airports, "Id", "Id", flight.DepartureAirportId);
            return View(flight);
        }

        // GET: Flights/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .Include(f => f.Aircraft)
                .Include(f => f.ArrivalAirport)
                .Include(f => f.DepartureAirport)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        // POST: Flights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flight = await _context.Flights.FindAsync(id);
            if (flight != null)
            {
                _context.Flights.Remove(flight);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlightExists(int id)
        {
            return _context.Flights.Any(e => e.Id == id);
        }
    }
}
