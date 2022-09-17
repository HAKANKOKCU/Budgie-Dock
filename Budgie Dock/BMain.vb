Imports System.Runtime.InteropServices
Imports System.ComponentModel

Module BMain
    Public Declare Auto Function IsIconic Lib "user32.dll" (ByVal hwnd As IntPtr) As Boolean
    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Public Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As UInteger, ByVal lParam As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll", SetLastError:=True)> _
    Public Function GetWindowThreadProcessId(ByVal hWnd As IntPtr, _
    ByRef lpdwProcessId As Integer) As Integer
    End Function

    Public arWindows As ArrayList
    Public arWinnameFromDict As Dictionary(Of IntPtr, String)

    Delegate Function EnumWindowDelegate(ByVal hWnd As IntPtr, ByVal Lparam As IntPtr) As Boolean
    Private Callback As EnumWindowDelegate = New EnumWindowDelegate(AddressOf EnumWindowProc)
    Private Function EnumWindowProc(ByVal hWnd As IntPtr, ByVal Lparam As IntPtr) As Boolean
        If IsWindowVisible(hWnd) Then
            Dim TheLength As Integer = GetWindowTextLengthA(hWnd)
            Dim TheReturn(TheLength * 2) As Byte '2x the size of the Max length
            GetWindowText(hWnd, TheReturn, TheLength + 1)
            Dim TheText As String = ""
            For x = 0 To (TheLength - 1) * 2
                If TheReturn(x) <> 0 Then
                    TheText &= Chr(TheReturn(x))
                End If
            Next
            Dim id
            GetWindowThreadProcessId(hWnd, id)
            If TheText <> "" Then
                arWindows.Add({TheText, id, hWnd})
                arWinnameFromDict.Add(hWnd, TheText)
            End If
        End If
        Return True
    End Function

    Sub refreshOpenedWindows()
        arWindows = New ArrayList
        arWinnameFromDict = New Dictionary(Of IntPtr, String)
        Dim Result As Boolean = EnumWindows(Callback, IntPtr.Zero)
    End Sub

    Private Declare Function EnumWindows Lib "User32.dll" (ByVal WNDENUMPROC As EnumWindowDelegate, ByVal lparam As IntPtr) As Boolean
    Private Declare Auto Function GetWindowText Lib "User32.dll" (ByVal Hwnd As IntPtr, ByVal Txt As Byte(), ByVal Lng As Integer) As Integer
    Private Declare Function IsWindowVisible Lib "User32.dll" (ByVal hwnd As IntPtr) As Boolean
    Private Declare Function GetWindowTextLengthA Lib "User32.dll" (ByVal hwnd As IntPtr) As Integer
    Public Const WM_SYSCOMMAND As UInt32 = &H112
    Public Const SC_RESTORE As UInt32 = &HF120
    Public Const SC_MAXIMIZE As UInt32 = &HF030
    Public Const SC_MINIMIZE As UInt32 = &HF020
    Public Const VersionBD As Integer = 0
    Public WithEvents UpdateNotifyWorker As New BackgroundWorker
    Public Sub ActivateApp(ByVal handle As IntPtr)
        'Minimize Window
        SendMessage(handle,
          WM_SYSCOMMAND, SC_MINIMIZE, CType(0, IntPtr))

        'Restore Window
        SendMessage(handle,
          WM_SYSCOMMAND, SC_RESTORE, CType(0, IntPtr))
    End Sub
    Public Sub showSpecialDiag(ByVal str As String)
        If str = "!AppsDrawer" Then
            Dim ad As New appdrawer
            'containerwin.Visibility = Visibility.Hidden
            ad.ShowDialog()
            'containerwin.Visibility = Visibility.Visible
        ElseIf str = "!Shutdown" Then
            Dim ad As New shutdownDialog
            ad.ShowDialog()
        ElseIf str.StartsWith("!WebSearch") Then
            Dim ws As New WebSearch
            ws.se = str.Split(":")(1)
            ws.ShowDialog()
        End If
    End Sub

    Private Sub UpdateNotifyWorker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles UpdateNotifyWorker.DoWork
        'Try
        If My.Computer.FileSystem.FileExists(My.Computer.FileSystem.SpecialDirectories.Temp & "\BDockUpdate.txt") Then
            My.Computer.FileSystem.DeleteFile(My.Computer.FileSystem.SpecialDirectories.Temp & "\BDockUpdate.txt")
        End If
        My.Computer.Network.DownloadFile("https://docs.google.com/uc?export=download&id=1zJbCkbfEGEb_nDELq1ZMBEsjdj8Ps8CN", My.Computer.FileSystem.SpecialDirectories.Temp & "\BDockUpdate.txt")
        Dim ui As Integer = My.Computer.FileSystem.ReadAllText(My.Computer.FileSystem.SpecialDirectories.Temp & "\BDockUpdate.txt")
        If ui > VersionBD Then
            If MsgBox("An update found. Do you want to download it?", MsgBoxStyle.YesNo, "Budgie Dock Update") = MsgBoxResult.Yes Then
                Process.Start("https://github.com/HAKANKOKCU/Budgie-Dock/raw/main/Budgie%20Dock%20Setup.exe")
            End If
        End If
        'Catch
        'End Try
    End Sub
End Module
