Imports TripXMLMain
Imports System.Net
Imports System.Xml
Imports System.IO
Imports System.Text
Imports System.Web.Configuration

Public Class PortalAdapter

    Private ttProviderSystems As TripXMLProviderSystems
    Private mHttpRequest As HttpWebRequest
    Private sb As StringBuilder = New StringBuilder()
    Private strNameSpace = WebConfigurationManager.AppSettings("BENameSpace")

    Public Sub New(ByVal ProviderSystems As TripXMLProviderSystems)

        ttProviderSystems = ProviderSystems

    End Sub

    Private Sub HttpConnect(ByVal SOAPAction As String)
        Dim strUrl As String
        strUrl = sb.Append(ttProviderSystems.Profile).Append("/ClientWS/default.asmx").ToString()
        sb.Remove(0, sb.Length())
        mHttpRequest = CType(WebRequest.Create(strUrl), HttpWebRequest)
        CoreLib.SendTrace(ttProviderSystems.UserID, "ttPortalAdapter", "Connect to Portal", strUrl)
        mHttpRequest.Method = "POST"
        mHttpRequest.ContentType = "text/xml; charset=utf-8"
        mHttpRequest.Headers.Add("SOAPAction", SOAPAction)
        mHttpRequest.Timeout = 60000   ' 1 Minute
    End Sub

    Public Function CreateSession() As String
        Dim strRequest As String = ""
        Dim strResponse As String = ""

        strRequest = sb.Append("<CreateSession xmlns=""http://" + strNameSpace + "/webservices/""><SiteItemID>").Append( _
                    ttProviderSystems.PCC).Append("</SiteItemID><siteItemDomain>").Append( _
                    ttProviderSystems.Password).Append("</siteItemDomain><extRefOne /><extRefTwo /><Guid>BlCNUjHe3TM=</Guid></CreateSession>").ToString()
        sb.Remove(0, sb.Length())

        strResponse = SendMessage(strRequest, """http://" + strNameSpace + "/webservices/CreateSession""")

        If strResponse.IndexOf("<SessionID>") = -1 Then
            Return ""
        Else
            strResponse = strResponse.Substring(strResponse.IndexOf("<SessionID>") + 11, strResponse.IndexOf("</SessionID>") - (strResponse.IndexOf("<SessionID>") + 11))
        End If

        Return strResponse
        sb = Nothing
    End Function

    Public Function CloseSession(ByVal token As String) As String
        Dim strRequest As String = ""
        Dim strResponse As String = ""

        strRequest = sb.Append("<CloseSession xmlns=""http://" + strNameSpace + "/webservices/""><sessionID>").Append(token).Append("</sessionID></CloseSession>").ToString()
        sb.Remove(0, sb.Length())

        strResponse = SendMessage(strRequest, """http://" + strNameSpace + "/webservices/CloseSession""")

        Return strResponse
        sb = Nothing
    End Function

    Public Function SendMessage(ByVal Message As String, ByVal SOAPAction As String) As String
        Dim oWriter As StreamWriter = Nothing
        Dim oReader As StreamReader = Nothing
        Dim oHttpResponse As HttpWebResponse = Nothing
        Dim strResponse As String = ""
        Dim strAuthentication As String = ""
        Dim StartTime As Date

        Try

            StartTime = Now

            HttpConnect(SOAPAction)

            Message = sb.Append("<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><soap:Body>").Append(Message).Append("</soap:Body></soap:Envelope>").ToString()
            sb.Remove(0, sb.Length())

            mHttpRequest.ContentLength = Message.Length

            oWriter = New StreamWriter(mHttpRequest.GetRequestStream())

            CoreLib.SendTrace(ttProviderSystems.UserID, "ttPortalAdapter", "Sending to Portal", "url=" & ttProviderSystems.Profile & " action=" & SOAPAction)

            oWriter.Write(Message)

            CoreLib.SendTrace(ttProviderSystems.UserID, "ttPortalAdapter", "Sent to Portal", Message)

        Catch ex As Exception
            Throw New Exception(sb.Append("Error connecting to Portal. Portal system may be down.").Append(vbNewLine).Append(ex.Message).ToString())
            sb.Remove(0, sb.Length())
        Finally
            If Not oWriter Is Nothing Then
                oWriter.Close()
            End If
        End Try

        Try
            oHttpResponse = CType(mHttpRequest.GetResponse(), HttpWebResponse)

            oReader = New StreamReader(oHttpResponse.GetResponseStream())

            strResponse = oReader.ReadToEnd

            strResponse = strResponse.Replace("<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">", "")
            strResponse = strResponse.Replace("<soapenv:Body>", "")
            strResponse = strResponse.Replace("</soapenv:Body>", "")
            strResponse = strResponse.Replace("</soapenv:Envelope>", "")
            strResponse = strResponse.Replace("<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">", "")
            strResponse = strResponse.Replace("<soap:Body>", "")
            strResponse = strResponse.Replace("</soap:Body>", "")
            strResponse = strResponse.Replace("</soap:Envelope>", "")
            strResponse = strResponse.Replace(" xsi:type=""AirResponse""", "")
            strResponse = strResponse.Replace(" xsi:type=""Error""", "")
            strResponse = strResponse.Replace(" xmlns=""http://" + strNameSpace + "/webservices/""", "")
            strResponse = strResponse.Replace("<?xml version=""1.0"" encoding=""utf-8""?>", "")

            CoreLib.SendTrace(ttProviderSystems.UserID, "ttPortalAdapter", "Received from Portal", strResponse)

            Return strResponse

        Catch ex As Exception
            Throw New Exception(sb.Append("Error connecting to Portal. Portal system may be down.").Append(vbNewLine).Append(ex.Message).ToString())
            sb.Remove(0, sb.Length())
        Finally

            CoreLib.SendTrace(ttProviderSystems.UserID, "ttPortalAdapter", sb.Append("Portal Response Time = ").Append(CType(Now.Subtract(StartTime).TotalSeconds, Integer)).Append(" seconds.").ToString(), "")
            sb.Remove(0, sb.Length())

            If Not oHttpResponse Is Nothing Then
                oHttpResponse.Close()
            End If
            If Not oReader Is Nothing Then
                oReader.Close()
            End If
            If Not mHttpRequest Is Nothing Then mHttpRequest = Nothing
        End Try
        sb = Nothing
    End Function

End Class
