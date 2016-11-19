using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;

namespace BigTree.Models
{
    public interface IWorldRepository
    {
        IEnumerable<Trip> GetAllTrips();

        Trip GetTripByName(string tripName);
        Trip GetTripByUserName(string name);
        Trip GetUserTripByName(string tripName, string name);



        void AddTrip(Trip trip);

        Task<bool> SaveChangesAsync();

        void AddStop(string tripName, Stop newStop, string userName);
    }
}