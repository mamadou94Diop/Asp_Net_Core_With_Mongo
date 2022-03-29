using AutoMapper;
using DriveMeShop.Entity;

namespace DriveMeShop.Mapper
{
    public class CarProfile : Profile
    {
        public CarProfile()
        {
            createMapperV1();
            createMapperV2();
        }

        private void createMapperV1()
        {
            CreateMap<Model.V1.UnidentifiedCarModel, Car>()
                          .ForMember(destination => destination.TransmissionMode, options => options.MapFrom(source => source.IsTransmissionAutomatic ? "AUTOMATIC" : "MANUAL"));

            CreateMap<Model.V1.IdentifiedCarModel, Car>()
            .ForMember(destination => destination.TransmissionMode, options => options.MapFrom(source => source.IsTransmissionAutomatic ? "AUTOMATIC" : "MANUAL"));


            CreateMap<Car, Model.V1.IdentifiedCarModel>()
            .ForMember(destination => destination.IsTransmissionAutomatic, options => options.MapFrom(source => source.TransmissionMode == "AUTOMATIC"));
        }

        private void createMapperV2()
        {
            CreateMap<Model.V2.UnidentifiedCarModel, Car>()
                         .ForMember(destination => destination.TransmissionMode, options => options.MapFrom(source => source.IsTransmissionAutomatic ? "AUTOMATIC" : "MANUAL"));

            CreateMap<Model.V2.IdentifiedCarModel, Car>()
            .ForMember(destination => destination.TransmissionMode, options => options.MapFrom(source => source.IsTransmissionAutomatic ? "AUTOMATIC" : "MANUAL"));


            CreateMap<Car, Model.V2.IdentifiedCarModel>()
            .ForMember(destination => destination.IsTransmissionAutomatic, options => options.MapFrom(source => source.TransmissionMode == "AUTOMATIC"));
        }

    }
}
