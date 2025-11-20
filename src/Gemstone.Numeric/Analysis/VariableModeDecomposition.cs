//******************************************************************************************************
//  VariableModeDecomposition.cs - Gbtc
//
//  Copyright © 2014, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  04/30/2014 - Stephen E. Chisholm
//       Generated original version of source code.
//
//******************************************************************************************************

//------------------------------------------------------------------------------------------------------
// Code base on Wikipedia article http://en.wikipedia.org/wiki/Box–Muller_transform
//------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Xml.Linq;
using Gemstone.Numeric.Random.Normal;
using Gemstone.Numeric.Random.Uniform;
using JetBrains.Annotations;
using MathNet.Numerics;
using MathNet.Numerics.Data.Matlab;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Factorization;
using MathNet.Numerics.LinearAlgebra.Solvers;
using MathNet.Numerics.Random;
using Microsoft.Extensions.Hosting;

namespace Gemstone.Numeric.Analysis;

/// <summary>
/// Implements a BoxMuller method for generating statistically normal random numbers.
/// </summary>
public static class VariableModeDecomposition
{

    //public static void Test()
    //{
    //    string fileName = "D:\\Matlab\\DEF Algorithm\\m-code\\Test.mat";
    //    Matrix<double> x = MatlabReader.Read<double>(fileName, "x");
    //    vmd(x.ToColumnArrays().First());
    //}


    /// <summary>
    /// returns intrinsic mode functions (IMFs) and a residual signal corresponding to the variational mode decomposition (VMD) of X, with default decomposition parameters.
    /// </summary>
    /// <param name="x">The input signal to oeprate on </param>
    /// <param name="MaxIterations">The maximum Numebr of Itterations. </param>
    /// <param name="NumIMFs">The number of intrinsic mode Functions to be found.</param>
    /// <param name="PenaltyFactor"> The penalty factor for the VMD algorithm. </param>
    public static Matrix<double> vmd(IEnumerable<double> x, int MaxIterations = 500, int NumIMFs = 5, double PenaltyFactor = 1000)
    {
        // not sure how we get the opts...
        int nFFt = 2 * x.Count();

        double tau = 0.01;
        int nSignalLength = x.Count();
        int nHalfSignalLenght = (int)Math.Floor(nSignalLength * 0.5);
        double relativeDiff = double.PositiveInfinity;
        double relativeTolerance = 0.005;
        double absoluteDiff = double.PositiveInfinity;
        double absoluteTolerance = 1e-6;
        int nMirroredSignalLength = nSignalLength * 2 + (nHalfSignalLenght - (int)Math.Ceiling(nSignalLength * 0.5));
        int nFFTLength = nMirroredSignalLength;

        int NumHalfFreqSamples;
        if (nFFTLength % 2 == 0)
            NumHalfFreqSamples = nFFTLength / 2 + 1;
        else
            NumHalfFreqSamples = (nFFTLength + 1) / 2;

        Matrix<ComplexNumber> initIMFfd = new(NumHalfFreqSamples, NumIMFs, new ComplexNumber(0.0D, 0.0D));
        Matrix<ComplexNumber> initialLM = new(NumHalfFreqSamples, 1, new(0.0D, 0));

        Matrix<ComplexNumber> xComplex = new(x.Select(v => new ComplexNumber(v, 0.0D)).ToArray(), 1);
        Matrix<ComplexNumber> sigFD = SignalBoundary(xComplex, nHalfSignalLenght, nMirroredSignalLength, nSignalLength, false).GetSubmatrix(0, 0, NumHalfFreqSamples, 1);

        // fft for initial IMFs and get half of bandwidth
        initIMFfd.OperateByColumn((c, _) =>
        {
            Complex32[] fft = c.Select((v) => new Complex32((float)v.Real, (float)v.Imaginary)).Concat(Enumerable.Repeat(new Complex32(0, 0), nFFt - c.Length)).ToArray();

            MathNet.Numerics.IntegralTransforms.Fourier.Forward(fft, MathNet.Numerics.IntegralTransforms.FourierOptions.Matlab);
            c = fft.Take(NumHalfFreqSamples).Select((c) => new ComplexNumber((double)c.Real, (double)c.Imaginary)).ToArray();
        });


        initIMFfd = initIMFfd + 2.2204e-16;

        //IMFfd = initIMFfd;
        ComplexNumber[] sumIMF = initIMFfd.RowSums;
        Matrix<ComplexNumber> LM = (Matrix<ComplexNumber>)initialLM.Clone();

        // Frequency vector from[0, 0.5) for odd nfft and[0, 0.5] for even nfft (nfft should never be odd due to how we calculate it...)
        Matrix<double> f = new(Enumerable.Range(0, NumHalfFreqSamples).Select((v) => (double)(v / (double)nFFt)).ToArray(), 1);

        // Get the initial central frequencies
        double[] centralFreq = InitialCentralFreqByFindPeaks(sigFD.GetColumnEnumerable(0).Select(v => v.Magnitude), f.GetColumnEnumerable(0), nFFTLength, NumIMFs);

        int iter = 0;
        Matrix<double> initIMFNorm = initIMFfd.TransformByValue<double>(((v, i, j) => v.Magnitude * v.Magnitude));
        Matrix<double> normIMF = new(initIMFNorm.NRows, initIMFNorm.NColumns, 0.0D);

        Matrix<ComplexNumber> imffd = (Matrix<ComplexNumber>)initIMFfd.Clone();

        while (iter < MaxIterations && (relativeDiff > relativeTolerance || absoluteDiff > absoluteTolerance))
        {
            for (int kk = 0; kk < NumIMFs; kk++)
            {
                sumIMF = sumIMF.Select((s, i) => s - imffd[i][kk]).ToArray();
                imffd.ReplaceSubmatrix(sigFD.TransformByValue((v, i, j) => (v - sumIMF[i] + LM[i][j] * 0.5D) / (PenaltyFactor * (f[i][j] - centralFreq[kk]) * (f[i][j] - centralFreq[kk]) + 1.0D)), 0, kk);
                normIMF.ReplaceSubmatrix(new(imffd.GetColumnEnumerable(kk).Select((v, i) => v.Magnitude * v.Magnitude).ToArray(), 1), 0, kk);
                centralFreq[kk] = f.TransposeAndMultiply(normIMF.GetColumn(kk))[0][0] / normIMF.GetColumnEnumerable(kk).Sum();
                sumIMF = sumIMF.Select((s, i) => s + imffd[i][kk]).ToArray();
            }


            LM = LM + tau * (sigFD - sumIMF);
            double[] absDiff = (imffd - initIMFfd).TransformByValue((v, i, j) => v.Magnitude * v.Magnitude / imffd.NRows).ColumnSums;
            absoluteDiff = absDiff.Sum();
            relativeDiff = absDiff.Select((v, i) => v / initIMFNorm.GetColumn(i).Average()).Sum();

            int[] sortedIndex = imffd.TransformByValue((v, i, j) => v.Magnitude * v.Magnitude).ColumnSums.Select((v, i) => new Tuple<int, double>(i, v)).OrderByDescending((v) => v.Item2).Select(v => v.Item1).ToArray();
            imffd = Matrix<ComplexNumber>.Combine(sortedIndex.Select((i) => new Matrix<ComplexNumber>(imffd.GetColumn(i), 1)).ToArray());

            centralFreq = sortedIndex.Take(centralFreq.Length).Select((i) => centralFreq[i]).ToArray();

            initIMFfd = (Matrix<ComplexNumber>)imffd.Clone();
            initIMFNorm = (Matrix<double>)normIMF.Clone();

            iter = iter + 1;
        }

        // Transform to time domain
        Matrix<ComplexNumber> IMFfdFull = new(nFFt, NumIMFs, new ComplexNumber(0.0D, 0.0D));
        IMFfdFull.ReplaceSubmatrix(imffd, 0, 0);


        if (nFFTLength % 2 == 0)
            IMFfdFull.ReplaceSubmatrix(imffd.FlipUpsideDown().GetSubmatrix(1,0,imffd.NRows - 2, imffd.NColumns).TransformByValue<ComplexNumber>((c) => c.Conjugate), imffd.NRows, 0); 
        else
            IMFfdFull.ReplaceSubmatrix(imffd.FlipUpsideDown().GetSubmatrix(0, 0, imffd.NRows - 1, imffd.NColumns).TransformByValue<ComplexNumber>((c) => c.Conjugate), imffd.NRows, 0);



        IEnumerable<int> sortIndex = centralFreq.Select((v, i) => new Tuple<int, double>(i, v)).OrderByDescending((v) => v.Item2).Select(v => v.Item1);

        return Matrix<double>.Combine(sortIndex.Select((i) => SignalBoundary(new(IMFfdFull.GetColumn(i), 1), nHalfSignalLenght, nMirroredSignalLength, nSignalLength, true).TransformByValue((v, i, j) => v.Real)).ToArray());
    }

    /// <summary>
    /// signalBoundary applies mirroring to signal if ifInverse is 0 and removes mirrored signal otherwise.Mirror extension of the signal by half its
    /// length on each side.Removing mirrored signal is a inverse process of the mirror extension. 
    /// </summary>
    /// <param name=""></param>
    /// <param name=""></param>
    /// <param name=""></param>
    /// <returns></returns>
    private static Matrix<ComplexNumber> SignalBoundary(Matrix<ComplexNumber> x, int nHalfSignalLenght, int nMirroredSignalLength, int nSignalLength, bool isInverse)
    {
        Complex32[] fft;

        if (isInverse) //removed mirrored signal
        {
            fft = x.GetColumn(0).Select((v) => new Complex32((float)v.Real, (float)v.Imaginary)).ToArray();
            
            MathNet.Numerics.IntegralTransforms.Fourier.Inverse(fft, MathNet.Numerics.IntegralTransforms.FourierOptions.Matlab);
            return new(fft.Select((c) => new ComplexNumber((double)c.Real,(double)c.Imaginary)).Skip(nHalfSignalLenght).Take(nMirroredSignalLength-2*nHalfSignalLenght).ToArray(),1);
        }

        fft = x.GetColumnEnumerable(0).Take(nHalfSignalLenght).Reverse().Concat(x.GetColumnEnumerable(0)).Concat(x.GetColumnEnumerable(0).Skip((int)Math.Ceiling(nSignalLength / 2.0D)).Reverse())
            .Select((v) => new Complex32((float)v.Real, (float)v.Imaginary)).ToArray();

        MathNet.Numerics.IntegralTransforms.Fourier.Forward(fft, MathNet.Numerics.IntegralTransforms.FourierOptions.Matlab);
        return new (fft.Select((c) => new ComplexNumber((double)c.Real, (double)c.Imaginary)).ToArray(),1);
    }

    /// <summary>
    /// Initialize central frequencies by finding the locations of signal peaks
    /// in frequency domain by using findpeaks function.The number of peaks is
    /// determined by NumIMFs.
    /// </summary>
    /// <param name=""></param>
    /// <param name=""></param>
    /// <param name=""></param>
    /// <returns></returns>
    private static double[] InitialCentralFreqByFindPeaks(IEnumerable<double> x, IEnumerable<double> f, int FFTLength, int NumIMFs)
    {
        double BW = 2.0D / FFTLength;
        double minBWGapIndex = 2.0D * BW / f.Skip(1).First();
        double xMean = x.Average();
        IEnumerable<double> xfilt = x.Select(v => v < xMean ? xMean : v);
        bool[] TF = IsLocalMax(xfilt, (int)minBWGapIndex);


        IEnumerable<Tuple<double,double>> peaks = xfilt.Zip(f, (v, freq) => new Tuple<double, double>(v, freq)).Where((v,i) => TF[i]);

        int numpPeaks = peaks.Count();


        // Check for DC component
        if (x.First() >= x.Skip(1).First())
            peaks = peaks.Prepend(new(x.First(), f.First()));
        

        UniformRandomNumberGenerator random = new UniformRandomNumberGenerator(0);
        IEnumerable<double> centralFreq = random.Next(NumIMFs).Select(v => v.Value);

        if (peaks.Count() < NumIMFs)
            centralFreq = peaks.Select(v => v.Item2).Concat(centralFreq).Take(NumIMFs);
        else
            centralFreq = peaks.OrderByDescending((v) => v.Item1).Take(NumIMFs).Select(v => v.Item2);

        return centralFreq.ToArray();
    }

    /// <summary>
    /// returns a logical array whose elements are true when a local maximum is detected in the corresponding element of x.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="minSepperation"></param>
    /// <returns></returns>
    private static bool[] IsLocalMax(IEnumerable<double> x, int minSepperation = 0)
    {

        IEnumerable<double> s = x.Skip(1).Zip(x, (a, b) => a > b ? 1.0 : (a == b ? 0 : -1.0));
        if (!s.Any(v => v != 0))
            return Enumerable.Repeat(false, x.Count()).ToArray();
        

        s = FillMissing<double>(s, v => v == 0).ToArray();

        IEnumerable<bool> maxVals = s.Skip(1).Zip(s, (a, b) => a < b).Prepend(false).Append(false);

        if (minSepperation == 0)
            return maxVals.ToArray();


        // Compute inflectionPts
        IEnumerable<bool> inflectionPoints = x.Skip(1).Zip(x, (a, b) => a != b).Prepend(true);
        inflectionPoints = s.Skip(1).Zip(s, (a, b) => a != b).Zip(inflectionPoints.Skip(1), (a, b) => a && b).Prepend(true).Append(true);

        // This will also restrict to the top N most prominent maxima.
        int[] locMaxima = maxVals.Select((t, i) => new Tuple<int, bool>(i, t)).Where(v => v.Item2).Select(v => v.Item1).ToArray();

        double[] P;
        if (locMaxima.Length == 0)
            P = x.Select(x => 0.0D).ToArray();
        else
            P = ComputeProminence(x, locMaxima).ToArray();
       

        Tuple<int,int>[] flatIndices = locMaxima.Select((v) => { 
            double val = x.ElementAt(v);
            var right = x.Select((val2, i) => (Value: val2, Index: i-1)).Skip(v).Where((o) => o.Value != val).FirstOrDefault();
            if (right == default)
                return new Tuple<int, int>(v, x.Count() - 1);

            return new Tuple<int, int>(v,right.Index);
        }).ToArray();

        // Iterate through each local maxima.
        int left = 0;
        int right = 0;

        bool[] filteredMaxVals = maxVals.ToArray();
        for(int i = 0; i < locMaxima.Length; i++)
        {
            while ((flatIndices[i].Item1 - flatIndices[left].Item2) >= minSepperation)
                    left++;

            right = Math.Max(right, i);

            while ((right <= (locMaxima.Length - 2)) && ((flatIndices[right + 1].Item1 - flatIndices[i].Item2) < minSepperation))
                right = right + 1;

            IEnumerable<int> leftIdx = locMaxima.Skip(left).Take(i - left);
            leftIdx = leftIdx.Where((i) => filteredMaxVals[i]);

            if (leftIdx.Count() > 0)
            {
                double leftMax = leftIdx.Select((i) => P[i]).Max();
                if (leftMax >= P[locMaxima[i]])
                    filteredMaxVals[locMaxima[i]] = false;
            }
            if (right - i > 0)
            { 
                double rightMax = locMaxima.Skip(i+1).Take(right-i).Select((index) => P[index]).Max();           
                if (rightMax > P[locMaxima[i]])
                    filteredMaxVals[locMaxima[i]] = false;
            }
        }

        IEnumerable<int> changeIndices = x.Skip(1).Zip(x, (a, b) => a - b).Select((v, i) => new Tuple<double, int>(v, i)).Where((s) => s.Item1 != 0).Select((v) => v.Item2 + 1).Prepend(1);
        Tuple<int, int>[] flatRanges = changeIndices.Skip(1).Zip(changeIndices,(i1,i2) => new Tuple<int,int>(i2,i1 -1)).Append(new Tuple<int, int>(changeIndices.Last() + 1, x.Count() - 1)).ToArray();

        foreach (Tuple<int, int> range in flatRanges)
        { 
            if (filteredMaxVals[range.Item1] && range.Item1 < range.Item2)
            {
                filteredMaxVals[range.Item1] = false;
                int center = (int)Math.Floor((range.Item1 + range.Item2) * 0.5D);
                filteredMaxVals[center] = true;
            }
        }

        return filteredMaxVals;


    }

    private static IEnumerable<double> ComputeProminence(IEnumerable<double> data, IEnumerable<int> localMaxIndices)
    {
        IEnumerator<double> dataEnumerator = data.GetEnumerator();
        IEnumerator<int> localMaxEnumerator = localMaxIndices.GetEnumerator();

        localMaxEnumerator.MoveNext();
        int i = 0;
        while (dataEnumerator.MoveNext())
        {
            if (i != localMaxEnumerator.Current)
            {
                yield return 0.0D;
                i++;
                continue;
            }
            int right = i;
            int left = i;
            double v = dataEnumerator.Current;
            while (v <= dataEnumerator.Current && left > 0)
            {
                left--;
                v = data.ElementAt(left);
            }
            v = dataEnumerator.Current;
            while (v <= dataEnumerator.Current && right < (data.Count() - 1))
            {
                right++;
                v = data.ElementAt(right);
            }
            
            double minLeft = data.Skip(left).Take(i - left).Min();
            double minRight = data.Skip(i + 1).Take(right - i).Min();

            yield return dataEnumerator.Current - Math.Max(minLeft, minRight);
            i++;

            if (!localMaxEnumerator.MoveNext())
                break; 
        }
        while (dataEnumerator.MoveNext())
            yield return 0.0D;
        }

    /// <summary>
    /// Replace values meeting condition in <see cref="replace"/> with the next Value not meeting this criteria.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="initial"></param>
    /// <param name="replace"></param>
    /// <returns></returns>
    private static IEnumerable<T> FillMissing<T>(IEnumerable<T> data, Func<T,bool> replace)
    {


        T last = data.Reverse().First((v) => !replace(v));

        IEnumerator<T> enumerator = data.GetEnumerator();
        IEnumerator<T>  checkEnumerator = data.GetEnumerator();

        int i = 0, j = 0;

        if(!checkEnumerator.MoveNext())
            yield break;

        while (enumerator.MoveNext())
        {
            if (!replace(enumerator.Current))
                yield return enumerator.Current;
            else
            {
                while ((i > j || replace(checkEnumerator.Current)) && checkEnumerator.MoveNext())
                {
                    j++;
                }

                if (replace(checkEnumerator.Current))
                    yield return last;
                else
                    yield return checkEnumerator.Current;
            }
            i++;
        }
    }
}
