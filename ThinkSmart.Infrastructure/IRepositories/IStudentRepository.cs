using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IStudentRepository
    {
        IEnumerable<Student> GetAllStudent();
        List<StamppingDetails> GetStamppingList();
        

    }
}
