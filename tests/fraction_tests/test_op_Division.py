import pytest
from hypothesis import given

from tests.binding import (BigInteger,
                           Fractions,
                           System)
from tests.hints import Rational
from tests.utils import (equivalence,
                         is_fraction_valid)
from . import strategies


@given(strategies.fractions, strategies.non_zero_fractions)
def test_basic(dividend: Fractions.Fraction,
               divisor: Fractions.Fraction) -> None:
    result = dividend / divisor

    assert isinstance(result, Fractions.Fraction)
    assert is_fraction_valid(result)


@given(strategies.non_zero_fractions, strategies.non_zero_fractions)
def test_commutative_case(dividend: Fractions.Fraction,
                          divisor: Fractions.Fraction) -> None:
    assert equivalence(
            dividend / divisor == divisor / dividend,
            Fractions.Fraction.Abs(dividend) == Fractions.Fraction.Abs(divisor)
    )


@given(strategies.zero_fractions, strategies.non_zero_fractions)
def test_left_absorbing_element(dividend: Fractions.Fraction,
                                divisor: Fractions.Fraction) -> None:
    assert dividend / divisor == dividend


@given(strategies.fractions, strategies.non_zero_big_integers)
def test_big_integer_argument(dividend: Fractions.Fraction,
                              divisor: BigInteger) -> None:
    result = dividend / divisor

    assert result == dividend / Fractions.Fraction(divisor)


@given(strategies.fractions, strategies.zero_rationals)
def test_zero_divisor(dividend: Fractions.Fraction, divisor: Rational) -> None:
    with pytest.raises(System.DivideByZeroException):
        dividend / divisor
