//******************************************************************************************************
//  UniformRandomNumber.cs - Gbtc
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
//  06/24/2020 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;

namespace Gemstone.Numeric.Random
{
    /// <summary>
    /// Pseudo-Random number distributed across the Uniform(0,1) distribution
    /// </summary>
    public class UniformRandomNumber
    {
        /// <summary>
        /// Takes a <see cref="double"/> between zero and 1 and sets the Value. 
        /// </summary>
        /// <param name="value"> <see cref="double"/> used to set Value.</param>
        public UniformRandomNumber(double value) {
            if (value >= 0 && value <= 1)
                Value = value;
            else
                throw new Exception("Unable to create Uniform random number because the value lies outside the distribution");
        }

        /// <summary>
        /// Property holding the actual value of the Uniform random number.
        /// </summary>
        public double Value { get; }
    }
}
