#!/bin/bash

set -e
set -x

cp ./JsonFx/JsonFx.Json.UnitTests/bin/Release/net461/JsonFx.Json.dll ./JsonFx-Unity/Assets/Tests/JsonFx.Json.dll
cp ./JsonFx/JsonFx.Json.UnitTests/bin/Release/net461/JsonFx.Json.UnitTests.dll ./JsonFx-Unity/Assets/Tests/JsonFx.Json.UnitTests.dll
