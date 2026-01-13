// The Sharpener project licenses this file to you under the MIT license.

using Autodesk.Revit.DB.Events;

namespace Autodesk.Revit.UI;

public class ControlledApplication
{
    public event EventHandler<ApplicationInitializedEventArgs> ApplicationInitialized;
}
