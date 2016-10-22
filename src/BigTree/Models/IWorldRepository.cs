using System.Collections.Generic;

namespace BigTree.Models
{
    public interface IWorldRepository
    {
        IEnumerable<Trip> GetAllTrips();
    }
}