using System.Collections.Generic;
using System.Threading.Tasks;

namespace BigTree.Models
{
    public interface IWorldRepository
    {
        IEnumerable<Trip> GetAllTrips();

        Trip GetTripByName(string tripName);

        void AddTrip(Trip trip);

        Task<bool> SaveChangesAsync();

        void AddStop(string tripName, Stop newStop);

    }
}