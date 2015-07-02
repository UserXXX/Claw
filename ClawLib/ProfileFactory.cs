using Claw.Documents;
using Claw.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Claw
{
    /// <summary>
    /// A factory for profiles, used for loading and saving.
    /// </summary>
    public static class ProfileFactory
    {
        /// <summary>
        /// Loads a profile.
        /// </summary>
        /// <param name="fileName">The name of the file to load from.</param>
        /// <returns>The loaded profile.</returns>
        public static MadCatzProfile Load(string fileName)
        {
            return Load(fileName, new EmptyReport());
        }

        /// <summary>
        /// Loads a profile.
        /// </summary>
        /// <param name="file">The file to load from.</param>
        /// <returns>The loaded profile.</returns>
        public static MadCatzProfile Load(FileInfo file)
        {
            return Load(file, new EmptyReport());
        }

        /// <summary>
        /// Loads a profile.
        /// </summary>
        /// <param name="stream">The stream to load from. The stream will get closed after the operation.</param>
        /// <returns>The loaded profile.</returns>
        public static MadCatzProfile Load(Stream stream)
        {
            return Load(stream, new EmptyReport());
        }

        /// <summary>
        /// Loads a profile.
        /// </summary>
        /// <param name="fileName">The name of the file to load from.</param>
        /// <param name="report">The report to write infos, warnings and errors to.</param>
        /// <returns>The loaded profile.</returns>
        public static MadCatzProfile Load(string fileName, ValidationReport report)
        {
            FileStream stream = null;
            try
            {
                stream = new FileStream(fileName, FileMode.Open);
                MadCatzProfile profile = Load(stream, report);
                stream.Close();
                stream = null;
                return profile;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        /// <summary>
        /// Loads a profile.
        /// </summary>
        /// <param name="file">The file to load from.</param>
        /// <param name="report">The report to write infos, warnings and errors to.</param>
        /// <returns>The loaded profile.</returns>
        public static MadCatzProfile Load(FileInfo file, ValidationReport report)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }

            return Load(file.OpenRead(), report);
        }

        /// <summary>
        /// Loads a profile.
        /// </summary>
        /// <param name="stream">The stream to load from. The stream will get closed after the operation.</param>
        /// <param name="report">The report to write infos, warnings and errors to.</param>
        /// <returns>The loaded profile.</returns>
        public static MadCatzProfile Load(Stream stream, ValidationReport report)
        {
            Node node = DocumentFactory.Load(stream);
            NodeValidator validator = new NodeValidator(report);
            return new MadCatzProfile(validator, node);
        }

        /// <summary>
        /// Saves a profile.
        /// </summary>
        /// <param name="fileName">Name of the file to save to.</param>
        /// <param name="profile">The profile to save.</param>
        public static void Save(string fileName, MadCatzProfile profile)
        {
            FileStream stream = null;
            try
            {
                stream = new FileStream(fileName, FileMode.Create);
                Save(stream, profile);
                stream.Close();
                stream = null;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        /// <summary>
        /// Saves a profile.
        /// </summary>
        /// <param name="file">File to save to.</param>
        /// <param name="profile">The profile to save.</param>
        public static void Save(FileInfo file, MadCatzProfile profile)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }

            Save(file.Open(FileMode.Create), profile);
        }

        /// <summary>
        /// Saves a profile.
        /// </summary>
        /// <param name="stream">Stream to save to.</param>
        /// <param name="profile">The profile to save.</param>
        public static void Save(Stream stream, MadCatzProfile profile)
        {
            if (profile == null)
            {
                throw new ArgumentNullException("profile");
            }

            Node node = profile.CreateNodes();
            DocumentFactory.Save(node, stream);
        }
    }
}
