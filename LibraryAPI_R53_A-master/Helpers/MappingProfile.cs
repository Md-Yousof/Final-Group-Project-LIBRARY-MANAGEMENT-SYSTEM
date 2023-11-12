using AutoMapper;
using LibraryAPI_R53_A.Core.Domain;
using LibraryAPI_R53_A.DTOs;

namespace LibraryAPI_R53_A.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PublisherDto, Publisher>();
            CreateMap<SubscriptionPlanDto, SubscriptionPlan>();
            CreateMap<CategoryDto, Category>();

            CreateMap<BookDto, Book>()
            .ForMember(dest => dest.Cover, opt => opt.Ignore())
            .ForMember(dest => dest.EBook, opt => opt.Ignore())
            .ForMember(dest => dest.AgreementBookCopy, opt => opt.Ignore());
        }
    }
}
