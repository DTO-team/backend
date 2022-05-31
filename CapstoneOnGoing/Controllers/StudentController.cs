using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;
using System;

namespace CapstoneOnGoing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public StudentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetStudent()
        {
            var student = _unitOfWork.Student.GetById(new Guid("0AA89F67-3F20-44C8-9908-422B8897CB25"));
            return Ok(student);
        }
    }
}
