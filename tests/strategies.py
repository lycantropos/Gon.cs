from hypothesis import strategies as _st
from hypothesis_geometry import planar as _planar

from tests.utils import context as _context

scalars = _st.integers() | _st.fractions() | _st.floats(allow_infinity=False,
                                                        allow_nan=False)
points = _planar.points(scalars,
                        context=_context)
