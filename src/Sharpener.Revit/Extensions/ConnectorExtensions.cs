// The Sharpener project licenses this file to you under the MIT license.

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;

namespace Sharpener.Revit.Extensions;

/// <summary>
///     Extensions for Revit <see cref="Connector" />s.
/// </summary>
public static class ConnectorExtensions
{
    /// <summary>
    ///     A simple helper to cast the members of a <see cref="ConnectorSet" /> to <see cref="Connector" /> references.
    /// </summary>
    /// <param name="connectorSet">The <see cref="ConnectorSet" /> to get the <see cref="Connector" />s from.</param>
    /// <returns>A collection of <see cref="Connector" />s.</returns>
    public static IEnumerable<Connector> AsConnectors(this ConnectorSet connectorSet)
    {
        return connectorSet.Cast<Connector>();
    }

    /// <summary>
    ///     Gets the <see cref="Connector" /> that the provided <see cref="Connector" /> is connected to.
    /// </summary>
    /// <param name="connector">The <see cref="Connector" /> to get the connection from.</param>
    /// <returns>The connected <see cref="Connector" /> or null if no connection was found.</returns>
    public static Connector? Connection(this Connector connector)
    {
        if (!connector.IsConnected)
        {
            return null;
        }

        return connector.AllRefs.AsConnectors()
            .FirstOrDefault(c => c.Owner.Id != connector.Owner.Id);
    }

    /// <summary>
    ///     Disconnects any <see cref="Connector" /> from the provided <see cref="Connector" />.
    /// </summary>
    /// <param name="connector">The <see cref="Connector" /> to disconnect.</param>
    /// <returns>The <see cref="Connector" /> that was disconnected from the provided <see cref="Connector" />.</returns>
    public static Connector? Disconnect(this Connector connector)
    {
        if (!connector.IsConnected)
        {
            return null;
        }

        var connection = connector.Connection();
        if (connection is null)
        {
            return null;
        }

        connector.DisconnectFrom(connection);
        return connection;
    }

    /// <summary>
    ///     Find the closest <see cref="Connector" /> in the collection to the provided <see cref="XYZ" />.
    /// </summary>
    /// <param name="connectors">The <see cref="Connector" />s from which the closest <see cref="Connector" /> will be found.</param>
    /// <param name="point">The <see cref="XYZ" /> point to search relative to.</param>
    /// <returns>The closest <see cref="Connector" /> to the <see cref="XYZ" />.</returns>
    public static Connector FindClosestTo(this IEnumerable<Connector> connectors, XYZ point)
    {
        return connectors.OrderBy(c => c.Origin.DistanceTo(point)).First();
    }

    /// <summary>
    ///     Gets the <see cref="ConnectorManager" /> for the element type.
    /// </summary>
    /// <remarks>
    ///     It's just ridiculous how often we have to parse the type of the object and then find the
    ///     <see cref="ConnectorManager" />. So this makes it all much more shorthand.
    ///     This only current supports these types and their derivatives. Consider adding to it when needed.
    ///     <see cref="FabricationPart" />, <see cref="FamilyInstance" /> with <see cref="MEPModel" />,
    ///     <see cref="MechanicalFitting" />, and <see cref="MEPCurve" />.
    /// </remarks>
    /// <param name="element">The element whose <see cref="ConnectorManager" /> is to be gotten.</param>
    /// <returns>The <see cref="ConnectorManager" /> or null if none were found.</returns>
    public static ConnectorManager? GetConnectorManager(this Element element)
    {
        switch (element)
        {
            case FabricationPart fab:
                return fab.ConnectorManager;

            case FamilyInstance fi:
                var mepModel = fi.MEPModel;
                if (mepModel is MechanicalFitting fitting)
                {
                    return fitting.ConnectorManager;
                }

                return mepModel?.ConnectorManager;
            case MEPCurve mepCurve:
                return mepCurve.ConnectorManager;

            default:
                return null;
        }
    }

    /// <summary>
    ///     A shorthand way to just get all of the <see cref="Connector" />s from an element. This has the same element
    ///     limitations as <see cref="GetConnectorManager" />.
    /// </summary>
    /// <param name="element">The element to get the <see cref="Connector" />s from.</param>
    /// <returns>A collection of <see cref="Connector" />s, empty if there are none.</returns>
    public static IEnumerable<Connector> GetConnectors(this Element element)
    {
        return element.GetConnectorManager()?.Connectors.AsConnectors() ?? [];
    }

    /// <summary>
    ///     Filters for all the connectors that are not connected to another one.
    /// </summary>
    /// <param name="element">The element to get the <see cref="Connector" />s from.</param>
    /// <returns>A collection of <see cref="Connector" />s that are not connected to anything, empty if there are none.</returns>
    public static IEnumerable<Connector> GetUnusedConnectors(this Element element)
    {
        return element.GetConnectorManager()?.UnusedConnectors.AsConnectors() ?? [];
    }

    /// <summary>
    ///     Moves a <see cref="Connector" /> to an <see cref="XYZ" /> point.
    /// </summary>
    /// <remarks>
    ///     A very handy way to move elements by connector instead of by origin.
    /// </remarks>
    /// <param name="connector">The <see cref="Connector" /> to move.</param>
    /// <param name="point">The <see cref="XYZ" /> to move it to.</param>
    public static void Move(this Connector connector, XYZ point)
    {
        var translation = point.Subtract(connector.Origin);
        ElementTransformUtils.MoveElement(connector.Owner.Document, connector.Owner.Id, translation);
    }

    /// <summary>
    ///     Gets the primary <see cref="Connector" /> of an element.
    /// </summary>
    /// <remarks>
    ///     Note that whenever the element is adjusted in Revit, there's a high chance that this can change. Only rely upon
    ///     this within single code execution contexts.
    /// </remarks>
    /// <param name="element">The element whose primary <see cref="Connector" /> is to be gotten.</param>
    /// <returns>The primary <see cref="Connector" /> or null if none found.</returns>
    public static Connector? PrimaryConnector(this Element element)
    {
        return element.GetConnectors().FirstOrDefault(c => c.GetMEPConnectorInfo().IsPrimary);
    }

    /// <summary>
    ///     Gets the secondary <see cref="Connector" /> of an element.
    /// </summary>
    /// <remarks>
    ///     Note that whenever the element is adjusted in Revit, there's a high chance that this can change. Only rely upon
    ///     this within single code execution contexts.
    /// </remarks>
    /// <param name="element">The element whose secondary <see cref="Connector" /> is to be gotten.</param>
    /// <returns>The secondary <see cref="Connector" /> or null if none found.</returns>
    public static Connector? SecondaryConnector(this Element element)
    {
        return element.GetConnectors().FirstOrDefault(c => c.GetMEPConnectorInfo().IsSecondary);
    }

    /// <summary>
    ///     Attempts to connect two elements by finding the closest pair of <see cref="Connector" />s between the two of them
    ///     and then connecting them. If <see cref="allowMove" /> is false, this only makes the connection within the tolerance
    ///     (1.0e-09), which is prescribed by Revit itself to achieve success. If <see cref="allowMove" /> is true, it will
    ///     move <see cref="element" /> to <see cref="destinationElement" /> and then make the connection.
    /// </summary>
    /// <param name="element">The first element to connect.</param>
    /// <param name="destinationElement">The second element to connect.</param>
    /// <param name="allowMove">
    ///     Whether to allow moving <see cref="element" /> to <see cref="destinationElement" /> to achieve
    ///     the connection. Defaults to false.
    /// </param>
    /// <returns>True if the connection was made, false if it was not.</returns>
    public static bool TryConnect(this Element element, Element destinationElement, bool allowMove = false)
    {
        var query = from connFrom in element.GetConnectors()
            from connTo in destinationElement.GetConnectors()
            where connFrom.ConnectorType == connTo.ConnectorType
            select new { From = connFrom, To = connTo, Distance = connFrom.Origin.DistanceTo(connTo.Origin) };

        var closest = query.OrderBy(x => x.Distance).First();

        if (allowMove && !closest.From.Origin.IsAlmostEqualTo(closest.To.Origin))
        {
            closest.From.Move(closest.To.Origin);
        }

        closest.From.ConnectTo(closest.To);
        return true;
    }
}
