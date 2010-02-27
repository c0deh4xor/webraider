Imports System.ComponentModel.Composition
Imports WebRaider.Plugins.Parser
Imports System.Text.RegularExpressions
Imports Logger


''' <summary>
''' HTML Parser responsible with all kind of parsing
''' </summary>
<Export(GetType(IParser))> _
Public Class Parser
    Implements IParser

    ''' <summary>
    ''' Gets the name of the Parser.
    ''' </summary>
    ''' <value>The name.</value>
    Public ReadOnly Property Name() As String Implements IParser.Name
        Get
            Return "Simple HTML Parser"
        End Get
    End Property


    Const RegExAttributeName As String = "{{name}}"

    ''' <summary>
    ''' Link Type
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum LinkType
        InternalLink
        ExternalLink
        [Error]
    End Enum


    Private _html As String

    ''' <summary>
    ''' Gets or sets the HTML.
    ''' </summary>
    ''' <value>The HTML.</value>
    Public ReadOnly Property Html() As String
        Get
            Return _html
        End Get
    End Property


    Private _links As List(Of String)

    ''' <summary>
    ''' Gets or sets the links.
    ''' </summary>
    ''' <value>The links.</value>
    Public ReadOnly Property Links() As List(Of String) Implements IParser.Links
        Get
            Return _links
        End Get
    End Property

    Private _forms As List(Of WR.Form)

    ''' <summary>
    ''' Gets or sets the Forms.
    ''' </summary>
    ''' <value>The links.</value>
    Public ReadOnly Property Forms() As List(Of WR.Form) Implements IParser.Forms
        Get
            Return _forms
        End Get
    End Property



    ''' <summary>
    ''' Extract links from source code.
    ''' </summary>
    Private Sub ParseLinks()

        For Each Match As Match In Regex.Matches(Html, LinkRegEx, RegexOptions.Compiled Or RegexOptions.CultureInvariant Or RegexOptions.Multiline Or RegexOptions.IgnoreCase)
            AddLink(Match.Groups("url").Captures(0).Value)
        Next Match

    End Sub

    ''' <summary>
    ''' Gets the form start position.
    ''' </summary>
    ''' <param name="StartPosition">The start position.</param>
    ''' <returns></returns>
    Private Function GetFormStartPosition(ByVal StartPosition As Integer) As Integer

        Dim FormStartPosition As Integer = Html.IndexOf(HTMLCode.TagStart & HTMLCode.FormTagName, StartPosition, Html.Length - StartPosition, StringComparison.OrdinalIgnoreCase)

        Return FormStartPosition
    End Function


    ''' <summary>
    ''' Parse HTML and return New form with action and method.
    ''' </summary>
    ''' <param name="formStartPosition">The form start position.</param>
    ''' <param name="formTagEndPosition">The form tag end position.</param>
    ''' <returns>New form if found else <c>Nothing</c></returns>
    Private Function GetNewForm(ByVal formStartPosition As Integer, ByRef formTagEndPosition As Integer) As WR.Form

        'Parse form tag
        Dim formTagHtml As String = ExtractText(Html, formStartPosition, HTMLCode.TagEnd, formTagEndPosition, True)

        'Not found
        If formTagHtml Is Nothing Then
            Log.Instance.Write("There is no form close tag!", Log.LogCategory.Error)
            formTagEndPosition = -1
            Return Nothing
        End If

        'Generate New Form
        Dim form As New WR.Form()

        'Parse attributes
        Dim formMethod, formAction, EncType As String
        formMethod = ParseAttribute(formTagHtml, HTMLCode.FormMethodName)

        EncType = ParseAttribute(formTagHtml, HTMLCode.FormEncytypeName)

        If formMethod Is Nothing Then
            form.Method = WR.Form.FormMethod.GET

        Else
            If EncType.Equals("multipart/form-data", StringComparison.CurrentCultureIgnoreCase) Then
                form.Method = WR.Form.FormMethod.MultiPart

            Else

                'Choose Form Method
                Select Case formMethod.ToLowerInvariant
                    Case HTMLCode.FormMethodGet
                        form.Method = WR.Form.FormMethod.GET

                    Case HTMLCode.FormMethodPost
                        form.Method = WR.Form.FormMethod.POST

                    Case Else
                        Debug.Assert(True, "What is this form type?")
                        Log.Instance.Write("We couldn't find form type", Log.LogCategory.Error)

                End Select
            End If

        End If

        'Form Action
        formAction = ParseAttribute(formTagHtml, HTMLCode.FormActionName)

        'If not found, assume this page
        If formAction Is Nothing Then formAction = ""

        form.Action = formAction

        Return form
    End Function


    ''' <summary>
    ''' Parses the forms and extract all input information and form information.
    ''' </summary>
    ''' <param name="StartPosition">The start position.</param>
    ''' <remarks>Very complex for a recursive function. Need to fix it and not do recursive optionally.</remarks>
    Private Sub ParseForms(Optional ByVal StartPosition As Integer = 0)
        'Quit if finished or End of the HTML page
        If StartPosition < 0 OrElse Html.Length <= StartPosition Then
            Log.Instance.Write("Form search finished", Log.LogCategory.Stop)
            Exit Sub
        End If

        Dim FormStartPosition As Integer = GetFormStartPosition(StartPosition)

        'If there is no form exit
        If FormStartPosition = -1 Then Exit Sub

        'Write Log
        Log.Instance.WriteIf(StartPosition = 0, "Form search start", Log.LogCategory.Start)
        Log.Instance.WriteIf(StartPosition > 0, "Form search resume", Log.LogCategory.Resume)

        'Store end positions with byrefs
        Dim FormTagEndPosition As Integer

        'Get New form (with action and method)
        Dim form As WR.Form = GetNewForm(FormStartPosition, FormTagEndPosition)

        'Form not found
        If form.GetType() Is Nothing Then
            Exit Sub
        End If

        'Extract data between form start and form end
        Dim formHtmlEndPosition As Integer
        Dim formHtml As String = ExtractText(Html, FormTagEndPosition, HTMLCode.FormTagEnd, formHtmlEndPosition, True)

        Log.Instance.WriteIf(formHtml Is Nothing, "We couldn't get form content", Log.LogCategory.Error)

        'Extract Inputs
        Dim inputAdded As Boolean
        Dim inputMatches As MatchCollection = Regex.Matches(formHtml, InputRegEx, RegexOptions.IgnoreCase Or RegexOptions.CultureInvariant)

        'Add Inputs
        For Each match As Match In inputMatches
            If AddInputToForm(form, match.Value) Then inputAdded = True
        Next match

        'Add Textareas
        If AddTextarea(form, formHtml) Then inputAdded = True
        If AddSelect(form, formHtml) Then inputAdded = True

        'TODO : If there is no form still we need to add link to the crawler!
        'Add generated form to form collection
        If inputAdded Then AddForm(form)

        'Go to start with new form position
        ParseForms(formHtmlEndPosition + 1)

    End Sub


    ''' <summary>
    ''' Adds the textarea.
    ''' </summary>
    ''' <param name="form">The form.</param>
    ''' <param name="html">The HTML of textarea.</param>
    ''' <returns><c>true</c> if Textarea added else otherwise.</returns>
    Private Shared Function AddTextarea(ByVal form As WR.Form, ByVal html As String) As Boolean

        If html Is Nothing Then Return False

        Dim Added As Boolean

        For Each Match As Match In Regex.Matches(html, TextareaRegEx, RegexOptions.IgnoreCase Or RegexOptions.CultureInvariant)

            Dim input As New WR.Input()
            input.Name = ParseAttribute(Match.Groups("tag").Captures(0).Value, HTMLCode.TagName)

            'Dont accept empty string as valid input
            'Endswith("=") is a temp solution for bypassing empty attribute names
            If input.Name Is Nothing OrElse input.Name = String.Empty OrElse (input.Name.EndsWith("=")) Then
                Log.Instance.Write("Input Name is empty, skipped!", Log.LogCategory.Information)
                Continue For
            End If

            input.Value = Match.Groups("value").Captures(0).Value
            Debug.Assert(Not input.Value Is Nothing, "Textarea value is nothing !")

            input.Type = WR.Input.InputType.Textarea

            form.AddInput(input)
            Added = True

        Next Match

        Return Added
    End Function


    ''' <summary>
    ''' Add Select Tags
    ''' </summary>
    ''' <param name="form">Form to add</param>
    ''' <param name="html">Source Code to extract select tags</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function AddSelect(ByVal form As WR.Form, ByVal html As String) As Boolean
        If html Is Nothing Then Return False

        Dim Added As Boolean

        For Each Match As Match In Regex.Matches(html, SelectRegEx, RegexOptions.IgnoreCase Or RegexOptions.CultureInvariant Or RegexOptions.Singleline)

            Dim input As New WR.Input()
            input.Name = ParseAttribute(Match.Captures(0).Value, HTMLCode.TagName)

            'Dont accept empty string as valid input
            If input.Name Is Nothing OrElse input.Name = String.Empty Then
                Log.Instance.Write("Select Name is empty, skipped!", Log.LogCategory.Information)
                Continue For
            End If

            'Extract Options for input
            For Each OptionMatch As Match In Regex.Matches(Match.Captures(0).Value, SelectOptionRegEx, RegexOptions.IgnoreCase Or RegexOptions.CultureInvariant Or RegexOptions.Singleline)

                input.AddValue(ParseAttribute(OptionMatch.Captures(0).Value, HTMLCode.TagValue))

            Next

            input.Type = WR.Input.InputType.Select

            form.AddInput(input)
            Added = True

        Next Match


        Return Added
    End Function


    ''' <summary>
    ''' Adds the input to form.
    ''' </summary>
    ''' <param name="form">The form.</param>
    ''' <param name="inputHtml">The input HTML.</param>
    ''' <returns></returns>
    Private Function AddInputToForm(ByRef form As WR.Form, ByVal inputHtml As String) As Boolean

        'Check input, Return False if not valid
        If inputHtml Is Nothing OrElse inputHtml = String.Empty Then Return False

        'Parse Input Type
        Dim inputType As String = ParseAttribute(inputHtml, HTMLCode.InputTypeName)
        If inputType Is Nothing OrElse inputType = String.Empty Then Return False

        'New Input Object
        Dim input As New WR.Input()

        'TODO2 : Handle Textarea and Select
        'Check against valid types
        Select Case inputType.ToLowerInvariant

            Case InputTypeHidden
                input.Type = WR.Input.InputType.Hidden

                'TODO2: Possible upload screen
            Case InputTypeFile
                input.Type = WR.Input.InputType.File

            Case InputTypeButton
                input.Type = WR.Input.InputType.Button

            Case InputTypeImage
                input.Type = WR.Input.InputType.Image

            Case InputTypeSubmit
                input.Type = WR.Input.InputType.Submit

            Case InputTypeText
                input.Type = WR.Input.InputType.Text

            Case InputTypeCheckbox
                input.Type = WR.Input.InputType.Checkbox

            Case InputTypePassword
                input.Type = WR.Input.InputType.Password

            Case Else 'Exit if not valid type or error
                'TODO: Enable and Fix
                'Debug.Assert(False, "Unknown input type - " & inputType.ToString)

        End Select

        'Parse Name
        input.Name = ParseAttribute(inputHtml, HTMLCode.TagName)

        'If we don't have name it's useless
        If input.Name Is Nothing OrElse input.Name = String.Empty Then
            Log.Instance.Write("Input name is empty, skipped", Log.LogCategory.Information)
            Return False
        End If

        'Parse Value
        input.Value = ParseAttribute(inputHtml, HTMLCode.TagValue)
        If input.Value Is Nothing Then input.Value = String.Empty

        'Add it to form
        Return form.AddInput(input)

    End Function

    '''' <summary>
    '''' RegEx for parsing HTML attributes
    '''' </summary>
    '''' <remarks>
    '''' Works on single quotes, double quotes, no quotes.
    '''' Data should supplied limited, designed to get only one attribute at a time. Attribute name shouldn't include any special character which requires escape from RegEx
    '''' </remarks>
    '    Private Const AttributeRegEx As String = ".*{{name}}\s*=\s*(?:[\s]*).(?<value>.*?).(?:[\s>])"

    'Sucked at 
    'Private Const AttributeRegEx As String = ".*{{name}}\s*=\s*(?:[\s""""']*)(?<value>.*?)(?:[\s>""""'])"


    'Old Regex bugged at property values
    Private Const AttributeRegEx As String = ".*{{name}}\s*=\s*(?:[\s""']*)(?<value>.*?)(?:[\s>""'])"


    ''' <summary>
    ''' Extract all inputs from input
    ''' </summary>
    ''' <remarks>
    ''' Designed to work only in a form tag but should also work massively in an HTML file
    ''' </remarks>
    Private Const InputRegEx As String = "<\s*input\s*[^>]*>"


    ''' <summary>
    ''' Extract select inputs
    ''' </summary>
    ''' <remarks>
    ''' Designed to work only in a form tag but should also work massively in an HTML file
    ''' </remarks>
    Private Const SelectRegEx As String = "<\s*select\s*[^>]*>.*</select>"

    ''' <summary>
    ''' Extract options from select
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SelectOptionRegEx As String = "<\s*option\s*[^>]*>"


    ''' <summary>
    ''' Textarea RegEx
    ''' </summary>
    ''' <remarks>
    ''' Find 2 groups, First is tag which is including attributes. Second one is value which includes textarea value
    ''' </remarks>
    Private Const TextareaRegEx As String = "(?<tag><\s*textarea\s*[^>]*>)(?<value>[^</]*)"

    ''' <summary>
    ''' Gets the attributes.
    ''' </summary>
    ''' <param name="htmlCode">The HTML code.</param>
    ''' <param name="name">The name to extract.</param>
    ''' <returns>NULL if not found</returns>
    Private Shared Function ParseAttribute(ByVal htmlCode As String, ByVal name As String) As String

        Dim matchedAtt As Match = Regex.Match(htmlCode, AttributeRegEx.Replace(RegExAttributeName, name), RegexOptions.IgnoreCase Or RegexOptions.CultureInvariant)

        'Always return first
        If matchedAtt Is Nothing Then Return Nothing

        Return matchedAtt.Groups("value").Value()
    End Function


    ''' <summary>
    ''' Extracts the text.
    ''' </summary>
    ''' <param name="code">The code.</param>
    ''' <param name="startPosition">The start position.</param>
    ''' <param name="endText">The end text.</param>
    ''' <param name="endPosition">The end position.</param>
    ''' <param name="tryParse">if set to <c>true</c> tries to find endText and if not found returns all string from start.</param>
    ''' <returns></returns>
    Public Shared Function ExtractText(ByVal code As String, ByVal startPosition As Integer, ByVal endText As String, ByRef endPosition As Integer, Optional ByVal tryParse As Boolean = False) As String

        'Start is wrong
        If startPosition = -1 Then Return Nothing

        endPosition = code.IndexOf(endText, startPosition, StringComparison.OrdinalIgnoreCase)

        'There is no end position
        If endPosition = -1 Then

            'If try parse enabled return all text
            If tryParse Then
                endPosition = code.Length

            Else ' Just return with Nothing
                Return Nothing

            End If

        End If


        'Return found string
        Return code.Substring(startPosition + 1, endPosition - startPosition - 1).Trim

    End Function


    ''' <summary>
    ''' Parses the specified HTML.
    ''' </summary>
    Public Sub Parse() Implements IParser.Parse
        'Parse Links
        ParseLinks()

        'Parse Forms
        ParseForms()

    End Sub

    ''' <summary>
    ''' Adds the link.
    ''' </summary>
    ''' <param name="link">The link.</param>
    Private Sub AddLink(ByVal link As String)
        If _links Is Nothing Then _links = New List(Of String)
        _links.Add(link)
    End Sub

    ''' <summary>
    ''' Adds new form to collection.
    ''' </summary>
    ''' <param name="form">The form.</param>
    Private Sub AddForm(ByVal form As WR.Form)
        If _forms Is Nothing Then _forms = New List(Of WR.Form)
        _forms.Add(form)
    End Sub

    ''' <summary>
    ''' Link RegEx
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LinkRegEx As String = "(?:(?<linktype>href|src)\s*=)(?:[\s""""']*)(?<url>.*?)(?:[\s>""""'])"

    'Private Const LinkRegEx As String = "(?:(?<linktype>href|src)\s*=)(?:[\s""""']*)(?!#|mailto|location.|javascript|.*css|.*this\.)(?<url>.*?)(?:[\s>""""'])"


    ''' <summary>
    ''' Initializes a new instance of the <see cref="Parser" /> class.
    ''' </summary>
    ''' <param name="html">HTML source code to parse.</param>
    Public Sub New()
    End Sub

    Public Sub Setup(ByVal html As String, ByVal uri As Uri) Implements IParser.Setup
        Me._html = html
    End Sub

End Class
