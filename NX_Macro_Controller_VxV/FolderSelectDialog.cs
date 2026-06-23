using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NX_Macro_Controller_VxV;

public class FolderSelectDialog
{
	[ComImport]
	[Guid("DC1C5A9C-E88A-4dde-A5A1-60F82A20AEF7")]
	private class FileOpenDialogInternal
	{
	}

	[ComImport]
	[Guid("42f85136-db7e-439c-85f1-e4075d135fc8")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	private interface IFileOpenDialog
	{
		[PreserveSig]
		uint Show([In] IntPtr hwndParent);

		void SetFileTypes();

		void SetFileTypeIndex();

		void GetFileTypeIndex();

		void Advise();

		void Unadvise();

		void SetOptions([In] FOS fos);

		void GetOptions();

		void SetDefaultFolder();

		void SetFolder(IShellItem psi);

		void GetFolder();

		void GetCurrentSelection();

		void SetFileName();

		void GetFileName();

		void SetTitle([In][MarshalAs(UnmanagedType.LPWStr)] string pszTitle);

		void SetOkButtonLabel();

		void SetFileNameLabel();

		void GetResult(out IShellItem ppsi);

		void AddPlace();

		void SetDefaultExtension();

		void Close();

		void SetClientGuid();

		void ClearClientData();

		void SetFilter();

		void GetResults();

		void GetSelectedItems();
	}

	[ComImport]
	[Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	private interface IShellItem
	{
		void BindToHandler();

		void GetParent();

		void GetDisplayName([In] SIGDN sigdnName, [MarshalAs(UnmanagedType.LPWStr)] out string ppszName);

		void GetAttributes();

		void Compare();
	}

	private enum SIGDN : uint
	{
		SIGDN_FILESYSPATH = 2147844096u
	}

	[Flags]
	private enum FOS
	{
		FOS_FORCEFILESYSTEM = 0x40,
		FOS_PICKFOLDERS = 0x20
	}

	private class NativeMethods
	{
		public const uint ERROR_CANCELLED = 2147943623u;

		[DllImport("shell32.dll")]
		public static extern int SHILCreateFromPath([MarshalAs(UnmanagedType.LPWStr)] string pszPath, out IntPtr ppIdl, ref uint rgflnOut);

		[DllImport("shell32.dll")]
		public static extern int SHCreateShellItem(IntPtr pidlParent, IntPtr psfParent, IntPtr pidl, out IShellItem ppsi);
	}

	public string Path { get; set; }

	public string Title { get; set; }

	public DialogResult ShowDialog()
	{
		return ShowDialog(IntPtr.Zero);
	}

	public DialogResult ShowDialog(IWin32Window owner)
	{
		return ShowDialog(owner.Handle);
	}

	public DialogResult ShowDialog(IntPtr owner)
	{
		IFileOpenDialog fileOpenDialog = new FileOpenDialogInternal() as IFileOpenDialog;
		try
		{
			fileOpenDialog.SetOptions(FOS.FOS_FORCEFILESYSTEM | FOS.FOS_PICKFOLDERS);
			IShellItem ppsi;
			if (!string.IsNullOrEmpty(Path))
			{
				uint rgflnOut = 0u;
				if (NativeMethods.SHILCreateFromPath(Path, out var ppIdl, ref rgflnOut) == 0 && NativeMethods.SHCreateShellItem(IntPtr.Zero, IntPtr.Zero, ppIdl, out ppsi) == 0)
				{
					fileOpenDialog.SetFolder(ppsi);
				}
			}
			if (!string.IsNullOrEmpty(Title))
			{
				fileOpenDialog.SetTitle(Title);
			}
			uint num = fileOpenDialog.Show(owner);
			if (num.Equals(2147943623u))
			{
				return DialogResult.Cancel;
			}
			if (!num.Equals(0u))
			{
				return DialogResult.Abort;
			}
			fileOpenDialog.GetResult(out ppsi);
			ppsi.GetDisplayName(SIGDN.SIGDN_FILESYSPATH, out var ppszName);
			Path = ppszName;
			return DialogResult.OK;
		}
		finally
		{
			Marshal.FinalReleaseComObject(fileOpenDialog);
		}
	}
}
