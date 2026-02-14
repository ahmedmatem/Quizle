using AutoMapper;
using Quizle.Core.Dtos;
using Quizle.Web.Areas.Student.Models;

namespace Quizle.Web.Automapper
{
    public class WebMappingProfile : Profile
    {
        protected WebMappingProfile()
        {
            CreateMap<SaveAnswerReq, SaveAnswerDto>().ReverseMap();
        }
    }
}
