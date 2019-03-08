using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        [HttpGet()]
        public IActionResult GetCities()
        {
            // an empty list of cities is a valid response: there are no cities
            return Ok(CitiesDataStore.Current.Cities);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id)
        {
            //find city
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(p => p.Id == id);
            if (city == null)
                return NotFound();

            return Ok(city);
        }
    }
}