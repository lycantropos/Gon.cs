Gon.cs
======

[![](https://github.com/lycantropos/Gon.cs/workflows/CI/badge.svg)](https://github.com/lycantropos/Gon.cs/actions/workflows/ci.yml "Github Actions")
[![](https://img.shields.io/github/license/lycantropos/Gon.cs.svg)](https://github.com/lycantropos/Gon.cs/blob/master/LICENSE "License")
[![](https://img.shields.io/nuget/v/Gon.svg?style=flat-square)](https://www.nuget.org/packages/Gon/ "NuGet")

Installation
------------

Install the latest
[`.NET SDK`](https://learn.microsoft.com/en-us/dotnet/core/sdk#how-to-install-the-net-sdk).

### User

Download and install the latest stable version from `NuGet` repository

```bash
dotnet add package Gon
```

### Developer

Download the latest version from `GitHub` repository

```bash
git clone https://github.com/lycantropos/Gon.cs
```

Install

```bash
dotnet add reference Gon.cs/src/Gon/Gon.csproj
```

Usage
-----

```cs
using System.Diagnostics;

using Point = Gon.Point<double>;
using Contour = Gon.Contour<double>;
using Polygon = Gon.Polygon<double>;
using Location = Gon.Location;

public static class Basic
{
    public static void RunExamples()
    {
        // construction
        var squareBorder = new Contour(
            new[] { new Point(0, 0), new Point(4, 0), new Point(4, 4), new Point(0, 4) }
        );
        var square = new Polygon(squareBorder);

        // accessing various properties
        Debug.Assert(square.Border == squareBorder);
        Debug.Assert(square.Border.Vertices.Length == 4);
        Debug.Assert(square.Holes.Length == 0);

        // equality checks
        Debug.Assert(square == new Polygon(squareBorder));

        // point-in-polygon checks
        Debug.Assert(square.Contains(new Point(2, 2)));
        Debug.Assert(square.Contains(new Point(4, 4)));
        Debug.Assert(!square.Contains(new Point(6, 6)));

        // point location queries
        Debug.Assert(square.Locate(new Point(2, 2)) == Location.Interior);
        Debug.Assert(square.Locate(new Point(4, 4)) == Location.Boundary);
        Debug.Assert(square.Locate(new Point(6, 6)) == Location.Exterior);

        // set intersection
        Polygon[] intersection = square & square;
        Debug.Assert(intersection.Length == 1);
        Debug.Assert(intersection[0] == square);

        // set union
        Polygon[] union = square | square;
        Debug.Assert(union.Length == 1);
        Debug.Assert(union[0] == square);

        // set difference
        Polygon[] difference = square - square;
        Debug.Assert(difference.Length == 0);

        // set symmetric difference
        Polygon[] symmetricDifference = square ^ square;
        Debug.Assert(symmetricDifference.Length == 0);
    }
}
```

More examples can be found at [src/GonExamples directory](src/GonExamples).

Development
-----------

### Bumping version

#### Preparation

Install
[bump2version](https://github.com/c4urself/bump2version#installation).

#### Pre-release

Choose which version number category to bump following [semver
specification](http://semver.org/).

Test bumping version

```bash
bump2version --dry-run --verbose $CATEGORY
```

where `$CATEGORY` is the target version number category name, possible
values are `patch`/`minor`/`major`.

Bump version

```bash
bump2version --verbose $CATEGORY
```

This will set version to `major.minor.patch-alpha`.

#### Release

Test bumping version

```bash
bump2version --dry-run --verbose release
```

Bump version

```bash
bump2version --verbose release
```

This will set version to `major.minor.patch`.

### Running tests

In what follows `python` is an alias for `python3.8`
or any later version (`python3.9` and so on).

Install dependencies

```bash
python -m pip install -r requirements-tests.txt
```

Run tests

```bash
pytest
```
