using System;
namespace dotnet_backend_api.Models
{
    public class ServiceResponse<T>
    {
        public T Data { get; set; }

        public bool Success { get; set; } = true;

        public string Message { get; set; } = null;
       
    }
}
