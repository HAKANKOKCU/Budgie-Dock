Module SettingsHostModule
    Dim SettingsIni As Ini
    Public SettingsLoadID As String = ""
    Sub InitSettings()
        SettingsIni = New Ini(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\Settings" + SettingsLoadID + ".ini")
        CheckAndInsertInvalid()
    End Sub

    Sub SetSetting(ByVal Key As String, ByVal Value As Object, ByVal isInt As Boolean)
        If isInt Then
            Dim a As Integer = Value
        End If
        SettingsIni.SetValue("Main", Key, Value, True)
    End Sub

    Sub CheckAndInsertInvalid()
        Try
            My.Computer.FileSystem.WriteAllText(My.Computer.FileSystem.SpecialDirectories.Temp & "\BDockDefaultSettings.ini", My.Resources.DefaultSettings, False)
            Dim TempINI As New Ini(My.Computer.FileSystem.SpecialDirectories.Temp & "\BDockDefaultSettings.ini")
            Dim sec = TempINI.GetINIDictonary("Main".ToUpper)
            Dim scec = SettingsIni.GetINIDictonary("Main".ToUpper)
            For Each kval As String In sec.Keys
                If Not scec.ContainsKey(kval) Then
                    SettingsIni.SetValue("Main".ToUpper, kval, sec(kval), True)
                End If
            Next
            TempINI.DeleteSection("Main".ToUpper, True)
            sec.Clear()
            My.Computer.FileSystem.DeleteFile(My.Computer.FileSystem.SpecialDirectories.Temp & "\BDockDefaultSettings.ini")
        Catch ex As Exception
            'MsgBox(ex.Message + vbNewLine + "Deleting Settings File May Fix This." + vbNewLine + "Budgie Dock Can't Load Settings And Will Crash After This Error.", MsgBoxStyle.Critical)
            insertToLog(ex.ToString)
        End Try
    End Sub

    Function GetSetting(ByVal Key As String)
        Return SettingsIni.GetValue("Main", Key)
    End Function
End Module
