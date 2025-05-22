using ProiectPAWRusuMihnea;
using System;
[Serializable]
public class Booking
{
    public int bookingId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Route route;
    public DateTime dateOfDeparture { get; set; }
    public DateTime dateOfReturn { get; set; }

    public Booking(string firstName, string lastName, Route route, DateTime dep, DateTime ret)
    {
        this.FirstName = firstName;
        this.LastName = lastName;
        this.route = route;
        this.dateOfDeparture = dep;
        this.dateOfReturn = ret;
    }

    public Booking(int bookingId, string firstName, string lastName, Route route, DateTime dep, DateTime ret)
    {
        this.bookingId = bookingId;
        this.FirstName = firstName;
        this.LastName = lastName;
        this.route = route;
        this.dateOfDeparture = dep;
        this.dateOfReturn = ret;
    }

    public override string ToString()
    {
        return $"{FirstName} {LastName} | {route?.departure} → {route?.destination} | {dateOfDeparture:yyyy-MM-dd}";
    }
}