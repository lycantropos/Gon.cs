from hypothesis import given

from tests.binding import Point
from tests.utils import (equivalence,
                         implication)
from . import strategies


@given(strategies.points)
def test_irreflexivity(point: Point) -> None:
    assert not point < point


@given(strategies.points, strategies.points)
def test_asymmetry(first: Point, second: Point) -> None:
    assert implication(first < second, not second < first)


@given(strategies.points, strategies.points, strategies.points)
def test_transitivity(first: Point, second: Point, third: Point) -> None:
    assert implication(first < second < third, first < third)


@given(strategies.points, strategies.points)
def test_alternatives(first: Point, second: Point) -> None:
    assert equivalence(first < second, first <= second and first != second)
    assert equivalence(first < second, first <= second and not first == second)
    assert equivalence(first < second, second >= first and not first == second)
    assert equivalence(first < second, second >= first and first != second)
    assert equivalence(first < second, second > first)
    assert equivalence(first < second, not second <= first)
    assert equivalence(first < second, not first >= second)
