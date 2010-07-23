Public Class SQLPayload
    Private _Name As String
    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property

    Private _Payload As String
    Public Property Payload() As String
        Get
            Return _Payload
        End Get
        Set(ByVal value As String)
            _Payload = value
        End Set
    End Property

    Private _IsEncoded As Boolean
    Public Property IsEncoded() As Boolean
        Get
            Return _IsEncoded
        End Get
        Set(ByVal value As Boolean)
            _IsEncoded = value
        End Set
    End Property

    Private _ConsoleEncoding As Boolean

    Public Property ConsoleEncoding() As Boolean
        Get
            Return _ConsoleEncoding
        End Get
        Set(ByVal value As Boolean)
            _ConsoleEncoding = value
        End Set
    End Property

End Class
