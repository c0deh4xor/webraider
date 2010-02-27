Imports System.Windows.Forms
Imports Logger
Imports WebRaider.SharedLibrary

Namespace WR

    ''' <summary>
    ''' Form structure, stores inputs, form information
    ''' </summary>
    Public Class Form

        Public Function GetAsLink(ByVal attackResponse As AttackResponse) As Link

            'New Link based on Form Location
            Dim Link As New Link(New Uri(attackResponse.UriManager.Uri, Me.Action))

            'Add parameters
            For Each FormInput As KeyValuePair(Of String, Input) In Me.Inputs

                If FormInput.Value.Type = Input.InputType.File Then
                    Link.UriManager.AddParameter(New Parameter(FormInput.Value.Name, FormInput.Value.Value, Parameter.ParameterType.Upload))
                Else

                    Select Case Me.Method
                        Case WR.Form.FormMethod.GET
                            Link.UriManager.AddQueryParameter(FormInput.Value.Name, FormInput.Value.Value)

                        Case WR.Form.FormMethod.POST, FormMethod.MultiPart
                            Link.UriManager.AddPostParameter(FormInput.Value.Name, FormInput.Value.Value)

                    End Select


                End If

            Next FormInput

            Return Link
        End Function

        ''' <summary>
        ''' Form Methods
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum FormMethod
            [GET] = 0
            POST
            MultiPart 'Upload
        End Enum

        Private _method As FormMethod

        ''' <summary>
        ''' Gets or sets the form method.
        ''' </summary>
        ''' <value>The method.</value>
        Public Property Method() As FormMethod
            Get
                Return _method
            End Get
            Set(ByVal value As FormMethod)
                _method = value
            End Set
        End Property

        ''' <summary>
        ''' Initializes a new instance of the Form class.
        ''' </summary>
        Public Sub New()
        End Sub

        Public AutoCompleteEnabled As Boolean

        ''' <summary>
        ''' Initializes a new instance of the <see cref="Form" /> class.
        ''' </summary>
        ''' <param name="formElement">The form element.</param>
        Public Sub New(ByVal formElement As HtmlElement)

            'Choose Form Method
            Select Case formElement.GetAttribute("method").ToLowerInvariant()
                Case HTMLCode.FormMethodGet
                    Me.Method = Form.FormMethod.GET

                Case HTMLCode.FormMethodPost
                    Me.Method = Form.FormMethod.POST

                Case Else
                    Debug.Assert(True, "What is this form type?")
                    Logger.Log.Instance.Write("We couldn't find form type", Log.LogCategory.Error)

            End Select

            Me.Action = formElement.GetAttribute("action")

            'AutoCompleteEnabled = Not GetAttribute(formElement, "autocomplete").ToLowerInvariant() = "off"
            AutoCompleteEnabled = Not (formElement.GetAttribute("autocomplete").ToLowerInvariant() = "off")

            For Each CurrentInput As HtmlElement In formElement.All

                If CurrentInput.TagName.ToLowerInvariant() = "input" Then

                    Dim Input As New WR.Input(CurrentInput)
                    If Not String.IsNullOrEmpty(Input.Name) Then
                        Me.AddInput(Input)
                    End If

                End If

            Next
        End Sub

        Private _action As String

        ''' <summary>
        ''' Gets or sets the action URL.
        ''' </summary>
        ''' <value>The action.</value>
        Public Property Action() As String
            Get
                Return _action
            End Get
            Set(ByVal value As String)
                _action = value
            End Set
        End Property


        Private _inputs As SortedList(Of String, Input)

        ''' <summary>
        ''' Gets or sets the inputs.
        ''' </summary>
        ''' <value>The inputs.</value>
        Public ReadOnly Property Inputs() As SortedList(Of String, Input)
            Get
                If _inputs Is Nothing Then _inputs = New SortedList(Of String, Input)
                Return _inputs
            End Get
        End Property


        ''' <summary>
        ''' Initializes a new instance of the <see cref="Form" /> class.
        ''' </summary>
        ''' <param name="method">The method.</param>
        ''' <param name="action">The action.</param>
        Public Sub New(ByVal method As FormMethod, ByVal action As String)
            Me.Method = method
            Me.Action = action
        End Sub


        ''' <summary>
        ''' Adds the input.
        ''' </summary>
        ''' <param name="input">The input.</param>
        Public Function AddInput(ByVal input As Input) As Boolean

            If Not Inputs.Keys.Contains(input.Name) Then
                Inputs.Add(input.Name, input)
                Return True

            Else
                Return False

            End If

        End Function

    End Class

End Namespace