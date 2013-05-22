// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFileReference.cs" company="Matthias Friedrich">
//   Copyright © Matthias Friedrich 2009 - 2013
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FxCop.Setup
{
    using System;

    internal interface IFileReference : IDisposable
    {
        /// <summary>
        ///     Gets a value indicating the absolute path of the file.
        /// </summary>
        string FullName { get; }
    }
}