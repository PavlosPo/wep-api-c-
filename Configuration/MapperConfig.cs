using System;
using AutoMapper;
using WebApiApp.Data;
using WebApiApp.DTO;

namespace WebApiApp.Configuration
{
	public class MapperConfig : Profile
	{
		public MapperConfig()
		{
			CreateMap<CustomerCreateDTO, Data.Customer>().ReverseMap();
            CreateMap<CustomerUpdateDTO, Data.Customer>().ReverseMap();
            CreateMap<CustomerReadOnlyDTO, Data.Customer>().ReverseMap();
			CreateMap<OrderCreateDTO, Order>().ReverseMap();
            CreateMap<OrderUpdateDTO, Order>().ReverseMap();
			CreateMap<Order, OrderReadOnlyDTO>().ForMember(q =>
				q.CustomerName, d => d.MapFrom(map =>
					$"{map.Customer!.Firstname} {map.Customer.Lastname}")).ReverseMap();
        }
	}
}

