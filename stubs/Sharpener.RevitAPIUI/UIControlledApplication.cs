// The Sharpener project licenses this file to you under the MIT license.

using Autodesk.Revit.UI.Events;

namespace Autodesk.Revit.UI;

// ReSharper disable once InconsistentNaming
public class UIControlledApplication
{
    public ControlledApplication ControlledApplication { get; set; } = new();
    public event EventHandler<ThemeChangedEventArgs> ThemeChanged = null!;

    public RibbonPanel CreateRibbonPanel(string name, string panelName)
    {
        return new RibbonPanel();
    }

    public void CreateRibbonTab(string tabName)
    {
    }

    protected virtual void OnThemeChanged(ThemeChangedEventArgs args)
    {
        ThemeChanged?.Invoke(this, args);
    }

    public void RegisterDockablePane(DockablePaneId id, string title, IDockablePaneProvider provider)
    {
    }
}
