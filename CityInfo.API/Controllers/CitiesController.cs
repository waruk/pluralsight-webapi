using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            var results = new List<CityWithoutPOIsDto>();

            foreach (var cityEntity in cities)
            {
                results.Add(new CityWithoutPOIsDto
                {
                    Id = cityEntity.Id,
                    Name = cityEntity.Name,
                    Description = cityEntity.Description
                });
            }

            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id)
        {
            //find city
            var city = _repository.GetCity(id, false);
            if (city == null)
                return NotFound();

            return Ok(city);
        }
    }
}