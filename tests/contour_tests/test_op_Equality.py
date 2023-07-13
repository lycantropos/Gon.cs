from hypothesis import given

from tests.binding import Contour
from tests.utils import (equivalence,
                         implication)
from . import strategies


@given(strategies.contours)
def test_reflexivity(contour: Contour) -> None:
    assert contour == contour


@given(strategies.contours, strategies.contours)
def test_symmetry(first: Contour, second: Contour) -> None:
    assert equivalence(first == second, second == first)


@given(strategies.contours, strategies.contours, strategies.contours)
def test_transitivity(first: Contour, second: Contour, third: Contour) -> None:
    assert implication(first == second and second == third, first == third)


@given(strategies.contours, strategies.contours)
def test_alternatives(first: Contour, second: Contour) -> None:
    assert equivalence(first == second, not first != second)
