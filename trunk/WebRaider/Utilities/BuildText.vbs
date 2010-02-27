'FM - 14/02/09
'Generate a zipped hex representation of a binary file

Set objArgs = WScript.Arguments

If objArgs.Count < 2 Then
	
	Wscript.stdout.write "Usage:" & vbNewline
	Wscript.stdout.write "-------------------------------" & vbNewline
	Wscript.stdout.write "BuildText.vbs inputfile outputfile" & vbNewline
	Wscript.stdout.write "input : binary file" & vbNewline
	Wscript.stdout.write "output : text file (will be overwritten)" & vbNewline
	
	Wscript.Quit 1
End If

input = objArgs(0)
output = objArgs(1)

BinaryText = ReadBinaryFile(input)
WriteFile output, "d=""" & Optimise(ByteArray2Text(BinaryText)) & """"

'Simply zip the 0s
Function Optimise(binary)
  For i=0 To Len(binary) Step 2
    
    Current = Mid(binary,i+1,2)
    Out = Current 
    
    If Current = "00" Then

      NextBit = "00"
      repeat = 0
      
      While NextBit = "00" AND repeat < 255
        
        repeat = repeat + 1
        NextBit = Mid(binary,i+1+(repeat*2),2)
      Wend
      
      If repeat > 1 Then 
        Out = "x" & Right("0" & Hex(repeat),2)
                
        'Fix the normal loop position
        i = i+(repeat*2)-2   
      End If 
                     
    End If       
    
    OutTxt = OutTxt & Out  
      
  Next
   
  Optimise = OutTxt

End Function

Function ByteArray2Text(varByteArray)
    strData = ""
    strBuffer = ""
    HexS = ""
        
    For lngCounter = 0 to UBound(varByteArray)        
        HexS = Hex(Ascb(Midb(varByteArray,lngCounter + 1, 1)))
        If Len(HexS) < 2 Then HexS = "0" & HexS
        
        strBuffer = strBuffer & HexS
                
        'Keep strBuffer at 1k bytes maximum // REMOVABLE?
        If lngCounter Mod 1000 = 0 Then
            strData = strData & strBuffer
            strBuffer = ""
        End If
    Next
    
    ByteArray2Text = strData & strBuffer
End Function


Function ReadBinaryFile(FileName)  
  Set BinaryStream = CreateObject("ADODB.Stream")    
  BinaryStream.Type = 1 'BinaryType
  BinaryStream.Open
  BinaryStream.LoadFromFile FileName
  ReadBinaryFile = BinaryStream.Read  
End Function

Sub WriteFile(file, text)
  Set myFSO = CreateObject("Scripting.FileSystemObject")
  Set WriteStuff = myFSO.OpenTextFile(file, 2, True)
  WriteStuff.Writeline(text)
  WriteStuff.Close
  SET WriteStuff = NOTHING
  SET myFSO = NOTHING
End Sub