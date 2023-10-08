using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Oracle.API.Tests;

public abstract class TestBase
{
    protected HttpClient Client;

    [SetUp]
    public void SetUp()
    {
        Client = SetupFixture.Client;
    }

    [TearDown]
    public async Task TearDown()
    {
        await SetupFixture.ClearDownDatabase();
    }
}