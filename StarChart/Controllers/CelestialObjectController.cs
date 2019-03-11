
using Microsoft.AspNetCore.Mvc;

namespace StarChart.Controllers
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Hosting.Internal;

    using Remotion.Linq.Clauses;

    using StarChart.Data;
    using StarChart.Models;

    [ApiController]
    [Route("")]
    public class CelestialObjectController : ControllerBase
    {
        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        private readonly ApplicationDbContext _context;

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects.Find(id);
            if (celestialObject == null)
            {
                return NotFound();
            }

            celestialObject.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == id).ToList();


            return Ok(celestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celesticalObjects = _context.CelestialObjects.Where(x => x.Name == name).ToList();

            if (!celesticalObjects.Any())
            {
                return NotFound();
            }

            foreach (var celesticalObject in celesticalObjects)
            {
                celesticalObject.Satellites = _context.CelestialObjects
                    .Where(x => x.OrbitedObjectId == celesticalObject.Id).ToList();
            }
            return Ok(celesticalObjects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList();

            foreach (var celestialObject in celestialObjects)
            {
                celestialObject.Satellites =
                    celestialObjects.Where(x => x.OrbitedObjectId == celestialObject.Id).ToList();
            }

            return Ok(celestialObjects);
        }
    }
}
