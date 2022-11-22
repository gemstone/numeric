//******************************************************************************************************
//  RandomNumberExtensions.cs - Gbtc
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
//  06/24/2020 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

namespace Gemstone.Numeric.Random
{
    /// <summary>
    /// Extension functions for Random number generation.
    /// </summary>
    public static class RandomNumberExtensions
    {
        /// <summary>
        /// Linq extension function used to Transform an enumerable of <see cref="UniformRandomNumber"/> to an enumerable of <see cref="GeometricRandomNumber"/>
        /// </summary>
        /// <param name="uniforms">enumerable of <see cref="UniformRandomNumber"/></param>
        /// <param name="probability">Probability of bernoulli trial success</param>
        /// <returns><see cref="IEnumerable{GeometricRandomNumber}"/></returns>
        public static IEnumerable<GeometricRandomNumber> ToGeometricDistribution(this IEnumerable<UniformRandomNumber> uniforms, double probability) => uniforms.Select(x => new GeometricRandomNumber(x, probability));

        /// <summary>
        /// Linq extension function used to Transform an enumerable of <see cref="UniformRandomNumber"/> to an enumerable of <see cref="NormalRandomNumber"/>
        /// </summary>
        /// <param name="uniforms">enumerable of <see cref="UniformRandomNumber"/></param>
        /// <param name="mean">Mean of normal distribution</param>
        /// <param name="variance">Variance of normal distribution</param>
        /// <returns><see cref="IEnumerable{NormalRandomNumber}"/></returns>
        public static IEnumerable<NormalRandomNumber> ToNormalDistribution(this IEnumerable<UniformRandomNumber> uniforms, double mean = 0, double variance = 1) => uniforms.Select(x => new NormalRandomNumber(x, mean, variance));

        /// <summary>
        /// Linq extension function used to Transform an enumerable of <see cref="UniformRandomNumber"/> to an enumerable of <see cref="LogNormalRandomNumber"/>
        /// </summary>
        /// <param name="uniforms">enumerable of <see cref="UniformRandomNumber"/></param>
        /// <param name="power">Power of the Log Normal distribution</param>
        /// <param name="mean">Mean of normal distribution</param>
        /// <param name="variance">Variance of normal distribution</param>
        /// <returns><see cref="IEnumerable{LogNormalRandomNumber}"/></returns>
        public static IEnumerable<LogNormalRandomNumber> ToLogNormalDistribution(this IEnumerable<UniformRandomNumber> uniforms, double power = Math.E, double mean = 0, double variance = 1) => uniforms.Select(x => new LogNormalRandomNumber(x, power, mean, variance));

        /// <summary>
        /// Linq extension function used to Transform an enumerable of <see cref="UniformRandomNumber"/> to an enumerable of <see cref="DiscreteUniformRandomNumber"/>
        /// </summary>
        /// <param name="uniforms">enumerable of <see cref="UniformRandomNumber"/></param>
        /// <param name="upperLimit">Upper limit of distribution</param>
        /// <param name="lowerLimit">Lower limit of distribution</param>
        /// <returns></returns>
        public static IEnumerable<DiscreteUniformRandomNumber> ToDiscreteUniformDistribution(this IEnumerable<UniformRandomNumber> uniforms, double upperLimit, double lowerLimit = 0) => uniforms.Select(x => new DiscreteUniformRandomNumber(x, upperLimit, lowerLimit));

    }
}
