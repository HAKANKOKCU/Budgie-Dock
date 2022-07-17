Module SettingsHostModule
    Dim SettingsIni As Ini
    Sub InitSettings()
        SettingsIni = New Ini(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\Settings.ini")
    End Sub

    Sub SetSetting(ByVal Key As String, ByVal Value As Object, ByVal isInt As Boolean)
        If isInt Then
            Dim a As Integer = Value
        End If
        SettingsIni.SetValue("Main", Key, Value, True)
    End Sub

    Function GetSetting(ByVal Key As String)
        Return SettingsIni.GetValue("Main", Key)
    End Function
End Module
