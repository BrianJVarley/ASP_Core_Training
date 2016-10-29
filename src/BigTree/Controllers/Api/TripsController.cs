using AutoMapper;
using BigTree.Models;
using BigTree.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BigTree.Controllers.Api
{
    [Route("api/trips")]
    public class TripsController : Controller
    {

        private IWorldRepository _repository;

        public TripsController(IWorldRepository repository)
        {
            this._repository = repository;
        }

        [HttpGet("")]
        public IActionResult Get()
        {
            try
            {
                var result = _repository.GetAllTrips();
                return Ok(Mapper.Map<IEnumerable<TripViewModel>>(result));

            }
            catch (Exception ex)
            {
                //TODO Logging
                return BadRequest("Something strange happened..");

            }


        }

        [HttpPost("")]
        public IActionResult Post([FromBody]TripViewModel trip)
        {
            if (ModelState.IsValid)
            {
                //Save to DB
                var newTrip = Mapper.Map<Trip>(trip);
                return Created($"api/trips/{newTrip.Name}", Mapper.Map<TripViewModel>(newTrip));

            }

            return BadRequest(ModelState);
            
        }
    }
}
