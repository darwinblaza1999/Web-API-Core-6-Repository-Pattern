using AutoMapper;
using WatchCatalogAPI.DTO;
using WatchCatalogAPI.Model;

namespace WatchCatalogAPI.AutoMapper
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap(typeof(ServiceResponse<>), typeof(ResponseMessage<>));
            CreateMap(typeof(Response<>), typeof(DTOResponse<>));
            CreateMap<JWTAuthModel, DTOJwtAuthModel>();
            CreateMap<WatchDetails1, DTOWatchModel>();
        }
    }
}
