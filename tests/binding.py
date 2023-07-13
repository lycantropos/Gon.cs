from __future__ import annotations

import enum
import typing as t
from fractions import Fraction
from pathlib import Path

import pythonnet
import typing_extensions as te

pythonnet.load('coreclr')
import System
from System.Numerics import BigInteger

_dlls_directory = Path(__file__).parent

System.Reflection.Assembly.LoadFile(str(_dlls_directory / 'Fractions.dll'))
import Fractions

System.Reflection.Assembly.LoadFile(str(_dlls_directory / 'Gon.dll'))
import Gon

_Scalar = t.Union[Fraction, float, int]


class Orientation(enum.IntEnum):
    CLOCKWISE = -1
    COLLINEAR = 0
    COUNTERCLOCKWISE = 1


class Point:
    @property
    def x(self) -> Fraction:
        return _fraction_from_raw(self._raw.X)

    @property
    def y(self) -> Fraction:
        return _fraction_from_raw(self._raw.Y)

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

    @t.overload
    def __ge__(self, other: te.Self) -> bool:
        ...

    @t.overload
    def __ge__(self, other: t.Any) -> t.Any:
        ...

    def __ge__(self, other: t.Any) -> t.Any:
        return (self._raw >= other._raw
                if isinstance(other, Point)
                else NotImplemented)

    @t.overload
    def __gt__(self, other: te.Self) -> bool:
        ...

    @t.overload
    def __gt__(self, other: t.Any) -> t.Any:
        ...

    def __gt__(self, other: t.Any) -> t.Any:
        return (self._raw > other._raw
                if isinstance(other, Point)
                else NotImplemented)

    def __hash__(self) -> int:
        return hash((self.x, self.y))

    @t.overload
    def __le__(self, other: te.Self) -> bool:
        ...

    @t.overload
    def __le__(self, other: t.Any) -> t.Any:
        ...

    def __le__(self, other: t.Any) -> t.Any:
        return (self._raw <= other._raw
                if isinstance(other, Point)
                else NotImplemented)

    @t.overload
    def __lt__(self, other: te.Self) -> bool:
        ...

    @t.overload
    def __lt__(self, other: t.Any) -> t.Any:
        ...

    def __lt__(self, other: t.Any) -> t.Any:
        return (self._raw < other._raw
                if isinstance(other, Point)
                else NotImplemented)

    @t.overload
    def __ne__(self, other: te.Self) -> bool:
        ...

    @t.overload
    def __ne__(self, other: t.Any) -> t.Any:
        ...

    def __ne__(self, other: t.Any) -> t.Any:
        return (self._raw != other._raw
                if isinstance(other, Point)
                else NotImplemented)

    def __repr__(self) -> str:
        return f'{type(self).__qualname__}({self.x!r}, {self.y!r})'

    def __str__(self) -> str:
        return f'{type(self).__qualname__}({self.x}, {self.y})'


class Segment:
    @property
    def end(self) -> Point:
        return _point_from_raw(self._raw.End)

    @property
    def start(self) -> Point:
        return _point_from_raw(self._raw.Start)

    _raw: Gon.Segment[Fractions.Fraction]

    def __new__(cls, start: Point, end: Point) -> te.Self:
        self = super().__new__(cls)
        self._raw = Gon.Segment[Fractions.Fraction](
                _point_to_raw(start), _point_to_raw(end)
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
                if isinstance(other, Segment)
                else NotImplemented)

    @t.overload
    def __ne__(self, other: te.Self) -> bool:
        ...

    @t.overload
    def __ne__(self, other: t.Any) -> t.Any:
        ...

    def __ne__(self, other: t.Any) -> t.Any:
        return (self._raw != other._raw
                if isinstance(other, Segment)
                else NotImplemented)

    def __repr__(self) -> str:
        return f'{type(self).__qualname__}({self.start!r}, {self.end!r})'

    def __str__(self) -> str:
        return f'{type(self).__qualname__}({self.start}, {self.end})'


class Contour:
    @property
    def segments(self) -> t.Sequence[Segment]:
        return [_segment_from_raw(segment) for segment in self._raw.Segments]

    @property
    def vertices(self) -> t.Sequence[Point]:
        return [_point_from_raw(vertex) for vertex in self._raw.Vertices]

    _raw: Gon.Contour[Fractions.Fraction]

    def __new__(cls, vertices: t.Sequence[Point]) -> te.Self:
        self = super().__new__(cls)
        self._raw = Gon.Contour[Fractions.Fraction](
                System.Array[Gon.Point[Fractions.Fraction]](
                        [_point_to_raw(vertex) for vertex in vertices]
                )
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
                if isinstance(other, Contour)
                else NotImplemented)

    def __hash__(self) -> int:
        result = int(self._raw.GetHashCode())
        return result - (result == -1)

    def __repr__(self) -> str:
        return f'{type(self).__qualname__}({self.vertices!r})'

    def __str__(self) -> str:
        return (f'{type(self).__qualname__}'
                f'([{", ".join(map(str, self.vertices))}])')


class Polygon:
    @property
    def border(self) -> Contour:
        return _contour_from_raw(self._raw.Border)

    @property
    def holes(self) -> t.Sequence[Contour]:
        return [_contour_from_raw(hole) for hole in self._raw.Holes]

    @property
    def segments(self) -> t.Sequence[Segment]:
        return [_segment_from_raw(segment) for segment in self._raw.Segments]

    _raw: Gon.Polygon[Fractions.Fraction]

    def __new__(cls, border: Contour, holes: t.Sequence[Contour]) -> te.Self:
        self = super().__new__(cls)
        self._raw = Gon.Polygon[Fractions.Fraction](
                _contour_to_raw(border),
                System.Array[Gon.Contour[Fractions.Fraction]](
                        [_contour_to_raw(hole) for hole in holes]
                )
        )
        return self

    @t.overload
    def __and__(self, other: te.Self) -> t.List[Polygon]:
        ...

    @t.overload
    def __and__(self, other: t.Any) -> t.Any:
        ...

    def __and__(self, other: t.Any) -> t.Any:
        return ([_polygon_from_raw(raw_polygon)
                 for raw_polygon in self._raw & other._raw]
                if isinstance(other, Polygon)
                else NotImplemented)

    @t.overload
    def __eq__(self, other: te.Self) -> bool:
        ...

    @t.overload
    def __eq__(self, other: t.Any) -> t.Any:
        ...

    def __eq__(self, other: t.Any) -> t.Any:
        return (self._raw == other._raw
                if isinstance(other, Polygon)
                else NotImplemented)

    def __hash__(self) -> int:
        result = int(self._raw.GetHashCode())
        return result - (result == -1)

    def __repr__(self) -> str:
        return f'{type(self).__qualname__}({self.border!r}, {self.holes!r})'

    def __str__(self) -> str:
        return (f'{type(self).__qualname__}({self.border}, '
                f'[{", ".join(map(str, self.holes))}])')


def cross_multiply(first_start: Point,
                   first_end: Point,
                   second_start: Point,
                   second_end: Point) -> Fraction:
    return _fraction_from_raw(
            Gon.CrossMultiplier[Fractions.Fraction].CrossMultiply(
                    _point_to_raw(first_start), _point_to_raw(first_end),
                    _point_to_raw(second_start), _point_to_raw(second_end)
            )
    )


def orient(vertex: Point,
           first_ray_point: Point,
           second_ray_point: Point) -> Orientation:
    return _orientation_from_raw(
            Gon.Orienteer[Fractions.Fraction].Orient(
                    _point_to_raw(vertex), _point_to_raw(first_ray_point),
                    _point_to_raw(second_ray_point)
            )
    )


def _polygon_from_raw(value: Gon.Polygon[Fractions.Fraction]) -> Polygon:
    return Polygon(_contour_from_raw(value.Border),
                   [_contour_from_raw(hole) for hole in value.Holes])


def _contour_from_raw(value: Gon.Contour[Fractions.Fraction]) -> Contour:
    return Contour([_point_from_raw(vertex) for vertex in value.Vertices])


def _contour_to_raw(value: Contour) -> Gon.Contour[Fractions.Fraction]:
    return Gon.Contour[Fractions.Fraction](
            System.Array[Gon.Point[Fractions.Fraction]](
                    [_point_to_raw(vertex) for vertex in value.vertices]
            )
    )


def _fraction_from_raw(value: Fractions.Fraction) -> Fraction:
    return Fraction(_int_from_raw(value.numerator),
                    _int_from_raw(value.denominator))


def _fraction_to_raw(value: Fraction) -> Fractions.Fraction:
    return Fractions.Fraction(value.numerator, value.denominator)


def _int_from_raw(value: BigInteger) -> int:
    return int.from_bytes(value.ToByteArray(False, False), 'little',
                          signed=True)


def _orientation_from_raw(value: Gon.Orientation) -> Orientation:
    return (Orientation.CLOCKWISE
            if value == Gon.Orientation.Clockwise
            else (Orientation.COLLINEAR
                  if value == Gon.Orientation.Collinear
                  else Orientation.COUNTERCLOCKWISE))


def _point_from_raw(value: Gon.Point[Fractions.Fraction]) -> Point:
    return Point(_fraction_from_raw(value.X), _fraction_from_raw(value.Y))


def _point_to_raw(value: Point) -> Gon.Point[Fractions.Fraction]:
    return Gon.Point[Fractions.Fraction](_fraction_to_raw(value.x),
                                         _fraction_to_raw(value.y))


def _segment_from_raw(value: Gon.Segment[Fractions.Fraction]) -> Segment:
    return Segment(_point_from_raw(value.Start), _point_from_raw(value.End))
