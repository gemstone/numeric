//******************************************************************************************************
//  DateTimeExtensions.cs - Gbtc
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
//  02/23/2003 - J. Ritchie Carroll
//       Generated original version of source code.
//  12/21/2005 - J. Ritchie Carroll
//       Migrated 2.0 version of source code from 1.1 source (GSF.Shared.DateTime).
//  08/31/2007 - Darrell Zuercher
//       Edited code comments.
//  09/08/2008 - J. Ritchie Carroll
//       Converted to C# extensions.
//  02/16/2009 - Josh L. Patterson
//       Edited Code Comments.
//  09/14/2009 - Stephen C. Wills
//       Added new header and license agreement.
//  01/06/2010 - Andrew K. Hill
//       Modified the following methods per unit testing:
//       LocalTimeTo(DateTime, string)
//       LocalTimeTo(DateTime, TimeZoneInfo)
//       UniversalTimeTo(DateTime, string)
//       UniversalTimeTo(DateTime, TimeZoneInfo)
//       TimeZoneToTimeZone(DateTime, string, string)
//       TimeZoneToTimeZone(DateTime, TimeZoneInfo, TimeZoneInfo)
//  12/14/2012 - Starlynn Danyelle Gilliam
//       Modified Header.
//
//******************************************************************************************************

using System;
using System.Globalization;

namespace gemstone.numeric.units
{
    /// <summary>
    /// Defines extension functions related to Date/Time manipulation.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>Determines if the specified UTC time is valid, by comparing it to the system clock.</summary>
        /// <param name="utcTime">UTC time to test for validity.</param>
        /// <param name="lagTime">The allowed lag time, in seconds, before assuming time is too old to be valid.</param>
        /// <param name="leadTime">The allowed lead time, in seconds, before assuming time is too advanced to be
        /// valid.</param>
        /// <returns>True, if time is within the specified range.</returns>
        /// <remarks>
        /// <para>Time is considered valid if it exists within the specified lag time/lead time range of current
        /// time.</para>
        /// <para>Note that lag time and lead time must be greater than zero, but can be set to sub-second
        /// intervals.</para>
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">LagTime and LeadTime must be greater than zero, but can
        /// be less than one.</exception>
        public static bool UtcTimeIsValid(this DateTime utcTime, double lagTime, double leadTime) => ((Ticks)utcTime).UtcTimeIsValid(lagTime, leadTime);

        /// <summary>Determines if the specified local time is valid, by comparing it to the system clock.</summary>
        /// <param name="localTime">Time to test for validity.</param>
        /// <param name="lagTime">The allowed lag time, in seconds, before assuming time is too old to be valid.</param>
        /// <param name="leadTime">The allowed lead time, in seconds, before assuming time is too advanced to be
        /// valid.</param>
        /// <returns>True, if time is within the specified range.</returns>
        /// <remarks>
        /// <para>Time is considered valid if it exists within the specified lag time/lead time range of current
        /// time.</para>
        /// <para>Note that lag time and lead time must be greater than zero, but can be set to sub-second
        /// intervals.</para>
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">LagTime and LeadTime must be greater than zero, but can
        /// be less than one.</exception>
        public static bool LocalTimeIsValid(this DateTime localTime, double lagTime, double leadTime) => ((Ticks)localTime).LocalTimeIsValid(lagTime, leadTime);

        /// <summary>Determines if time is valid, by comparing it to the specified current time.</summary>
        /// <param name="testTime">Time to test for validity.</param>
        /// <param name="currentTime">Specified current time (e.g., could be Date.Now or Date.UtcNow).</param>
        /// <param name="lagTime">The allowed lag time, in seconds, before assuming time is too old to be valid.</param>
        /// <param name="leadTime">The allowed lead time, in seconds, before assuming time is too advanced to be
        /// valid.</param>
        /// <returns>True, if time is within the specified range.</returns>
        /// <remarks>
        /// <para>Time is considered valid if it exists within the specified lag time/lead time range of current
        /// time.</para>
        /// <para>Note that lag time and lead time must be greater than zero, but can be set to sub-second
        /// intervals.</para>
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">LagTime and LeadTime must be greater than zero, but can
        /// be less than one.</exception>
        public static bool TimeIsValid(this DateTime testTime, DateTime currentTime, double lagTime, double leadTime) => ((Ticks)testTime).TimeIsValid(currentTime, lagTime, leadTime);

        /// <summary>Gets the distance, in <see cref="Ticks"/>, beyond the top of the <paramref name="timestamp"/> second.</summary>
        /// <param name="timestamp">Timestamp to evaluate.</param>
        /// <returns>Timestamp's tick distance from the top of the second.</returns>
        public static Ticks DistanceBeyondSecond(this DateTime timestamp) => ((Ticks)timestamp).DistanceBeyondSecond();

        /// <summary>Creates a baselined timestamp which begins at the specified time interval.</summary>
        /// <param name="timestamp">Timestamp to baseline.</param>
        /// <param name="interval">
        /// <see cref="BaselineTimeInterval"/> to which <paramref name="timestamp"/> should be baselined.
        /// </param>
        /// <returns>
        /// A new <see cref="DateTime"/> value that represents a baselined timestamp that begins at the
        /// specified <see cref="BaselineTimeInterval"/>.
        /// </returns>
        /// <remarks>
        /// Baselining to the <see cref="BaselineTimeInterval.Second"/> would return the <see cref="DateTime"/>
        /// value starting at zero milliseconds.<br/>
        /// Baselining to the <see cref="BaselineTimeInterval.Minute"/> would return the <see cref="DateTime"/>
        /// value starting at zero seconds and milliseconds.<br/>
        /// Baselining to the <see cref="BaselineTimeInterval.Hour"/> would return the <see cref="DateTime"/>
        /// value starting at zero minutes, seconds and milliseconds.<br/>
        /// Baselining to the <see cref="BaselineTimeInterval.Day"/> would return the <see cref="DateTime"/>
        /// value starting at zero hours, minutes, seconds and milliseconds.<br/>
        /// Baselining to the <see cref="BaselineTimeInterval.Month"/> would return the <see cref="DateTime"/>
        /// value starting at day one, zero hours, minutes, seconds and milliseconds.<br/>
        /// Baselining to the <see cref="BaselineTimeInterval.Year"/> would return the <see cref="DateTime"/>
        /// value starting at month one, day one, zero hours, minutes, seconds and milliseconds.
        /// </remarks>
        public static DateTime BaselinedTimestamp(this DateTime timestamp, BaselineTimeInterval interval) => ((Ticks)timestamp).BaselinedTimestamp(interval);
    }
}
