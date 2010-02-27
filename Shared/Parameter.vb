''' <summary>
''' Query or Post Parameter
''' </summary>
''' <remarks></remarks>
<Serializable()> _
Public Structure Parameter

    Public Const POSTPREFIX As String = "#POST#"

    ''' <summary>
    ''' Parameter Name 
    ''' </summary>
    ''' <remarks>See <see>KeyName</see> to get name for collections.</remarks>
    Private _Name As String
    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property

    ''' <summary>
    ''' KeyName
    ''' </summary>
    ''' <remarks>Always unique. Key name of post parameters, Usefull using with collections.</remarks>
    Public ReadOnly Property KeyName() As String
        Get
            If Type = ParameterType.Post OrElse Type = ParameterType.Upload Then
                Return POSTPREFIX & Name

            Else
                Return Name

            End If
        End Get
    End Property


    Private _values As List(Of String)
    ''' <summary>
    ''' Gets or sets the values. This is used for parameters which have more than one value such as select/radio/checkbox etc.
    ''' </summary>
    ''' <value>The values.</value>
    Public Property Values() As List(Of String)
        Get
            Return _values
        End Get
        Set(ByVal value As List(Of String))
            _values = value
        End Set
    End Property



    ''' <summary>
    ''' Parameter Type 
    ''' </summary>
    ''' <remarks></remarks>
    Private _Type As ParameterType
    Public Property Type() As ParameterType
        Get
            Return _Type
        End Get
        Set(ByVal value As ParameterType)
            _Type = value
        End Set
    End Property

    ''' <summary>
    ''' Parameter Types
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum ParameterType
        Querystring
        Post
        FullQueryString
        Upload
    End Enum

    ''' <summary>
    ''' Parameter Value
    ''' </summary>
    ''' <remarks></remarks>
    Private _value As String

    Public Property Value() As String
        Get
            Return _value
        End Get
        Set(ByVal value As String)
            _value = value
        End Set
    End Property

    ''' <summary>
    ''' New Query or Post Parameter (default querystring)
    ''' </summary>
    ''' <param name="Name">Name of parameter</param>
    ''' <param name="Value">Value of parameter</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal name As String, ByVal value As String)

        Me.Value = value

        If name.StartsWith(POSTPREFIX) Then
            Me.Type = ParameterType.Post
            Me.Name = name.Remove(0, POSTPREFIX.Length)

        Else
            Me.Type = ParameterType.Querystring
            Me.Name = name

        End If

    End Sub

    ''' <summary>
    ''' New Parameter with a specified type
    ''' </summary>
    ''' <param name="Name">Name of parameter</param>
    ''' <param name="Value">Value of parameter</param>
    ''' <param name="Type">Type of parameter</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal name As String, ByVal value As String, ByVal type As ParameterType)
        Me.New(name, value)
        Me.Type = type
    End Sub

    ''' <summary>
    ''' New Parameter as Full Query String
    ''' </summary>
    ''' <param name="FullQueryString"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal fullQueryString As String)
        Me.Name = String.Empty
        Me.Value = fullQueryString
        Me.Type = ParameterType.FullQueryString

    End Sub

    Private _isUrlEncoded As Boolean
    Public ReadOnly Property IsUrlEncoded() As Boolean
        Get
            Return _isUrlEncoded
        End Get
    End Property

    Public Sub UrlEncode()
        If (IsUrlEncoded = False) Then
            Me.Value = Web.HttpUtility.UrlEncode(Me.Value)
            _isUrlEncoded = True
        End If
    End Sub

    Public Sub UrlDecode()
        If (IsUrlEncoded) Then
            Me.Value = Web.HttpUtility.UrlDecode(Me.Value)
            _isUrlEncoded = False
        End If
    End Sub

End Structure