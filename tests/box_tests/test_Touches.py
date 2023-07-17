from hypothesis import given

from tests.binding import Box
from tests.utils import (equivalence,
                         reverse_box_coordinates)
from . import strategies


@given(strategies.boxes)
def test_irreflexivity(box: Box) -> None:
    assert not box.touches(box)


@given(strategies.boxes, strategies.boxes)
def test_symmetry(first: Box, second: Box) -> None:
    assert equivalence(first.touches(second), second.touches(first))


@given(strategies.boxes, strategies.boxes)
def test_coordinates_reversals(first: Box, second: Box) -> None:
    assert equivalence(first.touches(second),
                       reverse_box_coordinates(first).touches(
                               reverse_box_coordinates(second)
                       ))
