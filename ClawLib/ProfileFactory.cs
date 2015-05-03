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
        /// <param name="filename">The name of the file to load from.</param>
        /// <returns>The loaded profile.</returns>
        public static Profile Load(string filename)
        {
            return Load(filename, new EmptyReport());
        }

        /// <summary>
        /// Loads a profile.
        /// </summary>
        /// <param name="file">The file to load from.</param>
        /// <returns>The loaded profile.</returns>
        public static Profile Load(FileInfo file)
        {
            return Load(file, new EmptyReport());
        }

        /// <summary>
        /// Loads a profile.
        /// </summary>
        /// <param name="stream">The stream to load from. The stream will get closed after the operation.</param>
        /// <returns>The loaded profile.</returns>
        public static Profile Load(Stream stream)
        {
            return Load(stream, new EmptyReport());
        }

        /// <summary>
        /// Loads a profile.
        /// </summary>
        /// <param name="filename">The name of the file to load from.</param>
        /// <param name="report">The report to write infos, warnings and errors to.</param>
        /// <returns>The loaded profile.</returns>
        public static Profile Load(string filename, ValidationReport report)
        {
            return Load(new FileStream(filename, FileMode.Open), report);
        }

        /// <summary>
        /// Loads a profile.
        /// </summary>
        /// <param name="file">The file to load from.</param>
        /// <param name="report">The report to write infos, warnings and errors to.</param>
        /// <returns>The loaded profile.</returns>
        public static Profile Load(FileInfo file, ValidationReport report)
        {
            return Load(file.OpenRead(), report);
        }

        /// <summary>
        /// Loads a profile.
        /// </summary>
        /// <param name="stream">The stream to load from. The stream will get closed after the operation.</param>
        /// <param name="report">The report to write infos, warnings and errors to.</param>
        /// <returns>The loaded profile.</returns>
        public static Profile Load(Stream stream, ValidationReport report)
        {
            Node node = DocumentFactory.Instance.Load(stream);
            NodeValidator validator = new NodeValidator(report);
            return new Profile(validator, node);
        }

        /// <summary>
        /// Saves a profile.
        /// </summary>
        /// <param name="filename">Name of the file to save to.</param>
        /// <param name="profile">The profile to save.</param>
        public static void Save(string filename, Profile profile)
        {
            Save(new FileStream(filename, FileMode.Create), profile);
        }

        /// <summary>
        /// Saves a profile.
        /// </summary>
        /// <param name="file">File to save to.</param>
        /// <param name="profile">The profile to save.</param>
        public static void Save(FileInfo file, Profile profile)
        {
            Save(file.Open(FileMode.Create), profile);
        }

        /// <summary>
        /// Saves a profile.
        /// </summary>
        /// <param name="stream">Stream to save to.</param>
        /// <param name="profile">The profile to save.</param>
        public static void Save(Stream stream, Profile profile)
        {
            Node node = profile.CreateNodes();
            DocumentFactory.Instance.Save(node, stream);
        }
    }
}
