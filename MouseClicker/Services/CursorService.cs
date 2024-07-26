using System.Runtime.InteropServices;

namespace MouseClicker.Services;

public class CursorService
{
    private const int MouseEventLiftDown = 0x02;
    private const int MouseEventLiftUp = 0x04;

    private struct CursorPoint
    {
        public int X;
        public int Y;
    }

    [DllImport("user32.dll")]
    private static extern bool GetCursorPos(out CursorPoint lpPoint);

    [DllImport("user32.dll",
        EntryPoint = "mouse_event",
        CharSet = CharSet.Auto,
        CallingConvention = CallingConvention.StdCall
    )]
    private static extern void MouseEvent(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

    public static void MouseClick()
    {
        GetCursorPos(out var cursorPos);
        var x = (uint)cursorPos.X;
        var y = (uint)cursorPos.Y;

        MouseEvent(MouseEventLiftDown | MouseEventLiftUp, x, y, 0, 0);
    }
}