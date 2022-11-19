﻿using System;
using System.Diagnostics.CodeAnalysis;

namespace tweetz.core.PowerManagement
{
    [Flags]
    [SuppressMessage("Major Code Smell", "S4070:Non-flags enums should not be marked with \"FlagsAttribute\"")]
    [SuppressMessage("Usage", "CA2217:Do not mark enums with FlagsAttribute")]
    public enum ExecutionStates
    {
        /// <summary>
        ///     No state configured.
        /// </summary>
        None = 0,

        /// <summary>
        ///     Forces the system to be in the working state by resetting the system idle timer.
        /// </summary>
        SystemRequired = 0x1,

        /// <summary>
        ///     Forces the display to be on by resetting the display idle timer.
        /// </summary>
        DisplayRequired = 0x2,

        /// <summary>
        ///     Enables away mode. This value must be specified with ES_CONTINUOUS.
        ///     Away mode should be used only by media-recording and media-distribution applications that must perform critical
        ///     background processing on desktop computers while the computer appears to be sleeping. See Remarks.
        ///     Windows Server 2003 and Windows XP/2000:  ES_AWAYMODE_REQUIRED is not supported.
        /// </summary>
        AwayModeRequired = 0x40,

        /// <summary>
        ///     Informs the system that the state being set should remain in effect until the next call that uses ES_CONTINUOUS and
        ///     one of the other state flags is cleared.
        /// </summary>
        Continuous = unchecked((int)0x80000000)
    }
}