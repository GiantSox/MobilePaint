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

    public GameObject hand;
    public bool EditorOnlyLeftyMode = false;
    
    private OSCServer oscin;
    Vector3 head;
    Vector3 leftHand;
    Vector3 rightHand;

    void Start()
    {
        oscin = new OSCServer(oscPort);
        Debug.Log("osc server started on port " + oscPort);
    }

    void OnApplicationQuit()
    {
        oscin.Close();
    }

    GvrSettings.UserPrefsHandedness getHandedness()
    {
        if(EditorOnlyLeftyMode)
        {
            return GvrSettings.UserPrefsHandedness.Left;
        }
        else
        {
            return GvrSettings.Handedness;
        }
    }

    void parsePacket()
    {
        OSCPacket message = oscin.LastReceivedPacket;
        if (message != null)
        {
            if (message.IsBundle())
            {
                parseBundle(message);
            }
            else
            {
                parseMessage(message);
            }
        }
    }

    void parseBundle(OSCPacket msg)
    {
        #region Non-Bundle Assert
        if (!msg.IsBundle())
        {
            Debug.Assert(msg.IsBundle(), "parseBundle is only used for parsing bundles!");
        }
        #endregion
        OSCBundle bundle = (OSCBundle)msg;
        foreach (OSCPacket submessage in bundle.Data)
        {
            parseMessage(submessage);

        }
    }

    void parseMessage(OSCPacket msg)
    {
        #region Bundle Assert
        if (msg.IsBundle())
        {
            Debug.Assert(!msg.IsBundle(), "Use parseBundle() to parse bundles!");
        }
        #endregion

        //Debug.Log("message with address: " + msg.Address + " and value: " + msg.Data[0]);
        switch (msg.Address)
        {
            case "/head/":  //case not case:
                head.x = (float)msg.Data[0];
                head.y = (float)msg.Data[1];
                head.z = (float)msg.Data[2];
                break;
            case "/leftHand/":
                leftHand.x = (float)msg.Data[0];
                leftHand.y = (float)msg.Data[1];
                leftHand.z = (float)msg.Data[2];
                break;
            case "/rightHand/":
                rightHand.x = (float)msg.Data[0];
                rightHand.y = (float)msg.Data[1];
                rightHand.z = (float)msg.Data[2];
                break;
        }
    }

    void Update()
    {
        parsePacket();

        //head.z = head.z * -1;
        head.z = 0 - head.z;
        transform.position = head;
        if(getHandedness() == GvrSettings.UserPrefsHandedness.Left)
        {
            leftHand.z = 0 - leftHand.z;
            hand.gameObject.transform.position = leftHand;
        }
        else
        {
            rightHand.z = 0 - rightHand.z;
            hand.gameObject.transform.position = rightHand;
        }
    }
}