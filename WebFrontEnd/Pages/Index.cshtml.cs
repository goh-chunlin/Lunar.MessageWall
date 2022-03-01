using Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Web;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace WebFrontEnd.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ITokenAcquisition _tokenAcquisition;

        public IndexModel(ILogger<IndexModel> logger, ITokenAcquisition tokenAcquisition)
        {
            _logger = logger;
            _tokenAcquisition = tokenAcquisition;
        }

        public async Task OnGet()
        {
            if ((bool)(User.Identity?.IsAuthenticated))
            {
                using (var client = new HttpClient())
                {

                    string accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(new[] { "https://lunarchunlin.onmicrosoft.com/message-api/messages.read" });
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var request = new HttpRequestMessage();
                    request.RequestUri = new Uri("http://messagewebapi/UserMessage");
                    var response = await client.SendAsync(request);
                    string apiOutput = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        List<UserMessage> userMessages = JsonSerializer.Deserialize<List<UserMessage>>(apiOutput)!;

                        ViewData["Messages"] = userMessages;
                    }

                }
            }
            
        }

        public async Task OnPost()
        {
            var message = Request.Form["MessageContent"];

            if ((bool)(User.Identity?.IsAuthenticated))
            {
                using (var client = new HttpClient())
                {

                    string accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(new[] { "https://lunarchunlin.onmicrosoft.com/message-api/messages.write" });
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    UserSubmitData data = new UserSubmitData 
                    {
                        Message = message,
                    };
                    string jsonData = JsonSerializer.Serialize(data);

                    StringContent postData = new StringContent(jsonData, UnicodeEncoding.UTF8, MediaTypeNames.Application.Json);

                    var request = new HttpRequestMessage();
                    var response = await client.PostAsync("http://messagewebapi/UserMessage", postData);
                    string apiOutput = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        List<UserMessage> userMessages = JsonSerializer.Deserialize<List<UserMessage>>(apiOutput)!;

                        ViewData["Messages"] = userMessages;
                    }

                }
            }
        }
    }
}