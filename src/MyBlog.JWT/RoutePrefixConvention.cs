using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Linq;

namespace MyBlog.JWT
{
    public class RoutePrefixConvention : IControllerModelConvention
    {
        private readonly string _prefix;

        public RoutePrefixConvention(string prefix)
        {
            _prefix = prefix;
        }

        public void Apply(ControllerModel controller)
        {
            if (controller.Selectors.Any())
            {
                foreach (var selector in controller.Selectors)
                {
                    var route = selector.AttributeRouteModel;
                    if (route == null)
                    {
                        selector.AttributeRouteModel = new AttributeRouteModel
                        {
                            Template = _prefix + "/" + controller.ControllerName
                        };
                    }
                    else
                    {
                        route.Template = _prefix + "/" + route.Template;
                    }
                }
            }
        }
    }
}
