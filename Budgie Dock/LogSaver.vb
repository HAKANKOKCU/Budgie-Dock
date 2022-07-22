Module LogSaver
    Sub insertToLog(ByVal text As String)
        My.Computer.FileSystem.WriteAllText(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\log.log", vbNewLine & Now.Day & "/" & Now.Month & "/" & Now.Year & " (D/M/Y), " & Now.Second & ":" & Now.Minute & ":" & Now.Hour & " = " & text, True)
    End Sub
End Module
