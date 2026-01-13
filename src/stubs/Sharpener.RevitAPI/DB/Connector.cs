// The Sharpener project licenses this file to you under the MIT license.

namespace Autodesk.Revit.DB;

public class Connector
{
    public bool IsConnected { get; set; }

    public ConnectorSet AllRefs { get; set; }

    public XYZ Origin { get; set; }

    public Element Owner { get; set; }
}

