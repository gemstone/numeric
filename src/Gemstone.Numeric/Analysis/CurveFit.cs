using System;
using System.Collections.Generic;
using System.Linq;

namespace Gemstone.Numeric.Analysis;

/// <summary>
/// Used to hold x-y coordinates
/// </summary>
/// <param name="X">x-coordinate</param>
/// <param name="Y">y-coordinate</param>
public record Point(double X, double Y)
{
}

/// <summary>
/// Linear regression algorithm.
/// </summary>
public static class CurveFit
{
    /// <summary>
    /// Computes linear regression over given values.
    /// </summary>
    /// <param name="polynomialOrder">An <see cref="int"/> for the polynomial order.</param>
    /// <param name="values">A list of values.</param>
    /// <returns>An array of <see cref="double"/> values.</returns>
    public static double[] Compute(int polynomialOrder, IEnumerable<Point> values)
    {
        Point[] source = values.ToArray();
        return Compute(polynomialOrder, source.Select(point => point.X).ToList(), source.Select(point => point.Y).ToList());
    }

    /// <summary>
    /// Computes linear regression over given values.
    /// </summary>
    /// <param name="polynomialOrder">An <see cref="int"/> for the polynomial order.</param>
    /// <param name="xValues">A list of <see cref="double"/> x-values.</param>
    /// <param name="yValues">A list of <see cref="double"/> y-values.</param>
    /// <returns>An array of <see cref="double"/> values.</returns>
    public static double[] Compute(int polynomialOrder, IList<double> xValues, IList<double> yValues)
    {
        if (xValues is null)
            throw new ArgumentNullException(nameof(xValues));

        if (yValues is null)
            throw new ArgumentNullException(nameof(yValues));

        if (xValues.Count != yValues.Count)
            throw new ArgumentException("Point count for x-values and y-values must be equal");

        if (!(xValues.Count >= polynomialOrder + 1))
            throw new ArgumentException("Point count must be greater than requested polynomial order");

        if (polynomialOrder is < 1 or > 7)
            throw new ArgumentOutOfRangeException(nameof(polynomialOrder), "Polynomial order must be between 1 and 7");

        // Curve fit function (courtesy of Brian Fox from DatAWare client code)
        double[] coeffs = new double[8];
        double[] sum = new double[22];
        double[] v = new double[12];
        double[,] b = new double[12, 13];
        double p, divB, fMultB, sigma;
        int i1, i, j, k, l;
        int pointCount = xValues.Count;

        int ls = polynomialOrder * 2;
        int lb = polynomialOrder + 1;
        int lv = polynomialOrder;
        sum[0] = pointCount;

        for (i = 0; i < pointCount; i++)
        {
            p = 1.0;
            v[0] = v[0] + yValues[i];

            for (j = 1; j <= lv; j++)
            {
                p = xValues[i] * p;
                sum[j] = sum[j] + p;
                v[j] = v[j] + yValues[i] * p;
            }

            for (j = lb; j <= ls; j++)
            {
                p = xValues[i] * p;
                sum[j] = sum[j] + p;
            }
        }

        for (i = 0; i <= lv; i++)
        {
            for (k = 0; k <= lv; k++)
            {
                b[k, i] = sum[k + i];
            }
        }

        for (k = 0; k <= lv; k++)
        {
            b[k, lb] = v[k];
        }

        for (l = 0; l <= lv; l++)
        {
            divB = b[0, 0];
            for (j = l; j <= lb; j++)
            {
                if (divB == 0)
                    divB = 1;
                b[l, j] = b[l, j] / divB;
            }

            i1 = l + 1;

            if (i1 - lb < 0)
            {
                for (i = i1; i <= lv; i++)
                {
                    fMultB = b[i, l];
                    for (j = l; j <= lb; j++)
                    {
                        b[i, j] = b[i, j] - b[l, j] * fMultB;
                    }
                }
            }
            else
            {
                break;
            }
        }

        coeffs[lv] = b[lv, lb];
        i = lv;

        do
        {
            sigma = 0;
            for (j = i; j <= lv; j++)
            {
                sigma = sigma + b[i - 1, j] * coeffs[j];
            }
            i--;
            coeffs[i] = b[i, lb] - sigma;
        }
        while (i - 1 > 0);

        #region [ Old Code ]

        //    For i = 1 To 7
        //        Debug.Print "Coeffs(" & i & ") = " & Coeffs(i)
        //    Next i

        //For i = 1 To 60
        //    '        CalcY(i).TTag = xValues(1) + ((i - 1) / (xValues(pointCount) - xValues(1)))

        //    CalcY(i).TTag = ((i - 1) / 59) * xValues(pointCount) - xValues(1)
        //    CalcY(i).Value = Coeffs(1)

        //    For j = 1 To polynomialOrder
        //        CalcY(i).Value = CalcY(i).Value + Coeffs(j + 1) * CalcY(i).TTag ^ j
        //    Next
        //Next

        //    SSERROR = 0
        //    For i = 1 To pointCount
        //        SSERROR = SSERROR + (yValues(i) - CalcY(i).Value) * (yValues(i) - CalcY(i).Value)
        //    Next i
        //    SSERROR = SSERROR / (pointCount - polynomialOrder)
        //    sError = SSERROR

        #endregion

        // Return slopes...
        return coeffs;
    }

    /// <summary>
    /// Uses least squares linear regression to estimate the coefficients a, b, and c
    /// from the given (x,y,z) data points for the equation z = a + bx + cy.
    /// </summary>
    /// <param name="zValues">z-value array</param>
    /// <param name="xValues">x-value array</param>
    /// <param name="yValues">y-value array</param>
    /// <param name="a">the out a coefficient</param>
    /// <param name="b">the out b coefficient</param>
    /// <param name="c">the out c coefficient</param>
    public static void LeastSquares(double[] zValues, double[] xValues, double[] yValues, out double a, out double b, out double c)
    {
        double n = zValues.Length;

        double xSum = 0;
        double ySum = 0;
        double zSum = 0;

        double xySum = 0;
        double xzSum = 0;
        double yzSum = 0;

        double xxSum = 0;
        double yySum = 0;

        for (int i = 0; i < n; i++)
        {
            double x = xValues[i];
            double y = yValues[i];
            double z = zValues[i];

            xSum += x;
            ySum += y;
            zSum += z;

            xySum += x * y;
            xzSum += x * z;
            yzSum += y * z;

            xxSum += x * x;
            yySum += y * y;
        }

        double coeff00 = zSum;
        double coeff01 = n;
        double coeff02 = xSum;
        double coeff03 = ySum;

        double coeff10 = xzSum - (xSum * zSum) / n;
        double coeff12 = xxSum - (xSum * xSum) / n;
        double coeff13 = xySum - (xSum * ySum) / n;

        double coeff20 = yzSum - (ySum * zSum) / n - (coeff10 * coeff13) / coeff12;
        double coeff23 = yySum - (ySum * ySum) / n - (coeff13 * coeff13) / coeff12;

        c = coeff20 / coeff23;
        b = (coeff10 - c * coeff13) / coeff12;
        a = (coeff00 - b * coeff02 - c * coeff03) / coeff01;
    }

    /// <summary>
    /// Uses least squares linear regression to estimate the coefficients a and b
    /// from the given (x,y) data points for the equation y = a + bx.
    /// </summary>
    /// <param name="xValues">x-value array</param>
    /// <param name="yValues">y-value array</param>
    /// <param name="a">the out a coefficient</param>
    /// <param name="b">the out b coefficient</param>
    public static void LeastSquares(double[] xValues, double[] yValues, out double a, out double b)
    {
        double n = xValues.Length;

        double xSum = 0;
        double ySum = 0;

        double xySum = 0;

        double xxSum = 0;
        double yySum = 0;

        for (int i = 0; i < n; i++)
        {
            double x = xValues[i];
            double y = yValues[i];

            xSum += x;
            ySum += y;

            xySum += x * y;

            xxSum += x * x;
            yySum += y * y;
        }

        // XtX = [n, x; x, xx]

        // inv(XtX) = 1/div * [xx, -x; -x, n]

        // XtY = [y; xy]

        // veta = inv(XtX) * XtY = 1/d [xx*y - x*xy; -x*y + n*xy]
        double coeff00 = xxSum*ySum - xSum*xySum;
        double coeff01 = n * xySum - xSum * ySum;

        double divisor = 1.0D/(-xSum*xSum + xxSum*n);
      
        b = coeff00 * divisor;
        a = coeff01 * divisor;
    }

}
