// The Sharpener project licenses this file to you under the MIT license.

namespace Autodesk.Revit.DB;

public class Connector
{
    public bool IsConnected { get; set; }

    public ConnectorSet AllRefs { get; set; }

    public XYZ Origin { get; set; }

    public Element Owner { get; set; }

    public ConnectorType ConnectorType { get; set; }

    public void DisconnectFrom(Connector connector)
    {
    }

    public void ConnectTo(Connector connector)
    {
    }

    public MEPConnectorInfo GetMEPConnectorInfo()
    {
        return new MEPConnectorInfo();
    }
}
