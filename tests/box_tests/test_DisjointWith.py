from hypothesis import given

from tests.binding import Box
from tests.utils import (equivalence,
                         reverse_box_coordinates)
from . import strategies


@given(strategies.boxes)
def test_irreflexivity(box: Box) -> None:
    assert not box.disjoint_with(box)


@given(strategies.boxes, strategies.boxes)
def test_symmetry(first: Box, second: Box) -> None:
    assert equivalence(first.disjoint_with(second),
                       second.disjoint_with(first))


@given(strategies.boxes, strategies.boxes)
def test_coordinates_reversals(first: Box, second: Box) -> None:
    assert equivalence(first.disjoint_with(second),
                       reverse_box_coordinates(first).disjoint_with(
                               reverse_box_coordinates(second)
                       ))
