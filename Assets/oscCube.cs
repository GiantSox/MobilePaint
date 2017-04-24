using UnityEngine;
using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityOSC;

public class oscCube : MonoBehaviour
{
    public int oscPort = 12345;

    private Thread thread;
    private OSCServer oscin;
    Vector3 head;
    Vector3 leftHand;
    Vector3 rightHand;

    void Start()
    {
        oscin = new OSCServer(oscPort);
        Debug.Log("osc server started on port " + oscPort);
        //thread = new Thread(new ThreadStart(UpdateOSC));
        //thread.Start();
    }

    void OnApplicationQuit()
    {
        oscin.Close();
        /*thread.Interrupt();
        if (!thread.Join(2000))
        {
            thread.Abort();
        }*/
    }

    /*void UpdateOSC()
    {
        while (true)
        {
            OSCPacket msg = oscin.LastReceivedPacket;
            if (msg != null)
            {
                if (msg.IsBundle())
                {
                    OSCBundle b = (OSCBundle)msg;
                    foreach (OSCPacket subm in b.Values)
                    {
                        parseMessage(subm);
                    }
                }
                else {
                    parseMessage(msg);
                }
            }
            //Thread.Sleep(5);
        }
    }*/

    void parseMessage(OSCPacket msg)
    {
        Debug.Log("message with address: " + msg.Address + " and value: " + msg.Data[0]);
        if (msg.Address == "/head/")
        {
            head.x = (float)msg.Data[0];
            head.y = (float)msg.Data[1];
            //head.z = (float)msg.Data[2];
        }/*
        else if (msg.Address == "/leftHand/")
        {
            leftHand.x = (float)msg.Data[0];
            leftHand.y = (float)msg.Data[1];
            //leftHand.z = (float)msg.Data[2];
        }
        else if (msg.Address == "/rightHand/")
        {
            rightHand.x = (float)msg.Data[0];
            rightHand.y = (float)msg.Data[1];
            //rightHand.z = (float)msg.Data[2];
        }*/
    }

    void Update()
    {
        OSCPacket message = oscin.LastReceivedPacket;
        if (message != null)
        {
            parseMessage(message);
        }
        transform.position = head;

    }
}