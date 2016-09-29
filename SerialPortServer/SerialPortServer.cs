using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace GK.SerialPortServer
{
    public class SerialPortServer
    {
        private string _PortName;
        private int _BaudRate;
        private Parity _Parity;
        private int _DataBits;
        private StopBits _StopBits;
        private SerialPort port;

        public SerialPortServer(string portName,int baudRate)
        {
            _PortName = portName;
            _BaudRate = baudRate;
            port = new SerialPort(_PortName, _BaudRate);
        }
        public SerialPortServer(string portName,int baudRate,Parity parity)
        { 
            _PortName=portName;
            _BaudRate=baudRate;
            _Parity=parity;
            port = new SerialPort(_PortName, _BaudRate, _Parity);
        }
        public SerialPortServer(string portName,int baudRate,int dataBits,Parity parity,StopBits stopBits)
        { 
            _PortName=portName;
            _BaudRate=baudRate;
            _DataBits = dataBits;
            _Parity=parity;
            _StopBits = stopBits;
            port = new SerialPort(_PortName, _BaudRate, _Parity, _DataBits, _StopBits);
        }
        public void Start()
        {
            port.Open();
        }

        public SerialPort Port
        {
            get { return port; }
        }
        public void Stop()
        {
            port.Close();
        }
    }
}
