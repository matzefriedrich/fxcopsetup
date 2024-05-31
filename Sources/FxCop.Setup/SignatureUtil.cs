// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignatureUtil.cs" company="Matthias Friedrich">
//   Copyright © Matthias Friedrich 2009 - 2013
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FxCop.Setup
{
    using System;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;

    internal static class SignatureUtil
    {
        public static string GetPublicKeyFrom(string signedFile)
        {
            if (string.IsNullOrWhiteSpace(signedFile)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(signedFile));
            var cert = X509Certificate.CreateFromSignedFile(signedFile);
            return ExtractPublicKey(cert);
        }

        private static string ExtractPublicKey(X509Certificate cert)
        {
            byte[] publicKey = cert.GetPublicKey();
            var sb = new StringBuilder();
            foreach (byte next in publicKey) sb.AppendFormat("{0:x2}", next);

            return sb.ToString();
        }
    }
}