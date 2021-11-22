using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_backend_api.Data;
using dotnet_backend_api.Dtos.Marvel;
using dotnet_backend_api.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet_backend_api.Services.MarvelService
{
    public class MarvelService : IMarvelService
    {

        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public MarvelService(IMapper mapper, DataContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<GetMarvelDto>> AddMarvel(AddMarvelDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<GetMarvelDto>();
            Marvel character = _mapper.Map<Marvel>(newCharacter);
            _context.Marvels.Add(character);
            await _context.SaveChangesAsync();
            Marvel dbCharacter = _context.Marvels.OrderByDescending(m => m.Id).FirstOrDefault();
            serviceResponse.Data = _mapper.Map<GetMarvelDto>(dbCharacter);
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetMarvelDto>>> GetAllMarvels()
        {
            var serviceResponse = new ServiceResponse<List<GetMarvelDto>>();
            try
            {
                Console.WriteLine(Environment.GetEnvironmentVariable("In Service fetching"));
                var dbMarvels = await _context.Marvels.ToListAsync();
                Console.WriteLine(Environment.GetEnvironmentVariable("After fetch"));
                serviceResponse.Data = dbMarvels.Select(c => _mapper.Map<GetMarvelDto>(c)).ToList();
                return serviceResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine(Environment.GetEnvironmentVariable("Exception is " + ex.Message));
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
            
        }

        public async Task<ServiceResponse<GetMarvelDto>> GetMarvelById(int id)
        {
            var serviceResponse = new ServiceResponse<GetMarvelDto>();
            var dbCharacter = await _context.Marvels.FirstOrDefaultAsync(c => c.Id == id);
            serviceResponse.Data = _mapper.Map<GetMarvelDto>(dbCharacter);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetMarvelDto>> UpdateMarvel(UpdateMarvelDto updatedMarvel)
        {
            var serviceRespoonse = new ServiceResponse<GetMarvelDto>();
            try
            {
                Marvel marvel = await _context.Marvels.FirstOrDefaultAsync(c => c.Id == updatedMarvel.Id);
                marvel.Name = updatedMarvel.Name;
                marvel.RealName = updatedMarvel.RealName;
                marvel.Team = updatedMarvel.Team;
                marvel.CreatedBy = updatedMarvel.CreatedBy;
                marvel.Publisher = updatedMarvel.Publisher;
                marvel.ImageURL = updatedMarvel.ImageURL;
                marvel.Bio = updatedMarvel.Bio;
                await _context.SaveChangesAsync();
                serviceRespoonse.Data = _mapper.Map<GetMarvelDto>(marvel);
            }
            catch (Exception ex)
            {
                serviceRespoonse.Success = false;
                serviceRespoonse.Message = ex.Message;
            }

            return serviceRespoonse;
        }

        public async Task<ServiceResponse<List<GetMarvelDto>>> DeleteMarvel(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetMarvelDto>>();
            try
            {
                Marvel character = await _context.Marvels.FirstAsync(c => c.Id == id);
                _context.Marvels.Remove(character);
                serviceResponse.Data = _context.Marvels.Select(c => _mapper.Map<GetMarvelDto>(c)).ToList();
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

    }
}
