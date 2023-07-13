from hypothesis_geometry import planar as _planar

from tests import strategies as _strategies
from tests.utils import context as _context

scalars = _strategies.scalars
polygons = _planar.polygons(scalars,
                            context=_context)
non_zero_integers = _strategies.non_zero_integers
