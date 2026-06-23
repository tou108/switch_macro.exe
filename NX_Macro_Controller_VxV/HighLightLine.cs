using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using BZComponent;
using ICSharpCode.AvalonEdit.Rendering;

namespace NX_Macro_Controller_VxV;

public class HighLightLine : IBackgroundRenderer
{
	private static System.Windows.Media.Pen pen;

	private static System.Windows.Media.Brush highLight;

	private static System.Windows.Media.Brush highLight2;

	private static int highLightLine = -1;

	public KnownLayer Layer => KnownLayer.Background;

	public static void HighLightLineSet(int lineNum)
	{
		highLightLine = lineNum;
	}

	public static void HighLightSet(System.Drawing.Color highLight, System.Drawing.Color highLight2)
	{
		HighLightLine.highLight = highLight.ToBrush();
		HighLightLine.highLight2 = highLight2.ToBrush();
		pen = new System.Windows.Media.Pen(BZStyle.NormalColor.ToBrush(), 0.0);
	}

	public HighLightLine(System.Drawing.Color highLight, System.Drawing.Color highLight2)
	{
		HighLightLine.highLight = highLight.ToBrush();
		HighLightLine.highLight2 = highLight2.ToBrush();
		pen = new System.Windows.Media.Pen(BZStyle.NormalColor.ToBrush(), 0.0);
	}

	public void Draw(TextView textView, DrawingContext drawingContext)
	{
		foreach (VisualLine visualLine in textView.VisualLines)
		{
			Rect rect = BackgroundGeometryBuilder.GetRectsFromVisualSegment(textView, visualLine, 0, 1000).First();
			int num = visualLine.FirstDocumentLine.LineNumber - 1;
			System.Windows.Media.Brush brush = null;
			System.Windows.Media.Brush brush2 = null;
			if (num == highLightLine)
			{
				brush = highLight;
				brush2 = highLight2;
			}
			drawingContext.DrawRectangle(brush2, pen, new Rect(0.0, rect.Top, textView.ActualWidth, rect.Height));
			drawingContext.DrawRectangle(brush, pen, new Rect(1.0, rect.Top + 1.0, textView.ActualWidth - 2.0, rect.Height - 2.0));
		}
	}
}
