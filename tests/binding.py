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

for _file in Path(__file__).parent.glob('*.dll'):
    System.Reflection.Assembly.LoadFile(str(_file))

import Fractions
import Gon

_Scalar = t.Union[Fraction, float, int]


class Location(enum.IntEnum):
    EXTERIOR = -1
    BOUNDARY = 0
    INTERIOR = 1


class Orientation(enum.IntEnum):
    CLOCKWISE = -1
    COLLINEAR = 0
    COUNTERCLOCKWISE = 1


class Box:
    @property
    def max_x(self) -> Fraction:
        return _fraction_from_raw(self._raw.MaxX)

    @property
    def max_y(self) -> Fraction:
        return _fraction_from_raw(self._raw.MaxY)

    @property
    def min_x(self) -> Fraction:
        return _fraction_from_raw(self._raw.MinX)

    @property
    def min_y(self) -> Fraction:
        return _fraction_from_raw(self._raw.MinY)

    def disjoint_with(self, other: Box) -> bool:
        return self._raw.DisjointWith(other._raw)

    def touches(self, other: Box) -> bool:
        return self._raw.Touches(other._raw)

    _raw: Gon.Box[Fractions.Fraction]

    def __new__(
            cls, min_x: _Scalar, max_x: _Scalar, min_y: _Scalar, max_y: _Scalar
    ) -> te.Self:
        self = super().__new__(cls)
        self._raw = Gon.Box[Fractions.Fraction](
                _fraction_to_raw(Fraction(min_x)),
                _fraction_to_raw(Fraction(max_x)),
                _fraction_to_raw(Fraction(min_y)),
                _fraction_to_raw(Fraction(max_y)),
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
                if isinstance(other, Box)
                else NotImplemented)

    def __hash__(self) -> int:
        result = self._raw.GetHashCode()
        return result - (result == -1)

    @t.overload
    def __ne__(self, other: te.Self) -> bool:
        ...

    @t.overload
    def __ne__(self, other: t.Any) -> t.Any:
        ...

    def __ne__(self, other: t.Any) -> t.Any:
        return (self._raw != other._raw
                if isinstance(other, Box)
                else NotImplemented)

    def __repr__(self) -> str:
        return (f'{type(self).__qualname__}({self.min_x!r}, {self.max_x!r}, '
                f'{self.min_y!r}, {self.max_y!r})')

    def __str__(self) -> str:
        return (f'{type(self).__qualname__}({self.min_x}, {self.max_x}, '
                f'{self.min_y}, {self.max_y})')


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
        result = self._raw.GetHashCode()
        return result - (result == -1)

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
    def bounding_box(self) -> Box:
        return _box_from_raw(self._raw.BoundingBox)

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

    def __contains__(self, point: Point) -> bool:
        return self._raw.Contains(_point_to_raw(point))

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

    def locate(self, point: Point) -> Location:
        return _location_from_raw(self._raw.Locate(_point_to_raw(point)))

    _raw: Gon.Polygon[Fractions.Fraction]

    def __new__(cls,
                border: Contour,
                holes: t.Optional[t.Sequence[Contour]] = None) -> te.Self:
        self = super().__new__(cls)
        border = _contour_to_raw(border)
        self._raw = (
            Gon.Polygon[Fractions.Fraction](border)
            if holes is None
            else Gon.Polygon[Fractions.Fraction](
                    border, System.Array[Gon.Contour[Fractions.Fraction]](
                            [_contour_to_raw(hole) for hole in holes]
                    )
            )
        )
        return self

    @t.overload
    def __and__(self,
                other: t.Union[te.Self, Multipolygon]) -> t.List[Polygon]:
        ...

    @t.overload
    def __and__(self, other: t.Any) -> t.Any:
        ...

    def __and__(self, other: t.Any) -> t.Any:
        return ([_polygon_from_raw(raw_polygon)
                 for raw_polygon in self._raw & other._raw]
                if isinstance(other, Polygon)
                else
                ([_polygon_from_raw(raw_polygon)
                  for raw_polygon in self._raw & _multipolygon_to_raw(other)]
                 if isinstance(other, Multipolygon)
                 else NotImplemented))

    def __contains__(self, point: Point) -> bool:
        return self._raw.Contains(_point_to_raw(point))

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

    @t.overload
    def __or__(self, other: t.Union[te.Self, Multipolygon]) -> t.List[Polygon]:
        ...

    @t.overload
    def __or__(self, other: t.Any) -> t.Any:
        ...

    def __or__(self, other: t.Any) -> t.Any:
        return ([_polygon_from_raw(raw_polygon)
                 for raw_polygon in self._raw | other._raw]
                if isinstance(other, Polygon)
                else
                ([_polygon_from_raw(raw_polygon)
                  for raw_polygon in self._raw | _multipolygon_to_raw(other)]
                 if isinstance(other, Multipolygon)
                 else NotImplemented))

    def __repr__(self) -> str:
        return f'{type(self).__qualname__}({self.border!r}, {self.holes!r})'

    def __str__(self) -> str:
        return (f'{type(self).__qualname__}({self.border}, '
                f'[{", ".join(map(str, self.holes))}])')

    @t.overload
    def __sub__(self,
                other: t.Union[te.Self, Multipolygon]) -> t.List[Polygon]:
        ...

    @t.overload
    def __sub__(self, other: t.Any) -> t.Any:
        ...

    def __sub__(self, other: t.Any) -> t.Any:
        return ([_polygon_from_raw(raw_polygon)
                 for raw_polygon in self._raw - other._raw]
                if isinstance(other, Polygon)
                else
                ([_polygon_from_raw(raw_polygon)
                  for raw_polygon in self._raw - _multipolygon_to_raw(other)]
                 if isinstance(other, Multipolygon)
                 else NotImplemented))

    @t.overload
    def __xor__(self,
                other: t.Union[te.Self, Multipolygon]) -> t.List[Polygon]:
        ...

    @t.overload
    def __xor__(self, other: t.Any) -> t.Any:
        ...

    def __xor__(self, other: t.Any) -> t.Any:
        return ([_polygon_from_raw(raw_polygon)
                 for raw_polygon in self._raw ^ other._raw]
                if isinstance(other, Polygon)
                else
                ([_polygon_from_raw(raw_polygon)
                  for raw_polygon in self._raw ^ _multipolygon_to_raw(other)]
                 if isinstance(other, Multipolygon)
                 else NotImplemented))


class Multipolygon:
    @property
    def polygons(self) -> t.Sequence[Polygon]:
        return [_polygon_from_raw(polygon) for polygon in self._raw.Polygons]

    def locate(self, point: Point) -> Location:
        return _location_from_raw(self._raw.Locate(_point_to_raw(point)))

    _raw: Gon.Multipolygon[Fractions.Fraction]

    def __new__(cls, polygons: t.Sequence[Polygon]) -> te.Self:
        self = super().__new__(cls)
        self._raw = Gon.Multipolygon[Fractions.Fraction](
                System.Array[Gon.Polygon[Fractions.Fraction]](
                        [_polygon_to_raw(polygon) for polygon in polygons]
                )
        )
        return self

    @t.overload
    def __and__(self,
                other: t.Union[te.Self, Multipolygon]) -> t.List[Polygon]:
        ...

    @t.overload
    def __and__(self, other: t.Any) -> t.Any:
        ...

    def __and__(self, other: t.Any) -> t.Any:
        return ([_polygon_from_raw(raw_polygon)
                 for raw_polygon in self._raw & other._raw]
                if isinstance(other, Multipolygon)
                else
                ([_polygon_from_raw(raw_polygon)
                  for raw_polygon in self._raw & _polygon_to_raw(other)]
                 if isinstance(other, Polygon)
                 else NotImplemented))

    def __contains__(self, point: Point) -> bool:
        return self._raw.Contains(_point_to_raw(point))

    @t.overload
    def __eq__(self, other: te.Self) -> bool:
        ...

    @t.overload
    def __eq__(self, other: t.Any) -> t.Any:
        ...

    def __eq__(self, other: t.Any) -> t.Any:
        return (self._raw == other._raw
                if isinstance(other, Multipolygon)
                else NotImplemented)

    def __hash__(self) -> int:
        result = int(self._raw.GetHashCode())
        return result - (result == -1)

    @t.overload
    def __or__(self, other: t.Union[te.Self, Multipolygon]) -> t.List[Polygon]:
        ...

    @t.overload
    def __or__(self, other: t.Any) -> t.Any:
        ...

    def __or__(self, other: t.Any) -> t.Any:
        return ([_polygon_from_raw(raw_polygon)
                 for raw_polygon in self._raw | other._raw]
                if isinstance(other, Multipolygon)
                else
                ([_polygon_from_raw(raw_polygon)
                  for raw_polygon in self._raw | _polygon_to_raw(other)]
                 if isinstance(other, Polygon)
                 else NotImplemented))

    def __repr__(self) -> str:
        return f'{type(self).__qualname__}({self.polygons!r})'

    def __str__(self) -> str:
        return (f'{type(self).__qualname__}'
                f'([{", ".join(map(str, self.polygons))}])')

    @t.overload
    def __sub__(self,
                other: t.Union[te.Self, Multipolygon]) -> t.List[Polygon]:
        ...

    @t.overload
    def __sub__(self, other: t.Any) -> t.Any:
        ...

    def __sub__(self, other: t.Any) -> t.Any:
        return ([_polygon_from_raw(raw_polygon)
                 for raw_polygon in self._raw - other._raw]
                if isinstance(other, Multipolygon)
                else
                ([_polygon_from_raw(raw_polygon)
                  for raw_polygon in self._raw - _polygon_to_raw(other)]
                 if isinstance(other, Polygon)
                 else NotImplemented))

    @t.overload
    def __xor__(self,
                other: t.Union[te.Self, Multipolygon]) -> t.List[Polygon]:
        ...

    @t.overload
    def __xor__(self, other: t.Any) -> t.Any:
        ...

    def __xor__(self, other: t.Any) -> t.Any:
        return ([_polygon_from_raw(raw_polygon)
                 for raw_polygon in self._raw ^ other._raw]
                if isinstance(other, Multipolygon)
                else
                ([_polygon_from_raw(raw_polygon)
                  for raw_polygon in self._raw ^ _polygon_to_raw(other)]
                 if isinstance(other, Polygon)
                 else NotImplemented))


def _box_from_raw(value: Gon.Box[Fractions.Fraction]) -> Box:
    return Box(_fraction_from_raw(value.MinX), _fraction_from_raw(value.MaxX),
               _fraction_from_raw(value.MinY), _fraction_from_raw(value.MaxY))


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


def _location_from_raw(value: Gon.Location) -> Location:
    return (Location.EXTERIOR
            if value == Gon.Location.Exterior
            else (Location.BOUNDARY
                  if value == Gon.Location.Boundary
                  else Location.INTERIOR))


def _multipolygon_to_raw(
        value: Multipolygon
) -> Gon.Multipolygon[Fractions.Fraction]:
    return Gon.Multipolygon[Fractions.Fraction](
            System.Array[Gon.Polygon[Fractions.Fraction]](
                    [_polygon_to_raw(polygon) for polygon in value.polygons]
            )
    )


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


def _polygon_from_raw(value: Gon.Polygon[Fractions.Fraction]) -> Polygon:
    return Polygon(_contour_from_raw(value.Border),
                   [_contour_from_raw(hole) for hole in value.Holes])


def _polygon_to_raw(value: Polygon) -> Gon.Polygon[Fractions.Fraction]:
    return Gon.Polygon[Fractions.Fraction](
            _contour_to_raw(value.border),
            System.Array[Gon.Contour[Fractions.Fraction]](
                    [_contour_to_raw(hole) for hole in value.holes]
            )
    )


def _segment_from_raw(value: Gon.Segment[Fractions.Fraction]) -> Segment:
    return Segment(_point_from_raw(value.Start), _point_from_raw(value.End))
