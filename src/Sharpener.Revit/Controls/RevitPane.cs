// The Sharpener project licenses this file to you under the MIT license.

using System.Windows.Controls;
using Autodesk.Revit.UI;

namespace Sharpener.Revit.Controls;

/// <summary>
///     A convenient subclass that helps to stand up a dockable pane. Dockable Revit pages can be created with this easily.
/// </summary>
public class RevitPane : Page, IDockablePaneProvider
{
    /// <summary>
    ///     Instantiates a new Revit dockable pane.
    /// </summary>
    /// <param name="paneName">The name of the pane, which can be searched for later, or can feed its own labels and controls.</param>
    protected RevitPane(string paneName)
    {
        PaneName = paneName;
        DockablePaneId = new DockablePaneId(Guid.NewGuid());
    }

    /// <summary>
    ///     The unique identifier of the dockable pane.
    /// </summary>
    public DockablePaneId DockablePaneId { get; }

    /// <summary>
    ///     The name of the pane, which can be searched for later, or can feed its own labels and controls.
    /// </summary>
    public string PaneName { get; }

    /// <inheritdoc />
    public virtual void SetupDockablePane(DockablePaneProviderData data)
    {
        data.FrameworkElement = this;
        data.InitialState = new DockablePaneState();
    }
}
