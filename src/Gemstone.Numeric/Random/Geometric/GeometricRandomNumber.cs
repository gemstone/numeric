//******************************************************************************************************
//  GeometricRandomNumber.cs - Gbtc
//
//  Copyright © 2022, Grid Protection Alliance.  All Rights Reserved.
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
//  06/29/2020 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************


using System;

namespace Gemstone.Numeric.Random
{
    /// <summary>
    /// Pseudo-Random number distributed across Geometric(probability)
    /// </summary>
    public class GeometricRandomNumber
    {

        /// <summary>
        /// Property holding the actual value of the Geometric(probability) random number.
        /// </summary>
        public int Value { get; }

        /// <summary>
        /// Transforms a Uniform(0,1) into a Geometric(probability)
        /// </summary>
        /// <param name="uniform"><see cref="UniformRandomNumber"/>Uniform(0,1)</param>
        /// <param name="probability">Probability of bernoulli trial success</param>
        public GeometricRandomNumber(UniformRandomNumber uniform, double probability)
        {
            Value = (int)Math.Ceiling(Math.Log(uniform.Value)/ Math.Log(1 - probability));
        }
    }
}
