﻿//******************************************************************************************************
//  VoltageLevel.cs - Gbtc
//
//  Copyright © 2021, Grid Protection Alliance.  All Rights Reserved.
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
//  12/28/2020 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Gemstone.EnumExtensions;

#pragma warning disable IDE1006 // Naming Styles

namespace Gemstone.Numeric.EE;

/// <summary>
/// Common transmission voltage levels enumeration.
/// </summary>
[Serializable]
public enum VoltageLevel : byte
{
    // Note: Not assigning kV level as enum value is intentional so
    // that enum value will fit within a byte. Use extension method
    // "Value()" to get actual voltage level value, e.g.:
    // (int)VoltageLevel.kV115 == 3 && VoltageLevel.kV115.Value() == 115

    /// <summary>
    /// 44 kV.
    /// </summary>
    [Description("44")]
    kV44 = 1,
    /// <summary>
    /// 69 kV.
    /// </summary>
    [Description("69")]
    kV69,
    /// <summary>
    /// 115 kV.
    /// </summary>
    [Description("115")]
    kV115,
    /// <summary>
    /// 138 kV.
    /// </summary>
    [Description("138")]
    kV138,
    /// <summary>
    /// 161 kV.
    /// </summary>
    [Description("161")]
    kV161,
    /// <summary>
    /// 169 kV.
    /// </summary>
    [Description("169")]
    kV169,
    /// <summary>
    /// 230 kV.
    /// </summary>
    [Description("230")]
    kV230,
    /// <summary>
    /// 345 kV.
    /// </summary>
    [Description("345")]
    kV345,
    /// <summary>
    /// 500 kV.
    /// </summary>
    [Description("500")]
    kV500,
    /// <summary>
    /// 765 kV.
    /// </summary>
    [Description("765")]
    kV765,
    /// <summary>
    /// 1100 kV.
    /// </summary>
    [Description("1100")]
    kV1100
}

/// <summary>
/// Defines extension functions related to <see cref="VoltageLevel"/> enumeration.
/// </summary>
public static class VoltageLevelExtensions
{
    private static readonly Dictionary<VoltageLevel, int> m_voltageLevelMap;

    static VoltageLevelExtensions()
    {
        m_voltageLevelMap = new Dictionary<VoltageLevel, int>();

        foreach (VoltageLevel value in Enum.GetValues(typeof(VoltageLevel)))
        {
            if (int.TryParse(value.GetDescription(), out int level))
                m_voltageLevelMap[value] = level;
        }
    }

    /// <summary>
    /// Gets the voltage level for the specified <see cref="VoltageLevel"/> enum value.
    /// </summary>
    /// <param name="level">Target <see cref="VoltageLevel"/> enum value.</param>
    /// <returns>Voltage level for the specified <paramref name="level"/>.</returns>
    public static int Value(this VoltageLevel level) =>
        m_voltageLevelMap.TryGetValue(level, out int value) ? value : 0;

    /// <summary>
    /// Attempts to get the <see cref="VoltageLevel"/> enum value for the source kV <paramref name="value"/>.
    /// </summary>
    /// <param name="value">kV value to attempt to find.</param>
    /// <param name="level">Mapped <see cref="VoltageLevel"/> enum value, if found.</param>
    /// <returns>
    /// <c>true</c> if matching <see cref="VoltageLevel"/> enum value is found for specified kV
    /// <paramref name="value"/>; otherwise, <c>false</c>.
    /// </returns>
    public static bool TryGetVoltageLevel(this int value, out VoltageLevel level)
    {
        foreach (KeyValuePair<VoltageLevel, int> kvp in m_voltageLevelMap)
        {
            if (kvp.Value == value)
            {
                level = kvp.Key;
                return true;
            }
        }

        level = default;
        return false;
    }
}
