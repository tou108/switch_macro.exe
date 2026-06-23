using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace HongliangSoft.Utilities;

[DefaultEvent("MouseHooked")]
public class MouseHook : Component
{
	private delegate int MouseHookDelegate(int code, MouseMessage message, ref MouseState state);

	private IntPtr module = LoadLibrary("user32.dll");

	private const int MouseLowLevelHook = 14;

	private IntPtr hook;

	private GCHandle hookDelegate;

	private static readonly object EventMouseHooked = new object();

	private bool disposed;

	public event MouseHookedEventHandler MouseHooked
	{
		add
		{
			base.Events.AddHandler(EventMouseHooked, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventMouseHooked, value);
		}
	}

	[DllImport("user32.dll", SetLastError = true)]
	private static extern IntPtr SetWindowsHookEx(int hookType, MouseHookDelegate hookDelegate, IntPtr hInstance, uint threadId);

	[DllImport("user32.dll", SetLastError = true)]
	private static extern int CallNextHookEx(IntPtr hook, int code, MouseMessage message, ref MouseState state);

	[DllImport("user32.dll", SetLastError = true)]
	private static extern bool UnhookWindowsHookEx(IntPtr hook);

	[DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
	internal static extern IntPtr LoadLibrary(string lpFileName);

	public MouseHook()
	{
		if (Environment.OSVersion.Platform != PlatformID.Win32NT)
		{
			throw new PlatformNotSupportedException("Windows 98/Meではサポートされていません。");
		}
		MouseHookDelegate value = CallNextHook;
		hookDelegate = GCHandle.Alloc(value);
		hook = SetWindowsHookEx(14, value, module, 0u);
		if (hook == IntPtr.Zero)
		{
			throw new Win32Exception(Marshal.GetLastWin32Error());
		}
	}

	public MouseHook(MouseHookedEventHandler handler)
		: this()
	{
		MouseHooked += handler;
	}

	protected virtual void OnMouseHooked(MouseHookedEventArgs e)
	{
		if (base.Events[EventMouseHooked] is MouseHookedEventHandler mouseHookedEventHandler)
		{
			mouseHookedEventHandler(this, e);
		}
	}

	private int CallNextHook(int code, MouseMessage message, ref MouseState state)
	{
		if (code >= 0)
		{
			OnMouseHooked(new MouseHookedEventArgs(message, ref state));
		}
		return CallNextHookEx(hook, code, message, ref state);
	}

	protected override void Dispose(bool disposing)
	{
		if (!disposed)
		{
			disposed = true;
			UnhookWindowsHookEx(hook);
			hook = IntPtr.Zero;
			base.Dispose(disposing);
		}
	}
}
