using System;
namespace dotnet_backend_api.Dtos.Marvel
{
    public class AddMarvelDto
    {
        public string Name { get; set; }
        public string RealName { get; set; }
        public string Team { get; set; }
        public string CreatedBy { get; set; }
        public string Publisher { get; set; }
        public string ImageURL { get; set; }
        public string Bio { get; set; }

    }
}
