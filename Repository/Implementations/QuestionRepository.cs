using Models.Models;
using Repository.Interfaces;


namespace Repository.Implementations
{
    public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
