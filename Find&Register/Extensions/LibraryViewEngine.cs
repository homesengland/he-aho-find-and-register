using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Find_Register.Extensions
{
    public class LibraryViewEngine : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            yield return "/{1}/{0}.cshtml";
            yield return "/Shared/{0}.cshtml";
            yield return "Views/{1}/{0}.cshtml";
            yield return "Views/Shared/{0}.cshtml";
            yield return "Views/GenericErrors/{0}.cshtml";
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
        }
    }
}