//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: proto/LSToGC.proto
namespace LSToGC
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"LoginResult")]
  public partial class LoginResult : global::ProtoBuf.IExtensible
  {
    public LoginResult() {}
    

    private LSToGC.MsgID _msgid = LSToGC.MsgID.eMsgToGCFromLS_NotifyLoginResult;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"msgid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(LSToGC.MsgID.eMsgToGCFromLS_NotifyLoginResult)]
    public LSToGC.MsgID msgid
    {
      get { return _msgid; }
      set { _msgid = value; }
    }

    private int _result = default(int);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int result
    {
      get { return _result; }
      set { _result = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ServerInfo")]
  public partial class ServerInfo : global::ProtoBuf.IExtensible
  {
    public ServerInfo() {}
    

    private string _ServerName = "";
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"ServerName", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string ServerName
    {
      get { return _ServerName; }
      set { _ServerName = value; }
    }

    private string _ServerAddr = "";
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"ServerAddr", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string ServerAddr
    {
      get { return _ServerAddr; }
      set { _ServerAddr = value; }
    }

    private int _ServerPort = default(int);
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"ServerPort", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int ServerPort
    {
      get { return _ServerPort; }
      set { _ServerPort = value; }
    }

    private int _ServerState = default(int);
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"ServerState", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int ServerState
    {
      get { return _ServerState; }
      set { _ServerState = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ServerBSAddr")]
  public partial class ServerBSAddr : global::ProtoBuf.IExtensible
  {
    public ServerBSAddr() {}
    

    private LSToGC.MsgID _msgid = LSToGC.MsgID.eMsgToGCFromLS_NotifyServerBSAddr;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"msgid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(LSToGC.MsgID.eMsgToGCFromLS_NotifyServerBSAddr)]
    public LSToGC.MsgID msgid
    {
      get { return _msgid; }
      set { _msgid = value; }
    }
    private readonly global::System.Collections.Generic.List<LSToGC.ServerInfo> _serverinfo = new global::System.Collections.Generic.List<LSToGC.ServerInfo>();
    [global::ProtoBuf.ProtoMember(2, Name=@"serverinfo", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<LSToGC.ServerInfo> serverinfo
    {
      get { return _serverinfo; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
    [global::ProtoBuf.ProtoContract(Name=@"MsgID")]
    public enum MsgID
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"eMsgToGCFromLS_NotifyLoginResult", Value=512)]
      eMsgToGCFromLS_NotifyLoginResult = 512,
            
      [global::ProtoBuf.ProtoEnum(Name=@"eMsgToGCFromLS_NotifyServerBSAddr", Value=513)]
      eMsgToGCFromLS_NotifyServerBSAddr = 513
    }
  
}