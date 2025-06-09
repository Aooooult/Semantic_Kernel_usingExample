using Microsoft.SemanticKernel;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace ConsoleApp1;

public class AutoInvocationFilter : IAutoFunctionInvocationFilter
    {
        public async Task OnAutoFunctionInvocationAsync(
            AutoFunctionInvocationContext context,
            Func<AutoFunctionInvocationContext, Task> next)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Function {context.Function.Name} called");

                if (context.Arguments != null)
                {
                    for (int i = 0; i < context.Arguments.Count; i++)
                    {
                        Console.WriteLine($"-> Arg: {context.Arguments.ToList()[i]}");
                    }
                }

                await next(context); // здесь происходит вызов функции
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: " + e.Message);
                throw;
            }
            finally
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
