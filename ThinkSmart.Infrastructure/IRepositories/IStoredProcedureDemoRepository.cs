using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IStoredProcedureDemoRepository
    {
        DataTable Trial1();

        DataTable Trial2();
        DataTable Trial3();
    }
}
