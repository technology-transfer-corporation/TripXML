
'''<remarks/>
<System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True),
 System.Xml.Serialization.XmlRootAttribute([Namespace]:="", IsNullable:=False)>
Partial Public Class SessionCreateRS
    '''<remarks/>
    Public Property Success As Object

    '''<remarks/>
    Public Property ConversationID As SessionCreateRSConversationID

    '''<remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()>
    Public Property Version As Decimal
End Class

'''<remarks/>
<System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True)>
Partial Public Class SessionCreateRSConversationID
    '''<remarks/>
    Public Property Errors As SessionCreateRSConversationIDErrors
End Class

'''<remarks/>
<System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True)>
Partial Public Class SessionCreateRSConversationIDErrors
    '''<remarks/>
    Public Property [Error] As String
End Class

