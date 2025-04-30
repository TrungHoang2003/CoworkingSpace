using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace CoworkingSpace.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<VenueAddressDto, VenueAddress>();
        CreateMap<UpdateVenueRequest, Venue>();
        CreateMap<BookingWindowDto, BookingWindow>();
        CreateMap<SignUpVenueRequest, Venue>();
        CreateMap<SetHourlySpaceRequest, Space>();
        CreateMap<GuestHourDto, GuestHour>();
    }
}