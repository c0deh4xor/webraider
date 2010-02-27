Imports System.Text.RegularExpressions

''' <summary>
''' Attack Pattern
''' </summary>
''' <remarks>
''' Stores information for Attack Patterns
''' </remarks>
<Serializable()> _
Public Class AttackPattern

    Public Const ATTACK_HOLDER As String = "{{attack}}"

    ''' <summary>
    ''' Generic holder for dynamic values in attacks. Automatically only works for pattern if there is payload node in the same attack XML node.
    ''' </summary>
    ''' <remarks></remarks>
    Public Const PAYLOAD_HOLDER As String = "{{payload}}"

    ''' <summary>
    ''' This will be replaced with the actual crawled parameter in the request during the attack process. It should be in the attack node to work.
    ''' </summary>
    ''' <remarks></remarks>
    Public Const CURRENTVALUE_HOLDER As String = "{{parameter}}"

#Region "Public Enums"

    ''' <summary>
    ''' Payload Encoders
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum PayloadEncoder As Integer
        ''' <summary>
        ''' No encoding
        ''' </summary>
        ''' <remarks></remarks>
        None = 0


        ''' <summary>
        ''' MS SQL Char() style encoding to avoid single quotes
        ''' </summary>
        ''' <remarks></remarks>
        MSSQLChar = 1

        ''' <summary>
        ''' MySQL Benchmark encoding which translates seconds into number to iterate certain function. Results depends based on the target server.
        ''' </summary>
        ''' <remarks></remarks>
        MySQLBenchmark = 2

    End Enum




#End Region


#Region "Properties"

    Private _OriginalValue As String

    ''' <summary>
    ''' Gets or sets the original value of Parameter.
    ''' </summary>
    ''' <value>The original value.</value>
    ''' <remarks>DEV : This can be moved into AttackParameter instead of AttackPattern</remarks>
    Public Property OriginalValue() As String
        Get
            Return _OriginalValue
        End Get
        Set(ByVal value As String)
            _OriginalValue = value
        End Set
    End Property

    Private _Encoder As PayloadEncoder

    ''' <summary>
    ''' Gets or sets the payload encoder.
    ''' </summary>
    ''' <value>The payload encoder.</value>
    Public Property Encoder() As PayloadEncoder
        Get
            Return _Encoder
        End Get
        Set(ByVal value As PayloadEncoder)
            _Encoder = value
        End Set
    End Property


    Private _pattern As String

    ''' <summary>
    ''' Gets or sets the pattern.
    ''' </summary>
    ''' <value>The pattern.</value>
    Public Property Pattern() As String
        Get

            'Fill Payload and return
            If Not String.IsNullOrEmpty(Me.Payload) Then
                Return _pattern.Replace(AttackPattern.PAYLOAD_HOLDER, Me.Payload)
            End If

            Return _pattern
        End Get
        Set(ByVal value As String)
            _pattern = value
        End Set
    End Property


    Private _Payload As String
    ''' <summary>
    ''' JS Payload
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Payload() As String
        Get
            Return _Payload
        End Get
        Set(ByVal value As String)
            _Payload = value
        End Set
    End Property


    'Private _signature As String

    '''' <summary>
    '''' Gets or sets the signature.
    '''' </summary>
    '''' <value>The signature.</value>
    '''' <remarks>Signatures are RegExes to test SQL Injection</remarks>
    'Public Property Signature() As String
    '    Get
    '        Return _signature
    '    End Get
    '    Set(ByVal value As String)

    '        'Update RegEx when signature change
    '        Me.CompiledRegEx = Nothing
    '        _signature = value
    '    End Set
    'End Property

    Private _name As String

    ''' <summary>
    ''' Gets or sets the name.
    ''' </summary>
    ''' <value>The name.</value>
    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property


    Private _spesificType As Integer
    ''' <summary>
    ''' Gets or sets the spesific.
    ''' </summary>
    ''' <value>The spesific.</value>
    Public Property SpesificType() As Integer
        Get
            Return _spesificType
        End Get
        Set(ByVal value As Integer)
            _spesificType = value
        End Set
    End Property

#End Region

    ''' <summary>
    ''' Initializes a new instance of the <see cref="AttackPattern" /> class.
    ''' </summary>
    ''' <param name="name">The name.</param>
    ''' <param name="pattern">The pattern.</param>
    Public Sub New(ByVal name As String, ByVal pattern As String)
        Me.Name = name
        Me.Pattern = pattern
    End Sub

    ''' <summary>
    ''' Initializes a new instance of the <see cref="AttackPattern" /> class.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
    End Sub

End Class
