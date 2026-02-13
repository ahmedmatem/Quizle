using Quizle.Core.Contracts;
using Quizle.Infrastructure.Data.Repositories;

namespace Quizle.Web
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IQuizRepository, IQuizRepository>();
            services.AddScoped<ISchoolGroupRepository, SchoolGroupRepository>();
            services.AddScoped<IQuizAttemptRepository, QuizAttemptRepository>();

            return services;
        }
    }
}
