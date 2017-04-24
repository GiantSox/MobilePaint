using UnityEngine;
using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityOSC;

public class oscBundleTest : MonoBehaviour {

    
    //private Thread thread;
    private OSCServer oscin;


    void Start()
    {
        oscin = new OSCServer( 31337 );
        /*thread = new Thread(new ThreadStart(UpdateOSC));
        thread.Start();*/
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

    void UpdateOSC()
    {
        while (true)
        {
            OSCPacket msg = oscin.LastReceivedPacket;
            if (msg != null)
            {
                Debug.Log("new osc message");
                if (msg.IsBundle())
                {
                    Debug.Log("new osc bundle");
                    OSCBundle b = (OSCBundle)msg;
                    foreach (OSCPacket subm in b.Data)
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
    }

    void parseMessage(OSCPacket msg)
    {
        Debug.Log("message with address: " + msg.Address);
        // get a value:
        int v = (int)msg.Data[0];
    }

    void Update()
    {
        OSCPacket msg = oscin.LastReceivedPacket;
        if(msg != null)
        {
            parseMessage(msg);
            /*Debug.Log("message received");
            if(msg.IsBundle())
            {
                Debug.Log("bundle received");
                //OSCBundle bundle = (OSCBundle)msg;
            }
            else
            {
                parseMessage(msg);
            }*/
           
        }
    }
}