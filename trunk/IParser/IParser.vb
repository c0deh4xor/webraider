''' <summary>
''' Parser Interface
''' </summary>
Public Interface IParser

    ''' <summary>
    ''' Gets the name of the Parser.
    ''' </summary>
    ''' <value>The name.</value>
    ReadOnly Property Name() As String

    ''' <summary>
    ''' Setups the specified Parser.
    ''' </summary>
    ''' <param name="html">The HTML Source Code to parse.</param>
    ''' <param name="uri">The Uri of the target.</param>
    Sub Setup(ByVal html As String, ByVal uri As Uri)

    ''' <summary>
    ''' Parse the input.
    ''' </summary>
    Sub Parse()


    ReadOnly Property Links() As List(Of String)
    ReadOnly Property Forms() As List(Of WR.Form)


End Interface
