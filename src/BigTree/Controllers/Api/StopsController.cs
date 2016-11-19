using BigTree.Models;
using BigTree.Services;
using BigTree.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BigTree.Controllers.Api
{
    [Authorize]
    [Route("/api/trips/{tripName}/stops")]
    public class StopsController : Controller
    {
        private ILogger<StopsController> _logger;
        private IWorldRepository _repository;
        private GeoCoordsService _coordsService;

        public StopsController(IWorldRepository repository, ILogger<StopsController> logger,
            GeoCoordsService coordsService)
        {

            _logger = logger;
            _repository = repository;
            _coordsService = coordsService;

        }


        [HttpPost("")]
        public async Task<IActionResult> Post(string tripName, [FromBody]StopViewModel vm)
        {

            try
            {

                if (ModelState.IsValid)
                {
                    var newStop = AutoMapper.Mapper.Map<Stop>(vm);

                    //lookup geocodes
                    var result = await _coordsService.GetCoordsAsync(newStop.Name);
                    if(!result.Success)
                    {
                        _logger.LogError(result.Message);
                    }
                    else
                    {
                        newStop.Longitude = result.Longitude;
                        newStop.Latitude = result.Latitude;
                    }

                    //save to DB
                    _repository.AddStop(tripName, newStop, User.Identity.Name);

                    if (await _repository.SaveChangesAsync())
                    {
                        return Created($"/api/trips/{tripName}/stops/{newStop.Name}",
                                 AutoMapper.Mapper.Map<StopViewModel>(newStop));
                    }

                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Post stop failed: " + ex);
            }

            return BadRequest("Posting stop failed");


        }


        [HttpGet("")]
        public IActionResult Get(string tripName)
        {

            try
            {

                var trip = _repository.GetUserTripByName(tripName, User.Identity.Name);
                return Ok(AutoMapper.Mapper.Map<IEnumerable<StopViewModel>>(trip.Stops.OrderBy(s => s.Order).ToList()));

            }
            catch (Exception ex)
            {

                _logger.LogError("Failed to get stops: " + ex);

            }

            return BadRequest("Failed to get stops");
        }
    }
}
