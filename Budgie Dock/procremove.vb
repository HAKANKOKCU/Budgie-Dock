Public Class procremove
    Property processName As String
    Sub hide()
        If MsgBox("Blacklist Process From Dock?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Blacklist Process") = MsgBoxResult.Yes Then
            Dim ff As String = My.Computer.FileSystem.ReadAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\BlacklistProceses.data")
            If Not ff = "" Then
                My.Computer.FileSystem.WriteAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\BlacklistProceses.data", ff + "|" + processName, False)
            Else
                My.Computer.FileSystem.WriteAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\BlacklistProceses.data", processName, False)
            End If
        End If
    End Sub
End Class
