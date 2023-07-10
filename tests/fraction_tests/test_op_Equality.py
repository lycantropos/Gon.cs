from hypothesis import given

from tests.binding import Fractions
from tests.hints import Rational
from tests.utils import (equivalence,
                         implication)
from . import strategies


@given(strategies.fractions)
def test_reflexivity(fraction: Fractions.Fraction) -> None:
    assert fraction == fraction


@given(strategies.fractions, strategies.fractions)
def test_symmetry(first: Fractions.Fraction,
                  second: Fractions.Fraction) -> None:
    assert equivalence(first == second, second == first)


@given(strategies.fractions, strategies.fractions, strategies.fractions)
def test_transitivity(first: Fractions.Fraction,
                      second: Fractions.Fraction,
                      third: Fractions.Fraction) -> None:
    assert implication(first == second and second == third, first == third)


@given(strategies.fractions, strategies.rationals)
def test_connection_with_inequality(first: Fractions.Fraction,
                                    second: Rational) -> None:
    assert equivalence(not first == second, first != second)
