from operator import attrgetter as _attrgetter

from hypothesis_geometry import planar as _planar

from tests import strategies as _strategies
from tests.utils import context as _context

scalars = _strategies.scalars
segments = _planar.segments(scalars,
                            context=_context)
segments_endpoints = segments.map(_attrgetter('start', 'end'))
