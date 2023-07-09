from hypothesis import given

from tests.binding import Point
from tests.utils import (equivalence,
                         implication)
from . import strategies


@given(strategies.points)
def test_reflexivity(point: Point) -> None:
    assert point >= point


@given(strategies.points, strategies.points)
def test_antisymmetry(first: Point, second: Point) -> None:
    assert equivalence(first >= second >= first, first == second)


@given(strategies.points, strategies.points, strategies.points)
def test_transitivity(first: Point, second: Point, third: Point) -> None:
    assert implication(first >= second >= third, first >= third)


@given(strategies.points, strategies.points)
def test_alternatives(first: Point, second: Point) -> None:
    assert equivalence(first >= second, first > second or first == second)
    assert equivalence(first >= second, first > second or not first != second)
    assert equivalence(first >= second, second < first or not first != second)
    assert equivalence(first >= second, second < first or first == second)
    assert equivalence(first >= second, second <= first)
    assert equivalence(first >= second, not second > first)
    assert equivalence(first >= second, not first < second)
