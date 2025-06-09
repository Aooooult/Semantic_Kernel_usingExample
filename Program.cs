using Microsoft.Extensions.Logging; // добавляем логи (для отслеживания действий программы) (необязательно)
using Microsoft.SemanticKernel; // для подключения SemanticKernel
using Microsoft.SemanticKernel.Connectors.OpenAI; // подсоединение OpenAI
using System; // базовые функции
using System.Threading.Tasks; // asyc await
using System.Text; // для Encoding
using Microsoft.SemanticKernel.ChatCompletion; // интерфейсы для чата (IChatCompletionService, ChatHistory)
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ConsoleApp1;
using ConsoleApp1.Plugins;



public class Program
{
    private const string azureOpenAiEndpoint = "https://anast-mbjamgb5-eastus2.openai.azure.com";

    private const string azureOpenAiKey =
        "CBM6iGtmzTBuEVuVAYLVyQANFinakZ3NgnmUQ81DVRRxALNhg5HhJQQJ99BFACHYHv6XJ3w3AAAAACOGqOpS";
    private const string azureOpenAiDeployement = "gpt-4o";
    static async Task Main()
    {
        // Create a kernel with Azure OpenAI chat completion
        var builder = Kernel.CreateBuilder().AddAzureOpenAIChatCompletion(azureOpenAiDeployement, azureOpenAiEndpoint, azureOpenAiKey);
		
        // Add enterprise components
        //builder.Services.AddLogging(services => services.AddConsole().SetMinimumLevel(LogLevel.Trace));
        
        // Регистрируем фильтр вызова функций
        builder.Services.AddSingleton<IAutoFunctionInvocationFilter, AutoInvocationFilter>();
        
        //добавляем плагин на время
        builder.Plugins.AddFromType<TimePlugin>();
        builder.Plugins.AddFromType<FilePlugin>();
        
        Kernel kernel = builder.Build();
		var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>(); // используем чат сервис

        
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
        history.AddSystemMessage("Ты — дружелюбный помощник 🤖. Всегда отвечай на русском языке и помогай с программированием.");
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


