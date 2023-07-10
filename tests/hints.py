import typing as _t
from fractions import Fraction as _Fraction

from tests.binding import (BigInteger as _BigInteger,
                           Fractions as _Fractions)

Rational = _t.Union[_BigInteger, _Fractions.Fraction]
Scalar = _t.Union[_Fraction, float, int]
