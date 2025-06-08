@echo off
echo Building DllMetadataExtractor...
dotnet publish -c Release -o ./publish
echo Build complete. The executable is in the 'publish' directory.
pause 