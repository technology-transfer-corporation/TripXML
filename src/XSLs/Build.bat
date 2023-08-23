@echo off
setlocal
path=C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools;%path%

mkdir DLLs

cd Sabre
call SabreNuget.bat

cd ../AmadeusWS
call AmadeusNuget.bat

cd ../Galileo
call GalileoNuget.bat

cd ../Worldspan
call WorldspanNuget.bat