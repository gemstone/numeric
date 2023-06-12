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

namespace Gemstone.Numeric.UInt24Extensions;

/// <summary>
/// Defines extension methods related to <see cref="UInt24"/> bit operations.
/// </summary>
public static class BitExtensions
{
    /// <summary>
    /// Returns value with specified <paramref name="bits"/> set.
    /// </summary>
    /// <param name="source">Value source.</param>
    /// <param name="bits"><see cref="Bits"/> to set.</param>
    /// <returns><see cref="UInt24"/> value with specified <paramref name="bits"/> set.</returns>
    public static UInt24 SetBits(this UInt24 source, Bits bits) => 
        SetBits(source, (UInt24)bits);

    /// <summary>
    /// Returns value with specified <paramref name="bits"/> set.
    /// </summary>
    /// <param name="source">Value source.</param>
    /// <param name="bits">Bit-mask of the bits to set.</param>
    /// <returns><see cref="UInt24"/> value with specified <paramref name="bits"/> set.</returns>
    public static UInt24 SetBits(this UInt24 source, UInt24 bits) => 
        source | bits;

    /// <summary>
    /// Returns value with specified <paramref name="bits"/> cleared.
    /// </summary>
    /// <param name="source">Value source.</param>
    /// <param name="bits"><see cref="Bits"/> to clear.</param>
    /// <returns><see cref="UInt24"/> value with specified <paramref name="bits"/> cleared.</returns>
    public static UInt24 ClearBits(this UInt24 source, Bits bits) => 
        ClearBits(source, (UInt24)bits);

    /// <summary>
    /// Returns value with specified <paramref name="bits"/> cleared.
    /// </summary>
    /// <param name="source">Value source.</param>
    /// <param name="bits">Bit-mask of the bits to clear.</param>
    /// <returns><see cref="UInt24"/> value with specified <paramref name="bits"/> cleared.</returns>
    public static UInt24 ClearBits(this UInt24 source, UInt24 bits) => 
        source & ~bits;

    /// <summary>
    /// Determines if specified <paramref name="bits"/> are set.
    /// </summary>
    /// <param name="source">Value source.</param>
    /// <param name="bits"><see cref="Bits"/> to check.</param>
    /// <returns>true if specified <paramref name="bits"/> are set in <paramref name="source"/> value; otherwise false.</returns>
    public static bool CheckBits(this UInt24 source, Bits bits) => 
        CheckBits(source, (UInt24)bits);

    /// <summary>
    /// Determines if specified <paramref name="bits"/> are set.
    /// </summary>
    /// <param name="source">Value source.</param>
    /// <param name="bits">Bit-mask of the bits to check.</param>
    /// <returns>true if specified <paramref name="bits"/> are set in <paramref name="source"/> value; otherwise false.</returns>
    public static bool CheckBits(this UInt24 source, UInt24 bits) => 
        CheckBits(source, bits, true);

    /// <summary>
    /// Determines if specified <paramref name="bits"/> are set.
    /// </summary>
    /// <param name="source">Value source.</param>
    /// <param name="bits"><see cref="Bits"/> to check.</param>
    /// <param name="allBits">true to check if all <paramref name="bits"/> are set; otherwise false.</param>
    /// <returns>true if specified <paramref name="bits"/> are set in <paramref name="source"/> value; otherwise false.</returns>
    public static bool CheckBits(this UInt24 source, Bits bits, bool allBits) => 
        CheckBits(source, (UInt24)bits, allBits);

    /// <summary>
    /// Determines if specified <paramref name="bits"/> are set.
    /// </summary>
    /// <param name="source">Value source.</param>
    /// <param name="bits">Bit-mask of the bits to check.</param>
    /// <param name="allBits">true to check if all <paramref name="bits"/> are set; otherwise false.</param>
    /// <returns>true if specified <paramref name="bits"/> are set in <paramref name="source"/> value; otherwise false.</returns>
    public static bool CheckBits(this UInt24 source, UInt24 bits, bool allBits) => 
        allBits ? (source & bits) == bits : (source & bits) != 0;

    /// <summary>
    /// Returns value with specified <paramref name="bits"/> toggled.
    /// </summary>
    /// <param name="source">Value source.</param>
    /// <param name="bits"><see cref="Bits"/> to toggle.</param>
    /// <returns><see cref="UInt24"/> value with specified <paramref name="bits"/> toggled.</returns>
    public static UInt24 ToggleBits(this UInt24 source, Bits bits) => 
        ToggleBits(source, (UInt24)bits);

    /// <summary>
    /// Returns value with specified <paramref name="bits"/> toggled.
    /// </summary>
    /// <param name="source">Value source.</param>
    /// <param name="bits">Bit-mask of the bits to toggle.</param>
    /// <returns><see cref="UInt24"/> value with specified <paramref name="bits"/> toggled.</returns>
    public static UInt24 ToggleBits(this UInt24 source, UInt24 bits) => 
        source ^ bits;

    /// <summary>
    /// Returns value stored in the bits represented by the specified <paramref name="bitmask"/>.
    /// </summary>
    /// <param name="source">Value source.</param>
    /// <param name="bitmask"><see cref="Bits"/> that make-up the bit-mask.</param>
    /// <returns><see cref="UInt24"/> value.</returns>
    public static UInt24 GetMaskedValue(this UInt24 source, Bits bitmask) => 
        GetMaskedValue(source, (UInt24)bitmask);

    /// <summary>
    /// Returns value stored in the bits represented by the specified <paramref name="bitmask"/>.
    /// </summary>
    /// <param name="source">Value source.</param>
    /// <param name="bitmask">Bit-mask of the bits involved.</param>
    /// <returns><see cref="UInt24"/> value.</returns>
    public static UInt24 GetMaskedValue(this UInt24 source, UInt24 bitmask)
    {
        return source & bitmask;
    }

    /// <summary>
    /// Returns value after setting a new <paramref name="value"/> for the bits specified by the <paramref name="bitmask"/>.
    /// </summary>
    /// <param name="source">Value source.</param>
    /// <param name="bitmask"><see cref="Bits"/> that make-up the bit-mask.</param>
    /// <param name="value">New value.</param>
    /// <returns><see cref="UInt24"/> value.</returns>
    public static UInt24 SetMaskedValue(this UInt24 source, Bits bitmask, UInt24 value)
    {
        return SetMaskedValue(source, (UInt24)bitmask, value);
    }

    /// <summary>
    /// Returns value after setting a new <paramref name="value"/> for the bits specified by the <paramref name="bitmask"/>.
    /// </summary>
    /// <param name="source">Value source.</param>
    /// <param name="bitmask">Bit-mask of the bits involved.</param>
    /// <param name="value">New value.</param>
    /// <returns><see cref="UInt24"/> value.</returns>
    public static UInt24 SetMaskedValue(this UInt24 source, UInt24 bitmask, UInt24 value)
    {
        return (source & ~bitmask) | value;
    }

    /// <summary>
    /// Performs rightwise bit-rotation for the specified number of rotations.
    /// </summary>
    /// <param name="value">Value used for bit-rotation.</param>
    /// <param name="rotations">Number of rotations to perform.</param>
    /// <returns>Value that has its bits rotated to the right the specified number of times.</returns>
    /// <remarks>
    /// Actual rotation direction is from a big-endian perspective - this is an artifact of the native
    /// .NET bit shift operators. As a result bits may actually appear to rotate right on little-endian
    /// architectures.
    /// </remarks>
    public static UInt24 BitRotL(this UInt24 value, int rotations)
    {
        for (int x = 1; x <= rotations % 24; x++)
        {
            bool hiBitSet = value.CheckBits(Bits.Bit23);

            value <<= 1;

            if (hiBitSet)
                value = value.SetBits(Bits.Bit00);
        }

        return value;
    }

    /// <summary>
    /// Performs rightwise bit-rotation for the specified number of rotations.
    /// </summary>
    /// <param name="value">Value used for bit-rotation.</param>
    /// <param name="rotations">Number of rotations to perform.</param>
    /// <returns>Value that has its bits rotated to the right the specified number of times.</returns>
    /// <remarks>
    /// Actual rotation direction is from a big-endian perspective - this is an artifact of the native
    /// .NET bit shift operators. As a result bits may actually appear to rotate left on little-endian
    /// architectures.
    /// </remarks>
    public static UInt24 BitRotR(this UInt24 value, int rotations)
    {
        for (int x = 1; x <= rotations % 24; x++)
        {
            bool loBitSet = value.CheckBits(Bits.Bit00);

            value >>= 1;

            value = loBitSet ? value.SetBits(Bits.Bit23) : value.ClearBits(Bits.Bit23);
        }

        return value;
    }
}
