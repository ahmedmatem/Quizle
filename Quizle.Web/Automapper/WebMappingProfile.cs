using AutoMapper;
using Quizle.Core.Dtos;
using Quizle.Web.Areas.Student.Models;

namespace Quizle.Web.Automapper
{
    public class WebMappingProfile : Profile
    {
        public WebMappingProfile()
        {
            CreateMap<SaveAnswerDto, SaveAnswerReq>().ReverseMap();
            CreateMap<SolveQuestionDto, SolveQuestionVm>().ReverseMap();
            CreateMap<QuizDto, ActiveQuizCardVm>().ReverseMap();
        }
    }
}
