// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TemporaryFileReference.cs" company="Matthias Friedrich">
//   Copyright © Matthias Friedrich 2009 - 2013
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FxCop.Setup
{
    using System;
    using System.IO;

    internal class TemporaryFileReference : IFileReference
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TemporaryFileReference" /> class.
        /// </summary>
        /// <param name="file">
        ///     The path to the temporary file.
        /// </param>
        public TemporaryFileReference(string file)
        {
            if (string.IsNullOrEmpty(file)) throw new ArgumentNullException(nameof(file));

            this.FullName = file;
        }

        public void Dispose()
        {
            try
            {
                File.Delete(this.FullName);
            }
            catch
            {
            }
        }

        /// <summary>
        ///     Gets a value indicating the absolute path of the file.
        /// </summary>
        public string FullName { get; }
    }
}