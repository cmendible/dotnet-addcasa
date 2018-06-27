[![Build status](https://ci.appveyor.com/api/projects/status/fs8a9ffavahitcya?svg=true)](https://ci.appveyor.com/project/cmendible/dotnet-addcasa)

# dotnet-addcasa

A simple .NET Core tool to enable CodeAnalysis and StyleCop in your projects.

**Note:** [.NET Core SDK 2.1.300-preview1](https://www.microsoft.com/net/download/dotnet-core/sdk-2.1.300-preview1) or higher is needed.

## Installation

To install dotnet-addcasa as a global tool run:

``` powershell
dotnet install tool -g dotnet-addcasa
```

## Usage

To add CodeAnalysis and StyleCop to all your projects run the following command from the root folder of your solution or workspace.

``` powershell
dotnet addcasa
```
