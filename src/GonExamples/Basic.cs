using System.Diagnostics;

using Point = Gon.Point<double>;
using Contour = Gon.Contour<double>;
using Polygon = Gon.Polygon<double>;
using Location = Gon.Location;

public static class Basic
{
    public static void RunExamples()
    {
        // construction
        var squareBorder = new Contour(
            new[] { new Point(0, 0), new Point(4, 0), new Point(4, 4), new Point(0, 4) }
        );
        var square = new Polygon(squareBorder);

        // accessing various properties
        Debug.Assert(square.Border == squareBorder);
        Debug.Assert(square.Border.Vertices.Length == 4);
        Debug.Assert(square.Holes.Length == 0);

        // equality checks
        Debug.Assert(square == new Polygon(squareBorder));

        // point-in-polygon checks
        Debug.Assert(square.Contains(new Point(2, 2)));
        Debug.Assert(square.Contains(new Point(4, 4)));
        Debug.Assert(!square.Contains(new Point(6, 6)));

        // point location queries
        Debug.Assert(square.Locate(new Point(2, 2)) == Location.Interior);
        Debug.Assert(square.Locate(new Point(4, 4)) == Location.Boundary);
        Debug.Assert(square.Locate(new Point(6, 6)) == Location.Exterior);

        // set intersection
        Polygon[] intersection = square & square;
        Debug.Assert(intersection.Length == 1);
        Debug.Assert(intersection[0] == square);

        // set union
        Polygon[] union = square | square;
        Debug.Assert(union.Length == 1);
        Debug.Assert(union[0] == square);

        // set difference
        Polygon[] difference = square - square;
        Debug.Assert(difference.Length == 0);

        // set symmetric difference
        Polygon[] symmetricDifference = square ^ square;
        Debug.Assert(symmetricDifference.Length == 0);
    }
}
