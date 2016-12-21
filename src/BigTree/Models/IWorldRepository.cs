using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;

namespace BigTree.Models
{
    public interface IWorldRepository
    {
        IEnumerable<Trip> GetAllTrips();
        IEnumerable<Trip> GetTripByUserName(string name);
        Trip GetTripByName(string tripName);
        Trip GetUserTripByName(string tripName, string name);

        void AddTrip(Trip trip);
        void AddStop(string tripName, Stop newStop, string userName);

        Task<bool> SaveChangesAsync();

    }
}