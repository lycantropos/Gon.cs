from hypothesis import given

from tests.binding import Point
from tests.utils import equivalence
from . import strategies


@given(strategies.points)
def test_irreflexivity(point: Point) -> None:
    assert not point != point


@given(strategies.points, strategies.points)
def test_symmetry(first: Point, second: Point) -> None:
    assert equivalence(first != second, second != first)


@given(strategies.points, strategies.points)
def test_equivalents(first: Point, second: Point) -> None:
    assert equivalence(first != second, not first == second)
    assert equivalence(first != second, first > second or first < second)
    assert equivalence(first != second, first > second or second > first)
    assert equivalence(first != second, second < first or second > first)
    assert equivalence(first != second, second < first or first < second)
