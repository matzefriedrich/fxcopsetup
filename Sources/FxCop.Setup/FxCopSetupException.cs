// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FxCopSetupException.cs" company="Matthias Friedrich">
//   Copyright © Matthias Friedrich 2009 - 2013
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FxCop.Setup
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class FxCopSetupException : Exception
    {
        public FxCopSetupException()
        {
        }

        public FxCopSetupException(string message)
            : base(message)
        {
        }

        public FxCopSetupException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected FxCopSetupException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}