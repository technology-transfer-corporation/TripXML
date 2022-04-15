
Option Strict Off
Option Explicit On 

Imports System.Xml.Serialization

Namespace wsTravelTalk.wmMultiMessageIn

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class MultiMessageRQ

        '<remarks/>
        Public POS As POSType

        '<remarks/>
        Public MultiMessage As String
    End Class

    '<remarks/>
    Public Class POSType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Source")> _
        Public Source() As SourceType

        '<remarks/>
        Public TPA_Extensions As TPA_ExtensionsType
    End Class

    '<remarks/>
    Public Class SourceType

        '<remarks/>
        Public RequestorID As RequestorIDType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PseudoCityCode As String
    End Class

    '<remarks/>
    Public Class RequestorIDType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Type As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ID As String
    End Class

    '<remarks/>
    Public Class ProviderType

        '<remarks/>
        Public Name As String

        '<remarks/>
        Public System As String

        '<remarks/>
        Public Userid As String

        '<remarks/>
        Public Password As String
    End Class

    '<remarks/>
    Public Class TPA_ExtensionsType

        '<remarks/>
        Public Provider As ProviderType
    End Class
End Namespace

