Imports System.Windows.Forms

Namespace WR

    ''' <summary>
    ''' Form Input Structure
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure Input

        ''' <summary>
        ''' Input type (also includes Textarea and Select)
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum InputType
            Hidden
            Text
            Textarea
            Submit
            Reset
            Button
            Image
            File
            Radio
            [Select]
            Checkbox
            Password
        End Enum

        Public AutoCompleteEnabled As Boolean
        ''' <summary>
        ''' Initializes a new instance of the <see cref="Input" /> struct based on an Input HTMLElement.
        ''' </summary>
        ''' <param name="inputElement">The input element.</param>
        Public Sub New(ByVal inputElement As HtmlElement)
            AutoCompleteEnabled = Not inputElement.GetAttribute("autocomplete").ToLowerInvariant() = "off"

            Dim Type As String = DirectCast(inputElement.GetAttribute("type"), String)
            If Not String.IsNullOrEmpty(Type) Then
                Try
                    Me.Type = CType([Enum].Parse(GetType(InputType), Type, True), InputType)
                Catch ex As Exception
                    Debug.WriteLine("Weird type!")

                End Try
            End If

            Me.Value = inputElement.GetAttribute("value")
            Me.Name = inputElement.GetAttribute("name")

        End Sub





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

        Private _value As String

        ''' <summary>
        ''' Gets or sets the value.
        ''' </summary>
        ''' <value>The value.</value>
        Public Property Value() As String
            Get
                'If value is empty return first values from values
                If _value Is Nothing Then

                    'Check Values
                    If values IsNot Nothing Then
                        _value = DirectCast(values(0), String)
                    End If

                End If

                Return _value
            End Get
            Set(ByVal value As String)
                _value = value
            End Set
        End Property


        Private _type As InputType

        ''' <summary>
        ''' Gets or sets the type.
        ''' </summary>
        ''' <value>The type.</value>
        Public Property Type() As InputType
            Get
                Return _type
            End Get
            Set(ByVal value As InputType)
                _type = value
            End Set
        End Property

        ''' <summary>
        ''' Returns the fully qualified type name of this instance.
        ''' </summary>
        ''' <returns>
        ''' A <see cref="T:System.String"></see> containing a fully qualified type name.
        ''' </returns>
        Public Overrides Function ToString() As String
            Return String.Format("Name: {0}, Value: {1}, Type:{2}", Me.Name, Me.Value, Me.Type)
        End Function


        Private values As ArrayList

        Public ReadOnly Property ValuesProperty() As ArrayList
            Get
                Return values
            End Get
        End Property

        ''' <summary>
        ''' Add new value to values
        ''' </summary>
        ''' <remarks>
        ''' Currently do not store labels
        ''' </remarks>
        Public Sub AddValue(ByVal value As String)

            If Me.values Is Nothing Then Me.values = New ArrayList()
            If value Is Nothing Then value = String.Empty

            Me.values.Add(value)

        End Sub



    End Structure

End Namespace