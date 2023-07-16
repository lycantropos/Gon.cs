from hypothesis import given

from tests.binding import Multipolygon
from tests.utils import (equivalence,
                         implication,
                         reverse_multipolygon,
                         reverse_multipolygon_coordinates,
                         reverse_multipolygon_polygons_borders,
                         reverse_multipolygon_polygons_holes,
                         reverse_multipolygon_polygons_holes_contours,
                         rotate_multipolygon,
                         rotate_multipolygon_polygons_borders,
                         rotate_multipolygon_polygons_holes,
                         rotate_multipolygon_polygons_holes_contours)
from . import strategies


@given(strategies.multipolygons)
def test_reflexivity(multipolygon: Multipolygon) -> None:
    assert multipolygon == multipolygon


@given(strategies.multipolygons, strategies.multipolygons)
def test_symmetry(first: Multipolygon, second: Multipolygon) -> None:
    assert equivalence(first == second, second == first)


@given(strategies.multipolygons, strategies.multipolygons,
       strategies.multipolygons)
def test_transitivity(first: Multipolygon,
                      second: Multipolygon,
                      third: Multipolygon) -> None:
    assert implication(first == second and second == third, first == third)


@given(strategies.multipolygons, strategies.multipolygons)
def test_alternatives(first: Multipolygon, second: Multipolygon) -> None:
    assert equivalence(first == second, not first != second)


@given(strategies.multipolygons, strategies.multipolygons)
def test_coordinates_reversals(first: Multipolygon,
                               second: Multipolygon) -> None:
    assert equivalence(first == second,
                       (reverse_multipolygon_coordinates(first)
                        == reverse_multipolygon_coordinates(second)))


@given(strategies.multipolygons)
def test_vertices_reversals(multipolygon: Multipolygon) -> None:
    assert multipolygon == reverse_multipolygon(multipolygon)
    assert multipolygon == reverse_multipolygon_polygons_borders(multipolygon)
    assert multipolygon == reverse_multipolygon_polygons_holes(multipolygon)
    assert multipolygon == reverse_multipolygon_polygons_holes_contours(
            multipolygon
    )


@given(strategies.multipolygons, strategies.non_zero_integers)
def test_vertices_rotations(multipolygon: Multipolygon, offset: int) -> None:
    assert multipolygon == rotate_multipolygon(multipolygon, offset)
    assert multipolygon == rotate_multipolygon_polygons_borders(multipolygon,
                                                                offset)
    assert multipolygon == rotate_multipolygon_polygons_holes(multipolygon,
                                                              offset)
    assert multipolygon == rotate_multipolygon_polygons_holes_contours(
            multipolygon, offset
    )
