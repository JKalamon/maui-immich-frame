﻿using SimpleImmichFrame.Exceptions;
using SimpleImmichFrame.ImmichApi;
using SimpleImmichFrame.Models;

namespace SimpleImmichFrame.Services
{
	public class PhotoManager(ISettingsService settingsService, ImmichClient client)
	{
		private readonly int maxCachedPhotos = 2;

		private List<AssetResponseDto>? imagesMetadata;

		private List<DisplayPhoto> nextCachedPhotos = [];

		private DisplayPhoto? currentImage = null;

		public async Task<DisplayPhoto> GetNextPhoto()
		{
			if (imagesMetadata?.Any() != true)
				imagesMetadata = (await client.LoadRandomAssets()).DistinctBy(x => x.Id).ToList();

			if (imagesMetadata is null || !imagesMetadata.Any())
			{
				throw new SupportedException("No images found");
			}

			var culture = new System.Globalization.CultureInfo(settingsService.Settings.Culture);

			while (nextCachedPhotos.Count < maxCachedPhotos && imagesMetadata.Any())
			{
				var metadata = imagesMetadata.First();
				using var memoryStream = new MemoryStream();
				using var imageFile = await client.GetImage(Guid.Parse(metadata.Id));
				await imageFile.Stream.CopyToAsync(memoryStream);


				var date = metadata.FileCreatedAt.UtcDateTime.ToString(settingsService.Settings.DateFormat, culture);
				nextCachedPhotos.Add(new DisplayPhoto(memoryStream.ToArray(), date, metadata.ExifInfo.Iso, metadata.ExifInfo.FNumber, metadata.ExifInfo.ExposureTime, metadata.ExifInfo.FocalLength));
				imagesMetadata.RemoveAt(0);
			}

			if (currentImage != null)
				currentImage.Dispose();

			currentImage = nextCachedPhotos.First();
			nextCachedPhotos.RemoveAt(0);
			return currentImage;
		}
	}
}