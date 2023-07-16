from operator import attrgetter as _attrgetter

from hypothesis_geometry import planar as _planar

from tests import strategies as _strategies
from tests.utils import context as _context

scalars = _strategies.scalars
contours = _planar.contours(scalars,
                            context=_context)
polygons = _planar.polygons(scalars,
                            context=_context)
shaped_geometries = polygons | _planar.multipolygons(scalars,
                                                     context=_context)
borders_with_holes = polygons.map(_attrgetter('border', 'holes'))
non_zero_integers = _strategies.non_zero_integers
