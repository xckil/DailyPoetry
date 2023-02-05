

namespace DailypoetryM.Services;

public interface IPreferencesStoage
{
    void Set(string key, int value);
    int Get(string key,int defaultValue);
}
