from hypothesis import given

from tests.binding import Fractions
from tests.utils import (equivalence,
                         implication)
from . import strategies


@given(strategies.fractions)
def test_reflexivity(fraction: Fractions.Fraction) -> None:
    assert fraction <= fraction


@given(strategies.fractions, strategies.fractions)
def test_antisymmetry(first: Fractions.Fraction,
                      second: Fractions.Fraction) -> None:
    assert equivalence(first <= second <= first, first == second)


@given(strategies.fractions, strategies.fractions, strategies.fractions)
def test_transitivity(first: Fractions.Fraction,
                      second: Fractions.Fraction,
                      third: Fractions.Fraction) -> None:
    assert implication(first <= second <= third, first <= third)


@given(strategies.fractions, strategies.fractions)
def test_equivalents(first: Fractions.Fraction,
                     second: Fractions.Fraction) -> None:
    result = first <= second

    assert equivalence(result, second >= first)
    assert equivalence(result, first < second or first == second)
    assert equivalence(result, second > first or first == second)
