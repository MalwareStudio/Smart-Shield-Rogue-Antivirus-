using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedIO
{
    public class advancedIO
    {
        public string[] GetDrives()
        {
            DriveInfo[] driveInfo = DriveInfo.GetDrives();

            List<string> drives = new List<string>();

            foreach (DriveInfo drive in driveInfo)
            {
                drives.Add(drive.Name);
            }

            return drives.ToArray();
        }
        /// <summary>
        /// Gets files safely without throwing exceptions unlike function GetFiles or EnumerateFiles.
        /// Files which cannot be access will be ignored.
        /// </summary>
        /// <param name="ticked">Enables function. When set to false, it returns empty List</param>
        /// <param name="path">Path to the directory for obtaining all files and files located in all subdirectories</param>
        /// <param name="fileExtention">Search only for specific files.
        /// Use it without "*." (e.g: "temp")</param>
        /// <returns></returns>
        private static List<string> allDirs = new List<string>();
        public static string currentFile = string.Empty;
        private static string[] subDirectories;
        public List<string> GetAllFilesSafely(bool? ticked, string path, string fileExtention)
        {
            if (ticked == false)
                return new List<string>();

            allDirs.Clear();
            List<string> collectedFiles = new List<string>();

            //Get files from the main directory
            try
            {
                string[] mainFiles = Directory.GetFiles(path, "*." + fileExtention);

                foreach (string mainFile in mainFiles)
                {
                    collectedFiles.Add(mainFile);
                }
            }
            catch { }

            IEnumerable<string> topDirectories = Directory.GetDirectories(path);

            //Get all directories and subdirectories
            foreach (string topdir in topDirectories)
            {
                allDirs.Add(topdir);
                TraverseDirs(topdir);
            }

            //Get all files from all collected directories
            string[] files = new string[0];
            foreach (string directory in allDirs)
            {
                try
                {
                    files = Directory.GetFiles(directory, "*." + fileExtention);
                    foreach (string file in files)
                    {
                        collectedFiles.Add(file);
                    }
                }
                catch { }
            }

            return collectedFiles;
        }
        public List<string> GetAllFilesSafely(bool? ticked, string[] paths, string fileExtention)
        {
            if (ticked == false)
                return new List<string>();

            allDirs.Clear();
            List<string> collectedFiles = new List<string>();

            string[] mainFiles = new string[0];
            string[] files = new string[0];
            foreach (string path in paths)
            {
                //Get files from the main directory
                try
                {
                    mainFiles = Directory.GetFiles(path, "*." + fileExtention);

                    foreach (string mainFile in mainFiles)
                    {
                        collectedFiles.Add(mainFile);
                    }
                }
                catch { }

                IEnumerable<string> topDirectories = Directory.GetDirectories(path);

                //Get all directories and subdirectories
                foreach (string topdir in topDirectories)
                {
                    allDirs.Add(topdir);
                    TraverseDirs(topdir);
                }

                //Get all files from all collected directories
                foreach (string directory in allDirs)
                {
                    try
                    {
                        files = Directory.GetFiles(directory, "*." + fileExtention);
                        foreach (string file in files)
                        {
                            collectedFiles.Add(file);
                        }
                    }
                    catch { }
                }
            }

            return collectedFiles;
        }

        private static List<string> collectedDirs = new List<string>();
        public List<string> GetAllDirectoriesSafely(string mainDir)
        {
            collectedDirs.Clear();
            try
            {
                string[] getTopDirs = Directory.GetDirectories(mainDir);

                foreach (string topDir in getTopDirs)
                {
                    collectedDirs.Add(topDir);
                    TraverseDirs(topDir);
                }
            }
            catch { return collectedDirs; }

            return collectedDirs;
        }

        private static void TraverseDirs(string dirPath)
        {
            try
            {
                subDirectories = Directory.GetDirectories(dirPath);

                foreach (string subDirectory in subDirectories)
                {
                    allDirs.Add(subDirectory);
                    TraverseDirs(subDirectory);
                }
            }
            catch { }
        }
    }
}
