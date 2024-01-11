using AutoMapper;
using Core.Entites.Concrete;
using Core.Helpers;
using Entities.DTO;

namespace Business.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap(typeof(PagedResult<UserDto>), typeof(PagedResult<User>)).ReverseMap();

            CreateMap<OperationClaimDto, OperationClaim>().ReverseMap();
            CreateMap(typeof(PagedResult<OperationClaimDto>), typeof(PagedResult<OperationClaim>)).ReverseMap();

            CreateMap<UserOperationClaimDto, UserOperationClaim>().ReverseMap();
            CreateMap(typeof(PagedResult<UserOperationClaimDto>), typeof(PagedResult<UserOperationClaim>)).ReverseMap();
        }
    }

}
