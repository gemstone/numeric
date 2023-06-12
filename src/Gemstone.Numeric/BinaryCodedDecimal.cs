﻿//******************************************************************************************************
//  BinaryCodedDecimal.cs - Gbtc
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

#region [ Contributor License Agreements ]

/**************************************************************************\
   Copyright © 2009 - J. Ritchie Carroll
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

using System;
using System.Runtime.CompilerServices;
using Gemstone.WordExtensions;

// ReSharper disable RedundantCast

namespace Gemstone.Numeric;

/// <summary>
/// Defines functions related to binary-coded decimals.
/// </summary>
/// <remarks>
/// See <a href="http://en.wikipedia.org/wiki/Binary-coded_decimal">Binary-coded decimal</a> for details.
/// </remarks>
public static class BinaryCodedDecimal
{
    private const byte TenP1 = 10;          // 10 to the power of 1 (for one byte integer)
    private const ushort TenP2 = 100;       // 10 to the power of 2 (for two byte integer)
    private const uint TenP4 = 10000;       // 10 to the power of 4 (for four byte integer)
    private const ulong TenP8 = 100000000;  // 10 to the power of 8 (for eight byte integer)

    /// <summary>
    /// Gets binary value from binary-coded decimal.
    /// </summary>
    /// <param name="bcd">Binary-coded decimal value.</param>
    /// <returns>Standard binary representation of binary-coded decimal value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte Decode(byte bcd) => 
        (byte)(bcd.HighNibble() * TenP1 + bcd.LowNibble());

    /// <summary>
    /// Gets binary value from two-byte binary-coded decimal.
    /// </summary>
    /// <param name="bcd">Two-byte binary-coded decimal value.</param>
    /// <returns>Standard binary representation of binary-coded decimal value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort Decode(ushort bcd) => 
        (ushort)(Decode(bcd.HighByte()) * TenP2 + Decode(bcd.LowByte()));

    /// <summary>
    /// Gets binary value from four-byte binary-coded decimal.
    /// </summary>
    /// <param name="bcd">Four-byte binary-coded decimal value.</param>
    /// <returns>Standard binary representation of binary-coded decimal value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint Decode(uint bcd) => 
        (uint)(Decode(bcd.HighWord()) * TenP4 + Decode(bcd.LowWord()));

    /// <summary>
    /// Gets binary value from eight-byte binary-coded decimal.
    /// </summary>
    /// <param name="bcd">Eight-byte binary-coded decimal value.</param>
    /// <returns>Standard binary representation of binary-coded decimal value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong Decode(ulong bcd) => 
        (ulong)(Decode(bcd.HighDoubleWord()) * TenP8 + Decode(bcd.LowDoubleWord()));

    /// <summary>
    /// Gets binary-coded decimal from binary value.
    /// </summary>
    /// <param name="value">Binary value.</param>
    /// <returns>Binary-coded decimal representation of standard binary value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">A binary-coded decimal has a maximum value of 99 for a single byte.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte Encode(byte value)
    {
        if (value > (byte)99)
            throw new ArgumentOutOfRangeException(nameof(value), "A binary-coded decimal has a maximum value of 99 for a single byte");

        byte high = (byte)((value / TenP1) & 0x0F);
        byte low = (byte)((value % TenP1) & 0x0F);

        return (byte)(low + (high << 4));
    }

    /// <summary>
    /// Gets binary-coded decimal from binary value.
    /// </summary>
    /// <param name="value">Binary value.</param>
    /// <returns>Binary-coded decimal representation of standard binary value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">A binary-coded decimal has a maximum value of 9,999 for two bytes.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort Encode(ushort value)
    {
        if (value > (ushort)9999)
            throw new ArgumentOutOfRangeException(nameof(value), "A binary-coded decimal has a maximum value of 9,999 for two bytes");

        byte high = Encode((byte)(value / TenP2));
        byte low = Encode((byte)(value % TenP2));

        return Word.MakeWord(high, low);
    }

    /// <summary>
    /// Gets binary-coded decimal from binary value.
    /// </summary>
    /// <param name="value">Binary value.</param>
    /// <returns>Binary-coded decimal representation of standard binary value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">A binary-coded decimal has a maximum value of 99,999,999 for four bytes.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint Encode(uint value)
    {
        if (value > (uint)99999999)
            throw new ArgumentOutOfRangeException(nameof(value), "A binary-coded decimal has a maximum value of 99,999,999 for four bytes");

        ushort high = Encode((ushort)(value / TenP4));
        ushort low = Encode((ushort)(value % TenP4));

        return Word.MakeDoubleWord(high, low);
    }

    /// <summary>
    /// Gets binary-coded decimal from binary value.
    /// </summary>
    /// <param name="value">Binary value.</param>
    /// <returns>Binary-coded decimal representation of standard binary value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">A binary-coded decimal has a maximum value of 9,999,999,999,999,999 for eight bytes.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong Encode(ulong value)
    {
        if (value > (ulong)9999999999999999)
            throw new ArgumentOutOfRangeException(nameof(value), "A binary-coded decimal has a maximum value of 9,999,999,999,999,999 for eight bytes");

        uint high = Encode((uint)(value / TenP8));
        uint low = Encode((uint)(value % TenP8));

        return Word.MakeQuadWord(high, low);
    }
}
