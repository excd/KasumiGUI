using Discord;
using KasumiGUI.Data.Models;
using KasumiGUI.Utility;
using Microsoft.EntityFrameworkCore;

namespace KasumiGUI.Data.Services
{
    internal class DBInitializer(DBContext botResponseContext)
    {
        private const string LoggerSrc = "DBInit";

        public async Task InitializeAsync()
        {
            await botResponseContext.Database.MigrateAsync();
            await InitializeResourceAsync(botResponseContext.Dechi, "dechi.txt");
            await InitializeResourceAsync(botResponseContext.EightBall, "8ball.txt");
            await InitializeResourceAsync(botResponseContext.Poke, "poke.txt");
            await InitializeResourceAsync(botResponseContext.Bully, "bully.txt");
            await InitializeResourceAsync(botResponseContext.Choice, "choice.txt");
            _ = await botResponseContext.SaveChangesAsync();
        }

        private static async Task InitializeResourceAsync<T>(DbSet<T> dbSet, string fileName) where T : BaseResponse, new()
        {
            if (!await dbSet.AnyAsync())
            {
                string resourcePath = Path.Join(AppContext.BaseDirectory, "resources", fileName);
                if (!File.Exists(resourcePath))
                {
                    await Logger.LogAsync(new LogMessage(LogSeverity.Warning, LoggerSrc, $"Resource file not found: {resourcePath}"));
                    return;
                }
                try
                {
                    await dbSet.AddRangeAsync(fileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase)
                        ? await ParseTxtBotResponseAsync<T>(resourcePath)
                        : await ParseJsonBotResponseAsync<T>(resourcePath));
                    await Logger.LogAsync(new LogMessage(LogSeverity.Info, LoggerSrc, $"Initialized database resource from {fileName}"));
                }
                catch (Exception ex)
                {
                    await Logger.LogAsync(new LogMessage(LogSeverity.Error, LoggerSrc, $"Failed to initialize database: {ex.Message}", ex));
                }
            }
            else
            {
                await Logger.LogAsync(new LogMessage(LogSeverity.Verbose, LoggerSrc, $"Database resource already initialized, skipping {fileName}"));
            }
        }

        private static async Task<List<T>> ParseJsonBotResponseAsync<T>(string path) where T : BaseResponse, new() => System.Text.Json.JsonSerializer.Deserialize<List<T>>(await File.ReadAllTextAsync(path)) ?? [];

        private static async Task<List<T>> ParseTxtBotResponseAsync<T>(string path) where T : BaseResponse, new()
        {
            List<T> responses = [];
            foreach (string line in await File.ReadAllLinesAsync(path))
            {
                if (!string.IsNullOrWhiteSpace(line))
                    responses.Add(new T { Message = line });
            }
            return responses;
        }
    }
}