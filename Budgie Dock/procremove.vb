Public Class procremove
    Property process As Process
    Property hw As IntPtr
    Sub hide()
        Dim pat = "[Error While Getting Path: %pe%]"
        Try
            pat = process.MainModule.FileName
        Catch ex As Exception
            pat = pat.Replace("%pe%", ex.Message)
        End Try
        If MsgBox("Blacklist Process From Dock?" & vbNewLine & "Name: " & process.ProcessName & vbNewLine & "Path: " & pat & vbNewLine & "Window IntPtr: " & hw.ToString, MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Blacklist Process") = MsgBoxResult.Yes Then
            Dim ff As String = My.Computer.FileSystem.ReadAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\BlacklistProceses.data")
            If Not ff = "" Then
                My.Computer.FileSystem.WriteAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\BlacklistProceses.data", ff + "|" + process.ProcessName, False)
            Else
                My.Computer.FileSystem.WriteAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\BlacklistProceses.data", process.ProcessName, False)
            End If
        End If
    End Sub
End Class
