

using System.Linq.Expressions;
using DailyPoetryM.Models;

namespace DailyPoetryM.Services;


/// <summary>
/// 1.
/// </summary>



public class PoetryStorage : IPoetryStorage
{
    
    public bool IsInitialized =>
        Preferences.Get(PoetryStorageConstant.VersionKey, 0) == PoetryStorageConstant.Version;

    public Task<Poetry> GetPoertryAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Poetry>> GetPoetriesAsync(Expression<Func<Poetry, bool>> where, int skip, int take)
    {
        throw new NotImplementedException();
    }

    public async Task InitializeAsync()
    {
        throw new NotImplementedException();
    }
}

//判断数据库初始化
public static class PoetryStorageConstant
{
    public const int Version = 1;

    public const string VersionKey = nameof(PoetryStorageConstant)+"."+ nameof(Version);
}