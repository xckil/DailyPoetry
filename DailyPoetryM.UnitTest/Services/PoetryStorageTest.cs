
using System.Linq.Expressions;
using DailypoetryM.Services;
using DailyPoetryM.Models;
using DailyPoetryM.Services;
using Moq;

namespace DailyPoetryM.UnitTest.Services;

//一个函数一个测试 测试名 + 测试条件
public class PoetryStorageTest : IDisposable
{

    ///xunit
    public PoetryStorageTest()
    {
        File.Delete(PoetryStorage.PoetryDbPath);
    }
    public void Dispose()
    {
        File.Delete(PoetryStorage.PoetryDbPath);
    }

    [Fact]
    public void IsInitialized_Default()
    {
        var preferenceStorageMock = new Mock<IPreferencesStoage>();   //制造一个接口实例
        preferenceStorageMock
            .Setup(p => p.Get(PoetryStorageConstant.VersionKey, 0))
            .Returns(PoetryStorageConstant.Version);   //设置行为

        var mockPreferenceStorage = preferenceStorageMock.Object;

        var poetryStorage = new PoetryStorage(mockPreferenceStorage);
        Assert.True(poetryStorage.IsInitialized);
    }

    [Fact]
    public async Task TestInitializeAsync_Default()
    {
        var preferenceStorageMock = new Mock<IPreferencesStoage>();
        var mockPreferenceStorage = preferenceStorageMock.Object;

        var poetryStorage = new PoetryStorage(mockPreferenceStorage);

        Assert.False(File.Exists(PoetryStorage.PoetryDbPath));
        await poetryStorage.InitializeAsync();
        Assert.True(File.Exists(PoetryStorage.PoetryDbPath));

    }

    [Fact]
    public async Task GetPoertryAsync_Default()
    {
        var preferenceStorageMock = new Mock<IPreferencesStoage>();
        var mockPreferenceStorage = preferenceStorageMock.Object;

        var poetryStorage = new PoetryStorage(mockPreferenceStorage);
        await poetryStorage.InitializeAsync();

        var poetry = await poetryStorage.GetPoertryAsync(10001);
        Assert.Equal("临江仙 · 夜归临皋",poetry.Name);
        await poetryStorage.CloseAsync();
    }

    [Fact]
    public async Task GetPoetriesAsync_Default()
    {
        var poetryStorage = await GetInitializedPoetryStorage();
        var poetries = await poetryStorage.GetPoetriesAsync(Expression.Lambda<Func<Poetry,bool>>(Expression.Constant(true),Expression.Parameter(typeof(Poetry),"p")),0,int.MaxValue);
        Assert.Equal(30, poetries.Count());
        await poetryStorage.CloseAsync();
    }

    public static async Task<PoetryStorage> GetInitializedPoetryStorage()
    {
        var preferenceStorageMock = new Mock<IPreferencesStoage>();
        var mockPreferenceStorage = preferenceStorageMock.Object;
        var poetryStorage = new PoetryStorage(mockPreferenceStorage);
        await poetryStorage.InitializeAsync();

        return poetryStorage;
    }

}
