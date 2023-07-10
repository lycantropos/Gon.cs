from hypothesis import given

from tests.binding import (BigInteger,
                           Fractions)
from tests.utils import is_fraction_valid
from . import strategies


@given(strategies.fractions, strategies.fractions)
def test_basic(first: Fractions.Fraction, second: Fractions.Fraction) -> None:
    result = first * second

    assert isinstance(result, Fractions.Fraction)
    assert is_fraction_valid(result)


@given(strategies.fractions, strategies.fractions)
def test_commutativity(first: Fractions.Fraction,
                       second: Fractions.Fraction) -> None:
    assert first * second == second * first


@given(strategies.fractions, strategies.zero_fractions)
def test_absorbing_element(first: Fractions.Fraction,
                           second: Fractions.Fraction) -> None:
    assert first * second == second == second * first


@given(strategies.fractions, strategies.fractions, strategies.fractions)
def test_associativity(first: Fractions.Fraction,
                       second: Fractions.Fraction,
                       third: Fractions.Fraction) -> None:
    assert (first * second) * third == first * (second * third)


@given(strategies.fractions, strategies.big_integers)
def test_integer_argument(first: Fractions.Fraction,
                          second: BigInteger) -> None:
    result = first * second

    assert result == first * Fractions.Fraction(second)
