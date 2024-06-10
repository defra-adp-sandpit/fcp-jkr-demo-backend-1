using Fcp.Jkr.Demo.Backend.1.Api.Models;
using Fcp.Jkr.Demo.Backend.1.Core.Entities;

using AutoMapper;

namespace Fcp.Jkr.Demo.Backend.1.Api.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<ItemRequest, Item>();
    }
}
