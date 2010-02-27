Imports System.Text

''' <summary>
''' Parse Raw HTTP Requests and Responses
''' </summary>
''' <remarks>
''' This class originally developed for XSS Tunnel, Modified for BSQL Hacker and introduced to WebRaider
''' </remarks>
Public Class HttpParser

#Region "Enums"

    ''' <summary>
    ''' HTTP Client Methods
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum HTTPMethod
        [GET]
        POST
        CONNECT
        HEAD
        TRACE
        PUT
        DELETE
        OPTIONS
        LINK
        UNLINK
        PATCH
    End Enum

#End Region

#Region "Properties"

    Private _RequestBody As String = String.Empty

    ''' <summary>
    ''' Request Body (generally post)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RequestBody() As String
        Get
            Return _RequestBody
        End Get
        Set(ByVal value As String)
            _RequestBody = value
        End Set
    End Property

    Private _Method As HTTPMethod

    ''' <summary>
    ''' HTTP Method
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Method() As HTTPMethod
        Get
            Return _Method
        End Get
        Set(ByVal value As HTTPMethod)
            _Method = value
        End Set
    End Property

    Private _Version As String
    ''' <summary>
    ''' HTTP Version
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Version() As String
        Get
            Return _Version
        End Get
        Set(ByVal value As String)
            _Version = value
        End Set
    End Property

    Private _ParseAsResponsense As Boolean
    ''' <summary>
    ''' Parse as HTTP Response 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ParseAsResponse() As Boolean
        Get
            Return _ParseAsResponsense
        End Get
        Set(ByVal value As Boolean)
            _ParseAsResponsense = value
        End Set
    End Property

    Private _expectSSL As Boolean
    ''' <summary>
    ''' Process this Raw Request as SSL connection
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ExpectSSL() As Boolean
        Get
            Return _expectSSL
        End Get
        Set(ByVal value As Boolean)
            _expectSSL = value
        End Set
    End Property


    Private _HttpResponse As HttpResponse
    ''' <summary>
    ''' HTTP Response
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property HttpResponse() As HttpResponse
        Get
            If _HttpResponse Is Nothing Then _HttpResponse = New HttpResponse()
            Return _HttpResponse
        End Get
        Set(ByVal value As HttpResponse)
            _HttpResponse = value
        End Set
    End Property


    Private _Url As Uri
    ''' <summary>
    ''' Requested Path
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Url() As Uri
        Get
            Return _Url
        End Get
        Set(ByVal value As Uri)
            _Url = value
        End Set
    End Property

#End Region

#Region "Constructor"

    ''' <summary>
    ''' New HTTP Request Parser
    ''' </summary>
    ''' <param name="request">Request</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal request As String, ByVal expectSSL As Boolean)
        Me.ExpectSSL = True
        ParseQuery(request)
    End Sub


    ''' <summary>
    ''' New HTTP Response Parser
    ''' </summary>
    ''' <param name="request">Request</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal request As String, ByVal body As Byte())
        Me.ParseAsResponse = True
        Me.HttpResponse.Body = body
        ParseQuery(request)
    End Sub

    ''' <summary>
    ''' New HTTP Request from buffer
    ''' </summary>
    ''' <param name="request">HTTP Request Buffer</param>
    ''' <param name="readData">Bytes to read</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal request() As Byte, ByVal readData As Integer)
        Dim RequestString As String = Encoding.ASCII.GetString(request, 0, readData)
        ParseQuery(RequestString)
    End Sub



#End Region

#Region "Methods"

    ''' <summary>
    ''' Parse HTTP Request or Response
    ''' </summary>
    ''' <param name="request"></param>
    ''' <remarks></remarks>
    Private Sub ParseQuery(ByVal request As String)
        Dim Path As String = Nothing

        If Not Me.ParseAsResponse Then
            Dim BodyPos As Integer = request.IndexOf(vbNewLine & vbNewLine)
            If BodyPos > -1 Then
                Me.RequestBody = request.Substring(BodyPos + 4, request.Length - (BodyPos + 4))
            End If
        End If

        request = request.Replace(vbNewLine, Chr(13))
        Dim Lines() As String = request.Split(Chr(13))

        Dim Cnt, Ret As Integer

        If Lines.Length > 0 Then
            Ret = Lines(0).IndexOf(" ")

            If (Ret > 0) Then

                Dim FirstPart As String = Lines(0).Substring(0, Ret)
                If Me.ParseAsResponse Then
                    If Not Integer.TryParse(FirstPart, Me.HttpResponse.Status) Then
                        'Status Parse Error !
                        Debug.WriteLine("ERR : Status not integer!")
                        Me.HttpResponse.Status = 999
                    End If

                    Me.HttpResponse.StatusText = Lines(0).Substring(Ret).Trim()

                Else
                    Me.Method = CType(System.Enum.Parse(GetType(HTTPMethod), FirstPart), HTTPMethod)

                End If

                Lines(0) = Lines(0).Substring(Ret).Trim()

            End If

            'If Parsing HTTP Request, Get requested URI
            If Not Me.ParseAsResponse Then
                'Parse the Http Version and the Requested Path

                Ret = Lines(0).LastIndexOf(" ")
                If (Ret > 0) Then
                    Me.Version = Lines(0).Substring(Ret).Trim()
                    Path = Lines(0).Substring(0, Ret)

                Else
                    Path = Lines(0)

                End If

            End If

        End If

        'Generate Headers
        For Cnt = 1 To Lines.Length - 1

            Ret = Lines(Cnt).IndexOf(":")
            If (Ret > 0 AndAlso Ret < Lines(Cnt).Length - 1) Then

                Try
                    Dim HeaderName, HeaderValue As String
                    HeaderName = Lines(Cnt).Substring(0, Ret)
                    HeaderValue = Lines(Cnt).Substring(Ret + 1).Trim()

                    Me.HttpResponse.AddHeader(HeaderName, HeaderValue)

                Catch ex As Exception
                    Debug.WriteLine("Bad header response...")

                End Try
            End If

        Next Cnt


        CreateUri(Path)


    End Sub


    ''' <summary>
    ''' Currrent Raw Requests protocol 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Protocol() As String
        Get
            If Me.ExpectSSL Then
                Return "https://"

            Else
                Return "http://"

            End If

        End Get
    End Property


    ''' <summary>
    ''' Try to Create Uri
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CreateUri(ByVal path As String)

        Dim URL As String = String.Empty
        If path.StartsWith("http://", StringComparison.OrdinalIgnoreCase) Then
            URL = path

        Else
            'Check Headers and combine host and stuff
            If Me.HttpResponse.Headers.ContainsKey("Host") Then
                URL = Me.Protocol & Me.HttpResponse.Headers("Host") & path
            End If

        End If


        Dim RetUri As Uri = Nothing
        Uri.TryCreate(URL, UriKind.Absolute, RetUri)

        Me.Url = RetUri

    End Sub

#End Region

    Private Shared _ReservedHeaders As List(Of String)

    ''' <summary>
    ''' Reserved HTTP Headers
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' List of HTTP Headers which can mess our requests if we add them over our requests
    ''' Thus these are never imported
    ''' </remarks>
    Public Shared Property ReserverHeaders() As List(Of String)
        Get
            If _ReservedHeaders Is Nothing Then
                _ReservedHeaders = New List(Of String)(15)
                With _ReservedHeaders
                    .Add("Cookie")
                    .Add("Connection")
                    .Add("Proxy-Connection")
                    .Add("Content-Type")
                    .Add("Content-Length")
                    .Add("Host")
                    .Add("User-Agent")
                    .Add("Accept")
                    .Add("Accept-Charset")
                    .Add("Accept-Language")
                    .Add("Referer")
                End With
            End If

            Return _ReservedHeaders
        End Get
        Set(ByVal value As List(Of String))
            _ReservedHeaders = value
        End Set
    End Property


#Region "Parse HTTP Elements"


    ''' <summary>
    ''' Parse HTTP Headers and return as a collection
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function ParseRaw(ByVal rawData As String, ByVal lineDelimiter As String, ByVal splitChar As Char) As Dictionary(Of String, String)

        Dim HeadersDic As New Dictionary(Of String, String)

        Dim Headers() As String = rawData.Split(New String() {lineDelimiter}, StringSplitOptions.RemoveEmptyEntries)

        For Each header As String In Headers

            Dim ParamName As String = Nothing
            Dim ParamValue As String = Nothing

            ParseParameter(header, ParamName, ParamValue, splitChar)

            'Skip Reserved HTTP Header
            If HttpParser.ReserverHeaders.IndexOf(ParamName) > -1 Then Continue For

            'This header already exist
            If String.IsNullOrEmpty(ParamName) OrElse HeadersDic.ContainsKey(ParamName) Then Continue For
            If ParamValue Is Nothing Then ParamValue = String.Empty
            If Not HeadersDic.ContainsKey(ParamName.TrimStart(" "c)) Then
                HeadersDic.Add(ParamName.TrimStart(" "c), ParamValue)
            End If

        Next header


        Return HeadersDic

    End Function


    ''' <summary>
    ''' Parse HTTP Headers from raw 
    ''' </summary>
    ''' <param name="rawHeaders"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ParseHeaders(ByVal rawHeaders As String) As Dictionary(Of String, String)
        Return ParseRaw(rawHeaders, vbNewLine, ":"c)
    End Function

    ''' <summary>
    ''' Parse cookies from raw
    ''' </summary>
    ''' <param name="rawHeaders"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ParseCookies(ByVal rawHeaders As String) As Dictionary(Of String, String)
        Return ParseRaw(rawHeaders, ";"c, "="c)
    End Function

    ''' <summary>
    ''' Parse POST values from raw
    ''' </summary>
    ''' <param name="rawHeaders"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ParsePost(ByVal rawHeaders As String) As Dictionary(Of String, String)
        Return ParseRaw(rawHeaders, "&"c, "="c)
    End Function

    ''' <summary>
    ''' Parse Queries values from raw
    ''' </summary>
    ''' <param name="rawHeaders"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ParseQueries(ByVal rawHeaders As String) As Dictionary(Of String, String)
        Return ParsePost(rawHeaders)
    End Function

    ''' <summary>
    ''' Parses the specified name.
    ''' </summary>
    ''' <param name="RawString">The raw string.</param>
    ''' <param name="Name">The name.</param>
    ''' <param name="Value">The value.</param>
    Public Shared Sub ParseParameter(ByVal RawString As String, ByRef Name As String, ByRef Value As String, Optional ByVal Identifier As Char = "="c)
        Dim ValPos As Integer = RawString.IndexOf(Identifier)

        'Not Found
        If ValPos = -1 Then Exit Sub

        Name = RawString.Substring(0, ValPos)
        Value = RawString.Substring(ValPos + 1, RawString.Length - ValPos - 1)
    End Sub

#End Region

End Class