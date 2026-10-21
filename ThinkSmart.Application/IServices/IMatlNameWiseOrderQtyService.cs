using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;

namespace TSWebApplication.Application.IServices
{
    public interface IMatlNameWiseOrderQtyService
    {
   
        List<MatlNameWiseOrderQtyDetails> GetMaterialOrderQtyData(int Batch_Roll);
    }
}
