from hypothesis import given

from tests.binding import Contour
from tests.utils import (implication,
                         reverse_contour,
                         rotate_contour)
from . import strategies


@given(strategies.contours)
def test_determinism(contour: Contour) -> None:
    result = hash(contour)

    assert result == hash(contour)


@given(strategies.contours, strategies.contours)
def test_surjection(first: Contour, second: Contour) -> None:
    assert implication(first == second, hash(first) == hash(second))


@given(strategies.contours)
def test_reversals(contour: Contour) -> None:
    assert hash(contour) == hash(reverse_contour(contour))


@given(strategies.contours, strategies.non_zero_integers)
def test_vertices_rotations(contour: Contour, offset: int) -> None:
    assert hash(contour) == hash(rotate_contour(contour, offset))


@given(strategies.contours, strategies.non_zero_integers)
def test_vertices_rotations_of_reversal(contour: Contour, offset: int) -> None:
    assert hash(contour) == hash(rotate_contour(reverse_contour(contour),
                                                offset))
