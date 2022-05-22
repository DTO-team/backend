using Models.Models;
using Repository.Interfaces;


namespace Repository.Implementations
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        public ProjectRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
