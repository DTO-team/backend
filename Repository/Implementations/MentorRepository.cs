using Models.Models;
using Repository.Interfaces;


namespace Repository.Implementations
{
    public class MentorRepository : GenericRepository<Mentor>, IMentorRepository
    {
        public MentorRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
