namespace SimpleImmichFrame.Models;

public record DisplayPhoto(byte[] PhotoData, string CreatedDate, double? Iso, double? Aperture, string? ShutterSpeed, double? FocalLength)
{
	public ImageSource ImageSource => ImageSource.FromStream(() => new MemoryStream(PhotoData));

	public bool DisplayAperture => this.Aperture > 0.5;

	public bool DisplayShutterSpeed => !string.IsNullOrWhiteSpace(this.ShutterSpeed);

	public bool DisplayFocalLength => this.FocalLength > 0;

	public bool DisplayIso => this.Iso > 0.5;
}
