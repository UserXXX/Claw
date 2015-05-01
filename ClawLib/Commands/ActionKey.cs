using System;

namespace Claw.Commands
{
    /// <summary>
    /// Enumeration for keys on mouse and keyboard.
    /// </summary>
    public enum ActionKey : uint
    {
        /// <summary>
        /// A key.
        /// </summary>
        A = 0x00000004,
        /// <summary>
        /// B key.
        /// </summary>
        B = 0x00000005,
        /// <summary>
        /// C key.
        /// </summary>
        C = 0x00000006,
        /// <summary>
        /// D key.
        /// </summary>
        D = 0x00000007,
        /// <summary>
        /// E key.
        /// </summary>
        E = 0x00000008,
        /// <summary>
        /// F key.
        /// </summary>
        F = 0x00000009,
        /// <summary>
        /// G key.
        /// </summary>
        G = 0x0000000A,
        /// <summary>
        /// H key.
        /// </summary>
        H = 0x0000000B,
        /// <summary>
        /// I key.
        /// </summary>
        I = 0x0000000C,
        /// <summary>
        /// J key.
        /// </summary>
        J = 0x0000000D,
        /// <summary>
        /// K key.
        /// </summary>
        K = 0x0000000E,
        /// <summary>
        /// L key.
        /// </summary>
        L = 0x0000000F,
        /// <summary>
        /// M key.
        /// </summary>
        M = 0x00000010,
        /// <summary>
        /// N key.
        /// </summary>
        N = 0x00000011,
        /// <summary>
        /// O key.
        /// </summary>
        O = 0x00000012,
        /// <summary>
        /// P key.
        /// </summary>
        P = 0x00000013,
        /// <summary>
        /// Q key.
        /// </summary>
        Q = 0x00000014,
        /// <summary>
        /// R key.
        /// </summary>
        R = 0x00000015,
        /// <summary>
        /// S key.
        /// </summary>
        S = 0x00000016,
        /// <summary>
        /// T key.
        /// </summary>
        T = 0x00000017,
        /// <summary>
        /// U key.
        /// </summary>
        U = 0x00000018,
        /// <summary>
        /// V key.
        /// </summary>
        V = 0x00000019,
        /// <summary>
        /// W key.
        /// </summary>
        W = 0x0000001A,
        /// <summary>
        /// X key.
        /// </summary>
        X = 0x0000001B,
        /// <summary>
        /// Y key.
        /// </summary>
        Y = 0x0000001D,
        /// <summary>
        /// Z key.
        /// </summary>
        Z = 0x0000001C,

        /// <summary>
        /// One (1) key.
        /// </summary>
        One = 0x0000001E,
        /// <summary>
        /// Two (2) key.
        /// </summary>
        Two = 0x0000001F,
        /// <summary>
        /// Three (3) key.
        /// </summary>
        Three = 0x00000020,
        /// <summary>
        /// Four (4) key.
        /// </summary>
        Four = 0x00000021,
        /// <summary>
        /// Five (5) key.
        /// </summary>
        Five = 0x00000022,
        /// <summary>
        /// Six (6) key.
        /// </summary>
        Six = 0x00000023,
        /// <summary>
        /// Seven (7) key.
        /// </summary>
        Seven = 0x00000024,
        /// <summary>
        /// Eight (8) key.
        /// </summary>
        Eight = 0x00000025,
        /// <summary>
        /// Nine (9) key.
        /// </summary>
        Nine = 0x00000026,
        /// <summary>
        /// Zero (0) key.
        /// </summary>
        Zero = 0x00000027,
        /// <summary>
        /// Enter key.
        /// </summary>
        Enter = 0x00000028,
        /// <summary>
        /// Escape key.
        /// </summary>
        Escape = 0x00000029,
        /// <summary>
        /// Backspace key.
        /// </summary>
        Backspace = 0x0000002A,
        /// <summary>
        /// Tabulator key.
        /// </summary>
        Tab = 0x0000002B,
        /// <summary>
        /// Spacebar.
        /// </summary>
        Space = 0x0000002C,
        /// <summary>
        /// German sharp s (ß) key.
        /// </summary>
        SharpS = 0x0000002D,
        /// <summary>
        /// Acute (´) key.
        /// </summary>
        Acute = 0x0000002E,
        /// <summary>
        /// German ü key.
        /// </summary>
        Ue = 0x0000002F,
        /// <summary>
        /// Plus (+) key on the main keyboard.
        /// </summary>
        Plus = 0x00000030,
        /// <summary>
        /// Sharp (#) key on the keyboard, also referred as "hashtag".
        /// </summary>
        Sharp = 0x00000031,

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
        /// <summary>
        /// Capslock key.
        /// </summary>
        Capslock = 0x00000039,
        /// <summary>
        /// Function 1 key.
        /// </summary>
        F1 = 0x0000003A,
        /// <summary>
        /// Function 2 key.
        /// </summary>
        F2 = 0x0000003B,
        /// <summary>
        /// Function 3 key.
        /// </summary>
        F3 = 0x0000003C,
        /// <summary>
        /// Function 4 key.
        /// </summary>
        F4 = 0x0000003D,
        /// <summary>
        /// Function 5 key.
        /// </summary>
        F5 = 0x0000003E,
        /// <summary>
        /// Function 6 key.
        /// </summary>
        F6 = 0x0000003F,
        /// <summary>
        /// Function 7 key.
        /// </summary>
        F7 = 0x00000040,
        /// <summary>
        /// Function 8 key.
        /// </summary>
        F8 = 0x00000041,
        /// <summary>
        /// Function 9 key.
        /// </summary>
        F9 = 0x00000042,
        /// <summary>
        /// Function 10 key.
        /// </summary>
        F10 = 0x00000043,
        /// <summary>
        /// Function 11 key.
        /// </summary>
        F11 = 0x00000044,
        /// <summary>
        /// Function 12 key.
        /// </summary>
        F12 = 0x00000045,
        /// <summary>
        /// Print key.
        /// </summary>
        Print = 0x00000046,
        /// <summary>
        /// Roll key.
        /// </summary>
        Roll = 0x00000047,
        /// <summary>
        /// Pause key.
        /// </summary>
        Pause = 0x00000048,
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
        /// Num pad toggle key.
        /// </summary>
        Num = 0x00000053,
        /// <summary>
        /// Num pad divide (/) key.
        /// </summary>
        NumDivide = 0x00000054,
        /// <summary>
        /// Num pad multiply (*) key.
        /// </summary>
        NumMultiply = 0x00000055,
        /// <summary>
        /// Num pad subtract (-) key.
        /// </summary>
        NumSubtract = 0x00000056,
        /// <summary>
        /// Num pad add (+) key.
        /// </summary>
        NumAdd = 0x00000057,
        /// <summary>
        /// Num pad enter key.
        /// </summary>
        NumEnter = 0x00000058,
        /// <summary>
        /// Num pad one (1) key.
        /// </summary>
        NumOne = 0x00000059,
        /// <summary>
        /// Num pad two (2) key.
        /// </summary>
        NumTwo = 0x0000005A,
        /// <summary>
        /// Num pad three (3) key.
        /// </summary>
        NumThree = 0x0000005B,
        /// <summary>
        /// Num pad four (4) key.
        /// </summary>
        NumFour = 0x0000005C,
        /// <summary>
        /// Num pad five (5) key.
        /// </summary>
        NumFive = 0x0000005D,
        /// <summary>
        /// Num pad six (6) key.
        /// </summary>
        NumSix = 0x0000005E,
        /// <summary>
        /// Num pad seven (7) key.
        /// </summary>
        NumSeven = 0x0000005F,
        /// <summary>
        /// Num pad eight (8) key.
        /// </summary>
        NumEight = 0x00000060,
        /// <summary>
        /// Num pad nine (9) key.
        /// </summary>
        NumNine = 0x00000061,
        /// <summary>
        /// Num pad zero (0) key.
        /// </summary>
        NumZero = 0x00000062,
        /// <summary>
        /// Num pad del / period key.
        /// </summary>
        NumDel = 0x00000063,
        /// <summary>
        /// Angle bracket (<) key.
        /// </summary>
        AngleBracket = 0x00000064,
        /// <summary>
        /// Application key.
        /// </summary>
        Application = 0x00000065,

        /// <summary>
        /// Left control key.
        /// </summary>
        LeftCtrl = 0x000000E0,
        /// <summary>
        /// Left shift key.
        /// </summary>
        LeftShift = 0x000000E1,
        /// <summary>
        /// Alt key (on the left side).
        /// </summary>
        Alt = 0x000000E2,
        /// <summary>
        /// Left windows key.
        /// </summary>
        LeftWindows = 0x000000E3,
        /// <summary>
        /// Right control key.
        /// </summary>
        RightCtrl = 0x000000E4,
        /// <summary>
        /// Right shift key.
        /// </summary>
        RightShift = 0x000000E5,
        /// <summary>
        /// Big alt key (missing on American keyboard).
        /// </summary>
        AltGr = 0x000000E6,
    }
}
