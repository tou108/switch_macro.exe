using System;
using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;
using BZComponent;

namespace NX_Macro_Controller_VxV;

internal static class Program
{
	[STAThread]
	private static void Main()
	{
		Thread.GetDomain().SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
		if (!((WindowsPrincipal)Thread.CurrentPrincipal).IsInRole(WindowsBuiltInRole.Administrator))
		{
			Process.Start(new ProcessStartInfo
			{
				WorkingDirectory = Environment.CurrentDirectory,
				FileName = Assembly.GetEntryAssembly().Location,
				Verb = "RunAs"
			});
			return;
		}
		if (GlobalVar.debugBuild)
		{
			BZStyle.SetDebugTheme();
		}
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(defaultValue: false);
		ToolStripManager.RenderMode = ToolStripManagerRenderMode.Professional;
		ToolStripManager.Renderer = new ToolStripProfessionalRenderer
		{
			RoundedEdges = false
		};
		ToolStripManager.VisualStylesEnabled = true;
		Application.Run(new NXMC_VxV());
	}
}
