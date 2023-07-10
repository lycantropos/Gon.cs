from hypothesis import given

from tests.binding import Fractions
from tests.hints import Rational
from tests.utils import (equivalence,
                         is_fraction_valid)
from . import strategies


@given(strategies.fractions)
def test_basic(fraction: Fractions.Fraction) -> None:
    result = -fraction

    assert isinstance(result, Fractions.Fraction)
    assert is_fraction_valid(result)


@given(strategies.fractions)
def test_fixed_point(fraction: Fractions.Fraction) -> None:
    result = -fraction

    assert equivalence(fraction == result, fraction.IsZero)


@given(strategies.fractions)
def test_involution(fraction: Fractions.Fraction) -> None:
    result = -fraction

    assert -result == fraction


@given(strategies.fractions, strategies.rationals)
def test_op_Addition_operand(first: Fractions.Fraction,
                             second: Rational) -> None:
    assert -(first + second) == (-first) + (-second)


@given(strategies.fractions, strategies.rationals)
def test_op_Subtraction_operand(first: Fractions.Fraction,
                                second: Rational) -> None:
    assert -(first - second) == (-first) - (-second)


@given(strategies.fractions, strategies.rationals)
def test_op_Multiply_operand(first: Fractions.Fraction,
                             second: Rational) -> None:
    assert -(first * second) == (-first) * second == first * (-second)


@given(strategies.fractions, strategies.non_zero_rationals)
def test_op_Division_operand(first: Fractions.Fraction,
                             second: Rational) -> None:
    assert -(first / second) == (-first) / second == first / (-second)
