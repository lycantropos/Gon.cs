from tests.binding import GonExamples


def test_basic() -> None:
    result = GonExamples.Basic.RunExamples()

    assert result is None


def test_polygons_without_holes_binary_operations() -> None:
    result = GonExamples.PolygonsWithoutHolesBinaryOperations.RunExamples()

    assert result is None
