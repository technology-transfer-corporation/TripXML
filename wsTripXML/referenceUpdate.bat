@echo off
rem setlocal
xcopy "..\TripXMLMain\Xsl\Aggregation\*.dll"  "bin"  /c /d /i /y
xcopy "..\TripXMLMain\Xsl\AmadeusWS\*.dll"  "bin"  /c /d /i /y
xcopy "..\TripXMLMain\Xsl\BL\*.dll"  "bin"  /c /d /i /y
xcopy "..\TripXMLMain\Xsl\Galileo\*.dll"  "bin"  /c /d /i /y
xcopy "..\TripXMLMain\Xsl\Sabre\*.dll"  "bin"  /c /d /i /y
xcopy "..\TripXMLMain\Xsl\TCML\*.dll"  "bin"  /c /d /i /y
xcopy "..\TripXMLMain\Xsl\Travelport\*.dll"  "bin"  /c /d /i /y
xcopy "..\TripXMLMain\Xsl\Worldspan\*.dll"  "bin"  /c /d /i /y

rem del "*.dll" 