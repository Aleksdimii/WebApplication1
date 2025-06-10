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

        private string GetAirlineLogo(string airline, string logoUrl)
        {
            if (!string.IsNullOrEmpty(logoUrl))
                return logoUrl;

            if (string.IsNullOrEmpty(airline))
                return "/images/default-plane.png";

            var formatted = airline.ToLower().Replace(" ", "-");
            return $"https://content.airhex.com/content/logos/airlines_{formatted}_200_200_s.png?api_key=0579a42ff493048836027c94181849";
        }

        private async Task<(double lat, double lng)?> GetCoordinatesAsync(string locationName)
        {
            using var httpClient = new HttpClient();
            var apiKey = "20d6ac341a904822972ac260da5ab7";
            var url = $"https://api.opencagedata.com/geocode/v1/json?q={Uri.EscapeDataString(locationName)}&key={apiKey}&limit=1";

            var response = await httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject(content);
            if (json.results.Count == 0) return null;

            double lat = json.results[0].geometry.lat;
            double lng = json.results[0].geometry.lng;
            return (lat, lng);
        }


        // GET: Flights
        public async Task<IActionResult> Index(int? airportId)
        {
            var now = DateTime.Now;

            var airports = await _context.Airports.ToListAsync();

            var departuresQuery = _context.Flights
                .Include(f => f.Aircraft)
                .Include(f => f.ArrivalAirport)
                .Include(f => f.DepartureAirport)
                .Where(f => f.DepartureTime >= now);

            var arrivalsQuery = _context.Flights
                .Include(f => f.Aircraft)
                .Include(f => f.ArrivalAirport)
                .Include(f => f.DepartureAirport)
                .Where(f => f.ArrivalTime >= now);

            if (airportId.HasValue)
            {
                departuresQuery = departuresQuery.Where(f => f.DepartureAirportId == airportId.Value);
                arrivalsQuery = arrivalsQuery.Where(f => f.ArrivalAirportId == airportId.Value);
            }

            var departures = await departuresQuery.OrderBy(f => f.DepartureTime).ToListAsync();
            var arrivals = await arrivalsQuery.OrderBy(f => f.ArrivalTime).ToListAsync();

            foreach (var flight in departures.Concat(arrivals))
            {
                flight.AirlineLogoUrl = GetAirlineLogo(flight.Airline, flight.AirlineLogoUrl);
            }

            var viewModel = new FlightIndexViewModel
            {
                Departures = departures,
                Arrivals = arrivals,
                Airports = airports,
                SelectedAirportId = airportId
            };

            return View(viewModel);
        }




        // GET: Flights/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var flight = await _context.Flights
                .Include(f => f.Aircraft)
                .Include(f => f.ArrivalAirport)
                .Include(f => f.DepartureAirport)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (flight == null) return NotFound();

            var departureCoords = await GetCoordinatesAsync(flight.DepartureAirport?.Name);
            var arrivalCoords = await GetCoordinatesAsync(flight.ArrivalAirport?.Name);

            ViewBag.DepartureLat = departureCoords?.lat;
            ViewBag.DepartureLng = departureCoords?.lng;
            ViewBag.ArrivalLat = arrivalCoords?.lat;
            ViewBag.ArrivalLng = arrivalCoords?.lng;

            return View(flight);
        }



        // GET: Flights/Create
        public IActionResult Create()
        {
            ViewData["AircraftId"] = new SelectList(_context.Aircrafts, "Id", "Model");
            ViewData["ArrivalAirportId"] = new SelectList(_context.Airports, "Id", "Name");
            ViewData["DepartureAirportId"] = new SelectList(_context.Airports, "Id", "Name");
            return View();
        }

        // POST: Flights/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FlightNumber,Airline,DepartureAirportId,ArrivalAirportId,DepartureTime,ArrivalTime,Status,Type,AirlineLogoUrl,AircraftId")] Flight flight)
        {
            if (ModelState.IsValid)
            {
                _context.Add(flight);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AircraftId"] = new SelectList(_context.Aircrafts, "Id", "Model", flight.AircraftId);
            ViewData["ArrivalAirportId"] = new SelectList(_context.Airports, "Id", "Name", flight.ArrivalAirportId);
            ViewData["DepartureAirportId"] = new SelectList(_context.Airports, "Id", "Name", flight.DepartureAirportId);
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
            ViewData["AircraftId"] = new SelectList(_context.Aircrafts, "Id", "Model", flight.AircraftId);
            ViewData["ArrivalAirportId"] = new SelectList(_context.Airports, "Id", "Name", flight.ArrivalAirportId);
            ViewData["DepartureAirportId"] = new SelectList(_context.Airports, "Id", "Name", flight.DepartureAirportId);
            return View(flight);
        }

        // POST: Flights/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FlightNumber,Airline,DepartureAirportId,ArrivalAirportId,DepartureTime,ArrivalTime,Status,Type,AirlineLogoUrl,AircraftId")] Flight flight)
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
            ViewData["AircraftId"] = new SelectList(_context.Aircrafts, "Id", "Model", flight.AircraftId);
            ViewData["ArrivalAirportId"] = new SelectList(_context.Airports, "Id", "Name", flight.ArrivalAirportId);
            ViewData["DepartureAirportId"] = new SelectList(_context.Airports, "Id", "Name", flight.DepartureAirportId);
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
