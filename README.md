# mdlgen

A Source code generator for model definitions.

[![Build status](https://ci.appveyor.com/api/projects/status/uftlt64mg0v359vw/branch/master?svg=true)](https://ci.appveyor.com/project/angrifel/mdlgen/branch/master)

## Description

**mdlgen** is a source code generator that generates model classes used by [REST](https://en.wikipedia.org/wiki/Representational_state_transfer) services.

## Features

- Generates entities (as classes) and enums.
- Targets C# and TypeScript.
- Supports type aliases.
- Single standalone console executable. No dlls or config files needed.
- Model definitions written in yaml.

## Requirements

- [.NET Framework 4](https://www.microsoft.com/en-us/download/details.aspx?id=17718) or higher.

``` 
Usage: mdlgen <model-file-path>
