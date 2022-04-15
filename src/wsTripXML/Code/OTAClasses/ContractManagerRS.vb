Option Strict Off
Option Explicit On

Namespace wsTravelTalk.wmContractManager
    Public Class ContractManagerRS
        '<remarks/>
        Public Success As Success

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("Warning", IsNullable:=False)> _
        Public Warnings() As String

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("Error", IsNullable:=False)> _
        Public Errors() As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Success
    End Class
End Namespace
