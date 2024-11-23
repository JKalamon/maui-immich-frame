namespace SimpleImmichFrame.Models;

public record DisplayPhoto(byte[] PhotoData, string CreatedDate, double? Iso, double? Aperture, string? ShutterSpeed, double? FocalLength) : IDisposable
{
	public ImageSource ImageSource => ImageSource.FromStream(() => new MemoryStream(PhotoData));

	//public ImageSource ImageSource => new UriImageSource
	//{
	//	Uri = new Uri("https://aka.ms/campus.jpg"),
	//	CacheValidity = new TimeSpan(10, 0, 0, 0)
	//};

	public void Dispose()
	{
	}
}
