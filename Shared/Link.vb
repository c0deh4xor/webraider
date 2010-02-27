''' <summary>
''' A crawled or manually added Link
''' </summary>

<Serializable()> _
Public Class Link

#Region "Other Properties"
    Private _statusCode As Net.HttpStatusCode
    Public Property StatusCode() As Net.HttpStatusCode
        Get
            Return _statusCode
        End Get
        Set(ByVal value As Net.HttpStatusCode)
            If _statusCode = value Then
                Return
            End If
            _statusCode = value
        End Set
    End Property

    Private _url As Uri
    ''' <summary>
    ''' Gets or sets the URL.
    ''' </summary>
    ''' <value>The URL.</value>
    Public Property Url() As Uri
        Get
            Return _url
        End Get
        Set(ByVal value As Uri)
            _url = value
        End Set
    End Property

    Private _UriManager As UriManager
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns>
    ''' Related Uri Manager if there is no UriManager already will be return a new one with current URL
    ''' </returns>
    ''' <remarks></remarks>
    Public Property UriManager() As UriManager
        Get
            'Fix Referer if it's not in there
            If _UriManager Is Nothing Then _UriManager = New UriManager(Me.Url, Referrer)

            Return _UriManager
        End Get
        Set(ByVal value As UriManager)
            _UriManager = value
        End Set
    End Property

    Private _redirectCount As Integer

    ''' <summary>
    ''' Gets or sets the redirect count.
    ''' </summary>
    ''' <value>The redirect count.</value>
    Public Property RedirectCount() As Integer
        Get
            Return _redirectCount
        End Get
        Set(ByVal value As Integer)
            _redirectCount = value
        End Set
    End Property

    Private _attacked As Boolean

    ''' <summary>
    ''' Gets or sets a value indicating whether this <see cref="Link" /> is attacked.
    ''' </summary>
    ''' <value><c>true</c> if attacked; otherwise, <c>false</c>.</value>
    Public Property Attacked() As Boolean
        Get
            Return _attacked
        End Get
        Set(ByVal value As Boolean)
            _attacked = value
        End Set
    End Property


    Private _responseTime As Double
    Public Property ResponseTime() As Double
        Get
            Return _responseTime
        End Get
        Set(ByVal value As Double)
            If _responseTime = value Then
                Return
            End If
            _responseTime = value
        End Set
    End Property


    Private _retryCount As Integer

    ''' <summary>
    ''' Gets retry count.
    ''' </summary>
    ''' <value>The retry count.</value>
    Public ReadOnly Property RetryCount() As Integer
        Get
            Return _retryCount
        End Get
    End Property

#End Region


#Region "Critical Fields"


    Private _SourceCode As String

    ''' <summary>
    ''' Gets or sets the source code.
    ''' </summary>
    ''' <value>The source code.</value>
    Public ReadOnly Property SourceCode() As String
        Get
            Return _SourceCode
        End Get
    End Property

    Private _RawRequest As String

    ''' <summary>
    ''' Gets or sets the raw http request.
    ''' </summary>
    ''' <value>The raw request.</value>
    Public ReadOnly Property RawRequest() As String
        Get
            Return _RawRequest
        End Get
    End Property

    Private _RawResponse As String
    ''' <summary>
    ''' Gets or sets the raw http response.
    ''' </summary>
    ''' <value>The raw response.</value>
    Public ReadOnly Property RawResponse() As String
        Get
            Return _RawResponse
        End Get
    End Property


#End Region

    ''' <summary>
    ''' Gets the URI.
    ''' </summary>
    ''' <value>The URI.</value>
    Public ReadOnly Property RequestUri() As System.Uri
        Get
            Return Me.Url
        End Get
    End Property



    ''' <summary>
    ''' Updates the URI.
    ''' </summary>
    ''' <param name="uri">The URI.</param>
    Public Sub UpdateUri(ByVal uri As Uri)
        Me.Url = uri
        Me.UriManager.Uri = uri
    End Sub

    ''' <summary>
    ''' Referrer Page
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Referrer() As Uri
        Get
            Return Me.Url
        End Get
    End Property




#Region "Constructors"

    ''' <summary>
    ''' Initializes a new instance of the <see cref="Link" /> class.
    ''' </summary>
    ''' <param name="uri">The URI.</param>
    Public Sub New(ByVal uri As Uri)
        Me.Url = uri
    End Sub


    ''' <summary>
    ''' New Link from URL String
    ''' </summary>
    ''' <param name="URL">Link URL</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal url As String)
        Me.New(New Uri(url))
    End Sub


    Public Sub New()

    End Sub
#End Region

End Class
