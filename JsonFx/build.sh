#!/bin/bash
set -e
export DOTNET_CLI_TELEMETRY_OPTOUT=1

set -x #echo on

rm -rf */bin */obj

dotnet build -c Release
