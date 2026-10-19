using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IMatlNameWiseOrderQtyRepository
    {
   
        List<MatlNameWiseOrderQtyDetails> GetMaterialOrderQtyData(int Batch_Roll);
    }
}
