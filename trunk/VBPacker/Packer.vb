''' <summary>
''' Simple VbScript packer by Ferruh Mavituna, this is a really quick and dirty packer. Spesifically designed for One Click Ownage vbscript.
''' </summary>
''' <remarks></remarks>
Module Packer
    ''' <summary>
    ''' Simple VbScript packer
    ''' </summary>
    Sub Main()
        If My.Application.CommandLineArgs.Count < 2 Then
            Console.WriteLine("Usage")
            Console.WriteLine(My.Application.Info.AssemblyName & " input.vbs output.vbs")
            Exit Sub
        End If

        Dim File As String = My.Computer.FileSystem.ReadAllText(My.Application.CommandLineArgs(0))

        Dim OriginalSize As Integer = File.Length

        File = File.Replace(vbTab, " ")

        While File.IndexOf("  ") > -1
            File = File.Replace("  ", " ")
        End While

        While File.IndexOf(vbNewLine & vbNewLine) > -1
            File = File.Replace(vbNewLine & vbNewLine, vbNewLine)
        End While

        File = File.Replace(vbNewLine, ":")
        File = File.Replace(": ", ":")
        File = File.Replace(" :", ":")

        While File.IndexOf("::") > -1
            File = File.Replace("::", ":")
        End While

        File = File.TrimEnd(":"c)

        My.Computer.FileSystem.WriteAllText(My.Application.CommandLineArgs(1), File, False, System.Text.Encoding.ASCII)

        Console.WriteLine("Packed " & OriginalSize & " to " & File.Length & ".")
    End Sub

End Module
