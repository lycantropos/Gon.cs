from hypothesis import given

from tests.binding import Point
from tests.utils import (equivalence,
                         implication,
                         reverse_point_coordinates)
from . import strategies


@given(strategies.points)
def test_reflexivity(point: Point) -> None:
    assert point == point


@given(strategies.points, strategies.points)
def test_symmetry(first: Point, second: Point) -> None:
    assert equivalence(first == second, second == first)


@given(strategies.points, strategies.points, strategies.points)
def test_transitivity(first: Point, second: Point, third: Point) -> None:
    assert implication(first == second and second == third, first == third)


@given(strategies.points, strategies.points)
def test_alternatives(first: Point, second: Point) -> None:
    assert equivalence(first == second, not first != second)


@given(strategies.points, strategies.points)
def test_coordinates_reversals(first: Point, second: Point) -> None:
    assert equivalence(first == second, (reverse_point_coordinates(first)
                                         == reverse_point_coordinates(second)))
