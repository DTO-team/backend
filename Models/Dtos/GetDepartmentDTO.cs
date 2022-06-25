using System;
namespace Models.Dtos
{
    public class GetDepartmentDTO{
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}