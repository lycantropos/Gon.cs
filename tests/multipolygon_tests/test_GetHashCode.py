from hypothesis import given

from tests.binding import Multipolygon
from tests.utils import (implication,
                         reverse_multipolygon,
                         reverse_multipolygon_polygons_borders,
                         reverse_multipolygon_polygons_holes,
                         reverse_multipolygon_polygons_holes_contours,
                         rotate_multipolygon,
                         rotate_multipolygon_polygons_borders,
                         rotate_multipolygon_polygons_holes,
                         rotate_multipolygon_polygons_holes_contours)
from . import strategies


@given(strategies.multipolygons)
def test_determinism(multipolygon: Multipolygon) -> None:
    result = hash(multipolygon)

    assert result == hash(multipolygon)


@given(strategies.multipolygons, strategies.multipolygons)
def test_surjection(first: Multipolygon, second: Multipolygon) -> None:
    assert implication(first == second, hash(first) == hash(second))


@given(strategies.multipolygons)
def test_reversals(multipolygon: Multipolygon) -> None:
    result = hash(multipolygon)

    assert result == hash(reverse_multipolygon(multipolygon))
    assert result == hash(reverse_multipolygon_polygons_borders(multipolygon))
    assert result == hash(reverse_multipolygon_polygons_holes(multipolygon))
    assert result == hash(
            reverse_multipolygon_polygons_holes_contours(multipolygon)
    )


@given(strategies.multipolygons, strategies.non_zero_integers)
def test_vertices_rotations(multipolygon: Multipolygon, offset: int) -> None:
    result = hash(multipolygon)
    assert result == hash(rotate_multipolygon(multipolygon, offset))
    assert result == hash(
            rotate_multipolygon_polygons_borders(multipolygon, offset)
    )
    assert result == hash(
            rotate_multipolygon_polygons_holes(multipolygon, offset)
    )
    assert result == hash(
            rotate_multipolygon_polygons_holes_contours(multipolygon, offset)
    )
