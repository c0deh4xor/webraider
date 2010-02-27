''' <summary>
''' This module include constants for HTML tags, attributes and other parser related stuff
''' </summary>
''' <remarks></remarks>

Public Module HTMLCode

    'Core
    Public Const TagStart As String = "<"
    Public Const TagEnd As String = ">"
    Public Const Equal As String = "="

    'Form Related
    Public Const FormTagName As String = "form"
    Public Const FormActionName As String = "action"
    Public Const FormMethodName As String = "method"
    Public Const FormEncytypeName As String = "enctype"

    'Input Related
    Public Const InputTagName As String = "input"
    Public Const InputTypeName As String = "type"

    'Select
    Public Const InputSelectTag As String = "select"

    'Input Types
    Public Const InputTypeHidden As String = "hidden"
    Public Const InputTypeText As String = "text"
    Public Const InputTypePassword As String = "password"
    Public Const InputTypeCheckbox As String = "checkbox"
    Public Const InputTypeRadio As String = "radio"
    Public Const InputTypeSubmit As String = "submit"
    Public Const InputTypeReset As String = "reset"
    Public Const InputTypeFile As String = "file"
    Public Const InputTypeImage As String = "image"
    Public Const InputTypeButton As String = "button"


    'Tag Standards
    Public Const TagValue As String = "value"
    Public Const TagName As String = "name"


    'Tag Start / End
    Public Const FormTagEnd As String = "</form>"

    'Form Methods
    Public Const FormMethodGet As String = "get"
    Public Const FormMethodPost As String = "post"

End Module