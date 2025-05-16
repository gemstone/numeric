//******************************************************************************************************
//  Matrix.cs - Gbtc
//
//  Copyright © 2012, Grid Protection Alliance.  All Rights Reserved.
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
//  04/08/2025 - C Lackner
//       Initial version of source generated.
//
//******************************************************************************************************


using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Gemstone.Numeric.ComplexExtensions;
using Gemstone.Units;
using System.Numerics;
using System.Linq;
using System.Collections.Generic;
using System.Data;

// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable PossibleInvalidOperationException
namespace Gemstone.Numeric;

/// <summary>
/// Represents a complex number.
/// </summary>
public struct Matrix<T> : ICloneable where T : struct, INumberBase<T>, IComparisonOperators<T, T, bool>
{
    #region [ Members ]

    // Fields
    private T[][] m_value;

    #endregion

    #region [ Constructors ]

    /// <summary>
    /// Creates a <see cref="Matrix{T}"/> of given size, prepopulated with a value. 
    /// </summary>
    /// <param name="cols">The number of columns of this <see cref="Matrix{T}"/>.</param>
    /// <param name="rows">The number of rows of this <see cref="Matrix{T}"/>.</param>
    /// <param name="value"> The value to populate the <see cref="Matrix{T}"/> with.</param>
    public Matrix(int rows, int cols, T value)
        : this()
    {
        T[] row = new T[cols];
        for (int i = 0; i < cols; i++)
        {
            row[i] = value;
        }
        m_value = new T[rows][];

        for (int j = 0; j < rows; j++)
        {
            m_value[j] = (T[])row.Clone();
        }

    }

    /// <summary>
    /// Creates a <see cref="Matrix{T}"/> from the given data. 
    /// </summary>
    /// <param name="data">The data of this <see cref="Matrix{T}"/>.</param>
    public Matrix(T[][] data)
        : this()
    {
        m_value = data;

    }

    /// <summary>
    /// Creates a <see cref="Matrix{T}"/> of repeating rows. 
    /// </summary>
    /// <param name="rows">The number of rows of this <see cref="Matrix{T}"/>.</param>
    /// <param name="row">The values of a row of this <see cref="Matrix{T}"/>.</param>
    public Matrix(int rows, T[] row)
        : this()
    {
        m_value = new T[rows][];

        for (int j = 0; j < rows; j++)
        {
            m_value[j] = (T[])row.Clone();
        }
    }

    /// <summary>
    /// Creates a <see cref="Matrix{T}"/> of repeating columns. 
    /// </summary>
    /// <param name="cols">The number of columns of this <see cref="Matrix{T}"/>.</param>
    /// <param name="column">The values of a collumn of this <see cref="Matrix{T}"/>.</param>
    public Matrix(T[] column, int cols)
        : this()
    {
        m_value = column.Select(v => Enumerable.Repeat(v, cols).ToArray()).ToArray();
    }

    /// <summary>
    /// Gets the Transpose of the <see cref="Matrix{T}"/>.
    /// </summary>
    public Matrix<T> Transpose
    {
        get
        {
            Matrix<T> transposed = new Matrix<T>(NColumns, NRows, default(T));
            for (int j = 0; j < NColumns; j++)
            {
                for (int i = 0; i < NRows; i++)
                {
                    transposed[j][i] = this[i][j];
                }
            }

            return transposed;
        }
    }
    #endregion

    #region [ Properties ]

    /// <summary>
    /// Gets  the value of this <see cref="Matrix{T}"/>.
    /// </summary>
    public T[][] Value
    {
        get => m_value;
    }

    /// <summary>
    /// Gets the number of columns in this <see cref="Matrix{T}"/>.
    /// </summary>
    public int NColumns
    {
        get => NRows > 0 ? m_value[0].Length : 0;
    }

    /// <summary>
    /// Gets the number of rows in this <see cref="Matrix{T}"/>.
    /// </summary>
    public int NRows
    {
        get => m_value.Length;
    }

    /// <summary>
    /// Gets the sum of each column of the <see cref="Matrix{T}"/>.
    /// </summary>
    public T[] ColumnSums
    {
        get
        {
            T[] columnSums = new T[NColumns];

            for (int i = 0; i < NRows; i++)
            {
                for (int j = 0; j < NColumns; j++)
                {
                    if (i == 0)
                        columnSums[j] = m_value[i][j];
                    else
                        columnSums[j] = columnSums[j] + m_value[i][j];
                }
            }
            return columnSums;
        }
    }

    /// <summary>
    /// Gets the sum of each row of the <see cref="Matrix{T}"/>.
    /// </summary>
    public T[] RowSums
    {
        get
        {
            T[] rowSums = new T[NRows];

            for (int i = 0; i < NRows; i++)
            {
                for (int j = 0; j < NColumns; j++)
                {
                    if (j == 0)
                        rowSums[i] = m_value[i][j];
                    else
                        rowSums[i] = rowSums[i] + m_value[i][j];
                }
            }
            return rowSums;
        }
    }

    /// <summary>
    /// Accesses the element stored at (i,j)
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public T this[Tuple<int, int> key]
    {
        get { return m_value[key.Item1][key.Item2]; }
        set { m_value[key.Item1][key.Item2] = value; }
    }

    /// <summary>
    /// Accesses the element stored at (i,j)
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public T[] this[int key]
    {
        get { return m_value[key]; }
        set { m_value[key] = value; }
    }

    #endregion

    #region [ Methods ]

    /// <summary>
    /// Returns a <see cref="Matrix{T}"/> resulting from transposing this matrix and multiplying it with a <see cref="Matrix{U}"/>
    /// </summary>
    /// <typeparam name="U"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public Matrix<T> TransposeAndMultiply<U>(Matrix<U> value) where U : struct, INumberBase<U>, IMultiplyOperators<U, T, T>, IComparisonOperators<U, U, bool>
    {
        if (NRows != value.NRows)
            throw new ArgumentException("Cannot multiply matrices due to dimension missmatch.");
        Matrix<T> data = new Matrix<T>(NColumns, value.NColumns, default(T));
        for (int i = 0; i < NColumns; ++i)
        {
            for (int k = 0; k < NRows; ++k)
            {
                for (int j = 0; j < value.NColumns; ++j)
                {
                    if (k == 0)
                        data[i][j] = value[k][j] * m_value[k][i];
                    else
                        data[i][j] += value[k][j] * m_value[k][i];
                }
            }
        }
        return data;
    }


    /// <summary>
    /// Returns a <see cref="Matrix{T}"/> resulting from transposing this matrix and multiplying it with a 1xN <see cref="Matrix{U}"/>
    /// </summary>
    /// <typeparam name="U"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public Matrix<T> TransposeAndMultiply<U>(U[] value) where U : struct, INumberBase<U>, IMultiplyOperators<U, T, T>, IComparisonOperators<U, U, bool>
    { 
        if (NColumns == value.Length)
            return TransposeAndMultiply(new Matrix<U>(1, value));
        if (NRows == value.Length)
            return TransposeAndMultiply(new Matrix<U>(value, 1));
        throw new ArgumentException("Cannot multiply matrices due to dimension missmatch.");
    }


    /// <summary>
    /// Applies the given function to each row of the <see cref="Matrix{T}"/>.
    /// </summary>
    /// <param name="func"></param>
    public void OperateByRow(Action<T[]> func)
    {
        for (int i = 0; i < NRows; ++i)
            func.Invoke(m_value[i]);
    }

    /// <summary>
    /// Applies the given function to each column of the <see cref="Matrix{T}"/>.
    /// </summary>
    /// <param name="func"></param>
    public void OperateByColumn(Action<T[]> func)
    {
        for (int i = 0; i < NColumns; ++i)
        {
            T[] col = GetColumn(i);
            func.Invoke(col);
            for (int j = 0; j < col.Length; j++)
                m_value[j][i] = col[j];
        }
    }

    /// <summary>
    /// Applies the given function to each value of the <see cref="Matrix{T}"/>.
    /// </summary>
    /// <param name="func"></param>
    public void OperateByValue(Func<T, int, int, T> func)
    {
        for (int i = 0; i < NRows; ++i)
        {
            for (int k = 0; k < NColumns; ++k)
            {
                this[i][k] = func.Invoke(this[i][k], i, k);
            }
        }
    }

    public Matrix<U> TransformByValue<U>(Func<T, int, int, U> func) where U : struct, INumberBase<U>, IComparisonOperators<U, U, bool>
    {
        Matrix<U> data = new Matrix<U>(NRows, NColumns, default(U));
        for (int i = 0; i < NRows; ++i)
        {
            for (int k = 0; k < NColumns; ++k)
            {
                data[i][k] = func.Invoke(this[i][k], i, k);
            }
        }
        return data;
    }

    public Matrix<U> TransformByValue<U>(Func<T, U> func) where U : struct, INumberBase<U>, IComparisonOperators<U, U, bool>
        => TransformByValue((v, i, j) => func.Invoke(v));


    /// <summary>
    /// returns the speciefied row of the Matrix
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public T[] GetRow(int index) => m_value[index];

    /// <summary>
    /// returns the specified column of the Matrix
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public T[] GetColumn(int index)
    {
        return GetColumnEnumerable(index).ToArray();
    }

    public IEnumerable<T> GetColumnEnumerable(int index)
    {
        if (index < 0 || index >= NColumns)
            throw new ArgumentOutOfRangeException(nameof(index), "Index out of range.");

        int i = 0;
        while (i < NRows)
        {
            yield return m_value[i][index];
            i++;
        }
    }

    /// <summary>
    /// Gets a submatrix of the <see cref="Matrix{T}"/> starting at the given row and column, with the given number of rows and columns.
    /// </summary>
    /// <param name="startRow"> the first row of the submatrix. </param>
    /// <param name="startColumn"> the first column of the submarix. </param>
    /// <param name="numRows"> the number of rows of the submatrix. </param>
    /// <param name="numCols"> the number of columns of the submatrix. </param>
    /// <returns>A new <see cref="Matrix{T}"/> that is the submatrix speciffied. </returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public Matrix<T> GetSubmatrix(int startRow, int startColumn, int numRows, int numCols)
    {
        if (startRow < 0 || startRow >= NRows)
            throw new ArgumentOutOfRangeException(nameof(startRow), "Index out of range.");
        if (startColumn < 0 || startColumn >= NColumns)
            throw new ArgumentOutOfRangeException(nameof(startColumn), "Index out of range.");

        if (startColumn + numCols > NColumns)
            throw new ArgumentOutOfRangeException(nameof(startColumn), "Index out of range.");
        if (startRow + numRows > NRows)
            throw new ArgumentOutOfRangeException(nameof(startRow), "Index out of range.");

        Matrix<T> matrix = new Matrix<T>(numRows, numCols, default(T));
        for (int i = 0; i < numRows; ++i)
        {
            for (int j = 0; j < numCols; ++j)
            {
                matrix[i][j] = m_value[startRow + i][startColumn + j];
            }
        }

        return matrix;
    }

    public void ReplaceSubmatrix(Matrix<T> subMatrix, int startRow, int startColumn)
    {
        if (startRow < 0 || startRow >= NRows)
            throw new ArgumentOutOfRangeException(nameof(startRow), "Index out of range.");
        if (startColumn < 0 || startColumn >= NColumns)
            throw new ArgumentOutOfRangeException(nameof(startColumn), "Index out of range.");

        if (startColumn + subMatrix.NColumns > NColumns)
            throw new ArgumentOutOfRangeException(nameof(startColumn), "Index out of range.");
        if (startRow + subMatrix.NRows > NRows)
            throw new ArgumentOutOfRangeException(nameof(startRow), "Index out of range.");

        for (int i = 0; i < subMatrix.NRows; ++i)
        {
            for (int j = 0; j < subMatrix.NColumns; ++j)
            {
                this[i + startRow][j + startColumn] = subMatrix[i][j];
            }
        }
    }

    public Matrix<T> PointWhiseMultiply(Matrix<T> matrix)
    {
        if (this.NColumns != matrix.NColumns || matrix.NRows != this.NRows)
            throw new ArgumentException("Matrix sizes do not match.");

        Matrix<T> data = new Matrix<T>(NRows, NColumns, default(T));
        for (int i = 0; i < NRows; ++i)
        {
            for (int k = 0; k < NColumns; ++k)
            {
                data[i][k] = this[i][k] * matrix[i][k];
            }
        }
        return data;
    }

    public object Clone()
    {
        return new Matrix<T>(m_value.Select(v => (T[])v.Clone()).ToArray());
    }

    public Matrix<T> FlipUpsideDown()
    {
        Matrix<T> data = new Matrix<T>(NRows, NColumns, default(T));
        for (int i = 0; i < NRows; ++i)
        {
            data[NRows - i - 1] = (T[])m_value[i].Clone();
        }
        return data;
    }


    /// <summary>
    /// Returns the inverse of the matrix.
    /// </summary>
    public Matrix<T> Inverse()
    {
        Matrix<T> result = (Matrix<T>)this.Clone();

        int[] Permutation;
        Matrix<T> LU = CombinedLUPDecomposition(out Permutation);

        T[] b = Enumerable.Repeat(T.Zero, NRows).ToArray();
       
        for (int i = 0; i < NRows; ++i)
        {
            int j = Permutation.TakeWhile(x => x != i).Count();
            b[j] = T.One;

            T[] x = HelperSolve(LU, b); // 
            b[j] = T.Zero;
            for (int k = 0; k < NRows; k++)
                result[k][i] = x[k];
        }
        return result;
    }

    /// <summary>
    /// Does an LUP Decomposition.
    /// </summary>
    /// <param name="Lower"> The Lower Triangular Matrix</param>
    /// <param name="Upper"> The upper Triangular Matrix </param>
    /// <param name="Permutation"> The Permutation Matrix P</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public void LUDecomposition(out Matrix<T> Lower, out Matrix<T> Upper, out int[] Permutation)
    {
        // Doolittle LUP decomposition with partial pivoting.
        // rerturns: result is L (with 1s on diagonal) and U;
        // perm holds row permutations;
       
        Upper = CombinedLUPDecomposition(out Permutation);
        Lower = Identity(NRows);

        for (int i = 1; i  < NRows; i++)
        {
            for (int j = 0; j < i; j++)
            {
                Lower[i][j] = Upper[i][j];
                Upper[i][j] = T.Zero;
            }
        }
    }

    private Matrix<T> CombinedLUPDecomposition(out int[] Permutation)
    {
        // Doolittle LUP decomposition with partial pivoting.
        // rerturns: result is L (with 1s on diagonal) and U;
        // perm holds row permutations; toggle is +1 or -1 (even or odd)
        if (NRows != NColumns)
            throw new Exception("Attempt to decompose a non-square m");

        Matrix<T> result = new(this);

        Permutation = Enumerable.Range(0, NRows).ToArray();



        for (int j = 0; j < NColumns; ++j) // each column
        {
            T colMax = this[j][j];
            int pRow = j; // row of largest value in column j
            for (int i = j + 1; i < NRows; ++i)
            {
                if (T.Abs(this[i][j]) > T.Abs(colMax))
                {
                    colMax = T.Abs(this[i][j]);
                    pRow = i;
                }
            }

            if (pRow != j) // if largest value not on pivot, swap rows
            {
                T[] row = result[pRow];
                result[j] = result[j].Select((v, i) => (i <= j ? v : row[i])).ToArray();

                int tmp = Permutation[pRow]; // and swap perm info
                Permutation[pRow] = Permutation[j];
                Permutation[j] = tmp;
            }

            //  if (result[j][j] == 0.0)
            //  {
            //      // find a good row to swap
            //      int goodRow = -1;
            //      for (int row = j + 1; row less - than n;
            //      ++row)
            //{
            //          if (result[row][j] != 0.0)
            //              goodRow = row;
            //      }

            //      if (goodRow == -1)
            //          throw new Exception("Cannot use Doolittle's method");

            //      // swap rows so 0.0 no longer on diagonal
            //      double[] rowPtr = result[goodRow];
            //      result[goodRow] = result[j];
            //      result[j] = rowPtr;

            //      int tmp = perm[goodRow]; // and swap perm info
            //      perm[goodRow] = perm[j];
            //      perm[j] = tmp;

            //      toggle = -toggle; // adjust the row-swap toggle
            //  }
            //  // --------------------------------------------------
            //  // if diagonal after swap is zero . .
            //  //if (Math.Abs(result[j][j]) less-than 1.0E-20) 
            //  //  return null; // consider a throw

            for (int i = j + 1; i < NRows; ++i)
            {
                result[i][j] /= result[j][j];

                for (int k = j + 1; k < NRows; ++k)
                    result[i][k] -= result[i][j] * result[j][k];                
            }

        }

        return result;
    }

    private T[] HelperSolve(Matrix<T> LU, T[] b)
    {
        // before calling this helper, permute b using the perm array
        // from MatrixDecompose that generated luMatrix

        T[] x = new T[b.Length];
        x[0] = b[0];
      
        for (int i = 1; i < LU.NRows; ++i)
        {
            x[i] = b[i];
            for (int j = 0; j < i; ++j)
                x[i] -= LU[i][j] * x[j];
        }

        x[LU.NRows - 1] /= LU[LU.NRows - 1][LU.NRows - 1];

        for (int i = LU.NRows - 2; i >= 0; --i)
        {
            for (int j = i + 1; j < LU.NRows; ++j)
                x[i] -= LU[i][j] * x[j];
            x[i] = x[i] / LU[i][i];
        }

        return x;
    }

    #endregion

    #region [ Operators ]

    /// <summary>
    /// Implicitly converts a <see cref="Double"/> to a <see cref="ComplexNumber"/>.
    /// </summary>
    /// <param name="value">Operand.</param>
    /// <returns>ComplexNumber representing the result of the operation.</returns>
    public static implicit operator Matrix<T>(T[][] value) =>
        new(value);

    /// <summary>
    /// Implicitly converts a <see cref="ComplexNumber"/> to a .NET <see cref="Complex"/> value.
    /// </summary>
    /// <param name="value">Operand.</param>
    /// <returns>Complex representing the result of the operation.</returns>
    public static implicit operator T[][](Matrix<T> value) => value.Value;


    /// <summary>
    /// Returns the negated value.
    /// </summary>
    /// <param name="z">Left hand operand.</param>
    /// <returns>ComplexNumber representing the result of the unary negation operation.</returns>
    public static Matrix<T> operator -(Matrix<T> z) =>
        new(z.Value.Select(v => v.Select((x) => -x).ToArray()).ToArray());

    /// <summary>
    /// Returns computed sum of values.
    /// </summary>
    /// <param name="value1">Left hand operand.</param>
    /// <param name="value2">Right hand operand.</param>
    /// <returns><see cref="Matrix{T}"/> representing the result of the addition operation.</returns>
    public static Matrix<T> operator +(Matrix<T> value1, Matrix<T> value2)
    {
        if (value1.NColumns != value2.NColumns || value1.NRows != value2.NRows)
            throw new ArgumentException("Matrix sizes do not match.");
        return new(value1.Value.Select((v, i) => v.Select((x, j) => x + value2.Value[i][j]).ToArray()).ToArray());
    }

    /// <summary>
    /// Returns computed sum of values.
    /// </summary>
    /// <param name="value1">Left hand operand.</param>
    /// <param name="value2">Right hand operand.</param>
    /// <returns><see cref="Matrix{T}"/> representing the result of the addition operation.</returns>
    public static Matrix<T> operator +(Matrix<T> value1, T value2)
    {
        return new(value1.Value.Select((v, i) => v.Select((x, j) => x + value2).ToArray()).ToArray());
    }

    public static Matrix<T> operator +(T value1, Matrix<T> value2) => value2 + value1;


    /// <summary>
    /// Returns computed difference of values.
    /// </summary>
    /// <param name="value1">Left hand operand.</param>
    /// <param name="value2">Right hand operand.</param>
    /// <returns><see cref="Matrix{T}"/> representing the result of the subtraction operation.</returns>
    public static Matrix<T> operator -(Matrix<T> value1, Matrix<T> value2)
    {
        if (value1.NColumns != value2.NColumns || value1.NRows != value2.NRows)
            throw new ArgumentException("Matrix sizes do not match.");
        return new(value1.Value.Select((v, i) => v.Select((x, j) => x - value2.Value[i][j]).ToArray()).ToArray());
    }

    public static Matrix<T> operator -(Matrix<T> value1, T value2)
    {
        return new(value1.Value.Select((v, i) => v.Select((x, j) => x - value2).ToArray()).ToArray());
    }

    public static Matrix<T> operator -(T value1, Matrix<T> value2) => (-value2) + value1;

    public static Matrix<T> operator -(Matrix<T> value1, T[] value2)
    {
        if (value1.NColumns == value2.Length)
            return new(value1.Value.Select((v, i) => v.Select((x, j) => x - value2[j]).ToArray()).ToArray());
        if (value1.NRows == value2.Length)
            return new(value1.Value.Select((v, i) => v.Select((x, j) => x - value2[i]).ToArray()).ToArray());

        throw new ArgumentException("Matrix sizes do not match.");


    }

    /// <summary>
    /// Returns computed product of values.
    /// </summary>
    /// <param name="value1">Left hand operand.</param>
    /// <param name="value2">Right hand operand.</param>
    /// <returns><see cref="Matrix{T}"/> representing the result of the multiplication operation.</returns>
    public static Matrix<T> operator *(Matrix<T> value1, Matrix<T> value2)
    {
        if (value1.NColumns != value2.NRows)
            throw new ArgumentException("Cannot multiply matrices due to dimension missmatch.");

        Matrix<T> data = new Matrix<T>(value2.NRows, value2.NColumns, default(T));
        for (int i = 0; i < value1.NRows; ++i)
        {
            for (int k = 0; k < value2.NColumns; ++k)
            {
                data[i][k] = value1.GetRow(i).Zip(value2.GetColumnEnumerable(k), (v1, v2) => v1 * v2).Aggregate(default(T), (s, v) => s + v);
            }
        }
        return data;
    }

    /// <summary>
    /// Returns computed product of values.
    /// </summary>
    /// <param name="value1">Left hand operand.</param>
    /// <param name="value2">Right hand operand.</param>
    /// <returns><see cref="Matrix{T}"/> representing the result of the multiplication operation.</returns>
    public static Matrix<T> operator *(Matrix<T> value1, T value2)
    {
        return new(value1.Value.Select((v, i) => v.Select((x, j) => x * value2).ToArray()).ToArray());
    }

    /// <summary>
    /// Returns computed product of values.
    /// </summary>
    /// <param name="value1">Left hand operand.</param>
    /// <param name="value2">Right hand operand.</param>
    /// <returns><see cref="Matrix{T}"/> representing the result of the multiplication operation.</returns>
    public static Matrix<T> operator *(T value1, Matrix<T> value2) => value2 * value1;

    public static Matrix<T> operator /(Matrix<T> value1, T value2)
    {
        return new(value1.Value.Select((v, i) => v.Select((x, j) => x / value2).ToArray()).ToArray());
    }

    public static Matrix<T> operator /(Matrix<T> value1, Matrix<T> value2)
    {
        if (value1.NColumns != value2.NColumns || value1.NRows != value2.NRows)
            throw new ArgumentException("Matrix sizes do not match.");

        Matrix<T> data = new Matrix<T>(value1.NRows, value1.NColumns, default(T));
        for (int i = 0; i < value1.NRows; ++i)
        {
            for (int k = 0; k < value1.NColumns; ++k)
            {
                data[i][k] = value1[i][k] * value2[i][k];
            }
        }
        return data;
    }

    #endregion

    #region [ Static ]

    public static Matrix<T> Combine(Matrix<T>[] matrices)
    {
        if (matrices.Length == 0)
            throw new ArgumentException("List of matrices cannot be empty.");
        int nCols = matrices.Sum(m => m.NColumns);
        int nRows = matrices[0].NRows;

        if (matrices.Any(m => m.NRows != nRows))
            throw new ArgumentException("All matrices must have the same number of rows.");
        
        Matrix<T> matrix = new Matrix<T>(nRows, nCols, default(T));

        int count = 0;
        for (int j = 0; j < nRows; j++)
        {
            count = 0;
            for (int i = 0; i < matrices.Length; i++)
            {
                for (int k = 0; k < matrices[i].NColumns; k++)
                {
                    matrix[j][count] = matrices[i][j][k];
                    count++;
                }
            }
        }
        
        return matrix;
    }

    public static Matrix<T> Identity(int n)
    {
        Matrix<T> result = new Matrix<T>(n, n, T.Zero);
        for (int i = 0; i < n; i++)
            result[i][i] = T.One;
        return result;
    }
    #endregion
}
