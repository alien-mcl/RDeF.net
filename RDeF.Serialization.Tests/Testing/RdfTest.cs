using System.Reflection;
using System.Text.RegularExpressions;

namespace RDeF.Testing
{
    public abstract class RdfTest
    {
        protected string EmbeddedResourceExtension
        {
            get
            {
                return Regex.Match(GetType().GetTypeInfo().FullName, "\\.([A-Z][a-z]+).*(Reader|Writer)_class").Groups[1].Value.ToLower();
            }
        }
    }
}
