using KasumiGUI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace KasumiGUI.Data.Services
{
    public class DataProvider
    {
        public DBContext DB { get; private set; }

        public DataProvider() => DB = new();

        public static async Task<List<T>> GetBotResponsesAsync<T>(DbSet<T> dbSet) where T : BaseResponse => await Task.Run(() => dbSet.ToList());
    }
}