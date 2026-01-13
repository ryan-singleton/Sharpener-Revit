// The Sharpener project licenses this file to you under the MIT license.

using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using Sharpener.Revit.Controls;

namespace Sharpener.Revit.Services;

/// <summary>
///     A service that maintains all of the <see cref="RevitPane" /> in the application.
/// </summary>
public class RevitPaneProvider
{
    private readonly UIControlledApplication _application;
    private readonly IEnumerable<RevitPane> _revitPanes;

    /// <summary>
    ///     Creates a new <see cref="RevitPaneProvider" />.
    /// </summary>
    public RevitPaneProvider(UIControlledApplication application, IEnumerable<RevitPane> revitPanes)
    {
        _application = application;
        _revitPanes = revitPanes;
        application.ControlledApplication.ApplicationInitialized += OnInitialized;
    }

    /// <summary>
    ///     Gets the registered <see cref="RevitPane" /> by derived type.
    /// </summary>
    /// <typeparam name="T">The derived type of the <see cref="RevitPane" /> to get.</typeparam>
    /// <returns>The derived <see cref="RevitPane" /> type or null if not available.</returns>
    public T? GetPane<T>() where T : RevitPane
    {
        return _revitPanes.OfType<T>().FirstOrDefault();
    }

    private void OnInitialized(object? sender, ApplicationInitializedEventArgs e)
    {
        foreach (var pane in _revitPanes)
        {
            _application.RegisterDockablePane(pane.DockablePaneId, pane.PaneName, pane);
        }
    }
}
