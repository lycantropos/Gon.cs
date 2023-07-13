from hypothesis import given

from tests.binding import Polygon
from tests.utils import (implication,
                         reverse_polygon_border,
                         reverse_polygon_holes,
                         reverse_polygon_holes_contours,
                         rotate_polygon_border,
                         rotate_polygon_holes,
                         rotate_polygon_holes_contours)
from . import strategies


@given(strategies.polygons)
def test_determinism(polygon: Polygon) -> None:
    result = hash(polygon)

    assert result == hash(polygon)


@given(strategies.polygons, strategies.polygons)
def test_surjection(first: Polygon, second: Polygon) -> None:
    assert implication(first == second, hash(first) == hash(second))


@given(strategies.polygons)
def test_reversals(polygon: Polygon) -> None:
    result = hash(polygon)

    assert result == hash(reverse_polygon_border(polygon))
    assert result == hash(reverse_polygon_holes(polygon))
    assert result == hash(reverse_polygon_holes_contours(polygon))


@given(strategies.polygons, strategies.non_zero_integers)
def test_vertices_rotations(polygon: Polygon, offset: int) -> None:
    result = hash(polygon)
    assert result == hash(rotate_polygon_border(polygon, offset))
    assert result == hash(rotate_polygon_holes(polygon, offset))
    assert result == hash(rotate_polygon_holes_contours(polygon, offset))


@given(strategies.polygons, strategies.non_zero_integers)
def test_vertices_rotations_of_reversal(polygon: Polygon, offset: int) -> None:
    assert (hash(polygon)
            == hash(rotate_polygon_border(reverse_polygon_border(polygon),
                                          offset)))
