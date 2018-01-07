using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

namespace MVP.DLL.Wrappers
{
    internal class User32
    {
        public delegate int HookProc(int nCode, int wParam, IntPtr lParam);

        public static Rectangle GetClientRectangle(IntPtr handle)
        {
            GetClientRect(handle, out var rect);
            ClientToScreen(handle, out var point);
            return rect.ToRectangle(point);
        }

        [DllImport("user32.dll")]
        private static extern bool GetClientRect(IntPtr hWnd, out Rect lpRect);

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, uint dwFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, int dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern int UnhookWindowsHookEx(int idHook);

        [DllImport("user32.dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, out Point lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        public struct Margins
        {
            private int left, right, top, bottom;

            public static Margins FromRectangle(Rectangle rectangle)
            {
                var margins = new Margins
                {
                    left = rectangle.Left,
                    right = rectangle.Right,
                    top = rectangle.Top,
                    bottom = rectangle.Bottom
                };
                return margins;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            private readonly int left, top, right, bottom;

            public Rectangle ToRectangle(Point point)
            {
                return new Rectangle(point.X, point.Y, right - left, bottom - top);
            }
        }
    }
}
