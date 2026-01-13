// The Sharpener project licenses this file to you under the MIT license.

using System.Collections;

namespace Autodesk.Revit.DB;

public class ConnectorSet : IEnumerable
{
    public IEnumerator GetEnumerator()
    {
        return new List<Connector>().GetEnumerator();
    }
}
