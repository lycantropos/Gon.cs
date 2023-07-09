from __future__ import annotations

import typing as t
from fractions import Fraction
from pathlib import Path

import pythonnet
import typing_extensions as te

pythonnet.load()
from System import (Array,
                    Reflection)
from System.Numerics import BigInteger

_dlls_directory = Path(__file__).parent

Reflection.Assembly.LoadFile(str(_dlls_directory / 'Fractions.dll'))
import Fractions

Reflection.Assembly.LoadFile(str(_dlls_directory / 'Gon.dll'))
import Gon

_Scalar = t.Union[Fraction, float, int]


class Point:
    @property
    def x(self) -> Fraction:
        return _fraction_from_raw(self._raw.x)

    @property
    def y(self) -> Fraction:
        return _fraction_from_raw(self._raw.y)

    _raw: Gon.Point[Fractions.Fraction]

    def __new__(cls, x: _Scalar, y: _Scalar) -> te.Self:
        self = super().__new__(cls)
        self._raw = Gon.Point[Fractions.Fraction](
                _fraction_to_raw(Fraction(x)), _fraction_to_raw(Fraction(y))
        )
        return self

    @t.overload
    def __eq__(self, other: te.Self) -> bool:
        ...

    @t.overload
    def __eq__(self, other: t.Any) -> t.Any:
        ...

    def __eq__(self, other: t.Any) -> t.Any:
        return (self._raw == other._raw
                if isinstance(other, Point)
                else NotImplemented)

    def __repr__(self) -> str:
        return f'{type(self).__qualname__}({self.x!r}, {self.y!r})'

    def __str__(self) -> str:
        return f'{type(self).__qualname__}({self.x}, {self.y})'


class Segment:
    @property
    def end(self) -> Point:
        return _point_from_raw(self._raw.end)

    @property
    def start(self) -> Point:
        return _point_from_raw(self._raw.start)

    _raw: Gon.Segment[Fractions.Fraction]

    def __new__(cls, start: Point, end: Point) -> te.Self:
        self = super().__new__(cls)
        self._raw = Gon.Segment[Fractions.Fraction](
                _point_to_raw(start), _point_to_raw(end)
        )
        return self

    def __repr__(self) -> str:
        return f'{type(self).__qualname__}({self.start!r}, {self.end!r})'

    def __str__(self) -> str:
        return f'{type(self).__qualname__}({self.start}, {self.end})'


class Contour:
    @property
    def vertices(self) -> t.Sequence[Point]:
        return [_point_from_raw(vertex) for vertex in self._raw.vertices]

    _raw: Gon.Contour[Fractions.Fraction]

    def __new__(cls, vertices: t.Sequence[Point]) -> te.Self:
        self = super().__new__(cls)
        self._raw = Gon.Contour[Fractions.Fraction](
                Array[Gon.Point[Fractions.Fraction]](
                        [_point_to_raw(vertex) for vertex in vertices]
                )
        )
        return self

    def __repr__(self) -> str:
        return f'{type(self).__qualname__}({self.vertices!r})'

    def __str__(self) -> str:
        return (f'{type(self).__qualname__}'
                f'([{", ".join(map(str, self.vertices))}])')


def _fraction_to_raw(value: Fraction) -> Fractions.Fraction:
    return Fractions.Fraction(value.numerator, value.denominator)


def _fraction_from_raw(value: Fractions.Fraction) -> Fraction:
    return Fraction(_int_from_raw(value.numerator),
                    _int_from_raw(value.denominator))


def _int_from_raw(value: BigInteger) -> int:
    return int(value.ToString())


def _int_from_raw2(value: BigInteger) -> int:
    return int.from_bytes(value.ToByteArray(False, False), 'little',
                          signed=True)


def _point_to_raw(value: Point) -> Gon.Point[Fractions.Fraction]:
    return Gon.Point[Fractions.Fraction](_fraction_to_raw(value.x),
                                         _fraction_to_raw(value.y))


def _point_from_raw(value: Gon.Point[Fractions.Fraction]) -> Point:
    return Point(_fraction_from_raw(value.x), _fraction_from_raw(value.y))
