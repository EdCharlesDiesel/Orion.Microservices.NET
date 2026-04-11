using Azure.AI.Projects;
using Azure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orion.DataAccess.Postgres.Entities.Common;

namespace Orion.API.Apps.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ChatControllerController : ControllerBase
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly AIProjectClient _projectClient;
        private readonly string _modelDeployment;

        public ChatController(IConfiguration configuration, string modelDeployment)
        {
            var endpoint = new Uri(configuration["PROJECT_ENDPOINT"]);
            _modelDeployment = configuration["MODEL_DEPLOYMENT_NAME"];
            _projectClient = new AIProjectClient(endpoint, new DefaultAzureCredential());
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] ChatRequest request)
        {
          try
          {
              throw new NotImplementedException();
              // var chatClient = _projectClient.GetChatCompletionsClient(_modelDeployment);
              //
              // var response = await chatClient.GetChatCompletionsAsync(
              //     new ChatCompletionsOptions
              //     {
              //         Messages =
              //         {
              //             new ChatMessage(ChatRole.System, "You are an assistant."),
              //             new ChatMessage(ChatRole.User, request.Message)
              //         }
              //     });
              //
              // var result = response.Value.Choices.FirstOrDefault()?.Message.Content;
              // return Ok(new { reply = result });
          }
          catch (Exception ex)
          {
              return StatusCode(500, $"Error: {ex.Message}");
          }
        }
    }
}