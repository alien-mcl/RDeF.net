@ECHO OFF
ECHO Cleaning up...
IF EXIST RDeF.Contracts\bin RD /s /q RDeF.Contracts\bin
IF EXIST RDeF.Contracts\obj RD /s /q RDeF.Contracts\obj
IF EXIST RDeF.Core\bin RD /s /q RDeF.Core\bin
IF EXIST RDeF.Core\obj RD /s /q RDeF.Core\obj
IF EXIST RDeF.Core.Tests\bin RD /s /q RDeF.Core.Tests\bin
IF EXIST RDeF.Core.Tests\obj RD /s /q RDeF.Core.Tests\obj
IF EXIST RDeF.Mapping.Attributes\bin RD /s /q RDeF.Mapping.Attributes\bin
IF EXIST RDeF.Mapping.Attributes\obj RD /s /q RDeF.Mapping.Attributes\obj
IF EXIST RDeF.Mapping.Attributes.Tests\bin RD /s /q RDeF.Mapping.Attributes.Tests\bin
IF EXIST RDeF.Mapping.Attributes.Tests\obj RD /s /q RDeF.Mapping.Attributes.Tests\obj
IF EXIST RDeF.Mapping.Fluent\bin RD /s /q RDeF.Mapping.Fluent\bin
IF EXIST RDeF.Mapping.Fluent\obj RD /s /q RDeF.Mapping.Fluent\obj
IF EXIST RDeF.Mapping.Fluent.Tests\bin RD /s /q RDeF.Mapping.Fluent.Tests\bin
IF EXIST RDeF.Mapping.Fluent.Tests\obj RD /s /q RDeF.Mapping.Fluent.Tests\obj
IF EXIST RDeF.Serialization\bin RD /s /q RDeF.Serialization\bin
IF EXIST RDeF.Serialization\obj RD /s /q RDeF.Serialization\obj
IF EXIST RDeF.Serialization.Tests\bin RD /s /q RDeF.Serialization.Tests\bin
IF EXIST RDeF.Serialization.Tests\obj RD /s /q RDeF.Serialization.Tests\obj
IF EXIST NugetBuild RD /s /q NugetBuild
MD NugetBuild

ECHO Setting up a version...
@ECHO OFF
CALL ".build\Version"

ECHO Building .net Framework v4.6.1
@ECHO OFF
msbuild RDeF.sln /t:Restore
msbuild RDeF.sln /p:Configuration=Release

ECHO Building NETSTANDARD1.6
@ECHO OFF
msbuild RDeF.Core.sln /t:Restore
msbuild RDeF.Core.sln /p:Configuration=Release

ECHO "Building up Nuget package for RDeF.Contracts"
MD NugetBuild\RDeF.Contracts
MD NugetBuild\RDeF.Contracts\lib
MD NugetBuild\RDeF.Contracts\lib\net461
MD NugetBuild\RDeF.Contracts\lib\netstandard16
COPY RDeF.Contracts\bin\Release\RDeF.Contracts.dll NugetBuild\RDeF.Contracts\lib\net461
COPY RDeF.Contracts\bin\Release\RDeF.Contracts.xml NugetBuild\RDeF.Contracts\lib\net461
COPY RDeF.Contracts\bin\Release\netstandard1.6\RDeF.Contracts.dll NugetBuild\RDeF.Contracts\lib\netstandard16
COPY ".nuget\RDeF.Contracts.nuspec" NugetBuild\RDeF.Contracts
".build\nuget" pack NugetBuild\RDeF.Contracts\RDeF.Contracts.nuspec -version %tag:~1%.%version%.%release% -outputdirectory NugetBuild

ECHO "Building up Nuget package for RDeF.Core"
MD NugetBuild\RDeF.Core
MD NugetBuild\RDeF.Core\lib
MD NugetBuild\RDeF.Core\lib\net461
MD NugetBuild\RDeF.Core\lib\netstandard16
COPY RDeF.Core\bin\Release\RDeF.Core.dll NugetBuild\RDeF.Core\lib\net461
COPY RDeF.Core\bin\Release\RDeF.Core.xml NugetBuild\RDeF.Core\lib\net461
COPY RDeF.Core\bin\Release\netstandard1.6\RDeF.Core.dll NugetBuild\RDeF.Core\lib\netstandard16
COPY ".nuget\RDeF.Core.nuspec" NugetBuild\RDeF.Core
".build\nuget" pack NugetBuild\RDeF.Core\RDeF.Core.nuspec -version %tag:~1%.%version%.%release% -outputdirectory NugetBuild

ECHO "Building up Nuget package for RDeF.Mapping.Attributes"
MD NugetBuild\RDeF.Mapping.Attributes
MD NugetBuild\RDeF.Mapping.Attributes\lib
MD NugetBuild\RDeF.Mapping.Attributes\lib\net461
MD NugetBuild\RDeF.Mapping.Attributes\lib\netstandard16
COPY RDeF.Mapping.Attributes\bin\Release\RDeF.Mapping.Attributes.dll NugetBuild\RDeF.Mapping.Attributes\lib\net461
COPY RDeF.Mapping.Attributes\bin\Release\RDeF.Mapping.Attributes.xml NugetBuild\RDeF.Mapping.Attributes\lib\net461
COPY RDeF.Mapping.Attributes\bin\Release\netstandard1.6\RDeF.Mapping.Attributes.dll NugetBuild\RDeF.Mapping.Attributes\lib\netstandard16
COPY ".nuget\RDeF.Mapping.Attributes.nuspec" NugetBuild\RDeF.Mapping.Attributes
".build\nuget" pack NugetBuild\RDeF.Mapping.Attributes\RDeF.Mapping.Attributes.nuspec -version %tag:~1%.%version%.%release% -outputdirectory NugetBuild

ECHO "Building up Nuget package for RDeF.Mapping.Fluent"
MD NugetBuild\RDeF.Mapping.Fluent
MD NugetBuild\RDeF.Mapping.Fluent\lib
MD NugetBuild\RDeF.Mapping.Fluent\lib\net461
MD NugetBuild\RDeF.Mapping.Fluent\lib\netstandard16
COPY RDeF.Mapping.Fluent\bin\Release\RDeF.Mapping.Fluent.dll NugetBuild\RDeF.Mapping.Fluent\lib\net461
COPY RDeF.Mapping.Fluent\bin\Release\RDeF.Mapping.Fluent.xml NugetBuild\RDeF.Mapping.Fluent\lib\net461
COPY RDeF.Mapping.Fluent\bin\Release\netstandard1.6\RDeF.Mapping.Fluent.dll NugetBuild\RDeF.Mapping.Fluent\lib\netstandard16
COPY ".nuget\RDeF.Mapping.Fluent.nuspec" NugetBuild\RDeF.Mapping.Fluent
".build\nuget" pack NugetBuild\RDeF.Mapping.Fluent\RDeF.Mapping.Fluent.nuspec -version %tag:~1%.%version%.%release% -outputdirectory NugetBuild

ECHO "Building up Nuget package for RDeF.Serialization"
MD NugetBuild\RDeF.Serialization
MD NugetBuild\RDeF.Serialization\lib
MD NugetBuild\RDeF.Serialization\lib\net461
MD NugetBuild\RDeF.Serialization\lib\netstandard16
COPY RDeF.Serialization\bin\Release\RDeF.Serialization.dll NugetBuild\RDeF.Serialization\lib\net461
COPY RDeF.Serialization\bin\Release\RDeF.Serialization.xml NugetBuild\RDeF.Serialization\lib\net461
COPY RDeF.Serialization\bin\Release\netstandard1.6\RDeF.Serialization.dll NugetBuild\RDeF.Serialization\lib\netstandard16
COPY ".nuget\RDeF.Serialization.nuspec" NugetBuild\RDeF.Serialization
".build\nuget" pack NugetBuild\RDeF.Serialization\RDeF.Serialization.nuspec -version %tag:~1%.%version%.%release% -outputdirectory NugetBuild
:COMPLETED
