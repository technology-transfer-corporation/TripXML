@echo off

mkdir DLLs

cd Sabre
call SabreNuget.bat

cd ../AmadeusWS
call AmadeusNuget.bat

cd ../Galileo
call GalileoNuget.bat

cd ../Worldspan
call WorldspanNuget.bat