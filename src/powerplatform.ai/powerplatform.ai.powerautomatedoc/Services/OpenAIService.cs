using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;
using System.Text.Json;

namespace powerplatform.ai.powerautomatedoc.Services
{
    public class OpenAIService
    {
        public OpenAIService()
        {
        }

        public async Task<ChatCompletion> RequestChatGptResponse(string content)
        {
            AzureKeyCredential credential = new AzureKeyCredential("");

            // Initialize the AzureOpenAIClient
            AzureOpenAIClient azureClient = new(new Uri("https://oai-playground-dev-swec-001.openai.azure.com/"), credential);

            // Initialize the ChatClient with the specified deployment name
            ChatClient chatClient = azureClient.GetChatClient("gpt-4.1");

            try
            {
                var messages = new List<ChatMessage>
                {
                    new SystemChatMessage(@"You are an AI assistant that helps witn Microsoft Power Automate documentation"),
                    new UserChatMessage(content),
                    new UserChatMessage("Return documentation of the flow in md format with the following headers - 'Flow Purpose', 'Trigger', 'Actions' and 'Connection used' in this order. Just return the documentation nothing before or after.")
                };


                // Create the chat completion request
                ChatCompletion completion = await chatClient.CompleteChatAsync(messages);

                // Print the response
                if (completion != null)
                {
                    Console.WriteLine(JsonSerializer.Serialize(completion, new JsonSerializerOptions() { WriteIndented = true }));

                    return completion;
                }
                else
                {
                    Console.WriteLine("No response received.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }
    }
}
