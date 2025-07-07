//******************************************************************************************************
//  Pchip.cs - Gbtc
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
//  04/04/2022 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Gemstone.Numeric.Interpolation;

/// <summary>
/// Represents a piecebut-three-order Hermite Interpolation similar to Matlab Pchip.
/// </summary>
public static class Pchip
{
    private static double ExteriorSlope(double d1, double d2, double h1, double h2)
    {
        double s = ((2.0 * h1 + h2) * d1 - h1 * d2) / (h1 + h2);
        double signd1 = Math.Sign(d1);
        double signs = Math.Sign(s);

        if (signs != signd1)
        {
            s = 0.0;
        }
        else
        {
            signs = Math.Sign(d2);

            if (signd1 != signs && Math.Abs(s) > Math.Abs(3.0 * d1))
                s = 3.0 * d1;
        }

        return s;
    }

    /// <summary>
    /// interpolates to find Vq, the values of the underlying function y=F(x) at the query points Xq.
    /// </summary>
    /// <param name="x"> The x-values provided for the estimation of F(x).</param>
    /// <param name="y"> The y-values provided for the estimation of y = F(x) </param>
    /// <param name="new_x"> The x values to be estimated </param>
    /// <returns> the estimated y-values at location x = <see paramref="new_x"/> </returns>
    public static double[] Interp1(double[] x, double[] y, double[] new_x)
    {
        int x_len = x.Count();
        int new_x_len = new_x.Count();
        double[] new_y =new double[new_x_len];

        int low_ip1, ix, low_i, high_i, mid_i;
        double hs, hs3, w1;
        double[] del = new double[x_len - 1];
        double[] slopes = new double[x_len];
        double[] h = new double[x_len - 1];
        double[] pp_coefs = new double[x_len - 1 + 3 * (x_len - 1)];

        for (low_ip1 = 0; low_ip1 < x_len - 1; low_ip1++)
        {
            hs = x[low_ip1 + 1] - x[low_ip1];
            del[low_ip1] = (y[low_ip1 + 1] - y[low_ip1]) / hs;
            h[low_ip1] = hs;
        }

        for (low_ip1 = 0; low_ip1 < x_len - 2; low_ip1++)
        {
            hs = h[low_ip1] + h[low_ip1 + 1];
            hs3 = 3.0 * hs;
            w1 = (h[low_ip1] + hs) / hs3;
            hs = (h[low_ip1 + 1] + hs) / hs3;
            hs3 = 0.0;

            if (del[low_ip1] < 0.0)
            {
                if (del[low_ip1 + 1] <= del[low_ip1])
                {
                    hs3 = del[low_ip1] / (w1 * (del[low_ip1] / del[low_ip1 + 1]) + hs);
                }
                else
                {
                    if (del[low_ip1 + 1] < 0.0)
                        hs3 = del[low_ip1 + 1] / (w1 + hs * (del[low_ip1 + 1] / del[low_ip1]));
                }
            }
            else
            {
                if (del[low_ip1] > 0.0)
                {
                    if (del[low_ip1 + 1] >= del[low_ip1])
                    {
                        hs3 = del[low_ip1] / (w1 * (del[low_ip1] / del[low_ip1 + 1]) + hs);
                    }
                    else
                    {
                        if (del[low_ip1 + 1] > 0.0)
                            hs3 = del[low_ip1 + 1] / (w1 + hs * (del[low_ip1 + 1] / del[low_ip1]));
                    }
                }
            }

            slopes[low_ip1 + 1] = hs3;
        }

        slopes[0] = ExteriorSlope(del[0], del[1], h[0], h[1]);
        slopes[x_len - 1] = ExteriorSlope(del[x_len - 2], del[x_len - 3], h[x_len - 2], h[x_len - 3]);

        for (low_ip1 = 0; low_ip1 < x_len - 1; low_ip1++)
        {
            hs = (del[low_ip1] - slopes[low_ip1]) / h[low_ip1];
            hs3 = (slopes[low_ip1 + 1] - del[low_ip1]) / h[low_ip1];
            pp_coefs[low_ip1] = (hs3 - hs) / h[low_ip1];
            pp_coefs[low_ip1 + x_len - 1] = 2.0 * hs - hs3;
            pp_coefs[low_ip1 + 2 * (x_len - 1)] = slopes[low_ip1];
            pp_coefs[low_ip1 + 3 * (x_len - 1)] = y[low_ip1];
        }

        for (ix = 0; ix < new_x_len; ix++)
        {
            low_i = 0;
            low_ip1 = 2;
            high_i = x_len;

            while (high_i > low_ip1)
            {
                mid_i = (low_i + high_i + 1) >> 1;

                if (new_x[ix] >= x[mid_i - 1])
                {
                    low_i = mid_i - 1;
                    low_ip1 = mid_i + 1;
                }
                else
                {
                    high_i = mid_i;
                }
            }

            hs = new_x[ix] - x[low_i];
            hs3 = pp_coefs[low_i];

            for (low_ip1 = 0; low_ip1 < 3; low_ip1++)
                hs3 = hs * hs3 + pp_coefs[low_i + (low_ip1 + 1) * (x_len - 1)];

            new_y[ix] = hs3;
        }

        return new_y;
    }
}
