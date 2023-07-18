using System.Diagnostics;

using Point = Gon.Point<double>;
using Contour = Gon.Contour<double>;
using Polygon = Gon.Polygon<double>;

namespace GonExamples
{
    public static class PolygonsWithoutHolesBinaryOperations
    {
        public static Polygon FirstOperand = new Polygon(
            new Contour(
                new[] { new Point(1, 1), new Point(5, 1), new Point(5, 5), new Point(1, 5) }
            )
        );
        public static Polygon SecondOperand = new Polygon(
            new Contour(
                new[] { new Point(0, 3), new Point(3, 0), new Point(6, 3), new Point(3, 6) }
            )
        );

        static void RunDifferenceExample()
        {
            var difference = FirstOperand - SecondOperand;
            Debug.Assert(difference.Length == 4);
            Debug.Assert(
                difference[0]
                    == new Polygon(
                        new Contour(new[] { new Point(1, 1), new Point(2, 1), new Point(1, 2) })
                    )
            );
            Debug.Assert(
                difference[1]
                    == new Polygon(
                        new Contour(new[] { new Point(1, 4), new Point(2, 5), new Point(1, 5) })
                    )
            );
            Debug.Assert(
                difference[2]
                    == new Polygon(
                        new Contour(new[] { new Point(4, 1), new Point(5, 1), new Point(5, 2) })
                    )
            );
            Debug.Assert(
                difference[3]
                    == new Polygon(
                        new Contour(new[] { new Point(4, 5), new Point(5, 4), new Point(5, 5) })
                    )
            );
        }

        static void RunIntersectionExample()
        {
            var intersection = FirstOperand & SecondOperand;
            Debug.Assert(intersection.Length == 1);
            Debug.Assert(
                intersection[0]
                    == new Polygon(
                        new Contour(
                            new[]
                            {
                                new Point(1, 2),
                                new Point(2, 1),
                                new Point(4, 1),
                                new Point(5, 2),
                                new Point(5, 4),
                                new Point(4, 5),
                                new Point(2, 5),
                                new Point(1, 4)
                            }
                        )
                    )
            );
        }

        static void RunSymmetricDifferenceExample()
        {
            var symmetricDifference = FirstOperand ^ SecondOperand;
            Debug.Assert(symmetricDifference.Length == 8);
            Debug.Assert(
                symmetricDifference[0]
                    == new Polygon(
                        new Contour(new[] { new Point(0, 3), new Point(1, 2), new Point(1, 4) })
                    )
            );
            Debug.Assert(
                symmetricDifference[1]
                    == new Polygon(
                        new Contour(new[] { new Point(1, 1), new Point(2, 1), new Point(1, 2) })
                    )
            );
            Debug.Assert(
                symmetricDifference[2]
                    == new Polygon(
                        new Contour(new[] { new Point(1, 4), new Point(2, 5), new Point(1, 5) })
                    )
            );
            Debug.Assert(
                symmetricDifference[3]
                    == new Polygon(
                        new Contour(new[] { new Point(2, 1), new Point(3, 0), new Point(4, 1) })
                    )
            );
            Debug.Assert(
                symmetricDifference[4]
                    == new Polygon(
                        new Contour(new[] { new Point(2, 5), new Point(4, 5), new Point(3, 6) })
                    )
            );
            Debug.Assert(
                symmetricDifference[5]
                    == new Polygon(
                        new Contour(new[] { new Point(4, 1), new Point(5, 1), new Point(5, 2) })
                    )
            );
            Debug.Assert(
                symmetricDifference[6]
                    == new Polygon(
                        new Contour(new[] { new Point(4, 5), new Point(5, 4), new Point(5, 5) })
                    )
            );
            Debug.Assert(
                symmetricDifference[7]
                    == new Polygon(
                        new Contour(new[] { new Point(5, 2), new Point(6, 3), new Point(5, 4) })
                    )
            );
        }

        static void RunUnionExample()
        {
            var union = FirstOperand | SecondOperand;
            Debug.Assert(union.Length == 1);
            Debug.Assert(
                union[0]
                    == new Polygon(
                        new Contour(
                            new[]
                            {
                                new Point(0, 3),
                                new Point(1, 2),
                                new Point(1, 1),
                                new Point(2, 1),
                                new Point(3, 0),
                                new Point(4, 1),
                                new Point(5, 1),
                                new Point(5, 2),
                                new Point(6, 3),
                                new Point(5, 4),
                                new Point(5, 5),
                                new Point(4, 5),
                                new Point(3, 6),
                                new Point(2, 5),
                                new Point(1, 5),
                                new Point(1, 4)
                            }
                        )
                    )
            );
        }
    }
}
