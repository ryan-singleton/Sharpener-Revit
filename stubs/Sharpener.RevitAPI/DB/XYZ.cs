// The Sharpener project licenses this file to you under the MIT license.

namespace Autodesk.Revit.DB;

public class XYZ
{
    public XYZ Subtract(XYZ point)
    {
        return new XYZ();
    }

    public double DistanceTo(XYZ source)
    {
        return 1;
    }

    public bool IsAlmostEqualTo(XYZ point)
    {
        return false;
    }
}
