using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Application.IServices;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Domain.Model;

namespace TSWebApplication.Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        public IEnumerable<Student> GetAllStudent()
        {
            return _studentRepository.GetAllStudent();
        }

        public List<StamppingDetails> GetStamppingList()
        {
            return _studentRepository.GetStamppingList();
        }
       

    }
}
