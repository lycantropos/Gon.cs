from hypothesis import given

from tests.binding import Polygon
from tests.utils import (are_polygons_sequences_equivalent,
                         equivalence,
                         reverse_polygon_coordinates)
from . import strategies


@given(strategies.polygons)
def test_self_inverse(polygon: Polygon) -> None:
    result = polygon - polygon

    assert result == []


@given(strategies.polygons, strategies.polygons)
def test_commutative_case(first: Polygon, second: Polygon) -> None:
    assert equivalence(first - second == second - first, first == second)


@given(strategies.polygons, strategies.polygons)
def test_reversals(first: Polygon, second: Polygon) -> None:
    result = first - second

    assert are_polygons_sequences_equivalent(
            result, [reverse_polygon_coordinates(polygon)
                     for polygon in (reverse_polygon_coordinates(first)
                                     - reverse_polygon_coordinates(second))]
    )