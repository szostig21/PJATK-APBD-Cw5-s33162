using Microsoft.AspNetCore.Mvc;
using TrainingCenterApi.Data;
using TrainingCenterApi.Models;

namespace TrainingCenterApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<Reservation>> GetAll(
            [FromQuery] DateOnly? date,
            [FromQuery] string? status,
            [FromQuery] int? roomId)
        {
            IEnumerable<Reservation> reservations = AppData.Reservations;

            if (date.HasValue)
            {
                reservations = reservations.Where(r => r.Date == date.Value);
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                reservations = reservations.Where(r =>
                    r.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
            }

            if (roomId.HasValue)
            {
                reservations = reservations.Where(r => r.RoomId == roomId.Value);
            }

            return Ok(reservations);
        }

        [HttpGet("{id:int}")]
        public ActionResult<Reservation> GetById(int id)
        {
            var reservation = AppData.Reservations.FirstOrDefault(r => r.Id == id);

            if (reservation is null)
            {
                return NotFound();
            }

            return Ok(reservation);
        }

        [HttpPost]
        public ActionResult<Reservation> Create([FromBody] Reservation reservation)
        {
            var room = AppData.Rooms.FirstOrDefault(r => r.Id == reservation.RoomId);

            if (room is null)
            {
                return BadRequest(new { message = "Room does not exist" });
            }

            if (!room.IsActive)
            {
                return BadRequest(new { message = "Cannot create reservation for inactive room." });
            }

            bool hasConflict = AppData.Reservations.Any(r =>
                r.RoomId == reservation.RoomId &&
                r.Date == reservation.Date &&
                !r.Status.Equals("cancelled", StringComparison.OrdinalIgnoreCase) &&
                !reservation.Status.Equals("cancelled", StringComparison.OrdinalIgnoreCase) &&
                reservation.StartTime < r.EndTime &&
                reservation.EndTime > r.StartTime);

            if (hasConflict)
            {
                return Conflict(new
                {
                    message = "Reservation time conflicts with an existing reservation."
                });
            }

            int newId = AppData.Reservations.Any() ? AppData.Reservations.Max(r => r.Id) + 1 : 1;
            reservation.Id = newId;

            AppData.Reservations.Add(reservation);

            return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
        }

        [HttpPut("{id:int}")]
        public ActionResult<Reservation> Update(int id, [FromBody] Reservation updatedReservation)
        {
            var existingReservation = AppData.Reservations.FirstOrDefault(r => r.Id == id);

            if (existingReservation is null)
            {
                return NotFound();
            }

            var room = AppData.Rooms.FirstOrDefault(r => r.Id == updatedReservation.RoomId);

            if (room is null)
            {
                return BadRequest(new { message = "Room does not exist." });
            }

            if (!room.IsActive)
            {
                return BadRequest(new { message = "Cannot assign reservation to inactive room." });
            }

            bool hasConflict = AppData.Reservations.Any(r =>
                r.Id != id &&
                r.RoomId == updatedReservation.RoomId &&
                r.Date == updatedReservation.Date &&
                !r.Status.Equals("cancelled", StringComparison.OrdinalIgnoreCase) &&
                !updatedReservation.Status.Equals("cancelled", StringComparison.OrdinalIgnoreCase) &&
                updatedReservation.StartTime < r.EndTime &&
                updatedReservation.EndTime > r.StartTime);

            if (hasConflict)
            {
                return Conflict(new
                {
                    message = "Reservation time conflicts with an existing reservation."
                });
            }

            existingReservation.RoomId = updatedReservation.RoomId;
            existingReservation.OrganizerName = updatedReservation.OrganizerName;
            existingReservation.Topic = updatedReservation.Topic;
            existingReservation.Date = updatedReservation.Date;
            existingReservation.StartTime = updatedReservation.StartTime;
            existingReservation.EndTime = updatedReservation.EndTime;
            existingReservation.Status = updatedReservation.Status;

            return Ok(existingReservation);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var reservation = AppData.Reservations.FirstOrDefault(r => r.Id == id);

            if (reservation is null)
            {
                return NotFound();
            }

            AppData.Reservations.Remove(reservation);
            return NoContent();
        }
    }
}