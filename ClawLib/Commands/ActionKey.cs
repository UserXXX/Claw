using System;

namespace Claw.Commands
{
    /// <summary>
    /// Enumeration for keys on mouse and keyboard.
    /// </summary>
    public enum ActionKey : uint
    {
        A = 0x00000004,
        B = 0x00000005,
        C = 0x00000006,
        D = 0x00000007,
        E = 0x00000008,
        F = 0x00000009,
        G = 0x0000000A,
        H = 0x0000000B,
        I = 0x0000000C,
        J = 0x0000000D,
        K = 0x0000000E,
        L = 0x0000000F,
        M = 0x00000010,
        N = 0x00000011,
        O = 0x00000012,
        P = 0x00000013,
        Q = 0x00000014,
        R = 0x00000015,
        S = 0x00000016,
        T = 0x00000017,
        U = 0x00000018,
        V = 0x00000019,
        W = 0x0000001A,
        X = 0x0000001B,
        Y = 0x0000001D,
        Z = 0x0000001C,

        One = 0x0000001E,
        Two = 0x0000001F,
        Three = 0x00000020,
        Four = 0x00000021,
        Five = 0x00000022,
        Six = 0x00000023,
        Seven = 0x00000024,
        Eight = 0x00000025,
        Nine = 0x00000026,
        Zero = 0x00000027,

        Enter = 0x00000028,

        /// <summary>
        /// Plus (+) key on the main keyboard.
        /// </summary>
        Plus = 0x00000030,
        /// <summary>
        /// Sharp (#) key on the keyboard, also referred as "hashtag".
        /// </summary>
        Sharp = 0x00000031,

        Backspace = 0x0000002A,

        Space = 0x0000002C,

        /// <summary>
        /// German sharp s (ß) key.
        /// </summary>
        SharpS = 0x0000002D,

        /// <summary>
        /// German ü key.
        /// </summary>
        Ue = 0x0000002F,

        /// <summary>
        /// Acute (´) key.
        /// </summary>
        Acute = 0x0000002E,

        /// <summary>
        /// German ö key.
        /// </summary>
        Oe = 0x00000033,
        /// <summary>
        /// German ä key.
        /// </summary>
        Ae = 0x00000034,
        /// <summary>
        /// Pow (^) key.
        /// </summary>
        Caret = 0x00000035,
        /// <summary>
        /// Equals (=) key.
        /// </summary>
        Equals = 0x00000036,
        /// <summary>
        /// Period (.) key.
        /// </summary>
        Period = 0x00000037,
        /// <summary>
        /// Minus (-) key.
        /// </summary>
        Minus = 0x00000038,

        F1 = 0x0000003A,
        F2 = 0x0000003B,
        F3 = 0x0000003C,
        F4 = 0x0000003D,
        F5 = 0x0000003E,
        F6 = 0x0000003F,
        F7 = 0x00000040,
        F8 = 0x00000041,
        F9 = 0x00000042,
        F10 = 0x00000043,
        F11 = 0x00000044,
        F12 = 0x00000045,

        /// <summary>
        /// Insert key.
        /// </summary>
        Insert = 0x00000049,
        /// <summary>
        /// Pos1 key.
        /// </summary>
        Pos1 = 0x0000004A,
        /// <summary>
        /// Page up key.
        /// </summary>
        PageUp = 0x0000004B,
        /// <summary>
        /// Del key.
        /// </summary>
        Del = 0x0000004C,
        /// <summary>
        /// End key.
        /// </summary>
        End = 0x0000004D,
        /// <summary>
        /// Page down key.
        /// </summary>
        PageDown = 0x0000004E,

        /// <summary>
        /// Right arrow key.
        /// </summary>
        Right = 0x0000004F,
        /// <summary>
        /// Left arrow key.
        /// </summary>
        Left = 0x00000050,
        /// <summary>
        /// Down arrow key.
        /// </summary>
        Down = 0x00000051,
        /// <summary>
        /// Up arrow key.
        /// </summary>
        Up = 0x00000052,

        /// <summary>
        /// Angle bracket (<) key.
        /// </summary>
        AngleBracket = 0x00000064,
    }
}
