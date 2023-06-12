//******************************************************************************************************
//  NormalRandomNumber.cs - Gbtc
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
using Gemstone.Numeric.Random.Uniform;

namespace Gemstone.Numeric.Random.Normal;

/// <summary>
/// Pseudo-Random number distributed across Normal(mean,variance)
/// </summary>
public class NormalRandomNumber
{

    /// <summary>
    /// Property holding the actual value of the Normal(mean,variance) random number.
    /// </summary>
    public double Value { get; }

    /// <summary>
    /// Transforms a Uniform(0,1) into a Normal(mean,variance)
    /// </summary>
    /// <param name="uniform"><see cref="UniformRandomNumber"/>Uniform(0,1)</param>
    /// <param name="mean">Mean of the Normal distribution</param>
    /// <param name="variance">Variance of the Normal distribution</param>
    public NormalRandomNumber(UniformRandomNumber uniform, double mean = 0, double variance = 1)
    {
        double c0 = 2.51557;
        double c1 = 0.802853;
        double c2 = 0.010328;
        double d1 = 1.432788;
        double d2 = 0.189269;
        double d3 = 0.001308;

        double t = T(uniform.Value);
        double s = Sign(uniform.Value - 0.5);
        double numerator = c0 + c1 * t + c2 * Math.Pow(t, 2);
        double denominator = 1 + d1 * t + d2 * Math.Pow(t, 2) + d3 * Math.Pow(t, 3);

        double z = s * (t - numerator / denominator);

        Value = mean + Math.Sqrt(variance) * z;
    }


    private int Sign(double uniform)
    {
        if (uniform == 0)
            return 0;
        if (uniform > 0)
            return 1;
        return -1;
    }

    private double T(double uniform)
    {
        double min = Math.Min(uniform, 1 - uniform);
        double ln = Math.Log(Math.Pow(min, 2));
        return Math.Sqrt(-1 * ln);
    }
}
