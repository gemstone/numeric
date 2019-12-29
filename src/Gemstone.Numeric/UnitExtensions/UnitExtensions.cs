//******************************************************************************************************
//  UnitExtensions.cs - Gbtc
//
//  Copyright © 2016, Grid Protection Alliance.  All Rights Reserved.
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
//  12/12/2016 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Gemstone.Numeric.Analysis;
using Gemstone.Units;

namespace Gemstone.Numeric.UnitExtensions
{
    /// <summary>
    /// Defines extension functions related to unit structures.
    /// </summary>
    public static class UnitExtensions
    {
        private const double TwoPI = 2.0D * Math.PI;

        /// <summary>
        /// Gets the equivalent angle moved within the range of <paramref name="minValue"/>
        /// and <paramref name="minValue"/> + 2.0 * <see cref="Math.PI"/>.
        /// </summary>
        /// <param name="angle">Source angle.</param>
        /// <param name="minValue">The minimum value of the range.</param>
        /// <param name="inclusive">Indicates whether the range is inclusive of the minimum value.</param>
        /// <returns>The equivalent angle within the specified range.</returns>
        public static Angle ToRange(this Angle angle, Angle minValue, bool inclusive = true)
        {
            return inclusive ? Euclidean.Wrap(angle, minValue, TwoPI) : Euclidean.Wrap(angle, minValue + TwoPI, -TwoPI);
        }

        /// <summary>
        /// Unwraps a set of <see cref="Angle"/> values so a comparable mathematical operation can be applied.
        /// </summary>
        /// <param name="source">Sequence of <see cref="Angle"/> values to unwrap.</param>
        /// <returns>Unwrapped set of <see cref="Angle"/> values.</returns>
        /// <remarks>
        /// For Angles that wrap, e.g., between -180 and +180, this algorithm unwraps the values to make the values mathematically comparable.
        /// </remarks>
        public static IEnumerable<Angle> Unwrap(this IEnumerable<Angle> source)
        {
            double[] sourceAngles = source.Select(angle => (double)angle).ToArray();

            return Unwrap(sourceAngles).Select(angle => new Angle(angle));
        }

        /// <summary>
        /// Calculates an average of the specified sequence of <see cref="Angle"/> values.
        /// </summary>
        /// <param name="source">Sequence of <see cref="Angle"/> values over which to calculate average.</param>
        /// <returns>Average of the specified sequence of <see cref="Angle"/> values.</returns>
        /// <remarks>
        /// For Angles that wrap, e.g., between -180 and +180, this algorithm takes the wrapping into account when calculating the average.
        /// </remarks>
        public static Angle Average(this IEnumerable<Angle> source)
        {
            double[] sourceAngles = source.Select(angle => (double)angle).ToArray();

            return new Angle(Unwrap(sourceAngles).Average()).ToRange(-Math.PI, false);
        }

        private static double[] Unwrap(double[] sourceAngles)
        {
            double[] unwrappedAngles = new double[sourceAngles.Length];

            if (sourceAngles.Length > 0)
            {
                double offset = 0.0D, dis0, dis1, dis2;

                unwrappedAngles[0] = sourceAngles[0];

                // Unwrap all source angles
                for (int i = 1; i < sourceAngles.Length; i++)
                {
                    dis0 = Math.Abs(sourceAngles[i] + offset - unwrappedAngles[i - 1]);
                    dis1 = Math.Abs(sourceAngles[i] + offset - unwrappedAngles[i - 1] + TwoPI);
                    dis2 = Math.Abs(sourceAngles[i] + offset - unwrappedAngles[i - 1] - TwoPI);

                    if (dis1 < dis0 && dis1 < dis2)
                        offset = offset + TwoPI;
                    else if (dis2 < dis0 && dis2 < dis1)
                        offset = offset - TwoPI;

                    unwrappedAngles[i] = sourceAngles[i] + offset;
                }
            }

            return unwrappedAngles;
        }
    }
}
