using SQLite.Net.Interop;

namespace TataAppMac.Interfaces
{
    public interface IConfig
    {
		string DirectoryDB { get; }

		ISQLitePlatform Platform { get; }
    }
}
