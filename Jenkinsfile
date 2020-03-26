#!/usr/bin/env groovy

@Library('sharedcode')_

sharedCodePipeline(
    projectSubDirectory: "JsonFx",
    projectConfigs: [ "Release", "Release-net35" ],
    testDlls: [ "JsonFx/JsonFx.Json.UnitTests/bin/Release/JsonFx.Json.UnitTests.dll" ]
)