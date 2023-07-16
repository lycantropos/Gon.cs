from hypothesis import given

from tests.binding import Polygon
from tests.utils import (are_polygons_sequences_equivalent,
                         reverse_polygon_coordinates)
from . import strategies


@given(strategies.polygons, strategies.polygons)
def test_basic(first: Polygon, second: Polygon) -> None:
    result = first & second

    assert isinstance(result, list)
    assert all(isinstance(element, Polygon) for element in result)


@given(strategies.polygons)
def test_idempotence(polygon: Polygon) -> None:
    assert polygon & polygon == [polygon]


@given(strategies.polygons, strategies.polygons)
def test_commutativity(first: Polygon, second: Polygon) -> None:
    assert first & second == second & first


@given(strategies.polygons, strategies.polygons)
def test_reversals(first: Polygon, second: Polygon) -> None:
    result = first & second

    assert are_polygons_sequences_equivalent(
            result, [reverse_polygon_coordinates(polygon)
                     for polygon in (reverse_polygon_coordinates(first)
                                     & reverse_polygon_coordinates(second))]
    )
