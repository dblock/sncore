using System;
using System.Reflection;
using NHibernate;

namespace SnCore.Services
{
    /// <summary>
    /// Managed system information.
    /// </summary>
    public class ManagedSystem : ManagedService
    {
        public ManagedSystem(ISession session)
            : base(session)
        {
        }

        /// <summary>
        /// System version.
        /// </summary>
        public static string Version
        {
            get
            {
                return string.Format("{0}.{1}.{2}.{3}",
                    Assembly.GetAssembly(typeof(ManagedSystem)).GetName().Version.Major,
                    Assembly.GetAssembly(typeof(ManagedSystem)).GetName().Version.Minor,
                    Assembly.GetAssembly(typeof(ManagedSystem)).GetName().Version.Build,
                    Assembly.GetAssembly(typeof(ManagedSystem)).GetName().Version.Revision);
            }
        }

        /// <summary>
        /// Product version.
        /// </summary>
        public static string ProductVersion
        {
            get
            {
                return string.Format("{0}.{1}",
                    Assembly.GetAssembly(typeof(ManagedSystem)).GetName().Version.Major,
                    Assembly.GetAssembly(typeof(ManagedSystem)).GetName().Version.Minor);
            }
        }

        /// <summary>
        /// Build number.
        /// </summary>
        public static string ProductBuild
        {
            get
            {
                return string.Format("{0}.{1}",
                    Assembly.GetAssembly(typeof(ManagedSystem)).GetName().Version.Build,
                    Assembly.GetAssembly(typeof(ManagedSystem)).GetName().Version.Revision);
            }
        }

        /// <summary>
        /// System title.
        /// </summary>
        public static string Title
        {
            get
            {
                return ((AssemblyTitleAttribute)AssemblyTitleAttribute.GetCustomAttribute(
                    Assembly.GetAssembly(typeof(ManagedSystem)), typeof(AssemblyTitleAttribute))).Title;
            }
        }

        /// <summary>
        /// Product copyright.
        /// </summary>
        public static string Copyright
        {
            get
            {
                return ((AssemblyCopyrightAttribute)AssemblyCopyrightAttribute.GetCustomAttribute(
                    Assembly.GetAssembly(typeof(ManagedSystem)), typeof(AssemblyCopyrightAttribute))).Copyright;
            }
        }

        /// <summary>
        /// Product description.
        /// </summary>
        public static string Description
        {
            get
            {
                return ((AssemblyDescriptionAttribute)AssemblyDescriptionAttribute.GetCustomAttribute(
                    Assembly.GetAssembly(typeof(ManagedSystem)), typeof(AssemblyDescriptionAttribute))).Description;
            }
        }
    }
}
