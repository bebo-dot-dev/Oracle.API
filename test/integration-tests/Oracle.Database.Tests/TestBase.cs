using System.Threading.Tasks;
using NUnit.Framework;

namespace Oracle.Database.Tests;

public abstract class TestBase
{
    [TearDown]
    public async Task TearDown()
    {
        await SetupFixture.ClearDownDatabase();
    }
}