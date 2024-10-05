﻿using UnityEngine;

using strange.extensions.dispatcher.eventdispatcher.api;

public interface IClientService 
{
    IEventDispatcher dispatcher { get; set; }
    void SendDataToServer(IBaseMsg data);
}
public class SendDataService : IClientService
{
    [Inject]
    public IEventDispatcher dispatcher { get; set; }
    [Inject] 
    public IMdData gameData { get; set; }
    private TestClient.Client _client;
    public void SendDataToServer(IBaseMsg data)
    {
        if (gameData.CurrentMode== MdMode.MutiPlayer)//多人模式
        {
            if (_client == null)
            {
                ClientService clientService = GameObject.Find("NetClient").GetComponent<ClientService>();
                _client = clientService._client;
            }
            //if (!_client.IsConnected)
            //{
            //    _client.InitClient(ServiceConst.IPAddress_Server, ServiceConst.Port_Server);
            //    _client.StartConnect();             
            //}

            Debug.Log(_client);
            _client.SendMsg(data.ToJson());
        }
        else//单机模式
        {
            Debug.Log(data.ToJson());
            RemoteCMD_Data recData = LitJson.JsonMapper.ToObject<RemoteCMD_Data>(data.ToJson());
            GameObject.Find("PVEController").GetComponent<LocalService_View>().
                SendDataToLocal(recData);//发送数据到本地
        }
    }
}