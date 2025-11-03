<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class lab9stuff
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.SerialPort1 = New System.IO.Ports.SerialPort(Me.components)
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.ConnectButton = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.ReadDataTimer = New System.Windows.Forms.Timer(Me.components)
        Me.SendButton = New System.Windows.Forms.Button()
        Me.TrackBar1 = New System.Windows.Forms.TrackBar()
        Me.ADCRequestButton = New System.Windows.Forms.Button()
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SerialPort1
        '
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(644, 319)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(144, 24)
        Me.ComboBox1.TabIndex = 0
        '
        'ConnectButton
        '
        Me.ConnectButton.Location = New System.Drawing.Point(644, 377)
        Me.ConnectButton.Name = "ConnectButton"
        Me.ConnectButton.Size = New System.Drawing.Size(144, 61)
        Me.ConnectButton.TabIndex = 1
        Me.ConnectButton.Text = "Connect"
        Me.ConnectButton.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(482, 321)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(144, 22)
        Me.TextBox1.TabIndex = 2
        '
        'ReadDataTimer
        '
        '
        'SendButton
        '
        Me.SendButton.Location = New System.Drawing.Point(483, 378)
        Me.SendButton.Name = "SendButton"
        Me.SendButton.Size = New System.Drawing.Size(143, 60)
        Me.SendButton.TabIndex = 3
        Me.SendButton.Text = "Send"
        Me.SendButton.UseVisualStyleBackColor = True
        '
        'TrackBar1
        '
        Me.TrackBar1.Location = New System.Drawing.Point(413, 212)
        Me.TrackBar1.Maximum = 15
        Me.TrackBar1.Name = "TrackBar1"
        Me.TrackBar1.Size = New System.Drawing.Size(375, 56)
        Me.TrackBar1.TabIndex = 4
        '
        'ADCRequestButton
        '
        Me.ADCRequestButton.Location = New System.Drawing.Point(323, 377)
        Me.ADCRequestButton.Name = "ADCRequestButton"
        Me.ADCRequestButton.Size = New System.Drawing.Size(143, 61)
        Me.ADCRequestButton.TabIndex = 5
        Me.ADCRequestButton.Text = "Request"
        Me.ADCRequestButton.UseVisualStyleBackColor = True
        '
        'lab9stuff
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.ADCRequestButton)
        Me.Controls.Add(Me.TrackBar1)
        Me.Controls.Add(Me.SendButton)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.ConnectButton)
        Me.Controls.Add(Me.ComboBox1)
        Me.Name = "lab9stuff"
        Me.Text = "Form1"
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents SerialPort1 As IO.Ports.SerialPort
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents ConnectButton As Button
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents ReadDataTimer As Timer
    Friend WithEvents SendButton As Button
    Friend WithEvents TrackBar1 As TrackBar
    Friend WithEvents ADCRequestButton As Button
End Class
