using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;

namespace TSWebApplication.Application.IServices
{
    public interface IStudentService
    {
        IEnumerable<Student> GetAllStudent();
        List<StamppingDetails> GetStamppingList();



    }
}
