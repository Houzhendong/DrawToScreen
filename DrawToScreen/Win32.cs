using System;
using System.Runtime.InteropServices;

namespace DrawToScreen
{
    public static class Win32
    {
        private delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumWindows(EnumWindowProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, IntPtr windowTitle);

        [DllImport("user32.dll")]
        private static extern IntPtr GetDCEx(IntPtr hWnd, IntPtr hrgnClip, uint flags);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessageTimeout(IntPtr windowHandle, uint Msg, IntPtr wParam, IntPtr lParam, uint flags, uint timeout, out IntPtr result);

        public static IntPtr Desktop()
        {
            var result = IntPtr.Zero;

            SendMessageTimeout(FindWindow("Progman", null), 0x052C, new IntPtr(0), IntPtr.Zero, 0x0, 1000, out result);

            var findWindowEx = IntPtr.Zero;

            EnumWindows((topHandel, topParamHandle) =>
            {
                var p = FindWindowEx(topHandel, IntPtr.Zero, "SHELLDLL_DefView", IntPtr.Zero);
                if (p != IntPtr.Zero)
                {
                    findWindowEx = FindWindowEx(IntPtr.Zero, topHandel, "WorkerW", IntPtr.Zero);
                }
                return true;
            }, IntPtr.Zero);

            var dc = GetDCEx(findWindowEx, IntPtr.Zero, 0x403);

            return findWindowEx;
        }
    }
}
