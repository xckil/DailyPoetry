
using System.Linq.Expressions;
using DailypoetryM.Services;
using DailyPoetryM.Models;
using SQLite;

namespace DailyPoetryM.Services;


/// <summary>
/// 1.
/// </summary>



public class PoetryStorage : IPoetryStorage
{
    public const string DbName = "poetrydb.sqlite3"; //文件名

    public static readonly string PoetryDbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DbName);//连接名称文件名进行拼合

    /// <summary>
    /// 打开数据库连接
    /// </summary>
    private SQLiteAsyncConnection? _connection;
    private SQLiteAsyncConnection Connection => _connection ??= new SQLiteAsyncConnection(PoetryDbPath);

    private readonly IPreferencesStoage _preferencesStoage;

    public PoetryStorage(IPreferencesStoage preferencesStoage)
    {
        _preferencesStoage = preferencesStoage;
    }

    public bool IsInitialized =>
        _preferencesStoage.Get(PoetryStorageConstant.VersionKey, 0) == PoetryStorageConstant.Version;

    public async Task<Poetry> GetPoertryAsync(int id) => await Connection.Table<Poetry>().FirstOrDefaultAsync(p => p.Id == id);

    public async Task<IEnumerable<Poetry>> GetPoetriesAsync(Expression<Func<Poetry, bool>> where, int skip, int take)=> await Connection.Table<Poetry>().Where(where).Skip(skip).Take(take).ToListAsync();
 

    public async Task InitializeAsync()
    {
       
        await using var dbFileStream = new FileStream(PoetryDbPath, FileMode.OpenOrCreate);

        await using var dbAssetStream = typeof(PoetryStorage).Assembly.GetManifestResourceStream(DbName);

        await dbAssetStream.CopyToAsync(dbFileStream);

        _preferencesStoage.Set(PoetryStorageConstant.VersionKey, PoetryStorageConstant.Version);
    }

    public async Task CloseAsync() => await Connection.CloseAsync();
}

//判断数据库初始化
public static class PoetryStorageConstant
{
    public const int Version = 1;

    public const string VersionKey = nameof(PoetryStorageConstant) + "." + nameof(Version);
}