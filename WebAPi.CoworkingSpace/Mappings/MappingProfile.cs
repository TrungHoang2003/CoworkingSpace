using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace CoworkingSpace.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<VenueAddressDto, VenueAddress>();
        CreateMap<VenueDetailsDto, Venue>();
        CreateMap<UpdateVenueRequest, Venue>();
        CreateMap<BookingWindowDto, BookingWindow>();
        CreateMap<SignUpVenueRequest, Venue>();
        CreateMap<SetHourlySpaceRequest, Space>();
        CreateMap<GuestHourDto, GuestHour>();
        CreateMap<VenueAddressDto, VenueAddress>();
        CreateMap<AddBookingWindowRequest, BookingWindow>();
        CreateMap<UpdateBookingWindowRequest, BookingWindow>();
    }
}