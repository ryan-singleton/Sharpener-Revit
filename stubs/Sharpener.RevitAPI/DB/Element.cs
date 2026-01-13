// The Sharpener project licenses this file to you under the MIT license.

namespace Autodesk.Revit.DB;

public class Element
{
    public ElementId Id { get; set; } = new();

    public Document Document { get; set; } = new();
}
