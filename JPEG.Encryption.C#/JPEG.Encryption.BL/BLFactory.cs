/// BLFactory.cs
/// 
/// This static class provides a factory for creating the business logic.
/// 
/// Authors:
/// Stefan Auer, Alexander Bliem
/// 
/// Date: 01.03.2013


using System;
using System.Collections.Generic;
using JPEG.Encryption.BL;
using System.Reflection;
using System.Configuration;

namespace JPEG.Encryption.BL
{
    /// <summary>
    /// Factory for generating business objects
    /// </summary>
    public class BLFactory
    {
        // name of the assembly that will be loaded
        private static string assemblyName;

        // the assembly itself
        private static Assembly blAssembly;

        /// <summary>
        /// load the factory (configuration in the App.config.xml)
        /// </summary>
        static BLFactory()
        {
            assemblyName = ConfigurationManager.AppSettings["BLAssembly"];
            blAssembly = Assembly.Load(assemblyName);
        }

        /// <summary>
        /// creates the business logic using reflections
        /// </summary>
        /// <returns>returns a IBusinessLogic object
        public static IBusinessLogic CreateBusinessLogic()
        {
            // use reflections to load assembly
            string businessLogicClassName = assemblyName + ".BusinessLogic";
            Type blClass = blAssembly.GetType(businessLogicClassName);
            return Activator.CreateInstance(blClass) as IBusinessLogic;
        }
    }
}