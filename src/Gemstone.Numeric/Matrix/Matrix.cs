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

// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable PossibleInvalidOperationException
namespace Gemstone.Numeric;

/// <summary>
/// Represents a complex number.
/// </summary>
public struct Matrix<T> : ICloneable where T : struct, IEquatable<T>, IAdditionOperators<T, T, T>, IUnaryNegationOperators<T, T>, ISubtractionOperators<T, T, T>, IMultiplyOperators<T, T, T>, IDivisionOperators<T, T, T>
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
        m_value = Enumerable.Repeat(Enumerable.Repeat(value, cols).ToArray(), rows).ToArray();
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
        m_value = Enumerable.Repeat(row, rows).ToArray();
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
            Matrix<T> transposed = new Matrix<T>(NRows, NColumns, default(T));
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

    // Needs to be Fixed
    public T[] ColumnSums
    {
        get
        {
            int i = 0;
            T[] columnSums = new T[m_value.Length];

            while (i < m_value.Length)
            {
                columnSums[i] = m_value[i][0];
                int j = 1;
                while (j < m_value[i].Length)
                {
                    columnSums[i] = columnSums[i] + m_value[i][j];
                    j++;
                }
                i++;
            }
            return columnSums;
        }
    }

    // Needs to be Fixed
    public T[] RowSums
    {
        get
        {
            int i = 0;
            T[] columnSums = new T[m_value.Length];

            while (i < m_value.Length)
            {
                columnSums[i] = m_value[i][0];
                int j = 1;
                while (j < m_value[i].Length)
                {
                    columnSums[i] = columnSums[i] + m_value[i][j];
                    j++;
                }
                i++;
            }
            return columnSums;
        }
    }

    public T this[Tuple<int, int> key]
    {
        get { return m_value[key.Item1][key.Item2]; }
        set { m_value[key.Item1][key.Item2] = value; }
    }

    public T[] this[int key]
    {
        get { return m_value[key]; }
        set { m_value[key] = value; }
    }

    #endregion

    #region [ Methods ]

    public Matrix<T> TransposeAndMultiply<U>(Matrix<U> value) where U : struct, IEquatable<U>, IAdditionOperators<U, U, U>, IUnaryNegationOperators<U, U>, ISubtractionOperators<U, U, U>, IMultiplyOperators<U, U, U>, IMultiplyOperators<U, T, T>, IDivisionOperators<U, U, U>
    {

        if (NColumns != value.NRows)
            throw new ArgumentException("Cannot multiply matrices due to dimension missmatch.");
        Matrix<T> data = new Matrix<T>(value.NColumns, value.NRows, default(T));
        for (int i = 0; i < NColumns; ++i)
        {
            for (int k = 0; k < NRows; ++k)
            {
                for (int j = 0; j < NColumns; ++j)
                {
                    data[i][j] += value[k][j] * m_value[i][k];
                }
            }
        }
        return data;
    }

    public Matrix<T> TransposeAndMultiply<U>(U[] value) where U : struct, IEquatable<U>, IAdditionOperators<U, U, U>, IUnaryNegationOperators<U, U>, ISubtractionOperators<U, U, U>, IMultiplyOperators<U, U, U>, IMultiplyOperators<U, T, T>, IDivisionOperators<U, U, U>
    {
        if (NColumns == value.Length)
            return TransposeAndMultiply(new Matrix<U>(value, 1));
        if (NRows == value.Length)
            return TransposeAndMultiply(new Matrix<U>(1, value));
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
    /// Applies the given function to each value of the <see cref="Matrix{T}"/>.
    /// </summary>
    /// <param name="func"></param>
    public void OperateByValue(Func<T, T> func)
    {
        for (int i = 0; i < NRows; ++i)
        {
            for (int k = 0; k < NColumns; ++k)
            {
                this[i][k] = func.Invoke(this[i][k]);
            }
        }
    }

    public Matrix<U> TransformByValue<U>(Func<T, U> func) where U : struct, IEquatable<U>, IAdditionOperators<U, U, U>, IUnaryNegationOperators<U, U>, ISubtractionOperators<U, U, U>, IMultiplyOperators<U, U, U>, IDivisionOperators<U, U, U>
    {
        Matrix<U> data = new Matrix<U>(NRows, NColumns, default(U));
        for (int i = 0; i < NRows; ++i)
        {
            for (int k = 0; k < NColumns; ++k)
            {
                data[i][k] = func.Invoke(this[i][k]);
            }
        }
        return data;
    }

    public T[] GetRow(int index) => m_value[index];


    public T[] GetColumn(int index)
    {
        return GetColumnEnumerable(index).ToArray();
    }

    public IEnumerable<T> GetColumnEnumerable(int index)
    {
        if (index < 0 || index >= NColumns)
            throw new ArgumentOutOfRangeException(nameof(index), "Index out of range.");

        int i = 0;
        while (i < NColumns)
        {
            yield return m_value[i][index];
            i++;
        }
    }

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

    public static Matrix<T> Combine(IEnumerable<Matrix<T>> matrices)
    {
        if (matrices.Count() == 0)
            throw new ArgumentException("List of matrices cannot be empty.");
        int nCols = matrices.Sum(m => m.NColumns);
        int nRows = matrices.First().NRows;

        if (matrices.Any(m => m.NRows != nRows))
            throw new ArgumentException("All matrices must have the same number of rows.");
        
        return new Matrix<T>(matrices.SelectMany((m) => m.Value).ToArray());
    }
    #endregion
}
