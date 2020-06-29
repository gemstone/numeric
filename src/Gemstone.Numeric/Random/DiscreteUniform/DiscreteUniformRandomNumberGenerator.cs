//******************************************************************************************************
//  DiscreteUniformRandomNumberGenerator.cs - Gbtc
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

using System.Collections.Generic;

namespace Gemstone.Numeric.Random
{
    /// <summary>
    /// Generates Normal(mean,variance) distribution, full period cycle length > 2 billion
    /// </summary>
    public class DiscreteUniformRandomNumberGenerator
    {
        private double UpperLimit { get; set; }
        private double LowerLimit { get; set; }
        private UniformRandomNumberGenerator UniformGenerator { get; }

        /// <summary>
        /// Instantiates Normal distribution generator to Normal(mean,variance)
        /// </summary>
        /// <param name="seed">Seed value for Uniform generator</param>
        /// <param name="upperLimit">Upper limit of distribution</param>
        /// <param name="lowerLimit">Lower limit of distribution</param>
        public DiscreteUniformRandomNumberGenerator(int seed, double upperLimit, double lowerLimit = 0)
        {
            UpperLimit = upperLimit;
            LowerLimit = lowerLimit;
            UniformGenerator = new UniformRandomNumberGenerator(seed);
        }

        /// <summary>
        /// Gets next <see cref="DiscreteUniformRandomNumber"/> in the sequence
        /// </summary>
        /// <returns><see cref="DiscreteUniformRandomNumber"/></returns>
        public DiscreteUniformRandomNumber Next()
        {
            UniformRandomNumber rv = UniformGenerator.Next();
            return new DiscreteUniformRandomNumber(rv, UpperLimit, LowerLimit);
        }

        /// <summary>
        /// Gets the next n number of <see cref="DiscreteUniformRandomNumber"/>
        /// </summary>
        /// <param name="number"></param>
        /// <returns><see cref="IEnumerable{DiscreteUniformRandomNumber}"/></returns>
        public IEnumerable<DiscreteUniformRandomNumber> Next(int number)
        {
            List<DiscreteUniformRandomNumber> list = new List<DiscreteUniformRandomNumber>();
            for (int i = 0; i < number; i++)
                list.Add(this.Next());

            return list;
        }


    }
}
