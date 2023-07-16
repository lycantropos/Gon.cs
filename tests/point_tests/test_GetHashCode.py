from hypothesis import given

from tests.binding import Point
from tests.utils import (equivalence,
                         implication,
                         reverse_point_coordinates)
from . import strategies


@given(strategies.points)
def test_determinism(point: Point) -> None:
    result = hash(point)

    assert result == hash(point)


@given(strategies.points, strategies.points)
def test_surjection(first: Point, second: Point) -> None:
    assert implication(first == second, hash(first) == hash(second))


@given(strategies.points, strategies.points)
def test_coordinates_reversals(first: Point, second: Point) -> None:
    assert equivalence(hash(first) == hash(second),
                       (hash(reverse_point_coordinates(first))
                        == hash(reverse_point_coordinates(second))))
