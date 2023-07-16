from hypothesis import given

from tests.binding import Polygon
from tests.utils import (equivalence,
                         implication,
                         reverse_polygon_border,
                         reverse_polygon_coordinates,
                         reverse_polygon_holes,
                         reverse_polygon_holes_contours,
                         rotate_polygon_border,
                         rotate_polygon_holes,
                         rotate_polygon_holes_contours)
from . import strategies


@given(strategies.polygons)
def test_reflexivity(polygon: Polygon) -> None:
    assert polygon == polygon


@given(strategies.polygons, strategies.polygons)
def test_symmetry(first: Polygon, second: Polygon) -> None:
    assert equivalence(first == second, second == first)


@given(strategies.polygons, strategies.polygons, strategies.polygons)
def test_transitivity(first: Polygon, second: Polygon, third: Polygon) -> None:
    assert implication(first == second and second == third, first == third)


@given(strategies.polygons, strategies.polygons)
def test_alternatives(first: Polygon, second: Polygon) -> None:
    assert equivalence(first == second, not first != second)


@given(strategies.polygons, strategies.polygons)
def test_coordinates_reversals(first: Polygon, second: Polygon) -> None:
    assert equivalence(first == second,
                       (reverse_polygon_coordinates(first)
                        == reverse_polygon_coordinates(second)))


@given(strategies.polygons)
def test_vertices_reversals(polygon: Polygon) -> None:
    assert polygon == reverse_polygon_border(polygon)
    assert polygon == reverse_polygon_holes(polygon)
    assert polygon == reverse_polygon_holes_contours(polygon)


@given(strategies.polygons, strategies.non_zero_integers)
def test_vertices_rotations(polygon: Polygon, offset: int) -> None:
    assert polygon == rotate_polygon_border(polygon, offset)
    assert polygon == rotate_polygon_holes(polygon, offset)
    assert polygon == rotate_polygon_holes_contours(polygon, offset)
