using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSWebApplication.Web.Models
{
    public interface IViewRenderService
    {
        string RenderToString(string viewName, object model);
    }
}
