Imports System.Runtime.Serialization

Namespace wsTravelTalk.wmIssueMCOModels
    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)>
    <Serializable>
    Public Class MCO
#Region "Request Fields"
        Public Property ErrorMessage As String
        Public Property Amount As String
        Public Property PassengerNumber As String
        Public Property PaxNumber As String
        Public Property PassengerName As String
        Public Property PaxType As String
        Public Property TicketingAirlineCode As String
        Public Property [To] As String
        Public Property AT As String
        Public Property Tax As String
        Public Property TaxCode As String
        Public Property TypeOfService() As String
        Public Property PQNumber As Integer
        Public Property TourNumber As String
        Public Property Endorsements() As String
        Public Property Remarks() As String
        Public Property TicketNumber As String
        Public Property CurrencyCode As String
        Public Property EquivelentAmount As String
        Public Property EquivCurrencyCode As String
        Public Property RateOfExchange As String
        Public Property CommissionPercentage As String
        Public Property CommissionAmount As String
        Public Property InternationalItin As Boolean

        'Form of payments
        Public Property CASH As Boolean
        Public Property Check As Boolean
        Public Property Cheque As Boolean
        Public Property CreditCard As String
        Public Property Expiration As String
        Public Property ApprovalCode As String
        Public Property Vendor As String
#End Region
    End Class
    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)>
    <KnownType(GetType(MCO))>
    <Serializable>
    Public Class MCOMask
        Inherits MCO
        Public Sub New()
            ErrorMessage = String.Empty
            AT = "NYC"
            StatementInformation = String.Empty
            TourNumber = String.Empty
            TicketNumber = String.Empty
            Ignore = False
            CurrencyCode = "USD"
            EquivelentAmount = String.Empty
            EquivCurrencyCode = String.Empty
            RateOfExchange = String.Empty
            Total = String.Empty
            CommissionPercentage = String.Empty
            CommissionAmount = String.Empty
            InternationalItin = False
            CASH = False
            Cheque = False
            SelfSale = False
            CreditCard = " "
            Expiration = " "
            ApprovalCode = " "
            isAX = False
            isSuppressed = False
            SGR = " "
            Other = " "
            GTR = " "
            IgnoreTJR = False
            IgnoreCouponPrint = False
            DisplayPrevMask = False
            IssueDocuments = True
            IsDone = False
            Id = String.Empty
        End Sub

#Region "MCO Creat Fields"
        Public Property Total As String
        Public Property Id As String
        Public Property StatementInformation As String
        Public Property Ignore As Boolean
        Public Property SelfSale As Boolean
        Public Property isAX As Boolean
        Public Property isSuppressed As Boolean
        Public Property SGR As String
        Public Property Other As String
        Public Property GTR As String
        Public Property IgnoreTJR As Boolean
        Public Property IgnoreCouponPrint As Boolean
        Public Property DisplayPrevMask As Boolean
        Public Property IssueDocuments As Boolean
        Public Property IsDone As Boolean
#End Region

#Region "MCO Exchange Fields"
        Public Property MCONumber As String
        Public Property ChangeFeeAmount As String
        Public Property IsEMDFee As Boolean
        Public Property TaxComparison As Boolean
        Public Property NewTktCommAmount As String
        Public Property AddCollCommAmount As String
        Public Property Waiver As String
        Public Property IsModifyBAG As Boolean
        Public Property IsEndorsmentsUpdate As Boolean
        Public Property ManualApproval As String
        Public Property IsTicketing As Boolean
        Public Property IsRetain As Boolean
        Public Property IsPrevious As Boolean
        Public Property IsNext As Boolean
        Public Property IsQuit As Boolean
#End Region

    End Class

    '<remarks/>
    Public Enum TransactionStatusCode

        '<remarks/>
        Start

        '<remarks/>
        [End]

        '<remarks/>
        Rollback

        '<remarks/>
        InSeries
    End Enum

    '<remarks/>
    Public Enum Target
        '<remarks/>
        Test
        '<remarks/>
        Production
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)>
    Public Class POS

        '<remarks/>
        Public Source As Source

        '<remarks/>
        Public TPA_Extensions As TPA_Extensions
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)>
    Public Class Source

        '<remarks/>
        Public RequestorID As RequestorID

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public AgentSine As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public PseudoCityCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public ISOCountry As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public ISOCurrency As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public AgentDutyCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public AirlineVendorID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public AirportCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public FirstDepartPoint As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public ERSP_UserID As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)>
    Public Class RequestorID

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="anyURI")>
        Public URL As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Type As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Instance As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public ID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public ID_Context As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)>
    Public Class UniqueID

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public ID As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)>
    Public Class Provider

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
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)>
    Public Class Success
    End Class
End Namespace

