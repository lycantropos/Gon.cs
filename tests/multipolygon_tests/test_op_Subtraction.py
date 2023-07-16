from hypothesis import given

from tests.binding import Multipolygon
from tests.hints import Shaped
from tests.utils import (are_polygons_sequences_equivalent,
                         equivalence,
                         reverse_multipolygon_coordinates,
                         reverse_polygon_coordinates,
                         reverse_shaped_coordinates)
from . import strategies


@given(strategies.multipolygons, strategies.shaped_geometries)
def test_basic(multipolygon: Multipolygon, shaped: Shaped) -> None:
    result = multipolygon - shaped

    assert isinstance(result, list)
    assert all(isinstance(element, Multipolygon) for element in result)


@given(strategies.multipolygons)
def test_self_inverse(multipolygon: Multipolygon) -> None:
    assert multipolygon - multipolygon == []


@given(strategies.multipolygons, strategies.multipolygons)
def test_commutative_case(first: Multipolygon, second: Multipolygon) -> None:
    assert equivalence(first - second == second - first, first == second)


@given(strategies.multipolygons, strategies.shaped_geometries)
def test_reversals(multipolygon: Multipolygon, shaped: Multipolygon) -> None:
    result = multipolygon - shaped

    assert are_polygons_sequences_equivalent(
            result, [reverse_polygon_coordinates(multipolygon)
                     for multipolygon in (
                             reverse_multipolygon_coordinates(multipolygon)
                             - reverse_shaped_coordinates(shaped)
                     )]
    )
