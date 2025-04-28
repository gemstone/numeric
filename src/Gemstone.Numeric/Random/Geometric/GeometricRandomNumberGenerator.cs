//******************************************************************************************************
//  GeometricRandomNumberGenerator.cs - Gbtc
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

using System.Collections.Generic;
using Gemstone.Numeric.Random.Uniform;

namespace Gemstone.Numeric.Random.Geometric;

/// <summary>
/// Generates Geometric(probability) distribution, full period cycle length > 2 billion
/// </summary>
public class GeometricRandomNumberGenerator
{
    /// <summary>
    /// Instantiates Normal distribution generator to Geometric(probability)
    /// </summary>
    /// <param name="seed">Seed value for Uniform generator</param>
    /// <param name="probability">Probability of bernoulli trial success</param>
    public GeometricRandomNumberGenerator(int seed, double probability)
    {
        Probability = probability;
        UniformGenerator = new UniformRandomNumberGenerator(seed);
    }

    private double Probability { get; set; }
    private UniformRandomNumberGenerator UniformGenerator { get; }

    /// <summary>
    /// Gets next <see cref="GeometricRandomNumber"/> in the sequence
    /// </summary>
    /// <returns><see cref="GeometricRandomNumber"/></returns>
    public GeometricRandomNumber Next()
    {
        UniformRandomNumber rv = UniformGenerator.Next();
        return new GeometricRandomNumber(rv, Probability);
    }

    /// <summary>
    /// Gets the next n number of <see cref="GeometricRandomNumber"/>
    /// </summary>
    /// <param name="number"></param>
    /// <returns><see cref="IEnumerable{GeometricRandomNumber}"/></returns>
    public IEnumerable<GeometricRandomNumber> Next(int number)
    {
        List<GeometricRandomNumber> list = [];
        for (int i = 0; i < number; i++)
            list.Add(Next());

        return list;
    }


}
