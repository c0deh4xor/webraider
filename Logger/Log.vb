''' <summary>
''' Debug / Trace / Logging
''' </summary>
''' <remarks></remarks>
Public Class Log

    Private Shared _instance As New Log
    Private Shared syncRoot As Object = New Object()

    Private _logPath As String
    ''' <summary>
    ''' Gets or sets the log path which logs will be stored.
    ''' </summary>
    ''' <value>The log path.</value>
    Public Property LogPath() As String
        Get
            If String.IsNullOrEmpty(_logPath) Then
                _logPath = My.Application.Info.DirectoryPath() & "log.txt"
            End If

            Return _logPath
        End Get
        Set(ByVal Value As String)
            _logPath = Value
        End Set
    End Property

    ''' <summary>
    ''' Gets the instance of Singleton.
    ''' </summary>
    ''' <value>The instance.</value>
    Public Shared ReadOnly Property Instance() As Log
        Get
            If _instance Is Nothing Then

                SyncLock syncRoot
                    If _instance Is Nothing Then
                        _instance = New Log()
                    End If
                End SyncLock

            End If

            Return _instance
        End Get
    End Property

    Private WriteLock As New Object()

    ''' <summary>
    ''' Semantic Log Categories
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum LogCategory As Integer

        ''' <summary>
        ''' Critical Errors
        ''' </summary>
        ''' <remarks></remarks>
        [Error] = 1

        ''' <summary>
        ''' Old bug or a potential error fixed
        ''' </summary>
        ''' <remarks></remarks>
        ErrorFix = 2

        ''' <summary>
        ''' Exception
        ''' </summary>
        ''' <remarks></remarks>
        ExceptionError = 3

        ''' <summary>
        ''' Logical Problems, Assert conditions etc.
        ''' </summary>
        ''' <remarks></remarks>
        LogicError = 4

        ''' <summary>
        ''' Plain Information
        ''' </summary>
        ''' <remarks></remarks>
        Information = 100

        ''' <summary>
        ''' New link added
        ''' </summary>
        ''' <remarks></remarks>
        LinkAdded = 200

        ''' <summary>
        ''' Request done
        ''' </summary>
        ''' <remarks></remarks>
        Request = 201

        ''' <summary>
        ''' Received Response
        ''' </summary>
        ''' <remarks></remarks>
        Response = 202


        ''' <summary>
        ''' Some event started
        ''' </summary>
        ''' <remarks></remarks>
        Start = 300

        ''' <summary>
        ''' Some event stopped
        ''' </summary>
        ''' <remarks></remarks>
        [Stop] = 301

        ''' <summary>
        ''' some event resumed
        ''' </summary>
        ''' <remarks></remarks>
        [Resume] = 702


        ''' <summary>
        ''' Unexpected Issues (weird need to remove potentially)
        ''' </summary>
        ''' <remarks></remarks>
        Unexpected = 900

        ''' <summary>
        ''' Single Attack
        ''' </summary>
        ''' <remarks></remarks>
        Attack = 1000

        ''' <summary>
        ''' Vulnerability Found
        ''' </summary>
        ''' <remarks></remarks>
        VulnerabilityFound = 50


        ''' <summary>
        ''' HTTP Request Failed
        ''' </summary>
        ''' <remarks></remarks>
        RequestFailed = 20

        ''' <summary>
        ''' Link not found
        ''' </summary>
        ''' <remarks></remarks>
        LinkNotFound = 99

        ''' <summary>
        ''' Full Request
        ''' </summary>
        ''' <remarks></remarks>
        FullRequest = 210
    End Enum


    ''' <summary>
    ''' Writes the specified message to the log.
    ''' </summary>
    ''' <param name="message">The message.</param>
    Public Sub Write(ByVal message As String)
        Write(message, LogCategory.Information)
    End Sub

    ''' <summary>
    ''' Current time as in LongTime Format.
    ''' </summary>
    ''' <returns>Time in LongTime format with one space padding at the end</returns>
    Public Function CurrentTime() As String
        'Add time information
        Return DateTime.Now.ToLongTimeString & " "
    End Function


    ''' <summary>
    ''' Write log for and user as well as system system log
    ''' </summary>
    ''' <param name="message"></param>
    ''' <param name="category"></param>
    ''' <remarks></remarks>
    Public Sub WriteEndUser(ByVal message As String, ByVal category As LogCategory)
        Write(message, category, True)
    End Sub

    ''' <summary>
    ''' Write log for system
    ''' </summary>
    ''' <param name="message"></param>
    ''' <param name="category"></param>
    ''' <remarks></remarks>
    Public Sub Write(ByVal message As String, ByVal category As LogCategory)
        Write(message, category, False)
    End Sub


    ''' <summary>
    ''' Writes the specified message to log.
    ''' </summary>
    ''' <param name="message">The message.</param>
    ''' <param name="category">The log category.</param>
    Public Sub Write(ByVal message As String, ByVal category As LogCategory, ByVal EndUser As Boolean)

        'Write log
        Try
            My.Application.Log.WriteEntry(message, TraceEventType.Verbose, category)
            message = CurrentTime() & message

        Catch ex As Exception
            Debug.WriteLine("Log write error!")

        End Try

    End Sub

    ''' <summary>
    ''' Writes exception to the log.
    ''' </summary>
    ''' <param name="ex">The Exception.</param>
    ''' <param name="additionalInfo">Additional info about log.</param>
    Public Sub WriteException(ByVal ex As Exception, ByVal additionalInfo As String)

        Try
            My.Application.Log.WriteException(ex, TraceEventType.Error, CurrentTime() & additionalInfo, LogCategory.ExceptionError)
            Dim ExcMessage As String = ex.ToString()
            If ex.InnerException IsNot Nothing Then ExcMessage &= ex.InnerException.ToString()

            Write(ExcMessage, LogCategory.ExceptionError, True)


        Catch LogEx As Exception
            Debug.WriteLine("Log write error")

        End Try

    End Sub


    ''' <summary>
    ''' Writes log message if condition is true.
    ''' </summary>
    ''' <param name="condition"><c>true</c> to cause message to be written, <c>false</c> otherwise.</param>
    ''' <param name="message">The log message.</param>
    Public Sub WriteIf(ByVal condition As Boolean, ByVal message As String)
        WriteIf(condition, message, LogCategory.Information)
    End Sub

    ''' <summary>
    ''' Writes log message if condition is true.
    ''' </summary>
    ''' <param name="condition"><c>true</c> to cause message to be written, <c>false</c> otherwise.</param>
    ''' <param name="message">The log message.</param>
    ''' <param name="category">The log category.</param>
    Public Sub WriteIf(ByVal condition As Boolean, ByVal message As String, ByVal category As LogCategory)
        If condition Then Write(message, category)
    End Sub


End Class
