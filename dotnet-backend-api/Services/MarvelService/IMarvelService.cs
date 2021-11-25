using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dotnet_backend_api.Models;
using dotnet_backend_api.Dtos.Marvel;

namespace dotnet_backend_api.Services.MarvelService
{
    public interface IMarvelService
    {
        Task<ServiceResponse<List<GetMarvelDto>>> GetAllMarvels();

        Task<ServiceResponse<GetMarvelDto>> GetMarvelById(int id);

        Task<ServiceResponse<GetMarvelDto>> AddMarvel(AddMarvelDto newCharacter);

        Task<ServiceResponse<GetMarvelDto>> UpdateMarvel(UpdateMarvelDto updatedMarvel);

        Task<ServiceResponse<List<GetMarvelDto>>> DeleteMarvel(int id);
    }
}
