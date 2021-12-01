using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;
using System.Threading.Tasks;
using TMPro;

public class OPC_UA : MonoBehaviour
{
    private Session session;

    bool connected = false;
    public static float Power_OPCUA, WindSpeed_OPCUA, Blade_OPCUA, WindDir_OPCUA, Feedback_OPCUA, Command_OPCUA;
    ApplicationConfiguration config;

    public string windspeed_wr;
    public string WindDir_wr;
    public string Blade_wr;
    public string windspeed_rd;
    public string WindDir_rd;
    public string creator_rd;
    public string Blade_rd;
    public string Feedback;
    public string Command;


    //public Button WriteData;
    public Button OPC_Connection;
    public TMP_InputField IPInputData;
    public static string IPAddress;
    bool flag;
    int timecount = 0;
    int timemax = 50;

    /// <summary>
    /// Gets or sets the server URL.
    /// </summary>
    //public string ServerUrl { get; set; } = "opc.tcp://localhost:62541/Quickstarts/ReferenceServer";
    //public string ServerUrl { get; set; } = "opc.tcp://10.24.91.169:62541/Quickstarts/ReferenceServer"; //Ashkan IP
    //public string ServerUrl { get; set; } = "opc.tcp://10.24.93.140:62541/Quickstarts/ReferenceServer";//Zhicheng IP
    public string ServerUrl { get; set; } = IPAddress;
    public void Start()
    {
        flag = false;
        IPInputData.text = "10.24.91.169";
        //WriteData.onClick.AddListener(WriteDataToOPC);
    }

    void FixedUpdate()
    {

        IPAddress = ("opc.tcp://" + IPInputData.text.ToString() + ":62541/Quickstarts/ReferenceServer");
        ServerUrl = IPAddress;
        if (flag)
        {
            if (timecount >= timemax)
            {

                WindDir_OPCUA = float.Parse(WindDir_rd);
                WindSpeed_OPCUA = float.Parse(windspeed_rd);
                Blade_OPCUA = float.Parse(Blade_rd);
                Feedback_OPCUA = float.Parse(Feedback);
                Command_OPCUA = float.Parse(Command);
                WriteDataToOPC();
            }
            timecount++;
            //Debug.Log("WindSpeed = " + WindDir_OPCUA);
        }
    }

    void WriteDataToOPC()
    {
        //Blade_wr = InputData.text.ToString(); //UnityEngine.Random.value.ToString();
        //WriteNodes("Blade", Blade_wr);

    }
    //void OnGUI()
    //{

    //    GUIStyle mystyle2 = new GUIStyle(GUI.skin.button);
    //    mystyle2.fontSize = 25;
    //    mystyle2.alignment = TextAnchor.MiddleLeft;
    //    mystyle2.fontStyle = FontStyle.Bold;
    //    mystyle2.normal.textColor = Color.red;

    //    GUI.backgroundColor = Color.black;

    //    GUIStyle mystyle = new GUIStyle(GUI.skin.button);
    //    mystyle.fontSize = 25;
    //    mystyle.alignment = TextAnchor.MiddleLeft;
    //    mystyle.fontStyle = FontStyle.Bold;
    //    mystyle.normal.textColor = Color.yellow;



    //    GUI.Box(new Rect(10, 180, 200, 40), UAClient.windspeed_rd.ToString(), mystyle);
    //    GUI.Box(new Rect(10, 240, 200, 40), UAClient.windpower_rd.ToString(), mystyle);


    //}

    public void onClick()
    {
        //OPCStart();
        _ = Init_OPC();
        Connect_OPC();
        OPC_Connection.GetComponent<Image>().color = new Color32(87, 201, 98, 255);
    }

    public async Task Init_OPC()
    {
        /*
        //console = new ConsoleOutput();
        Debug.Log("OPC UA Console Reference Client");
        try
        {
            // Define the UA Client application
            ApplicationInstance application = new ApplicationInstance();
            application.ApplicationName = "Quickstart Console Reference Client";
            application.ApplicationType = ApplicationType.Client;

            // load the application configuration.
            await application.LoadApplicationConfiguration("ConsoleReferenceClient.Config.xml", silent: false);
            // check the application certificate.
            //await application.CheckApplicationInstanceCertificate(silent: false, minimumKeySize: 0);

            // create the UA Client object and connect to configured server.
            uaClient = new UAClient(application.ApplicationConfiguration, ClientBase.ValidateResponse);
            // set form1
            //uaClient.displayer = this;



            //Debug.Log("\nProgram ended.");
            //Debug.Log("Press any key to finish...");
            //Console.ReadKey();
            connected = await uaClient.ConnectAsync();
            if (connected)
            {
                // Run tests for available methods.
                //uaClient.ReadNodes();
                //uaClient.WriteNodes();
                uaClient.Browse();
                uaClient.CallMethod();

                uaClient.SubscribeToDataChanges();
                // Wait for some DataChange notifications from MonitoredItems
                //await Task.Delay(20_000);

                //uaClient.Disconnect();
            }
            else
            {
                Debug.Log("Could not connect to server!");
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }*/
        Debug.Log("Step 1 - Create a config.");
        config = new ApplicationConfiguration()
        {
            ApplicationName = "test-opc",
            ApplicationType = ApplicationType.Client,
            SecurityConfiguration = new SecurityConfiguration { ApplicationCertificate = new CertificateIdentifier() },
            TransportConfigurations = new TransportConfigurationCollection(),
            TransportQuotas = new TransportQuotas { OperationTimeout = 15000 },
            ClientConfiguration = new ClientConfiguration { DefaultSessionTimeout = 60000 }
        };
        await config.Validate(ApplicationType.Client);
        if (config.SecurityConfiguration.AutoAcceptUntrustedCertificates)
        {
            config.CertificateValidator.CertificateValidation += (s, e) => { e.Accept = (e.Error.StatusCode == StatusCodes.BadCertificateUntrusted); };
        }

        Debug.Log("Step 2 - Create a session with your server.");
    }

    private async void Connect_OPC()
    {
        //connected = await uaClient.ConnectAsync();
        session = await Session.Create(config, new ConfiguredEndpoint(null, new EndpointDescription(ServerUrl)), true, "", 60000, null, null);

        if (session != null)//connected)
        {
            //// Run tests for available methods.
            ////uaClient.ReadNodes();
            ////uaClient.WriteNodes();
            //uaClient.Browse();
            //uaClient.CallMethod();

            //uaClient.SubscribeToDataChanges();
            //// Wait for some DataChange notifications from MonitoredItems
            ////await Task.Delay(20_000);

            ////uaClient.Disconnect();

            Debug.Log("Step 3 - Browse the server namespace.");
            ReferenceDescriptionCollection refs;
            byte[] cp;
            session.Browse(null, null, ObjectIds.ObjectsFolder, 0u, BrowseDirection.Forward, ReferenceTypeIds.HierarchicalReferences, true, (uint)NodeClass.Variable | (uint)NodeClass.Object | (uint)NodeClass.Method, out cp, out refs);
            Debug.Log("DisplayName: BrowseName, NodeClass");
            foreach (var rd in refs)
            {
                Debug.Log(rd.DisplayName + ": " + rd.BrowseName + ", " + rd.NodeClass);
                ReferenceDescriptionCollection nextRefs;
                byte[] nextCp;
                session.Browse(null, null, ExpandedNodeId.ToNodeId(rd.NodeId, session.NamespaceUris), 0u, BrowseDirection.Forward, ReferenceTypeIds.HierarchicalReferences, true, (uint)NodeClass.Variable | (uint)NodeClass.Object | (uint)NodeClass.Method, out nextCp, out nextRefs);
                foreach (var nextRd in nextRefs)
                {
                    Debug.Log("+ " + nextRd.DisplayName + ": " + nextRd.BrowseName + ", " + nextRd.NodeClass);
                }
            }

            Subscription subscription = new Subscription(session.DefaultSubscription);

            subscription.DisplayName = "Unity3D_opctest";
            subscription.PublishingEnabled = true;
            subscription.PublishingInterval = 1000;

            session.AddSubscription(subscription);

            // Create the subscription on Server side
            subscription.Create();
            Debug.Log("New Subscription created with SubscriptionId = " + subscription.Id);

            // Create MonitoredItems for data changes

            MonitoredItem intMonitoredItem = new MonitoredItem(subscription.DefaultItem);

            MonitoredItem stringMonitoredItemCreator = new MonitoredItem(subscription.DefaultItem);
            // String Node - Objects\CTT\NTNU_ICT_WindFarm_Demo\WindFarm_Creator
            stringMonitoredItemCreator.StartNodeId = new NodeId("ns=2;s=WindFarm_Creator");
            stringMonitoredItemCreator.AttributeId = Attributes.Value;
            stringMonitoredItemCreator.DisplayName = "Wind Farm Creator";
            stringMonitoredItemCreator.SamplingInterval = 1000;
            stringMonitoredItemCreator.Notification += OnMonitoredItemNotification;

            subscription.AddItem(stringMonitoredItemCreator);

            MonitoredItem doubleMonitoredItemWindDir = new MonitoredItem(subscription.DefaultItem);
            // Double Node - Objects\CTT\NTNU_ICT_WindFarm_Demo\WindFarm_Power
            doubleMonitoredItemWindDir.StartNodeId = new NodeId("ns=2;s=WindFarm_WindDir");
            doubleMonitoredItemWindDir.AttributeId = Attributes.Value;
            doubleMonitoredItemWindDir.DisplayName = "Wind Farm WindDir";
            doubleMonitoredItemWindDir.SamplingInterval = 1000;
            doubleMonitoredItemWindDir.Notification += OnMonitoredItemNotification;

            subscription.AddItem(doubleMonitoredItemWindDir);

            MonitoredItem doubleMonitoredItemWSpeed = new MonitoredItem(subscription.DefaultItem);
            // Double Node - Objects\CTT\NTNU_ICT_WindFarm_Demo\WindFarm_Windspeed
            doubleMonitoredItemWSpeed.StartNodeId = new NodeId("ns=2;s=WindFarm_Windspeed");
            doubleMonitoredItemWSpeed.AttributeId = Attributes.Value;
            doubleMonitoredItemWSpeed.DisplayName = "Wind Farm Windspeed";
            doubleMonitoredItemWSpeed.SamplingInterval = 1000;
            doubleMonitoredItemWSpeed.Notification += OnMonitoredItemNotification;

            subscription.AddItem(doubleMonitoredItemWSpeed);

            MonitoredItem doubleMonitoredItemBlade = new MonitoredItem(subscription.DefaultItem);
            // Double Node - Objects\CTT\NTNU_ICT_WindFarm_Demo\WindFarm_Windspeed
            doubleMonitoredItemBlade.StartNodeId = new NodeId("ns=2;s=WindFarm_Blade");
            doubleMonitoredItemBlade.AttributeId = Attributes.Value;
            doubleMonitoredItemBlade.DisplayName = "Wind Farm Blade";
            doubleMonitoredItemBlade.SamplingInterval = 1000;
            doubleMonitoredItemBlade.Notification += OnMonitoredItemNotification;

            subscription.AddItem(doubleMonitoredItemBlade);



            MonitoredItem doubleMonitoredItemFeedback = new MonitoredItem(subscription.DefaultItem);
            // Double Node - Objects\CTT\NTNU_ICT_WindFarm_Demo\WindFarm_Windspeed
            doubleMonitoredItemFeedback.StartNodeId = new NodeId("ns=2;s=WindFarm_Feedback");
            doubleMonitoredItemFeedback.AttributeId = Attributes.Value;
            doubleMonitoredItemFeedback.DisplayName = "Wind Farm Feedback";
            doubleMonitoredItemFeedback.SamplingInterval = 1000;
            doubleMonitoredItemFeedback.Notification += OnMonitoredItemNotification;

            subscription.AddItem(doubleMonitoredItemFeedback);


            MonitoredItem doubleMonitoredItemCommand = new MonitoredItem(subscription.DefaultItem);
            // Double Node - Objects\CTT\NTNU_ICT_WindFarm_Demo\WindFarm_Windspeed
            doubleMonitoredItemCommand.StartNodeId = new NodeId("ns=2;s=WindFarm_Command");
            doubleMonitoredItemCommand.AttributeId = Attributes.Value;
            doubleMonitoredItemCommand.DisplayName = "Wind Farm Command";
            doubleMonitoredItemCommand.SamplingInterval = 1000;
            doubleMonitoredItemCommand.Notification += OnMonitoredItemNotification;

            subscription.AddItem(doubleMonitoredItemCommand);

            // Create the monitored items on Server side
            subscription.ApplyChanges();
            flag = true;
            Debug.Log("Finished client initialization");
        }
        else
        {
            Debug.Log("Could not connect to server!");
        }
    }

    public void WriteNodes(string tagname, string value)
    {
        if (session == null || session.Connected == false)
        {
            Debug.Log("Session not connected!");
            return;
        }

        try
        {
            // Write the configured nodes
            WriteValueCollection nodesToWrite = new WriteValueCollection();

            double dDataValue = 0;
            // Double Node - Objects\CTT\NTNU_ICT_WindFarm_Demo\WindFarm_Power
            //WriteValue doubleWriteValPower = new WriteValue();
            //doubleWriteValPower.NodeId = new NodeId("ns=2;s=WindFarm_Power");
            //doubleWriteValPower.AttributeId = Attributes.Value;
            //doubleWriteValPower.Value = new DataValue();
            //double.TryParse(windpower_wr, out dDataValue);
            //doubleWriteValPower.Value.Value = dDataValue;
            //nodesToWrite.Add(doubleWriteValPower);

            //dDataValue = 0;
            //// Double Node - Objects\CTT\NTNU_ICT_WindFarm_Demo\WindFarm_Windspeed
            //WriteValue doubleWriteValWSpeed = new WriteValue();
            //doubleWriteValWSpeed.NodeId = new NodeId("ns=2;s=WindFarm_Windspeed");
            //doubleWriteValWSpeed.AttributeId = Attributes.Value;
            //doubleWriteValWSpeed.Value = new DataValue();
            //double.TryParse(windspeed_wr, out dDataValue);
            //doubleWriteValWSpeed.Value.Value = dDataValue;
            //nodesToWrite.Add(doubleWriteValWSpeed);

            dDataValue = 0;
            // Double Node - Objects\CTT\NTNU_ICT_WindFarm_Demo\WindFarm_Blade
            WriteValue doubleWriteValWBlade = new WriteValue();
            doubleWriteValWBlade.NodeId = new NodeId("ns=2;s=WindFarm_" + tagname);
            doubleWriteValWBlade.AttributeId = Attributes.Value;
            doubleWriteValWBlade.Value = new DataValue();
            double.TryParse(value, out dDataValue);
            doubleWriteValWBlade.Value.Value = dDataValue;
            nodesToWrite.Add(doubleWriteValWBlade);

            // Write the node attributes
            StatusCodeCollection results = null;
            DiagnosticInfoCollection diagnosticInfos;
            Debug.Log("Writing nodes...");

            // Call Write Service
            session.Write(null,
                            nodesToWrite,
                            out results,
                            out diagnosticInfos);

            // Validate the response
            //m_validateResponse(results, nodesToWrite);

            // Display the results.
            Debug.Log("Write Results :");

            foreach (StatusCode writeResult in results)
            {
                Debug.Log(writeResult);
            }
        }
        catch (Exception ex)
        {
            // Log Error
            Debug.Log($"Write Nodes Error : {ex.Message}.");
        }
    }

    /// <summary>
    /// Handle DataChange notifications from Server
    /// </summary>
    private void OnMonitoredItemNotification(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e)
    {
        try
        {
            // Log MonitoredItem Notification event
            MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;
            //Debug.Log("Notification Received for Variable \"{0}\" and Value = {1}." + monitoredItem.DisplayName + notification.Value);
            // update the form1 labels content
            if (monitoredItem.DisplayName == "Wind Farm Creator")
                creator_rd = notification.Value.ToString();
            //displayer.lblCreator.Text = notification.Value.ToString();
            else if (monitoredItem.DisplayName == "Wind Farm WindDir")
                WindDir_rd = notification.Value.ToString();
            //displayer.lblWindPower.Text = notification.Value.ToString();
            else if (monitoredItem.DisplayName == "Wind Farm Windspeed")
                windspeed_rd = notification.Value.ToString();

            else if (monitoredItem.DisplayName == "Wind Farm Blade")
                Blade_rd = notification.Value.ToString();
            //displayer.lblWindPower.Text = notification.Value.ToString();
            //
            else if (monitoredItem.DisplayName == "Wind Farm Feedback")
                Feedback = notification.Value.ToString();

            else if (monitoredItem.DisplayName == "Wind Farm Command")
                Command = notification.Value.ToString();
        }
        catch (Exception ex)
        {
            Debug.Log("OnMonitoredItemNotification error: {0}" + ex.Message);
        }
    }
}