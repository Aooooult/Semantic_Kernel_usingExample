using Microsoft.SemanticKernel;
using System;

namespace ConsoleApp1.Plugins;

public class TimePlugin
{
    [KernelFunction]
    public string GetTime()
    {
        return DateTime.Now.ToString("F"); 
    }
}