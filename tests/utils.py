import math
import typing as t

from ground.base import get_context

from tests.binding import (BigInteger,
                           Contour,
                           Fractions,
                           Point,
                           Polygon,
                           Segment)

context = get_context().replace(contour_cls=Contour,
                                segment_cls=Segment,
                                point_cls=Point,
                                polygon_cls=Polygon)


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


def reverse_contour(contour: Contour) -> Contour:
    return Contour(contour.vertices[::-1])


def reverse_contour_coordinates(contour: Contour) -> Contour:
    return Contour([reverse_point_coordinates(vertex)
                    for vertex in contour.vertices])


def reverse_point_coordinates(point: Point) -> Point:
    return Point(point.y, point.x)


def rotate_contour(contour: Contour, offset: int) -> Contour:
    return Contour(rotate_sequence(contour.vertices, offset))


def rotate_sequence(sequence: t.Sequence[_T], offset: int) -> t.List[_T]:
    if not sequence:
        return []
    offset = (offset % len(sequence)) - len(sequence) * (offset < 0)
    return [*sequence[-offset:], *sequence[:-offset]]
