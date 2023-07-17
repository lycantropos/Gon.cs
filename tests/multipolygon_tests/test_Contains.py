from hypothesis import given

from tests.binding import Multipolygon
from . import strategies


@given(strategies.multipolygons)
def test_vertices(multipolygon: Multipolygon) -> None:
    assert all(vertex in multipolygon
               for polygon in multipolygon.polygons
               for vertex in polygon.border.vertices)
    assert all(vertex in multipolygon
               for polygon in multipolygon.polygons
               for hole in polygon.holes
               for vertex in hole.vertices)
