using DevJourney.Application.Interfaces.Infrastructure.Persistence.Repositories.Courses;
using DevJourney.Infrastructure.Persistence.Context;

namespace DevJourney.Infrastructure.Persistence.Repositories.Courses;

public class CourseRepository(AppDbContext context) : ICourseRepository
{
}