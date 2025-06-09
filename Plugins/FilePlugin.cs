using Microsoft.SemanticKernel;
using System;
using System.IO;

namespace ConsoleApp1.Plugins;

public class FilePlugin
{
    private readonly string _rootFolder;

    public FilePlugin()
    {
        _rootFolder = Path.Combine(Directory.GetCurrentDirectory(), "Plugins", "HelloPlugin");
        if (!Directory.Exists(_rootFolder))
        {
            Directory.CreateDirectory(_rootFolder);
        }
    }
    
    private string SecurePath(string relativePath)
    {
        string fullPath = Path.GetFullPath(Path.Combine(_rootFolder, relativePath));

        if (!fullPath.StartsWith(_rootFolder))
            throw new UnauthorizedAccessException("Недопустимый путь: выход за пределы разрешённой папки.");

        return fullPath;
    }

    [KernelFunction("get_root_folder")]
    public string GetRootFolder()
    {
        return _rootFolder;
    }

    [KernelFunction("delete_root_folder")]
    public void DeleteRootFolder()
    {
        Directory.Delete(_rootFolder, recursive: true);
    }

    [KernelFunction("create_folder")]
    public void CreateFolder(string folderName)
    {
        string fullPath = SecurePath(folderName);
        Directory.CreateDirectory(fullPath);
    }

    [KernelFunction("create_file")]
    public void CreateFile(string fileName)
    {
        string fullPath = SecurePath(fileName);
        File.Create(fullPath).Close();
    }

    [KernelFunction("get_content_of_file")]
    public string GetContentOfFile(string fileName)
    {
        string fullPath = SecurePath(fileName);
        return File.ReadAllText(fullPath);
    }

    [KernelFunction("write_to_file")]
    public void WriteToFile(string fileName, string content)
    {
        string fullPath = SecurePath(fileName);
        File.WriteAllText(fullPath, content);
    }

    [KernelFunction("move_file")]
    public void MoveFile(string sourceName, string destinationName)
    {
        string source = SecurePath(sourceName);
        string destination = SecurePath(destinationName);
        File.Move(source, destination);
    }

    [KernelFunction("move_folder")]
    public void MoveFolder(string sourceName, string destinationName)
    {
        string source = SecurePath(sourceName);
        string destination = SecurePath(destinationName);
        Directory.Move(source, destination);
    }

    [KernelFunction("get_files_for_folder")]
    public string[] GetFilesForFolder(string folderName)
    {
        string fullPath = SecurePath(folderName);
        return Directory.GetFiles(fullPath);
    }
}
