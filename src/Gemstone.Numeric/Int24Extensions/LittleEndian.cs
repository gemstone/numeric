//******************************************************************************************************
//  LittleEndian.cs - Gbtc
//
//  Copyright © 2019, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may not use this
//  file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  05/06/2014 - Steven E. Chisholm
//       Generated original version of source code based on EndianOrder.cs
//  08/20/2014 - Steven E. Chisholm
//       Added encoding for decimal numbers and support for pointer methods.
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

using System;
using System.Runtime.CompilerServices;

namespace Gemstone.Numeric.Int24Extensions
{
    /// <summary>
    /// Defines methods related to <see cref="Int24"/> little endian operations.
    /// </summary>
    public static class LittleEndian
    {
        /// <summary>
        /// Returns a 24-bit signed integer converted from three bytes, accounting for target endian-order, at a specified position in a byte array.
        /// </summary>
        /// <param name="buffer">An array of bytes (i.e., buffer containing binary image of value).</param>
        /// <returns>A 24-bit signed integer formed by three bytes beginning at startIndex.</returns>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">startIndex is less than zero or greater than the length of value minus 1.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Int24 ToInt24(byte* buffer)
        {
            int int32 = buffer[0] |
                        buffer[1] << 8 |
                        buffer[2] << 16;

            // Check bit 23, the sign bit in a signed 24-bit integer
            if ((int32 & 0x00800000) > 0)
            {
                // If the sign-bit is set, this number will be negative - set all high-byte bits (keeps 32-bit number in 24-bit range)
                int32 |= Int24.BitMask;
            }
            else
            {
                // If the sign-bit is not set, this number will be positive - clear all high-byte bits (keeps 32-bit number in 24-bit range)
                int32 &= ~Int24.BitMask;
            }

            return (Int24)int32;
        }

        /// <summary>
        /// Returns a 24-bit signed integer converted from three bytes, accounting for target endian-order, at a specified position in a byte array.
        /// </summary>
        /// <param name="buffer">An array of bytes (i.e., buffer containing binary image of value).</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A 24-bit signed integer formed by three bytes beginning at startIndex.</returns>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">startIndex is less than zero or greater than the length of value minus 1.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int24 ToInt24(byte[] buffer, int startIndex)
        {
            int int32 = buffer[startIndex + 0] |
                        buffer[startIndex + 1] << 8 |
                        buffer[startIndex + 2] << 16;

            // Check bit 23, the sign bit in a signed 24-bit integer
            if ((int32 & 0x00800000) > 0)
            {
                // If the sign-bit is set, this number will be negative - set all high-byte bits (keeps 32-bit number in 24-bit range)
                int32 |= Int24.BitMask;
            }
            else
            {
                // If the sign-bit is not set, this number will be positive - clear all high-byte bits (keeps 32-bit number in 24-bit range)
                int32 &= ~Int24.BitMask;
            }

            return (Int24)int32;
        }
        
        /// <summary>
        /// Returns the specified 24-bit signed integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes with length 3.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] GetBytes(Int24 value)
        {
            int int32 = value;

            return new[]
            {
                (byte)int32,
                (byte)(int32 >> 8),
                (byte)(int32 >> 16)
            };
        }
        
        /// <summary>
        /// Copies the specified 24-bit signed integer value as an array of 3 bytes in the target endian-order to the destination array.
        /// </summary>
        /// <param name="value">The number to convert and copy.</param>
        /// <param name="destinationArray">The destination buffer.</param>
        /// <param name="destinationIndex">The byte offset into <paramref name="destinationArray"/>.</param>
        /// <returns>Length of bytes copied into array based on size of <paramref name="value"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CopyBytes(Int24 value, byte[] destinationArray, int destinationIndex)
        {
            int int32 = value;

            destinationArray[destinationIndex + 0] = (byte)int32;
            destinationArray[destinationIndex + 1] = (byte)(int32 >> 8);
            destinationArray[destinationIndex + 2] = (byte)(int32 >> 16);

            return 3;
        }
        
        /// <summary>
        /// Copies the specified 24-bit signed integer value as an array of 3 bytes in the target endian-order to the destination array.
        /// </summary>
        /// <param name="value">The number to convert and copy.</param>
        /// <param name="destination">The destination buffer.</param>
        /// <returns>Length of bytes copied into array based on size of <paramref name="value"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int CopyBytes(Int24 value, byte* destination)
        {
            int int32 = value;

            destination[0] = (byte)int32;
            destination[1] = (byte)(int32 >> 8);
            destination[2] = (byte)(int32 >> 16);

            return 3;
        }
    }
}
