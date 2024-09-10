using System.Reflection;

namespace Modules.CRM.Domain;

public class ApplicationAssemblyHook
{
    public static Assembly Assembly => typeof(ApplicationAssemblyHook).Assembly;
}