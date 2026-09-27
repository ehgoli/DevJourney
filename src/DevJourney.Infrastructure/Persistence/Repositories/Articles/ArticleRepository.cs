using DevJourney.Application.Interfaces.Infrastructure.Persistence.Repositories.Articles;
using DevJourney.Infrastructure.Persistence.Context;

namespace DevJourney.Infrastructure.Persistence.Repositories.Articles;

public class ArticleRepository(AppDbContext context) : IArticleRepository
{
}