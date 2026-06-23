using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using DirectShowLib;
using NxInterface;

namespace NX_Macro_Controller_VxV;

public class DSHDMICapture : IDisposable, ISampleGrabberCB
{
	private const int WMGraphNotify = 1037;

	private int pWidth;

	private int pHeight;

	private int hr;

	private int px;

	private int py;

	public int Width;

	public int Height;

	private Size _renderingSize;

	private IGraphBuilder graphBuilder;

	private IMediaEventEx mediaEventEx;

	private IMediaControl mediaControl;

	private IVideoWindow videoWindow;

	private IBasicVideo basicVideo;

	private VideoInfoHeader _videoInfoHeader;

	private ISampleGrabber sampleGrabber;

	private ICaptureGraphBuilder2 captureGraphBuilder;

	public bool selectmode;

	public int x1 = -1;

	public int x2;

	public int y1;

	public int y2;

	private byte[] imgbuf;

	private bool updateF;

	public Size sourceSize => new Size(pWidth, pHeight);

	public Point SourcePoint => new Point(px, py);

	public Size renderingSize
	{
		get
		{
			return _renderingSize;
		}
		set
		{
			_renderingSize = value;
			Rectangle rectangle = default(Rectangle);
			if ((double)renderingSize.Width / (double)Width < (double)renderingSize.Height / (double)Height)
			{
				rectangle.X = 0;
				rectangle.Width = value.Width;
				rectangle.Height = (int)((double)Height * ((double)value.Width / (double)Width));
				rectangle.Y = (value.Height - rectangle.Height) / 2;
			}
			else
			{
				rectangle.Y = 0;
				rectangle.Height = value.Height;
				rectangle.Width = (int)((double)Width * ((double)value.Height / (double)Height));
				rectangle.X = (value.Width - rectangle.Width) / 2;
			}
			px = rectangle.X;
			py = rectangle.Y;
			(graphBuilder as IVideoWindow).put_Width(rectangle.Width);
			(graphBuilder as IVideoWindow).put_Height(rectangle.Height);
			(graphBuilder as IVideoWindow).put_Left(rectangle.X);
			(graphBuilder as IVideoWindow).put_Top(rectangle.Y);
		}
	}

	private static IBaseFilter GetCaptureDevice(int deviceIndex)
	{
		DsDevice[] devicesOfCat = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
		if (devicesOfCat.Length <= deviceIndex)
		{
			return null;
		}
		DsDevice obj = devicesOfCat[deviceIndex];
		Guid riidResult = typeof(IBaseFilter).GUID;
		obj.Mon.BindToObject(null, null, ref riidResult, out var ppvResult);
		return (IBaseFilter)ppvResult;
	}

	void IDisposable.Dispose()
	{
		if (graphBuilder != null)
		{
			Marshal.ReleaseComObject(graphBuilder);
		}
		if (captureGraphBuilder != null)
		{
			Marshal.ReleaseComObject(captureGraphBuilder);
		}
		if (sampleGrabber != null)
		{
			Marshal.ReleaseComObject(sampleGrabber);
		}
	}

	int ISampleGrabberCB.SampleCB(double SampleTime, IMediaSample pSample)
	{
		return 0;
	}

	unsafe int ISampleGrabberCB.BufferCB(double SampleTime, IntPtr pBuffer, int BufferLen)
	{
		if (_videoInfoHeader == null)
		{
			return 0;
		}
		updateF = true;
		Bitmap bitmap = new Bitmap(_videoInfoHeader.BmiHeader.Width, _videoInfoHeader.BmiHeader.Height, _videoInfoHeader.BmiHeader.Width * _videoInfoHeader.BmiHeader.BitCount / 8, PixelFormat.Format24bppRgb, pBuffer);
		bitmap.RotateFlip(RotateFlipType.Rotate180FlipX);
		NxCommand.CurrentFrame = bitmap;
		if (GlobalVar.MAINFORM._captureNow)
		{
			int num = 0;
			int num2 = 3;
			byte* ptr = (byte*)pBuffer.ToPointer();
			int num3 = Math.Min(GlobalVar.MAINFORM.NxSel.X1, GlobalVar.MAINFORM.NxSel.X2);
			int num4 = Math.Max(GlobalVar.MAINFORM.NxSel.X1, GlobalVar.MAINFORM.NxSel.X2);
			int num5 = Math.Min(GlobalVar.MAINFORM.NxSel.Y1, GlobalVar.MAINFORM.NxSel.Y2);
			int num6 = Math.Max(GlobalVar.MAINFORM.NxSel.Y1, GlobalVar.MAINFORM.NxSel.Y2);
			for (int i = 0; i < _videoInfoHeader.BmiHeader.Height; i++)
			{
				for (int j = 0; j < _videoInfoHeader.BmiHeader.Width; j++)
				{
					if (num5 == -1 || _videoInfoHeader.BmiHeader.Height - i <= num5 || _videoInfoHeader.BmiHeader.Height - i >= num6 || j <= num3 || j >= num4)
					{
						ptr[num + j * num2] = (byte)(ptr[num + j * num2] / 2);
						ptr[num + j * num2 + 1] = (byte)(ptr[num + j * num2 + 1] / 2);
						ptr[num + j * num2 + 2] = (byte)(ptr[num + j * num2 + 2] / 2);
					}
				}
				num += _videoInfoHeader.BmiHeader.Width * _videoInfoHeader.BmiHeader.BitCount / 8;
			}
		}
		updateF = false;
		return 0;
	}

	public DSHDMICapture(int deviceIndex, IntPtr Handle)
	{
		_renderingSize = new Size(320, 240);
		graphBuilder = (IGraphBuilder)new FilterGraph();
		captureGraphBuilder = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
		IBaseFilter captureDevice = GetCaptureDevice(deviceIndex);
		DsError.ThrowExceptionForHR(captureGraphBuilder.SetFiltergraph(graphBuilder));
		DsError.ThrowExceptionForHR(graphBuilder.AddFilter(captureDevice, "Video Capture"));
		sampleGrabber = (ISampleGrabber)new SampleGrabber();
		AMMediaType aMMediaType = new AMMediaType
		{
			majorType = MediaType.Video,
			subType = MediaSubType.RGB24,
			formatType = FormatType.VideoInfo
		};
		DsError.ThrowExceptionForHR(sampleGrabber.SetMediaType(aMMediaType));
		_videoInfoHeader = (VideoInfoHeader)Marshal.PtrToStructure(aMMediaType.formatPtr, typeof(VideoInfoHeader));
		DsUtils.FreeAMMediaType(aMMediaType);
		DsError.ThrowExceptionForHR(graphBuilder.AddFilter((IBaseFilter)sampleGrabber, "Sample Grabber"));
		DsError.ThrowExceptionForHR(captureGraphBuilder.RenderStream(PinCategory.Preview, MediaType.Video, captureDevice, (IBaseFilter)sampleGrabber, null));
		Marshal.ReleaseComObject(captureDevice);
		mediaEventEx = (IMediaEventEx)graphBuilder;
		mediaControl = (IMediaControl)graphBuilder;
		videoWindow = graphBuilder as IVideoWindow;
		basicVideo = graphBuilder as IBasicVideo;
		mediaEventEx.SetNotifyWindow(Handle, 1037, IntPtr.Zero);
		DsError.ThrowExceptionForHR(videoWindow.put_Owner(Handle));
		DsError.ThrowExceptionForHR(videoWindow.put_WindowStyle(WindowStyle.Child | WindowStyle.ClipSiblings | WindowStyle.ClipChildren));
		videoWindow.put_MessageDrain(Handle);
		renderingSize = _renderingSize;
		graphBuilder.EnumFilters(out var ppEnum);
		IBaseFilter[] array = new IBaseFilter[1];
		while (ppEnum.Next(array.Length, array, IntPtr.Zero) == 0)
		{
			array[0].QueryFilterInfo(out var _);
			array[0].GetClassID(out var _);
			array[0] = null;
		}
		AMMediaType aMMediaType2 = new AMMediaType();
		DsError.ThrowExceptionForHR(sampleGrabber.GetConnectedMediaType(aMMediaType2));
		_videoInfoHeader = (VideoInfoHeader)Marshal.PtrToStructure(aMMediaType2.formatPtr, typeof(VideoInfoHeader));
		DsUtils.FreeAMMediaType(aMMediaType2);
		Width = _videoInfoHeader.BmiHeader.Width;
		Height = _videoInfoHeader.BmiHeader.Height;
	}

	public void Play()
	{
		sampleGrabber.SetBufferSamples(BufferThem: true);
		sampleGrabber.SetOneShot(OneShot: false);
		sampleGrabber.SetCallback(this, 1);
		DsError.ThrowExceptionForHR(mediaControl.Run());
	}

	public void Stop()
	{
		videoWindow.put_Visible(OABool.False);
		int num = mediaControl.Stop();
		videoWindow.put_Visible(OABool.False);
		DsError.ThrowExceptionForHR(num);
	}

	public void Dispose()
	{
		Stop();
		mediaControl.Stop();
		if (graphBuilder != null)
		{
			Marshal.ReleaseComObject(graphBuilder);
			graphBuilder = null;
		}
		if (captureGraphBuilder != null)
		{
			Marshal.ReleaseComObject(captureGraphBuilder);
			captureGraphBuilder = null;
		}
		if (sampleGrabber != null)
		{
			Marshal.ReleaseComObject(sampleGrabber);
			sampleGrabber = null;
		}
	}
}
