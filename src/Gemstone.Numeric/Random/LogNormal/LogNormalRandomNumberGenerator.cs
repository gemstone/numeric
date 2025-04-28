//******************************************************************************************************
//  LogNormalRandomNumberGenerator.cs - Gbtc
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
using Gemstone.Numeric.Random.Uniform;

namespace Gemstone.Numeric.Random.LogNormal;

/// <summary>
/// Generates LogNormal(mean,variance) distribution, full period cycle length > 2 billion
/// </summary>
public class LogNormalRandomNumberGenerator
{
    /// <summary>
    /// Instantiates Normal distribution generator to LogNormal(mean,variance)
    /// </summary>
    /// <param name="seed">Seed value for Uniform generator</param>
    /// <param name="power">Power of the Log Normal distribution</param>
    /// <param name="mean">Mean of the Normal distribution</param>
    /// <param name="variance">Variance of the Normal distribution</param>
    public LogNormalRandomNumberGenerator(int seed, double power = Math.E, double mean = 0, double variance = 1)
    {
        Power = power;
        Mean = mean;
        Variance = variance;
        UniformGenerator = new UniformRandomNumberGenerator(seed);
    }

    private double Power { get; set; }
    private double Mean { get; set; }
    private double Variance { get; set; }
    private UniformRandomNumberGenerator UniformGenerator { get; }

    /// <summary>
    /// Gets next <see cref="LogNormalRandomNumber"/> in the sequence
    /// </summary>
    /// <returns><see cref="LogNormalRandomNumber"/></returns>
    public LogNormalRandomNumber Next()
    {
        UniformRandomNumber rv = UniformGenerator.Next();
        return new LogNormalRandomNumber(rv, Power, Mean, Variance);
    }

    /// <summary>
    /// Gets the next n number of <see cref="LogNormalRandomNumber"/>
    /// </summary>
    /// <param name="number"></param>
    /// <returns><see cref="IEnumerable{LogNormalRandomNumber}"/></returns>
    public IEnumerable<LogNormalRandomNumber> Next(int number)
    {
        List<LogNormalRandomNumber> list = [];
        for (int i = 0; i < number; i++)
            list.Add(Next());

        return list;
    }


}
