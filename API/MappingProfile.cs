using API.Models;
using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<NewQuestionDto, Question>()
            .ForMember(dest => dest.QuestionDateAndTime, opt => opt.MapFrom(src => DateTime.Now));
        CreateMap<NewAnswerDto, Answer>()
             .ForMember(dest => dest.AnswerDateAndTime, opt => opt.MapFrom(src => DateTime.Now));
    }
}
