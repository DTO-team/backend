using Models.Models;
using Repository.Interfaces;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Repository.Implementations
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        public StudentRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }

        public Student GetStudentWithTeamStudentsAndTeamById(Guid studentId)
        {
            Student student = dbSet
                .Include(stu => stu.TeamStudents)
                .Include(stu => stu.Teams)
                .Where(stu => stu.Id.Equals(studentId))
                .AsNoTracking()
                .FirstOrDefault();
            return student;
        }
    }
}
