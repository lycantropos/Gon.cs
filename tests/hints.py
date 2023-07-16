import typing as _t
from fractions import Fraction as _Fraction

from tests.binding import (BigInteger as _BigInteger,
                           Fractions as _Fractions,
                           Multipolygon as _Multipolygon,
                           Polygon as _Polygon)

Rational = _t.Union[_BigInteger, _Fractions.Fraction]
Scalar = _t.Union[_Fraction, float, int]
Shaped = _t.Union[_Multipolygon, _Polygon]
