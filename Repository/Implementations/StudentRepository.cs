using Models.Models;
using Repository.Interfaces;


namespace Repository.Implementations
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        public StudentRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
