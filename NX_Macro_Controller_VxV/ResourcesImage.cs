using System.Drawing;

namespace NX_Macro_Controller_VxV;

public class ResourcesImage
{
	public Image image;

	public string label;

	public ResourcesImage(Image im, string st)
	{
		image = (Image)im.Clone();
		label = st;
	}
}
