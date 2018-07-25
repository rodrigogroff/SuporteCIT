﻿using DataModel;
using System;
using System.Net.Sockets;
using System.Collections;
using System.Threading;
using System.Linq;

#region - ClientConnectionPool - 

public class ClientConnectionPool
{
    Queue SyncdQ = Queue.Synchronized(new Queue());

    public void Enqueue(ClientHandler client)
    {
        SyncdQ.Enqueue(client);
    }

    public ClientHandler Dequeue()
    {
        return (ClientHandler)(SyncdQ.Dequeue());
    }

    public int Count
    {
        get { return SyncdQ.Count; }
    }

    public object SyncRoot
    {
        get { return SyncdQ.SyncRoot; }
    }
}

#endregion

#region - ClientService - 

public class ClientService
{
    const int NUM_OF_THREAD = 50;

    ClientConnectionPool ConnectionPool;
    Thread[] ThreadTask = new Thread[NUM_OF_THREAD];

    bool ContinueProcess = false;

    public ClientService(ClientConnectionPool ConnectionPool)
    {
        this.ConnectionPool = ConnectionPool;
    }

    public void Start()
    {
        ContinueProcess = true;

        // Start threads to handle Client Task
        for (int i = 0; i < ThreadTask.Length; i++)
        {
            ThreadTask[i] = new Thread(new ThreadStart(this.Process));
            ThreadTask[i].Start();
        }
    }

    void Process()
    {
        while (ContinueProcess)
        {
            ClientHandler client = null;

            lock (ConnectionPool.SyncRoot)
            {
                if (ConnectionPool.Count > 0)
                    client = ConnectionPool.Dequeue();
            }

            if (client != null)
            {
                client.Process();

                if (client.Alive)
                    ConnectionPool.Enqueue(client);
            }

            Thread.Sleep(1);
        }
    }

    public void Stop()
    {
        ContinueProcess = false;

        for (int i = 0; i < ThreadTask.Length; i++)
        {
            if (ThreadTask[i] != null && ThreadTask[i].IsAlive)
                ThreadTask[i].Join();
        }

        while (ConnectionPool.Count > 0)
        {
            ClientHandler client = ConnectionPool.Dequeue();
            client.Close();
            Console.WriteLine("Client connection is closed!");
        }
    }
}

#endregion

public class SynchronousSocketListener
{
    public static int portNum = 2700;   // software express

    public static int Main(String[] args)
    {
        T_Loja loj;

        Console.WriteLine("\n" + DateTime.Now + "]");
        Console.WriteLine("\nCNET ISO [" + portNum + "]");

        using (var db = new AutorizadorCNDB())
        {
            var term = db.T_Cartao.FirstOrDefault();
            Console.WriteLine("DBCHECK -> OK!");
        }

        //var ch = new ClientHandler();
        //ch.msgReceived.Append("xx0200B238040020C0100000000000000000000020000000000044820605103044050064103044060502137826766008998000973012650821          CX0000030000000000068084F592E55666434A4");
        //ch.ProcessDataReceived();

        StartListening();

        Console.WriteLine("\nHit enter to continue...");
        Console.Read();

        return 0;
    }

    #region - code - 

    public static void StartListening()
    {
        var ConnectionPool = new ClientConnectionPool();
        var ClientTask = new ClientService(ConnectionPool);

        ClientTask.Start();

        var listener = new TcpListener(portNum);

        try
        {
            listener.Start();

            int ClientNbr = 0;

            // Start listening for connections.
            Console.WriteLine("Waiting for a connection...");

            while (true)
            {
                TcpClient handler = listener.AcceptTcpClient();

                if (handler != null)
                {
                    ++ClientNbr;
                    ConnectionPool.Enqueue(new ClientHandler(handler, ClientNbr));
                }

                Thread.Sleep(1);
            }

            //listener.Stop();
            //ClientTask.Stop();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

#endregion
}