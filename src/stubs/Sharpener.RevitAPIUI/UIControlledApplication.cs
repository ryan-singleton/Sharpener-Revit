// The Sharpener project licenses this file to you under the MIT license.

namespace Autodesk.Revit.UI;

// ReSharper disable once InconsistentNaming
public class UIControlledApplication
{
    public delegate void ThemeChangedEventHandler(object sender, ThemeChangedEventArgs args);

    public ControlledApplication ControlledApplication { get; set; }

    public RibbonPanel CreateRibbonPanel(string name, string panelName)
    {
        return new RibbonPanel();
    }

    public void CreateRibbonTab(string tabName)
    {
    }

    public event ThemeChangedEventHandler? ThemeChanged;

    protected virtual void OnThemeChanged(ThemeChangedEventArgs args)
    {
        ThemeChanged?.Invoke(this, args);
    }

    public void RegisterDockablePane(DockablePaneId id, string title, IDockablePaneProvider provider)
    {
    }
}
