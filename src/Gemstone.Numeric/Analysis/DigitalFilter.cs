//******************************************************************************************************
//  DigitalFilter.cs - Gbtc
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
//  05/10/2025 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Gemstone.Units;

namespace Gemstone.Numeric.Analysis;

/// <summary>
/// A class that implements a digital filter.
/// </summary>
public class DigitalFilter
{
    private List<double> m_b; // Numerator coefficients
    private List<double> m_a; // Denominator coefficients

    private DigitalFilter(double[] b, double[] a, int size)
    {
        m_b = new List<double>(size);
        m_b.AddRange(b);

        m_a = new List<double>(size);
        m_a.AddRange(a);

        Resize(m_b, size);
        Resize(m_a, size);
    }

    // Inspired by https://github.com/pgii/FiltfiltSharp
    private double[] FiltFilt(double[] x)
    {
        if (x == null)
            throw new ArgumentNullException(nameof(x));

        int nx = x.Length;
        int na = m_a.Count;
        int nb = m_b.Count;
        int order = Math.Max(nb, na);
        int factor = 3 * (order - 1);

        if (nx <= factor)
            throw new ArgumentOutOfRangeException(nameof(x), "FiltFilt signal 'x' size is too small");

        //Resize(m_b, order);
        //Resize(m_a, order);

        List<int> rows = new(order);
        List<int> cols = new(order);

        AddIndexRange(rows, 0, order - 2);

        if (order > 2)
        {
            AddIndexRange(rows, 1, order - 2);
            AddIndexRange(rows, 0, order - 3);
        }

        AddIndexCount(cols, 0, order - 1);

        if (order > 2)
        {
            AddIndexRange(cols, 1, order - 2);
            AddIndexRange(cols, 1, order - 2);
        }

        int count = rows.Count;
        List<double> data = new(count);

        Resize(data, count);

        data[0] = 1 + m_a[1];
        int j = 1;

        if (order > 2)
        {
            for (int i = 2; i < order; i++)
                data[j++] = m_a[i];

            for (int i = 0; i < order - 2; i++)
                data[j++] = 1.0;

            for (int i = 0; i < order - 2; i++)
                data[j++] = -1.0;
        }

        List<double> leftPad = SubvectorReverse(x, factor, 1);
        leftPad = leftPad.Select(q => 2 * x[0] - q).ToList();

        List<double> rightPad = SubvectorReverse(x, nx - 2, nx - factor - 1);
        rightPad = rightPad.Select(q => 2 * x[nx - 1] - q).ToList();

        int maxRowCount = rows.Max() + 1;
        int maxColCount = cols.Max() + 1;

        List<double> signal1 = new(nx + factor * 2);
        List<double> signal2 = new(signal1.Capacity);
        List<double> zi = new(maxRowCount);

        signal1.AddRange(leftPad);
        signal1.AddRange(x);
        signal1.AddRange(rightPad);

        Matrix<double> sp = new(maxRowCount, maxColCount, 0);

        for (int k = 0; k < count; ++k)
            sp[rows[k]][cols[k]] = data[k];

        double[] segment1 = Segment(m_b, 1, order - 1);
        double[] segment2 = Segment(m_a, 1, order - 1);
        double[] mzi = sp.RREF(Calc(segment1, m_b.ToArray()[0], segment2));

        Resize(zi, mzi.Length, 1);
        ScaleZi(mzi, zi, signal1[0]);
        Filter(signal1, signal2, zi);

        signal2.Reverse();

        ScaleZi(mzi, zi, signal2[0]);
        Filter(signal2, signal1, zi);

        return SubvectorReverse(signal1, signal1.Count - factor - 1, factor).ToArray();
    }

    private void Filter(List<double> x, List<double> y, List<double> zi)
    {
        if (m_a.Count == 0)
            throw new Exception("FiltFilt coefficient 'a' is empty");

        if (m_a.All(val => val == 0.0D))
            throw new Exception("FiltFilt coefficient 'a' must have at least one non-zero number");

        if (m_a[0] == 0)
            throw new Exception("FiltFilt coefficient 'a' first element cannot be zero");

        m_a = m_a.Select(q => q / m_a[0]).ToList();
        m_b = m_b.Select(q => q / m_a[0]).ToList();

        int nx = x.Count;
        int order = Math.Max(m_a.Count, m_b.Count);

        Resize(m_b, order);
        Resize(m_a, order);
        Resize(zi, order);
        Resize(y, nx);

        for (int i = 0; i < nx; i++)
        {
            int index = order - 1;

            while (index != 0)
            {
                if (i >= index)
                    zi[index - 1] = m_b[index] * x[i - index] - m_a[index] * y[i - index] + zi[index];

                index--;
            }

            y[i] = m_b[0] * x[i] + zi[0];
        }

        zi.RemoveAt(zi.Count - 1);
    }

    private static void Resize(List<double> vector, int length, double empty = 0.0D)
    {
        if (vector.Count >= length)
            return;

        vector.AddRange(Enumerable.Repeat(empty, length - vector.Count));
    }

    private static void AddIndexRange(List<int> vector, int start, int stop, int increment = 1)
    {
        for (int i = start; i <= stop; i += increment)
            vector.Add(i);
    }

    private static void AddIndexCount(List<int> vector, int value, int count)
    {
        while (count-- != 0)
            vector.Add(value);
    }

    private static List<double> SubvectorReverse(IReadOnlyList<double> vector, int stop, int start)
    {
        int length = stop - start + 1;
        List<double> result = new(Enumerable.Repeat(0.0D, length));
        int endIndex = length - 1;

        for (int i = start; i <= stop; i++)
            result[endIndex--] = vector[i];

        return result;
    }

    private static void ScaleZi(double[] mzi, List<double> zi, double factor)
    {
        for (int i = 0; i < mzi.Length; i++)
            zi[i] = mzi[i] * factor;
    }

    private static double[] Calc(double[] segment1, double d, double[] segment2)
    {
        double[] result = new double[segment1.Length];

        for (int i = 0; i < segment1.Length; i++)
            result[i] = segment1[i] - d * segment2[i];

        return result;
    }

    private static double[] Segment(List<double> vector, int start, int stop)
    {
        int length = stop - start + 1;
        double[] result = new double[length];

        for (int i = 0; i < length; i++)
            result[i] = vector[start + i];

        return result;
    }

    public static double[] FiltFilt(double[] b, double[] a, double[] x)
    {
        if (b == null)
            throw new ArgumentNullException(nameof(b));

        if (a == null)
            throw new ArgumentNullException(nameof(a));

        if (b.Length == 0 || a.Length == 0)
            throw new ArgumentException("FiltFilt coefficients b and a cannot be empty.");

        DigitalFilter filter = new(b, a, Math.Max(b.Length, a.Length));
        return filter.FiltFilt(x);
    }
}
