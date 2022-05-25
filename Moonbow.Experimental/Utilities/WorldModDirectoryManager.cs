using System.Collections.Generic;
using System.IO;
using System.Text;
using Plukit.Base;
using Staxel;

namespace AetharNet.Moonbow.Experimental.Utilities
{
    /// <summary>
    /// Scuffed directory manager for mods that store configuration files on a per-world basis.
    /// <example>
    /// <code>
    /// var directory = new WorldModDirectoryManager("modName");
    /// if (directory.FileExists("config.json"))
    /// {
    ///     var config = directory.ReadFileAsBlob("config.json");
    ///     // Do something with config
    /// }
    /// else
    /// {
    ///     directory.WriteFileFromBlob("config.example.json", existingBlob);
    /// }
    /// </code>
    /// </example>
    /// </summary>
    public class WorldModDirectoryManager
    {
        /// <summary>
        /// Path for the current world's <c>Mods</c> directory.
        /// </summary>
        public static readonly string WorldModDirectoryPath = Path.Combine(GameContext.ContentLoader.LocalContentDirectory, "Mods");
        
        public string ModName { get; }
        
        public WorldModDirectoryManager(string modName)
        {
            ModName = modName;

            if (!Directory.Exists(GetPath()))
            {
                Directory.CreateDirectory(GetPath());
            }
        }
        
        public string GetPath(string path = ".")
        {
            return Path.Combine(WorldModDirectoryPath, ModName, path);
        }

        // File Methods
        
        public IEnumerable<string> GetFiles(string path = ".")
        {
            return Directory.GetFiles(GetPath(path));
        }

        public bool FileExists(string fileName)
        {
            return File.Exists(GetPath(fileName));
        }

        public FileInfo GetFileInfo(string fileName)
        {
            return new(GetPath(fileName));
        }

        public string ReadFile(string fileName)
        {
            return File.ReadAllText(GetPath(fileName), Encoding.UTF8);
        }

        /// <summary>
        /// Retrieve a configuration file's content into a blob.
        /// </summary>
        /// <remarks>Be sure to call <c>Blob.Deallocate(ref blob)</c> once done.</remarks>
        /// <param name="fileName">The name of the file containing the data to read from. Can also be a file path, if the file is in a subdirectory.</param>
        /// <returns>Blob containing file data.</returns>
        public Blob ReadFileAsBlob(string fileName)
        {
            using var stream = OpenReadFileStream(fileName);
            var blob = BlobAllocator.Blob(true);
            
            blob.LoadJsonStream(stream);
            
            return blob;
        }
        
        public FileStream OpenReadFileStream(string fileName)
        {
            return File.OpenRead(GetPath(fileName));
        }

        public void WriteFile(string fileName, string data)
        {
            File.WriteAllText(GetPath(fileName), data, Encoding.UTF8);
        }

        public void WriteFileFromBlob(string fileName, Blob blob)
        {
            using var stream = OpenWriteFileStream(fileName);
            blob.SaveJsonStream(stream);
        }
        
        public FileStream OpenWriteFileStream(string fileName)
        {
            return File.OpenWrite(GetPath(fileName));
        }

        public void DeleteFile(string fileName)
        {
            File.Delete(GetPath(fileName));
        }
        
        // Directory Methods
        
        public IEnumerable<string> GetDirectories(string path = ".")
        {
            return Directory.GetDirectories(GetPath(path));
        }
        
        public bool DirectoryExists(string directoryName)
        {
            return Directory.Exists(GetPath(directoryName));
        }

        public DirectoryInfo GetDirectoryInfo(string directoryName)
        {
            return new(GetPath(directoryName));
        }

        public DirectoryInfo CreateDirectory(string directoryName)
        {
            return Directory.CreateDirectory(GetPath(directoryName));
        }

        public void DeleteDirectory(string directoryName, bool recursive = false)
        {
            Directory.Delete(GetPath(directoryName), recursive);
        }
    }
}