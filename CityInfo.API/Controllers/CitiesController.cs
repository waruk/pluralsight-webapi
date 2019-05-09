using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        private ICityInfoRepository _repository;

        public CitiesController(ICityInfoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet()]
        public IActionResult GetCities()
        {
            // an empty list of cities is a valid response: there are no cities
            var cities = _repository.GetCities();

            var results = Mapper.Map<IEnumerable<CityWithoutPOIsDto>>(cities);

            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool includePointsOfInterest = false)
        {
            //find city
            var city = _repository.GetCity(id, includePointsOfInterest);
            if (city == null)
                return NotFound();

            if (includePointsOfInterest)
            {
                var cityResult = Mapper.Map<CityDto>(city);

                return Ok(cityResult);
            }

            var cityWithoutPointsOfInterestsResult = Mapper.Map<CityWithoutPOIsDto>(city);
            return Ok(cityWithoutPointsOfInterestsResult);
        }
    }
}