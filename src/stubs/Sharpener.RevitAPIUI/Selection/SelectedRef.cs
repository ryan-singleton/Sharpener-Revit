// The Sharpener project licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace Autodesk.Revit.UI.Selection;

public class SelectedRef
{
    public IEnumerable<Element> Select(Func<int, Element> func)
    {
        return [];
    }
}
