using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace TSWebApplication.Application.IServices
{
    public interface IStoredProcedureDemoServices
    {
        DataTable TrialDemo1();
        DataTable TrialDemo2();
        DataTable TrialDemo3();
    }
}
