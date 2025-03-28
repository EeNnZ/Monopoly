using AutoMapper;
using Monopoly.Warehouse.Core.Domain.Warehouse.Entities;
using Monopoly.Warehouse.WebHost.Models.Box;

namespace Monopoly.Warehouse.WebHost.Mapping;

/// <summary>
/// 
/// </summary>
public class AutoMapperProfile : Profile
{
    /// <summary>
    /// 
    /// </summary>
    public AutoMapperProfile()
    {
        CreateMap<Box, BoxShortResponse>();
        //TODO Map boxes and pallets
    }
}