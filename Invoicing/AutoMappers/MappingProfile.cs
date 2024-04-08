using AutoMapper;
using Invoicing.DTOs;
using Invoicing.Models;

namespace Invoicing.AutoMappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductInsertDTO, Product>();
            CreateMap<Product, ProductDTO>().ForMember(dto => dto.Id, m => m.MapFrom(o => o.ProductId));
            CreateMap<ProductUpdateDTO, Product>();

            CreateMap<ClientInsertDTO, Client>();
            CreateMap<Client, ClientDTO>().ForMember(dto => dto.Id, m => m.MapFrom(o => o.ClientId));
            CreateMap<ClientUpdateDTO, Client>();

            CreateMap<InvoiceInsertDTO, Invoice>();
            CreateMap<Invoice, InvoiceDTO>().ForMember(dto => dto.Hash, m => m.MapFrom(o => o.Hash));

            CreateMap<ItemInsertDTO, Item>();
            CreateMap<Item, ItemDTO>();
        }
    }
}
