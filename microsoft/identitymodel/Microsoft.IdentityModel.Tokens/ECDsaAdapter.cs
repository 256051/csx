﻿//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    delegate ECDsa CreateECDsaDelegate(JsonWebKey jsonWebKey, bool usePrivateKey);

    /// <summary>
    /// This adapter abstracts the <see cref="ECDsa"/> differences between versions of .Net targets.
    /// </summary>
    internal class ECDsaAdapter
    {
        internal readonly CreateECDsaDelegate CreateECDsaFunction = null;
        internal static ECDsaAdapter Instance = new ECDsaAdapter();

        /// <summary>
        /// Initializes a new instance of the <see cref="ECDsaAdapter"/> class.
        /// </summary>
        /// <exception cref="PlatformNotSupportedException">
        /// <see cref="ECDsa"/> creation is not supported by some platforms.
        /// For more details, see https://aka.ms/IdentityModel/create-ecdsa.
        /// </exception>
        internal ECDsaAdapter()
        {
#if NETSTANDARD2_0
            if (SupportsECParameters())
                CreateECDsaFunction = CreateECDsaUsingECParams;
            else
                CreateECDsaFunction = CreateECDsaUsingCNGKey;
#elif DESKTOP
            CreateECDsaFunction = CreateECDsaUsingCNGKey;
#endif
        }

        /// <summary>
        /// Creates an ECDsa object using the <paramref name="jsonWebKey"/> and <paramref name="usePrivateKey"/>.
        /// </summary>
        internal ECDsa CreateECDsa(JsonWebKey jsonWebKey, bool usePrivateKey)
        {
            if (CreateECDsaFunction != null)
                return CreateECDsaFunction(jsonWebKey, usePrivateKey);

            // we will get here on platforms that are not supported.
            throw LogHelper.LogExceptionMessage(new PlatformNotSupportedException(LogMessages.IDX10690));
        }

        /// <summary>
        /// Creates an ECDsa object using the <paramref name="jsonWebKey"/> and <paramref name="usePrivateKey"/>.
        /// 'ECParameters' structure is available in .NET Framework 4.7+, .NET Standard 1.6+, and .NET Core 1.0+.
        /// This method is supported only on Windows as other platforms don't support operations with <see cref="CngKey"/>.
        /// </summary>
        private ECDsa CreateECDsaUsingCNGKey(JsonWebKey jsonWebKey, bool usePrivateKey)
        {
            if (jsonWebKey == null)
                throw LogHelper.LogArgumentNullException(nameof(jsonWebKey));

            if (jsonWebKey.Crv == null)
                throw LogHelper.LogArgumentNullException(nameof(jsonWebKey.Crv));

            if (jsonWebKey.X == null)
                throw LogHelper.LogArgumentNullException(nameof(jsonWebKey.X));

            if (jsonWebKey.Y == null)
                throw LogHelper.LogArgumentNullException(nameof(jsonWebKey.Y));

            GCHandle keyBlobHandle = new GCHandle();
            try
            {
                uint dwMagic = GetMagicValue(jsonWebKey.Crv, usePrivateKey);
                uint cbKey = GetKeyByteCount(jsonWebKey.Crv);
                byte[] keyBlob;
#if NET45
                if (usePrivateKey)
                    keyBlob = new byte[3 * cbKey + 2 * Marshal.SizeOf(typeof(uint))];
                else
                    keyBlob = new byte[2 * cbKey + 2 * Marshal.SizeOf(typeof(uint))];
#else
                if (usePrivateKey)
                    keyBlob = new byte[3 * cbKey + 2 * Marshal.SizeOf<uint>()];
                else
                    keyBlob = new byte[2 * cbKey + 2 * Marshal.SizeOf<uint>()];
#endif

                keyBlobHandle = GCHandle.Alloc(keyBlob, GCHandleType.Pinned);
                IntPtr keyBlobPtr = keyBlobHandle.AddrOfPinnedObject();

                byte[] x = Base64UrlEncoder.DecodeBytes(jsonWebKey.X);
                if (x.Length > cbKey)
                    throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(nameof(jsonWebKey), LogHelper.FormatInvariant(LogMessages.IDX10675, nameof(jsonWebKey), nameof(jsonWebKey.X), cbKey, x.Length)));

                byte[] y = Base64UrlEncoder.DecodeBytes(jsonWebKey.Y);
                if (y.Length > cbKey)
                    throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(nameof(jsonWebKey), LogHelper.FormatInvariant(LogMessages.IDX10675, nameof(jsonWebKey), nameof(jsonWebKey.Y), cbKey, y.Length)));

                Marshal.WriteInt64(keyBlobPtr, 0, dwMagic);
                Marshal.WriteInt64(keyBlobPtr, 4, cbKey);

                int index = 8;
                foreach (byte b in x)
                    Marshal.WriteByte(keyBlobPtr, index++, b);

                foreach (byte b in y)
                    Marshal.WriteByte(keyBlobPtr, index++, b);

                if (usePrivateKey)
                {
                    if (jsonWebKey.D == null)
                        throw LogHelper.LogArgumentNullException(nameof(jsonWebKey.D));

                    byte[] d = Base64UrlEncoder.DecodeBytes(jsonWebKey.D);
                    if (d.Length > cbKey)
                        throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(nameof(jsonWebKey), LogHelper.FormatInvariant(LogMessages.IDX10675, nameof(jsonWebKey), nameof(jsonWebKey.D), cbKey, d.Length)));

                    foreach (byte b in d)
                        Marshal.WriteByte(keyBlobPtr, index++, b);

                    Marshal.Copy(keyBlobPtr, keyBlob, 0, keyBlob.Length);
                    using (CngKey cngKey = CngKey.Import(keyBlob, CngKeyBlobFormat.EccPrivateBlob))
                    {
                        return new ECDsaCng(cngKey);
                    }
                }
                else
                {
                    Marshal.Copy(keyBlobPtr, keyBlob, 0, keyBlob.Length);
                    using (CngKey cngKey = CngKey.Import(keyBlob, CngKeyBlobFormat.EccPublicBlob))
                    {
                        return new ECDsaCng(cngKey);
                    }
                }
            }
            catch (Exception ex)
            {
                throw LogHelper.LogExceptionMessage(new CryptographicException(LogMessages.IDX10689, ex));
            }
            finally
            {
                if (keyBlobHandle != null && keyBlobHandle.IsAllocated)
                    keyBlobHandle.Free();
            }
        }

        /// <summary>
        /// Returns the size of key in bytes
        /// </summary>
        /// <param name="curveId">Represents ecdsa curve -P256, P384, P521</param>
        /// <returns>Size of the key in bytes</returns>
        private static uint GetKeyByteCount(string curveId)
        {
            if (string.IsNullOrEmpty(curveId))
                throw LogHelper.LogArgumentNullException(nameof(curveId));

            uint keyByteCount;
            switch (curveId)
            {
                case JsonWebKeyECTypes.P256:
                    keyByteCount = 32;
                    break;
                case JsonWebKeyECTypes.P384:
                    keyByteCount = 48;
                    break;
                case JsonWebKeyECTypes.P512: // treat 512 as 521. 512 doesn't exist, but we released with "512" instead of "521", so don't break now.
                case JsonWebKeyECTypes.P521:
                    keyByteCount = 66;
                    break;
                default:
                    throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10645, curveId)));
            }
            return keyByteCount;
        }

        /// <summary>
        /// Magic numbers identifying ECDSA blob types
        /// </summary>
        private enum KeyBlobMagicNumber : uint
        {
            BCRYPT_ECDSA_PUBLIC_P256_MAGIC = 0x31534345,
            BCRYPT_ECDSA_PUBLIC_P384_MAGIC = 0x33534345,
            BCRYPT_ECDSA_PUBLIC_P521_MAGIC = 0x35534345,
            BCRYPT_ECDSA_PRIVATE_P256_MAGIC = 0x32534345,
            BCRYPT_ECDSA_PRIVATE_P384_MAGIC = 0x34534345,
            BCRYPT_ECDSA_PRIVATE_P521_MAGIC = 0x36534345,
        }

        /// <summary>
        /// Returns the magic value representing the curve corresponding to the curve id.
        /// </summary>
        /// <param name="curveId">Represents ecdsa curve -P256, P384, P512</param>
        /// <param name="willCreateSignatures">Whether the provider will create signatures or not</param>
        /// <returns>Uint representing the magic number</returns>
        private static uint GetMagicValue(string curveId, bool willCreateSignatures)
        {
            if (string.IsNullOrEmpty(curveId))
                throw LogHelper.LogArgumentNullException(nameof(curveId));

            KeyBlobMagicNumber magicNumber;
            switch (curveId)
            {
                case JsonWebKeyECTypes.P256:
                    if (willCreateSignatures)
                        magicNumber = KeyBlobMagicNumber.BCRYPT_ECDSA_PRIVATE_P256_MAGIC;
                    else
                        magicNumber = KeyBlobMagicNumber.BCRYPT_ECDSA_PUBLIC_P256_MAGIC;
                    break;
                case JsonWebKeyECTypes.P384:
                    if (willCreateSignatures)
                        magicNumber = KeyBlobMagicNumber.BCRYPT_ECDSA_PRIVATE_P384_MAGIC;
                    else
                        magicNumber = KeyBlobMagicNumber.BCRYPT_ECDSA_PUBLIC_P384_MAGIC;
                    break;
                case JsonWebKeyECTypes.P512: // treat 512 as 521. 512 doesn't exist, but we released with "512" instead of "521", so don't break now.
                case JsonWebKeyECTypes.P521:
                    if (willCreateSignatures)
                        magicNumber = KeyBlobMagicNumber.BCRYPT_ECDSA_PRIVATE_P521_MAGIC;
                    else
                        magicNumber = KeyBlobMagicNumber.BCRYPT_ECDSA_PUBLIC_P521_MAGIC;
                    break;
                default:
                    throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10645, curveId)));
            }
            return (uint)magicNumber;
        }

        /// <summary>
        /// Tests if user's runtime platform supports operations using <see cref="CngKey"/>.
        /// </summary>
        /// <returns>True if operations using <see cref="CngKey"/> are supported on user's runtime platform, false otherwise.</returns>
        [MethodImpl(MethodImplOptions.NoOptimization)]
        private static bool SupportsCNGKey()
        {
            try
            {
                _ = CngKeyBlobFormat.EccPrivateBlob;
                return true;
            }
            catch
            {
                return false;
            }
        }

#if NETSTANDARD2_0
        /// <summary>
        /// Creates an ECDsa object using the <paramref name="jsonWebKey"/> and <paramref name="usePrivateKey"/>.
        /// 'ECParameters' structure is available in .NET Framework 4.7+, .NET Standard 1.6+, and .NET Core 1.0+.
        /// </summary>
        private ECDsa CreateECDsaUsingECParams(JsonWebKey jsonWebKey, bool usePrivateKey)
        {
            if (jsonWebKey == null)
                throw LogHelper.LogArgumentNullException(nameof(jsonWebKey));

            if (jsonWebKey.Crv == null)
                throw LogHelper.LogArgumentNullException(nameof(jsonWebKey.Crv));

            if (jsonWebKey.X == null)
                throw LogHelper.LogArgumentNullException(nameof(jsonWebKey.X));

            if (jsonWebKey.Y == null)
                throw LogHelper.LogArgumentNullException(nameof(jsonWebKey.Y));

            try
            {
                var ecParams = new ECParameters
                {
                    Curve = GetNamedECCurve(jsonWebKey.Crv),
                    Q = { X = Base64UrlEncoder.DecodeBytes(jsonWebKey.X), Y = Base64UrlEncoder.DecodeBytes(jsonWebKey.Y) }
                };

                if (usePrivateKey)
                {
                    if (jsonWebKey.D == null)
                        throw LogHelper.LogArgumentNullException(nameof(jsonWebKey.D));

                    ecParams.D = Base64UrlEncoder.DecodeBytes(jsonWebKey.D);
                }

                return ECDsa.Create(ecParams);
            }
            catch (Exception ex)
            {
                throw LogHelper.LogExceptionMessage(new CryptographicException(LogMessages.IDX10689, ex));
            }
        }

        /// <summary>
        /// Returns the elliptic curve corresponding to the curve id.
        /// </summary>
        /// <param name="curveId">Represents ecdsa curve -P256, P384, P512</param>
        private static ECCurve GetNamedECCurve(string curveId)
        {
            if (string.IsNullOrEmpty(curveId))
                throw LogHelper.LogArgumentNullException(nameof(curveId));

            switch (curveId)
            {
                case JsonWebKeyECTypes.P256:
                    return ECCurve.NamedCurves.nistP256;
                case JsonWebKeyECTypes.P384:
                    return ECCurve.NamedCurves.nistP384;
                case JsonWebKeyECTypes.P512: // treat 512 as 521. 512 doesn't exist, but we released with "512" instead of "521", so don't break now.
                case JsonWebKeyECTypes.P521:
                    return ECCurve.NamedCurves.nistP521;
                default:
                    throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10645, curveId)));
            }
        }

        internal static string GetCrvParameterValue(ECCurve curve)
        {
            if (curve.Oid == null)
                throw LogHelper.LogArgumentNullException(nameof(curve.Oid));

            if (string.Equals(curve.Oid.Value, ECCurve.NamedCurves.nistP256.Oid.Value, StringComparison.Ordinal) || string.Equals(curve.Oid.FriendlyName, ECCurve.NamedCurves.nistP256.Oid.FriendlyName, StringComparison.Ordinal))
                return JsonWebKeyECTypes.P256;
            else if (string.Equals(curve.Oid.Value, ECCurve.NamedCurves.nistP384.Oid.Value, StringComparison.Ordinal) || string.Equals(curve.Oid.FriendlyName, ECCurve.NamedCurves.nistP384.Oid.FriendlyName, StringComparison.Ordinal))
                return JsonWebKeyECTypes.P384;
            else if (string.Equals(curve.Oid.Value, ECCurve.NamedCurves.nistP521.Oid.Value, StringComparison.Ordinal) || string.Equals(curve.Oid.FriendlyName, ECCurve.NamedCurves.nistP521.Oid.FriendlyName, StringComparison.Ordinal))
                return JsonWebKeyECTypes.P521;
            else
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10645, (curve.Oid.Value ?? curve.Oid.FriendlyName) ?? "null")));
        }
            

        /// <summary>
        /// Tests if user application's runtime supports <see cref="ECParameters"/> structure.
        /// </summary>
        /// <returns>True if <see cref="ECParameters"/> structure is supported, false otherwise.</returns>
        internal static bool SupportsECParameters()
        {
            try
            {
                LoadECParametersType();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Throws <see cref="TypeLoadException"/> during runtime if user application's runtime doesn't support <see cref="ECParameters"/> structure.
        /// </summary>
#pragma warning disable CS0168 //the variable is declared but never used
        [MethodImpl(MethodImplOptions.NoOptimization)]
        private static void LoadECParametersType()
        {
            ECParameters _;
        }
#pragma warning restore CS0168 //the variable is declared but never used
#endif
    }
}