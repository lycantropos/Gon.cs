import math
import typing as t
from functools import singledispatch

from ground.base import get_context

from tests.binding import (BigInteger,
                           Box,
                           Contour,
                           Fractions,
                           Multipolygon,
                           Point,
                           Polygon,
                           Segment)

context = get_context().replace(box_cls=Box,
                                contour_cls=Contour,
                                multipolygon_cls=Multipolygon,
                                point_cls=Point,
                                polygon_cls=Polygon,
                                segment_cls=Segment)


def are_polygons_sequences_equivalent(first: t.Sequence[Polygon],
                                      second: t.Sequence[Polygon]) -> bool:
    return len(first) == len(second) and frozenset(first) == frozenset(second)


def equivalence(left: bool, right: bool) -> bool:
    return left is right


def implication(antecedent: bool, consequent: bool) -> bool:
    return not antecedent or consequent


def is_even_permutation(index: int, size: int) -> bool:
    return ((index % math.factorial(size) - 1) % 4) > 1


def is_fraction_valid(value: Fractions.Fraction) -> bool:
    return (value.denominator > 0
            and BigInteger.GreatestCommonDivisor(value.numerator,
                                                 value.denominator).IsOne)


def nth_permutation(index: int, size: int) -> t.Sequence[int]:
    permutations_count = math.factorial(size)
    index %= permutations_count
    indices = list(range(size))
    result = []
    for rest_size in range(size, 0, -1):
        permutations_count //= rest_size
        step, index = divmod(index, permutations_count)
        result.append(indices.pop(step))
    return result


_T = t.TypeVar('_T')


def permute(sequence: t.Sequence[_T], index: int) -> t.Sequence[_T]:
    return [sequence[index] for index in nth_permutation(index, len(sequence))]


def reverse_box_coordinates(box: Box) -> Box:
    return Box(box.min_y, box.max_y, box.min_x, box.max_x)


def reverse_contour(contour: Contour) -> Contour:
    return Contour(contour.vertices[::-1])


def reverse_contour_coordinates(contour: Contour) -> Contour:
    return Contour([reverse_point_coordinates(vertex)
                    for vertex in contour.vertices])


def reverse_multipolygon(multipolygon: Multipolygon) -> Multipolygon:
    return Multipolygon(reverse_sequence(multipolygon.polygons))


def reverse_multipolygon_polygons_borders(
        multipolygon: Multipolygon
) -> Multipolygon:
    return Multipolygon([reverse_polygon_border(polygon)
                         for polygon in multipolygon.polygons])


def reverse_multipolygon_polygons_holes(
        multipolygon: Multipolygon
) -> Multipolygon:
    return Multipolygon([reverse_polygon_holes(polygon)
                         for polygon in multipolygon.polygons])


def reverse_multipolygon_polygons_holes_contours(
        multipolygon: Multipolygon
) -> Multipolygon:
    return Multipolygon([reverse_polygon_holes_contours(polygon)
                         for polygon in multipolygon.polygons])


def reverse_multipolygon_coordinates(
        multipolygon: Multipolygon
) -> Multipolygon:
    return Multipolygon([reverse_polygon_coordinates(polygon)
                         for polygon in multipolygon.polygons])


def reverse_point_coordinates(point: Point) -> Point:
    return Point(point.y, point.x)


def reverse_polygon_border(polygon: Polygon) -> Polygon:
    return Polygon(reverse_contour(polygon.border), polygon.holes)


def reverse_polygon_coordinates(polygon: Polygon) -> Polygon:
    return Polygon(reverse_contour_coordinates(polygon.border),
                   [reverse_contour_coordinates(hole)
                    for hole in polygon.holes])


def reverse_polygon_holes(polygon: Polygon) -> Polygon:
    return Polygon(polygon.border, reverse_sequence(polygon.holes))


def reverse_polygon_holes_contours(polygon: Polygon) -> Polygon:
    return Polygon(polygon.border, [reverse_contour(hole)
                                    for hole in polygon.holes])


def reverse_sequence(value: t.Sequence[_T]) -> t.Sequence[_T]:
    return value[::-1]


_ShapedT = t.TypeVar('_ShapedT', Multipolygon, Polygon)


@singledispatch
def reverse_shaped_coordinates(shaped: _ShapedT) -> _ShapedT:
    raise TypeError(type(shaped))


reverse_shaped_coordinates.register(Polygon, reverse_polygon_coordinates)
reverse_shaped_coordinates.register(Multipolygon,
                                    reverse_multipolygon_coordinates)


def rotate_contour(contour: Contour, offset: int) -> Contour:
    return Contour(rotate_sequence(contour.vertices, offset))


def rotate_multipolygon(multipolygon: Multipolygon,
                        offset: int) -> Multipolygon:
    return Multipolygon(rotate_sequence(multipolygon.polygons, offset))


def rotate_multipolygon_polygons_borders(multipolygon: Multipolygon,
                                         offset: int) -> Multipolygon:
    return Multipolygon([rotate_polygon_border(polygon, offset)
                         for polygon in multipolygon.polygons])


def rotate_multipolygon_polygons_holes(multipolygon: Multipolygon,
                                       offset: int) -> Multipolygon:
    return Multipolygon([rotate_polygon_holes(polygon, offset)
                         for polygon in multipolygon.polygons])


def rotate_multipolygon_polygons_holes_contours(multipolygon: Multipolygon,
                                                offset: int) -> Multipolygon:
    return Multipolygon([rotate_polygon_holes_contours(polygon, offset)
                         for polygon in multipolygon.polygons])


def rotate_polygon_border(polygon: Polygon, offset: int) -> Polygon:
    return Polygon(rotate_contour(polygon.border, offset), polygon.holes)


def rotate_polygon_holes(polygon: Polygon, offset: int) -> Polygon:
    return Polygon(polygon.border, rotate_sequence(polygon.holes, offset))


def rotate_polygon_holes_contours(polygon: Polygon, offset: int) -> Polygon:
    return Polygon(polygon.border, [rotate_contour(hole, offset)
                                    for hole in polygon.holes])


def rotate_sequence(sequence: t.Sequence[_T], offset: int) -> t.List[_T]:
    if not sequence:
        return []
    offset = (offset % len(sequence)) - len(sequence) * (offset < 0)
    return [*sequence[-offset:], *sequence[:-offset]]
