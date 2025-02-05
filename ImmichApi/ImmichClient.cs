using System.Diagnostics.CodeAnalysis;

namespace SimpleImmichFrame.ImmichApi
{
	public class ImmichClient : IDisposable
	{
		private readonly ISettingsService settings;
		private HttpClient client;
		private ImmichApi immichApi;

		public ImmichClient(ISettingsService settings)
		{
			this.settings = settings;
			this.SetupHttpClient();

			this.settings.SettingsChanged += SettingsChanged;
		}

		public async Task<IEnumerable<AssetResponseDto>> LoadRandomAssets()
		{
			var searchBody = new RandomSearchDto
			{
				Size = 250,
				Type = AssetTypeEnum.IMAGE,
				WithExif = true,
				WithPeople = true
			};

			return await this.immichApi.SearchRandomAsync(searchBody);
		}

		public async Task<FileResponse> GetImage(Guid id) => await immichApi.ViewAssetAsync(id, string.Empty, AssetMediaSize.Preview);

		public async Task<FileResponse> GetImageThumbnail(Guid id) => await immichApi.ViewAssetAsync(id, string.Empty, AssetMediaSize.Thumbnail);

		public async Task<AlbumResponseDto> GetAlbumPhotos(Guid id) => await immichApi.GetAlbumInfoAsync(id, string.Empty, false);

		public async Task DeleteImage(Guid id) => await immichApi.DeleteAssetsAsync(new() { Ids = [id] });

		public void Dispose()
		{
			this.settings.SettingsChanged -= SettingsChanged;
			this.client.Dispose();
		}
		private void SettingsChanged(object? sender, AppConfiguration args)
		{
			this.client?.Dispose();
			this.SetupHttpClient();
		}

		[MemberNotNull(nameof(client))]
		[MemberNotNull(nameof(immichApi))]
		private void SetupHttpClient()
		{
			this.client = new HttpClient();
			client.DefaultRequestHeaders.Add("X-API-KEY", this.settings.Settings.ApiKey);
			this.immichApi = new ImmichApi(this.settings.Settings.ImmichServerUrl, this.client);
		}
	}
}
