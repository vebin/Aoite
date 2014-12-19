using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Aoite.Reflection
{
    /// <summary>
    /// Extension methods for inspecting assemblies.
    /// </summary>
    public static class AssemblyExtensions
    {
        #region Types
        /// <summary>
        /// Gets all types in the given <paramref name="assembly"/> matching the optional list 
        /// <paramref name="names"/>.
        /// </summary>
        /// <param name="assembly">The assembly in which to look for types.</param>
        /// <param name="names">An optional list of names against which to filter the value.  If this is
        /// <c>null</c> or left empty, all types are returned.</param>
        /// <returns>A list of all matching types. This method never returns null.</returns>
        public static IList<Type> Types(this Assembly assembly, params string[] names)
        {
            return assembly.Types(Flags.None, names);
        }

        /// <summary>
        /// Gets all types in the given <paramref name="assembly"/> matching the specified
        /// <paramref name="bindingFlags"/> and the optional list <paramref name="names"/>.
        /// </summary>
        /// <param name="assembly">The assembly in which to look for types.</param>
        /// <param name="bindingFlags">The <see cref="BindingFlags"/> used to customize how results
        /// are filters. If the <see href="Flags.PartialNameMatch"/> option is specified any name
        /// comparisons will use <see href="String.Contains"/> instead of <see href="String.Equals"/>.</param>
        /// <param name="names">An optional list of names against which to filter the value.  If this is
        /// <c>null</c> or left empty, all types are returned.</param>
        /// <returns>A list of all matching types. This method never returns null.</returns>
        public static IList<Type> Types(this Assembly assembly, Flags bindingFlags, params string[] names)
        {
            Type[] types = assembly.GetTypes();

            bool hasNames = names != null && names.Length > 0;
            bool partialNameMatch = bindingFlags.IsSet(Flags.PartialNameMatch);

            return hasNames
                    ? types.Where(t => names.Any(n => partialNameMatch ? t.Name.Contains(n) : t.Name == n)).ToArray()
                    : types;
        }
        #endregion

        #region TypesImplementing
        /// <summary>
        /// Gets all types in the given <paramref name="assembly"/> that implement the given <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The interface types should implement.</typeparam>
        /// <param name="assembly">The assembly in which to look for types.</param>
        /// <returns>A list of all matching types. This method never returns null.</returns>
        public static IList<Type> TypesImplementing<T>(this Assembly assembly)
        {
            Type[] types = assembly.GetTypes();
            return types.Where(t => t.Implements<T>()).ToList();
        }
        #endregion

        #region TypesWith Lookup
        /// <summary>
        /// Gets all types in the given <paramref name="assembly"/> that are decorated with an
        /// <see href="Attribute"/> of the given <paramref name="attributeType"/>.
        /// </summary>
        /// <returns>A list of all matching types. This value will never be null.</returns>
        public static IList<Type> TypesWith(this Assembly assembly, Type attributeType)
        {
            IEnumerable<Type> query = from t in assembly.GetTypes()
                                      where t.HasAttribute(attributeType)
                                      select t;
            return query.ToArray();
        }

        /// <summary>
        /// Gets all types in the given <paramref name="assembly"/> that are decorated with an
        /// <see href="Attribute"/> of the given type <typeparamref name="T"/>.
        /// </summary>
        /// <returns>A list of all matching types. This value will never be null.</returns>
        public static IList<Type> TypesWith<T>(this Assembly assembly) where T : Attribute
        {
            return assembly.TypesWith(typeof(T));
        }
        #endregion
    }
}