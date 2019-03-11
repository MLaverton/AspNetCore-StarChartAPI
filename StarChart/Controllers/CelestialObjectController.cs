
using Microsoft.AspNetCore.Mvc;

namespace StarChart.Controllers
{
    using System.Linq;

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

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celest)
        {
            _context.CelestialObjects.Add(celest);
            _context.SaveChanges();

            return CreatedAtRoute(
                "GetById",
                new
                {
                    id = celest.Id
                },
                celest);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celest)
        {
            var celestialObject = _context.CelestialObjects.Find(id);

            if (celestialObject == null)
            {
                return NotFound();
            }

            celestialObject.Name = celest.Name;
            celestialObject.OrbitalPeriod = celest.OrbitalPeriod;
            celestialObject.OrbitedObjectId = celest.OrbitedObjectId;

            _context.CelestialObjects.Update(celestialObject);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var celestialObject = _context.CelestialObjects.Find(id);

            if (celestialObject == null)
            {
                return NotFound();
            }

            celestialObject.Name = name;

            _context.CelestialObjects.Update(celestialObject);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var celestialObjectList = _context.CelestialObjects.Where(x => x.Id == id || x.OrbitedObjectId == id).ToList();

            if (!celestialObjectList.Any())
            {
                return NotFound();
            }

            _context.CelestialObjects.RemoveRange(celestialObjectList);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
