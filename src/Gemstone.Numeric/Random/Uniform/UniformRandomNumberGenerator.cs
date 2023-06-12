//******************************************************************************************************
//  UniformRandomNumberGenerator.cs - Gbtc
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

namespace Gemstone.Numeric.Random.Uniform;

/// <summary>
/// Based on 16807 (BFS 1987) LCG implementation, full period cycle length > 2 billion, generates Uniform(0,1) distribution
/// </summary>
public class UniformRandomNumberGenerator
{
    /// <summary>
    /// Instantiates generator with given seed
    /// </summary>
    /// <param name="seed"><see cref="int"/> value used to set generator seed.</param>
    public UniformRandomNumberGenerator(int seed)
    {
        Ximinus1 = Generate(seed);
    }

    private int Ximinus1 { get; set; }

    private int Generate(int seed)
    {
        int k = (int)Math.Floor((double)seed / 127773);
        int xi = 16807 * (seed - 127773 * k) - 2836 * k;
        if (xi < 0)
            xi = xi + 2147483647;
        return xi;
    }

    /// <summary>
    /// Gets next <see cref="UniformRandomNumber"/> in the sequence
    /// </summary>
    /// <returns><see cref="UniformRandomNumber"/></returns>
    public UniformRandomNumber Next()
    {
        Ximinus1 = Generate(Ximinus1);
        return new UniformRandomNumber(Ximinus1 * 4.656612875e-10);
    }

    /// <summary>
    /// Gets the next n number of <see cref="UniformRandomNumber"/>
    /// </summary>
    /// <param name="number"></param>
    /// <returns><see cref="IEnumerable{UniformRandomNumber}"/></returns>
    public IEnumerable<UniformRandomNumber> Next(int number)
    {
        List<UniformRandomNumber> list = new();
        for (int i = 0; i < number; i++)
            list.Add(Next());

        return list;
    }


}
