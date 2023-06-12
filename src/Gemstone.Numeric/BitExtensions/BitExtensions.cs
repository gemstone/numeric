//******************************************************************************************************
//  BitExtensions.cs - Gbtc
//
//  Copyright © 2012, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  01/25/2008 - J. Ritchie Carroll
//       Initial version of source generated.
//  09/14/2009 - Stephen C. Wills
//       Added new header and license agreement.
//  12/14/2012 - Starlynn Danyelle Gilliam
//       Modified Header.
//
//******************************************************************************************************

// Ignore Spelling: bitmask

#region [ Contributor License Agreements ]

/**************************************************************************\
   Copyright © 2009 - J. Ritchie Carroll, Pinal C. Patel
   All rights reserved.
  
   Redistribution and use in source and binary forms, with or without
   modification, are permitted provided that the following conditions
   are met:
  
      * Redistributions of source code must retain the above copyright
        notice, this list of conditions and the following disclaimer.
       
      * Redistributions in binary form must reproduce the above
        copyright notice, this list of conditions and the following
        disclaimer in the documentation and/or other materials provided
        with the distribution.
  
   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDER "AS IS" AND ANY
   EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
   IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR
   PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
   CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
   EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
   PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
   PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY
   OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
   (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
   OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
  
\**************************************************************************/

#endregion

using static Gemstone.Numeric.RadixCodec;

namespace Gemstone.Numeric.BitExtensions;

/// <summary>
/// Defines extension methods related to bit operations.
/// </summary>
public static class BitExtensions
{
    private static string RemoveSign(string value) =>
    value.Length > 0 && value[0] == '-' ? value[1..] : value;

    /// <summary>
    /// Encodes <paramref name="value"/> as binary, i.e., a string of bit values (0 or 1).
    /// </summary>
    /// <param name="value">Integer value to encode.</param>
    /// <returns>Binary encoding of <paramref name="value"/>.</returns>
    public static string ToBinaryString(this sbyte value) => 
        RemoveSign(Radix2.Encode(value)).PadLeft(8, '0');

    /// <summary>
    /// Encodes <paramref name="value"/> as binary, i.e., a string of bit values (0 or 1).
    /// </summary>
    /// <param name="value">Integer value to encode.</param>
    /// <returns>Binary encoding of <paramref name="value"/>.</returns>
    public static string ToBinaryString(this byte value) => 
        Radix2.Encode((ushort)value).PadLeft(8, '0');

    /// <summary>
    /// Encodes <paramref name="value"/> as binary, i.e., a string of bit values (0 or 1).
    /// </summary>
    /// <param name="value">Integer value to encode.</param>
    /// <returns>Binary encoding of <paramref name="value"/>.</returns>
    public static string ToBinaryString(this short value) => 
        RemoveSign(Radix2.Encode(value)).PadLeft(16, '0');

    /// <summary>
    /// Encodes <paramref name="value"/> as binary, i.e., a string of bit values (0 or 1).
    /// </summary>
    /// <param name="value">Integer value to encode.</param>
    /// <returns>Binary encoding of <paramref name="value"/>.</returns>
    public static string ToBinaryString(this ushort value) => 
        Radix2.Encode(value).PadLeft(16, '0');

    /// <summary>
    /// Encodes <paramref name="value"/> as binary, i.e., a string of bit values (0 or 1).
    /// </summary>
    /// <param name="value">Integer value to encode.</param>
    /// <returns>Binary encoding of <paramref name="value"/>.</returns>
    public static string ToBinaryString(this Int24 value) => 
        RemoveSign(Radix2.Encode(value)).PadLeft(24, '0');

    /// <summary>
    /// Encodes <paramref name="value"/> as binary, i.e., a string of bit values (0 or 1).
    /// </summary>
    /// <param name="value">Integer value to encode.</param>
    /// <returns>Binary encoding of <paramref name="value"/>.</returns>
    public static string ToBinaryString(this UInt24 value) => 
        Radix2.Encode(value).PadLeft(24, '0');

    /// <summary>
    /// Encodes <paramref name="value"/> as binary, i.e., a string of bit values (0 or 1).
    /// </summary>
    /// <param name="value">Integer value to encode.</param>
    /// <returns>Binary encoding of <paramref name="value"/>.</returns>
    public static string ToBinaryString(this int value) => 
        RemoveSign(Radix2.Encode(value)).PadLeft(32, '0');

    /// <summary>
    /// Encodes <paramref name="value"/> as binary, i.e., a string of bit values (0 or 1).
    /// </summary>
    /// <param name="value">Integer value to encode.</param>
    /// <returns>Binary encoding of <paramref name="value"/>.</returns>
    public static string ToBinaryString(this uint value) => 
        Radix2.Encode(value).PadLeft(32, '0');

    /// <summary>
    /// Encodes <paramref name="value"/> as binary, i.e., a string of bit values (0 or 1).
    /// </summary>
    /// <param name="value">Integer value to encode.</param>
    /// <returns>Binary encoding of <paramref name="value"/>.</returns>
    public static string ToBinaryString(this long value) => 
        RemoveSign(Radix2.Encode(value).PadLeft(64, '0'));

    /// <summary>
    /// Encodes <paramref name="value"/> as binary, i.e., a string of bit values (0 or 1).
    /// </summary>
    /// <param name="value">Integer value to encode.</param>
    /// <returns>Binary encoding of <paramref name="value"/>.</returns>
    public static string ToBinaryString(this ulong value) => 
        Radix2.Encode(value).PadLeft(64, '0');
}
