//******************************************************************************************************
//  RandomInt24.cs - Gbtc
//
//  Copyright © 2020, Grid Protection Alliance.  All Rights Reserved.
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
//  01/05/2020 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Security.Cryptography;
using Random = Gemstone.Security.Cryptography.Random;

namespace Gemstone.Numeric
{
    /// <summary>
    /// Generates cryptographically strong random <see cref="Int24"/> numbers.
    /// </summary>
    public static class RandomInt24
    {
        /// <summary>
        /// Generates a cryptographically strong 24-bit random integer.
        /// </summary>
        /// <exception cref="CryptographicException">The cryptographic service provider (CSP) cannot be acquired.</exception>
        public static Int24 Value
        {
            get
            {
                byte[] value = new byte[3];

                Random.GetBytes(value);

                return Int24.GetValue(value, 0);
            }
        }

        /// <summary>
        /// Generates a cryptographically strong 24-bit random integer between specified values.
        /// </summary>
        /// <exception cref="CryptographicException">The cryptographic service provider (CSP) cannot be acquired.</exception>
        /// <param name="startNumber">A <see cref="Int24"/> that is the low end of our range.</param>
        /// <param name="stopNumber">A <see cref="Int24"/> that is the high end of our range.</param>
        /// <returns>A <see cref="Int24"/> that is generated between the <paramref name="startNumber"/> and the <paramref name="stopNumber"/>.</returns>
        public static Int24 ValueBetween(Int24 startNumber, Int24 stopNumber)
        {
            if (stopNumber < startNumber)
                throw new ArgumentException("stopNumber must be greater than startNumber");

            return (Int24)(Random.GetRandomNumberLessThan((int)stopNumber - (int)startNumber) + (int)startNumber);
        }
    }
}
