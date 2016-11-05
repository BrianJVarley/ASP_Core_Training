using AutoMapper;
using BigTree.Models;
using BigTree.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private ILogger<TripViewModel> _logger;

        public TripsController(IWorldRepository repository, ILogger<TripViewModel> logger)
        {
            this._repository = repository;
            this._logger = logger;
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
                _logger.LogError("Failed to get All Trips:" + ex);
                return BadRequest("Error occurred");

            }


        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]TripViewModel trip)
        {


            if (ModelState.IsValid)
            {
                //Save to DB
                var newTrip = Mapper.Map<Trip>(trip);
                _repository.AddTrip(newTrip);

                if(await _repository.SaveChangesAsync()) //persist the dat here from context
                {
                    return Created($"api/trips/{newTrip.Name}", Mapper.Map<TripViewModel>(newTrip));
                }
            }

            return BadRequest("Failed to save trip");


        }
    }
}
