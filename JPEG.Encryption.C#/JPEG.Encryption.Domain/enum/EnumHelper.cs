/// EnumHelper.cs
/// 
/// Generic helper class used to parse strings and convert to enums.
/// 
/// Authors:
/// Stefan Auer, Alexander Bliem
/// 
/// Date: 01.03.2013


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JPEG.Encryption.BL
{
    /// <summary>
    /// generic helper class used to parse strings and convert to enums
    /// </summary>
	public class EnumHelper
	{
        /// <summary>
        /// convert string to corresponding enum
        /// </summary>
        /// <typeparam name="T">the enum type</typeparam>
        /// <param name="name">name as string</param>
        /// <returns>enum object</returns>
        public static T StringToEnum<T>(string name)
        {
            return (T)Enum.Parse(typeof(T), name);
        }
	}
}
