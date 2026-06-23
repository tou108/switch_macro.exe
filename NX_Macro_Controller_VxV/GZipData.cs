using System.IO;
using System.IO.Compression;

namespace NX_Macro_Controller_VxV;

internal static class GZipData
{
	public static byte[] GZipDecompress(byte[] bytes)
	{
		byte[] array = new byte[1024];
		using MemoryStream memoryStream = new MemoryStream();
		using (GZipStream gZipStream = new GZipStream(new MemoryStream(bytes), CompressionMode.Decompress))
		{
			while (true)
			{
				int num = gZipStream.Read(array, 0, array.Length);
				if (num == 0)
				{
					break;
				}
				memoryStream.Write(array, 0, num);
			}
		}
		return memoryStream.ToArray();
	}

	public static byte[] GZipCompress(byte[] bytes)
	{
		using MemoryStream memoryStream = new MemoryStream();
		using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionLevel.Fastest))
		{
			gZipStream.Write(bytes, 0, bytes.Length);
		}
		return memoryStream.ToArray();
	}
}
