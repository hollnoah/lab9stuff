Imports System.IO.Ports
Imports System.Windows.Forms.VisualStyles.VisualStyleElement


Public Class lab9stuff
    Sub PopulatePorts()
        Dim portNames() As String = SerialPort.GetPortNames()
        Dim previousSelection As String = If(ComboBox1.SelectedItem IsNot Nothing, ComboBox1.SelectedItem.ToString(), String.Empty)

        ComboBox1.BeginUpdate()
        ComboBox1.Items.Clear()
        For Each pn In portNames
            ComboBox1.Items.Add(pn)
        Next

        If Not String.IsNullOrEmpty(previousSelection) AndAlso Array.IndexOf(portNames, previousSelection) >= 0 Then
            ComboBox1.SelectedItem = previousSelection
        ElseIf ComboBox1.Items.Count > 0 Then
            ComboBox1.SelectedIndex = 0
        End If
        ComboBox1.EndUpdate()
    End Sub

    Sub Connect()
        ' Use the combo box selection; do not repopulate here (call PopulatePorts separately)
        If ComboBox1.SelectedItem Is Nothing Then
            Console.WriteLine("No COM port selected.")
            Return
        End If

        Dim selectedPort As String = ComboBox1.SelectedItem.ToString()
        Dim available = SerialPort.GetPortNames()
        If Array.IndexOf(available, selectedPort) < 0 Then
            Console.WriteLine($"Selected port {selectedPort} is not available. Available: {String.Join(", ", available)}")
            Return
        End If

        Try
            If SerialPort1.IsOpen Then SerialPort1.Close()
            SerialPort1.BaudRate = 9600 'Q@ Board Default
            SerialPort1.Parity = Parity.None
            SerialPort1.StopBits = StopBits.One
            SerialPort1.DataBits = 8
            SerialPort1.PortName = selectedPort

            SerialPort1.Open()
            Console.WriteLine($"Opened {selectedPort}")
        Catch ex As Exception
            Console.WriteLine($"Failed to open {selectedPort}: {ex.Message}")
        End Try

        ReadDataTimer.Start()

    End Sub

    Sub Write()
        'writes a $ to the serial port to request data from the Q@ board
        If SerialPort1.IsOpen Then
            SerialPort1.Write("$")
        Else
            Console.WriteLine("Serial port is not open. Cannot write data.")
        End If


    End Sub
    '-------------------------------------------------EVENT HANDLERS----------------------------------------------------------------------------------------------------------
    Private Sub SerialPort1_DataReceived(sender As Object, e As SerialDataReceivedEventArgs) Handles SerialPort1.DataReceived
        CheckForIllegalCrossThreadCalls = False

        Dim dataReceived As String = SerialPort1.ReadExisting()
        TextBox1.Text = dataReceived
    End Sub

    Private Sub ComboBox1_DropDown(sender As Object, e As EventArgs) Handles ComboBox1.DropDown
        ' Refresh list when the user opens the dropdown
        PopulatePorts()
    End Sub

    Private Sub ConnectButton_Click(sender As Object, e As EventArgs) Handles ConnectButton.Click
        Connect()
        'Write()
    End Sub

    Private Sub ReadDataTimer_Tick(sender As Object, e As EventArgs) Handles ReadDataTimer.Tick
        Dim byteMyData(SerialPort1.BytesToRead) As Byte
        Dim temp As String
        SerialPort1.Read(byteMyData, 0, SerialPort1.BytesToRead)
        For Each thing In byteMyData
            temp &= Chr(thing)
        Next
        Me.Text = temp

    End Sub

    Private Sub lab9stuff_Click(sender As Object, e As EventArgs) Handles Me.Click
        Write()
    End Sub


End Class
