from hypothesis import given

from tests.binding import Polygon
from tests.hints import Shaped
from tests.utils import (are_polygons_sequences_equivalent,
                         reverse_polygon_coordinates,
                         reverse_shaped_coordinates)
from . import strategies


@given(strategies.polygons, strategies.shaped_geometries)
def test_basic(polygon: Polygon, shaped: Shaped) -> None:
    result = polygon ^ shaped

    assert isinstance(result, list)
    assert all(isinstance(element, Polygon) for element in result)


@given(strategies.polygons)
def test_self_inverse(polygon: Polygon) -> None:
    result = polygon ^ polygon

    assert result == []


@given(strategies.polygons, strategies.polygons)
def test_commutativity(first: Polygon, second: Polygon) -> None:
    assert are_polygons_sequences_equivalent(first ^ second, second ^ first)


@given(strategies.polygons, strategies.shaped_geometries)
def test_reversals(polygon: Polygon, shaped: Shaped) -> None:
    result = polygon ^ shaped

    assert are_polygons_sequences_equivalent(
            result, [reverse_polygon_coordinates(polygon)
                     for polygon in (reverse_polygon_coordinates(polygon)
                                     ^ reverse_shaped_coordinates(shaped))]
    )
