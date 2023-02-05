
using System.Linq.Expressions;
using DailyPoetryM.Models;

namespace DailyPoetryM.Services;

public interface IPoetryStorage
{
    public bool IsInitialized { get; }
    Task InitializeAsync();

    Task<Poetry> GetPoertryAsync(int id);

    Task<IEnumerable<Poetry>> GetPoetriesAsync(
        Expression<Func<Poetry, bool>> where,int skip,int take); //相当于where语句
}
