using UnityEngine;
using strange.extensions.mediation.impl;
using UnityEngine.UI;
using Common;

public class GameStartUI_View : EventView
{
    private Transform Login;
    private Button Btn_Start;
    private Button Btn_Register;
    private Button Btn_Guest;
    private InputField Name_Input;
    private InputField Pwd_Input;

    private Button Btn_RegisterOK;
    private Button Btn_RegisterClose;
    private InputField Register_NameInput;
    private InputField Register_PwdInput;
    private InputField Register_RepeatPwdInput;   
    private Text Message_Txt;//错误消息

    private GameObject Register_Panel;//用户注册界面
    private GameObject Message_Panel;//弹出消息界面
    private Text Message_Str;//错误消息
    private Text UserName;//错误消息
    private Text UserPwd;//错误消息


    public void Init()
    {
        Debug.unityLogger.logEnabled = false;

        Login = transform.Find("Login");

        Btn_Start = Login.Find("Btn_Start").GetComponent<Button>();
        Btn_Start.onClick.AddListener(Click_BtnStart);
        Btn_Register = Login.Find("Btn_Register").GetComponent<Button>();
        Btn_Register.onClick.AddListener(Click_BtnRegister);
        Btn_Guest = Login.Find("Btn_Guest").GetComponent<Button>();
        Btn_Guest.onClick.AddListener(Click_BtnGuest);

        Register_Panel = Login.transform.Find("Register_Panel").gameObject;
        Btn_RegisterOK = Register_Panel.transform.Find("RegisterButton").GetComponent<Button>();
        Btn_RegisterOK.onClick.AddListener(Click_RegisterOK);
        Btn_RegisterClose = Register_Panel.transform.Find("CloseButton").GetComponent<Button>();
        Btn_RegisterClose.onClick.AddListener(Click_RegisterClose);
        Register_NameInput = Register_Panel.transform.Find("NameInput").GetComponent<InputField>();
        Register_NameInput.placeholder.GetComponent<Text>().text = StringFanConst.InputName;
        Register_PwdInput = Register_Panel.transform.Find("PwdInput").GetComponent<InputField>();
        Register_PwdInput.placeholder.GetComponent<Text>().text = StringFanConst.InputPwd;
        Register_RepeatPwdInput = Register_Panel.transform.Find("RepeatPwdInput").GetComponent<InputField>();
        Register_RepeatPwdInput.placeholder.GetComponent<Text>().text = StringFanConst.RepeatPwd;
 
        //消息窗口        
        Message_Panel = Login.transform.Find("Message_Panel").gameObject;
        Message_Str = Message_Panel.transform.Find("Message_Str").GetComponent<Text>();
        //按钮文字
        Text txt_Start = Btn_Start.transform.Find("Text").GetComponent<Text>();
        txt_Start.text = StringFanConst.Start ;
        Text txt_Register = Btn_Register.transform.Find("Text").GetComponent<Text>();
        txt_Register.text = StringFanConst.Register;
        Text txt_Guest = Btn_Guest.transform.Find("Text").GetComponent<Text>();
        txt_Guest.text = StringFanConst.Guest;
        Text txt_RegOK = Btn_RegisterOK.transform.Find("Text").GetComponent<Text>();
        txt_RegOK.text = StringFanConst.Register;
        //文本框文字
        UserName = Login.Find("UserName").GetComponent<Text>();
        UserPwd = Login.Find("UserPwd").GetComponent<Text>();
        UserName.text = StringFanConst.NameTitle;
        UserPwd.text = StringFanConst.PwdTitle;
        //註冊窗口文本框文字
        Message_Txt = Register_Panel.transform.Find("Message_Txt").GetComponent<Text>();

        Name_Input = Login.Find("Name_Input").GetComponent<InputField>();
        Name_Input.placeholder.GetComponent<Text>().text = StringFanConst.InputName;
        Name_Input.ActivateInputField();
        Pwd_Input = Login.Find("Pwd_Input").GetComponent<InputField>();
        Pwd_Input.placeholder.GetComponent<Text>().text = StringFanConst.InputPwd;
        //Pwd_Input.text = StringFanConst.InputPwd;
        HideRegisterPanel();
        HideMessagePanel();
    }
    private void Click_BtnStart()
    {
        if (!string.IsNullOrEmpty(StringFanConst.TestVersion))
        {
            ShowMessagePanel(StringFanConst.TestVersion);
            return;
        }

        if (string.IsNullOrEmpty(Name_Input.text))
        {
            ShowMessagePanel(StringFanConst.InputName);
            Name_Input.ActivateInputField();
            return;
        }

        if (string.IsNullOrEmpty(Pwd_Input.text))
        {
            ShowMessagePanel(StringFanConst.InputPwd);
            Pwd_Input.ActivateInputField();
            return;
        }
        if (!string.IsNullOrEmpty(Name_Input.text) && !string.IsNullOrEmpty(Pwd_Input.text) )
            dispatcher.Dispatch(ViewConst.Click_StartGame, Name_Input.text + "|" + Pwd_Input.text);
        //NetConn.Instance.Send(RequestCode.User, ActionCode.Login, Name_Input.text + ";" + Pwd_Input.text);
    }

    private void Click_BtnGuest()
    {
       
            dispatcher.Dispatch(ViewConst.Click_StartGame, StringFanConst.Guest);
       
    }

    protected override void Start()
    {
        base.Start();
        setDesignContentScale();
        print("resolution");
    }
    private int scaleWidth = 0;
    private int scaleHeight = 0;
    public void setDesignContentScale()
    {
        if (scaleWidth == 0 && scaleHeight == 0)
        {
            int width = Screen.currentResolution.width;
            int height = Screen.currentResolution.height;
            int designWidth = 1920;
            int designHeight = 1080;
            float s1 = (float)designWidth / (float)designHeight;
            float s2 = (float)width / (float)height;
            if (s1 < s2)
            {
                designWidth = (int)Mathf.FloorToInt(designHeight * s2);
            }
            else if (s1 > s2)
            {
                designHeight = (int)Mathf.FloorToInt(designWidth / s2);
            }
            float contentScale = (float)designWidth / (float)width;
            if (contentScale < 1.0f)
            {
                scaleWidth = designWidth;
                scaleHeight = designHeight;
            }
        }
        if (scaleWidth > 0 && scaleHeight > 0)
        {
            if (scaleWidth % 2 == 0)
            {
                scaleWidth += 1;
            }
            else
            {
                scaleWidth -= 1;
            }
            Screen.SetResolution(scaleWidth, scaleHeight, true);
        }
    }

    public void HideRegisterPanel()
    {    

        Register_Panel.SetActive(false);
    }
    public void ShowMessagePanel(string strMessage, float showMessageTime = 5)
    {
        Message_Str.text = strMessage;
        Message_Panel.SetActive(true);
        if (strMessage != "")
        {
            Invoke("HideMessagePanel", showMessageTime);
        }
    }

    public void HideMessagePanel()
    {
        Message_Panel.SetActive(false);
    }

    public void Click_BtnRegister()
    {
        if (!string.IsNullOrEmpty(StringFanConst.TestVersion))
        {
            ShowMessagePanel(StringFanConst.TestVersion);
            return;
        }
        Text NameTitle = Register_Panel.transform.Find("NameTitle").GetComponent<Text>();
        Text PwdTitle = Register_Panel.transform.Find("PwdTitle").GetComponent<Text>();
        Text RepeatPwdTitle = Register_Panel.transform.Find("RepeatPwdTitle").GetComponent<Text>();
        NameTitle.text = StringFanConst.NameTitle;
        PwdTitle.text = StringFanConst.PwdTitle;
        RepeatPwdTitle.text = StringFanConst.RepeatPwdTitle;
        //輸入框提示信息
        InputField NameInput = Register_Panel.transform.Find("NameInput").GetComponent<InputField>();
        NameInput.text = StringFanConst.InputName;
        InputField PwdInput = Register_Panel.transform.Find("PwdInput").GetComponent<InputField>();
        PwdInput.text = StringFanConst.InputPwd;
        InputField RepeatInput = Register_Panel.transform.Find("RepeatPwdInput").GetComponent<InputField>();
        RepeatInput.text = StringFanConst.InputPwd;

        UserName.text = "";
        UserPwd.text = "";

        Register_Panel.SetActive(true);
        Register_NameInput.text = "";
        Register_PwdInput.text = "";
        Register_RepeatPwdInput.text = "";
        Register_NameInput.ActivateInputField();

        //Game.Instance.ShowPanel("RegisterPanel");
        //Game.Instance.HidePanel("LoginPanel");
    }

    private void Click_RegisterOK()
    {
        if (Register_NameInput.text == "")
        {
            //Message_Txt.text = StringFanConst.RegUserNull;
            ShowMessagePanel(StringFanConst.InputName);
            Register_NameInput.ActivateInputField();
            return;
        }
        if (Register_PwdInput.text == "")
        {
            //Message_Txt.text = StringFanConst.RegPwdNull;
            ShowMessagePanel(StringFanConst.InputPwd);
            Register_PwdInput.ActivateInputField();
            return;
        }
        if (Register_PwdInput.text != Register_RepeatPwdInput.text)
        {
            //Message_Txt.text = StringFanConst.RegPwdError;
            ShowMessagePanel(StringFanConst.RegPwdError); 
            Register_RepeatPwdInput.ActivateInputField();
            return;
        }
        //NetConn.Instance.Send(RequestCode.User, ActionCode.Register, Register_NameInput.text + ";" + Register_PwdInput.text);
        dispatcher.Dispatch(ViewConst.Click_RegisterOK,Register_NameInput.text+"|"+Register_PwdInput.text);
    }

    private void Click_RegisterClose()
    {
       HideRegisterPanel();
        UserName.text = StringFanConst.NameTitle;
        UserPwd.text = StringFanConst.PwdTitle;
    }

    void OnApplicationPause(bool paused)
    {
        if (paused)
        {
        }
        else
        {
            setDesignContentScale();
        }
    }
}