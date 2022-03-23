var client = new HttpClient();

client.DefaultRequestHeaders.Authorization =
	new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",
	"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5" +
	"4bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjo" +
	"iZ2ZoZ2ZoZmciLCJleHAiOjE2Nzk2ODAwODksImlzcyI6IkNoYXRTZXJ2ZXI" +
	"ifQ.zqpl4mDa2-oGstGdsD-C4PiERZ5OMpp3Yqg0rsTUICA");

string s;
try
{
	s = await client.GetStringAsync("https://localhost:7121/private");
}
catch (Exception ex)
{
	s = ex.Message;
}

Console.WriteLine(s);
