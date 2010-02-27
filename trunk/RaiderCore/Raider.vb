Public Class Raider

    Public Enum Type
        ''' <summary>
        ''' Just modifies the parameter
        ''' </summary>
        ''' <remarks></remarks>
        ActiveBlind

        ''' <summary>
        ''' Passively Analyses the Responses
        ''' </summary>
        ''' <remarks></remarks>
        Passive

        ''' <summary>
        ''' Sends HTTP Requests, Analyses Responses etc.
        ''' </summary>
        ''' <remarks></remarks>
        Custom

    End Enum

End Class
