using TrainingCenterApi.Models;

namespace TrainingCenterApi.Data
{
    public static class AppData
    {
        public static List<Room> Rooms { get; } = new()
        {
            new Room { Id = 1, Name = "Sala A101", BuildingCode = "A", Floor = 1, Capacity = 20, HasProjector = true,  IsActive = true },
            new Room { Id = 2, Name = "Sala A203", BuildingCode = "A", Floor = 2, Capacity = 30, HasProjector = true,  IsActive = true },
            new Room { Id = 3, Name = "Lab B204",  BuildingCode = "B", Floor = 2, Capacity = 24, HasProjector = true,  IsActive = true },
            new Room { Id = 4, Name = "Sala B105", BuildingCode = "B", Floor = 1, Capacity = 12, HasProjector = false, IsActive = true },
            new Room { Id = 5, Name = "Sala C301", BuildingCode = "C", Floor = 3, Capacity = 18, HasProjector = true,  IsActive = false }
        };

        public static List<Reservation> Reservations { get; } = new()
        {
            new Reservation
            {
                Id = 1,
                RoomId = 1,
                OrganizerName = "Anna Kowalska",
                Topic = "Warsztaty z REST API",
                Date = new DateOnly(2026, 5, 10),
                StartTime = new TimeOnly(9, 0, 0),
                EndTime = new TimeOnly(11, 0, 0),
                Status = "confirmed"
            },
            new Reservation
            {
                Id = 2,
                RoomId = 2,
                OrganizerName = "Jan Nowak",
                Topic = "Konsultacje ASP.NET",
                Date = new DateOnly(2026, 5, 10),
                StartTime = new TimeOnly(10, 0, 0),
                EndTime = new TimeOnly(12, 30, 0),
                Status = "planned"
            },
            new Reservation
            {
                Id = 3,
                RoomId = 3,
                OrganizerName = "Ewa Zielińska",
                Topic = "Szkolenie HTTP",
                Date = new DateOnly(2026, 5, 11),
                StartTime = new TimeOnly(8, 30, 0),
                EndTime = new TimeOnly(10, 0, 0),
                Status = "confirmed"
            },
            new Reservation
            {
                Id = 4,
                RoomId = 4,
                OrganizerName = "Piotr Adamski",
                Topic = "Spotkanie organizacyjne",
                Date = new DateOnly(2026, 5, 12),
                StartTime = new TimeOnly(13, 0, 0),
                EndTime = new TimeOnly(14, 0, 0),
                Status = "cancelled"
            },
            new Reservation
            {
                Id = 5,
                RoomId = 1,
                OrganizerName = "Marta Wiśniewska",
                Topic = "Warsztaty z Postmana",
                Date = new DateOnly(2026, 5, 13),
                StartTime = new TimeOnly(14, 0, 0),
                EndTime = new TimeOnly(16, 0, 0),
                Status = "planned"
            }
        };
    }
}