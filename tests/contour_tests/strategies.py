from operator import attrgetter as _attrgetter

from hypothesis import strategies as _st
from hypothesis_geometry import planar as _planar

from tests import strategies as _strategies
from tests.utils import context as _context

scalars = _strategies.scalars
contours = _planar.contours(scalars,
                            context=_context)
contours_vertices = contours.map(_attrgetter('vertices'))
non_zero_integers = _st.integers(max_value=-1) | _st.integers(1)
