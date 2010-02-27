Imports System.Net
Imports System.Text
Imports System.IO
Imports Logger

''' <summary>
''' Uri Manager
''' Responsible for preparing all kind of Requests
''' </summary>
''' <remarks>
''' </remarks>
<Serializable()> _
Public Class UriManager

#Region "Constants"
    ''' <summary>
    ''' Querystring Split Constant
    ''' </summary>
    ''' <remarks></remarks>
    Private Const QUERY_SPLITTER As Char = "&"c

#End Region

#Region "Constructors"

    ''' <summary>
    ''' New Uri Manager from an old Uri Manager
    ''' </summary>
    ''' <param name="UriManager"></param>
    ''' <remarks>Instead of set this as a structure we use this constructor to make it a bit cheap , so we can still generate new class from a previous one without care about is it in stack or heap (basicly we don't want to refer it we really want to duplicate it)</remarks>
    Public Sub New(ByVal uriManager As UriManager)

        If uriManager.Params.Count > 0 Then
            For i As Integer = 0 To uriManager.Params.Count - 1
                AddParameter(uriManager.Params(i))
            Next i
        End If

        Me.Uri = uriManager.Uri
        Me.Referrer = uriManager.Referrer
    End Sub

    ''' <summary>
    ''' New UriManager
    ''' </summary>
    ''' <param name="URL">The URL.</param>
    Public Sub New(ByVal url As String)
        Me.New(New Uri(url))
    End Sub

    ''' <summary>
    ''' New Uri Manager
    ''' </summary>
    ''' <param name="Uri">The URI.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal uri As Uri)
        Me.Uri = uri
        LoadURL()
    End Sub


    ''' <summary>
    ''' Initializes a new instance of the <see cref="UriManager" /> class.
    ''' </summary>
    ''' <param name="URL">The URL.</param>
    ''' <param name="Referrer">The referrer.</param>
    Public Sub New(ByVal url As String, ByVal referrer As Uri)
        Me.New(url)
        Me.Referrer = referrer
    End Sub

    ''' <summary>
    ''' Initializes a new instance of the <see cref="UriManager" /> class.
    ''' </summary>
    ''' <param name="URL">The URL.</param>
    ''' <param name="Referrer">The referrer.</param>
    Public Sub New(ByVal url As Uri, ByVal referrer As Uri)
        Me.New(url)
        Me.Referrer = referrer
    End Sub

#End Region

#Region "Properties"

    Public Sub New()

    End Sub

    Private _Uri As Uri
    ''' <summary>
    ''' Uri information of UriManager
    ''' </summary>
    Public Property Uri() As Uri
        Get
            Return _Uri
        End Get
        Set(ByVal value As Uri)
            _Uri = value
        End Set
    End Property

    Private _IncludeFormData As Boolean
    ''' <summary>
    ''' Include any POST parameter
    ''' </summary>
    Public ReadOnly Property IncludeFormData() As Boolean
        Get
            Return _IncludeFormData
        End Get
    End Property

    Private _IncludeQueryParams As Boolean
    ''' <summary>
    ''' Include any Querystring
    ''' </summary>
    Public ReadOnly Property IncludeQueryParams() As Boolean
        Get
            Return _IncludeQueryParams
        End Get
    End Property

    Private _IncludeFullQueryString As Boolean
    ''' <summary>
    ''' Include a Full Query String
    ''' </summary>
    Public ReadOnly Property IncludeFullQuerystring() As Boolean
        Get
            Return _IncludeFullQueryString
        End Get
    End Property


    Private _Referrer As Uri
    ''' <summary>
    ''' Gets or sets the referrer.
    ''' </summary>
    ''' <value>The referrer.</value>
    Public Property Referrer() As Uri
        Get
            Return _Referrer
        End Get
        Set(ByVal value As Uri)
            _Referrer = value
        End Set
    End Property

    Private _Params As List(Of Parameter)
    ''' <summary>
    ''' Parameters Dictionary Collection
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Params() As List(Of Parameter)
        Get
            If _Params Is Nothing Then _Params = New List(Of Parameter)
            Return _Params
        End Get
    End Property


    ''' <summary>
    ''' Parameter keys array of all queries just for signature generation purpoeses. All post parameters have <see cref="Parameter.POSTPREFIX">POSTPREFIX</see> prefix.
    ''' </summary>
    ''' <value>The param keys.</value>
    ''' <returns>Names of all <see cref="Parameter">Parameters</see></returns>
    ''' <remarks>Always sorted</remarks>
    ''' REFACTOR: Cache data so do not generate it again until it will change
    Public ReadOnly Property ParamKeys() As String()
        Get
            Dim StringParams(Me.Params.Count) As String
            Dim i As Integer = 0

            For Each Parameter As Parameter In Params
                StringParams(i) = Parameter.KeyName
                i = +1
            Next Parameter

            Array.Sort(StringParams)

            Return StringParams
        End Get
    End Property

    Private _AttackPattern As AttackPattern
    ''' <summary>
    ''' Gets or sets the attack string.
    ''' </summary>
    ''' <value>The attack string.</value>
    Public Property AttackPattern() As AttackPattern
        Get
            Return _AttackPattern
        End Get
        Set(ByVal value As AttackPattern)
            _AttackPattern = value
        End Set
    End Property

    Private _AttackParameter As Parameter
    ''' <summary>
    ''' Gets or sets the attack parameter.
    ''' </summary>
    ''' <value>The attack parameter.</value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AttackParameter() As Parameter
        Get
            Return _AttackParameter
        End Get
        Set(ByVal value As Parameter)
            _AttackParameter = value
        End Set
    End Property

    Dim WebReq As HttpWebRequest
#End Region

#Region "Methods"

    ''' <summary>
    ''' Add New Post Parameter
    ''' </summary>
    ''' <param name="name">The name.</param>
    ''' <param name="value">The value.</param>
    ''' <remarks></remarks>
    Public Sub AddPostParameter(ByVal name As String, ByVal value As String)
        'FIX:Dont add prefix blindly !
        AddPostParameter(New Parameter(Parameter.POSTPREFIX & name, value))
    End Sub

    ''' <summary>
    ''' Add or Replace parameter
    ''' </summary>
    ''' <param name="parameter">The parameter.</param>
    ''' <remarks></remarks>
    Public Sub AddParameter(ByVal parameter As Parameter)
        AddNewParameter(parameter)
    End Sub

    ''' <summary>
    ''' Add New Post Parameter
    ''' </summary>
    ''' <param name="Parameter"></param>
    ''' <remarks>
    ''' Replace old value with the new one if found already, add new if has not added yet
    ''' </remarks>
    Public Sub AddPostParameter(ByVal parameter As Parameter)

        parameter.Type = parameter.ParameterType.Post
        AddNewParameter(parameter)
    End Sub

    ''' <summary>
    ''' Add new Querystring Parameter
    ''' </summary>
    ''' <param name="Name">Parameter Name</param>
    ''' <param name="Value">Parameter Value</param>
    ''' <remarks></remarks>
    Public Sub AddQueryParameter(ByVal name As String, ByVal value As String)
        AddQueryParameter(New Parameter(name, value))
    End Sub

    ''' <summary>
    ''' Add New Querystring Parameter
    ''' </summary>
    ''' <param name="Parameter">Querystring Parameter</param>
    ''' <remarks></remarks>
    Public Sub AddQueryParameter(ByVal parameter As Parameter)
        AddNewParameter(parameter)
    End Sub


    ''' <summary>
    ''' Add new parameter
    ''' </summary>
    ''' <param name="Parameter">Parameter to add</param>
    ''' <remarks>Parameter type should be set before send. Default Parameter type is GET</remarks>
    Private Sub AddNewParameter(ByVal parameter As Parameter)

        'Update include fields
        Select Case parameter.Type
            Case parameter.ParameterType.Post
                _IncludeFormData = True

            Case parameter.ParameterType.Querystring
                _IncludeQueryParams = True

            Case parameter.ParameterType.FullQueryString
                _IncludeFullQueryString = True

            Case SharedLibrary.Parameter.ParameterType.Upload
                _UploadForm = True

        End Select


        ''Skip if it's empty
        'If Params.Count > 0 Then

        'Check if already exist
        For i As Integer = 0 To Params.Count - 1
            Dim OldParam As Parameter = Params(i)

            If OldParam.KeyName.Equals(parameter.KeyName, StringComparison.OrdinalIgnoreCase) Then

                'Update Parameter and Exit
                Params(i) = parameter
                Exit Sub

            End If

        Next i

        'End If

        'Add as new parameter
        Params.Add(parameter)
    End Sub

    ''' <summary>
    ''' Build Web Request
    ''' </summary>
    ''' <returns>New WebRequest</returns>
    ''' <remarks></remarks>
    Public Function GetWebRequest() As HttpWebRequest
        RefreshUri()

        Try
            WebReq = CType(HttpWebRequest.Create(Me.Uri), HttpWebRequest)

        Catch ex As NotSupportedException
            Return Nothing

        End Try
        WebReq.AllowAutoRedirect = True

        WebReq.Timeout = 5000

        'Add Referrer
        If Me.Referrer IsNot Nothing Then
            WebReq.Referer = Referrer.AbsoluteUri
        End If

        WebReq.AllowAutoRedirect = False


        'Cache Control - Do not cache
        WebReq.Headers.Add(HttpRequestHeader.CacheControl, "no-cache")

        'Choose Post Method
        If Me.UploadForm Then
            Dim Boundary As String = "----------------------------" + DateTime.Now.Ticks.ToString("x")
            WebReq.Method = HttpRequest.HttpMethod.Post.ToString
            WebReq.ContentType = "multipart/form-data; boundary=" & Boundary
            WebReq.KeepAlive = True

            Dim memStream As New System.IO.MemoryStream()

            Dim boundarybytes As Byte() = System.Text.Encoding.ASCII.GetBytes(vbNewLine & "--" & Boundary & vbNewLine)
            Dim formdataTemplate As String = vbNewLine & "--" + Boundary & vbNewLine & "Content-Disposition: form-data; name=""{0}"";" & vbNewLine & vbNewLine & "{1}"

            Dim PostParams = From PParam In Me.Params Where PParam.Type = Parameter.ParameterType.Post
            For Each PostParameter As Parameter In PostParams

                Dim formitem As String = String.Format(formdataTemplate, PostParameter.Name, PostParameter.Value)
                Dim formitembytes As Byte() = System.Text.Encoding.UTF8.GetBytes(formitem)
                memStream.Write(formitembytes, 0, formitembytes.Length)

            Next

            memStream.Write(boundarybytes, 0, boundarybytes.Length)

            Dim headerTemplate As String = "Content-Disposition: form-data; name=""{0}""; filename=""{1}""" & vbNewLine & " Content-Type: application/octet-stream" & vbNewLine & vbNewLine

            For Each FileParameter As Parameter In Me.Params.Where(Function(f) f.Type = Parameter.ParameterType.Upload)

                'No file to upload, Skip
                If Not IO.File.Exists(FileParameter.Value) Then Continue For


                'TODO: Change this to support multiple files with different input names
                Dim header As String = String.Format(headerTemplate, FileParameter.Name, FileParameter.Value)

                Dim headerbytes As Byte() = System.Text.Encoding.UTF8.GetBytes(header)

                memStream.Write(headerbytes, 0, headerbytes.Length)

                Dim fileStreamx As New FileStream(FileParameter.Value, FileMode.Open, FileAccess.Read)
                Dim buffer(1024 - 1) As Byte 'byte[] buffer = new byte[1024];

                Dim bytesRead As Integer = -1

                While (bytesRead <> 0)
                    bytesRead = fileStreamx.Read(buffer, 0, buffer.Length)
                    memStream.Write(buffer, 0, bytesRead)
                End While


                memStream.Write(boundarybytes, 0, boundarybytes.Length)
                fileStreamx.Close()

            Next


            WebReq.ContentLength = memStream.Length

            Dim requestStream As Stream = WebReq.GetRequestStream()

            memStream.Position = 0

            Dim tempBuffer(CInt(memStream.Length - 1)) As Byte
            memStream.Read(tempBuffer, 0, tempBuffer.Length)
            memStream.Close()

            requestStream.Write(tempBuffer, 0, tempBuffer.Length)
            requestStream.Close()

        ElseIf Me.IncludeFormData Then
            WebReq.Method = HttpRequest.HttpMethod.Post.ToString

            Dim PostData As String = GeneratePostData()

            WebReq.ContentType = "application/x-www-form-urlencoded"
            WebReq.ContentLength = PostData.Length

            Dim ReqStream As Stream = Nothing
            Try
                ReqStream = WebReq.GetRequestStream()
                Dim PostByte As Byte() = Encoding.ASCII.GetBytes(PostData)
                ReqStream.Write(PostByte, 0, PostByte.Length)

            Catch WebEx As Net.WebException
                Log.Instance.WriteException(WebEx, "Form GetRequestStream() Error !")

            Finally
                If ReqStream IsNot Nothing Then ReqStream.Close()

            End Try

        Else 'GET Request

            WebReq.Method = HttpRequest.HttpMethod.Get.ToString
            WebReq.KeepAlive = True

        End If

        Return WebReq
    End Function

    Private _UploadForm As Boolean
    Private Property UploadForm() As Boolean
        Get
            Return _UploadForm
        End Get
        Set(ByVal value As Boolean)
            _UploadForm = value
        End Set
    End Property

    ''' <summary>
    ''' Generate Post Data for request from PostParameters
    ''' </summary>
    ''' <returns>Suitable postdata stream for a WebRequest</returns>
    ''' <remarks></remarks>
    Public Function GeneratePostData() As String
        Dim PostData As String = String.Empty

        'Generate Post Data
        For Each PostParam As Parameter In Params
            If PostParam.Type = Parameter.ParameterType.Post Then
                PostParam.UrlEncode()
                PostData &= PostParam.Name & "=" & PostParam.Value & "&"
            End If
        Next PostParam

        'Fix latest (&)
        If Right(PostData, 1) = "&" Then PostData = PostData.Remove(PostData.Length - 1, 1)

        Return PostData
    End Function

    ''' <summary>
    ''' Refresh Uri Property with all queries
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub RefreshUri()

        'If there is any parameter
        If Not IncludeQueryParams AndAlso Not IncludeFullQuerystring Then Exit Sub

        'Setup root part
        Dim URL As String = Uri.GetLeftPart(UriPartial.Path)
        URL &= "?"

        'Generate QueryString
        For i As Integer = 0 To Params.Count - 1

            Dim QueryParam As Parameter = Params(i)
            If QueryParam.Type = Parameter.ParameterType.Querystring Then
                URL &= QueryParam.Name & "=" & QueryParam.Value & "&"
            End If

        Next i

        If Not IncludeFullQuerystring Then
            'Fix latest (&)
            If Right(URL, 1) = "&" Then URL = URL.Remove(URL.Length - 1, 1)
        End If

        'Add FullQueryParam
        For Each Param As Parameter In Params
            If Param.Type = Parameter.ParameterType.FullQueryString Then
                URL &= Param.Value
            End If
        Next

        'Update Property
        Me.Uri = New Uri(URL)
    End Sub


#End Region

    ''' <summary>
    ''' Load current URL and Parse Querystrings
    ''' </summary>
    ''' <remarks>
    ''' Also initiliaze QueryParams and PostParams Dictionaries
    ''' Supports Full query string parameters like "/?VALUE"
    ''' </remarks>
    Private Sub LoadURL()

        If Me.Uri.Query.Length < 2 Then
            Exit Sub
        End If

        Dim QueryArr() As String = Me.Uri.Query.Remove(0, 1).Split(QUERY_SPLITTER)

        For i As Integer = 0 To QueryArr.Length - 1
            Dim QName, QValue As String
            HttpParser.ParseParameter(QueryArr(i), QName, QValue, "="c)

            If Not String.IsNullOrEmpty(QValue) Then
                Me.AddQueryParameter(QName, QValue)

            Else
                Me.AddQueryParameter(New Parameter(QName))

            End If
        Next i

    End Sub




End Class
