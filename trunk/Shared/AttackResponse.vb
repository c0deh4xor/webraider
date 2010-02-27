Imports System.Net

''' <summary>
''' Web Response, also carry AttackString and SourceCode as String
''' </summary>
''' <remarks></remarks>
Public Class AttackResponse

    Private _sourceCode As String
    ''' <summary>
    ''' Gets or sets the source code.
    ''' </summary>
    ''' <value>The source code.</value>
    Public Property SourceCode() As String
        Get
            Return _sourceCode
        End Get
        Set(ByVal value As String)
            _sourceCode = value
        End Set
    End Property

    Private _Headers As WebHeaderCollection
    ''' <summary>
    ''' Gets or sets the headers.
    ''' </summary>
    ''' <value>The headers.</value>
    Public Property Headers() As WebHeaderCollection
        Get
            Return _Headers
        End Get
        Set(ByVal value As WebHeaderCollection)
            _Headers = value
        End Set
    End Property


    Private _uriManager As UriManager
    ''' <summary>
    ''' Gets or sets the URI manager.
    ''' </summary>
    ''' <value>The URI manager.</value>
    Public Property UriManager() As UriManager
        Get
            Return _uriManager
        End Get
        Set(ByVal value As UriManager)
            _uriManager = value
        End Set
    End Property

    Private _link As Link

    ''' <summary>
    ''' Gets or sets the link.
    ''' </summary>
    ''' <value>The link.</value>
    Public Property Link() As Link
        Get
            Return _link
        End Get
        Set(ByVal value As Link)
            _link = value
        End Set
    End Property

    ''' <summary>
    ''' New attack response
    ''' </summary>
    ''' <param name="responseUri">The response URI.</param>
    ''' <param name="statusCode">The status code.</param>
    ''' <param name="sourceCode">The source code.</param>
    ''' <param name="uriManager">The URI manager.</param>
    ''' <param name="link">The link.</param>
    ''' <param name="time">The time.</param>
    ''' <param name="rawResponse">The raw response.</param>
    ''' <param name="rawRequest">The raw request.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal responseUri As Uri, ByVal statusCode As HttpStatusCode, ByVal sourceCode As String, ByVal uriManager As UriManager, ByVal link As Link, ByVal time As Double, ByVal rawRequest As String, ByVal rawResponse As String)
        Me.SourceCode = sourceCode
        Me.UriManager = uriManager
        Me.Link = link

        Me.StatusCode = statusCode
        Me.ResponseUri = responseUri

        Me.ResponseTime = time

        Me.RawRequest = rawRequest
        Me.RawResponse = rawResponse
    End Sub

    Private _ResponseTime As Double
    ''' <summary>
    ''' How long request took
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ResponseTime() As Double
        Get
            Return _ResponseTime
        End Get
        Set(ByVal value As Double)
            _ResponseTime = value
        End Set
    End Property


    ''' <summary>
    ''' Initializes a new instance of the <see cref="AttackResponse" /> class.
    ''' </summary>
    ''' <param name="uriManager">The URI manager.</param>
    Public Sub New(ByVal uriManager As UriManager)
        Me.New(Nothing, HttpStatusCode.Found, String.Empty, uriManager, Nothing, Nothing, String.Empty, String.Empty)
    End Sub


    Private _statusCode As HttpStatusCode

    ''' <summary>
    ''' Gets or sets the status code (default is found [200]).
    ''' </summary>
    ''' <value>The status code.</value>
    Public Property StatusCode() As HttpStatusCode
        Get
            Return _statusCode
        End Get
        Set(ByVal value As HttpStatusCode)
            _statusCode = value
        End Set
    End Property

    Private _responseUri As Uri

    ''' <summary>
    ''' Gets or sets the response URI.
    ''' </summary>
    ''' <value>The response URI.</value>
    Public Property ResponseUri() As Uri
        Get
            Debug.Assert(Not (_responseUri Is Nothing), "What the hell you trying to access here for not-attacked attackresponse ")
            Return _responseUri
        End Get
        Set(ByVal value As Uri)
            _responseUri = value
        End Set
    End Property

    Private _RawResponse As String
    ''' <summary>
    ''' Gets or sets the raw response.
    ''' </summary>
    ''' <value>The raw response.</value>
    Public Property RawResponse() As String
        Get
            Return _RawResponse
        End Get
        Set(ByVal value As String)
            _RawResponse = value
        End Set
    End Property

    Private _RawRequest As String

    ''' <summary>
    ''' Gets or sets the raw request.
    ''' </summary>
    ''' <value>The raw request.</value>
    Public Property RawRequest() As String
        Get
            Return _RawRequest
        End Get
        Set(ByVal value As String)
            _RawRequest = value
        End Set
    End Property

    ''' <summary>
    ''' Returns a <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
    ''' </summary>
    ''' <returns>
    ''' A <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
    ''' </returns>
    Public Overrides Function ToString() As String

        Dim ResponseDetails As New Text.StringBuilder(3000)
        With ResponseDetails
            .AppendLine(Me.RawRequest)
            .AppendLine()
            .AppendLine()
            .AppendLine(New String("-"c, 30))

            .AppendLine(Me.RawResponse)
            .AppendLine()
            .AppendLine()
            .AppendLine(Me.SourceCode)
            .AppendLine(New String("-"c, 30))

        End With

        Return ResponseDetails.ToString()
    End Function

    Private _Cookies() As Net.Cookie
    Public Property Cookies() As Net.Cookie()
        Get
            Return _Cookies
        End Get
        Set(ByVal value As Net.Cookie())
            _Cookies = value
        End Set
    End Property

End Class