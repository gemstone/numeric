//******************************************************************************************************
//  MovingAverageRandomNumberGenerator.cs - Gbtc
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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gemstone.Numeric.Random
{
    /// <summary>
    /// Generates specifed order Moving Average time-series distribution, full period cycle length > 2 billion
    /// </summary>
    public class MovingAverageRandomNumberGenerator
    {
        /// <summary>
        /// Instantiates Moving Average generator.
        /// </summary>
        /// <param name="seed">Seed value for Uniform generator</param>
        /// <param name="order">Moving Average order</param>
        /// <param name="lambdas">Array of lambda values, ordered lambda i-1, lamda i-2, etc </param>
        /// <param name="mean">Mean of the Normal distribution</param>
        /// <param name="variance">Variance of the Normal distribution</param>
        public MovingAverageRandomNumberGenerator(int seed, int order, double[] lambdas, double mean = 0, double variance = 1)
        {
            if (lambdas.Count() != order)
                throw new Exception("Please provide a lambda value for each order level.");
            Order = order;
            Lambdas = lambdas;
            NormalGenerator = new NormalRandomNumberGenerator(seed, mean, variance);
            Priors = new List<double>(order);
            for (int i = 0; i < order; i++)
            {
                Priors.Add(NormalGenerator.Next().Value);
            }
        }

        private List<double> Priors { get; }
        private int Order { get; }
        private double[] Lambdas { get; }
        private NormalRandomNumberGenerator NormalGenerator { get; }

        /// <summary>
        /// Gets next <see cref="MovingAverageRandomNumber"/> in the sequence
        /// </summary>
        /// <returns><see cref="MovingAverageRandomNumber"/></returns>
        public MovingAverageRandomNumber Next()
        {
            double ei = NormalGenerator.Next().Value;
            double priorSum = Priors.Zip(Lambdas, (First, Second) => new { First = First, Second = Second }).Select(x => x.First * x.Second).Sum();
            Priors.Remove(Priors.Last());
            Priors.Insert(0, ei);
            return new MovingAverageRandomNumber(ei + priorSum);
        }

        /// <summary>
        /// Gets the next n number of <see cref="MovingAverageRandomNumber"/>
        /// </summary>
        /// <param name="number"></param>
        /// <returns><see cref="IEnumerable{MovingAverageRandomNumber}"/></returns>
        public IEnumerable<MovingAverageRandomNumber> Next(int number)
        {
            List<MovingAverageRandomNumber> list = new List<MovingAverageRandomNumber>();
            for (int i = 0; i < number; i++)
                list.Add(this.Next());

            return list;
        }

    }
}
