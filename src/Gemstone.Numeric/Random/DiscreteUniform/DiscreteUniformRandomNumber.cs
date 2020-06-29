//******************************************************************************************************
//  DiscreteUniformRandomNumber.cs - Gbtc
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
using System.Collections.Generic;
using System.Text;

namespace Gemstone.Numeric.Random
{
    /// <summary>
    ///  Discrete Pseudo-Random number distributed across Uniform(lower limit, upper limit)
    /// </summary>
    public class DiscreteUniformRandomNumber
    {
        /// <summary>
        /// Property holding the actual value of the Discrete Unif(lower limit, upper limit) random number.
        /// </summary>
        public double Value { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uniform"><see cref="UniformRandomNumber"/>Uniform(0,1)</param>
        /// <param name="upperLimit">Upper limit of distribution</param>
        /// <param name="lowerLimit">Lower limit of distribution</param>
        public DiscreteUniformRandomNumber(UniformRandomNumber uniform, double upperLimit, double lowerLimit = 0) {
            Value = Math.Ceiling((upperLimit - lowerLimit) * uniform.Value + lowerLimit);
        }
    }
}
