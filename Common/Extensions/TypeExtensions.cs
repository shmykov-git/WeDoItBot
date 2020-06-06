using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common.Extensions
{
    public static class TypeExtensions
    {
	    public static readonly Type[] NumberTypes = { typeof(double), typeof(float), typeof(int), typeof(decimal) };

	    public static bool IsFloatNumberType<T>(this T value)
	    {
		    return value?.GetType().IsFloatNumberType() ?? false;
	    }

	    public static bool IsFloatNumberType(this Type type)
	    {
		    return NumberTypes.Contains(type);
	    }

	    public static bool HasInterface<T>(this Type type)
	    {
		    return type.GetInterfaces().Contains(typeof(T));
	    }

        public static string[] PropertiesNames(this Type type)
        {
	        return type.GetTypeInfo().DeclaredProperties.Select(p => p.Name).ToArray();
        }

        public static int CountGetters<T>(this T t)
        {
	        return t.GetType().GetTypeInfo().DeclaredProperties.Count(p => p.CanRead);
        }

        public static Dictionary<string, Func<object>> PropertiesGetters<T>(this T t)
        {
	        return t.GetType().GetTypeInfo().DeclaredProperties.Where(p => p.CanRead)
		        .Select(m => new
		        {
			        m.Name,
			        Getter = (Func<object>)(() => m.GetValue(t))
		        })
		        .ToDictionary(m => m.Name, m => m.Getter);
        }

		public static Dictionary<string, Action<object>> PropertiesSetters<T>(this T t)
		{
			return t.GetType().GetTypeInfo().DeclaredProperties.Where(p => p.CanWrite)
				.Select(m => new
				{
					m.Name,
					Setter = (Action<object>)(v => m.SetValue(t, v))
				})
				.ToDictionary(m => m.Name, m => m.Setter);
		}

        public static TAttribute GetAttribute<TAttribute>(this Type type, bool inherit = true) where TAttribute : Attribute
        {
	        return type.GetCustomAttributes<TAttribute>(inherit).FirstOrDefault();
        }
	}
}