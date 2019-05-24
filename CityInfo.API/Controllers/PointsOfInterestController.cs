using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using CityInfo.API.Services;
using AutoMapper;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {
        private ILogger<PointsOfInterestController> _logger;
        private IMailService _mailService;
        private ICityInfoRepository _repository;

        // constructors
        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService, ICityInfoRepository repository)
        {
            _logger = logger;
            _mailService = mailService;
            _repository = repository;
        }

        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            if (!_repository.CityExists(cityId))
            {
                _logger.LogInformation($"City with id {cityId} was not found when accessing points of interest.");   // string interpolation syntax
                return NotFound();
            }

            var pointsOfInterestForCity = _repository.GetPointsOfInterestForCity(cityId);
            var pointsOfInterestForCityResults = Mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity);
            
            return Ok(pointsOfInterestForCityResults);
        }

        [HttpGet("{cityId}/pointsofinterest/{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            if (!_repository.CityExists(cityId))
            {
                _logger.LogInformation($"City with id {cityId} was not found when accessing points of interest.");   // string interpolation syntax
                return NotFound();
            }

            var pointOfInterest = _repository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterest == null)
                return NotFound();

            var pointOfInterestResult = Mapper.Map<PointOfInterestDto>(pointOfInterest);

            return Ok(pointOfInterestResult);
        }

        [HttpPost("{cityId}/pointsofinterest")]
        public IActionResult CreatePointOfInterest(int cityId, [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            if (pointOfInterest == null)
                return BadRequest();

            if (pointOfInterest.Description == pointOfInterest.Name)
                ModelState.AddModelError("Description", "The provided description should be different from name.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_repository.CityExists(cityId))
                return NotFound();

            var finalpointOfInterest = Mapper.Map<Entities.PointOfInterest>(pointOfInterest);
            _repository.AddPointOfInterestForCity(cityId, finalpointOfInterest);

            if (!_repository.Save())
                return StatusCode(500, "A problem happened while handling you request.");

            var createdPointOfInterest = Mapper.Map<PointOfInterestDto>(finalpointOfInterest);

            return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, id = finalpointOfInterest.Id }, createdPointOfInterest);
        }

        [HttpPut("{cityId}/pointsofinterest/{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id, [FromBody] PointOfInterestForUpdateDto pointOfInterest)
        {
            if (pointOfInterest == null)
                return BadRequest();

            if (pointOfInterest.Description == pointOfInterest.Name)
                ModelState.AddModelError("Description", "The provided description should be different from name.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_repository.CityExists(cityId))
                return NotFound();

            var pointOfInterestEntity = _repository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterestEntity == null)
                return NotFound();

            Mapper.Map(pointOfInterest, pointOfInterestEntity);
            if (!_repository.Save())
                return StatusCode(500, "A problem happened while handling you request.");

            return NoContent();
        }

        [HttpPatch("{cityId}/pointsofinterest/{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id, [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();

            if (!_repository.CityExists(cityId))
                return NotFound();

            var pointOfInterestEntity = _repository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterestEntity == null)
                return NotFound();

            var pointOfInterestToPatch = Mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);

            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            TryValidateModel(pointOfInterestToPatch);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);
            if (!_repository.Save())
                return StatusCode(500, "A problem happened while handling you request.");


            return NoContent();
        }

        [HttpDelete("{cityId}/pointsofinterest/{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            if (!_repository.CityExists(cityId))
                return NotFound();

            var pointOfInterestEntity = _repository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterestEntity == null)
                return NotFound();

            _repository.DeletePointOfInterest(pointOfInterestEntity);
            if (!_repository.Save())
                return StatusCode(500, "A problem happened while handling you request.");

            //_mailService.Send("Point of interest was deleted", $"Point of interest {pointOfInterestEntity.Name} with id {id} has been deleted.");

            return NoContent();
        }

    }
}
