using Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Contracts;

public interface IFirebaseMessageSender
{
    Task<FirebaseSendMessageResultDTO> SendToTopic(string title, string body, string topicName,
        Dictionary<string, string> data = null, bool isTest = false);
    Task<FirebaseSendMessageResultDTO> SendToDevice(string title, string body, string deviceToken,
        Dictionary<string, string> data = null, bool isTest = false);
    Task<FirebaseMultiDevicesSendMessageResultDTO> SendToDevices(string title, string body, List<string> devicesTokens,
        Dictionary<string, string> data = null, bool isTest = false);
}