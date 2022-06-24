using System;
namespace Models.Response
{
    public class GetDepartmentResponse{
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}