using Microsoft.AspNetCore.Mvc;
using TrainingCenterApi.Data;
using TrainingCenterApi.Models;

namespace TrainingCenterApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<Room>> GetAll(
            [FromQuery] int? minCapacity,
            [FromQuery] bool? hasProjector,
            [FromQuery] bool? activeOnly)
        {
            IEnumerable<Room> rooms = AppData.Rooms;

            if (minCapacity.HasValue)
            {
                rooms = rooms.Where(r => r.Capacity >= minCapacity.Value);
            }

            if (hasProjector.HasValue)
            {
                rooms = rooms.Where(r => r.HasProjector == hasProjector.Value);
            }

            if (activeOnly == true)
            {
                rooms = rooms.Where(r => r.IsActive);
            }

            return Ok(rooms);
        }

        [HttpGet("{id:int}")]
        public ActionResult<Room> GetById(int id)
        {
            var room = AppData.Rooms.FirstOrDefault(r => r.Id == id);

            if (room is null)
            {
                return NotFound();
            }

            return Ok(room);
        }

        [HttpGet("building/{buildingCode}")]
        public ActionResult<IEnumerable<Room>> GetByBuilding(string buildingCode)
        {
            var rooms = AppData.Rooms
                .Where(r => r.BuildingCode.Equals(buildingCode, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Ok(rooms);
        }

        [HttpPost]
        public ActionResult<Room> Create([FromBody] Room room)
        {
            int newId = AppData.Rooms.Any() ? AppData.Rooms.Max(r => r.Id) + 1 : 1;
            room.Id = newId;

            AppData.Rooms.Add(room);

            return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
        }

        [HttpPut("{id:int}")]
        public ActionResult<Room> Update(int id, [FromBody] Room updatedRoom)
        {
            var existingRoom = AppData.Rooms.FirstOrDefault(r => r.Id == id);

            if (existingRoom is null)
            {
                return NotFound();
            }

            existingRoom.Name = updatedRoom.Name;
            existingRoom.BuildingCode = updatedRoom.BuildingCode;
            existingRoom.Floor = updatedRoom.Floor;
            existingRoom.Capacity = updatedRoom.Capacity;
            existingRoom.HasProjector = updatedRoom.HasProjector;
            existingRoom.IsActive = updatedRoom.IsActive;

            return Ok(existingRoom);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var room = AppData.Rooms.FirstOrDefault(r => r.Id == id);

            if (room is null)
            {
                return NotFound();
            }

            bool hasRelatedReservations = AppData.Reservations.Any(r => r.RoomId == id);

            if (hasRelatedReservations)
            {
                return Conflict(new
                {
                    message = "Cannot delete room with related reservations."
                });
            }

            AppData.Rooms.Remove(room);
            return NoContent();
        }
    }
}