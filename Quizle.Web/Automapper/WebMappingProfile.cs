using AutoMapper;
using Quizle.Core.Dtos;
using Quizle.Web.Areas.Student.Models;
using Quizle.Web.Areas.Teacher.Models;

namespace Quizle.Web.Automapper
{
    public class WebMappingProfile : Profile
    {
        public WebMappingProfile()
        {
            // student-dashboard mappings
            CreateMap<SaveAnswerDto, SaveAnswerReq>().ReverseMap();
            CreateMap<SolveQuestionDto, SolveQuestionVm>().ReverseMap();
            CreateMap<OptionDto, OptionVm>().ReverseMap();
            CreateMap<QuizDto, ActiveQuizCardVm>().ReverseMap();

            // teacher-group mappings
            CreateMap<GroupListItemDto, GroupListItemVm>().ReverseMap();
            CreateMap<GroupDetailsDto, GroupDetailsVm>().ReverseMap();
            CreateMap<CreateGroupDto, CreateGroupVm>().ReverseMap();
            CreateMap<StudentRowDto, StudentRowVm>().ReverseMap();

            // teachers-questions mappings
            CreateMap<QuestionEditDto, QuestionEditVm>().ReverseMap();
            CreateMap<QuestionIndexDto, QuestionIndexVm>().ReverseMap();
            CreateMap<QuestionListItemDto, QuestionListItemVm>().ReverseMap();
            CreateMap<OptionInputDto, OptionInputVm>().ReverseMap();

            // teacher-quiz mappings
            CreateMap<QuizListItemDto, QuizListItemVm>().ReverseMap();
            CreateMap<QuizCreateDto, QuizCreateVm>().ReverseMap();
            CreateMap<QuizBuilderDto, QuizBuilderVm>().ReverseMap();
            CreateMap<QuizQuestionRowDto, QuizQuestionRowVm>().ReverseMap();
            CreateMap<QuestionBankRowDto, QuestionBankRowVm>().ReverseMap();
        }
    }
}
