using Microsoft.Extensions.Logging; 
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI; 
using System; 
using System.Threading.Tasks; 
using System.Text;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ConsoleApp1;
using ConsoleApp1.Plugins;



public class Program
{
    private const string azureOpenAiEndpoint = "";

    private const string azureOpenAiKey =
        "";
    private const string azureOpenAiDeployement = "";
    static async Task Main()
    {
        // Create a kernel with Azure OpenAI chat completion
        var builder = Kernel.CreateBuilder().AddAzureOpenAIChatCompletion(azureOpenAiDeployement, azureOpenAiEndpoint, azureOpenAiKey);
		
        // Add enterprise components
        //builder.Services.AddLogging(services => services.AddConsole().SetMinimumLevel(LogLevel.Trace));
        
        // Registering the function invocation filter
        builder.Services.AddSingleton<IAutoFunctionInvocationFilter, AutoInvocationFilter>();
        
        // Adding the time and file plugin
        builder.Plugins.AddFromType<TimePlugin>();
        builder.Plugins.AddFromType<FilePlugin>();
        
        Kernel kernel = builder.Build();
		var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>(); // используем чат сервис

        // simple chat program
        
        /*Console.Write("Введите вопрос: ");
        string input = Console.ReadLine() ?? "";
        var result = await kernel.InvokePromptAsync(input);
        
        string answer = result.GetValue<string>();
        Console.WriteLine("Ответ от модели: " + answer);*/

        OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        }; // настройки для выполнения промпта (запроса) в модели OpenAI.



		var history = new ChatHistory();
        history.AddSystemMessage("Ты — дружелюбный помощник. Всегда отвечай на русском языке и помогай с программированием.");
		// prompt engineering
		Console.OutputEncoding = Encoding.UTF8;
		// Initiate a back-and-forth chat
        string? userInput;
        do {
            // Collect user input
            Console.Write("User > ");
            userInput = Console.ReadLine();
        
            // Add user input
            history.AddUserMessage(userInput);
        
            // Get the response from the AI
            var result = await chatCompletionService.GetChatMessageContentAsync(
                history,
                executionSettings: openAIPromptExecutionSettings,
                kernel: kernel);
        
            // Print the results
            Console.WriteLine("Assistant > " + result);
        
            // Add the message from the agent to the chat history
            history.AddMessage(result.Role, result.Content ?? string.Empty);
        } while (userInput is not null);
		
    }
}


