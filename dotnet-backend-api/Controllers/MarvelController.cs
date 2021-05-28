using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_backend_api.Services.MarvelService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using dotnet_backend_api.Models;
using dotnet_backend_api.Dtos.Marvel;

namespace dotnet_backend_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MarvelController : ControllerBase
    {

        private readonly IMarvelService _marvelService;
        public MarvelController(IMarvelService marvelService)
        {
              _marvelService = marvelService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetMarvelDto>>>> Get()
        {
            return Ok(await _marvelService.GetAllMarvels());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetMarvelDto>>> GetSingle(int id)
        {
            return Ok(await _marvelService.GetMarvelById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetMarvelDto>>>> AddCharacter(AddMarvelDto newCharacter)
        {

            return Ok(await _marvelService.AddMarvel(newCharacter));
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<GetMarvelDto>>> UpdateCharacter(UpdateMarvelDto updateCharacterDto)
        {
            var response = await _marvelService.UpdateMarvel(updateCharacterDto);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete]
        public async Task<ActionResult<ServiceResponse<List<GetMarvelDto>>>> Delete(int id)
        {
            var response = await _marvelService.DeleteMarvel (id);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
