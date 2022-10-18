using AutoMapper;
using HotelListing.Data;
using HotelListing.DTOs.Country;
using HotelListing.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CountryController : ControllerBase
    {
        private readonly ILogger<CountryController> _logger;
        private readonly IMapper _mapper;
        private readonly ICountriesRepository _countriesRepository;

        public CountryController(ILogger<CountryController> logger, IMapper mapper, ICountriesRepository countriesRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _countriesRepository = countriesRepository;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCountryDTO>>> GetCountries()
        {
                var countries = await _countriesRepository.GetAllAsync();
                var result = _mapper.Map<List<GetCountryDTO>>(countries);
                return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CountryDTO>> GetCountry(int id)
        {
        var country = await _countriesRepository.GetDetails(id);
            if(country == null)
            {
                return NotFound();
            }
            var countryDto = _mapper.Map<CountryDTO>(country);
            return Ok(countryDto); 

            //try
            //{
            //    var country = await _unitOfWork.Countires.Get(c => c.Id == id, includes: new List<string> {"Hotels"});
            //    var result = _mapper.Map<CountryDTO>(country);
            //    return Ok(result);
            //} catch (Exception ex)
            //{
            //    _logger.LogError(ex,$"Something went wrong in the {nameof(GetCountries)}");
            //    return StatusCode(500,"Internal server error, please try again later.");
            //}
        }


        [HttpPut("{id}")]

        public async Task<IActionResult> PutCountry(int id, UpdateCountryDTO updateCountryDTO)
        {
        if(id != updateCountryDTO.Id)
            {
                return BadRequest("Invalid record Id");
            }

            var country = await _countriesRepository.GetAsync(id);

            if(country == null)
            {
                return NotFound();
            }
            _mapper.Map(updateCountryDTO,country);

            try
            {

            await _countriesRepository.UpdateAsync(country);
            }
            catch (DbUpdateConcurrencyException)
            {
            if(!await CountryExists(id))
                {
                    return NotFound();
                } else
                {
                    throw;
                }
            }

            return NoContent();

        }

        // POST: api/Countires
        [HttpPost()]
        public async Task<ActionResult<Country>> PostCountry(CreateCountryDTO createCountryDTO)
        {
            var country = _mapper.Map<Country>(createCountryDTO);
            await _countriesRepository.AddAsync(country);
            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        // DELETE: api/Countires/6
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await _countriesRepository.GetAsync(id);
            if(country == null)
            {
                return NotFound();
            }
            await _countriesRepository.DeleteAsync(id);
            return NoContent();

        }

        private async Task<bool> CountryExists(int id)
        {
            return await _countriesRepository.Exists(id);
        }
    }
}
