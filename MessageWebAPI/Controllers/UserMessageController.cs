using Common;
using MessageWebAPI.Models;
using MessageWebAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace MessageWebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserMessageController : ControllerBase
    {
        ICoreRepository _coreRepository;

        public UserMessageController(ICoreRepository coreRepository)
        {
            _coreRepository = coreRepository;
        }

        [HttpGet]
        [RequiredScope("messages.read")]
        public async Task<IEnumerable<UserMessage>> GetAsync()
        {
            List<Message> messages = await _coreRepository.GetMessagesAsync();

            List<UserMessage> result = new List<UserMessage>();
            foreach (var message in messages) 
            {
                result.Add(new UserMessage 
                {
                    Content = message.Content,
                    SubmittedBy = message.SubmittedBy,
                    SubmittedAt = message.SubmittedAt,
                    IsDeleted = message.IsDeleted,
                });
            }

            return result;
        }

        [HttpPost]
        [RequiredScope("messages.write")]
        public async Task<IEnumerable<UserMessage>> PostAsync(UserSubmitData postData)
        {
            await _coreRepository.InsertMessageAsync(new Message 
            {
                Content = postData.Message,
                SubmittedBy = User.Identity.Name
            });

            List<Message> messages = await _coreRepository.GetMessagesAsync();

            List<UserMessage> result = new List<UserMessage>();
            foreach (var message in messages)
            {
                result.Add(new UserMessage
                {
                    Content = message.Content,
                    SubmittedBy = message.SubmittedBy,
                    SubmittedAt = message.SubmittedAt,
                    IsDeleted = message.IsDeleted,
                });
            }

            return result;
        }
    }
}
