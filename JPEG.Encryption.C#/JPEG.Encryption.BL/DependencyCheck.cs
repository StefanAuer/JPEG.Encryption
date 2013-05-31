/// DependencyCheck.cs
/// 
/// This class checks if the necessary files are copied into the
/// current directory
/// 
/// Authors:
/// Stefan Auer, Alexander Bliem
/// 
/// Date: 22.04.2013

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JPEG.Encryption.BL
{
    public class DependencyCheck
    {
        // locker object
        private static object locker = new object();

        /// <summary>
        /// copies files from a directory in the app.config.xml and copies it 
        /// to the current directory
        /// </summary>
        /// <param name="appSettingString">the "key" string of the appSettings</param>
        /// <param name="copyIfNewer">copy if file is newer</param>
        /// <returns>true if successful</returns>
        public static bool LoadLibrary(string appSettingString, bool copyIfNewer=false)
        {
            // get the current directory and add a directory seperator (e.g. Windows: '\\')
            string currentDirectory = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar;
            
            // get the directory 
            string pathToLibraryFiles = ConfigurationManager.AppSettings[appSettingString];
            if (pathToLibraryFiles != "")
            {
                // avoid multiple file access attemps (e.g. during UnitTests)
                lock (locker)
                {
                    // get the files
                    string[] fileNames = Directory.GetFiles(pathToLibraryFiles);

                    // there has to be files in the given directory
                    if (fileNames.Count() > 0)
                    {
                        // for each file check: copy if necessary
                        foreach (string fileName in fileNames)
                        {
                            string fullFileName = currentDirectory + Path.GetFileName(fileName);
                            Console.Out.WriteLine(File.Exists(fullFileName).ToString() + ", " + File.GetCreationTime(fullFileName).Ticks + ", " + File.GetCreationTime(fileName).Ticks);
                            // copy file if not exists or newer
                            if (!File.Exists(fullFileName) ||
                                 (copyIfNewer == true && File.GetCreationTime(fullFileName).Date != File.GetCreationTime(fileName).Date))
                            {
                                File.Copy(fileName, currentDirectory + Path.GetFileName(fileName), true);
                                File.SetCreationTime(fullFileName, File.GetCreationTime(fileName));
                            }
                        }
                        return true;
                    }
                    else
                        return false;
                }
            }
            else
                return false;
        }

        /// <summary>
        /// execute the dependency check 
        /// </summary>
        /// <returns>true if successful</returns>
        public static bool Execute()
        {
            // load OpenCV and bridge.dll
            return LoadLibrary("PathToOpenCV") && LoadLibrary("PathToBridge", true);
        }
    }
}
