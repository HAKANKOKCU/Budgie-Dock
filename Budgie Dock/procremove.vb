Public Class procremove
    Property process As Process
    Sub hide()
        Dim pat = "[Error While Getting Path: %pe%]"
        Try
            pat = process.MainModule.FileName
        Catch ex As Exception
            pat = pat.Replace("%pe%", ex.Message)
        End Try
        If MsgBox("Blacklist Process From Dock?" & vbNewLine & "Path: " & pat, MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Blacklist Process") = MsgBoxResult.Yes Then
            Dim ff As String = My.Computer.FileSystem.ReadAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\BlacklistProceses.data")
            If Not ff = "" Then
                My.Computer.FileSystem.WriteAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\BlacklistProceses.data", ff + "|" + process.ProcessName.ToLower, False)
            Else
                My.Computer.FileSystem.WriteAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\BlacklistProceses.data", process.ProcessName.ToLower, False)
            End If
        End If
    End Sub
End Class
