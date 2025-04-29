using Application.BookingWindowService.DTOs;
using Application.DTOs;
using Application.ExceptionService.DTOs;
using Application.VenueService.DTOs;
using AutoMapper;
using Domain.Entities;

namespace CoworkingSpace.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<VenueAddressDto, VenueAddress>();
        CreateMap<UpdateVenueRequest, Venue>();
        CreateMap<SetBookingWindowRequest, BookingWindow>();
        CreateMap<SignUpVenueRequest, Venue>();
        CreateMap<SetUpExceptionRequest, ExceptionRule>();
        CreateMap<SetHourlySpaceRequest, Space>();
    }
}