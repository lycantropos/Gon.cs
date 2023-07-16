from hypothesis import given

from tests.binding import (Multipolygon,
                           Polygon)
from tests.hints import Shaped
from tests.utils import (are_polygons_sequences_equivalent,
                         reverse_multipolygon_coordinates,
                         reverse_polygon_coordinates,
                         reverse_shaped_coordinates)
from . import strategies


@given(strategies.multipolygons, strategies.shaped_geometries)
def test_basic(first: Multipolygon, second: Shaped) -> None:
    result = first & second

    assert isinstance(result, list)
    assert all(isinstance(element, Polygon) for element in result)


@given(strategies.multipolygons)
def test_idempotence(multipolygon: Multipolygon) -> None:
    assert are_polygons_sequences_equivalent(multipolygon & multipolygon,
                                             multipolygon.polygons)


@given(strategies.multipolygons, strategies.shaped_geometries)
def test_commutativity(first: Multipolygon, second: Shaped) -> None:
    assert first & second == second & first


@given(strategies.multipolygons, strategies.shaped_geometries)
def test_reversals(multipolygon: Multipolygon, shaped: Shaped) -> None:
    result = multipolygon & shaped

    assert are_polygons_sequences_equivalent(
            result, [reverse_polygon_coordinates(multipolygon)
                     for multipolygon in (
                             reverse_multipolygon_coordinates(multipolygon)
                             & reverse_shaped_coordinates(shaped)
                     )]
    )
