using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IWIPTagCombineRepository
    {
        WIPTagCombineDetails GetTagDetailsByCombineCD(int combineCD);
        void Delete(int combineCD);
    }
}
