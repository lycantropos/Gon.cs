from hypothesis import given

from tests.binding import Box
from tests.utils import (equivalence,
                         implication,
                         reverse_box_coordinates)
from . import strategies


@given(strategies.boxes)
def test_determinism(box: Box) -> None:
    result = hash(box)

    assert result == hash(box)


@given(strategies.boxes, strategies.boxes)
def test_surjection(first: Box, second: Box) -> None:
    assert implication(first == second, hash(first) == hash(second))


@given(strategies.boxes, strategies.boxes)
def test_coordinates_reversals(first: Box, second: Box) -> None:
    assert equivalence(hash(first) == hash(second),
                       (hash(reverse_box_coordinates(first))
                        == hash(reverse_box_coordinates(second))))
