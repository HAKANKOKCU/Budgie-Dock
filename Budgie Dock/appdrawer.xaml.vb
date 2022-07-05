﻿Public Class appdrawer
    Dim icopack As Ini = Nothing
    Dim handleclick As Boolean = True
    Private Sub appdrawer_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Me.KeyDown
        If e.Key = Key.Escape Then
            aaps.Children.Clear()
            Close()
        End If
    End Sub

    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        SearchTB.Focus()
        If My.Computer.FileSystem.FileExists(My.Settings.CurrentIconThemePath) Then
            icopack = New Ini(My.Settings.CurrentIconThemePath)
        Else
            icopack = Nothing
        End If
        findApps("")
    End Sub

    Sub findApps(ByVal Str As String)
        aaps.Children.Clear()
        For Each a As String In My.Computer.FileSystem.GetFiles(My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData & "\..\..\..\Microsoft\Windows\Start Menu\Programs", FileIO.SearchOption.SearchAllSubDirectories)
            Dim nm = My.Computer.FileSystem.GetName(a)
            Dim adda = False
            If nm.ToLower.Contains(SearchTB.Text) Or nm.ToUpper.Contains(SearchTB.Text) Or nm.Contains(SearchTB.Text) Then
                adda = True
            End If
            Dim fi As New IO.FileInfo(a)
            If fi.Extension = ".ini" Or fi.Extension = "ini" Then
                adda = False
            End If
            If adda Then
                Dim bdr As New Border
                Dim st As New StackPanel
                Dim ico As New Image
                Dim anm As New Label
                anm.Content = nm.Replace(fi.Extension, "")
                anm.Foreground = Brushes.White
                anm.HorizontalAlignment = Windows.HorizontalAlignment.Center
                Dim findicon = True
                If Not icopack Is Nothing Then
                    If Not icopack.GetValue("IconPaths", nm) = "Code_Item.NotFound" Then
                        findicon = False
                        Dim bm As New BitmapImage
                        bm.BeginInit()
                        bm.UriSource = New Uri(icopack.GetValue("IconPaths", nm).Replace("{Budgie.BDock.ConfigDirectory}", My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\").Replace("{iniDir}", My.Settings.CurrentIconThemePath.Replace(My.Computer.FileSystem.GetName(My.Settings.CurrentIconThemePath), "")))
                        bm.EndInit()
                        ico.Source = bm
                    End If
                End If
                If findicon Then
                    If Not My.Computer.FileSystem.FileExists(My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\Icons\" + a.Replace(":", "+").Replace("\", "+") + ".png") Then
                        Try
                            Dim bm As New BitmapImage
                            Dim ai As System.Drawing.Icon = System.Drawing.Icon.ExtractAssociatedIcon(a)
                            ai.ToBitmap().Save(My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\Icons\" + a.Replace(":", "+").Replace("\", "+") + ".png", System.Drawing.Imaging.ImageFormat.Png)
                            bm.BeginInit()
                            bm.UriSource = New Uri(My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\Icons\" + a.Replace(":", "+").Replace("\", "+") + ".png")
                            bm.EndInit()
                            ico.Source = bm
                        Catch
                            If My.Computer.FileSystem.FileExists(My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\Icons\" + a.Replace(":", "+").Replace("\", "+") + ".png") Then
                                Dim bm As New BitmapImage
                                bm.BeginInit()
                                bm.UriSource = New Uri(My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\Icons\" + a.Replace(":", "+").Replace("\", "+") + ".png")
                                bm.EndInit()
                                ico.Source = bm
                            End If
                        End Try
                    Else
                        Dim bm As New BitmapImage
                        bm.BeginInit()
                        bm.UriSource = New Uri(My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\Icons\" + a.Replace(":", "+").Replace("\", "+") + ".png")
                        bm.EndInit()
                        ico.Source = bm
                    End If
                End If
                ico.Margin = New Thickness(5)
                ico.Width = 40
                ico.Height = 40
                st.Children.Add(ico)
                st.Children.Add(anm)
                bdr.Child = st
                bdr.Padding = New Thickness(5)
                bdr.CornerRadius = New CornerRadius(10)
                bdr.Focusable = True
                Dim hvcolor As New SolidColorBrush(Color.FromArgb(50, 255, 255, 255))
                Dim appwithoutloop = a
                AddHandler bdr.MouseEnter, Sub(sender, e)
                                               bdr.Background = hvcolor
                                           End Sub
                AddHandler bdr.MouseLeave, Sub(sender, e)
                                               bdr.Background = Brushes.Transparent
                                           End Sub
                Dim ismdown As Boolean = False
                AddHandler bdr.MouseDown, Sub(sender, e)
                                              bdr.Background = Brushes.Black
                                              ismdown = True
                                          End Sub
                AddHandler bdr.MouseUp, Sub(sender, e)
                                            bdr.Background = Brushes.Transparent
                                            If ismdown And handleclick Then
                                                aaps.Children.Clear()
                                                Try
                                                    Process.Start(appwithoutloop)
                                                Catch ex As Exception
                                                    MsgBox(ex.Message, MsgBoxStyle.Critical)
                                                End Try
                                                Close()
                                                ismdown = False
                                            End If
                                        End Sub
                AddHandler bdr.GotFocus, Sub(sender, e)
                                             bdr.Background = Brushes.Black
                                         End Sub
                AddHandler bdr.LostFocus, Sub(sender, e)
                                              bdr.Background = Brushes.Transparent
                                          End Sub
                AddHandler bdr.KeyDown, Sub(sender, e)
                                            If e.Key = Key.Enter Then
                                                bdr.Background = Brushes.Transparent
                                                aaps.Children.Clear()
                                                Process.Start(appwithoutloop)
                                                Close()
                                            End If
                                        End Sub
                aaps.Children.Add(bdr)
            End If
        Next
        For Each a As String In My.Computer.FileSystem.GetFiles(My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData & "\..\..\..\Microsoft\Windows\Start Menu\Programs", FileIO.SearchOption.SearchAllSubDirectories)
            Dim nm = My.Computer.FileSystem.GetName(a)
            Dim adda = False
            If nm.ToLower.Contains(SearchTB.Text) Or nm.ToUpper.Contains(SearchTB.Text) Or nm.Contains(SearchTB.Text) Then
                adda = True
            End If
            Dim fi As New IO.FileInfo(a)
            If fi.Extension = ".ini" Or fi.Extension = "ini" Then
                adda = False
            End If
            If adda Then
                Dim bdr As New Border
                Dim st As New StackPanel
                Dim ico As New Image
                Dim anm As New Label
                anm.Content = nm.Replace(fi.Extension, "")
                anm.Foreground = Brushes.White
                anm.HorizontalAlignment = Windows.HorizontalAlignment.Center
                Dim findicon = True
                If Not icopack Is Nothing Then
                    If Not icopack.GetValue("IconPaths", nm) = "Code_Item.NotFound" Then
                        findicon = False
                        Dim bm As New BitmapImage
                        bm.BeginInit()
                        bm.UriSource = New Uri(icopack.GetValue("IconPaths", nm).Replace("{Budgie.BDock.ConfigDirectory}", My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\").Replace("{iniDir}", My.Settings.CurrentIconThemePath.Replace(My.Computer.FileSystem.GetName(My.Settings.CurrentIconThemePath), "")))
                        bm.EndInit()
                        ico.Source = bm
                    End If
                End If
                If findicon Then
                    Try
                        Dim bm As New BitmapImage
                        Dim ai As System.Drawing.Icon = System.Drawing.Icon.ExtractAssociatedIcon(a)
                        ai.ToBitmap().Save(My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\Icons\" + a.Replace(":", "+").Replace("\", "+") + ".png", System.Drawing.Imaging.ImageFormat.Png)
                        bm.BeginInit()
                        bm.UriSource = New Uri(My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\Icons\" + a.Replace(":", "+").Replace("\", "+") + ".png")
                        bm.EndInit()
                        ico.Source = bm
                    Catch
                        If My.Computer.FileSystem.FileExists(My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\Icons\" + a.Replace(":", "+").Replace("\", "+") + ".png") Then
                            Dim bm As New BitmapImage
                            bm.BeginInit()
                            bm.UriSource = New Uri(My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\BudgieDock\Icons\" + a.Replace(":", "+").Replace("\", "+") + ".png")
                            bm.EndInit()
                            ico.Source = bm
                        End If
                    End Try
                End If
                ico.Margin = New Thickness(5)
                ico.Width = 40
                ico.Height = 40
                st.Children.Add(ico)
                st.Children.Add(anm)
                bdr.Child = st
                bdr.Padding = New Thickness(5)
                bdr.CornerRadius = New CornerRadius(10)
                bdr.Focusable = True
                Dim hvcolor As New SolidColorBrush(Color.FromArgb(50, 255, 255, 255))
                Dim appwithoutloop = a
                AddHandler bdr.MouseEnter, Sub(sender, e)
                                               bdr.Background = hvcolor
                                           End Sub
                AddHandler bdr.MouseLeave, Sub(sender, e)
                                               bdr.Background = Brushes.Transparent
                                           End Sub
                Dim ismdown As Boolean = False
                AddHandler bdr.MouseDown, Sub(sender, e)
                                              bdr.Background = Brushes.Black
                                              ismdown = True
                                          End Sub
                AddHandler bdr.MouseUp, Sub(sender, e)
                                            bdr.Background = Brushes.Transparent
                                            If ismdown And handleclick Then
                                                aaps.Children.Clear()
                                                Try
                                                    Process.Start(appwithoutloop)
                                                Catch ex As Exception
                                                    MsgBox(ex.Message, MsgBoxStyle.Critical)
                                                End Try
                                                Close()
                                                ismdown = False
                                            End If
                                        End Sub
                AddHandler bdr.GotFocus, Sub(sender, e)
                                             bdr.Background = Brushes.Black
                                         End Sub
                AddHandler bdr.LostFocus, Sub(sender, e)
                                              bdr.Background = Brushes.Transparent
                                          End Sub
                AddHandler bdr.KeyDown, Sub(sender, e)
                                            If e.Key = Key.Enter Then
                                                bdr.Background = Brushes.Transparent
                                                aaps.Children.Clear()
                                                Try
                                                    Process.Start(appwithoutloop)
                                                Catch ex As Exception
                                                    MsgBox(ex.Message, MsgBoxStyle.Critical)
                                                End Try
                                                Close()
                                            End If
                                        End Sub
                aaps.Children.Add(bdr)
            End If
        Next
    End Sub

    Private Sub SearchTB_TextChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles SearchTB.TextChanged
        findApps(SearchTB.Text)
    End Sub

    Private Sub sp_ScrollChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.ScrollChangedEventArgs) Handles sp.ScrollChanged
        If e.VerticalOffset < 5 Then
            sbarea.Margin = New Thickness(40)
        Else
            sbarea.Margin = New Thickness(32 - (e.VerticalOffset / 5))
            If sbarea.Margin.Top < 10 Then
                sbarea.Margin = New Thickness(10)
            End If
        End If
    End Sub
End Class