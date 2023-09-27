using System.Collections;

namespace Itmo.Dev.Asap.Gateway.Presentation.Controllers.Tests.TheoryData;

internal sealed class ControllersClassesTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { typeof(IAssemblyMarker) };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}