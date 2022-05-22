using Models.Models;
using Repository.Interfaces;


namespace Repository.Implementations
{
    public class QuestionCopyRepository : GenericRepository<QuestionCopy>, IQuestionCopyRepository
    {
        public QuestionCopyRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
