using System;
using AutoMapper;
using DriveMeShop.Entity;
using DriveMeShop.Model;

namespace DriveMeShop.Mapper
{
    public class CarProfile : Profile
    {
        public CarProfile()
        {
            CreateMap<UnidentifiedCarModel, Car>()
              .ForMember(destination => destination.TransmissionMode, options => options.MapFrom(source => source.IsTransmissionAutomatic ? "AUTOMATIC" : "MANUAL"));

            CreateMap<IdentifiedCarModel, Car>()
            .ForMember(destination => destination.TransmissionMode, options => options.MapFrom(source => source.IsTransmissionAutomatic ? "AUTOMATIC" : "MANUAL"));


            CreateMap<Car, IdentifiedCarModel>()
            .ForMember(destination => destination.IsTransmissionAutomatic, options => options.MapFrom(source => source.TransmissionMode == "AUTOMATIC"));
        }
    }
}
