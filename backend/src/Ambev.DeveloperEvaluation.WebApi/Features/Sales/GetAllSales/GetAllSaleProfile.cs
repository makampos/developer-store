using Ambev.DeveloperEvaluation.Application.Sales.GetAllSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetAllSale;

public class GetAllSaleProfile : Profile
{
    public GetAllSaleProfile()
    {
        CreateMap<GetAllSaleRequest, GetAllSaleCommand>();
    }
}