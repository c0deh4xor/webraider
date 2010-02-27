Imports System.Net
Imports System.IO
Imports System.Text
Imports Logger

''' <summary>
''' Make Request and fire up related events
''' </summary>
''' <remarks></remarks>
Public Class HttpRequest


#Region "Properties"
    Private _UriManager As UriManager

    ''' <summary>
    ''' WebRequest and every other stuff in here to accomplish a successful request
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UriManager() As UriManager
        Get
            Return _UriManager
        End Get
        Set(ByVal value As UriManager)
            _UriManager = value
        End Set
    End Property

    Private _Link As Link
    ''' <summary>
    ''' Gets or sets the link.
    ''' </summary>
    ''' <value>The link.</value>
    Public Property Link() As Link
        Get
            Return _Link
        End Get
        Set(ByVal value As Link)
            _Link = value
        End Set
    End Property

    Dim WebRequest As HttpWebRequest

#End Region

#Region "Enums"
    ''' <summary>
    ''' Http Methods
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum HttpMethod
        [Get] = 0 'Default Method
        Post
        Head
        Put
        Trace
        Options
        Delete
        PostMultiPart
    End Enum

#End Region

#Region "Constructors"

    Private Shared HTTPSettingsConfigured As Boolean

    Private Shared Sub SetHttpSettings()
        If HTTPSettingsConfigured Then Exit Sub

        HTTPSettingsConfigured = True

        'TODO: Can we extract these?
        ServicePointManager.ServerCertificateValidationCallback = New Net.Security.RemoteCertificateValidationCallback(AddressOf Helpers.ValidateCertificate)

        Net.ServicePointManager.DefaultConnectionLimit = 20
        Net.ServicePointManager.UseNagleAlgorithm = True
    End Sub

    ''' <summary>
    ''' New HttpRequest
    ''' </summary>
    ''' <param name="UriManager"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal uriManager As UriManager)
        Me.UriManager = uriManager
        SetHttpSettings()
    End Sub

    ''' <summary>
    ''' New HttpRequest from a Link Reference
    ''' </summary>
    ''' <param name="Link"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal link As Link)
        Me.New(link.UriManager)
        Me.Link = link
    End Sub
#End Region

#Region "Main Methods"

    ''' <summary>
    ''' Make a request
    ''' </summary>
    ''' <remarks></remarks>
    Public Function Request() As AttackResponse
        WebRequest = UriManager.GetWebRequest()

        'Check WebRequest
        If WebRequest Is Nothing Then
            Debug.Assert(False, "Request is nothing ?")
            Log.Instance.Write("Request is nothing ?", Log.LogCategory.Error)
            Return Nothing
        End If


        'Exit by default in exception
        Dim ExitFunction As Boolean = True

        Dim HttpRes As HttpWebResponse = Nothing
        Dim RequestTime As DateTime = DateTime.UtcNow
        Dim SourceCode As String = String.Empty


        Dim RawResponse As String = String.Empty

        Dim FailReason As String = String.Empty

        Try
            HttpRes = CType(WebRequest.GetResponse(), HttpWebResponse)
            ExitFunction = False

        Catch WebEx As Net.WebException 'Handle Web Exceptions

            Select Case WebEx.Status
                Case WebExceptionStatus.Timeout

                Case WebExceptionStatus.ProtocolError 'Status 500, 401 etc.
                    HttpRes = DirectCast(WebEx.Response, HttpWebResponse)
                    ExitFunction = False

                Case WebExceptionStatus.ConnectFailure
                    If Link IsNot Nothing Then
                        ExitFunction = True

                        FailReason = "Connection Failure"
                    End If

                Case WebExceptionStatus.TrustFailure
                    ExitFunction = True

                    FailReason = "Certificate Error"

                Case WebExceptionStatus.ServerProtocolViolation
                    ExitFunction = True

                    FailReason = "Server Protocol Violation"

                Case Else
                    ExitFunction = True

                    FailReason = "Unhandled : " & WebEx.Status.ToString()

            End Select

        Catch Ex As InvalidOperationException
            Log.Instance.WriteException(Ex, "Possible out of connections")
            ExitFunction = True


        Finally 'WebException

            Try
                If ExitFunction Then Exit Try

                Debug.Assert(Not HttpRes Is Nothing, "We shouldn't be in here if Response is nothing !")

                Try
                    Using SReader As StreamReader = New StreamReader(HttpRes.GetResponseStream)
                        SourceCode = SReader.ReadToEnd()
                    End Using


                Catch ex As Exception
                    'Stream Read Error etc. Summary: Fucked up!
                    Log.Instance.WriteException(ex, "Response stream error.")
                    ExitFunction = True

                    FailReason = "Error while reading the stream"

                End Try


                If Not ExitFunction Then
                    RawResponse = ResponseAsRaw(HttpRes)
                    Try
                        UpdateLinkStatus(HttpRes, RequestTime, SourceCode, Me.RequestAsRow(), RawResponse)

                    Catch ex As Data.DataException
                        Log.Instance.WriteException(ex, "Database ?")


                    Catch ex As Exception
                        Log.Instance.WriteException(ex, "What now?")


                    End Try

                End If

            Catch ex As Exception
                Log.Instance.WriteException(ex, "Web Response Exception!")

            Finally
                If HttpRes IsNot Nothing Then HttpRes.Close()

            End Try


        End Try 'Webexception

        'Just leave if we need to exit
        If ExitFunction Then Return Nothing

        Dim ResponseTime As Double = DateTime.UtcNow.Subtract(RequestTime).TotalMilliseconds

        'Prepare Response
        Dim Cookies(HttpRes.Cookies.Count - 1) As Net.Cookie
        HttpRes.Cookies.CopyTo(Cookies, 0)

        Dim AttackRes As New AttackResponse(HttpRes.ResponseUri, HttpRes.StatusCode, SourceCode, UriManager, Link, ResponseTime, Me.RequestAsRow, RawResponse) With {.Headers = HttpRes.Headers, .Cookies = Cookies}

        Return AttackRes
    End Function

    ''' <summary>
    ''' Update Link Status
    ''' </summary>
    ''' <param name="httpResponse">The HTTP response.</param>
    ''' <param name="requestTime">The request time.</param>
    ''' <param name="SourceCode">The source code.</param>
    ''' <remarks></remarks>
    Private Sub UpdateLinkStatus(ByVal httpResponse As HttpWebResponse, ByVal requestTime As DateTime, ByVal SourceCode As String, ByVal rawRequest As String, ByVal rawResponse As String)

        'Just Quit if this is called from Attacker
        If Link Is Nothing Then Exit Sub

        With Link

            .ResponseTime = DateTime.UtcNow.Subtract(requestTime).TotalMilliseconds
            .StatusCode = httpResponse.StatusCode
            Select Case httpResponse.StatusCode

                Case HttpStatusCode.Redirect, HttpStatusCode.RedirectKeepVerb, HttpStatusCode.RedirectMethod, HttpStatusCode.Moved, HttpStatusCode.MovedPermanently, HttpStatusCode.MultipleChoices

                    Try
                        Link.UpdateUri(New Uri(Me.Link.UriManager.Uri, httpResponse.Headers.Get("Location")))

                    Catch ex As Exception
                        Log.Instance.WriteException(ex, "Redirect Location error or something...")

                    End Try

                    Exit Sub

                Case HttpStatusCode.OK, HttpStatusCode.InternalServerError
                Case HttpStatusCode.Found
                Case HttpStatusCode.NotFound
                Case HttpStatusCode.Forbidden
                Case Else
            End Select
        End With


    End Sub


    Private _ResponseAsRaw As String

    ''' <summary>
    ''' Responses as raw.
    ''' </summary>
    ''' <param name="httpResponse">The HTTP response.</param>
    ''' <returns></returns>
    Private Function ResponseAsRaw(ByVal httpResponse As HttpWebResponse) As String

        If Not String.IsNullOrEmpty(_ResponseAsRaw) Then
            Return _ResponseAsRaw
        End If

        Dim RawResponse As New Text.StringBuilder(1000)


        With RawResponse

            .Append("HTTP/")
            .Append(httpResponse.ProtocolVersion)
            .Append(" ")
            .Append(Convert.ToInt32(httpResponse.StatusCode))
            .Append(" ")
            .Append(httpResponse.StatusDescription)
            .AppendLine()


            For HeaderIndex As Integer = 0 To httpResponse.Headers.Count - 1

                .Append(httpResponse.Headers.Keys(HeaderIndex))
                .Append(": ")
                .Append(httpResponse.Headers(HeaderIndex))
                .AppendLine()

            Next HeaderIndex

            .AppendLine()
            .AppendLine()

        End With

        _ResponseAsRaw = RawResponse.ToString()

        Return _ResponseAsRaw
    End Function


    Private _RequestAsRow As String

    ''' <summary>
    ''' String representation of HTTP Request
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' This is a ridiculous function and trying to convert and HTTP Request to a pseudo Raw Request.
    ''' .NET API doesn't allow us to access raw request we are trying to reconstruct in here as much as possible.
    ''' </remarks>
    Public Function RequestAsRow() As String

        If Not String.IsNullOrEmpty(_RequestAsRow) Then
            Return _RequestAsRow
        End If

        Dim RetRawReq As New Text.StringBuilder(1000)

        With RetRawReq

            .Append(Me.WebRequest.Method)
            .Append(" ")

            .Append(Me.WebRequest.RequestUri.PathAndQuery)
            .Append(" ")

            .Append("HTTP/")
            .Append(Me.WebRequest.ProtocolVersion)

            .AppendLine()


            'Headers
            For Each Header As String In Me.WebRequest.Headers
                .Append(Header)
                .Append(": ")
                .AppendLine(Me.WebRequest.Headers.Get(Header))
            Next Header

            If Me.WebRequest.Method = "POST" Then
                .AppendLine()
                .AppendLine(Me.UriManager.GeneratePostData())
            End If

        End With

        _RequestAsRow = RetRawReq.ToString

        Return _RequestAsRow
    End Function

#End Region


End Class
