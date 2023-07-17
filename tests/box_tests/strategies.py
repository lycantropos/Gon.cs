from hypothesis_geometry import planar as _planar

from tests import strategies as _strategies
from tests.utils import context as _context

scalars = _strategies.scalars
boxes = _planar.boxes(scalars,
                      context=_context)
