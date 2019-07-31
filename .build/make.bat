@ECHO OFF
SET nobuild=
SET config=Release
IF "%1"=="nobuild" (SET nobuild=yes) ELSE (SET config=%1)
IF "%2"=="nobuild" (SET nobuild=yes)
IF "%config%"=="" (SET config=Release)
@ECHO %nobuild%
IF "%nobuild%"=="" (
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
)

IF EXIST NugetBuild RD /s /q NugetBuild
MD NugetBuild

ECHO Setting up a version...
@ECHO OFF
CALL ".build\version"

IF "%nobuild%"=="" (
    ECHO Building .net Framework v4.6.1
    @ECHO OFF
    msbuild RDeF.sln /t:Restore
    msbuild RDeF.sln /p:Configuration=%config%

    ECHO Building NETSTANDARD2.0
    @ECHO OFF
    msbuild RDeF.Core.sln /t:Restore
    msbuild RDeF.Core.sln /p:Configuration=%config%
)

CALL ".build\pack" RDeF.Contracts %tag% %version% %release%
CALL ".build\pack" RDeF.Core %tag% %version% %release%
CALL ".build\pack" RDeF.Mapping.Attributes %tag% %version% %release%
CALL ".build\pack" RDeF.Mapping.Fluent %tag% %version% %release%
CALL ".build\pack" RDeF.Serialization %tag% %version% %release%

:COMPLETED
