namespace SimpleImmichFrame.ImmichApi;

public partial class ImmichApi
{
	public ImmichApi(string url, HttpClient httpClient)
	{
		if (!url.EndsWith('/'))
		{
			url += "/api/";
		}

		_baseUrl = url + _baseUrl;
		_httpClient = httpClient;
		_settings = new Lazy<Newtonsoft.Json.JsonSerializerSettings>(CreateSerializerSettings);
	}
}
