
'''<remarks/>
<System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True),
 System.Xml.Serialization.XmlRootAttribute([Namespace]:="", IsNullable:=False)>
Partial Public Class SessionCreateRS
    '''<remarks/>
    Public Property Success As Object

    '''<remarks/>
    Public Property ConversationID As ConversationID

    '''<remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()>
    Public Property Version As Decimal
End Class

'''<remarks/>
<System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True)>
Partial Public Class ConversationID
    '''<remarks/>
    Public Property Errors As Errors
End Class

'''<remarks/>
<System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True)>
Partial Public Class Errors
    '''<remarks/>
    Public Property [Error] As String
End Class

