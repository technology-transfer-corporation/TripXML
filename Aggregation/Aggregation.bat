set path=C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.2 Tools;%path%

xsltc /settings:document+,script+ Markups_LowFareRS.xsl
xsltc /settings:document+,script+ Aggregation_CarAvailRS.xsl
xsltc /settings:document+,script+ Aggregation_HotelAvailRS.xsl
xsltc /settings:document+,script+ Aggregation_LowFareRS.xsl
xsltc /settings:document+,script+ Aggregation_AirAvailRS.xsl

xcopy "*.dll"  "..\..\wsTripXML\bin"  /c /d /i /y
xcopy "*.dll"  "..\..\TripXMLMain\bin\Debug"  /c /d /i /y
xcopy "*.dll"  "..\..\TripXMLMain\bin\Release"  /c /d /i /y
xcopy "*.dll"  "C:\TripXML\Xsl\Aggregation"  /c /d /i /y

del "*.dll" 