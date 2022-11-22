//******************************************************************************************************
//  AutoregressiveRandomNumberGenerator.cs - Gbtc
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
    /// Generates specifed order Autoregressive time-series distribution, full period cycle length > 2 billion
    /// </summary>
    public class AutoregressiveRandomNumberGenerator
    {
        /// <summary>
        ///  Instantiates Autoregressive time series generator.
        /// </summary>
        /// <param name="seed">Seed value for Uniform generator</param>
        /// <param name="order">Moving Average order</param>
        /// <param name="phis">ordered phis phi i-1, phi i-2, etc </param>
        /// <param name="mean">Mean of the Normal distribution</param>
        /// <param name="variance">Variance of the Normal distribution</param>
        public AutoregressiveRandomNumberGenerator(int seed, int order, double[] phis, double mean = 0, double variance = 1)
        {
            if (phis.Count() != order)
                throw new Exception("Please provide a phi value for each order level.");
            Order = order;
            Phis = phis;
            NormalGenerator = new NormalRandomNumberGenerator(seed, mean, variance);
            Priors = new List<double>(order);
            for (int i = 0; i < order; i++)
            {
                Priors.Add(0);
            }
            for (int i = 0; i < order; i++)
            {
                if (phis[i] <= -1 || phis[i] >= 1)
                    throw new Exception("Please provide phi values that are -1 < phi < 1.");
                double ei = NormalGenerator.Next().Value / Math.Sqrt(1 - Math.Pow(phis.Sum(), 2));
                double priorSum = Priors.Zip(phis, (First, Second) => new { First = First, Second = Second }).Select(x => x.First * x.Second).Sum();
                Priors.Remove(Priors.Last());
                Priors.Insert(0, ei);
            }
        }

        private List<double> Priors { get; }
        private int Order { get; }
        private double[] Phis { get; }
        private NormalRandomNumberGenerator NormalGenerator { get; }

        /// <summary>
        /// Gets next <see cref="AutoregressiveRandomNumber"/> in the sequence
        /// </summary>
        /// <returns><see cref="AutoregressiveRandomNumber"/></returns>
        public AutoregressiveRandomNumber Next()
        {
            double ei = NormalGenerator.Next().Value / Math.Sqrt(1 - Math.Pow(Phis.Sum(), 2));
            double priorSum = Priors.Zip(Phis, (First, Second) => new { First = First, Second = Second }).Select(x => x.First * x.Second).Sum();
            Priors.Remove(Priors.Last());
            Priors.Insert(0, ei + priorSum);
            return new AutoregressiveRandomNumber(ei + priorSum);
        }

        /// <summary>
        /// Gets the next n number of <see cref="AutoregressiveRandomNumber"/>
        /// </summary>
        /// <param name="number"></param>
        /// <returns><see cref="IEnumerable{AutoregressiveRandomNumber}"/></returns>
        public IEnumerable<AutoregressiveRandomNumber> Next(int number)
        {
            List<AutoregressiveRandomNumber> list = new();
            for (int i = 0; i < number; i++)
                list.Add(this.Next());

            return list;
        }

    }
}
