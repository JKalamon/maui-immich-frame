using System.Data;

namespace SimpleImmichFrame.ImmichApi
{
	internal class ImmichClient
	{
		public ImmichClient(HttpClient client)
		{
		}

		private Task<bool> LoadRandomAssets()
		{
			return Task.FromResult(true);
			//using (var client = new HttpClient())
			//{
			//	try
			//	{
			//		var searchBody = new RandomSearchDto
			//		{
			//			Size = 250,
			//			Type = AssetTypeEnum.IMAGE,
			//			WithExif = true,
			//			WithPeople = true
			//		};
			//		var searchResponse = await immichApi.SearchRandomAsync(searchBody);

			//		var randomAssets = searchResponse;

			//		if (randomAssets.Any())
			//		{
			//			var excludedList = await ExcludedAlbumAssets;

			//			randomAssets = randomAssets.Where(x => !excludedList.Contains(Guid.Parse(x.Id))).ToList();

			//			RandomAssetList.AddRange(randomAssets);

			//			return true;
			//		}
			//	}
			//	catch (ApiException ex)
			//	{
			//		throw new PersonNotFoundException($"Asset was not found, check your settings file!{Environment.NewLine}{Environment.NewLine}{ex.Message}", ex);
			//	}

			//	return false;
			//}
		}
	}
}
