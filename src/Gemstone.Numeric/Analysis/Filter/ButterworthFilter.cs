//******************************************************************************************************
//  ButterworthFilter.cs - Gbtc
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
//  05/15/2025 - G. Santos
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Gemstone.Numeric.Analysis;

/// <summary>
/// A class that implements a butterworth digital filter.
/// </summary>
public class ButterworthFilter
{
    // Represents a bandpass butterworth filter
    // Todo: Some of this was copied from OpenSEE, do we want to just have one set of code for both?
    private class AnalogFilter
    {
        private List<Complex> ContinousPoles;
        private List<Complex> ContinousZeros;
        private double Gain;

        public AnalogFilter(List<Complex> poles, List<Complex> zeros, double gain)
        {
            ContinousPoles = poles;
            ContinousZeros = zeros;
            Gain = gain;
        }

        public static AnalogFilter BPButterworth(double omega1, double omega2, double attenuationStop, int order)
        {
            AnalogFilter result = NormalButter(order);
            result.LowPassToBandPassTransform(omega1, omega2);
            return result;
        }

        private static AnalogFilter NormalButter(int order)
        {
            List<Complex> analogZeros = new List<Complex>();
            List<Complex> analogPoles = new List<Complex>();

            //Generate poles
            for (int i = 1; i < (order + 1); i++)
            {
                double theta = Math.PI * (2 * i - 1.0D) / (2.0D * (double)order) + Math.PI / 2.0D;
                double re = Math.Cos(theta);
                double im = Math.Sin(theta);

                analogPoles.Add(new Complex(re, im));
            }

            //scale to fit new filter
            AnalogFilter result = new AnalogFilter(analogPoles, analogZeros, 1);
            return result;
        }

        private void LowPassToBandPassTransform(double omega1, double omega2)
        {
            if (ContinousZeros.Count() != 0)
                throw new InvalidOperationException("Implicit assumption in calculations violated: Assuming transformation of lowpass to to bandpass.");
            double omegaDelta = omega2 - omega1;
            double omegaNaughtSquared = omega1 * omega2;
            int lpOrder = ContinousPoles.Count();

            List<Complex> newPoles = new List<Complex>();
            List<Complex> newZeros = new List<Complex>();

            Gain *= Math.Pow(omegaDelta, lpOrder);
            foreach (Complex p in ContinousPoles)
            {
                newZeros.Add(0);
                Complex determinant = p * p * Math.Pow(omegaDelta, 2.0D) - 4 * omegaNaughtSquared;
                // Note: there are technically 2 solutions here for each sqrt since these are complex poles
                Complex determinantSqrt = Math.Sqrt(determinant.Magnitude) * (determinant + determinant.Magnitude) / (determinant + determinant.Magnitude).Magnitude;
                newPoles.Add((p * omegaDelta + determinantSqrt) / 2);
                newPoles.Add((p * omegaDelta - determinantSqrt) / 2);
            }

            ContinousPoles = newPoles;
            ContinousZeros = newZeros;
        }

        // Returns poles, zeros, gain
        public (Complex[], Complex[], double) BilinearTransform(double fs, double omegaNaught = 0)
        {
            double k = fs * 2;
            // ToDo: Seems to work better without the step, look into it later? maybe just prewarping the analog is fine
            //if (omegaNaught != 0)
                //k = omegaNaught / Math.Tan(omegaNaught / (fs * 2));
            Complex[] oldPoles = ContinousPoles.ToArray();
            Complex[] oldZeros = ContinousZeros.ToArray();
            int n = Math.Max(oldPoles.Length, oldZeros.Length);
            Complex poleProd = 1.0D;
            Complex zeroProd = 1.0D;
            Complex[] poles = new Complex[n];
            Complex[] zeros = new Complex[n];
            for (int i = 0; i < n; i++)
            {
                if (i >= oldPoles.Length)
                    poles[i] = -1;
                else
                {
                    poles[i] = (k + oldPoles[i]) / (k - oldPoles[i]);
                    poleProd = poleProd * (k - oldPoles[i]);
                }

                if (i >= oldZeros.Length)
                    zeros[i] = -1;
                else
                {
                    zeros[i] = (k + oldZeros[i]) / (k - oldZeros[i]);
                    zeroProd = zeroProd * (k - oldZeros[i]);
                }
            }
            double gain = (this.Gain * zeroProd / poleProd).Real;
            return (poles, zeros, gain);
        }

        // For prewarping
        public static double DigitalToAnalog(double freq, double fs)
        {
            // angular freq at rad/s
            double omega = 2.0D * Math.PI * freq;
            return 2.0D * fs * Math.Tan(omega / (fs * 2.0D));
        }
    }


    private DigitalFilter Filter;

    /// <summary>
    /// Implements a bandpass filter
    /// </summary>
    public ButterworthFilter(double fStop1, double fPass1, double fPass2, double fStop2, double stopAttenuation, double passRipple, double samplingFreq)
    {
        // Find minimum order
        // https://www.ni.com/docs/en-US/bundle/labview-api-ref/page/vi-lib/analysis/3filter-llb/butterworth-order-estimation-vi.html?srsltid=AfmBOoqFcBBZQ-wlDG_i_ukhujwoS854cOJQTlouYpNaoAcAvlvC5hOh
        double num = Math.Sqrt(
            (Math.Pow(10, stopAttenuation / 10) - 1) / 
            (Math.Pow(10, passRipple / 10) - 1));


        double fStop1_Ana = AnalogFilter.DigitalToAnalog(fStop1, samplingFreq);
        double fStop2_Ana = AnalogFilter.DigitalToAnalog(fStop2, samplingFreq);
        double fPass1_Ana = AnalogFilter.DigitalToAnalog(fPass1, samplingFreq);
        double fPass2_Ana = AnalogFilter.DigitalToAnalog(fPass2, samplingFreq);
        double omegaP = fPass2_Ana - fPass1_Ana;
        double omegaS = Math.Min(Math.Abs(fStop1_Ana - fPass1_Ana * fPass2_Ana / fStop1_Ana), Math.Abs(fStop2_Ana - fPass1_Ana * fPass2_Ana / fStop2_Ana));

        int minOrder = (int) Math.Ceiling(Math.Log(num) / Math.Log(omegaS /omegaP));

        AnalogFilter analogFilt = AnalogFilter.BPButterworth(fStop1_Ana, fStop2_Ana, stopAttenuation, minOrder);

        double omegaNaught = Math.Sqrt(fStop1_Ana * fStop2_Ana);
        (Complex[] poles, Complex[] zeros, double gain) = analogFilt.BilinearTransform(samplingFreq, omegaNaught);
        double[] polyNum = PolesToPolynomial(zeros).Select(z => z * gain).ToArray();
        double[] polyDen = PolesToPolynomial(poles);
        // No issue, always same number of poles and zeros after bilinear
        Filter = new DigitalFilter(polyNum, polyDen, polyNum.Length);
    }

    public double[] FiltFilt(double[] x)
    {
        return Filter.FiltFilt(x);
    }

    private double[] PolesToPolynomial(Complex[] poles)
    {
        IEnumerable<Complex> result = new List<Complex> { Complex.One };
        foreach(Complex p in poles)
        {
            // Copy current coefficients into higher order
            List<Complex> higherOrder = result.Select(c => new Complex(c.Real, c.Imaginary)).ToList();
            higherOrder.Add(Complex.Zero);
            // Copy and multiply into lower order
            List<Complex> lowerOrder = result.Select(c => c*-p).ToList();
            lowerOrder.Insert(0,Complex.Zero);
            result = higherOrder.Zip(lowerOrder, (h, l) => h + l);
        }
        return result.Select(c => c.Real).ToArray();
    }
}

