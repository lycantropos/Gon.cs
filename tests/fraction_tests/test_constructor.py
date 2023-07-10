import pytest
from hypothesis import given

from tests.binding import (BigInteger,
                           Fractions,
                           System)
from tests.utils import equivalence
from . import strategies


@given(strategies.numerators, strategies.denominators)
def test_basic(numerator: BigInteger, denominator: BigInteger) -> None:
    result = Fractions.Fraction(numerator, denominator)

    assert (numerator.IsZero and result.numerator.IsZero
            or (numerator % result.numerator).IsZero)
    assert (numerator.IsZero and result.numerator.IsZero
            or (denominator % result.denominator).IsZero)


def test_no_argument() -> None:
    result = Fractions.Fraction()

    assert result == Fractions.Fraction(0, 1)


@given(strategies.fractions)
def test_copy_constructor(fraction: Fractions.Fraction) -> None:
    result = Fractions.Fraction(fraction)

    assert result == fraction


@given(strategies.numerators, strategies.denominators)
def test_properties(numerator: int, denominator: int) -> None:
    result = Fractions.Fraction(numerator, denominator)

    assert equivalence(numerator.IsZero, result.numerator.IsZero)
    assert numerator * result.denominator == result.numerator * denominator
    assert result.denominator > BigInteger.Zero


@given(strategies.numerators, strategies.zero_integers)
def test_zero_denominator(numerator: BigInteger,
                          denominator: BigInteger) -> None:
    with pytest.raises(System.DivideByZeroException):
        Fractions.Fraction(numerator, denominator)
