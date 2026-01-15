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
    /// <param name="dockablePaneId">
    ///     Optional <see cref="Guid" /> that will be used to generate the
    ///     <see cref="DockablePaneId" /> for this <see cref="IDockablePaneProvider" />. If null, a random new one will be
    ///     generated specific to the Revit session.
    /// </param>
    protected RevitPane(string paneName, Guid? dockablePaneId = null)
    {
        PaneName = paneName;
        DockablePaneId = new DockablePaneId(dockablePaneId ?? Guid.NewGuid());
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
