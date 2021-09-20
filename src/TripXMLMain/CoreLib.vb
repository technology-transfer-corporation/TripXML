Imports System.Xml
Imports System.Net.Sockets
Imports System.Text
Imports System.Net.Mail
Imports System.Web.Configuration
Imports System.IO

Public Class CoreLib

#Region " Transform XML with XSLs "

    '    Public Shared Function TransformXML1(ByVal inputXml As String, ByVal xslPath As String, ByVal xslName As String, Optional ByVal FromFile As Boolean = False) As String
    '        Dim xslt As Xsl.XslTransform = Nothing
    '        Dim oDoc As XmlDocument = Nothing
    '        Dim oxNav As XPathNavigator = Nothing
    '        Dim oResolver As XmlUrlResolver = Nothing
    '        Dim oWriter As System.IO.StringWriter = Nothing
    '        Dim oReader As XmlTextReader = Nothing

    '#Const xslTInline = False

    '        Try
    '            oDoc = New XmlDocument
    '            If FromFile Then
    '                oDoc.Load(inputXml)
    '            Else
    '                oDoc.LoadXml(inputXml)
    '            End If
    '            oxNav = oDoc.CreateNavigator()

    '            oResolver = New XmlUrlResolver
    '            oResolver.Credentials = System.Net.CredentialCache.DefaultCredentials

    '            oWriter = New System.IO.StringWriter(New System.Text.StringBuilder)

    '            xslt = New Xsl.XslTransform

    '#If xslTInline = True Then
    '            Dim asmRf As System.Reflection.Assembly = System.Reflection.Assembly.GetCallingAssembly
    '            Dim AssemblyName As String = ""

    '            AssemblyName = asmRf.FullName.Substring(0, asmRf.FullName.IndexOf(",")) & "."
    '            oReader = New XmlTextReader(asmRf.GetManifestResourceStream(AssemblyName & xslName))
    '            xslt.Load(oReader, New XmlUrlResolver, asmRf.GetExecutingAssembly.Evidence)
    '#Else
    '            If xslPath.LastIndexOf("\") <> xslPath.Length - 1 Then
    '                xslPath &= "\"
    '            End If
    '            xslt.Load(xslPath & xslName)
    '            ' AC Test to catch the XSLs files Dim ValidateXSDOut = Web.HttpContext.Current.Application.Get("XSDElleipsisOut")

    '#End If

    '            xslt.Transform(oxNav, Nothing, oWriter, oResolver)

    '            Return oWriter.ToString()

    '        Catch ex As Exception
    '            Throw ex
    '        Finally
    '            If Not oReader Is Nothing Then
    '                oReader.Close()
    '                oReader = Nothing
    '            End If
    '            If Not oWriter Is Nothing Then
    '                oWriter.Close()
    '                oWriter = Nothing
    '            End If
    '            If Not oResolver Is Nothing Then oResolver = Nothing
    '            If Not oxNav Is Nothing Then oxNav = Nothing
    '            If Not oDoc Is Nothing Then oDoc = Nothing
    '            If Not xslt Is Nothing Then xslt = Nothing

    '        End Try

    '    End Function

    Public Shared Function TransformXML(ByVal inputXml As String, ByVal xslPath As String, ByVal xslName As String, Optional ByVal fromFile As Boolean = False) As String
        Dim xslt As Xsl.XslCompiledTransform
        Dim oDoc As XmlDocument
        Dim oWriter As StringWriter = Nothing
        Dim settings As Xsl.XsltSettings = Nothing

#Const xslTInline = True

        Try
            oDoc = New XmlDocument
            If fromFile Then
                oDoc.Load(inputXml)
            Else
                oDoc.LoadXml(inputXml)
            End If
            oWriter = New StringWriter
            xslt = New Xsl.XslCompiledTransform
            settings = New Xsl.XsltSettings(True, True)


#If xslTInline = True Then
            'Dim oReader As XmlTextReader = Nothing
            'oResolver = New XmlUrlResolver
            'oResolver.Credentials = System.Net.CredentialCache.DefaultCredentials
            'Dim asmRf As System.Reflection.Assembly = System.Reflection.Assembly.GetCallingAssembly

            'sbT.Append(asmRf.FullName.Substring(0, asmRf.FullName.IndexOf(",")))
            'sbT.Append(".").Append(xslName)
            'oReader = New XmlTextReader(asmRf.GetManifestResourceStream(sbT.ToString()))
            'sbT = Nothing
            'xslt.Load(oReader, New XmlUrlResolver, asmRf.GetExecutingAssembly.Evidence)
            Dim xxslt As String = xslName.Replace(".xsl", "")
            'xslt.Load(GetType(AmadeusWS_AirAvailRQ))
            xslt.Load(Reflection.Assembly.Load(xxslt).GetType(xxslt))
#Else
            If xslPath.LastIndexOf("\") <> xslPath.Length - 1 Then
                xslPath = sbT.Append(xslPath).Append("\").ToString()
            End If
            sbT.Append(xslPath).Append(xslName)
            xslt.Load(sbT.ToString(), settings, New XmlUrlResolver)
            sbT = Nothing
#End If
            xslt.Transform(oDoc.DocumentElement.ParentNode, Nothing, oWriter)
            Return oWriter.ToString

        Catch ex As Exception
            Throw
        Finally
            If Not oWriter Is Nothing Then
                oWriter.Close()
            End If
        End Try

    End Function

#End Region

#Region " Send Trace "

    Public Shared Sub SendTrace(ByVal userID As String, ByVal strFile As String, ByVal strText As String, ByVal strItem As String, ByVal strUUID As String)
        Dim udpClient As New UdpClient
        Dim sb As StringBuilder = New StringBuilder()
        Try
            strItem = strItem.Replace("<?xml version=""1.0"" encoding=""utf-16""?>", "")
            strItem = strItem.Replace("<?xml version='1.0' encoding='utf-8'?>", "")
            strItem = strItem.Replace("<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>", "")
            strItem = strItem.Replace("<?xml version=""1.0""  encoding=""ISO-8859-1"" standalone=""yes"" ?>", "")
            strItem = strItem.Replace("<?xml version=""1.0"" encoding=""ISO-8859-1""  standalone=""yes""?>", "")
            strItem = strItem.Replace("<?xml version=""1.0""   encoding=""ISO-8859-1""  standalone=""yes"" ?>", "")
            strItem = strItem.Replace("xmlns = """"", "")

            udpClient.Connect("localhost", 3070)
            Dim sendBytes As [Byte]()

            If Not userID Is Nothing Then
            Else
                userID = ""
            End If

            sb.Append("<").Append(strFile).Append("><Text>").Append(strText).Append("</Text><UUID>").Append(strUUID).Append("</UUID><Item>").Append(strItem).Append("</Item><UserID>").Append(userID).Append("</UserID></").Append(strFile).Append(">")

            sendBytes = Encoding.ASCII.GetBytes(sb.ToString())
            udpClient.Send(sendBytes, sendBytes.Length)
            udpClient.Close()

        Catch ex As Exception
            If Not udpClient Is Nothing Then
                udpClient.Close()
            End If
        End Try

    End Sub

#End Region

#Region " Validate XML against the Schema "

    Public Overloads Shared Function ValidateXML(ByVal xmlData As String, ByVal Service As Integer, ByVal SchemaType As Integer, ByVal UserID As String, ByVal Version As String) As Boolean
        Dim schemaFile As String

        Try
            schemaFile = GetSchemaFile(Service, SchemaType, Version)
            Return ValidateXML(xmlData, schemaFile, UserID)
        Catch ex As Exception
            Throw
        Finally

        End Try

    End Function

    Public Overloads Shared Function ValidateXML(ByVal xmlData As String, ByVal otaMessage As String, ByVal SchemaFolder As String, ByVal UserID As String, ByVal version As String) As Boolean
        Dim sb As StringBuilder = New StringBuilder()

        Try
            If Not SchemaFolder.EndsWith("\") Then
                sb.Append(SchemaFolder).Append("\")
            End If

            sb.Append(SchemaFolder).Append(GetSchemaFile(otaMessage, version))

            Return ValidateXML(xmlData, sb.ToString(), UserID)

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Overloads Shared Function ValidateXML(ByVal xmlData As String, ByVal SchemaFile As String, ByVal UserID As String) As Boolean
        Dim settings As XmlReaderSettings
        Dim vr As XmlReader
        Dim stream As StringReader

        Try
            If SchemaFile.IndexOf("\", StringComparison.Ordinal) = -1 Then SchemaFile = String.Concat(SchemaPath, SchemaFile)
            settings = New XmlReaderSettings()
            settings.ValidationType = ValidationType.Schema
            settings.Schemas.Add(Nothing, SchemaFile)
            stream = New StringReader(xmlData)
            vr = XmlReader.Create(stream, settings)

        Catch ex As Exception
            ' Send to Trace Validation Schema not found.
            SendTrace(UserID, "wsTravelTalk", "XML Validation Error", ex.Message, String.Empty)
            Return True
        End Try

        Try
            While vr.Read()
                ' Just Read the XML Document
            End While
        Catch ex As Exception
            Throw
        End Try

        Return True

    End Function

#End Region

#Region " Fantome "

    Public Shared Function SentToFantome(ByVal UserID As String, ByVal Provider As String, ByRef Request As String) As String
        Dim requestTag As String
        Dim delay As Integer
        Dim minDelay As Integer
        Dim maxDelay As Integer
        Dim response As String
        Dim oDoc As XmlDocument
        Dim oRoot As XmlElement
        Dim oNode As XmlNode
        Dim sb As StringBuilder = New StringBuilder()

        Try

            sb.Append("Received at Fantome Adapter").Append(Provider)
            SendTrace(UserID, "CoreLib", sb.ToString(), Request, String.Empty)
            sb.Remove(0, sb.Length())

            oDoc = New XmlDocument
            oDoc.LoadXml(Request)
            oRoot = oDoc.DocumentElement

            requestTag = oRoot.Name

            oDoc.Load("C:\TravelTalk\Tables\ACTRS.xml")
            oRoot = oDoc.DocumentElement

            sb.Append("Provider[@Name='").Append(Provider).Append("']")
            oNode = oRoot.SelectSingleNode(sb.ToString())
            sb.Remove(0, sb.Length())

            If oNode Is Nothing Then
                sb.Append("Provider : ").Append(Provider).Append(" not supported by Fantome")
                Throw New Exception(sb.ToString())
            End If

            minDelay = oNode.Attributes("MinDelay").Value
            maxDelay = oNode.Attributes("MaxDelay").Value

            response = oNode.SelectSingleNode(requestTag).InnerXml

            delay = CInt(Int((maxDelay - minDelay + 1) * Rnd() + minDelay))

            sb.Append("Delaying ").Append(delay).Append(" miliseconds")
            SendTrace(UserID, "CoreLib", sb.ToString(), response, String.Empty)
            sb.Remove(0, sb.Length())

            Threading.Thread.Sleep(delay)

            sb.Append("Return by Fantome Adapter ").Append(Provider)
            SendTrace(UserID, "CoreLib", sb.ToString(), response, String.Empty)
            sb.Remove(0, sb.Length())

            Return response

        Catch ex As Exception
            Throw
        End Try

    End Function

#End Region

#Region " GetNodeInnerText "

    Public Shared Function GetNodeInnerText(ByVal xmlData As String, ByVal sNode As String, Optional ByVal RetData As Boolean = True) As String
        Dim intStart As Integer
        Dim intLength As Integer
        Dim sb As StringBuilder = New StringBuilder()

        sb.Append("<").Append(sNode).Append(">")
        If xmlData.IndexOf(sb.ToString(), StringComparison.Ordinal) = -1 Then
            If RetData Then
                Return xmlData
            Else
                Return ""
            End If
        End If

        intStart = xmlData.IndexOf(sb.ToString(), StringComparison.Ordinal) + sNode.Length + 2
        sb.Remove(0, sb.Length)
        sb.Append("</").Append(sNode).Append(">")

        intLength = xmlData.IndexOf(sb.ToString(), StringComparison.Ordinal) - intStart

        Return xmlData.Substring(intStart, intLength).Replace(vbCr, "").Replace(vbLf, "").Trim

    End Function

#End Region

#Region " Mask Credit Card, Passport Number "
    'CardNumber, BankAcctNumber

    Public Shared Sub MaskPrivateData(ByRef Message As String, ByVal Attributes() As String)
        Dim index As Integer
        Dim length As Integer
        Dim i As Integer
        Dim sb As StringBuilder = New StringBuilder()

        For i = 0 To Attributes.Length - 1

            index = Message.IndexOf(Attributes(i), StringComparison.Ordinal)

            Do While index > -1
                index = index + Attributes(i).Length + 2
                length = Message.IndexOf(Chr(34), index + 1)
                If Message = "" Or index < 1 Or length < 0 Then
                    index = -1
                Else
                    sb.Append(Message.Substring(0, index)).Append("****************").Append(Message.Substring(length))
                    Message = sb.ToString()
                    sb.Remove(0, sb.Length())
                    index = Message.IndexOf(Attributes(i).ToString, length, StringComparison.Ordinal)
                End If
            Loop

        Next

    End Sub

#End Region

#Region " Send emails out "

    Public Shared Sub SendEmail(ByVal Subject As String, ByVal Body As String, Optional ByVal User As String = "")

        Dim mail As MailMessage
        Dim smtp As SmtpClient
        Dim oDoc As XmlDocument
        Dim oRoot As XmlElement
        Dim oNode As XmlNode
        Dim sendTo As String
        Dim strPath As String
        Dim i As Integer = 0
        Dim doSendEmail As SendEmailAsynch
        Dim sb As StringBuilder = New StringBuilder()

        sb.Append(WebConfigurationManager.AppSettings("TripXMLFolder")).Append("\Tables\Users\")
        strPath = sb.ToString()
        sb.Remove(0, sb.Length())

        oDoc = New XmlDocument

        Try
            sb.Append(strPath).Append("tt_acl.xml")
            oDoc.Load(sb.ToString())
            sb.Remove(0, sb.Length())
        Catch exr As Exception
            SendTrace("", "CoreLib", "TripXMLSendMail: Error Loading tt_acl.xml", exr.Message, String.Empty)
            Throw
        End Try

        oRoot = oDoc.DocumentElement

        If User = "" Then
            For Each oNode In oRoot.SelectNodes("SendMail/To")
                sb.Append(oNode.InnerText)
                i = i + 1

                If i < oRoot.SelectNodes("SendMail/To").Count Then
                    sb.Append(";")
                End If
            Next
            sendTo = sb.ToString()
            sb.Remove(0, sb.Length())
        Else
            sb.Append("Customer/User[Username='").Append(User).Append("']/@ErrEmail")
            oNode = oRoot.SelectSingleNode(sb.ToString())
            sb.Remove(0, sb.Length())

            If Not oNode Is Nothing Then
                sendTo = oNode.InnerText
            Else
                For Each oNode In oRoot.SelectNodes("SendMail/To")
                    sb.Append(oNode.InnerText)
                    i = i + 1

                    If i < oRoot.SelectNodes("SendMail/To").Count Then
                        sb.Append(";")
                    End If
                Next
                sendTo = sb.ToString()
                sb.Remove(0, sb.Length())
            End If
        End If

        mail = New MailMessage(oRoot.SelectSingleNode("SendMail/From").InnerText, sendTo, Subject, Body)

        If oRoot.SelectSingleNode("SendMail/@Format").InnerText = "Text" Then
            mail.IsBodyHtml = False
        Else
            mail.IsBodyHtml = True
        End If

        smtp = New SmtpClient(oRoot.SelectSingleNode("SendMail/SmtpServer").InnerText, 25)
        smtp.UseDefaultCredentials = False
        Dim smtpUserInfo As Net.NetworkCredential = New System.Net.NetworkCredential(oRoot.SelectSingleNode("SendMail/Username").InnerText, oRoot.SelectSingleNode("SendMail/Password").InnerText)
        smtp.Credentials = smtpUserInfo

        doSendEmail = New SendEmailAsynch(mail, smtp)
        doSendEmail.BeginSearch()

    End Sub

#End Region

End Class

Public Class SendEmailAsynch
    Private Delegate Sub StartSearch_Delegate()
    Private StartSearch_Wrapper As New StartSearch_Delegate(AddressOf DoSendEmail)
    Private mail As MailMessage
    Private smtp As SmtpClient

    Public Sub New(ByVal _mail As MailMessage, ByRef _smtp As SmtpClient)
        mail = _mail
        smtp = _smtp
    End Sub
    Public Sub BeginSearch()
        Dim cbr As AsyncCallback = New AsyncCallback(AddressOf EndSearch)
        Dim arr As IAsyncResult = StartSearch_Wrapper.BeginInvoke(cbr, Nothing)
    End Sub
    Private Sub EndSearch(ByVal asy As IAsyncResult)
        StartSearch_Wrapper.EndInvoke(asy)
        asy.AsyncWaitHandle.Close()
    End Sub
    Private Sub DoSendEmail()
        Try
            smtp.Send(mail)
            mail.Dispose()
        Catch exr As Exception
            CoreLib.SendTrace("", "CoreLib", "TripXMLSendMail: Error sending email", exr.Message, String.Empty)
            Throw
        End Try
        smtp = Nothing
    End Sub
End Class
