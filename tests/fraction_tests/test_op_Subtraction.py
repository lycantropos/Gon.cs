from hypothesis import given

from tests.binding import (BigInteger,
                           Fractions)
from tests.hints import Rational
from tests.utils import (equivalence,
                         is_fraction_valid)
from . import strategies


@given(strategies.fractions, strategies.rationals)
def test_basic(first: Fractions.Fraction, second: Rational) -> None:
    result = first - second

    assert isinstance(result, Fractions.Fraction)
    assert is_fraction_valid(result)


@given(strategies.fractions)
def test_diagonal(fraction: Fractions.Fraction) -> None:
    assert (fraction - fraction).IsZero


@given(strategies.fractions, strategies.fractions)
def test_commutative_case(first: Fractions.Fraction,
                          second: Fractions.Fraction) -> None:
    assert equivalence(first - second == second - first, first == second)


@given(strategies.fractions, strategies.zero_rationals)
def test_right_neutral_element(first: Fractions.Fraction,
                               second: Fractions.Fraction) -> None:
    assert first - second == first


@given(strategies.fractions, strategies.big_integers)
def test_integer_argument(first: Fractions.Fraction,
                          second: BigInteger) -> None:
    result = first - second

    assert result == first - Fractions.Fraction(second)


@given(strategies.fractions, strategies.fractions)
def test_alternatives(first: Fractions.Fraction,
                      second: Fractions.Fraction) -> None:
    result = first - second

    assert result == first + (-second)
