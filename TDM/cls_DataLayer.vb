Imports System.ComponentModel
Imports MDM.LogicLayer
Imports System.IO
Imports System.Drawing.Design
Imports System.ComponentModel.Design
Imports System.Windows.Forms.Design
Imports System.Windows.Forms





Namespace DataLayer

    Public Class Machine
        'Inherits BackUp


        Private myName As String
        Private myInvalidCharactersInColumnName As String
        Private myIPAddress As String
        Private myMarkerPLCState As Integer : Private myMarkerPLCStateTrueValue As Boolean
        Private myMarkerTTEditState As Integer : Private myMarkerTTEditStateTrueValue As Boolean
        Private myCheckManuallyInput As Boolean
        Private myParentConnectionLogbook As String
        Private myInvalidCharactersAutoCorrection As Boolean
        Private myControl As ControlName

        Public Sub New()
            initMembers(Me)
            TouchProbes.Add(New TouchProbe)
            ToolTableBackUp = New ToolTableBackUp
            PresetTableBackUp = New PresetTableBackUp
            myParentConnectionLogbook = Application.StartupPath
        End Sub

        <Category("Entwurf"), _
         DefaultValue(GetType(String), "Maschine"), _
         Description("Der Name der Maschine")> _
        Public Property Name_ID() As String
            Get
                Return myName
            End Get
            Set(ByVal value As String)
                If value IsNot Nothing And value <> "" Then
                    myName = value.Trim.Substring(0, Math.Min(16, value.Length))
                End If

            End Set
        End Property

        <Category("Kommunikation"), _
         DefaultValue(GetType(String), "/\{}[]()+!&%$§<>;:^°|²³äüö"), _
         Description("Alle Zeichen, die in der Werkzeugtabellenspalte 'Name' ungültig sind")> _
        Public Property InvalidCharactersInColumnName() As String
            Get
                Return myInvalidCharactersInColumnName
            End Get
            Set(ByVal value As String)
                myInvalidCharactersInColumnName = value.Trim.Substring(0, Math.Min(64, value.Length))
            End Set
        End Property

        <Category("Entwurf"), _
         Editor(GetType(CollectionEditorWithDescription), GetType(UITypeEditor)), _
         Description("Alle Taster der Maschine")> _
        Public Property TouchProbes As New TouchProbeCollection

        <Category("Kommunikation"), _
         DefaultValue(GetType(String), "127.0.0.1"), _
         Editor(GetType(UITypeEditorEditIP), GetType(UITypeEditor)), _
         TypeConverter(GetType(IPAddressStringConverter)), _
         Description("Die IP-Adresse der Maschine")> _
        Public Property IPAddress As String
            Get
                Return myIPAddress
            End Get
            Set(ByVal value As String)
                If value IsNot Nothing And value <> "" Then
                    myIPAddress = value
                End If
            End Set
        End Property

        <Category("Kommunikation"), _
         DefaultValue(GetType(Integer), "9959"), _
         Description("Der PLC-Status-Merker der Maschine")> _
        Public Property MarkerPLCState() As Integer
            Get
                Return myMarkerPLCState
            End Get
            Set(ByVal value As Integer)
                If value < 10000 And value > 0 Then
                    myMarkerPLCState = value
                End If
            End Set
        End Property

        <Category("Kommunikation"), _
         DefaultValue(GetType(Boolean), "True"), _
         Description("Der PLC-Status-Merker-TrueValue der Maschine")> _
        Public Property MarkerPLCStateTrueValue() As Boolean
            Get
                Return myMarkerPLCStateTrueValue
            End Get
            Set(ByVal value As Boolean)
                myMarkerPLCStateTrueValue = value
            End Set
        End Property

        <Category("Kommunikation"), _
         DefaultValue(GetType(Integer), "9978"), _
         Description("Der TT-Edit-Status-Merker der Maschine")> _
        Public Property MarkerTTEditState() As Integer
            Get
                Return myMarkerTTEditState
            End Get
            Set(ByVal value As Integer)
                If value < 10000 And value > 0 Then
                    myMarkerTTEditState = value
                End If
            End Set
        End Property

        <Category("Kommunikation"), _
         DefaultValue(GetType(Boolean), "True"), _
         Description("Der Werkzeugtabellen-Editierungsstatus-Merker-TrueValuee der Maschine")> _
        Public Property MarkerTTEditStateTrueValue() As Boolean
            Get
                Return myMarkerTTEditStateTrueValue
            End Get
            Set(ByVal value As Boolean)
                myMarkerTTEditStateTrueValue = value
            End Set
        End Property


        <Category("Kommunikation"), _
         DefaultValue(GetType(Boolean), "False"), _
         Description("Merker-Abgleich durch manuelle Eingabe/Bestätigung ersetzen?")> _
        Public Property CheckManuallyInput() As Boolean
            Get
                Return myCheckManuallyInput
            End Get
            Set(ByVal value As Boolean)
                myCheckManuallyInput = value
            End Set
        End Property


        <Category("Kommunikation"), _
         DefaultValue(GetType(Boolean), "True"), _
         Description("Fehlerhafte Zeichen in Spalte 'Name' automatisch korrigieren?")> _
        Public Property InvalidCharactersAutoCorrection() As Boolean
            Get
                Return myInvalidCharactersAutoCorrection
            End Get
            Set(ByVal value As Boolean)
                myInvalidCharactersAutoCorrection = value
            End Set
        End Property

        <Category("Kommunikation"), _
         Editor(GetType(UITypeEditorSelectFolder), GetType(UITypeEditor)), _
         DefaultValue(GetType(String), ""), _
         Description("Das Verzeichnis des 'großen' Verbindungslogbuches")> _
        Public Property ParentConnectionLogbook() As String
            Get
                Return myParentConnectionLogbook
            End Get
            Set(ByVal value As String)
                If value IsNot Nothing Or value <> "" Then
                    If Directory.Exists(value) = True Then
                        myParentConnectionLogbook = value
                    End If
                End If
            End Set
        End Property

        <Category("Steuerung"), _
         DefaultValue(GetType(ControlName), "1"), _
         Description("Die Heidenhain-NC-Steuerung der Maschine")> _
        Public Property [Control]() As ControlName
            Get
                Return myControl
            End Get
            Set(ByVal value As ControlName)
                myControl = value
            End Set
        End Property


        <Category("BackUp/Kommunikation"), _
         Editor(GetType(UITypeEditorEditBackUpConfig), GetType(UITypeEditor)), _
         Description("Die Presettabellen-BackUp-Konfiguration")> _
        Public Property PresetTableBackUp As PresetTableBackUp


        <Category("BackUp/Kommunikation"), _
         Editor(GetType(UITypeEditorEditBackUpConfig), GetType(UITypeEditor)), _
         Description("Die Werkzeugtabellen-BackUp-Konfiguration")> _
        Public Property ToolTableBackUp As ToolTableBackUp

        Enum ControlName As Int16
            [Heidenhain_iTNC530] = 1
            [Heidenhain_TNC420] = 2
        End Enum


#Region "Mathoden/Funktionen"
        Public Overrides Function ToString() As String
            Return Name_ID & " @ " & IPAddress
        End Function
#End Region

    End Class

    Public Class TouchProbe

        Private myToolNumber As Integer
        Private myName As String

        Public Sub New()
            initMembers(Me)
        End Sub

        Public Sub New(ByVal atrToolNumber As Integer, ByVal atrTouchProbeName As String)
            myToolNumber = atrToolNumber
            myName = atrTouchProbeName
        End Sub


        <Category("Entwurf"), _
         DefaultValue(GetType(Integer), "1"), _
         Description("Die Werkzeugnummer des 3D-Tasters in der iTNC-Werkzeugtabelle")>
        Public Property ToolNumber() As Integer
            Get
                Return myToolNumber
            End Get
            Set(ByVal value As Integer)
                If value < 65000 And value > 0 Then
                    myToolNumber = value
                End If
            End Set
        End Property

        <Category("Entwurf"), _
         DefaultValue(GetType(String), "3D-Taster"), _
         Description("Der Name des 3D-Tasters")>
        Public Property Name_ID() As String
            Get
                Return myName
            End Get
            Set(ByVal value As String)
                If value IsNot Nothing And value <> "" Then
                    myName = value.Trim.Substring(0, Math.Min(16, value.Length))
                End If
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return Name_ID & " @ " & ToolNumber
        End Function

    End Class

    Public Class LocalSettings
        Private myAutoConnectMachine As Boolean
        Private myDefaultMachine As String
        Private myShowMachineSelectDialog As Boolean
        Private myCheckIPs As Boolean
        Private myWriteDetailedConnectionLogbook As Boolean
        Private myGlobalSettingsFolder As String
        Private myCloseAfterSync As Boolean
        Private myAskBeforeSync As Boolean
        Private myEnableTDM As Boolean
        Private myEnableTCx000 As Boolean
        Private myEnablePDM As Boolean

        Public Sub New()
            initMembers(Me)
            Machines.Add(New Machine)
            myGlobalSettingsFolder = Application.StartupPath

            For i As Integer = 1 To 10
                Machines.Add(New Machine With {.Name_ID = "Maschine" & i.ToString})
            Next
        End Sub

        <Category("Entwurf"), _
        DefaultValue(GetType(Boolean), "True"), _
        Description("Mit Standardmaschine, oder ausgewählte Maschine automatisch verbinden?")> _
        Public Property AutoConnectMachine() As Boolean
            Get
                Return myAutoConnectMachine
            End Get
            Set(ByVal value As Boolean)
                myAutoConnectMachine = value
            End Set
        End Property

        <Category("Entwurf"), _
        Description("Die Standard-Maschine der lokalen Anwendungsinstanz"), _
        DefaultValue(GetType(String), "Maschine"), _
        TypeConverter(GetType(MachinesConverter)), _
        Editor(GetType(CollectionEditorWithDescription), GetType(UITypeEditor))> _
        Public Property DefaultMachine() As String
            Get
                Return myDefaultMachine
            End Get
            Set(ByVal value As String)
                myDefaultMachine = value
            End Set
        End Property

        <Category("Entwurf"), _
        DefaultValue(GetType(Boolean), "True"), _
        Description("Maschinen-Auswahl-Dialog beim Start anzeigen?")> _
        Public Property ShowMachineSelectDialog() As Boolean
            Get
                Return myShowMachineSelectDialog
            End Get
            Set(ByVal value As Boolean)
                myShowMachineSelectDialog = value
            End Set
        End Property

        <Category("Entwurf"), _
        DefaultValue(GetType(Boolean), "True"), _
        Description("Die Anzahl der gesendetet und empfangen IP-Pakete vergleichen/prüfen?")> _
        Public Property CheckIPs() As Boolean
            Get
                Return myCheckIPs
            End Get
            Set(ByVal value As Boolean)
                myCheckIPs = value
            End Set
        End Property

        <Category("Entwurf"), _
        DefaultValue(GetType(Boolean), "True"), _
        Description("Ausführliches Logbuch protokollieren?")> _
        Public Property WriteDetailedConnectionLogbook() As Boolean
            Get
                Return myWriteDetailedConnectionLogbook
            End Get
            Set(ByVal value As Boolean)
                myWriteDetailedConnectionLogbook = value
            End Set
        End Property


        <Category("Entwurf"), _
        Editor(GetType(UITypeEditorSelectFolder), GetType(UITypeEditor)), _
        DefaultValue(GetType(String), ""), _
        Description("Das Verzeichnis der lokalen Einstellungsdatei")> _
        Public Property GlobalSettingsFolder() As String
            Get
                Return myGlobalSettingsFolder
            End Get
            Set(ByVal value As String)
                If value IsNot Nothing Or value <> "" Then
                    If Directory.Exists(value) = True Then
                        myGlobalSettingsFolder = value
                    End If
                End If
            End Set
        End Property


        <Category("Entwurf"), _
        DefaultValue(GetType(Boolean), "False"), _
        Description("Programm nach Datenübertragung schließen?")> _
        Public Property CloseAfterSync() As Boolean
            Get
                Return myCloseAfterSync
            End Get
            Set(ByVal value As Boolean)
                myCloseAfterSync = value
            End Set
        End Property


        <Category("Entwurf"), _
        DefaultValue(GetType(Boolean), "True"), _
        Description("Abfrage vor Datenübertragung?")> _
        Public Property AskBeforeSync() As Boolean
            Get
                Return myAskBeforeSync
            End Get
            Set(ByVal value As Boolean)
                myAskBeforeSync = value
            End Set
        End Property


        <Category("Programmfunktionen aktiviren/deaktiviren"), _
        DefaultValue(GetType(Boolean), "True"), _
        Description("Werkzeugdatenmanagement aktiv?")> _
        Public Property EnableTDM() As Boolean
            Get
                Return myEnableTDM
            End Get
            Set(ByVal value As Boolean)
                myEnableTDM = value
            End Set
        End Property


        <Category("Programmfunktionen aktiviren/deaktiviren"), _
        DefaultValue(GetType(Boolean), "True"), _
        Description("TCx000 aktiv?")> _
        Public Property EnableTCx000() As Boolean
            Get
                Return myEnableTCx000
            End Get
            Set(ByVal value As Boolean)
                myEnableTCx000 = value
            End Set
        End Property

        <Category("Programmfunktionen aktiviren/deaktiviren"), _
        DefaultValue(GetType(Boolean), "True"), _
        Description("Presetdatenmanagement aktiv?")> _
        Public Property EnablePDM() As Boolean
            Get
                Return myEnablePDM
            End Get
            Set(ByVal value As Boolean)
                myEnablePDM = value
            End Set
        End Property

        <Category("Entwurf"), _
         Editor(GetType(CollectionEditorWithDescription), GetType(UITypeEditor)), _
         Description("Alle Maschinen")> _
        Property Machines As New MachineCollection

        Private WithEvents MachinesCache As MachineCollection = Machines 'Event Verwaltet das richtige Anzeigen einer gültigen DefaultMachine im DropDown
        Private Sub MachinesCache_ItemsChanged(ByVal Sender As Object) Handles MachinesCache.ItemsChanged
            MachinesConverter.OptionStringArray = Machines.AllItemsAsArray
        End Sub


    End Class

    Public Class BackUp
        Implements ICloneable

        Private myBackUpFileSyntax As String
        Private myBackUpFileCheckSyntax As String
        Private myBackUpFile As String
        Private myPCBackUpFolder As String
        Private myTNCBackUpFolder As String
        Private myCreatingPCBackUp As Boolean
        Private myCreatingTNCBackUp As Boolean
        Private myPCBackUpLifeTime As Integer
        Private myTNCBackUpLifeTime As Integer

        Public Sub New()
            initMembers(Me)
            myPCBackUpFolder = Path.Combine(Application.StartupPath, "BackUpFiles")
        End Sub

        <Category("BackUp-Syntax"), _
        DefaultValue(GetType(String), "%data%_%time:~0,2%-%time:~3,2%-%time:~6,2%.t"), _
        Description("Der Syntax der BackUp-Datei")> _
        Public Property BackUpFileSyntax() As String
            Get
                Return myBackUpFileSyntax
            End Get
            Set(ByVal value As String)
                myBackUpFileSyntax = value
            End Set
        End Property

        <Category("BackUp-Syntax"), _
        DefaultValue(GetType(String), "dd.MM.yyyy_HH-mm-ss"), _
        Description("Der VB.NET-Date/Time Syntax der das Alter der BackUps überprüft")> _
        Public Property BackUpFileCheckSyntax() As String
            Get
                Return myBackUpFileCheckSyntax
            End Get
            Set(ByVal value As String)
                myBackUpFileCheckSyntax = value
            End Set
        End Property

        <Category("BackUp-Einstellungen-PC"), _
        Editor(GetType(UITypeEditorSelectFolder), GetType(UITypeEditor)), _
        DefaultValue(GetType(String), ""), _
        Description("Das BackUp-Verzeichnis auf dem PC/dem Netzlaufwerk")> _
        Public Property PCBackUpFolder() As String
            Get
                Return myPCBackUpFolder
            End Get
            Set(ByVal value As String)
                If value IsNot Nothing Or value <> "" Then
                    If Directory.Exists(value) = True Then
                        myPCBackUpFolder = value
                    End If
                End If
            End Set
        End Property

        <Category("BackUp-Einstellungen-TNC"), _
        DefaultValue(GetType(String), "TNC:\BackUps"), _
        Description("Das BackUp-Verzeichnis auf der Maschine")> _
        Public Property TNCBackUpFolder() As String
            Get
                Return myTNCBackUpFolder
            End Get
            Set(ByVal value As String)
                myTNCBackUpFolder = value
            End Set
        End Property


        <Category("BackUp-Einstellungen-PC"), _
        DefaultValue(GetType(Boolean), "True"), _
        Description("PC-BackUp erstellen?")> _
        Public Property CreatingPCBackUp() As Boolean
            Get
                Return myCreatingPCBackUp
            End Get
            Set(ByVal value As Boolean)
                myCreatingPCBackUp = value
            End Set
        End Property

        <Category("BackUp-Einstellungen-TNC"), _
        DefaultValue(GetType(Boolean), "True"), _
        Description("TNC-BackUp erstellen?")> _
        Public Property CreatingTNCBackUp() As Boolean
            Get
                Return myCreatingTNCBackUp
            End Get
            Set(ByVal value As Boolean)
                myCreatingTNCBackUp = value
            End Set
        End Property

        <Category("BackUp-Einstellungen-PC"), _
        DefaultValue(GetType(Integer), "3"), _
        Description("Die Lebensdauer des PC-BackUp's (in Tagen), bevor es bei der nächsten Datenübertragung wieder gelöscht wird")> _
        Public Property PCBackUpLifeTime() As Integer
            Get
                Return myPCBackUpLifeTime
            End Get
            Set(ByVal value As Integer)
                If value < 8 And value > 0 Then
                    myPCBackUpLifeTime = value
                End If
            End Set
        End Property

        <Category("BackUp-Einstellungen-TNC"), _
        DefaultValue(GetType(Integer), "3"), _
        Description("Die Lebensdauer des TNC-BackUp's (in Tagen), bevor es bei der nächsten Datenübertragung wieder gelöscht wird")> _
        Public Property TNCBackUpLifeTime() As Integer
            Get
                Return myTNCBackUpLifeTime
            End Get
            Set(ByVal value As Integer)
                If value < 8 And value > 0 Then
                    myTNCBackUpLifeTime = value
                End If
            End Set
        End Property

        Public Function Clone() As Object Implements System.ICloneable.Clone
            Return MemberwiseClone()
        End Function

    End Class

    Public Class ToolTableBackUp
        Inherits BackUp

        Private myToolTable As String

        <Category("BackUp-Datei"), _
        DefaultValue(GetType(String), "TNC:\Tool.t"), _
        Description("Die TNC-Werkzeugtabelle (einschließlich Pfad) von welcher ein TNC- oder PC-BackUp erstellt werden soll")> _
        Public Property ToolTable() As String
            Get
                Return myToolTable
            End Get
            Set(ByVal value As String)
                myToolTable = value
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return ToolTable
        End Function

    End Class

    Public Class PresetTableBackUp
        Inherits BackUp

        Private myPresetTable As String

        <Category("BackUp-Datei"), _
        DefaultValue(GetType(String), "TNC:\Preset.pr"), _
        Description("Die TNC-Presettabelle (einschließlich Pfad) von welcher ein TNC- oder PC-BackUp erstellt werden soll")> _
        Public Property PresetTable() As String
            Get
                Return myPresetTable
            End Get
            Set(ByVal value As String)
                myPresetTable = value
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return PresetTable
        End Function

    End Class

    Public Class UITypeEditorEditBackUpConfig
        Inherits UITypeEditor

        Public Overrides Function GetEditStyle(context As System.ComponentModel.ITypeDescriptorContext) As System.Drawing.Design.UITypeEditorEditStyle
            If Not context Is Nothing AndAlso Not context.Instance Is Nothing Then
                Return UITypeEditorEditStyle.Modal
            End If
            Return UITypeEditorEditStyle.None
        End Function

        Public Overrides Function EditValue(context As System.ComponentModel.ITypeDescriptorContext, provider As System.IServiceProvider, value As Object) As Object

            Dim locDialog As New frmBackUpConfig

            Dim locDialogResult As BackUp = locDialog.ShowDiaAndEditObject(value)

            If locDialogResult IsNot Nothing Then
                Return locDialogResult
            Else
                Return value '<-- Dieser Parameter wird komischerweise immer verändert, obwohl er eigentlich nirgendwo verändert wird!? 
            End If

        End Function

    End Class

    Public Class UITypeEditorEditIP
        Inherits UITypeEditor

        Public Overrides Function GetEditStyle(context As System.ComponentModel.ITypeDescriptorContext) As System.Drawing.Design.UITypeEditorEditStyle
            If Not context Is Nothing AndAlso Not context.Instance Is Nothing Then
                Return UITypeEditorEditStyle.Modal
            End If
            Return UITypeEditorEditStyle.None
        End Function

        Public Overrides Function EditValue(context As System.ComponentModel.ITypeDescriptorContext, provider As System.IServiceProvider, value As Object) As Object
            Dim locDialog As New frmIPAdrs
            Dim locDialogResult As String = locDialog.ShowDiaAndEditObject(value)
            If locDialogResult IsNot Nothing Then
                Return locDialogResult
            Else
                Return value
            End If
        End Function
    End Class

    Public Class UITypeEditorSelectFile
        Inherits UITypeEditor

        Public Overrides Function GetEditStyle(context As System.ComponentModel.ITypeDescriptorContext) As System.Drawing.Design.UITypeEditorEditStyle
            If Not context Is Nothing AndAlso Not context.Instance Is Nothing Then
                Return UITypeEditorEditStyle.Modal
            End If
            Return UITypeEditorEditStyle.None
        End Function

        Public Overrides Function EditValue(context As System.ComponentModel.ITypeDescriptorContext, provider As System.IServiceProvider, value As Object) As Object
            Dim myDialog As New OpenFileDialog
            If myDialog.ShowDialog = DialogResult.OK Then
                Return myDialog.FileName
            Else
                Return Nothing
            End If
        End Function
    End Class

    Public Class UITypeEditorSelectFolder
        Inherits UITypeEditor

        Public Overrides Function GetEditStyle(context As System.ComponentModel.ITypeDescriptorContext) As System.Drawing.Design.UITypeEditorEditStyle
            If Not context Is Nothing AndAlso Not context.Instance Is Nothing Then
                Return UITypeEditorEditStyle.Modal
            End If
            Return UITypeEditorEditStyle.None
        End Function

        Public Overrides Function EditValue(context As System.ComponentModel.ITypeDescriptorContext, provider As System.IServiceProvider, value As Object) As Object
            Dim myDialog As New FolderBrowserDialog
            myDialog.Description = "Bitte den Ordner für " & context.PropertyDescriptor.DisplayName & _
                                   " von " & context.Instance.ToString & " wählen..."
            If Directory.Exists(value) Then myDialog.SelectedPath = value.ToString
            If myDialog.ShowDialog = DialogResult.OK Then
                Return myDialog.SelectedPath
            Else
                Return Nothing
            End If
        End Function

    End Class

    Public Class MachinesConverter
        Inherits StringConverter

        Friend Shared OptionStringArray() As String

        Public Overrides Function GetStandardValuesSupported(context As System.ComponentModel.ITypeDescriptorContext) As Boolean
            Return True
        End Function

        Public Overrides Function GetStandardValuesExclusive(context As System.ComponentModel.ITypeDescriptorContext) As Boolean
            Return True
        End Function

        Public Overrides Function GetStandardValues(context As System.ComponentModel.ITypeDescriptorContext) As System.ComponentModel.TypeConverter.StandardValuesCollection
            Return New StandardValuesCollection(MachinesConverter.OptionStringArray)
        End Function
    End Class

    Public Class ClassInfo

        Private myCreatedDate As Date
        Private myCreatedBy As String


        Public Sub New()
            MyBase.New()
            myCreatedDate = Date.Now
            myCreatedBy = My.Computer.Name
        End Sub

#Region "ReadOnly Properties"
        <Category("Klassen-Informationen"), _
         Description("Erstellungsdatum der Maschine...")>
        Public ReadOnly Property CreatedDate() As Date
            Get
                Return myCreatedDate
            End Get
        End Property

        <Category("Klassen-Informationen"), _
         Description("Maschine erstellt von...")>
        Public ReadOnly Property CreatedBy() As String
            Get
                Return myCreatedBy
            End Get
        End Property
#End Region

    End Class

    Class CollectionEditorWithDescription
        Inherits CollectionEditor

        Public Sub New(ByVal type As Type)
            MyBase.New(type)
        End Sub

        Protected Overrides Function CreateCollectionForm() As CollectionForm
            Dim form As CollectionForm = MyBase.CreateCollectionForm()
            AddHandler form.Shown, Sub()
                                       ShowDescription(form)
                                   End Sub
            Return form
        End Function

        Private Shared Sub ShowDescription(control As Control)
            Dim grid As PropertyGrid = TryCast(control, PropertyGrid)
            If grid IsNot Nothing Then grid.HelpVisible = True
            For Each child As Control In control.Controls
                ShowDescription(child)
            Next
        End Sub


        Public Overrides Function EditValue(context As System.ComponentModel.ITypeDescriptorContext, provider As System.IServiceProvider, value As Object) As Object

            Dim locTrans As DesignerTransaction = Nothing
            Dim locHost As IDesignerHost = provider.GetService(GetType(IDesignerHost))

            If locHost IsNot Nothing Then
                locTrans = locHost.CreateTransaction()
            End If

            Dim locForms As IWindowsFormsEditorService = provider.GetService(GetType(IWindowsFormsEditorService))

            Try

                Stop
                Dim locForm As New myCollectionForm(Me)

                locForm.EditValue = value

                Dim result As DialogResult = locForm.ShowEditorDialog(locForms)

                Stop

                If result = DialogResult.OK Then
                    value = locForm.EditValue
                    If locTrans IsNot Nothing Then
                        locTrans.Commit()
                    End If
                ElseIf locTrans IsNot Nothing Then
                    locTrans.Cancel()
                End If
            Catch
                If locTrans IsNot Nothing Then
                    locTrans.Cancel()
                End If
            End Try

            Return value

        End Function

        Protected Class myCollectionForm
            Inherits CollectionForm

            Public Sub New(ByVal editor As CollectionEditor)
                MyBase.New(editor)
            End Sub

            Overloads Function ShowEditorDialog(edSvc As System.Windows.Forms.Design.IWindowsFormsEditorService) As System.Windows.Forms.DialogResult
                Return MyBase.ShowEditorDialog(edSvc)
            End Function

            Protected Overrides Sub OnEditValueChanged()

            End Sub

        End Class

    End Class

    Class IPAddressStringConverter
        Inherits StringConverter

        Public Overrides Function GetStandardValuesExclusive(context As System.ComponentModel.ITypeDescriptorContext) As Boolean
            Return True
        End Function

    End Class



End Namespace
