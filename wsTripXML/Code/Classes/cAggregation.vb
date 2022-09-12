Imports System
Imports TripXMLMain
Imports System.Text
Imports TripXMLMain.modCore

Namespace wsTravelTalk


    Public Class cAggregation


        Public Shared Sub Aggregate(ByVal MessageID As ttServices, ByVal XslPath As String, ByVal Version As String, ByRef Response As String)
            Dim strXslName As String = ""
            Dim sb As StringBuilder = New StringBuilder()

            XslPath &= "Aggregation\"

            If Version.Length > 0 Then
                strXslName = sb.Append(Version).Append("_").ToString()
                sb.Remove(0, sb.Length())
            End If

            Select Case MessageID
                Case ttServices.AirAvail
                    strXslName = "Aggregation_AirAvailRS.xsl"
                Case ttServices.LowFare
                    strXslName = "Aggregation_LowFareRS.xsl"
                Case ttServices.LowFarePlus
                    strXslName = "Aggregation_LowFareRS.xsl"
                Case ttServices.LowFareSchedule
                    strXslName = "Aggregation_LowFareRS.xsl"
                Case ttServices.CarAvail
                    strXslName = "Aggregation_CarAvailRS.xsl"
                Case ttServices.HotelAvail
                    strXslName = "Aggregation_HotelAvailRS.xsl"
                Case ttServices.HotelSearch
                    strXslName = "Aggregation_HotelSearchRS.xsl"
                Case Else
                    strXslName = "NoSupported.xsl"
            End Select

            Try
                Response = CoreLib.TransformXML(Response, XslPath, strXslName)
            Catch ex As Exception
                Throw ex
            End Try
            sb = Nothing
        End Sub

        Public Shared Sub ProcessMarkup(ByVal XslPath As String, ByVal Version As String, ByRef Response As String)
            Dim strXslName As String = ""
            Dim sb As StringBuilder = New StringBuilder()

            XslPath &= "Aggregation\"

            If Version.Length > 0 Then
                strXslName = sb.Append(Version).Append("_").ToString()
                sb.Remove(0, sb.Length())
            End If

            strXslName = "Markups_LowFareRS.xsl"

            CoreLib.SendTrace("", "cAggregation", "markup", Response.Substring(0, 2000), String.Empty)

            Try
                Response = CoreLib.TransformXML(Response, XslPath, strXslName)
            Catch ex As Exception
                Throw ex
            End Try
            sb = Nothing
        End Sub

    End Class

End Namespace
